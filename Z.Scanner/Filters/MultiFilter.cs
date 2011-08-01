using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z.Scanner.Filters
{
    public class MultiFilter<T> : IFilter<T>
    {
        private IEnumerable<IFilter<T>> _filters;
        public MultiFilter(IEnumerable<IFilter<T>> filters)
        {
            _filters = filters;
        }

        public IEnumerable<T> Filter(IEnumerable<T> elems)
        {
            var matched = elems;
            foreach (var filter in _filters)
                matched = filter.Filter(matched);

            return matched;
        }
    }
}
