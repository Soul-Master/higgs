using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Higgs.Web
{
    public class LazyList<TSource> : LazyCache<TSource, SelectList>
    {
        private readonly Func<TSource, IEnumerable> _valueFactory;
        private readonly string _dataValueField;
        private readonly string _dataTextField;

        public LazyList(Func<TSource, IEnumerable> valueFactory, string dataValueField = "Value", string dataTextField = "Text")
        {
            _valueFactory = valueFactory;
            _dataValueField = dataValueField;
            _dataTextField = dataTextField;

            Reset();
        }

        protected override Func<SelectList> MapingFunc()
        {
            return () =>
            {
                var context = DependencyResolver.Current.GetService<TSource>();

                //  Force to retrieve data from current IEnumerable to prevent lazy loading data that 
                //  cause system always connect to database every time they generate data from selectlist.
                var loop = _valueFactory(context).GetEnumerator();
                var tempList = new List<object>();

                while (loop.MoveNext())
                {
                    tempList.Add(loop.Current);
                }

                return new SelectList(tempList, _dataValueField, _dataTextField);
            };
        }
    }
}
