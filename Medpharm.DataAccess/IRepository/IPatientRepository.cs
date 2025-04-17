using Medpharm.Models;
using System.Collections.Generic;

namespace Medpharm.DataAccess
{
    public interface IPatientRepository
    {
        List<Patient> GetAllPatients();
        bool CreatePatient(Patient patient);
        bool UpdatePatient(Patient patient);
        Patient GetPatientById(int id);
        bool DeletePatient(int id);
    }
}