// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <Locator.cs>
//  Created By: Alexey Golub
//  Date: 14/02/2016
// ------------------------------------------------------------------ 

using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Modboy.Services;
using System.Threading.Tasks;
using Modboy.ViewModels;

namespace Modboy
{
    public class Locator
    {
        private static readonly TaskFactory TaskFactory = new TaskFactory(TaskCreationOptions.PreferFairness,
            TaskContinuationOptions.PreferFairness);

        static Locator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            RegisterServices();
            RegisterViewModels();
        }

        private static void RegisterServices()
        {
            SimpleIoc.Default.Register<AliasService>();
            SimpleIoc.Default.Register<APIService>();
            SimpleIoc.Default.Register<ArchivingService>();
            SimpleIoc.Default.Register<AuthService>();
            SimpleIoc.Default.Register<BackupService>();
            SimpleIoc.Default.Register<CommandExecutionService>();
            SimpleIoc.Default.Register<DatabaseService>();
            SimpleIoc.Default.Register<HistoryService>();
            SimpleIoc.Default.Register<PersistenceService>();
            SimpleIoc.Default.Register<TaskExecutionService>();
            SimpleIoc.Default.Register<TaskFileBufferService>();
            SimpleIoc.Default.Register<UnhandledExceptionService>();
            SimpleIoc.Default.Register<UpdaterService>();
            SimpleIoc.Default.Register<WebService>();
            SimpleIoc.Default.Register<WindowService>();
        }

        private static void RegisterViewModels()
        {
            SimpleIoc.Default.Register<AboutViewModel>();
            SimpleIoc.Default.Register<AliasPromptViewModel>();
            SimpleIoc.Default.Register<ComboSelectViewModel>();
            SimpleIoc.Default.Register<HistoryViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MessageBoxViewModel>();
            SimpleIoc.Default.Register<OverviewViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<BugReportViewModel>();
            SimpleIoc.Default.Register<UpdaterViewModel>();
        }

        /// <summary>
        /// Start the services that require to be started manually
        /// </summary>
        public static async Task StartServicesAsync()
        {
            await TaskFactory.StartNew(() =>
            {
                // Order matters
                Resolve<UnhandledExceptionService>().Start();
                Resolve<AuthService>().Start();
                Resolve<UpdaterService>().Start();
                Resolve<HistoryService>().Start();
                Resolve<TaskFileBufferService>().Start();
            });
        }

        public static TService Resolve<TService>()
        {
            return SimpleIoc.Default.GetInstance<TService>();
        }

        public static TService Resolve<TService>(string uniqueKey)
        {
            return SimpleIoc.Default.GetInstance<TService>(uniqueKey);
        }

        // View models
        public AboutViewModel AboutViewModel => ServiceLocator.Current.GetInstance<AboutViewModel>();
        public AliasPromptViewModel AliasPromptViewModel => ServiceLocator.Current.GetInstance<AliasPromptViewModel>();
        public ComboSelectViewModel ComboSelectViewModel => ServiceLocator.Current.GetInstance<ComboSelectViewModel>();
        public HistoryViewModel HistoryViewModel => ServiceLocator.Current.GetInstance<HistoryViewModel>();
        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public MessageBoxViewModel MessageBoxViewModel => ServiceLocator.Current.GetInstance<MessageBoxViewModel>();
        public OverviewViewModel OverviewViewModel => ServiceLocator.Current.GetInstance<OverviewViewModel>();
        public SettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<SettingsViewModel>();
        public BugReportViewModel BugReportViewModel => ServiceLocator.Current.GetInstance<BugReportViewModel>();
        public UpdaterViewModel UpdaterViewModel => ServiceLocator.Current.GetInstance<UpdaterViewModel>();
    }
}