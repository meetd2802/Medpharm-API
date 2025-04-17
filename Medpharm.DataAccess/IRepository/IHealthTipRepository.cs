using Medpharm.Models;
using System.Collections.Generic;

namespace Medpharm.DataAccess
{
    public interface IHealthTipRepository
    {
        // Get all health tips
        List<HealthTip> GetAllHealthTips();

        // Create a new health tip
        bool CreateHealthTip(HealthTip healthTip);

        // Update an existing health tip
        bool UpdateHealthTip(HealthTip healthTip);

        // Get a health tip by its ID
        HealthTip GetHealthTipById(int id);

        // Delete a health tip by its ID
        bool DeleteHealthTip(int id);
    }
}