using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z.Scanner.Filters
{
    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> elems);
    }
}
