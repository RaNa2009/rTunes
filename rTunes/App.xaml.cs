﻿using System;
using System.Windows;
using System.Windows.Threading;
using rLyrics;

namespace rTunes
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Common.Logger MyLogger = new Common.Logger();
        public static Browser MyBrowser = new Browser();
        public static Fetcher MyLyrics = new Fetcher();

        public App() : base()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalExceptionHandler);
            Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(AppDispatcherUnhandledException);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow wnd = new MainWindow(MyLogger);
            wnd.Show();
        }

        #region Global Exception Handlers
        void GlobalExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = (Exception)args.ExceptionObject;
            Console.WriteLine("Global Exception caught: " + ex.Message);
        }
        void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            Exception ex = args.Exception;
            Console.WriteLine("Dispatcher Exception caught: " + ex.Message);
            args.Handled = true;
        }
        #endregion
    }
}
