using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;
using MistaksInHaramin.WinPhone;
using MistaksInHaramin;
[assembly: Dependency(typeof(MistaksInHaramin.WinPhone.ISaveAndLoad))]

namespace MistaksInHaramin.WinPhone
{
    public class ISaveAndLoad : MistaksInHaramin.ISaveAndLoad
    {
        #region ISaveAndLoad implementation

        public async Task SaveTextAsync(string filename, string text)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            IStorageFile file = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            using (StreamWriter streamWriter = new StreamWriter(file.Path))
            {
                await streamWriter.WriteAsync(text);
            }
        }

        public async Task<string> LoadTextAsync(string filename)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            IStorageFile file = await localFolder.GetFileAsync(filename);
            string text;

            using (StreamReader streamReader = new StreamReader(file.Path))
            {
                text = await streamReader.ReadToEndAsync();
            }
            return text;
        }
      

        bool MistaksInHaramin.ISaveAndLoad.FileExists(string filename)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            return File.Exists(Path.Combine(localFolder.Path, filename));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
