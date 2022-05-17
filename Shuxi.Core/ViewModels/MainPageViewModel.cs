using DAL.Model;
using DAL.Repository;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Shuxi.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Shuxi.Core.ViewModels
{
    public class MainPageViewModel : MvxViewModel
    {
        #region Commands

        public MvxCommand AddConditionCommand
        {
            get;
            private set;
        }
        public ICommand SearchCommand
        {
            get;
            private set;
        }
        public ICommand ResetCommand
        {
            get;
            private set;
        }
        public ICommand ClearConditionCommand
        {
            get;
            private set;
        }

        public ICommand OpenPathCommand { get; private set; }

        #endregion

        public string? ConditionValue
        {
            get => _conditionValue;
            set
            {
                SetProperty(ref _conditionValue, value);
                AddConditionCommand.RaiseCanExecuteChanged();
            }
        }

        public string? CurrentCondition
        {
            get => _currentCondition;
            set
            {
                SetProperty(ref _currentCondition, value);
                AddConditionCommand.RaiseCanExecuteChanged();
            }
        }

        public ICollection<string> ConditionSource
        {
            get
            {
                return new List<string> { 
                    nameof(DicomInfoData.PerformedProcedureStepID), 
                    nameof(DicomInfoData.PatientBirthDate), 
                    nameof(DicomInfoData.PerformedProcedureStepStartDate) 
                };
            }
        }

        public ICollectionView FilteredData
        {
            get => _dicomFilesViewSource.View;
        }

        public ObservableCollection<Condition> Conditions { get; private set; }
        public PagingController Pager { get; private set; }

        public MainPageViewModel(
            IDicomInfoDataRepository dicomInfoDataRepository,
            IMvxNavigationService navigationService)
        {
            _dicomInfoDataRepository = dicomInfoDataRepository;
            _navigationService = navigationService;
            AddConditionCommand = new MvxCommand(AddCondition, () => !string.IsNullOrWhiteSpace(ConditionValue) && !string.IsNullOrWhiteSpace(CurrentCondition));
            SearchCommand = new MvxCommand(ResetCondition);
            ResetCommand = new MvxAsyncCommand(GoToResetPage);
            ClearConditionCommand = new MvxCommand<Condition>(ClearCondition);
            OpenPathCommand = new MvxCommand<DicomInfoData>(OpenPath);

            Conditions = new ObservableCollection<Condition>();
            _dicomFilesViewSource.Source = _data;
            UpdatePager();
            UpdateDicomData();
        }

        public override async Task Initialize()
        {
            if (_data.Count == 0)
            {
                await GoToResetPage().ConfigureAwait(false);
                UpdatePager();
                UpdateDicomData();
            }
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            Pager.CurrentPageChanged -= OnPageChanged;
            base.ViewDestroy(viewFinishing);
        }

        private async Task GoToResetPage()
        {
            await _navigationService.Navigate<ReadFileViewModel>().ConfigureAwait(false);
        }

        private void OnPageChanged(object? sender, EventArgs e)
        {
            UpdateDicomData();
        }

        private void AddCondition()
        {
            var condition = new Condition(CurrentCondition, ConditionValue);
            var existingCondition = Conditions.FirstOrDefault(x => x.PropertyName == condition.PropertyName);
            if (existingCondition != null)
            {
                Conditions.Remove(existingCondition);
            }
            Conditions.Add(condition);
            UpdateDicomData();
        }

        private void ClearCondition(Condition condition)
        {
            Conditions.Remove(condition);
            UpdateDicomData();
        }

        private void ResetCondition()
        {
            Conditions.Clear();
            UpdateDicomData();
        }

        private void UpdatePager()
        {
            Pager = new PagingController(_dicomInfoDataRepository.Count());
            Pager.CurrentPageChanged += OnPageChanged;
        }

        private void UpdateDicomData()
        {
            _data.Clear();
            var data = _dicomInfoDataRepository.Get(Pager.CurrentPageStartIndex, Pager.PageSize, Conditions.ToArray());
            foreach (var item in data)
            {
                _data.Add(item);
            }
        }

        private void OpenPath(DicomInfoData data)
        {
            Process.Start("explorer.exe", $"/select, {data.FullName}");
        }

        private readonly IDicomInfoDataRepository _dicomInfoDataRepository;
        private readonly IMvxNavigationService _navigationService;
        private string? _conditionValue;
        private string? _currentCondition;
        private readonly ObservableCollection<DicomInfoData> _data = new ObservableCollection<DicomInfoData>();
        private readonly CollectionViewSource _dicomFilesViewSource = new CollectionViewSource();
    }
}
