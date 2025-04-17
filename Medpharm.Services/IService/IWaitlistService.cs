using Medpharm.Common;
using Medpharm.Models;

namespace Medpharm.Services.IService
{
    public interface IWaitlistService
    {
        BaseResponse<Waitlist> GetAllWaitlist();
        BaseResponse<Waitlist> CreateWaitlist(Waitlist waitlist);
        BaseResponse<Waitlist> GetWaitlistById(int id);
        BaseResponse<Waitlist> UpdateWaitlist(Waitlist waitlist);
        BaseResponse<bool> DeleteWaitlist(int id);
    }
}