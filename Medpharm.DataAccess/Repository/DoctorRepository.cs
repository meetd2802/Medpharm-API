using Medpharm.DataAccess.DBConnection;
using Medpharm.BusinessModels.Models;
using System.Collections.Generic;
using System.Linq;

namespace Medpharm.DataAccess.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        public List<Doctor> GetAllDoctors()
        {
            using (var context = new DBConnectionFactory())
            {
                return context.doctors.Select(x => new Doctor
                {
                    Id = x.Id,
                    Name = x.Name,
                    Speciality = x.Speciality,
                    ImagePath = x.ImagePath
                }).ToList();
            }
        }

        public Doctor GetDoctorById(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                return context.doctors
                    .Where(d => d.Id == id)
                    .Select(x => new Doctor
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Speciality = x.Speciality,
                        ImagePath = x.ImagePath
                    })
                    .FirstOrDefault();
            }
        }

        public bool CreateDoctor(Doctor doctor)
        {
            using (var context = new DBConnectionFactory())
            {
                var newDoctor = new DBConnectionFactory.Doctor
                {
                    Name = doctor.Name,
                    Speciality = doctor.Speciality,
                    ImagePath = doctor.ImagePath
                };

                context.doctors.Add(newDoctor);
                return context.SaveChanges() > 0;
            }
        }

        public bool UpdateDoctor(Doctor doctor)
        {
            using (var context = new DBConnectionFactory())
            {
                var existingDoctor = context.doctors.FirstOrDefault(d => d.Id == doctor.Id);
                if (existingDoctor == null)
                {
                    return false; // Doctor not found
                }

                // Update fields
                existingDoctor.Name = doctor.Name;
                existingDoctor.Speciality = doctor.Speciality;
                existingDoctor.ImagePath = doctor.ImagePath;

                context.SaveChanges();
                return true;
            }
        }

        public bool DeleteDoctor(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                var doctor = context.doctors.FirstOrDefault(d => d.Id == id);
                if (doctor == null)
                {
                    return false;
                }

                context.doctors.Remove(doctor);
                return context.SaveChanges() > 0;
            }
        }
    }
}
