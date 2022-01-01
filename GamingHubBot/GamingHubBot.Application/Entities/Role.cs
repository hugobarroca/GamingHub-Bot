using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingHubBot.Application.Entities
{
    public class Role
    {
        public ulong Id;
        public string Name;
        public bool Permitted;
        public ulong? ColorId;
    }
}
