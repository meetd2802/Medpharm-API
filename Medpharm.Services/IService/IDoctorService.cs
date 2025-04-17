using Medpharm.Common;
using Medpharm.BusinessModels.Models;

namespace Medpharm.Services.IService
{
    public interface IDoctorService
    {
        BaseResponse<Doctor> GetAllDoctors();
        BaseResponse<Doctor> CreateDoctor(Doctor doctor);
        BaseResponse<Doctor> GetDoctorById(int id);
        BaseResponse<Doctor> UpdateDoctor(Doctor doctor);
        BaseResponse<bool> DeleteDoctor(int id);
    }
}
