using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z.Projects.Scanner
{
    public interface IScanClue<T>
    {
        bool Match(T record);
    }
}
