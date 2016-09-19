using Microsoft.EntityFrameworkCore;
using Microsoft.QueryStringDotNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TimelineMe.Common;
using TimelineMe.Models;
using TimelineMe.Views;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TimelineMe
{
    sealed partial class App : Application
    {
        public static Frame ShellFrame;
        public static NotificationsAdmin NAdmin;
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            using (var db = new MediaContext())
            {
                db.Database.Migrate();
            }
            NAdmin = new NotificationsAdmin();
            FirstRunOnly();
        }

        private void FirstRunOnly()
        {
            try
            {
                if (localSettings.Values.ContainsKey("SettingsLoaded"))
                {

                    DateTime dueTime = new DateTime();
                    DateTime.TryParse((string)localSettings.Values["ToastDueTime"], out dueTime);
                    NAdmin.SendAlarmToast(true, dueTime);
                }
                else
                {
                    localSettings.Values["SettingsLoaded"] = true;
                    localSettings.Values["EnableOxford"] = true;
                    localSettings.Values["EnableToast"] = true;
                    localSettings.Values["DurationInSecForEachImage"] = 2;
                    localSettings.Values["ToastSentToday"] = false;
                    localSettings.Values["LastImageName"] = "";
                    TimeSpan temp = new TimeSpan(01, 00, 00);
                    DateTime alarmDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).Add(temp);
                    localSettings.Values["ToastDueTime"] = alarmDate.ToString();
                    localSettings.Values["DueTimeSpanOnly"] = string.Format(@"{0:hh\:mm\:ss}", temp);
                }
            }
            catch (Exception)
            {

                
            }
        }
        private void AdjustScreenMode()
        {
            try
            {
                bool isPhone = ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1);
                ApplicationView view = ApplicationView.GetForCurrentView();
                if (!isPhone)
                {
                    if (view.IsFullScreenMode)
                        view.ExitFullScreenMode();


                }
                else
                {
                    ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
                }
            }
            catch (Exception)
            {

                
            }
        }

        protected override void OnActivated(IActivatedEventArgs e)
        {

            // Get the root frame
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                Window.Current.Content = rootFrame;
            }


            // Handle toast activation
            if (e is ToastNotificationActivatedEventArgs)
            {
                var toastActivationArgs = e as ToastNotificationActivatedEventArgs;

                // Parse the query string
                QueryString args = QueryString.Parse(toastActivationArgs.Argument);

                // See what action is being requested 

                if (args["action"] == "CapturePage")
                {
                    if (ShellFrame.Content is CapturePage)
                    { }
                    else
                    {
                        rootFrame.Navigate(typeof(Shell), e.PreviousExecutionState);
                        //ShellFrame.Navigate(typeof(CapturePage));
                    }
                }
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(Shell), e.PreviousExecutionState);
                }

            }

            // TODO: Handle other types of activation

            // Ensure the current window is active
            AdjustScreenMode();
            Window.Current.Activate();
        }
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = false;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {

                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(Shell), e.Arguments);
                }

                AdjustScreenMode();
                Window.Current.Activate();



            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
