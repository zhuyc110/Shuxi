using System;
using System.Threading.Tasks;

namespace Shuxi.Core.Services
{
    public interface IDicomReader
    {
        int PrepareProgress(string directory);
        Task ReadFiles(string directory, IProgress<int> progress);
    }
}
