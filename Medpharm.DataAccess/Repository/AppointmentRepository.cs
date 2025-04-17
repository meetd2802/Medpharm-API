using Medpharm.DataAccess.DBConnection;
using Medpharm.BusinessModels.Models;
using System.Collections.Generic;
using System.Linq;

namespace Medpharm.DataAccess.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        public List<Appointment> GetAllAppointment()
        {
            using (var context = new DBConnectionFactory())
            {
                return context.appointment.Select(x => new Appointment
                {
                    AppointmentId = x.AppointmentId,
                    AppointmentTime = x.AppointmentTime,
                    Diseases = x.Diseases,
                    Doctor = x.Doctor,
                    History = x.History,
                    LabReport = x.LabReport,
                    Medicine = x.Medicine,
                    Name = x.Name,
                    Phone = x.Phone,
                    PaymentId = x.PaymentId,
                    OrderId = x.OrderId,
                    Signature = x.Signature,
                    Status = x.Status
                }).ToList();
            }
        }

        public Appointment GetAppointmentById(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                return context.appointment
                    .Where(a => a.AppointmentId == id)
                    .Select(x => new Appointment
                    {
                        AppointmentId = x.AppointmentId,
                        AppointmentTime = x.AppointmentTime,
                        Diseases = x.Diseases,
                        Doctor = x.Doctor,
                        History = x.History,
                        LabReport = x.LabReport,
                        Medicine = x.Medicine,
                        Name = x.Name,
                        Phone = x.Phone,
                        PaymentId = x.PaymentId,
                        OrderId = x.OrderId,
                        Signature = x.Signature,
                        Status = x.Status
                    })
                    .FirstOrDefault();
            }
        }


        public bool CreateAppointment(Appointment appointment)
        {
            using (var context = new DBConnectionFactory())
            {
                var newAppointment = new DBConnectionFactory.Appointment
                {
                    AppointmentTime = appointment.AppointmentTime,
                    Diseases = appointment.Diseases,
                    Doctor = appointment.Doctor,
                    History = appointment.History,
                    LabReport = appointment.LabReport,
                    Medicine = appointment.Medicine,
                    Name = appointment.Name,
                    Phone = appointment.Phone,
                    PaymentId =appointment.PaymentId,
                    OrderId = appointment.OrderId,
                    Signature = appointment.Signature,
                    Status = appointment.Status
                };

                context.appointment.Add(newAppointment);
                return context.SaveChanges() > 0;
            }
        }

     

        public bool UpdateAppointment(Appointment appointment)
        {
            using (var context = new DBConnectionFactory())
            {
                var existingAppointment = context.appointment.FirstOrDefault(a => a.AppointmentId == appointment.AppointmentId);
                if (existingAppointment == null)
                {
                    return false; // Return false if no appointment found
                }

                // Update fields
                existingAppointment.Name = appointment.Name;
                existingAppointment.Phone = appointment.Phone;
                existingAppointment.Diseases = appointment.Diseases;
                existingAppointment.History = appointment.History;
                existingAppointment.Medicine = appointment.Medicine;
                existingAppointment.LabReport = appointment.LabReport;
                existingAppointment.Doctor = appointment.Doctor;
                existingAppointment.AppointmentTime = appointment.AppointmentTime;
                existingAppointment.PaymentId = appointment.PaymentId;
                existingAppointment.OrderId = appointment.OrderId;
                existingAppointment.Signature = appointment.Signature;
                existingAppointment.Status = appointment.Status;

                context.SaveChanges();
                return true; // Return true if update is successful
            }
        }

        public bool DeleteAppointment(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                var appointment = context.appointment.FirstOrDefault(a => a.AppointmentId == id);
                if (appointment == null)
                {
                    return false;
                }

                context.appointment.Remove(appointment);
                return context.SaveChanges() > 0;
            }
        }

        public List<Appointment> GetAppointmentsByPhone(string phone)
        {
            using (var context = new DBConnectionFactory())
            {
                return context.appointment
                    .Where(a => a.Phone == phone)
                    .Select(x => new Appointment
                    {
                        AppointmentId = x.AppointmentId,
                        AppointmentTime = x.AppointmentTime,
                        Diseases = x.Diseases,
                        Doctor = x.Doctor,
                        History = x.History,
                        LabReport = x.LabReport,
                        Medicine = x.Medicine,
                        Name = x.Name,
                        Phone = x.Phone,
                        PaymentId = x.PaymentId,
                        OrderId = x.OrderId,
                        Signature = x.Signature,
                        Status = x.Status
                    })
                    .ToList();
            }
        }
    }
}
