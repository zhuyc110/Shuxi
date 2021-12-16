using MvvmCross.ViewModels;
using Shuxi.Core.ViewModels;

namespace Shuxi.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<MainPageViewModel>();
        }
    }
}
