using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MistaksInHaramin.Models;
using Xamarin.Forms;
using System.Text.RegularExpressions;

namespace MistaksInHaramin.HelperClasses
{
    class Helper
    {

        private static string FAVORITE_FOOTER_ICON_URI = "bookmark_footer_icon.png";
        private static string IS_FAVORITE_FOOTER_ICON_URI = "small_icon_bookmark_white.png";

        public static string ShareTitleOfChooseApp { get { return "Choose an share app"; } }

        public static Label NoContentLable { get {
                return new Label()
                {
                    Text = "لا يوجد محتوى",
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = ZadSpecialDesigen.GetFontSizeOfTitle(),
                    TextColor = Color.Black,
                }; } }

        public static string AboutInfoImageUri { get { return "about_page_conent.jpg"; } }


        /// <summary>
        /// IsExistsInArry: search in a provided array and check whether the provided integer(id) is exists or not
        /// </summary>
        /// <param name="id"></param>
        /// <param name="searchInArray"></param>
        /// <returns>return true if the id is exists, and false if not</returns>
        public static bool IsExistsInArray(int id, int[] searchInArray)
        {
            if (searchInArray == null)
                return false;
            
            foreach (var item in searchInArray)
            {
                if (item == id)
                    return true;
            }
            return false;
        }

        public static string GetTitle(Cateogry cateogry)
        {
            if(cateogry.Contents != null || cateogry.Contents.ar != null)
                return cateogry.Contents.ar.Title;

            return "";
        }

        public static string GetTitle(Post post)
        {
            if (post.Content != null || post.Content.ar != null)
                return post.Content.ar.Title;

            return "";
        }

        public static string GetNoInternetConnectinHttpStyle()
        {
            return @"
            <html> <body>
             <h1 style='color:#666666'; align = 'center'> لا يوجد اتصال بالإنترنت </h1>

              <h2 style='color:#666666'; align = 'center'> لا يمكن عرض المقطع، يرجى الاتصال بالإنترنت والمحاولة مرة أخرى </h2>
            </body> </html> ";
        }

		public async static Task<HtmlWebViewSource> GetAsyncNoInternetHTMLWebViewSource()
		{
			var htmlSource = new HtmlWebViewSource();
			htmlSource.Html = @"
            <html> <body>
             <h1 style='color:#666666'; align = 'center'> لا يوجد اتصال بالإنترنت </h1>

              <h2 style='color:#666666'; align = 'center'> لا يمكن عرض المقطع، يرجى الاتصال بالإنترنت والمحاولة مرة أخرى </h2>
            </body> </html> ";

			return htmlSource;
		}

        /// <summary>
        /// IsRelatedToArea: check whether a provided post is related to provided area
        /// </summary>
        /// <param name="post"></param>
        /// <param name="areaID"></param>
        /// <returns></returns>
        public static bool IsRelatedToArea(Post post, int areaID)
        {
            if (post == null || post.Taxonomies == null || post.Taxonomies.Area == null || post.Taxonomies.Area.Length == 0)
                return false;

            if (Helper.IsExistsInArray(areaID, post.Taxonomies.Area))
                return true;

            return false;
        }

        public static string GetCurrentLanguageTitle()
        {
            Language cur = App.CurrentLanguage;
            string title = "";
            switch (cur)
            {
                case Language.Arabic:
                    title = "عربي";
                    break;
                case Language.English:
                    title = "انكليزي";
                    break;
            }
            return title;
        }

        /// <summary>
        /// postId: Get the Post object of an provided postId
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public static Post GetPost(int postId)
        {
            if (App.Data.Data == null || App.Data.Data.Count == 0)
                return null;

            return App.Data.Data.Find(x => x.Id.Equals(postId));
        }
 
        public async static Task<string> GetAutoFavoriteIcon(int postId)
        {
            bool isFav = await Favorite.inFavorite(postId);
            if (isFav)
                return IS_FAVORITE_FOOTER_ICON_URI;
            else
               return FAVORITE_FOOTER_ICON_URI;
        }

        public static string GetUnFavoriteIconUri()
        {
            return FAVORITE_FOOTER_ICON_URI;
        }

        public static string GetIsFavoriteIconUri()
        {
            return IS_FAVORITE_FOOTER_ICON_URI;
        }

        public static Language GetLanguage(string strSequence)
        {
            switch (strSequence)
            {
                case "1":
                    return Language.English;
                default: // "0"
                    return Language.Arabic;
            }
        }
    }
    public static class StringEx
    {
        public static string CutString(this string sb, int MaxLength)
        {
            string s = Regex.Replace(sb, @"<[^>]+>|&nbsp;", "").Trim();
            string sc;
            if (s.Length > MaxLength)
            {
                sc = s.Substring(0, MaxLength);
                sc = sc + " ...";
                return sc;
            }
            return s;

        }
    }
    
}
