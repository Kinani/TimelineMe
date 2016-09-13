using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace TimelineMe.Common
{
    public class TLMESettings
    {
        [JsonProperty]
        public DateTimeOffset ScheduledDueTime { get; set; }
        //public static DateTimeOffset ScheduledDueTime { get; set; } = DateTimeOffset.UtcNow.AddSeconds(5);
        //public static DateTimeOffset ScheduledDueTime { get; set; } = DateTimeOffset.Parse("");
        [JsonProperty]
        public bool EnableOxford { get; set; }
        [JsonProperty]
        public bool UseToastNotification { get; set; }
        [JsonProperty]
        public int DurationInSecForEachItem { get; set; }
        //public static bool UseExternalCamApp { get; set; }


        //public static bool EnableOxford { get; set; } = true;
        //public static bool UseToastNotification { get; set; } = true;
        //public static int DurationInSecForEachItem { get; set; } = 2;
        //public static bool UseExternalCamApp { get; set; } = true;

        public TLMESettings()
        {
            ScheduledDueTime = DateTimeOffset.Now;
            EnableOxford = true;
            UseToastNotification = true;
            DurationInSecForEachItem = 2;
        }
    }
    public static class TLMESettingsStore
    {
        public static async Task<bool> SaveSettings(TLMESettings settClass)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(settClass);
                StorageFile settingsFile = await ApplicationData.Current.LocalFolder.
                    CreateFileAsync("settings.json", CreationCollisionOption.ReplaceExisting);
                using (IRandomAccessStream textStream = await settingsFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (DataWriter textWriter = new DataWriter(textStream))
                    {
                        textWriter.WriteString(jsonContent);
                        await textWriter.StoreAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static async Task<TLMESettings> LoadSettingsAsync()
        {
            TLMESettings settFile = new TLMESettings();
            try
            {
                StorageFile settingsFile = await ApplicationData.Current.LocalFolder.
                    CreateFileAsync("settings.json", CreationCollisionOption.OpenIfExists);

                using (IRandomAccessStream textStream = await settingsFile.OpenReadAsync())
                {
                    using (DataReader textReader = new DataReader(textStream))
                    {
                        uint textLength = (uint)textStream.Size;
                        await textReader.LoadAsync(textLength);

                        //Read It
                        string jsonContents = textReader.ReadString(textLength);
                        settFile = JsonConvert.DeserializeObject<TLMESettings>(jsonContents);

                    }
                }
                if (settFile == null)
                    return settFile = new TLMESettings();
                else
                    return settFile;
            }
            catch (Exception ex)
            {

                return settFile = new TLMESettings();
            }

        }
    }
}
