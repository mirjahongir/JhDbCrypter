using JhCrypter.Attributes;
using JohaEfCrypter.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace JohaEfCrypter.Expressions
{
    public sealed class EncryptWhereVisitor : ExpressionVisitor
    {
        Expression EquealAttribute(BinaryExpression node, PropertyInfo prop, EncryptedAttribute attr)
        {
            string plain = "";
            if (node.Right is ConstantExpression constant)
            {
                plain = constant.Value as string;
            }
            else if (node.Right is MemberExpression members)
            {
                // member expression qiymatini olish
                var lambda = Expression.Lambda(members);
                var compiled = lambda.Compile();
                plain = compiled.DynamicInvoke() as string;
            }

            if (attr.IsEncrypt)
            {
                // 🔐 ENCRYPT QILINADI
                var encrypted = CryptoExtension.EncryptStr(plain);
                var encryptedConstant = Expression.Constant(encrypted, typeof(string));
                //node.Left.Na
                return Expression.Equal(node.Left, encryptedConstant);

            }
            else if (attr.IsHash)
            {
                var hash = CryptoExtension.HashString(plain);
                var hashConstant = Expression.Constant(hash, typeof(string));
                return Expression.Equal(node.Left, hashConstant);
            }
            return this.VisitBinary(node);
        }
        void InSectorAttribyte(BinaryExpression node, PropertyInfo prop)
        {

        }
        protected override Expression VisitBinary(BinaryExpression node)
        {
            Console.WriteLine($"Visiting Binary Expression: {node.NodeType}");

            // Chap tomoni property bo‘lishi shart
            if (node.Left is not MemberExpression member)
                return base.VisitBinary(node);

            if (member.Member is not PropertyInfo prop)
                return base.VisitBinary(node);
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    if (prop.GetCustomAttribute<EncryptedAttribute>() is EncryptedAttribute attr && attr != null)
                        return EquealAttribute(node, prop, attr);
                    break;

            }
            // InSectorAttribyte(node, prop);

            return base.VisitBinary(node);

        }
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Arguments.Count > 0)
                if (node.Arguments[0] is MemberExpression memberExpr)
                {
                    var prop = memberExpr.Member as PropertyInfo;
                    var attr = prop.GetCustomAttribute<EncryptedAttribute>();
                    if (attr.IsHash)
                    {
                        Console.WriteLine(prop.Name);
                    }

                }
            if (string.Equals(node.Method.Name, "contains", StringComparison.OrdinalIgnoreCase))
            {
                // 1️⃣ Column (x.Id)
                var member = node.Arguments[1] as MemberExpression;
                var columnName = member?.Member.Name;

                // 2️⃣ Values (ids)
                var values = GetValues(node.Arguments[0]);



                // 3️⃣ Shu joyda SQL IN yasaysan
                // WHERE columnName IN (values...)

                // Masalan:
                // sqlBuilder.Append($"{columnName} IN ({string.Join(",", values)})");

                return node;
            }
            return base.VisitMethodCall(node);
        }
        static IEnumerable<object> GetValues(Expression expression)
        {
            if (expression is ConstantExpression constant)
            {
                // new[] {1,2,3}
                if (constant.Value is System.Collections.IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                        yield return item;
                }
            }
            else
            {
                // closure (var ids = ...)
                var lambda = Expression.Lambda(expression);
                var value = lambda.Compile().DynamicInvoke();

                if (value is System.Collections.IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                        yield return item;
                }
            }
        }

    }
}
