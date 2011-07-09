using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Z.Common.Logging;

namespace Z.Projects.Scanner
{    
    public abstract class BaseScanner<T> : IScanner<T>
    {
        private readonly IEnumerable<IScanClue<T>> _cluesToSearch;
        public IEnumerable<IScanClue<T>> CluesToSearch
        {
            get { return _cluesToSearch; }
        }

        public BaseScanner(IEnumerable<IScanClue<T>> cluesToSearch)
        {
            _cluesToSearch = cluesToSearch;
        }
        public IEnumerable<IScanClue<T>> Scan(IEnumerable<T> input)
        {
           // Log.Application.InfoFormat("{0} Started scanning", this.GetType().Name);
            foreach (var elem in input)
                foreach (var clue in _cluesToSearch)
                {
                    if (clue.Match(elem))
                        yield return clue;
                }
        }
    }
}
