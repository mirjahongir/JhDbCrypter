using System.Linq;
using System.Linq.Expressions;

namespace JohaEfCrypter.Expressions
{
    public sealed class EncryptedQueryProvider : IQueryProvider
    {
        private readonly IQueryProvider _inner;

        public EncryptedQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            expression = new EncryptWhereVisitor().Visit(expression);
            return _inner.CreateQuery(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            expression = new EncryptWhereVisitor().Visit(expression);
            return _inner.CreateQuery<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            expression = new EncryptWhereVisitor().Visit(expression);
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            expression = new EncryptWhereVisitor().Visit(expression);
            return _inner.Execute<TResult>(expression);
        }
    }
}
