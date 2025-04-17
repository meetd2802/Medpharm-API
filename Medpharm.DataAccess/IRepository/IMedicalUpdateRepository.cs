using Medpharm.BusinessModels.Models;
using System.Collections.Generic;

namespace Medpharm.DataAccess
{
    public interface IMedicalUpdateRepository
    {
        List<MedicalUpdate> GetAllMedicalUpdates();
        bool CreateMedicalUpdate(MedicalUpdate medicalUpdate);
        bool UpdateMedicalUpdate(MedicalUpdate medicalUpdate);
        MedicalUpdate GetMedicalUpdateById(int id);
        bool DeleteMedicalUpdate(int id);
    }
}