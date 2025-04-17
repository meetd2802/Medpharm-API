using Medpharm.BusinessModels.Models;
using System.Collections.Generic;

namespace Medpharm.DataAccess
{
    public interface IDoctorRepository
    {
        List<Doctor> GetAllDoctors();
        bool CreateDoctor(Doctor doctor);
        bool UpdateDoctor(Doctor doctor);
        Doctor GetDoctorById(int id);
        bool DeleteDoctor(int id);
    }
}
