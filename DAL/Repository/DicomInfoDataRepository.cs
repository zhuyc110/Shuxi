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

        public void UpdateCountCache(int count)
        {
            _cachedCount = count;
        }

        public int Count()
        {
            if (_cachedCount == 0)
            {
                _cachedCount = _context.DicomInfoDatas.Count();
            }
            return _cachedCount;
        }

        public void Add(IList<DicomInfoData> dicomInfoDatas)
        {
             _context.DicomInfoDatas.AddRange(dicomInfoDatas);

            _context.SaveChanges();

            _cachedCount += dicomInfoDatas.Count;
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

            IEnumerable<DicomInfoData> queryable = _context.DicomInfoDatas;

            foreach (var condition in conditions)
            {
                switch (condition.PropertyName)
                {
                    case nameof(DicomInfoData.PerformedProcedureStepID):
                        queryable = queryable.Where(x => x.PerformedProcedureStepID.Contains(condition.Value, System.StringComparison.OrdinalIgnoreCase));
                        break;
                    case nameof(DicomInfoData.PatientBirthDate):
                        queryable = queryable.Where(x => x.PatientBirthDate.ToShortDateString().Contains(condition.Value, System.StringComparison.OrdinalIgnoreCase));
                        break;
                    case nameof(DicomInfoData.PerformedProcedureStepStartDate):
                        queryable = queryable.Where(x => x.PerformedProcedureStepStartDate.ToShortDateString().Contains(condition.Value, System.StringComparison.OrdinalIgnoreCase));
                        break;

                    default:
                        break;
                }
            }

            return queryable.Skip(pageIndex).Take(pageSize);
        }

        public void Clear()
        {
            _context.DicomInfoDatas.RemoveRange(_context.DicomInfoDatas);

            _context.SaveChanges();

            _cachedCount = 0;
        }

        private int _cachedCount = 0;
        private readonly ShuxiContext _context;
    }
}
