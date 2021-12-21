using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;

namespace Shuxi.UI.Views
{
    /// <summary>
    /// Interaction logic for ReadFileView.xaml
    /// </summary>
    [MvxWindowPresentation(Identifier = nameof(ReadFileView), Modal = true)]
    public partial class ReadFileView : MvxWindow
    {
        public ReadFileView()
        {
            InitializeComponent();
        }
    }
}
