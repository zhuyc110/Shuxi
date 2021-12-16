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

        private readonly ShuxiContext _context;
    }
}
