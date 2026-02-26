using System.Linq.Expressions;
using System.Reflection;

using JhCrypter.Attributes;

using JohaEfCrypter.Extensions;

namespace JohaEfCrypter.Expressions
{
    public sealed class EncryptWhereVisitor : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression node)
        {
            // faqat == ni ushlaymiz
            if (node.NodeType != ExpressionType.Equal)
                return base.VisitBinary(node);

            // Chap tomoni property bo‘lishi shart
            if (node.Left is not MemberExpression member)
                return base.VisitBinary(node);

            if (member.Member is not PropertyInfo prop)
                return base.VisitBinary(node);

            // [Encrypted] bormi?
            if (prop.GetCustomAttribute<EncryptedAttribute>() is EncryptedAttribute attr && attr != null)
            {
                // O‘ng tomoni constant bo‘lishi kerak
                if (node.Right is not ConstantExpression constant)
                    return base.VisitBinary(node);
                if (constant.Value is not string plain)
                    return base.VisitBinary(node);

                if (attr.IsEncrypt)
                {
                    // 🔐 ENCRYPT QILINADI
                    var encrypted = CryptoExtension.EncryptStr(plain);
                    var encryptedConstant =Expression.Constant(encrypted, typeof(string));
                    //node.Left.Na
                    return Expression.Equal(node.Left, encryptedConstant);

                }
                else if (attr.IsHash)
                {
                    var hash = CryptoExtension.HashString(plain);
                    var hashConstant = Expression.Constant(hash, typeof(string));
                    return Expression.Equal(node.Left, hashConstant);
                }
            }
            return base.VisitBinary(node);

        }
    }
}
