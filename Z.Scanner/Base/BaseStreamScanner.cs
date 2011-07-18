﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Z.Scanner
{
    public abstract class BaseStreamScanner<T> : BaseScanner<T>
    {
        public BaseStreamScanner(IEnumerable<IScanClue<T>> cluesToSearch) : base(cluesToSearch) { }

        public IEnumerable<KeyValuePair<IScanClue<T>, T>> Scan(Stream inputStream)
        {
            return base.Scan(LoadStream(inputStream));
        }
        protected abstract IEnumerable<T> LoadStream(Stream inputStream);
    }
}