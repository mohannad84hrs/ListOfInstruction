using MistaksInHaramin.HelperClasses;
using MistaksInHaramin.Models;
using MistaksInHaramin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MistaksInHaramin.ViewData
{
    public enum PostType { Place, Mistake, Azkar, Ahkam, Virtue }
    public enum DataType { Image, Video, Audio, Map, None };

    /// <summary>
    /// PostDataType: analyize the type of data and type of post for the provided post..
    /// specify weather type of data is Image, Video, Map Or None.
    /// specify weather type of post is Place, Mistake, Azkar or Ahkam
    /// </summary>
    public class PostDataType
    {
        private const string MISTAKE_ICON_URI = "x_icon_green_bg.png";
        private const string AZKAR_ICON_URI = "hands_icon_clear_green.png";
        private const string AHKAM_ICON_URI = "book_icon_green_bg.png";
        private const string VIRTUE_ICON_URI= "virtue_green_bg_big.png";
        private const string DEFAULT_NONE_IMAGE = "default_blank_image.jpg";
        

        private Post post;
        public Post Post { get { return post; } }

        public PostType PostType { get; set; }
        public DataType DataType { get; set; }

        public int PostID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        private string dataSource;

        public string DataSource
        {
            get
            {
                if (this.DataType == DataType.Image)
                    return ZadSpecialDesigen.ImageUrl + dataSource;
                else
                    return dataSource;
            }
            set { dataSource = value; }
        }

        public Position Coordinate { get; private set; }

        public string MapAddress { get; set; }

        public void Init()
        {
            post = null;
            this.DataType = DataType.None;
            this.Title = "";
            this.Description = "";
            this.DataSource = "";
        }

        public PostDataType(int postID)
        {
            Init();
            this.post = Helper.GetPost(postID);

            if (post == null)
                return;

            this.PostID = post.Id;

            switch (post.Type)
            {
                case "place":
                    this.PostType = PostType.Place;
                    break;
                case "mistake":
                    this.PostType = PostType.Mistake;
                    break;
                case "azkar":
                    this.PostType = PostType.Azkar;
                    break;
                case "ahkam":
                    this.PostType = PostType.Ahkam;
                    break;
                case "virtue":
                    this.PostType = PostType.Virtue;
                    break;
            }

            if (post != null && post.Content != null)
            {
                List<Media> media = post.media;//when media has value: means type is Video
                List<Attachment> attachment = GetAttachments(); //when attachment has value: means type is image
                if (media != null && media.Count > 0) // if there is available video
                {
                    if (media[0].type == "soundcloud")
                        this.DataType = DataType.Audio;
                    else
                        this.DataType = DataType.Video;

                    this.dataSource = media[0].embeddedLink; // take first one only
                    
                }
                else
                if (attachment != null && attachment.Count > 0)
                {
                    this.DataType = DataType.Image;
                    this.dataSource = attachment[0].DiskName; // take first image only
                }
                else
                    this.DataType = DataType.None;
            }

            if (post != null && post.Content != null && GetContent() != null)
            {
                this.Title = GetContent().Title;
                this.Description = GetContent().Description;
            }

            if (post != null)
            {
                this.Coordinate = new Position(Convert.ToDouble(post.Latitude), Convert.ToDouble(post.Longitude));
            }

            Navigate();
        }

        public PostDataType(Cateogry mistakeCategory, int areaID)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.MistakePlacesPage(mistakeCategory, areaID));
        }

        public void Navigate()
        {
            if (this.PostType == PostType.Place)
            {
                App.Current.MainPage.Navigation.PushAsync(new Views.PlaceDetailsPage(this));
            }
            else
            {
                App.Current.MainPage.Navigation.PushAsync(new Views.DetailPage(this));
            } 
        }

        public string GetImageUri()
        {
            string imageUri = "";
            switch (this.DataType)
            {
                case DataType.Image:
                    imageUri = GetPlaceImageSource();
                    break;
                case DataType.None:
                    imageUri = DEFAULT_NONE_IMAGE; // default image for application
                    break;
            }
            return imageUri;
        }

        public string GetTitleIconUri()
        {
            string iconUri = "";
            switch (PostType)
            {
                case PostType.Place:
                    iconUri = "";
                    break;
                case PostType.Mistake:
                    iconUri = MISTAKE_ICON_URI; //// must be mistake icon
                    break;
                case PostType.Azkar:
                    iconUri = AZKAR_ICON_URI; //// must be azkar icon
                    break;
                case PostType.Ahkam:
                    iconUri = AHKAM_ICON_URI; //// must be ahkam icon
                    break;
                case PostType.Virtue:
                    iconUri = VIRTUE_ICON_URI; //// must be virtue icon
                    break;
            }

            return iconUri;         
        }

        private List<Attachment> GetAttachments()
        {
            return post.Content.ar.Attachments;
        }

        private Content GetContent()
        {
            return post.Content.ar;
        }

        public bool HasMapImage()
        {
            List<Attachment> attachments = GetAttachments();
            if (attachments != null && attachments.Count > 0)
            {
                // when image start with map_ that means this image is for map
                if (attachments.Find(x => x.DiskName.ToUpper().StartsWith("MAP_")) != null)
                    return true;
            }
            return false;
        }

        public bool HasPlaceImage()
        {
            List<Attachment> attachments = GetAttachments();
            if (attachments != null && attachments.Count > 0)
            {
                // when the iamge doesn't start with map_ that mean this image is a normal image just for place
                if (attachments.Find(x => !x.DiskName.ToUpper().StartsWith("MAP_")) != null)
                    return true;                
            }
            return false;
        }

        public string GetMapImageSource()
        {
            List<Attachment> attachments = GetAttachments();
            if (attachments != null && attachments.Count > 0)
            {
               Attachment atach = attachments.Find(x => x.DiskName.ToUpper().StartsWith("MAP_"));
               if(atach != null)
                    return ZadSpecialDesigen.ImageUrl + atach.DiskName;
            }
            return this.DataSource;
        }

        public string GetPlaceImageSource()
        {
            List<Attachment> attachments = GetAttachments();
            if (attachments != null && attachments.Count > 0)
            {
                Attachment atach = attachments.Find(x => !x.DiskName.ToUpper().StartsWith("MAP_"));
                if(atach != null)
                    return ZadSpecialDesigen.ImageUrl + atach.DiskName;
            }      
            return this.DataSource;
        }

        /// <summary>
        /// hasCoordinate or has Longitude and Latitude
        /// </summary>
        /// <returns> return true if there is a longitued and latitude for this place(post) </returns>
        public bool HasCoordinate()
        {
            if (post != null && post.Latitude != null && post.Longitude != null)
                return true;

            return false;
        }
        //////////////////////////////////////////////////////
        //////////////// STATIC METHODS //////////////////////
        //////////////////////////////////////////////////////
    }
}
