using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimelineMe.Common;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TimelineMe.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        //public TLMESettings settings;
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public NotificationsAdmin NAdmin;
        #region Properties
        private TimeSpan reminderDueTime;
        public TimeSpan ReminderDueTime
        {
            get
            {
                return reminderDueTime;
            }
            set
            {
                try
                {
                    reminderDueTime = value;
                    DateTime alarmDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).Add(value);
                    localSettings.Values["ToastDueTime"] = alarmDate.ToString();
                    localSettings.Values["DueTimeSpanOnly"] = value.ToString(@"hh\:mm\:ss");
                    RaisePropertyChanged("ReminderDueTime");
                }
                catch (Exception)
                {

                    
                }
            }
        }
        private bool enableOxford;
        public bool EnableOxford
        {
            get
            {
                return enableOxford;
            }
            set
            {
                try
                {
                    enableOxford = value;
                    if (value)
                        localSettings.Values["EnableOxford"] = true;
                    else
                        localSettings.Values["EnableOxford"] = false;

                    RaisePropertyChanged("EnableOxford");
                }
                catch (Exception)
                {

                    
                }
            }
        }
        private string durationInSec = "2";
        public string DurationInSec
        {
            get
            {
                return durationInSec;
            }
            set
            {

                try
                {
                    if (value != null && value != string.Empty)
                    {
                        durationInSec = value;
                        localSettings.Values["DurationInSecForEachImage"] = int.Parse(value);
                        RaisePropertyChanged("DurationInSec");

                    }
                }
                catch (Exception)
                {

                    
                }
            }
        }
        private bool enableToast;
        public bool EnableToast
        {
            get
            {
                return enableToast;
            }
            set
            {
                try
                {
                    enableToast = value;
                    if (value)
                        localSettings.Values["EnableToast"] = true;
                    else
                        localSettings.Values["EnableToast"] = false;
                    RaisePropertyChanged("EnableToast");
                }
                catch (Exception)
                {

                    
                }
            }
        }
        private bool isAlarmTimePickerOn = true;
        public bool IsAlarmTimePickerOn
        {
            get
            {
                return isAlarmTimePickerOn;
            }
            set
            {
                isAlarmTimePickerOn = value;
                RaisePropertyChanged("IsAlarmTimePickerOn");
            }
        }
        #endregion

        #region Commands
        private RelayCommand<object> oxfordToggled;
        public RelayCommand<object> OxfordToggled
        {
            get
            {
                try
                {
                    oxfordToggled = new RelayCommand<object>(tgle =>
                            {
                                RoutedEventArgs eventArgs = tgle as RoutedEventArgs;
                                ToggleSwitch tglesw = eventArgs.OriginalSource as ToggleSwitch;
                                EnableOxford = tglesw.IsOn;
                            });
                }
                catch (Exception)
                {

                    
                }
                return oxfordToggled;
            }
            set
            {
                oxfordToggled = value;
            }
        }
        private RelayCommand<object> toastToggled;
        public RelayCommand<object> ToastToggled
        {
            get
            {
                try
                {
                    toastToggled = new RelayCommand<object>(tgle =>
                            {
                                RoutedEventArgs eventArgs = tgle as RoutedEventArgs;
                                ToggleSwitch tglesw = eventArgs.OriginalSource as ToggleSwitch;
                                EnableToast = tglesw.IsOn;
                                if (EnableToast)
                                    IsAlarmTimePickerOn = true;
                                else
                                    IsAlarmTimePickerOn = false;
                            });
                }
                catch (Exception)
                {

                    
                }
                return toastToggled;
            }
            set
            {
                toastToggled = value;
            }
        }


        #endregion


        public SettingsPageViewModel()
        {
            try
            {
                NAdmin = new NotificationsAdmin();
                if (localSettings.Values.ContainsKey("SettingsLoaded"))
                {
                    EnableOxford = (bool)localSettings.Values["EnableOxford"];
                    EnableToast = (bool)localSettings.Values["EnableToast"];
                    IsAlarmTimePickerOn = (bool)localSettings.Values["EnableToast"];
                    DurationInSec = localSettings.Values["DurationInSecForEachImage"] as string;
                    TimeSpan temp = new TimeSpan();
                    string x = (string)localSettings.Values["DueTimeSpanOnly"];
                    TimeSpan.TryParse(x, out temp);
                    ReminderDueTime = temp;
                }
            }
            catch (Exception)
            {

                
            }
        }
    }
}
