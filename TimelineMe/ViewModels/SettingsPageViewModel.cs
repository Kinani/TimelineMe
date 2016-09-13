using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimelineMe.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TimelineMe.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        //public TLMESettings settings;
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
                reminderDueTime = value;
                App.GlobalSettings.ScheduledDueTime = DateTimeOffset.Parse(value.ToString());
                RaisePropertyChanged("ReminderDueTime");
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
                enableOxford = value;
                App.GlobalSettings.EnableOxford = value;
                RaisePropertyChanged("EnableOxford");
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
                enableToast = value;
                App.GlobalSettings.UseToastNotification = value;
                RaisePropertyChanged("EnableToast");
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
               oxfordToggled = new RelayCommand<object>(tgle =>
               {
                   RoutedEventArgs eventArgs = tgle as RoutedEventArgs;
                   ToggleSwitch tglesw = eventArgs.OriginalSource as ToggleSwitch;
                   EnableOxford = tglesw.IsOn;
               });
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
                return toastToggled;
            }
            set
            {
                toastToggled = value;
            }
        }
        //private RelayCommand<object> timePickerChanged;
        //public RelayCommand<object> TimePickerChanged
        //{
        //    get
        //    {
        //        timePickerChanged = new RelayCommand<object>(timepicker =>
        //        {
        //            RoutedEventArgs eventArgs = timepicker as RoutedEventArgs;
        //            TimePicker timePicker = eventArgs.OriginalSource as TimePicker;
        //            TLMESettings.ScheduledDueTime = DateTimeOffset.Parse(timePicker.Time.ToString());
        //        });
        //        return timePickerChanged;
        //    }
        //    set
        //    {
        //        timePickerChanged = value;
        //    }
        //}

      
        #endregion

        public void TimePickerTimeChangedEvent(object sender, TimePickerValueChangedEventArgs e)
        {
            ReminderDueTime = e.NewTime;
        }
        public async Task OnNavigatingTo()
        {
            App.GlobalSettings = await TLMESettingsStore.LoadSettingsAsync();
        }
        public async Task OnNavigatingFrom()
        {
            //if(EnableToast)
            //    NAdmin.SendAlarmToast(true, App.GlobalSettings.ScheduledDueTime);
            bool success = await TLMESettingsStore.SaveSettings(App.GlobalSettings);
        }
        public SettingsPageViewModel()
        {
            App.GlobalSettings = new TLMESettings();
            NAdmin = new NotificationsAdmin();
            //ReminderDueTime = TimeSpan.Parse(App.GlobalSettings.ScheduledDueTime.ToString("Hh:Mm"));
            EnableOxford = App.GlobalSettings.EnableOxford;
            EnableToast = App.GlobalSettings.UseToastNotification;
        }
    }
}
