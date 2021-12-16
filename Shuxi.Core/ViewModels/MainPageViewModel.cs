using DAL.Repository;
using MvvmCross.ViewModels;

namespace Shuxi.Core.ViewModels
{
    public class MainPageViewModel : MvxViewModel
    {
        public MainPageViewModel(IDicomInfoDataRepository dicomInfoDataRepository)
        {
            _dicomInfoDataRepository = dicomInfoDataRepository;
        }

        private readonly IDicomInfoDataRepository _dicomInfoDataRepository;
    }
}
