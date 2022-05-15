using DAL.Model;
using DAL.Repository;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using System.Linq;

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

        public ObservableCollection<DicomInfoData> Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        public ICollection<string> ConditionSource 
        { 
            get 
            {
                return _conditionSource.Keys; 
            } 
        }

        public ICollectionView FilteredData
        {
            get; private set;
        }

        public ObservableCollection<Condition> Conditions { get; set; }

        public MainPageViewModel(
            IDicomInfoDataRepository dicomInfoDataRepository,
            IMvxNavigationService navigationService)
        {
            _conditionSource.Add(nameof(DicomInfoData.PerformedProcedureStepID), PerformedProcedureStepIDFilter);
            _conditionSource.Add(nameof(DicomInfoData.OperatorsName), OperatorsNameFilter);
            _conditionSource.Add(nameof(DicomInfoData.PatientBirthDate), PatientBirthDateFilter);
            _conditionSource.Add(nameof(DicomInfoData.PerformingPhysicansName), PerformingPhysicansNameFilter);
            _conditionSource.Add(nameof(DicomInfoData.PerformedProcedureStepStartDate), PerformedProcedureStepStartDateFilter);

            _dicomInfoDataRepository = dicomInfoDataRepository;
            _navigationService = navigationService;
            AddConditionCommand = new MvxCommand(AddCondition, () => !string.IsNullOrWhiteSpace(ConditionValue) && !string.IsNullOrWhiteSpace(CurrentCondition));
            SearchCommand = new MvxCommand(ResetCondition);
            ResetCommand = new MvxAsyncCommand(GoToResetPage);
            ClearConditionCommand = new MvxCommand<Condition>(ClearCondition);
            Data = new MvxObservableCollection<DicomInfoData>(_dicomInfoDataRepository.GetAll());
            FilteredData = CollectionViewSource.GetDefaultView(Data);
            Conditions = new ObservableCollection<Condition>();
        }

        public override async Task Initialize()
        {
            if (Data.Count == 0)
            {
                await GoToResetPage().ConfigureAwait(false);
                Data = new MvxObservableCollection<DicomInfoData>(_dicomInfoDataRepository.GetAll());
                FilteredData = CollectionViewSource.GetDefaultView(Data);
            }
        }

        private void AddCondition()
        {
            var condition = new Condition(CurrentCondition, ConditionValue, _conditionSource[CurrentCondition]);
            var existingCondition = Conditions.FirstOrDefault(x => x.PropertyName == condition.PropertyName);
            Conditions.Remove(existingCondition);
            Conditions.Add(condition);
            FilteredData.Filter = BuildFilter();
        }

        private void ResetCondition()
        {
            Conditions.Clear();
            FilteredData.Filter = null;
        }

        private async Task GoToResetPage()
        {
            await _navigationService.Navigate<ReadFileViewModel>().ConfigureAwait(false);
        }

        private void ClearCondition(Condition condition)
        {
            Conditions.Remove(condition);
            FilteredData.Filter = BuildFilter();
        }

        private Predicate<object>? BuildFilter()
        {
            if (!Conditions.Any())
            {
                return null;
            }

            Predicate<object> result = x => Conditions.Any(condition => condition.Predicate(x));

            return result;
        }

        #region Filters

        private Predicate<object> PerformedProcedureStepIDFilter(string value)
        {
            return x => (x as DicomInfoData).PerformedProcedureStepID.Contains(value);
        }

        private Predicate<object> OperatorsNameFilter(string value)
        {
            return x => (x as DicomInfoData).OperatorsName.Contains(value);
        }

        private Predicate<object> PatientBirthDateFilter(string value)
        {
            return x => (x as DicomInfoData).PatientBirthDate.ToShortDateString().Contains(value);
        }

        private Predicate<object> PerformingPhysicansNameFilter(string value)
        {
            return x => (x as DicomInfoData).PerformingPhysicansName.Contains(value);
        }

        private Predicate<object> PerformedProcedureStepStartDateFilter(string value)
        {
            return x => (x as DicomInfoData).PerformedProcedureStepStartDate.ToShortDateString().Contains(value);
        }

        #endregion

        private readonly IDicomInfoDataRepository? _dicomInfoDataRepository;
        private readonly IMvxNavigationService _navigationService;
        private string? _conditionValue;
        private string? _currentCondition;
        private readonly IDictionary<string, Func<string, Predicate<object>>> _conditionSource = new Dictionary<string, Func<string, Predicate<object>>>();
        private ObservableCollection<DicomInfoData> _data;

        public class Condition : MvxViewModel
        {
            public string PropertyName { get; set; }

            public string DisplayValue 
            { 
                get => _displayValue;
                set => SetProperty(ref _displayValue, value);
            }
            public Predicate<object> Predicate { get; set; }

            public Condition(string propertyName, string conditionValue, Func<string, Predicate<object>> predicate)
            {
                PropertyName = propertyName;
                DisplayValue = $"{propertyName}={conditionValue}";
                Predicate = predicate(conditionValue);
            }

            private string _displayValue;
        }
    }
}
