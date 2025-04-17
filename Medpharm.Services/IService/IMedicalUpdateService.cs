using Medpharm.Common;
using Medpharm.BusinessModels.Models;

namespace Medpharm.Services.IService
{
    public interface IMedicalUpdateService
    {
        BaseResponse<MedicalUpdate> GetAllMedicalUpdates();
        BaseResponse<MedicalUpdate> CreateMedicalUpdate(MedicalUpdate medicalUpdate);
        BaseResponse<MedicalUpdate> GetMedicalUpdateById(int id);
        BaseResponse<MedicalUpdate> UpdateMedicalUpdate(MedicalUpdate medicalUpdate);
        BaseResponse<bool> DeleteMedicalUpdate(int id);
    }
}