using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimelineMe.Common;
using TimelineMe.Models;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml;

namespace TimelineMe.ViewModels
{
    public class CapturePageViewModel : ViewModelBase
    {
        private MediaAdmin mediaAdmin;
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        #region Properties    

        private ObservableCollection<Media> mediaCollection;
        public ObservableCollection<Media> MediaCollection
        {
            get
            {
                return mediaCollection;
            }
            set
            {
                mediaCollection = value;
                RaisePropertyChanged("MediaCollection");
            }
        }

        private string statusString;
        public string StatusString
        {
            get
            {
                return statusString;
            }
            set
            {
                statusString = value;
                RaisePropertyChanged("StatusString");

            }

        }
        private Visibility proBarVisibility;

        public Visibility ProBarVisibility
        {
            get
            {
                return proBarVisibility;
            }
            set
            {
                proBarVisibility = value;
                RaisePropertyChanged("ProBarVisibility");
            }
        }

        private string youLastTime;

        public string YouLastTime
        {
            get
            {
                return youLastTime;
            }
            set
            {
                youLastTime = value;
                RaisePropertyChanged("YouLastTime");
            }
        }

        private bool enableCapture = true;
        public bool EnableCapture
        {
            get
            {
                return enableCapture;
            }
            set
            {
                enableCapture = value;
                RaisePropertyChanged("EnableCapture");
            }
        }
        #endregion

        #region Commands
        private RelayCommand openCamera;

        public RelayCommand OpenCamera
        {
            get
            {
                if (openCamera == null)
                {
                    openCamera = new RelayCommand(async () =>
                    {
                        EnableCapture = false;
                        await OpenCameraUI();
                        EnableCapture = true;
                    });
                }
                return openCamera;
            }
            set
            {
                openCamera = value;
            }
        }
        #endregion

        public CapturePageViewModel()
        {
            mediaAdmin = new MediaAdmin();
            ProBarVisibility = Visibility.Collapsed;
            YouLastTime = string.Format("ms-appdata:///local/{0}", (string)localSettings.Values["LastImageName"]);
        }
        public async Task OpenCameraUI()
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            //captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (photo != null)
            {

                //TODO: Make sure mediaFolder is initialized before moving.
                //await photo.MoveAsync(mediaAdmin.mediaFolder);

                await photo.MoveAsync(ApplicationData.Current.LocalFolder, "timelineme.jpg",
                    NameCollisionOption.GenerateUniqueName);
                await AddImage(photo);
            }
        }

        private async Task AddImage(StorageFile image)
        {
            bool finished;
            ProBarVisibility = Visibility.Visible;
            StatusString = "Some AI spells is being casted, Please wait.";
            finished = await mediaAdmin.AddMedia(image);
            ProBarVisibility = Visibility.Collapsed;
            StatusString = "Done! Thank you friend. you may procced to the Gallery.";
        }
    }
}
