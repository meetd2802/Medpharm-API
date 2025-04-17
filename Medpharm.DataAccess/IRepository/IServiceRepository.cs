using Medpharm.BusinessModels.Models;
using System.Collections.Generic;

namespace Medpharm.DataAccess.Repository
{
    public interface IServiceRepository
    {
        List<Service> GetAllServices();
        Service GetServiceById(int id);
        bool CreateService(Service service);
        bool UpdateService(Service service);
        bool DeleteService(int id);
    }
}