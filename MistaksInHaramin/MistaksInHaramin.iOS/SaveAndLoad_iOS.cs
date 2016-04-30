using System;
using Xamarin.Forms;
using MistaksInHaramin.iOS;
using System.IO;
using System.Threading.Tasks;
using MistaksInHaramin;
using Foundation;
using System.Linq;

[assembly: Dependency(typeof(SaveAndLoad_iOS))]

namespace MistaksInHaramin.iOS
{
    public class SaveAndLoad_iOS : ISaveAndLoad
    {
        StreamWriter sw;
        StreamReader sr;
        public static string DocumentsPath
        {
            get
            {
                var documentsDirUrl = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User).Last();
                return documentsDirUrl.Path;
            }
        }

        #region ISaveAndLoad implementation

        public async Task SaveTextAsync(string filename, string text)
        {
            string path = CreatePathToFile(filename);
            using ( sw = File.CreateText(path))
                await sw.WriteAsync(text);
        }

        public async Task<string> LoadTextAsync(string filename)
        {
            string path = CreatePathToFile(filename);
            using ( sr = File.OpenText(path))
                return await sr.ReadToEndAsync();
        }

        public bool FileExists(string filename)
        {
            return File.Exists(CreatePathToFile(filename));
        }

        #endregion

        static string CreatePathToFile(string fileName)
        {
            return Path.Combine(DocumentsPath, fileName);
        }

        public void Dispose()
        {
            sr.Dispose();
            sw.Dispose();

        }
    }
}