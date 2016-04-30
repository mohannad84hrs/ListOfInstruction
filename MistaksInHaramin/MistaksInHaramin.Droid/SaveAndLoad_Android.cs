using System;
using Xamarin.Forms;
using MistaksInHaramin.Droid;
using System.IO;
using System.Threading.Tasks;
using MistaksInHaramin;

[assembly: Dependency(typeof(SaveAndLoad_Android))]

namespace MistaksInHaramin.Droid
{
    public class SaveAndLoad_Android : ISaveAndLoad
    {
        #region ISaveAndLoad implementation

        public async Task SaveTextAsync(string filename, string text)
        {
            var path = CreatePathToFile(filename);
            using (StreamWriter sw = File.CreateText(path))
                await sw.WriteAsync(text);
           
        }

        public async Task<string> LoadTextAsync(string filename)
        {
            var path = CreatePathToFile(filename);
            using (StreamReader sr = File.OpenText(path))
                return await sr.ReadToEndAsync();
        }

        public bool FileExists(string filename)
        {
            return File.Exists(CreatePathToFile(filename));
        }

        #endregion

        string CreatePathToFile(string filename)
        {
            var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(docsPath, filename);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}