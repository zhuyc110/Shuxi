using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    internal class DicomInfoDataRepository : IDicomInfoDataRepository
    {
        public event EventHandler DataChanged;

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

            DataChanged?.Invoke(this, null);
        }

        public IQueryable<DicomInfoData> GetAll()
        {
            return _context.DicomInfoDatas;
        }

        public void Clear()
        {
            _context.DicomInfoDatas.RemoveRange(_context.DicomInfoDatas);

            _context.SaveChanges();

            DataChanged?.Invoke(this, null);
        }

        private readonly ShuxiContext _context;
    }
}
