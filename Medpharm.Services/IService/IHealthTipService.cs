using Medpharm.Common;
using Medpharm.Models;

namespace Medpharm.Services.IService
{
    public interface IHealthTipService
    {
        // Method to get all health tips
        BaseResponse<HealthTip> GetAllHealthTips();

        // Method to create a new health tip
        BaseResponse<HealthTip> CreateHealthTip(HealthTip healthTip);

        // Method to get a health tip by its ID
        BaseResponse<HealthTip> GetHealthTipById(int id);

        // Method to update an existing health tip
        BaseResponse<HealthTip> UpdateHealthTip(HealthTip healthTip);

        // Method to delete a health tip by its ID
        BaseResponse<bool> DeleteHealthTip(int id);
    }
}