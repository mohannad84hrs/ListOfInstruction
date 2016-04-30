using MistaksInHaramin.CustomControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MistaksInHaramin.Models;

namespace MistaksInHaramin.ViewData
{
    public enum RelatedPostType { Supplications, Mistakes, Rules, Virtue}
    public class RelatedPosts
    {
        public string Title { get; set; }

        public string TitleIconUri { get; set; }

        public RelatedPostType Type { get; set; }

        public ObservableCollection<Post> RelatedPost { get; set; }
        public int Count { get { return this.RelatedPost.Count; } }

        public RelatedPosts()
        {
            RelatedPost = new ObservableCollection<Post>();
        }
        public RelatedPosts(string title, string titleIconUri, RelatedPostType type)
        {
            this.Title = title;
            this.TitleIconUri = titleIconUri;
            this.Type = type;
            RelatedPost = new ObservableCollection<Post>();
        }

        public void Add(Post relatedPost)
        {
            this.RelatedPost.Add(relatedPost);
        }

        public bool HasData()
        {
            if (this.RelatedPost.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Static methods: test if spefic post is related to specific place
        /// </summary>
        /// <param name="relatedPost"></param>
        /// <param name="placeID"></param>
        /// <returns>return ture if relatedPost is related to place</returns>
        public static bool IsRelated(Post relatedPost, int placeID)
        {
            bool isRelated = false;
            if (relatedPost.Relations == null || relatedPost.Relations.Place == null || relatedPost.Relations.Place.Length == 0)
                return false;
          
            foreach (var id in relatedPost.Relations.Place)
            {
                if (id == placeID)
                    return true;
            }
            return isRelated;
        }

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
    }
}
