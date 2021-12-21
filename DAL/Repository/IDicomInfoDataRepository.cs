using DAL.Model;
using System.Collections.Generic;

namespace DAL.Repository
{
    public interface IDicomInfoDataRepository
    {
        int Count();

        void Add(IEnumerable<DicomInfoData> dicomInfoDatas);

        IReadOnlyCollection<DicomInfoData> GetAll();
    }
}
