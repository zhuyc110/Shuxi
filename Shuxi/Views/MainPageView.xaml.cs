using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Shuxi.Core.ViewModels;

namespace Shuxi.Views
{
    /// <summary>
    /// Interaction logic for MainPageView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(MainPageViewModel))]
    public partial class MainPageView : MvxWpfView
    {
        public MainPageView()
        {
            InitializeComponent();
        }
    }
}
