using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebASP.Models
{
    public class PostViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public  FilterPostViewModel FilteViewModel { get; set; }
        public SortPostViewModel SortViewModel { get; set; }
    }
}
