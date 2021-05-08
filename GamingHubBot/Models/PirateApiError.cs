using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingHubBot.Application
{
    public class Error
    {
        public int code { get; set; }
        public string message { get; set; }
    }

    public class RootError
    {
        public Error error { get; set; }
    }
}
