﻿using DAL.Repository;
using Microsoft.WindowsAPICodePack.Dialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Shuxi.Core.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Shuxi.Core.ViewModels
{
    public class ReadFileViewModel : MvxViewModel
    {
        public string Path
        {
            get => _path; set => SetProperty(ref _path, value);
        }

        public int TotalProgress
        {
            get => _totalProgress; set => SetProperty(ref _totalProgress, value);
        }

        public int Progress
        {
            get => _progress; set => SetProperty(ref _progress, value);
        }

        public ICommand OpenFileCommand
        {
            get; private set;
        }

        public MvxAsyncCommand ReadCommand
        {
            get; private set;
        }

        public ICommand DeleteCommand
        {
            get; private set;
        }

        public ReadFileViewModel(IDicomReader dicomReader, IMvxNavigationService mvxNavigationService, IDicomInfoDataRepository dicomInfoDataRepository)
        {
            _dicomReader = dicomReader;
            _mvxNavigationService = mvxNavigationService;
            _dicomInfoDataRepository = dicomInfoDataRepository;
            OpenFileCommand = new MvxCommand(SelectFolder);
            ReadCommand = new MvxAsyncCommand(Read, () => !string.IsNullOrWhiteSpace(Path));
            DeleteCommand = new MvxCommand(DeleteAll);
        }

        private void SelectFolder()
        {
            _dicomInfoDataRepository.Clear();
            var openFileDialog = new CommonOpenFileDialog()
            {
                Title = "Select a folder",
                IsFolderPicker = true
            };

            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Path = openFileDialog.FileName;
                TotalProgress = _dicomReader.PrepareProgress(Path);
                ReadCommand.RaiseCanExecuteChanged();
            }
        }

        private async Task Read()
        {
            await Task.Run(() =>
            {
                var progress = new Progress<int>(x => Progress = x);
                _dicomReader.ReadFiles(Path, progress);
                _mvxNavigationService.Close(this);

            }).ConfigureAwait(false);
        }

        private void DeleteAll()
        {
            _dicomInfoDataRepository.Clear();
        }

        private readonly IDicomReader _dicomReader;
        private readonly IMvxNavigationService _mvxNavigationService;
        private readonly IDicomInfoDataRepository _dicomInfoDataRepository;
        private string _path = string.Empty;
        private int _progress = 0;
        private int _totalProgress = 100;
    }
}
