using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebASP.Models
{
    public class FilterPostViewModel
    {
        public int? SelectId { get; private set; }
        public string SelectTitle { get; private set; }
        public string SelectUser { get; private set; }
        public string SelectMessage { get; private set; }
        public DateTime SelectDate { get; private set; }

        public int SelectUser_ID { get; private set; }
        public string SelectPicture{ get; private set; }
        public FilterPostViewModel(int? id, string title, string message,int userid, string user, string picture, DateTime date)
        {
            SelectId = id;
            SelectTitle = title;
            SelectMessage = message;
            SelectUser = user;
            SelectUser_ID = userid;
            SelectPicture = picture;
            SelectDate = date;
        }
    }
}
