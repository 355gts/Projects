using JoelScottFitness.Common.Enumerations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class CreateBlogImageViewModel
    {        
        [DataMember(IsRequired = false)]
        public string ImagePath { get; set; }

        [DataMember(IsRequired = false)]
        public string CaptionTitle { get; set; }

        [DataMember(IsRequired = false)]
        public string Caption { get; set; }

        [DataMember(IsRequired = false)]
        public BlogCaptionTextColour CaptionColour { get; set; }
        
        [DataMember(IsRequired = false)]
        public HttpPostedFileBase PostedFile { get; set; }
        
        public List<KeyValuePair<string, string>> CaptionColours
        {
            get
            {
                return new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>(BlogCaptionTextColour.Black.ToString(), BlogCaptionTextColour.Black.ToString()),
                    new KeyValuePair<string, string>(BlogCaptionTextColour.White.ToString(), BlogCaptionTextColour.White.ToString()),
                };
            }
        }

        //public List<KeyValuePair<string, int>> CaptionColours
        //{
        //    get
        //    {
        //        return new List<KeyValuePair<string, int>>()
        //        {
        //            new KeyValuePair<string, int>(BlogCaptionTextColour.Black.ToString(), (int)BlogCaptionTextColour.Black),
        //            new KeyValuePair<string, int>(BlogCaptionTextColour.White.ToString(), (int)BlogCaptionTextColour.White),
        //        };
        //    }
        //}
    }
}
