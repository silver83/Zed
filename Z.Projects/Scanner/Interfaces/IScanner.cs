using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z.Projects.Scanner
{
    public interface IScanner<T>
    {
        IEnumerable<IScanClue<T>> CluesToSearch { get; }

        //Scan the stream to find matching clues
        IEnumerable<IScanClue<T>> Scan(IEnumerable<T> input);
    }
}
