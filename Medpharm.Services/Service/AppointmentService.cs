using Medpharm.BusinessModels.Models;
using Medpharm.Common;
using Medpharm.DataAccess;
using Medpharm.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Medpharm.Services.Service
{
    public class AppointmentService : IAppointmentService
    {
        readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public BaseResponse<Appointment> GetAllAppointments()
        {
            var results = new List<Appointment>();
            BaseResponse<Appointment> response;
            try
            {
                results = _appointmentRepository.GetAllAppointment();
                response = new BaseResponse<Appointment>((int)StatusEnum.Success, null, results, 0, string.Empty, null);
            }
            catch (Exception ex)
            {
                response = new BaseResponse<Appointment>((int)StatusEnum.Exception, null, results, 0, ex.Message, ex);
            }
            return response;
        }

        public BaseResponse<Appointment> CreateAppointment(Appointment appointment)
        {
            BaseResponse<Appointment> response;
            try
            {
                bool isCreated = _appointmentRepository.CreateAppointment(appointment);
                if (isCreated)
                {
                    response = new BaseResponse<Appointment>((int)StatusEnum.Success, null, new List<Appointment> { appointment }, 0, "Appointment created successfully.", null);
                }
                else
                {
                    response = new BaseResponse<Appointment>((int)StatusEnum.Failure, null, null, 0, "Failed to create appointment.", null);
                }
            }
            catch (Exception ex)
            {
                response = new BaseResponse<Appointment>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
            return response;
        }

        public BaseResponse<Appointment> GetAppointmentById(int id)
        {
            var result = _appointmentRepository.GetAppointmentById(id);
            if (result == null)
            {
                return new BaseResponse<Appointment>(
                    (int)StatusEnum.Failure,
                    null,
                    null,
                    0,
                    "Appointment not found",
                    null
                );
            }

            return new BaseResponse<Appointment>(
                (int)StatusEnum.Success,
                null,
                new List<Appointment> { result },
                1,
                "Appointment retrieved successfully",
                null
            );
        }


        public BaseResponse<Appointment> UpdateAppointment(Appointment appointment)
        {
            try
            {
                bool isUpdated = _appointmentRepository.UpdateAppointment(appointment);
                if (!isUpdated)
                {
                    return new BaseResponse<Appointment>((int)StatusEnum.NotFound, null, null, 0, "Appointment not found", null);
                }

                return new BaseResponse<Appointment>((int)StatusEnum.Success, null, null, 0, "Appointment updated successfully", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<Appointment>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
        }

        public BaseResponse<bool> DeleteAppointment(int id)
        {
            try
            {
                bool isDeleted = _appointmentRepository.DeleteAppointment(id);

                if (isDeleted)
                {
                    return new BaseResponse<bool>(
                        (int)StatusEnum.Success,
                        true,
                        new List<bool> { true },
                        1,
                        "Appointment deleted successfully",
                        null
                    );
                }
                else
                {
                    return new BaseResponse<bool>(
                        (int)StatusEnum.Failure,
                        false,
                        new List<bool> { false },
                        0,
                        "Appointment not found",
                        null
                    );
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>(
                    (int)StatusEnum.Exception,
                    false,
                    new List<bool> { false },
                    0,
                    ex.Message,
                    ex
                );
            }
        }

        public BaseResponse<Appointment> GetAppointmentsByPhone(string phone)
        {
            try
            {
                var appointments = _appointmentRepository.GetAppointmentsByPhone(phone);
                if (appointments == null || !appointments.Any())
                {
                    return new BaseResponse<Appointment>(
                        (int)StatusEnum.Failure,
                        null,
                        new List<Appointment>(),
                        0,
                        "No appointments found for this phone number",
                        null
                    );
                }

                return new BaseResponse<Appointment>(
                    (int)StatusEnum.Success,
                    null,
                    appointments,
                    appointments.Count,
                    "Appointments retrieved successfully",
                    null
                );
            }
            catch (Exception ex)
            {
                return new BaseResponse<Appointment>(
                    (int)StatusEnum.Exception,
                    null,
                    new List<Appointment>(),
                    0,
                    ex.Message,
                    ex
                );
            }
        }
    }
}
