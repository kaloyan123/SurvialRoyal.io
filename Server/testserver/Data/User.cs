using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testserver.Data
{
    public class User : IdentityUser<int>
    {
        public string Useremail { get; set; }

        public string Userusername { get; set; }

    }
}
