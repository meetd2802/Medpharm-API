using Medpharm.BusinessModels.Models;
using Medpharm.Common;
using Medpharm.DataAccess;
using Medpharm.DataAccess.DBConnection;
using Medpharm.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Medpharm.Services.Service
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly DBConnectionFactory _context;

        public AdminService(IAdminRepository adminRepository, DBConnectionFactory context)
        {
            _adminRepository = adminRepository;
            _context = context;
        }

        public BaseResponse<Admin> GetAllAdmins()
        {
            try
            {
                var admins = _adminRepository.GetAllAdmins();
                return new BaseResponse<Admin>((int)StatusEnum.Success, null, admins, admins.Count, "Admins retrieved successfully", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<Admin>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
        }

        public BaseResponse<Admin> CreateAdmin(Admin admin)
        {
            try
            {
                bool isCreated = _adminRepository.CreateAdmin(admin);
                if (isCreated)
                {
                    return new BaseResponse<Admin>((int)StatusEnum.Success, null, new List<Admin> { admin }, 1, "Admin created successfully.", null);
                }
                return new BaseResponse<Admin>((int)StatusEnum.Failure, null, null, 0, "Failed to create admin.", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<Admin>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
        }

        public BaseResponse<Admin> GetAdminById(int id)
        {
            try
            {
                var admin = _adminRepository.GetAdminById(id);
                if (admin == null)
                    return new BaseResponse<Admin>((int)StatusEnum.Failure, null, null, 0, "Admin not found.", null);

                return new BaseResponse<Admin>((int)StatusEnum.Success, null, new List<Admin> { admin }, 1, "Admin retrieved successfully.", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<Admin>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
        }

        public BaseResponse<Admin> UpdateAdmin(Admin admin)
        {
            try
            {
                bool isUpdated = _adminRepository.UpdateAdmin(admin);
                if (!isUpdated)
                    return new BaseResponse<Admin>((int)StatusEnum.Failure, null, null, 0, "Admin not found.", null);

                return new BaseResponse<Admin>((int)StatusEnum.Success, null, null, 0, "Admin updated successfully.", null);
            }
            catch (Exception ex)
            {
                var customException = new Exception(ex.Message);
                return new BaseResponse<Admin>((int)StatusEnum.Exception, null, null, 0, "An error occurred.", customException);
            }
        }

        public BaseResponse<bool> DeleteAdmin(int id)
        {
            try
            {
                bool isDeleted = _adminRepository.DeleteAdmin(id);
                if (!isDeleted)
                    return new BaseResponse<bool>((int)StatusEnum.Failure, false, null, 0, "Admin not found.", null);

                return new BaseResponse<bool>((int)StatusEnum.Success, true, new List<bool> { true }, 1, "Admin deleted successfully.", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>((int)StatusEnum.Exception, false, null, 0, ex.Message, ex);
            }
        }

        public Admin GetAdminByUsername(string username)
        {
            // Fetching the admin from the database (Data Model)
            var dbAdmin = _context.admins.FirstOrDefault(a => a.UserName == username);

            // If no admin found, return null
            if (dbAdmin == null) return null;

            // Mapping to Business Model
            return new Admin
            {
                Id = dbAdmin.Id,
                UserName = dbAdmin.UserName,
                Password = dbAdmin.Password,
                FullName = dbAdmin.FullName,
                Email = dbAdmin.Email,
                Role = dbAdmin.Role,
                Phone = dbAdmin.Phone
            };
        }
        
        public Admin AuthenticateAdmin(string username, string password)
        {
            // Query the database using DBConnectionFactory
            var dbAdmin = _context.admins.FirstOrDefault(a => a.UserName == username && a.Password == password);

            if (dbAdmin == null)
                return null;

            // Return mapped Admin object
            return new Admin
            {
                Id = dbAdmin.Id,
                UserName = dbAdmin.UserName,
                FullName = dbAdmin.FullName,
                Email = dbAdmin.Email,
                Role = dbAdmin.Role,
                Phone = dbAdmin.Phone
            };
        }

    }
}
