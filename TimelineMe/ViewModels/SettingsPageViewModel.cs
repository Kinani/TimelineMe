﻿using GalaSoft.MvvmLight;
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
        #region Properties
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
                   TLMESettings.EnableOxford = tglesw.IsOn;
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
                    TLMESettings.UseToastNotification = tglesw.IsOn;
                    if (TLMESettings.UseToastNotification)
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
        private RelayCommand<object> timePickerChanged;
        public RelayCommand<object> TimePickerChanged
        {
            get
            {
                timePickerChanged = new RelayCommand<object>(timepicker =>
                {
                    RoutedEventArgs eventArgs = timepicker as RoutedEventArgs;
                    TimePicker timePicker = eventArgs.OriginalSource as TimePicker;
                    TLMESettings.ScheduledDueTime = DateTimeOffset.Parse(timePicker.Time.ToString());
                });
                return timePickerChanged;
            }
            set
            {
                timePickerChanged = value;
            }
        }
        #endregion
        public SettingsPageViewModel() { }
    }
}
