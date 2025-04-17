using Medpharm.Common;
using Medpharm.BusinessModels.Models;

namespace Medpharm.Services.IService
{
    public interface IAdminService
    {
        BaseResponse<Admin> GetAllAdmins();
        BaseResponse<Admin> CreateAdmin(Admin admin);
        BaseResponse<Admin> GetAdminById(int id);
        BaseResponse<Admin> UpdateAdmin(Admin admin);
        BaseResponse<bool> DeleteAdmin(int id);
        
        Admin GetAdminByUsername(string username);
        
        Admin AuthenticateAdmin(string username, string password);
    }
}