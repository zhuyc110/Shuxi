using System;

namespace Shuxi.Core.Services
{
    public interface IDicomReader
    {
        int PrepareProgress(string directory);
        void ReadFiles(string directory, IProgress<int> progress);
    }
}
