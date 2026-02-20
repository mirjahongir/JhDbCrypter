using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace JohaEfCrypter.Expressions
{
    public sealed class EncryptedQueryable<T> : IQueryable<T>
    {

        private readonly IQueryable<T> _source;
        public EncryptedQueryable(IQueryable<T> source)
        {
            _source = source;
            Provider = new EncryptedQueryProvider(source.Provider);
        }
                
        public Type ElementType => typeof(T);
        public Expression Expression => _source.Expression;
        public IQueryProvider Provider { get; }
        public IEnumerator<T> GetEnumerator()
        {
            return _source.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

/*
 //public IEnumerator<T> GetEnumerator() => _source.GetEnumerator();
        //IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //public Type ElementType => typeof(T);
        //public Expression Expression => _source.Expression;
        //public IQueryProvider Provider { get; }
 
 */