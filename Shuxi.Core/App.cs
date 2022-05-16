using DAL;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Shuxi.Core.Services;
using Shuxi.Core.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Shuxi.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

            RegisterAppStart<MainPageViewModel>();

            _logger = Mvx.IoCProvider.GetSingleton<ILoggerFactory>().CreateLogger("root"); ;

            RegisterService();

            ConfigureDataBase();
        }

        private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            ShowCloseDialog(e.Exception);
        }

        private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;

            if (e.IsTerminating)
            {
                _logger.LogError(exception, "Unhanded exception occurred, application will be terminated.");
                ShowCloseDialog(exception);
            }
            else
            {
                _logger.LogWarning(exception, "Unhanded exception occurred application will ignore it.");
            }
        }

        private void ShowCloseDialog(Exception exception)
        {
            var messageBoxTitle = $"Unexpected Error Occurred: {exception.GetType()}";
            var messageBoxMessage = $"The following exception occurred:\n\n{exception}";

            messageBoxMessage += "\n\nNormally the app would die now. Close it?";
            var messageBoxButtons = MessageBoxButton.YesNo;
            // Let the user decide if the app should die or not (if applicable).
            if (MessageBox.Show(messageBoxMessage, messageBoxTitle, messageBoxButtons) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void RegisterService()
        {
            typeof(ShuxiContext).Assembly.CreatableTypes()
                .EndingWith("Repository")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            typeof(DicomReader).Assembly.CreatableTypes()
                .EndingWith("Reader")
                .AsInterfaces()
                .RegisterAsLazySingleton();
        }

        private void ConfigureDataBase()
        {
            try
            {
                var dbContext = new ShuxiContext();
                Mvx.IoCProvider.RegisterSingleton(dbContext);
                _ = dbContext.Database.EnsureCreated();

                _logger.LogInformation("Db initialized.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create database");
            }
        }

        private ILogger? _logger;
    }
}
