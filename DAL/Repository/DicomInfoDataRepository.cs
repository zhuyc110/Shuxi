using DAL.Model;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    internal class DicomInfoDataRepository : IDicomInfoDataRepository
    {
        public DicomInfoDataRepository(ShuxiContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.DicomInfoDatas.Count();
        }

        public void Add(IEnumerable<DicomInfoData> dicomInfoDatas)
        {
            _context.DicomInfoDatas.AddRange(dicomInfoDatas);

            _context.SaveChanges();
        }

        public IReadOnlyCollection<DicomInfoData> GetAll()
        {
            return _context.DicomInfoDatas.ToList();
        }

        private readonly ShuxiContext _context;
    }
}
