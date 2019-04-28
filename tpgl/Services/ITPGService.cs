using System;
using System.Threading.Tasks;
using tpgl.Models;

namespace tpgl.Services
{
    public interface ITPGService
    {
        Task<StopsResponse> GetStops();
        Task<GetNextDeparturesResponse> GetNextDepartures(Stop stop);
    }
}
