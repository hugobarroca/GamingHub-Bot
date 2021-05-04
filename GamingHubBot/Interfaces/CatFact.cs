using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GamingHubBot.Infrastructure.Interfaces
{
    public interface CatFact
    {
        Task<CatFact> CatFact { get; set; }
    }
}
