using System;

namespace Higgs.Core
{
    public class LazyCache<TModel> : IValue<TModel>
    {
        protected Lazy<TModel> LazyObj;
        private readonly Func<TModel> _valueFactory;

        // This constructor is used by inherited class only.
        protected LazyCache() {}

        public LazyCache(Func<TModel> valueFactory)
        {
            _valueFactory = valueFactory;

            Reset();
        }

        public void Reset()
        {
            LazyObj = new Lazy<TModel>(_valueFactory);
        }

        public TModel Value
        {
            get { return LazyObj.Value; }
        }
    }
}
