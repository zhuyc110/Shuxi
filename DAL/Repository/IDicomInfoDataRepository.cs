using DAL.Model;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    public interface IDicomInfoDataRepository
    {
        int Count();
        void UpdateCountCache(int count);

        void Clear();

        void Add(IList<DicomInfoData> dicomInfoDatas);
        IEnumerable<DicomInfoData> Get(int pageIndex, int pageSize, params Condition[] conditions);

        IQueryable<DicomInfoData> GetAll();
    }
}
