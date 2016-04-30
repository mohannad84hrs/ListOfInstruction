using MistaksInHaramin.Resx;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Xamarin.Forms;

namespace MistaksInHaramin
{

    /// <summary>
    /// the languages enumerator represented in two way.
    /// - by its name (Arabic, English)
    /// - by its sequence number (0, 1)
    /// </summary>
    public enum Language { Arabic, English }

    public class App : Application
    {
        ISaveAndLoad fileService;
        public static Models.JsonDataFromApi Data;
        public static Models.MainCategories Categroies;
        private const string AllDataFile = "Data.json";
        private const string LastDateGetDATAFile = "LastDate.txt";
        private const string CatgoryFileName = "Categroy.json";
        private const string SettingFile = "setting.txt";
        private HelperClasses.ILocationProvider LocService;
        private const string CURRENT_LANGUAGE_FILE = "currentLanguage.txt";
        /// <summary>
        /// General property to know the current selected language
        /// </summary>
        public static Language CurrentLanguage { get; private set; }

        CarouselPage carousal;
        int counter;
        public static NavigationPage navigationPage;
        OurCrousalPage page1, page2;
        public App()
        {

            LocService = DependencyService.Get<HelperClasses.ILocationProvider>();
            page1 = new OurCrousalPage("CarousalPage1.jpg");
            page2 = new OurCrousalPage("CarousalPage2.jpg");
            page1.btnEntry.IsEnabled = false;
            page2.btnEntry.IsEnabled = false;
            counter = 0;
            carousal = new CarouselPage();
            carousal.Children.Add(page1);
            carousal.Children.Add(page2);

            fileService = DependencyService.Get<ISaveAndLoad>();

            page1.btnEntry.Clicked += delegate
            {

                MainPage = new NavigationPage(new HomePage())
                {
                    BarBackgroundColor = ZadSpecialDesigen.ZadGreenNavBar,
                    BarTextColor = Color.White,
                    Icon = "icon2.png",

                };

                page1 = null;
                page2 = null;
                carousal = null;
            };

            page2.btnEntry.Clicked += delegate
            {

                navigationPage = new NavigationPage(new HomePage())
                {
                    BarBackgroundColor = ZadSpecialDesigen.ZadGreenNavBar,
                    BarTextColor = Color.White,
                    Icon = "icon2.png",
                };
                MainPage = navigationPage;
                page1 = null;
                page2 = null;
                carousal = null;
            };


            MainPage = carousal;
            if (Device.OS == TargetPlatform.iOS)
            {
                var time = TimeSpan.FromSeconds(3);
                Device.StartTimer(time, Swapping);
            }

        }
        protected bool Swapping()
        {
            if (carousal != null && counter < 5)
            {
                counter++;
                if (carousal.CurrentPage == page1)
                {

                    carousal.CurrentPage = page2;
                }
                else
                {

                    carousal.CurrentPage = page1;
                }
                var time = TimeSpan.FromSeconds(3);
                Device.StartTimer(time, Swapping);
            }
            return true;
        }
        async protected override void OnStart()
        {
            ////////////////////  Read language settings ////////////////////////////////
            string strLangSequence = await LoadDataFromLocal(CURRENT_LANGUAGE_FILE);
            if (strLangSequence == string.Empty)
            {
                CurrentLanguage = Language.Arabic; // default language
            }
            else
            {
                CurrentLanguage = HelperClasses.Helper.GetLanguage(strLangSequence);
            }

            System.Globalization.CultureInfo ci = null;
            try
            {
                switch (CurrentLanguage)
                {
                    case Language.English:
                        ci = new System.Globalization.CultureInfo("en");
                        break;
                    default:
                        ci = new System.Globalization.CultureInfo("ar");
                        break;
                }
                AppResources.Culture = ci;
            }
            catch (Exception) { }

            /////////////////////////////////////////////////////////////////////////////

            var cts = new System.Threading.CancellationTokenSource();
            // The root page of your application
            var a = DependencyService.Get<HelperClasses.INetworkConnection>();
            if (a.IsConnected)
            {
                try
                {
                    if (!await LoadNewData())
                    {
                        cts.CancelAfter(4000);// time out of update we can't check for updates more then 4sec
                        await CheckForUpdates(cts.Token);
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    if (Categroies != null)
                    {
                        foreach (var area in App.Categroies.CategoryList)
                        {
                            if (area.Type == "area")
                            {
                                if (area.Contents.ar.Title == "مكة")
                                {
                                    page1.AddText(area.Contents.ar.Slider);
                                }
                                else
                                {
                                    page2.AddText(area.Contents.ar.Slider);
                                }
                            }
                        }
                    }


                    page1.btnEntry.IsEnabled = true;
                    page2.btnEntry.IsEnabled = true;
                }
            }
            else
            {
                await LoadOldData();
                if (Categroies != null)
                {
                    foreach (var area in App.Categroies.CategoryList)
                    {
                        if (area.Type == "area")
                        {
                            if (area.Contents.ar.Title == "مكة")
                            {
                                page1.AddText(area.Contents.ar.Slider);
                            }
                            else
                            {
                                page2.AddText(area.Contents.ar.Slider);
                            }
                        }
                    }
                }
                page1.btnEntry.IsEnabled = true;
                page2.btnEntry.IsEnabled = true;
            }
            if (await LoadDataFromLocal(SettingFile) == string.Empty)
            {
                await SaveToLocal(SettingFile, "false");
            }
            else
            {
                bool.TryParse(await LoadDataFromLocal(SettingFile), out SettingPage.LocationEnabled);
                LocService.Enabled = SettingPage.LocationEnabled;

            }

            a = null;


            // Handle when your app starts
        }
        private async System.Threading.Tasks.Task LoadRemoteDataToLocalDataBase(string url, string filename)
        {
            try
            {
                var client = new HttpClient();
                var bb = await client.GetStringAsync(url);
                await SaveToLocal(filename, bb);
                client.Dispose();
            }
            catch (Exception) { }
            return;
        }

        private async System.Threading.Tasks.Task LoadRemoteDataToLocalDataBase(string url, string filename, System.Threading.CancellationToken cts)
        {
            try
            {
                var client = new HttpClient();
                var bb = await client.GetAsync(url, cts);
                await SaveToLocal(filename, await bb.Content.ReadAsStringAsync());
                client.Dispose();
            }
            catch (Exception ex)
            {
                var cc = ex;
            }


        }

        private async System.Threading.Tasks.Task SaveToLocal(string filename, string data)
        {

            await fileService.SaveTextAsync(filename, data);

        }


        private async System.Threading.Tasks.Task<string> LoadDataFromLocal(string filename)
        {

            if (fileService.FileExists(filename))
            {
                return await fileService.LoadTextAsync(filename);
            }
            else
            {
                await fileService.SaveTextAsync(filename, "");
                return "";
            }


        }
        private async System.Threading.Tasks.Task SetTime(string filena)
        {

            if (fileService.FileExists(filena))
            {
                await fileService.SaveTextAsync(filena, "");

                DateTime myDate1 = new DateTime(1970, 1, 1);
                DateTime myDate2 = DateTime.Now.ToUniversalTime();

                TimeSpan myDateResult;


                myDateResult = myDate2 - myDate1;

                int seconds = (int)myDateResult.TotalSeconds;

                await fileService.SaveTextAsync(filena, seconds.ToString());
            }
        }

        private async System.Threading.Tasks.Task<bool> LoadNewData()
        {

            if ((await LoadDataFromLocal(LastDateGetDATAFile) == string.Empty) || (await LoadDataFromLocal(AllDataFile) == string.Empty) || (await LoadDataFromLocal(CatgoryFileName) == string.Empty))
            {

                await LoadRemoteDataToLocalDataBase("http://miaa.muslimapps.net/api/v2/posts?relations=1&limit=0", AllDataFile);
                await LoadRemoteDataToLocalDataBase("http://miaa.muslimapps.net/api/v2/taxonomies?limit=0&relations=1", CatgoryFileName);
                await LoadOldData();
                await SetTime(LastDateGetDATAFile);

                return true;
            }
            return false;
        }
        private async System.Threading.Tasks.Task CheckForUpdates(System.Threading.CancellationToken cts)
        {
            await LoadOldData();
            string urll = "http://miaa.muslimapps.net/api/v2/posts/sync?date=" + await LoadDataFromLocal(LastDateGetDATAFile);
            bool thereisChanges = false;
            try
            {

                var client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(urll, cts);
                var c = await response.Content.ReadAsStringAsync();
                var newthings = JsonConvert.DeserializeObject<Models.Changes>(c);
                if (newthings.Created.Count > 0)
                {
                    foreach (var newthing in newthings.Created)
                    {
                        Data.Data.Add(newthing as Models.Post);
                        thereisChanges = true;
                        await SetTime(LastDateGetDATAFile);
                    }
                }
                if (newthings.Updated.Count > 0)
                {
                    foreach (var newthing in newthings.Updated)
                    {
                        var post = Data.Data.Find(x => x.Id.Equals(newthing.Id));
                        Data.Data.Remove(post);
                        await SetTime(LastDateGetDATAFile);
                        Data.Data.Add(newthing);
                        thereisChanges = true;
                    }
                }
                if (newthings.Deleted.Count > 0)
                {
                    foreach (var newthing in newthings.Deleted)
                    {
                        var post = Data.Data.Find(x => x.Id.Equals(newthing.Id));
                        Data.Data.Remove(post);
                        await SetTime(LastDateGetDATAFile);
                        thereisChanges = true;
                    }
                }
            }
            catch (Exception ex)
            {
                var exs = ex;
            }
            if (thereisChanges)
            {
                string s = JsonConvert.SerializeObject(Data);
                await SaveToLocal(AllDataFile, s);

            }

            await LoadRemoteDataToLocalDataBase("http://miaa.muslimapps.net/api/v2/taxonomies?limit=0&relations=1", CatgoryFileName, cts);
            await LoadOldData();
        }
        private async System.Threading.Tasks.Task LoadOldData()
        {
            Categroies = JsonConvert.DeserializeObject<Models.MainCategories>(await LoadDataFromLocal(CatgoryFileName));
         
            Data = JsonConvert.DeserializeObject<Models.JsonDataFromApi>(await LoadDataFromLocal(AllDataFile));
          
        }
        protected override void OnSleep()
        {

            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public async static void ChangeLanguage(Language newLanguage)
        {
            var fileService = DependencyService.Get<ISaveAndLoad>();

            // save the sequence number of language 0: arabic, 1: English
            string n = Convert.ToInt16(newLanguage).ToString();
            await fileService.SaveTextAsync(CURRENT_LANGUAGE_FILE, n);
            CurrentLanguage = newLanguage;

            System.Globalization.CultureInfo ci = null;
            try
            {
                switch (CurrentLanguage)
                {
                    case Language.English:
                        ci = new System.Globalization.CultureInfo("en");
                        break;
                    default:
                        ci = new System.Globalization.CultureInfo("ar");
                        break;
                }
                AppResources.Culture = ci;
            }
            catch (Exception) { }

           
        }

    }
}
