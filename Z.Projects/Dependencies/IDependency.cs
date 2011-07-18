using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z.Projects.Dependencies
{
    /// <summary>
    /// base interface for dependencies. 
    /// </summary>
    public interface IDependency
    {
        bool Verify(out string failureReason);
    }  
}
