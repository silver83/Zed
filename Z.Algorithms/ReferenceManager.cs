using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Z.Common.Logging;

namespace Z.Algorithms
{
    public class ReferenceManager
    {
        private static void VerifyReferences(Assembly asm, Dictionary<string, bool> history)
        {
            if (history.ContainsKey(asm.FullName))
                return;

            Log.Application.InfoFormat("verifying assembly references for {0}", asm.FullName);
            history.Add(asm.FullName, true);

            foreach (var refAsmName in asm.GetReferencedAssemblies())
            {
                Assembly refAsm = Assembly.Load(refAsmName);
                VerifyReferences(refAsm, history);
            }
        }

        public static void VerifyReferences(Assembly asm)
        {
            VerifyReferences(asm, new Dictionary<string,bool>());
        }
    }
}
