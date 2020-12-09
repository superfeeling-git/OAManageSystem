using System;
using System.Collections.Generic;
using System.Text;

namespace OA.Model.Entity
{
    public class OmsBlog
    {
        public int BlogID { get; set; }
        public string BlogTitle { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime AddTime { get; set; }
        public int CategoryID { get; set; }
        public OmsBlogCategory OmsBlogCategory { get; set; }
    }
}
