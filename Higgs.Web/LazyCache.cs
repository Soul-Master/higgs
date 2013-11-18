using System;
using System.Web.Mvc;
using Higgs.Core;

namespace Higgs.Web
{
    public class LazyCache<TSource, TModel> : LazyCache<TModel>
    {
        private readonly Func<TSource, TModel> _valueFactory;

        protected LazyCache() {}

        public LazyCache(Func<TSource, TModel> valueFactory)
        {
            _valueFactory = valueFactory;

            Reset();
        }

        public new void Reset()
        {
            LazyObj = new Lazy<TModel>(MapingFunc());
        }

        protected virtual Func<TModel> MapingFunc()
        {
            return () =>
            {
                var context = DependencyResolver.Current.GetService<TSource>();

                return _valueFactory(context);
            };
        }
    }
}
