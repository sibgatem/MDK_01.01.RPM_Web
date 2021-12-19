using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebASP.Models
{
    public class SortPostViewModel
    {

        public SortStatePost IdSort { get; private set; }
        public SortStatePost TitleSort { get; private set; }
        public SortStatePost UserIDSort { get; private set; } 
        public SortStatePost Post_DateSort { get; private set; }
        public SortStatePost Current { get; private set; }

        public SortPostViewModel(SortStatePost sortOrder)
        {
            IdSort = sortOrder == SortStatePost.IdAsc ?
                SortStatePost.IdDesc : SortStatePost.IdAsc;
            TitleSort = sortOrder == SortStatePost.TitleAsc ?
                SortStatePost.TitleDesc : SortStatePost.TitleAsc;
            UserIDSort = sortOrder == SortStatePost.User_IDAsc ?
                SortStatePost.User_IDDesc : SortStatePost.User_IDAsc;
            Post_DateSort = sortOrder == SortStatePost.Post_DateAsc ?
                SortStatePost.Post_DateDesc : SortStatePost.Post_DateAsc;
            Current = sortOrder;
        }
    }
}
