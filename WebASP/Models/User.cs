using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebASP.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string middle_Name { get; set; }
        public DateTime birth_day { get; set; }
        public bool Role { get; set; }
        public string Picture { get; set; }
        


    }
    public enum SortState
    {
        IdAsc,
        IdDesc,
        EmailAsc, 
        EmailDesc, 
        LoginAsc, 
        LoginDesc
    }
}
