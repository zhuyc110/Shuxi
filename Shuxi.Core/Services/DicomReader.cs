using DAL.Model;
using DAL.Repository;
using FellowOakDicom;
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

            return directoryInfo.GetFiles("*.dcm", SearchOption.AllDirectories).Count();
        }

        public int ReadFiles(string directory, IProgress<int> progress)
        {
            if (!Directory.Exists(directory))
            {
                return 0;
            }

            var directoryInfo = new DirectoryInfo(directory);

            var files = directoryInfo.GetFiles("*.dcm", SearchOption.AllDirectories).ToList();

            var dicoms = new List<DicomInfoData>();
            for (var index = 0; index < files.Count; index++)
            {
                var fileInfo = files[index];
                var dcm = DicomFile.Open(fileInfo.FullName);
                var performedProcedureStepID = dcm.Dataset.GetString(DicomTag.PerformedProcedureStepID);
                var operatorsName = dcm.Dataset.GetString(DicomTag.OperatorsName);
                var patientBirthDate = dcm.Dataset.GetDateTime(DicomTag.PatientBirthDate, DicomTag.PatientBirthTime);
                var performingPhysicansName = dcm.Dataset.GetString(DicomTag.PerformingPhysicianName);
                var procedureDate = dcm.Dataset.GetDateTime(DicomTag.PerformedProcedureStepStartDate, DicomTag.PerformedProcedureStepStartDateTime);

                var one = new DicomInfoData
                {
                    PerformedProcedureStepID = performedProcedureStepID,
                    OperatorsName = operatorsName,
                    PatientBirthDate = patientBirthDate,
                    PerformingPhysicansName = performingPhysicansName,
                    PerformedProcedureStepStartDate = procedureDate,
                    FileName = fileInfo.Name
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
