using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.QueryStringDotNET;

namespace TimelineMe.Common
{
    public class NotificationsAdmin
    {
        public string DefaultTitel { get; private set; } = "TimelineMe";
        public string DefaultContent { get; private set; } = "TimelineMe needs to see you today!";
        public string DefaultLogo { get; private set; } = "ms-appx:///Assets/.jpg";

        public void SendAlarmToast(bool UseDefaultToast,DateTime timeOffset, string title = "", string content = "")
        {
            if (UseDefaultToast)
            {
                ToastVisual visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                           new AdaptiveText()
                           {
                              Text = DefaultTitel
                           },
                           new AdaptiveText()
                           {
                              Text = DefaultContent
                           }
                        },
                        //AppLogoOverride = new ToastGenericAppLogo()
                        //{
                        //    Source = DefaultLogo,
                        //    HintCrop = ToastGenericAppLogoCrop.Circle
                        //}
                    }
                };

                ToastContent toastContent = new ToastContent()
                {
                    Scenario = ToastScenario.Alarm,
                    Audio = new ToastAudio()
                    {
                        Src = new Uri("ms-winsoundevent:Notification.Looping.Alarm")
                    },
                    Visual = visual,
                    Actions = new ToastActionsCustom()
                    {
                        Buttons =
                        {
                           new ToastButtonSnooze(),
                           new ToastButtonDismiss()
                        }
                    },
                    Launch = new QueryString()
                    {
                        { "action","CapturePage" }
                    }.ToString()
                };
                try
                {
                    ToastNotificationManager.History.Clear();
                    ScheduledToastNotification toast = new ScheduledToastNotification(toastContent.GetXml(), timeOffset);
                    ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
                }
                catch
                { }
                
            }
            else
            {
                ToastVisual visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                           new AdaptiveText()
                           {
                              Text = title
                           },
                           new AdaptiveText()
                           {
                              Text = content
                           }
                        },
                        //AppLogoOverride = new ToastGenericAppLogo()
                        //{
                        //    Source = DefaultLogo,
                        //    HintCrop = ToastGenericAppLogoCrop.Circle
                        //}
                    }

                };

                ToastContent toastContent = new ToastContent()
                {
                    Scenario = ToastScenario.Alarm,
                    Audio = new ToastAudio()
                    {
                        Src = new Uri("ms-winsoundevent:Notification.Looping.Alarm")
                    },
                    Visual = visual,
                    Actions = new ToastActionsCustom()
                    {
                        Buttons =
                        {
                           new ToastButtonSnooze(),
                           new ToastButtonDismiss()
                           
                        }
                    },
                    Launch = new QueryString()
                    {
                        { "action","CapturePage" }
                    }.ToString()
                };
                
                try
                {
                    ToastNotificationManager.History.Clear();
                    ScheduledToastNotification toast = new ScheduledToastNotification(toastContent.GetXml(), timeOffset);
                    ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
                }
                catch
                {

                   
                }
            };
            
        }
    }
}
