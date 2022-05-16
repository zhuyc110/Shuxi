using DAL.Model;
using DAL.Repository;
using FellowOakDicom;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shuxi.Core.Services
{
    internal class DicomReader : IDicomReader
    {
        public DicomReader(IDicomInfoDataRepository dicomInfoDataRepository, ILogger<DicomReader> logger)
        {
            _dicomInfoDataRepository = dicomInfoDataRepository;
            _logger = logger;
        }

        public int PrepareProgress(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return 0;
            }
            var directoryInfo = new DirectoryInfo(directory);

            return directoryInfo.EnumerateFiles("*.dcm", SearchOption.AllDirectories).Count();
        }

        public void ReadFiles(string directory, IProgress<int> progress)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }

            var directoryInfo = new DirectoryInfo(directory);
            var files = directoryInfo.EnumerateFiles("*.dcm", SearchOption.AllDirectories);

            var dicoms = new List<DicomInfoData>();
            var failedDicomsCount = 0;
            foreach (var fileInfo in files)
            {
                try
                {
                    var dcm = DicomFile.Open(fileInfo.FullName);
                    dcm.Dataset.TryGetString(DicomTag.PerformedProcedureStepID, out var performedProcedureStepID);
                    EnsureValue(ref performedProcedureStepID);
                    dcm.Dataset.TryGetString(DicomTag.OperatorsName, out var operatorsName);
                    EnsureValue(ref operatorsName);
                    var patientBirthDate = dcm.Dataset.GetDateTime(DicomTag.PatientBirthDate, DicomTag.PatientBirthTime);
                    dcm.Dataset.TryGetString(DicomTag.PerformingPhysicianName, out var performingPhysicansName);
                    EnsureValue(ref performingPhysicansName);
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

                    if (dicoms.Count > 1000)
                    {
                        _logger.LogInformation("1000 DICOM files have been read.");
                        _dicomInfoDataRepository.Add(dicoms);
                        dicoms.Clear();
                    }
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, $"Failed to read file {fileInfo.Name}");
                    failedDicomsCount++;
                    continue;
                }
            }

            if (dicoms.Any())
            {
                _dicomInfoDataRepository.Add(dicoms);
            }

            if (failedDicomsCount > 0)
            {
                _logger.LogWarning($"Failed to read {failedDicomsCount} files");
            }

            _logger.LogInformation("Finished reading DICOM file infos.");
        }

        private void EnsureValue(ref string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }
        }

        private readonly IDicomInfoDataRepository _dicomInfoDataRepository;
        private readonly ILogger<DicomReader> _logger;
    }
}
