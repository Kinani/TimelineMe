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


        private StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        public MediaAdmin()
        {
            
        }


        public void Initialize()
        {
            // Edited: Removed Task
            //try
            //{
            //    mediaFolder = await localFolder.CreateFolderAsync("MediaFolder", CreationCollisionOption.FailIfExists);

            //}
            //catch (Exception)
            //{

                
            //}
            using (var db = new MediaContext())
            {
                MediaList = db.Medias.ToList();
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
            AnalysisResult analysisResult = await DoVision(media);
            Emotion[] emotions = await DoFeel(media);
            newMedia = ExtractFeatures(analysisResult, emotions);
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
            using(var db = new MediaContext())
            {
                db.Medias.Remove(toDeleteMedia);
                db.SaveChanges();
            }
            MediaList.Remove(toDeleteMedia);
        }
        
        public async Task MergeMedias(List<StorageFile> mediaFiles,double duration = 2)
        {
            /*
            TODO:
            check MediaTrimmingPrefrence option
            output progress
            */
            MediaComposition compostion = new MediaComposition();
            List<MediaClip> mediaClips = new List<MediaClip>();
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile timelineOutputFolder = await localFolder.CreateFileAsync(
                "timelineMeOutput", CreationCollisionOption.GenerateUniqueName);
            //TODO: make sure this works.
            for(int i =0; i<= mediaFiles.Count;i++)
            {
                mediaClips.Add(await MediaClip.CreateFromImageFileAsync(mediaFiles[i], TimeSpan.FromSeconds(duration)));
                compostion.Clips.Add(mediaClips[i]);
            }
            var saveOperation = await compostion.RenderToFileAsync(timelineOutputFolder, MediaTrimmingPreference.Precise);

            
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
