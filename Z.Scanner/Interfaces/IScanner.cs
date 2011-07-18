using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z.Scanner
{
    public interface IScanner<T>
    {
        IEnumerable<IScanClue<T>> CluesToSearch { get; }

        //Scan the stream to find matching clues
        IEnumerable<KeyValuePair<IScanClue<T>, T>> Scan(IEnumerable<T> input);
    }
}
