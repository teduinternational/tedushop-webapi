using System;
using System.Collections.Generic;

namespace TeduShop.Web.Models
{
    public class AppUserViewModel
    {
        public string Id { set; get; }
        public string FullName { set; get; }
        public DateTime BirthDay { set; get; }
        public string Bio { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string UserName { set; get; }

        public string PhoneNumber { set; get; }

        public IEnumerable<ApplicationGroupViewModel> Groups { set; get; }
    }
}