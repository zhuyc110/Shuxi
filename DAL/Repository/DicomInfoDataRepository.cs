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

        public IQueryable<DicomInfoData> GetAll()
        {
            return _context.DicomInfoDatas;
        }

        public IEnumerable<DicomInfoData> Get(int pageIndex, int pageSize, params Condition[] conditions)
        {
            if (conditions == null || conditions.Length == 0)
            {
                return _context.DicomInfoDatas.Skip(pageIndex).Take(pageSize);
            }

            return _context.DicomInfoDatas.AsEnumerable().Where(x => conditions.Any(condition => condition.Predicate(x))).Skip(pageIndex).Take(pageSize);
        }

        public void Clear()
        {
            _context.DicomInfoDatas.RemoveRange(_context.DicomInfoDatas);

            _context.SaveChanges();
        }

        private readonly ShuxiContext _context;
    }
}
