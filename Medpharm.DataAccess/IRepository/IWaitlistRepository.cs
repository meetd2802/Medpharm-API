using Medpharm.Models;
using System.Collections.Generic;

namespace Medpharm.DataAccess
{
    public interface IWaitlistRepository
    {
        List<Waitlist> GetAllWaitlist();
        bool CreateWaitlist(Waitlist waitlist);
        bool UpdateWaitlist(Waitlist waitlist);
        Waitlist GetWaitlistById(int id);
        bool DeleteWaitlist(int id);
    }
}