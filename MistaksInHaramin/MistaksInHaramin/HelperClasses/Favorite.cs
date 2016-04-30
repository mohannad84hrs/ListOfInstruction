using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MistaksInHaramin.HelperClasses
{
   public static class Favorite
    {
        private const string favoriteFileName = "Faforite.txt";
       async public static void AddToFavofite(int id)
        {
            var fileService = DependencyService.Get<ISaveAndLoad>();
            if (fileService.FileExists(favoriteFileName))
            {
               var favoritelist= await fileService.LoadTextAsync(favoriteFileName);    
                favoritelist += id.ToString() + ";";
                await fileService.SaveTextAsync(favoriteFileName, favoritelist);
            }
            else
            {
                var favoritelist = id.ToString() + ";";
                await fileService.SaveTextAsync(favoriteFileName, favoritelist);
            }
        }
        async public static Task<bool> inFavorite(int id)
        {
            List<int> IDList = new List<int>();
            var fileService = DependencyService.Get<ISaveAndLoad>();
            if (fileService.FileExists(favoriteFileName))
            {            
                var favoritelist = await fileService.LoadTextAsync(favoriteFileName);
                var s1 = favoritelist.Split(';');
                foreach (var s in s1)
                {
                    int i;
                    if (int.TryParse(s, out i))
                        IDList.Add(i);
                }
            }
            return IDList.Exists(x=>x.Equals(id));
        }
        async public static Task<List<int>> GetFavoritList()
        {
            List<int> IDList = new List<int>();
            var fileService = DependencyService.Get<ISaveAndLoad>();
            if (fileService.FileExists(favoriteFileName))
            {
                var favoritelist = await fileService.LoadTextAsync(favoriteFileName);
                if (favoritelist != string.Empty)
                {
                    var s1 =favoritelist.Split(';');
                    foreach(var s in s1)
                    {
                        int i;
                        if(int.TryParse(s,out i))
                            IDList.Add(i);
                    }
                }
            }
            return IDList;

        }
        async public static Task RemoveFromFavorite(int id)
        {
            string newfavoritelist="";
            List<int> IDList = new List<int>();
            var fileService = DependencyService.Get<ISaveAndLoad>();
            if (fileService.FileExists(favoriteFileName))
            {
                var favoritelist = await fileService.LoadTextAsync(favoriteFileName);
                if (favoritelist != string.Empty)
                {
                    var s1 = favoritelist.Split(';');
                    foreach (var s in s1)
                    {
                        int i;
                        if (int.TryParse(s, out i))
                            IDList.Add(i);
                    }
                }
                IDList.Remove(id);
                foreach (var ID in IDList)
                {
                    newfavoritelist += ID.ToString() + ";";
                }
                await fileService.SaveTextAsync(favoriteFileName, newfavoritelist);
            }
        }
            
            




    }
}
