using Medpharm.DataAccess.DBConnection;
using Medpharm.BusinessModels.Models;
using System.Collections.Generic;
using System.Linq;

namespace Medpharm.DataAccess.Repository
{
    public class MedicalUpdateRepository : IMedicalUpdateRepository
    {
        public List<MedicalUpdate> GetAllMedicalUpdates()
        {
            using (var context = new DBConnectionFactory())
            {
                return context.medicalUpdates.Select(x => new MedicalUpdate
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ImagePath = x.ImagePath
                }).ToList();
            }
        }

        public MedicalUpdate GetMedicalUpdateById(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                return context.medicalUpdates
                    .Where(mu => mu.Id == id)
                    .Select(x => new MedicalUpdate
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        ImagePath = x.ImagePath
                    })
                    .FirstOrDefault();
            }
        }

        public bool CreateMedicalUpdate(MedicalUpdate medicalUpdate)
        {
            using (var context = new DBConnectionFactory())
            {
                var newMedicalUpdate = new DBConnectionFactory.MedicalUpdate
                {
                    Name = medicalUpdate.Name,
                    Description = medicalUpdate.Description,
                    ImagePath = medicalUpdate.ImagePath
                };

                context.medicalUpdates.Add(newMedicalUpdate);
                return context.SaveChanges() > 0;
            }
        }

        public bool UpdateMedicalUpdate(MedicalUpdate medicalUpdate)
        {
            using (var context = new DBConnectionFactory())
            {
                var existingMedicalUpdate = context.medicalUpdates.FirstOrDefault(mu => mu.Id == medicalUpdate.Id);
                if (existingMedicalUpdate == null)
                {
                    return false; // Not found
                }

                // Update fields
                existingMedicalUpdate.Name = medicalUpdate.Name;
                existingMedicalUpdate.Description = medicalUpdate.Description;
                existingMedicalUpdate.ImagePath = medicalUpdate.ImagePath;

                context.SaveChanges();
                return true;
            }
        }

        public bool DeleteMedicalUpdate(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                var medicalUpdate = context.medicalUpdates.FirstOrDefault(mu => mu.Id == id);
                if (medicalUpdate == null)
                {
                    return false;
                }

                context.medicalUpdates.Remove(medicalUpdate);
                return context.SaveChanges() > 0;
            }
        }
    }
}
