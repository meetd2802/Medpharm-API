using System;
using System.Collections.Generic;
using System.Text;

namespace Medpharm.Common
{
    public class CurrentUserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string TimezoneId { get; set; }
    }
}
