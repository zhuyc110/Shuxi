using System;

namespace Shuxi.Core.Services
{
    public interface IDicomReader
    {
        int PrepareProgress(string directory);
        int ReadFiles(string directory, IProgress<int> progress);
    }
}
