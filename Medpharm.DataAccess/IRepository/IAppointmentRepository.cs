using Medpharm.BusinessModels.Models;
using Medpharm.Common;
using System.Collections.Generic;

namespace Medpharm.DataAccess
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetAllAppointment();
        bool CreateAppointment(Appointment appointment);
        bool UpdateAppointment(Appointment appointment); // ✅ Ensure this returns bool
        Appointment GetAppointmentById(int id);
        bool DeleteAppointment(int id);
        List<Appointment> GetAppointmentsByPhone(string phone);
    }

}
