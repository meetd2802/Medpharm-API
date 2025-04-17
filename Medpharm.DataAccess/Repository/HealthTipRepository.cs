using Medpharm.DataAccess.DBConnection;
using Medpharm.Models;
using System.Collections.Generic;
using System.Linq;

namespace Medpharm.DataAccess.Repository
{
    public class HealthTipRepository : IHealthTipRepository
    {
        public List<HealthTip> GetAllHealthTips()
        {
            using (var context = new DBConnectionFactory())
            {
                return context.health_tips.Select(x => new HealthTip
                {
                    Id = x.Id,
                    Category = x.Category,
                    Title = x.Title,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    Author = x.Author,
                    DatePosted = x.DatePosted,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).ToList();
            }
        }

        public HealthTip GetHealthTipById(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                return context.health_tips
                    .Where(ht => ht.Id == id)
                    .Select(x => new HealthTip
                    {
                        Id = x.Id,
                        Category = x.Category,
                        Title = x.Title,
                        Description = x.Description,
                        ImageUrl = x.ImageUrl,
                        Author = x.Author,
                        DatePosted = x.DatePosted,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    })
                    .FirstOrDefault();
            }
        }

        public bool CreateHealthTip(HealthTip healthTip)
        {
            using (var context = new DBConnectionFactory())
            {
                var newHealthTip = new DBConnectionFactory.HealthTip
                {
                    Category = healthTip.Category,
                    Title = healthTip.Title,
                    Description = healthTip.Description,
                    ImageUrl = healthTip.ImageUrl,
                    Author = healthTip.Author,
                    DatePosted = healthTip.DatePosted,
                    CreatedAt = healthTip.CreatedAt,
                    UpdatedAt = healthTip.UpdatedAt
                };

                context.health_tips.Add(newHealthTip);
                return context.SaveChanges() > 0;
            }
        }

        public bool UpdateHealthTip(HealthTip healthTip)
        {
            using (var context = new DBConnectionFactory())
            {
                var existingHealthTip = context.health_tips.FirstOrDefault(ht => ht.Id == healthTip.Id);
                if (existingHealthTip == null)
                {
                    return false; // HealthTip not found
                }

                // Update fields
                existingHealthTip.Category = healthTip.Category;
                existingHealthTip.Title = healthTip.Title;
                existingHealthTip.Description = healthTip.Description;
                existingHealthTip.ImageUrl = healthTip.ImageUrl;
                existingHealthTip.Author = healthTip.Author;
                existingHealthTip.DatePosted = healthTip.DatePosted;
                existingHealthTip.UpdatedAt = healthTip.UpdatedAt;

                context.SaveChanges();
                return true; // Update successful
            }
        }

        public bool DeleteHealthTip(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                var healthTip = context.health_tips.FirstOrDefault(ht => ht.Id == id);
                if (healthTip == null)
                {
                    return false; // HealthTip not found
                }

                context.health_tips.Remove(healthTip);
                return context.SaveChanges() > 0;
            }
        }
    }
}
