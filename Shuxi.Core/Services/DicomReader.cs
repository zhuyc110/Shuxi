using DAL.Model;
using DAL.Repository;
using EvilDICOM.Core;
using EvilDICOM.Core.Element;
using EvilDICOM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shuxi.Core.Services
{
    internal class DicomReader : IDicomReader
    {
        public DicomReader(IDicomInfoDataRepository dicomInfoDataRepository)
        {
            this._dicomInfoDataRepository = dicomInfoDataRepository;
        }

        public int ReadFiles(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return 0;
            }

            var directoryInfo = new DirectoryInfo(directory);

            var files = directoryInfo.GetFiles("*.dcm");

            var dicoms = new List<DicomInfoData>();
            foreach (var fileInfo in files)
            {
                var dcm = DICOMObject.Read(fileInfo.FullName);
                var patientName = dcm.FindFirst(TagHelper.PatientName) as AbstractElement<string>;
                var studyInstanceNo = dcm.FindFirst(TagHelper.StudyInstanceUID) as AbstractElement<string>;
                var patientSex = dcm.FindFirst(TagHelper.PatientSex) as AbstractElement<string>;
                var patientAge = dcm.FindFirst(TagHelper.PatientAge) as AbstractElement<string>;
                var studyDate = dcm.FindFirst(TagHelper.StudyDate) as AbstractElement<System.DateTime>;

                var one = new DicomInfoData
                {
                    PatientName = patientName?.Data,
                    StudyInstanceId = studyInstanceNo?.Data,
                    PatientSex = patientSex?.Data,
                    PatientAge = patientAge?.Data,
                    StatyDate = studyDate?.GetDataOrDefault(),
                    FileName = fileInfo.FullName
                };

                dicoms.Add(one);
            }

            _dicomInfoDataRepository.Add(dicoms);
            return dicoms.Count;
        }

        private readonly IDicomInfoDataRepository _dicomInfoDataRepository;
    }
}
