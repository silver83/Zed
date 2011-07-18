using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z.Scanner
{
    public interface IScanClue<T>
    {
        bool Match(T record);
    }
}
