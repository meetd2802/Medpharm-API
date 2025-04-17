using System;
using Medpharm.DataAccess.DBConnection;
using Medpharm.BusinessModels.Models;
using System.Collections.Generic;
using System.Linq;

namespace Medpharm.DataAccess.Repository
{
    public class AdminRepository : IAdminRepository
    {
        public List<Admin> GetAllAdmins()
        {
            using (var context = new DBConnectionFactory())
            {
                return context.admins.Select(x => new Admin
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    UserName = x.UserName,
                    Email = x.Email,
                    Role = x.Role,
                    Phone = x.Phone,
                    Password = x.Password
                }).ToList();
            }
        }

        public Admin GetAdminById(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                return context.admins
                    .Where(a => a.Id == id)
                    .Select(x => new Admin
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        UserName = x.UserName,
                        Email = x.Email,
                        Role = x.Role,
                        Phone = x.Phone,
                        Password = x.Password
                    })
                    .FirstOrDefault();
            }
        }

        // Remove the duplicate CreateAdmin method
        public bool CreateAdmin(Admin admin)
        {
            using (var context = new DBConnectionFactory())
            {
                // Check if admin with the same username or email already exists
                if (context.admins.Any(a => a.UserName == admin.UserName || a.Email == admin.Email))
                {
                    throw new InvalidOperationException("An admin with the same username or email already exists.");
                }

                var newAdmin = new DBConnectionFactory.Admin
                {
                    FullName = admin.FullName,
                    UserName = admin.UserName,
                    Email = admin.Email,
                    Role = admin.Role,
                    Phone = admin.Phone,
                    Password = admin.Password
                };

                context.admins.Add(newAdmin);
                return context.SaveChanges() > 0;
            }
        }

        // Implement UpdateAdmin method
        public bool UpdateAdmin(Admin admin)
        {
            using (var context = new DBConnectionFactory())
            {
                var existingAdmin = context.admins.FirstOrDefault(a => a.Id == admin.Id);
                if (existingAdmin == null)
                {
                    return false; // Admin not found
                }

                // Update the admin fields
                existingAdmin.FullName = admin.FullName;
                existingAdmin.UserName = admin.UserName;
                existingAdmin.Email = admin.Email;
                existingAdmin.Role = admin.Role;
                existingAdmin.Phone = admin.Phone;
                existingAdmin.Password = admin.Password;

                context.SaveChanges();
                return true;
            }
        }

        public bool DeleteAdmin(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                var admin = context.admins.FirstOrDefault(a => a.Id == id);
                if (admin == null)
                {
                    return false;
                }

                context.admins.Remove(admin);
                return context.SaveChanges() > 0;
            }
        }
    }
}
