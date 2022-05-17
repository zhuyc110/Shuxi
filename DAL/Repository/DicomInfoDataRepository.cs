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
            var totalRecord = TryGetDicomGeneralInfos(DicomGeneralInfo.TotalCount, "0");

            return int.Parse(totalRecord.Value);
        }

        public void Add(IList<DicomInfoData> dicomInfoDatas)
        {
             _context.DicomInfoDatas.AddRange(dicomInfoDatas);

            var totalRecord = TryGetDicomGeneralInfos(DicomGeneralInfo.TotalCount, "0", false);
            totalRecord.Value = (int.Parse(totalRecord.Value) + dicomInfoDatas.Count).ToString();
            _context.DicomGeneralInfos.Update(totalRecord);

            _context.SaveChanges();
        }

        public IQueryable<DicomInfoData> GetAll()
        {
            return _context.DicomInfoDatas;
        }

        public IEnumerable<DicomInfoData> Get(int pageIndex, int pageSize, params Condition[] conditions)
        {
            IEnumerable<DicomInfoData> queryable = _context.DicomInfoDatas;

            if (conditions != null && conditions.Any())
            {
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
            }

            return queryable.Skip(pageIndex).Take(pageSize);
        }

        public void Clear()
        {
            _context.DicomInfoDatas.RemoveRange(_context.DicomInfoDatas);

            var totalRecord = TryGetDicomGeneralInfos(DicomGeneralInfo.TotalCount, "0", false);
            totalRecord.Value = "0";
            _context.DicomGeneralInfos.Update(totalRecord);

            _context.SaveChanges();
        }

        private DicomGeneralInfo TryGetDicomGeneralInfos(string key, string defaultValue, bool saveChanges = true)
        {
            var totalRecord = _context.DicomGeneralInfos.FirstOrDefault(x => x.Key == key);

            if (totalRecord == null)
            {
                totalRecord = _context.DicomGeneralInfos.Add(new DicomGeneralInfo { Key = key, Value = defaultValue }).Entity;

                if (saveChanges)
                {
                    _context.SaveChanges();
                }
            }

            return totalRecord;
        }

        private readonly ShuxiContext _context;
    }
}
