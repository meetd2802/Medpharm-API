using Medpharm.DataAccess.DBConnection;
using Medpharm.Models;
using System.Collections.Generic;
using System.Linq;

namespace Medpharm.DataAccess.Repository
{
    public class WaitlistRepository : IWaitlistRepository
    {
        public List<Waitlist> GetAllWaitlist()
        {
            using (var context = new DBConnectionFactory())
            {
                return context.waitlists.Select(x => new Waitlist
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    PreferredDoctor = x.PreferredDoctor,
                    PreferredTimeframe = x.PreferredTimeframe,
                    CreatedAt = x.CreatedAt
                }).ToList();
            }
        }

        public Waitlist GetWaitlistById(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                return context.waitlists
                    .Where(w => w.Id == id)
                    .Select(x => new Waitlist
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        Email = x.Email,
                        PhoneNumber = x.PhoneNumber,
                        PreferredDoctor = x.PreferredDoctor,
                        PreferredTimeframe = x.PreferredTimeframe,
                        CreatedAt = x.CreatedAt
                    })
                    .FirstOrDefault();
            }
        }

        public bool CreateWaitlist(Waitlist waitlist)
        {
            using (var context = new DBConnectionFactory())
            {
                var newWaitlist = new DBConnectionFactory.Waitlist
                {
                    FullName = waitlist.FullName,
                    Email = waitlist.Email,
                    PhoneNumber = waitlist.PhoneNumber,
                    PreferredDoctor = waitlist.PreferredDoctor,
                    PreferredTimeframe = waitlist.PreferredTimeframe,
                    CreatedAt = waitlist.CreatedAt
                };

                context.waitlists.Add(newWaitlist);
                return context.SaveChanges() > 0;
            }
        }

        public bool UpdateWaitlist(Waitlist waitlist)
        {
            using (var context = new DBConnectionFactory())
            {
                var existingWaitlist = context.waitlists.FirstOrDefault(w => w.Id == waitlist.Id);
                if (existingWaitlist == null)
                {
                    return false; // Waitlist entry not found
                }

                // Update fields
                existingWaitlist.FullName = waitlist.FullName;
                existingWaitlist.Email = waitlist.Email;
                existingWaitlist.PhoneNumber = waitlist.PhoneNumber;
                existingWaitlist.PreferredDoctor = waitlist.PreferredDoctor;
                existingWaitlist.PreferredTimeframe = waitlist.PreferredTimeframe;

                context.SaveChanges();
                return true; // Update successful
            }
        }

        public bool DeleteWaitlist(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                var waitlistEntry = context.waitlists.FirstOrDefault(w => w.Id == id);
                if (waitlistEntry == null)
                {
                    return false;
                }

                context.waitlists.Remove(waitlistEntry);
                return context.SaveChanges() > 0;
            }
        }
    }
}
