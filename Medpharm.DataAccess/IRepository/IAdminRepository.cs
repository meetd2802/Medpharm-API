using Medpharm.BusinessModels.Models;
using System.Collections.Generic;

namespace Medpharm.DataAccess
{
    public interface IAdminRepository
    {
        List<Admin> GetAllAdmins();
        bool CreateAdmin(Admin admin);
        bool UpdateAdmin(Admin admin);
        Admin GetAdminById(int id);
        bool DeleteAdmin(int id);
    }
}