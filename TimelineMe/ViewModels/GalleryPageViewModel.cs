﻿//    TODO: 
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
using Windows.Media.Editing;
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
        private bool mergeEnable; // TODO feature. [Merge two CMPs]

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

        private bool deleteEnable = false;
        public bool DeleteEnable
        {
            get
            {
                return deleteEnable;
            }
            set
            {
                deleteEnable = value;
                RaisePropertyChanged("DeleteEnable");
            }
        }

        #endregion

        #region Commands
        private RelayCommand<object> asbQuerySubmitted;

        public RelayCommand<object> ASBQuerySubmitted
        {
            get
            {
                try
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
                                    TextBlockHeader = "Images captured:";
                                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher
                                         .RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                                             {
                                                 var dialog = new MessageDialog("No results found:(");
                                                 dialog.Commands.Add(new UICommand("Ok") { Id = 0 });
                                                 await dialog.ShowAsync();
                                             });
                                }
                            });
                }
                catch (Exception)
                {


                }

                return asbQuerySubmitted;
            }
            set
            {
                asbQuerySubmitted = value;
            }
        }

        private RelayCommand finishedSearching;
        public RelayCommand FinishedSearching
        {
            get
            {
                try
                {
                    finishedSearching = new RelayCommand(() =>
                            {
                                SearchResultGridVisibility = Visibility.Collapsed;
                                ContentGridVisibility = Visibility.Visible;
                                TextBlockHeader = "Images captured:";
                            });
                }
                catch (Exception)
                {


                }
                return finishedSearching;
            }
            set
            {
                finishedSearching = value;
            }
        }


        private RelayCommand galleryPageLoaded;


        public RelayCommand GalleryPageLoaded
        {
            get
            {

                try
                {
                    galleryPageLoaded = new RelayCommand(() =>
                            {
                                mediaAdmin.Initialize();
                                ASPMediaCollectionResult = new ObservableCollection<Media>();
                                MediaCollection = new ObservableCollection<Media>(mediaAdmin.MediaList);
                                MediaGroupCollection = new ObservableCollection<MediaGroup>(mediaAdmin.MediaGroupList);
                                selectedCMPCollection = new List<MediaGroup>();//TODO Check this.
                                MediaSelectedON = false;
                                MediaGroupSelectedON = false;
                                TextBlockHeader = "Images captured:";
                                SearchResultGridVisibility = Visibility.Collapsed;
                                ContentGridVisibility = Visibility.Visible;

                            });
                }
                catch (Exception)
                {


                }

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

                try
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
                                        DeleteEnable = true;
                                        if (MediaGroupSelectedON)
                                            MergeEnable = true;
                                    }
                                    else
                                    {
                                        MediaSelectedON = false;
                                        MergeEnable = false;
                                        DeleteEnable = false;
                                    }
                                });
                }
                catch (Exception)
                {


                }
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

                try
                {
                    updateSelectedCMP = new RelayCommand<IList<object>>(
                                selectedCMP =>
                                {
                                    selectedCMPCollection = selectedCMP.Cast<MediaGroup>().ToList();
                                    if (selectedCMPCollection.Count == 1)
                                    {
                                        CMPPreview = true;
                                        DeleteEnable = true;
                                        MediaGroupSelectedON = true;
                                        if (MediaSelectedON)
                                            MergeEnable = true;
                                    }
                                    else
                                    {
                                        CMPPreview = false;
                                        DeleteEnable = false;
                                        MediaGroupSelectedON = false;
                                        if (!MediaSelectedON)
                                            MergeEnable = false;
                                    }
                                    // TODO
                                    //if(selectedCMPCollection.Count >= 2 && !MediaSelectedON)
                                    //{
                                    //    MergeEnable = true;
                                    //}

                                });
                }
                catch (Exception)
                {


                }
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
                try
                {
                    mergeSelectedImages = new RelayCommand(async () =>
                            {
                                await MergeImages();
                            });
                }
                catch (Exception)
                {


                }
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

                try
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
                }
                catch (Exception)
                {


                }
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

                try
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
                }
                catch (Exception)
                {


                }
                return cmpClikcedForPreview;
            }
            set
            {
                cmpClikcedForPreview = value;
            }
        }

        private RelayCommand cmpClikcedForSave;
        public RelayCommand CMPClikcedForSave
        {
            get
            {

                try
                {
                    cmpClikcedForSave = new RelayCommand(async
                               () =>
                            {
                                MediaGroup mediaGroupToSave = null;
                                if (selectedCMPCollection.Count == 1)
                                {
                                    mediaGroupToSave = selectedCMPCollection[0];
                                }
                                //await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher
                                //.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                //{
                                //    App.ShellFrame.Navigate(typeof(PreviewCompostionPage), mediaGroupToSave);

                                //});
                                ContentGridVisibility = Visibility.Collapsed;
                                ProBarVisibility = Visibility.Visible;
                                MediaComposition mediaComposition;
                                StorageFile cmpFile;
                                cmpFile = await ApplicationData.Current.LocalFolder.GetFileAsync(mediaGroupToSave.CompostionFileName + ".cmp");
                                mediaComposition = await MediaComposition.LoadAsync(cmpFile);

                                var picker = new Windows.Storage.Pickers.FileSavePicker();
                                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary;
                                picker.FileTypeChoices.Add("MP4 files", new List<string>() { ".mp4" });
                                picker.SuggestedFileName = "TimelineMe.mp4";

                                StorageFile file = await picker.PickSaveFileAsync();
                                if (file != null)
                                {
                                    // Call RenderToFileAsync
                                    var saveOperation = mediaComposition.RenderToFileAsync(file, MediaTrimmingPreference.Precise);
                                }
                                ContentGridVisibility = Visibility.Visible;
                                ProBarVisibility = Visibility.Collapsed;

                            });
                }
                catch (Exception)
                {


                }
                return cmpClikcedForSave;
            }
            set
            {
                cmpClikcedForSave = value;
            }
        }
        private RelayCommand deleteMedia;
        public RelayCommand DeleteMedia
        {
            get
            {
                try
                {
                    deleteMedia = new RelayCommand(async () =>
                    {
                        if (DeleteEnable)
                        {
                            IUICommand result;
                            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher
                                         .RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                                         {
                                             var dialog = new MessageDialog("Delete selected item(s)?");
                                             dialog.Commands.Add(new UICommand("Yes") { Id = 0 });
                                             dialog.Commands.Add(new UICommand("No") { Id = 1 });
                                             dialog.DefaultCommandIndex = 0;
                                             dialog.CancelCommandIndex = 1;
                                             result = await dialog.ShowAsync();
                                             if (result.Label == "Yes")
                                             {
                                                 ContentGridVisibility = Visibility.Collapsed;
                                                 ProBarVisibility = Visibility.Visible;
                                                 if (selectedMediaCollection.Count > 0)
                                                 {
                                                     foreach (Media item in selectedMediaCollection)
                                                     {
                                                         await mediaAdmin.RemoveMedia(item);
                                                     }
                                                 }
                                                 if (selectedCMPCollection.Count > 0)
                                                 {
                                                     foreach (MediaGroup item in selectedCMPCollection)
                                                     {
                                                         await mediaAdmin.RemoveMedia(item);
                                                     }
                                                 }
                                                 mediaAdmin.Initialize();
                                                 MediaCollection = new ObservableCollection<Media>(mediaAdmin.MediaList);
                                                 MediaGroupCollection = new ObservableCollection<MediaGroup>(mediaAdmin.MediaGroupList);
                                                 ContentGridVisibility = Visibility.Visible;
                                                 ProBarVisibility = Visibility.Collapsed;
                                             }
                                         });
                        }

                    });
                }
                catch (Exception)
                {


                }
                return deleteMedia;
            }
            set
            {
                deleteMedia = value;
            }

        }
        #endregion
        public GalleryPageViewModel()
        {
            try
            {
                mediaAdmin = new MediaAdmin();
            }
            catch (Exception)
            {


            }

        }

        private bool GetMatchingMedias(string userQuery)
        {
            try
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
            catch (Exception)
            {

                return false;
            }
        }

        public async Task OpenCameraUI()
        {
            try
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
            catch (Exception)
            {


            }
        }

        private async Task AddImage(StorageFile image)
        {
            //TODO: Add video resulted from merging
            await mediaAdmin.AddMedia(image);
        }

        //private async Task RemoveImage(StorageFile image)
        //{
        //    await mediaAdmin.RemoveMedia(image);
        //}

        private async Task MergeImages()
        {
            ContentGridVisibility = Visibility.Collapsed;
            ProBarVisibility = Visibility.Visible;
            try
            {
                if (selectedCMPCollection.Count == 1 && selectedMediaCollection.Count >= 1)
                {
                    await mediaAdmin.MergeMedias(selectedMediaCollection, selectedCMPCollection[0]);
                }
                else if (selectedCMPCollection.Count == 0 && selectedMediaCollection.Count >= 1)
                {
                    await mediaAdmin.MergeMedias(selectedMediaCollection);
                }
                // TODO feature.
                //else
                //{
                //    await mediaAdmin.MergeMediaGroups(selectedCMPCollection[0], selectedCMPCollection[1]);
                //}
                mediaAdmin.Initialize();
                MediaCollection = new ObservableCollection<Media>(mediaAdmin.MediaList);
                MediaGroupCollection = new ObservableCollection<MediaGroup>(mediaAdmin.MediaGroupList);
            }
            catch (Exception)
            {


            }

            ContentGridVisibility = Visibility.Visible;
            ProBarVisibility = Visibility.Collapsed;
        }

        public void CleanGalleryUI()
        {
            PreviewEnable = false;
            MergeEnable = false;
            CMPPreview = false;
            DeleteEnable = false;

        }


    }
}

