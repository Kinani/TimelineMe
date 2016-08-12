using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace TimelineMe.Converters
{
    public class CompostionFileNameToImageAsyncConverter : IValueConverter
    {
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                string compostionFileName = value as string;
                var task = Task.Run(() => ((GetThumbnailAsync(compostionFileName))));
                return new TaskCompletionNotifier<BitmapImage>(task);
            }
            //TODO
            return new Uri("ms-appx:///Assets/defaultImage.png", UriKind.RelativeOrAbsolute);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public async Task<BitmapImage> GetThumbnailAsync(string mediaGroupName)
        {
            BitmapImage image = null;
            StorageFile mediaGroupCMPFile = null;
            MediaComposition mediaComposition = null;
            var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
            try
            {
                await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                 {
                     image = new BitmapImage();
                     image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;

                     mediaGroupCMPFile = await localFolder.GetFileAsync(mediaGroupName + ".cmp");
                     mediaComposition = await MediaComposition.LoadAsync(mediaGroupCMPFile);

                     await image.SetSourceAsync(await mediaComposition.GetThumbnailAsync(
                         TimeSpan.Zero, 0, 0, VideoFramePrecision.NearestFrame));
                 });

                return image;
            }
            catch (Exception e)
            {

                return null;
            }


        }
        //TODO
        private async Task<bool> FileExists(string fileName)
        {
            try
            {
                StorageFile file = await localFolder.GetFileAsync(fileName);
                return true;
            }
            catch (FileNotFoundException ex)
            {
                return false;
            }
        }
    }

    public sealed class TaskCompletionNotifier<TResult> : INotifyPropertyChanged
    {
        public TaskCompletionNotifier(Task<TResult> task)
        {
            Task = task;
            if (!task.IsCompleted)
            {
                var scheduler = (SynchronizationContext.Current == null) ? TaskScheduler.Current : TaskScheduler.FromCurrentSynchronizationContext();
                task.ContinueWith(t =>
                {
                    var propertyChanged = PropertyChanged;
                    if (propertyChanged != null)
                    {
                        propertyChanged(this, new PropertyChangedEventArgs("IsCompleted"));
                        if (t.IsCanceled)
                        {
                            propertyChanged(this, new PropertyChangedEventArgs("IsCanceled"));
                        }
                        else if (t.IsFaulted)
                        {
                            propertyChanged(this, new PropertyChangedEventArgs("IsFaulted"));
                            propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
                        }
                        else
                        {
                            propertyChanged(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
                            propertyChanged(this, new PropertyChangedEventArgs("Result"));
                        }
                    }
                },
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                scheduler);
            }
        }

        // Gets the task being watched. This property never changes and is never <c>null</c>.
        public Task<TResult> Task { get; private set; }



        // Gets the result of the task. Returns the default value of TResult if the task has not completed successfully.
        public TResult Result { get { return (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : default(TResult); } }

        // Gets whether the task has completed.
        public bool IsCompleted { get { return Task.IsCompleted; } }

        // Gets whether the task has completed successfully.
        public bool IsSuccessfullyCompleted { get { return Task.Status == TaskStatus.RanToCompletion; } }

        // Gets whether the task has been canceled.
        public bool IsCanceled { get { return Task.IsCanceled; } }

        // Gets whether the task has faulted.
        public bool IsFaulted { get { return Task.IsFaulted; } }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
