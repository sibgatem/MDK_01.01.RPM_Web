using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebASP.Models
{
    public class Post
    { 
        [Key]
     public int id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Post_Date { get; set; }
        public int User_ID { get; set; }
        [ForeignKey("User_ID")]
        public User User{ get; set; }
        public string Picture{ get; set; }
       
    }
    public enum SortStatePost
    {
        IdAsc,
        IdDesc,
        TitleAsc,
        TitleDesc,
        User_IDAsc,
        User_IDDesc,
        Post_DateDesc,
        Post_DateAsc
    }
}
