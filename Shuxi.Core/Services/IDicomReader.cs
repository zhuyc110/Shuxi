using System;
using System.Collections.Generic;
using System.Text;

namespace Shuxi.Core.Services
{
    public interface IDicomReader
    {
        int ReadFiles(string directory);
    }
}
