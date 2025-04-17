using Medpharm.DataAccess.DBConnection;
using Medpharm.BusinessModels.Models;
using System.Collections.Generic;
using System.Linq;

namespace Medpharm.DataAccess.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        public List<Service> GetAllServices()
        {
            using (var context = new DBConnectionFactory())
            {
                return context.services.Select(x => new Service
                {
                    ServiceID = x.ServiceID,
                    ServiceName = x.ServiceName,
                    Description = x.Description,
                    ImagePath = x.ImagePath
                }).ToList();
            }
        }

        public Service GetServiceById(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                return context.services
                    .Where(s => s.ServiceID == id)
                    .Select(x => new Service
                    {
                        ServiceID = x.ServiceID,
                        ServiceName = x.ServiceName,
                        Description = x.Description,
                        ImagePath = x.ImagePath
                    })
                    .FirstOrDefault();
            }
        }

        public bool CreateService(Service service)
        {
            using (var context = new DBConnectionFactory())
            {
                var newService = new DBConnectionFactory.Service
                {
                    ServiceName = service.ServiceName,
                    Description = service.Description,
                    ImagePath = service.ImagePath
                };

                context.services.Add(newService);
                return context.SaveChanges() > 0;
            }
        }

        public bool UpdateService(Service service)
        {
            using (var context = new DBConnectionFactory())
            {
                var existingService = context.services.FirstOrDefault(s => s.ServiceID == service.ServiceID);
                if (existingService == null)
                {
                    return false; // Service not found
                }

                // Update fields
                existingService.ServiceName = service.ServiceName;
                existingService.Description = service.Description;
                existingService.ImagePath = service.ImagePath;

                context.SaveChanges();
                return true;
            }
        }

        public bool DeleteService(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                var service = context.services.FirstOrDefault(s => s.ServiceID == id);
                if (service == null)
                {
                    return false;
                }

                context.services.Remove(service);
                return context.SaveChanges() > 0;
            }
        }
    }
}
