using JhCrypter;
using JhCrypter.Attributes;
using JohaEfCrypter.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace JohaEfCrypter.Expressions
{
    public sealed class EncryptWhereVisitor : ExpressionVisitor
    {
        Expression EquealAttribute(BinaryExpression node, PropertyInfo prop, EncryptedAttribute attr)
        {

            string plain = GetStringValue(node.Right);
            if (attr.IsEncrypt)
            {
                // 🔐 ENCRYPT QILINADI
                var encrypted = CryptoExtension.EncryptStr(plain);
                var encryptedConstant = Expression.Constant(encrypted, typeof(string));
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
        Expression NotEqual(BinaryExpression node, PropertyInfo prop, EncryptedAttribute attr)
        {
            string plain = GetStringValue(node.Right);
            if (attr.IsEncrypt)
            {
                // 🔐 ENCRYPT QILINADI
                var encrypted = CryptoExtension.EncryptStr(plain);
                var encryptedConstant = Expression.Constant(encrypted, typeof(string));
                return Expression.NotEqual(node.Left, encryptedConstant);
            }
            else if (attr.IsHash)
            {
                var hash = CryptoExtension.HashString(plain);
                var hashConstant = Expression.Constant(hash, typeof(string));
                return Expression.NotEqual(node.Left, hashConstant);
            }
            return this.VisitBinary(node);
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
                case ExpressionType.NotEqual:
                    if (prop.GetCustomAttribute<EncryptedAttribute>() is EncryptedAttribute attr1 && attr1 != null)
                        return NotEqual(node, prop, attr1);
                    break;
            }
            return base.VisitBinary(node);

        }
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var name = node.Method.Name;

            if (string.Equals(node.Method.Name, "contains", StringComparison.OrdinalIgnoreCase))
            {
                if (node.Arguments.Count != 1) return base.VisitMethodCall(node);

                if (node.Arguments[0] is MemberExpression memberExpr)
                {
                    var prop = memberExpr.Member as PropertyInfo;
                    var attr = prop?.GetCustomAttribute<EncryptedAttribute>();

                    if (attr == null) return base.VisitMethodCall(node);
                    var values = GetValues(node.Object);
                    List<string> data = [];
                    if (attr.IsEncrypt)
                    {
                        foreach (string str in values.Cast<string>())
                        {
                            data.Add(CryptoExtension.EncryptStr(str));
                        }
                    }
                    else if (attr.IsHash)
                    {
                        foreach (string str in values.Cast<string>())
                        {
                            data.Add(CryptoExtension.HashString(str));
                        }
                    }
                    else
                    {
                        data = values.Cast<string>().ToList();
                    }
                    //  Expression.Constant(data);
                    var newCollection = Expression.Constant(data);

                    // YANGI Contains expression
                    var newContains = Expression.Call(
                        typeof(Enumerable),
                        "Contains",
                        [memberExpr.Type],
                        newCollection,
                        memberExpr
                    );
                    return newContains;
                }
            }
            return base.VisitMethodCall(node);
        }
        static string GetStringValue(Expression expression)
        {
            string plain = "";
            if (expression is ConstantExpression constant)
            {
                plain = constant.Value as string;
            }
            else if (expression is MemberExpression members)
            {
                // member expression qiymatini olish
                var lambda = Expression.Lambda(members);
                var compiled = lambda.Compile();
                plain = compiled.DynamicInvoke() as string;
            }
            return plain;
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
