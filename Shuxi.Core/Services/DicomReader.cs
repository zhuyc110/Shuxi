using DAL.Model;
using DAL.Repository;
using FellowOakDicom;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task ReadFiles(string directory, IProgress<int> progress)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }

            var directoryInfo = new DirectoryInfo(directory);
            var files = directoryInfo.EnumerateFiles("*.dcm", SearchOption.AllDirectories);

            var dicoms = new List<DicomInfoData>();
            var succeededDicomsCount = 0;
            var failedDicomsCount = 0;

            _logger.LogInformation($"Starts reading DICOM files from {directoryInfo.Name}.");
            foreach (var fileInfo in files)
            {
                try
                {
                    var dcm = await DicomFile.OpenAsync(fileInfo.FullName, FileReadOption.SkipLargeTags).ConfigureAwait(false);
                    dcm.Dataset.TryGetString(DicomTag.PerformedProcedureStepID, out var performedProcedureStepID);
                    EnsureValue(ref performedProcedureStepID);
                    var patientBirthDate = dcm.Dataset.GetDateTime(DicomTag.PatientBirthDate, DicomTag.PatientBirthTime);
                    var procedureDate = dcm.Dataset.GetDateTime(DicomTag.PerformedProcedureStepStartDate, DicomTag.PerformedProcedureStepStartDateTime);

                    var one = new DicomInfoData
                    {
                        PerformedProcedureStepID = performedProcedureStepID,
                        PatientBirthDate = patientBirthDate,
                        PerformedProcedureStepStartDate = procedureDate,
                        FileName = fileInfo.Name,
                        FullName = fileInfo.FullName
                    };

                    dicoms.Add(one);
                    progress.Report(++succeededDicomsCount);

                    if (dicoms.Count >= 1000)
                    {
                        Persist(dicoms);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, $"Failed to read file {fileInfo.Name}");
                    failedDicomsCount++;
                    continue;
                }
            }

            Persist(dicoms);

            if (failedDicomsCount > 0)
            {
                _logger.LogWarning($"Failed to read {failedDicomsCount} files");
            }

            _logger.LogInformation($"Finished reading {succeededDicomsCount} DICOM file infos.");
        }

        private void EnsureValue(ref string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }
        }

        private void Persist(IList<DicomInfoData> succeded)
        {
            if (succeded.Any())
            {
                _logger.LogInformation($"{succeded.Count} DICOM files have been read.");
                _dicomInfoDataRepository.Add(succeded);
                succeded.Clear();
            }
        }

        private readonly IDicomInfoDataRepository _dicomInfoDataRepository;
        private readonly ILogger<DicomReader> _logger;
    }
}
