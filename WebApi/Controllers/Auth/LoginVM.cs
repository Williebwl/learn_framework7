using System;
using System.Collections.Generic;
using System.Linq;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Auth
{
    public class LoginVM : ViewModel
    {
        public string LoginName { get; set; }

        public string Password { get; set; }
    }
}