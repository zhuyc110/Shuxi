using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Shuxi.Core.ViewModels;
using System.Collections.ObjectModel;

namespace Shuxi.UI.Views
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

    public class SexSource : ObservableCollection<string>
    {
        public SexSource()
        {

            Add("男");
            Add("女");
        }
    }
}
