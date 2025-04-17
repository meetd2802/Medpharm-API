using Medpharm.Common;
using Medpharm.BusinessModels.Models;

namespace Medpharm.Services.IService
{
    public interface IAppointmentService
    {
        BaseResponse<Appointment> GetAllAppointments();
        BaseResponse<Appointment> CreateAppointment(Appointment appointment);
        BaseResponse<Appointment> GetAppointmentById(int id);
        BaseResponse<Appointment> UpdateAppointment(Appointment appointment);
        BaseResponse<bool> DeleteAppointment(int id);
        BaseResponse<Appointment> GetAppointmentsByPhone(string phone);
    }
}
