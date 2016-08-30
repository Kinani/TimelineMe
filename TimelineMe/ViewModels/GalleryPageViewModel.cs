//    TODO: 
//    Take a look into MergeMedias() tyy.


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
using TimelineMe.Views;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TimelineMe.ViewModels
{
    public class GalleryPageViewModel : ViewModelBase
    {
        private MediaAdmin mediaAdmin;
        private List<Media> selectedMediaCollection;
        private List<MediaGroup> selectedCMPCollection;
        private bool MediaSelectedON, MediaGroupSelectedON = false;
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
        private ObservableCollection<Media> aspMediaCollectionResult;
        public ObservableCollection<Media> ASPMediaCollectionResult
        {
            get
            {
                return aspMediaCollectionResult;
            }
            set
            {
                aspMediaCollectionResult = value;
                RaisePropertyChanged("ASPMediaCollectionResult");
            }
        }
        private ObservableCollection<MediaGroup> mediaGroupCollection;
        public ObservableCollection<MediaGroup> MediaGroupCollection
        {
            get
            {
                return mediaGroupCollection;
            }
            set
            {
                mediaGroupCollection = value;
                RaisePropertyChanged("MediaGroupCollection");
            }
        }

        private bool isSearching = false;
        public bool IsSearching
        {
            get
            {
                return isSearching;
            }
            set
            {
                isSearching = value;
                RaisePropertyChanged("IsSearching");

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

        private ImageSource imageItem;

        public ImageSource ImageItem
        {
            get
            {
                return imageItem;
            }
            set
            {
                imageItem = value;
                RaisePropertyChanged("ImageItem");
            }
        }

        private Media selectedMediaForPreview;
        public Media SelectedMediaForPreview
        {
            get
            {
                return selectedMediaForPreview;
            }
            set
            {
                selectedMediaForPreview = value;
                RaisePropertyChanged("SelectedMediaForPreview");
            }
        }

        private bool previewEnable = false;

        public bool PreviewEnable
        {
            get
            {
                return previewEnable;
            }
            set
            {
                previewEnable = value;
                RaisePropertyChanged("PreviewEnable");
            }
        }
        private bool mergeEnable;

        public bool MergeEnable
        {
            get
            {
                return mergeEnable;
            }
            set
            {
                mergeEnable = value;
                RaisePropertyChanged("MergeEnable");
            }
        }

        private bool cmpPreview = false;

        public bool CMPPreview
        {
            get
            {
                return cmpPreview;
            }
            set
            {
                cmpPreview = value;
                RaisePropertyChanged("CMPPreview");
            }
        }
        private Visibility proBarVisibility = Visibility.Collapsed;

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
        private Visibility contentGridVisibility = Visibility.Visible;

        public Visibility ContentGridVisibility
        {
            get
            {
                return contentGridVisibility;
            }
            set
            {
                contentGridVisibility = value;
                RaisePropertyChanged("ContentGridVisibility");
            }
        }
        private Visibility searchResultGridVisibility = Visibility.Collapsed;
        public Visibility SearchResultGridVisibility
        {
            get
            {
                return searchResultGridVisibility;
            }
            set
            {
                searchResultGridVisibility = value;
                RaisePropertyChanged("SearchResultGridVisibility");
            }
        }
        private string textBlockHeader = "Captured Images";

        public string TextBlockHeader
        {
            get { return textBlockHeader; }
            set
            {
                textBlockHeader = value;
                RaisePropertyChanged("TextBlockHeader");
            }
        }

        #endregion

        #region Commands
        private RelayCommand<object> asbQuerySubmitted;

        public RelayCommand<object> ASBQuerySubmitted
        {
            get
            {
                asbQuerySubmitted = new RelayCommand<object>(async asbArgs =>
                {
                    var args = asbArgs as AutoSuggestBoxQuerySubmittedEventArgs;
                    ContentGridVisibility = Visibility.Collapsed;
                    ProBarVisibility = Visibility.Visible;
                    if (GetMatchingMedias(args.QueryText))
                    {
                        SearchResultGridVisibility = Visibility.Visible;
                        ProBarVisibility = Visibility.Collapsed;
                        TextBlockHeader = "Search results:";
                    }
                    else
                    {
                        ProBarVisibility = Visibility.Collapsed;
                        ContentGridVisibility = Visibility.Visible;
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher
                             .RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                                 {
                                     var dialog = new MessageDialog("No results found:(");
                                     dialog.Commands.Add(new UICommand("K") { Id = 0 });
                                     await dialog.ShowAsync();
                                 });
                    }
                });

                return asbQuerySubmitted;
            }
            set
            {
                asbQuerySubmitted = value;
            }
        }

        private RelayCommand galleryPageLoaded;


        public RelayCommand GalleryPageLoaded
        {
            get
            {

                galleryPageLoaded = new RelayCommand(() =>
                {
                    mediaAdmin.Initialize();
                    ASPMediaCollectionResult = new ObservableCollection<Media>();
                    MediaCollection = new ObservableCollection<Media>(mediaAdmin.MediaList);
                    MediaGroupCollection = new ObservableCollection<MediaGroup>(mediaAdmin.MediaGroupList);
                    MediaSelectedON = false;
                    MediaGroupSelectedON = false;
                    TextBlockHeader = "Images captured:";
                    SearchResultGridVisibility = Visibility.Collapsed;
                    ContentGridVisibility = Visibility.Visible;

                });

                return galleryPageLoaded;
            }
            set
            {
                galleryPageLoaded = value;
            }
        }
        private RelayCommand<IList<object>> updateSelectedImages;
        public RelayCommand<IList<object>> UpdateSelectedImages
        {
            get
            {

                updateSelectedImages = new RelayCommand<IList<object>>(
                    selectedImages =>
                    {
                        selectedMediaCollection = selectedImages.Cast<Media>().ToList();
                        if (selectedMediaCollection.Count == 1)
                        {
                            PreviewEnable = true;

                        }
                        else
                        {
                            PreviewEnable = false;
                            MergeEnable = true;
                        }
                           // TODO
                           if (selectedMediaCollection.Count >= 1)
                        {
                            MediaSelectedON = true;
                            if (MediaGroupSelectedON)
                                MergeEnable = true;
                        }
                        else
                        {
                            MediaSelectedON = false;
                            MergeEnable = false;
                        }
                    });
                return updateSelectedImages;
            }
            set
            {
                updateSelectedImages = value;
            }
        }
        private RelayCommand<IList<object>> updateSelectedCMP;
        public RelayCommand<IList<object>> UpdateSelectedCMP
        {
            get
            {

                updateSelectedCMP = new RelayCommand<IList<object>>(
                    selectedCMP =>
                    {
                        selectedCMPCollection = selectedCMP.Cast<MediaGroup>().ToList();
                        if (selectedCMPCollection.Count == 1)
                        {
                            CMPPreview = true;

                            MediaGroupSelectedON = true;
                            if (MediaSelectedON)
                                MergeEnable = true;
                        }
                        else
                        {
                            CMPPreview = false;

                            MediaGroupSelectedON = false;
                            if (!MediaSelectedON)
                                MergeEnable = false;
                        }

                    });
                return updateSelectedCMP;
            }
            set
            {
                updateSelectedCMP = value;
            }
        }
        private RelayCommand mergeSelectedImages;
        public RelayCommand MergeSelectedImages
        {
            get
            {
                mergeSelectedImages = new RelayCommand(async () =>
                {
                    await MergeImages();
                });
                return mergeSelectedImages;
            }
            set
            {
                mergeSelectedImages = value;
            }
        }
        private RelayCommand mediaClikcedForPreview;
        public RelayCommand MediaClikcedForPreview
        {
            get
            {

                mediaClikcedForPreview = new RelayCommand(async
                   () =>
                   {
                       Media mediaToPreview = null;
                       if (selectedMediaCollection.Count == 1)
                       {
                           mediaToPreview = selectedMediaCollection[0];
                       }
                       await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher
                       .RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            App.ShellFrame.Navigate(typeof(PreviewMediaPage), mediaToPreview);

                        });

                   });
                return mediaClikcedForPreview;
            }
            set
            {
                mediaClikcedForPreview = value;
            }
        }

        private RelayCommand cmpClikcedForPreview;
        public RelayCommand CMPClikcedForPreview
        {
            get
            {

                cmpClikcedForPreview = new RelayCommand(async
                   () =>
                {
                    MediaGroup mediaGroupToPreview = null;
                    if (selectedCMPCollection.Count == 1)
                    {
                        mediaGroupToPreview = selectedCMPCollection[0];
                    }
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher
                    .RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        App.ShellFrame.Navigate(typeof(PreviewCompostionPage), mediaGroupToPreview);

                    });

                });
                return cmpClikcedForPreview;
            }
            set
            {
                cmpClikcedForPreview = value;
            }
        }

        #endregion
        public GalleryPageViewModel()
        {
            mediaAdmin = new MediaAdmin();

        }

        private bool GetMatchingMedias(string userQuery)
        {
            // TODO
            // Improve eff. like how much of the query keyowrds found in each result. but screw it for now.
            string query = userQuery.ToLower();
            bool mediaPass = false;
            bool foundResult = false;
            ASPMediaCollectionResult.Clear();
            foreach (var media in MediaCollection)
            {
                string[] currentMediaTags = media.TagsSpacesSeperated.Split(
                    new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var tag in currentMediaTags)
                {
                    if (query.Contains(tag))
                        mediaPass = true;
                }
                if (mediaPass == true)
                {
                    ASPMediaCollectionResult.Add(media);
                    foundResult = true;
                }

                mediaPass = false;
            }
            return foundResult;
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
            //TODO: Add video resulted from merging
            await mediaAdmin.AddMedia(image);
        }

        private async Task RemoveImage(StorageFile image)
        {
            await mediaAdmin.RemoveMedia(image);
        }

        private async Task MergeImages()
        {
            ContentGridVisibility = Visibility.Collapsed;
            ProBarVisibility = Visibility.Visible;

            await mediaAdmin.MergeMedias(selectedMediaCollection);
            mediaAdmin.Initialize();
            MediaCollection = new ObservableCollection<Media>(mediaAdmin.MediaList);
            MediaGroupCollection = new ObservableCollection<MediaGroup>(mediaAdmin.MediaGroupList);

            ContentGridVisibility = Visibility.Visible;
            ProBarVisibility = Visibility.Collapsed;
        }


    }
}

