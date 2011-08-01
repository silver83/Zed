using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.Scanner;

namespace Z.Projects.Components
{
    public interface IProjectComponent
    {
        ResourceId ResourceId { get; }
    }
}
