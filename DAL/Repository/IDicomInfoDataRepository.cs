using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    public interface IDicomInfoDataRepository
    {
        event EventHandler DataChanged;
        int Count();

        void Clear();

        void Add(IEnumerable<DicomInfoData> dicomInfoDatas);

        IQueryable<DicomInfoData> GetAll();
    }
}
