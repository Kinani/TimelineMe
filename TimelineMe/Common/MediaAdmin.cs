/*
 * Hello friend:)
 * We need to make use of Oxford's emotions API here.
 * Do we need to migirate this class to MediaViewModel ? overkill?
  TODO: TagsSpacesSeperated was intended to be used within search.
*/
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Emotion;
using TimelineMe.Models;
using Windows.Media.Editing;
using Windows.Media.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.Media.Transcoding;
using Windows.Storage.Streams;

namespace TimelineMe.Common
{
    public class MediaAdmin
    {
        //private static MediaAdmin _mediaAdmin = new MediaAdmin();
        //private StorageFile currentMediaFile;
        //private string currentMediaName; // get data from db
        //private ProcessedFeatures currentMediaProcessedFeatures; // get data from db

        public StorageFolder mediaFolder;
        public List<Media> MediaList = new List<Media>();
        public List<MediaGroup> MediaGroupList = new List<MediaGroup>();

        private StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public MediaAdmin()
        {

        }


        public void Initialize()
        {
            // TODO: Possible bug{Wont refresh}
            using (var db = new MediaContext())
            {
                MediaList = db.Medias.ToList();
                MediaGroupList = db.MediaGroups.ToList();
            }

        }
        private async Task<AnalysisResult> DoVision(StorageFile file)
        {
            VisionServiceClient visonClient = new VisionServiceClient(MyCredentials.OxfordVision);
            VisualFeature[] visualFeatures = new VisualFeature[] { VisualFeature.Adult, VisualFeature.Description, VisualFeature.Tags };
            var fileStream = await file.OpenAsync(FileAccessMode.Read);
            return await visonClient.AnalyzeImageAsync(fileStream.AsStream(), visualFeatures);
        }

        private async Task<Emotion[]> DoFeel(StorageFile file)
        {
            EmotionServiceClient emotionClient = new EmotionServiceClient(MyCredentials.OxfordEmtoion);
            var fileStream = await file.OpenAsync(FileAccessMode.Read);
            return await emotionClient.RecognizeAsync(fileStream.AsStream());
        }


        public async Task<bool> AddMedia(StorageFile media)
        {
            Media newMedia = new Media();
            
            if ((bool)localSettings.Values["EnableOxford"])
            {
                AnalysisResult analysisResult = await DoVision(media);
                Emotion[] emotions = await DoFeel(media);
                newMedia = ExtractFeatures(analysisResult, emotions);
            }

            newMedia.IsMerged = false;
            newMedia.MediaName = media.Name;
            newMedia.CaptureDate = media.DateCreated.DateTime;
            using (var db = new MediaContext())
            {
                db.Medias.Add(newMedia);
                db.SaveChanges();
            }
            MediaList.Add(newMedia);
            return true;
        }

        public async Task RemoveMedia(StorageFile media)
        {
            Media toDeleteMedia = MediaList.Where(x => x.MediaName == media.Name).FirstOrDefault();
            using (var db = new MediaContext())
            {
                db.Medias.Remove(toDeleteMedia);
                db.SaveChanges();
            }
            MediaList.Remove(toDeleteMedia);
        }


        public async Task MergeMedias(List<Media> medias, MediaGroup currentMediaGroup = null)
        {
            
            // Use Profile for output vid/cmp? file 


            List<StorageFile> mediaFiles = new List<StorageFile>();
            MediaGroup mediaGroup = new MediaGroup();
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            MediaComposition composition = new MediaComposition();
            List<MediaClip> mediaClips = new List<MediaClip>();
            StorageFile timelineVidOutputFile;
            StorageFile timelineCMPOutputFile;
            bool deleteExistingFile = (currentMediaGroup != null) ? false : true;
            #region EmotionsMeanTemp

            double tmpAngerScoreMean = 0;
            double tmpContemptScoreMean = 0;
            double tmpDisgustScoreMean = 0;
            double tmpFearScoreMean = 0;
            double tmpHappinessScoreMean = 0;
            double tmpNeutralScoreMean = 0;
            double tmpSadnessScoreMean = 0;
            double tmpSurpriseScoreMean = 0;

            string tmpHighestEmotionMean = string.Empty;

            #endregion

            foreach (var media in medias)
            {

                mediaFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri(
                        string.Format("ms-appdata:///local/{0}", media.MediaName))));

                tmpAngerScoreMean += media.AngerScore;
                tmpContemptScoreMean += media.ContemptScore;
                tmpDisgustScoreMean += media.DisgustScore;
                tmpFearScoreMean += media.FearScore;
                tmpHappinessScoreMean += media.HappinessScore;
                tmpNeutralScoreMean += media.NeutralScore;
                tmpSadnessScoreMean += media.SadnessScore;
                tmpSurpriseScoreMean += media.SurpriseScore;
            }
            tmpAngerScoreMean = tmpAngerScoreMean / medias.Count;
            tmpContemptScoreMean = tmpContemptScoreMean / medias.Count;
            tmpDisgustScoreMean = tmpDisgustScoreMean / medias.Count;
            tmpFearScoreMean = tmpFearScoreMean / medias.Count;
            tmpHappinessScoreMean = tmpHappinessScoreMean / medias.Count;
            tmpNeutralScoreMean = tmpNeutralScoreMean / medias.Count;
            tmpSadnessScoreMean = tmpSadnessScoreMean / medias.Count;
            tmpSurpriseScoreMean = tmpSurpriseScoreMean / medias.Count;

            SortedList<string, double> tempForSortingScores = new SortedList<string, double>();
            tempForSortingScores.Add("AngerScoreMean", tmpAngerScoreMean);
            tempForSortingScores.Add("ContemptScoreMean", tmpContemptScoreMean);
            tempForSortingScores.Add("DisgustScoreMean", tmpDisgustScoreMean);
            tempForSortingScores.Add("FearScoreMean", tmpFearScoreMean);
            tempForSortingScores.Add("HappinessScoreMean", tmpHappinessScoreMean);
            tempForSortingScores.Add("NeutralScoreMean", tmpNeutralScoreMean);
            tempForSortingScores.Add("SadnessScoreMean", tmpSadnessScoreMean);
            tempForSortingScores.Add("SurpriseScoreMean", tmpSurpriseScoreMean);
            // TODO: check this.
            tmpHighestEmotionMean = tempForSortingScores.OrderByDescending(x => x.Value).FirstOrDefault().Key;

            if (currentMediaGroup == null)
            {
                timelineVidOutputFile = await localFolder.CreateFileAsync(
               "timelineMeOutput.mp4", CreationCollisionOption.GenerateUniqueName);

                string CMPFileName = string.Format(timelineVidOutputFile.DisplayName + ".cmp");
                timelineCMPOutputFile = await localFolder.CreateFileAsync(
               CMPFileName, CreationCollisionOption.OpenIfExists);

                for (int i = 0; i < mediaFiles.Count; i++)
                {
                    mediaClips.Add(await MediaClip.CreateFromImageFileAsync(mediaFiles[i], TimeSpan.FromSeconds(int.Parse(localSettings.Values["DurationInSecForEachImage"].ToString()))));
                    composition.Clips.Add(mediaClips[i]);
                }


                using (var db = new MediaContext())
                {
                    mediaGroup = new MediaGroup()
                    {
                        CompostionFileName = timelineVidOutputFile.DisplayName,
                        LastEditDate = DateTime.Now,
                        AngerScoreMean = tmpAngerScoreMean,
                        ContemptScoreMean = tmpContemptScoreMean,
                        DisgustScoreMean = tmpDisgustScoreMean,
                        FearScoreMean = tmpFearScoreMean,
                        HappinessScoreMean = tmpHappinessScoreMean,
                        NeutralScoreMean = tmpNeutralScoreMean,
                        SadnessScoreMean = tmpSadnessScoreMean,
                        SurpriseScoreMean = tmpSurpriseScoreMean,
                        HighestEmotionMean = tmpHighestEmotionMean

                    };

                    db.MediaGroups.Add(mediaGroup);
                    db.SaveChanges();

                }

                var action = composition.SaveAsync(timelineCMPOutputFile);
                action.Completed = (info, status) =>
                {
                    if (status != AsyncStatus.Completed)
                    {
                        //ShowErrorMessage("Error saving composition");
                    }

                };
                


            }
            else
            {
                using (var db = new MediaContext())
                {
                    mediaGroup =
                        db.MediaGroups.Where(
                            x => x.CompostionFileName == currentMediaGroup.CompostionFileName).
                            FirstOrDefault();
                    // TODO Check this
                    mediaGroup.LastEditDate = DateTime.Now;
                    mediaGroup.AngerScoreMean = mediaGroup.AngerScoreMean + tmpAngerScoreMean / 2;
                    mediaGroup.ContemptScoreMean = mediaGroup.ContemptScoreMean + tmpContemptScoreMean / 2;
                    mediaGroup.DisgustScoreMean = mediaGroup.DisgustScoreMean + tmpDisgustScoreMean / 2;
                    mediaGroup.FearScoreMean = mediaGroup.FearScoreMean + tmpFearScoreMean / 2;
                    mediaGroup.HappinessScoreMean = mediaGroup.HappinessScoreMean + tmpHappinessScoreMean / 2;
                    mediaGroup.NeutralScoreMean = mediaGroup.NeutralScoreMean + tmpNeutralScoreMean / 2;
                    mediaGroup.SadnessScoreMean = mediaGroup.SadnessScoreMean + tmpSadnessScoreMean / 2;
                    mediaGroup.SurpriseScoreMean = mediaGroup.SurpriseScoreMean + tmpSurpriseScoreMean / 2;

                    SortedList<string, double> temptempForSortingScores = new SortedList<string, double>();
                    temptempForSortingScores.Add("AngerScoreMean", mediaGroup.AngerScoreMean);
                    temptempForSortingScores.Add("ContemptScoreMean", mediaGroup.ContemptScoreMean);
                    temptempForSortingScores.Add("DisgustScoreMean", mediaGroup.DisgustScoreMean);
                    temptempForSortingScores.Add("FearScoreMean", mediaGroup.FearScoreMean);
                    temptempForSortingScores.Add("HappinessScoreMean", mediaGroup.HappinessScoreMean);
                    temptempForSortingScores.Add("NeutralScoreMean", mediaGroup.NeutralScoreMean);
                    temptempForSortingScores.Add("SadnessScoreMean", mediaGroup.SadnessScoreMean);
                    temptempForSortingScores.Add("SurpriseScoreMean", mediaGroup.SurpriseScoreMean);
                    // TODO: check this.
                    mediaGroup.HighestEmotionMean  = temptempForSortingScores.OrderByDescending(x => x.Value).FirstOrDefault().Key;
                    
                    db.MediaGroups.Update(mediaGroup);
                    db.SaveChanges();
                }

                timelineVidOutputFile = await localFolder.GetFileAsync(currentMediaGroup.CompostionFileName + ".mp4");
                timelineCMPOutputFile = await localFolder.GetFileAsync(currentMediaGroup.CompostionFileName + ".cmp");
                composition = await MediaComposition.LoadAsync(timelineCMPOutputFile);
                //TODO: make sure this works.
                for (int i = 0; i < mediaFiles.Count; i++)
                {
                    mediaClips.Add(await MediaClip.CreateFromImageFileAsync(mediaFiles[i], TimeSpan.FromSeconds(int.Parse(localSettings.Values["DurationInSecForEachImage"].ToString()))));
                    composition.Clips.Add(mediaClips[i]);
                }
                
                var action = composition.SaveAsync(timelineCMPOutputFile);
                action.Completed = (info, status) =>
                {
                    if (status != AsyncStatus.Completed)
                    {
                        //ShowErrorMessage("Error saving composition");
                    }

                };
            }

        }



        public Media ExtractFeatures(AnalysisResult rawResult, Emotion[] emotions)
        {
            Media _processedMedia = new Media();
            _processedMedia.adultScore = rawResult.Adult.AdultScore;
            if (rawResult.Description.Captions.Length > 0)
            {
                _processedMedia.CaptionsSoloConf = rawResult.Description.Captions[0].Confidence;
                _processedMedia.CaptionSoloText = rawResult.Description.Captions[0].Text;
            }
            if (rawResult.Tags.Length > 0)
            {
                StringBuilder tagsBuilder = new StringBuilder();
                foreach (var tag in rawResult.Tags)
                {
                    tagsBuilder.Append(tag.Name);
                    tagsBuilder.Append(" ");
                }
                _processedMedia.TagsSpacesSeperated = tagsBuilder.ToString();
            }


            _processedMedia.HighestEmotion = emotions[0].Scores.ToRankedList().ElementAt(0).Key;


            _processedMedia.AngerScore = emotions[0].Scores.Anger;
            _processedMedia.ContemptScore = emotions[0].Scores.Contempt;
            _processedMedia.DisgustScore = emotions[0].Scores.Disgust;
            _processedMedia.FearScore = emotions[0].Scores.Fear;
            _processedMedia.HappinessScore = emotions[0].Scores.Happiness;
            _processedMedia.NeutralScore = emotions[0].Scores.Neutral;
            _processedMedia.SadnessScore = emotions[0].Scores.Sadness;
            _processedMedia.SurpriseScore = emotions[0].Scores.Surprise;



            return _processedMedia;
        }
    }
}
