using DAL.Model;
using DAL.Repository;
using EvilDICOM.Core;
using EvilDICOM.Core.Element;
using EvilDICOM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shuxi.Core.Services
{
    internal class DicomReader : IDicomReader
    {
        public DicomReader(IDicomInfoDataRepository dicomInfoDataRepository)
        {
            _dicomInfoDataRepository = dicomInfoDataRepository;
        }

        public int PrepareProgress(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return 0;
            }
            var directoryInfo = new DirectoryInfo(directory);

            return directoryInfo.GetFiles("*.dcm").Count();
        }

        public int ReadFiles(string directory, IProgress<int> progress)
        {
            if (!Directory.Exists(directory))
            {
                return 0;
            }

            var directoryInfo = new DirectoryInfo(directory);

            var files = directoryInfo.GetFiles("*.dcm").ToList();

            var dicoms = new List<DicomInfoData>();
            for (var index = 0; index < files.Count; index++)
            {
                var fileInfo = files[index];
                var dcm = DICOMObject.Read(fileInfo.FullName);
                var patientName = dcm.FindFirst(TagHelper.PatientName) as AbstractElement<string>;
                var studyInstanceNo = dcm.FindFirst(TagHelper.StudyInstanceUID) as AbstractElement<string>;
                var patientSex = dcm.FindFirst(TagHelper.PatientSex) as AbstractElement<string>;
                var patientAge = dcm.FindFirst(TagHelper.PatientAge) as AbstractElement<string>;
                var studyDate = dcm.FindFirst(TagHelper.StudyDate) as AbstractElement<System.DateTime>;

                var one = new DicomInfoData
                {
#if DEBUG
                    PatientName = "ZHANG " + index,
                    StudyInstanceId = studyInstanceNo?.Data + index,
#else
                    PatientName = patientName?.Data,
                    StudyInstanceId = studyInstanceNo?.Data,
#endif
                    PatientSex = patientSex?.Data ?? "男",
                    PatientAge = patientAge?.Data,
                    StatyDate = studyDate?.GetDataOrDefault(),
                    FileName = fileInfo.FullName
                };

                dicoms.Add(one);
                progress.Report(dicoms.Count);
            }

            _dicomInfoDataRepository.Add(dicoms);
            return dicoms.Count;
        }

        private readonly IDicomInfoDataRepository _dicomInfoDataRepository;
    }
}
