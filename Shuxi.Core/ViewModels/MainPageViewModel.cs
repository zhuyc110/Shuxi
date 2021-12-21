using DAL.Model;
using DAL.Repository;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Shuxi.Core.ViewModels
{
    public class MainPageViewModel : MvxViewModel
    {
        public ICommand SearchCommand
        {
            get;
            private set;
        }

        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string? Sex
        {
            get => _sex;
            set => SetProperty(ref _sex, value);
        }

        public ObservableCollection<DicomInfoData> Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        //public MainPageViewModel()
        //{
        //    _dicomInfoDataRepository = null;
        //    SearchCommand = new MvxAsyncCommand(Search);
        //    _name = "张三";
        //    _sex = "男";
        //    var testData = new List<DicomInfoData> { new DicomInfoData
        //    {
        //        StudyInstanceId = "123",
        //        PatientName = "张三",
        //        PatientSex = "男",
        //        PatientAge = "18Y"
        //    }};
        //    _data = new MvxObservableCollection<DicomInfoData>(testData);
        //}

        public MainPageViewModel(IDicomInfoDataRepository dicomInfoDataRepository)
        {
            _dicomInfoDataRepository = dicomInfoDataRepository;
            SearchCommand = new MvxAsyncCommand(Search);
            _data = new MvxObservableCollection<DicomInfoData>(_dicomInfoDataRepository.GetAll());
        }

        private async Task Search()
        {
            await Task.CompletedTask;
        }

        private readonly IDicomInfoDataRepository? _dicomInfoDataRepository;
        private string? _name;
        private string? _sex;
        private ObservableCollection<DicomInfoData> _data;
    }
}
