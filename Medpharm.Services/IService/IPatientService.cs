using Medpharm.Common;
using Medpharm.Models;

namespace Medpharm.Services.IService
{
    public interface IPatientService
    {
        BaseResponse<Patient> GetAllPatients();
        BaseResponse<Patient> CreatePatient(Patient patient);
        BaseResponse<Patient> GetPatientById(int id);
        BaseResponse<Patient> UpdatePatient(Patient patient);
        BaseResponse<bool> DeletePatient(int id);
        BaseResponse<Patient> SearchPatients(string keyword, string gender);
    }
}