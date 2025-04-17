using Medpharm.BusinessModels.Models;
using Medpharm.Common;
using Medpharm.DataAccess;
using Medpharm.Services.IService;
using System;
using System.Collections.Generic;

namespace Medpharm.Services.Service
{
    public class DoctorService : IDoctorService
    {
        readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public BaseResponse<Doctor> GetAllDoctors()
        {
            var results = new List<Doctor>();
            BaseResponse<Doctor> response;
            try
            {
                results = _doctorRepository.GetAllDoctors();
                response = new BaseResponse<Doctor>((int)StatusEnum.Success, null, results, results.Count, "Doctors retrieved successfully", null);
            }
            catch (Exception ex)
            {
                response = new BaseResponse<Doctor>((int)StatusEnum.Exception, null, results, 0, ex.Message, ex);
            }
            return response;
        }

        public BaseResponse<Doctor> CreateDoctor(Doctor doctor)
        {
            BaseResponse<Doctor> response;
            try
            {
                bool isCreated = _doctorRepository.CreateDoctor(doctor);
                if (isCreated)
                {
                    response = new BaseResponse<Doctor>((int)StatusEnum.Success, null, new List<Doctor> { doctor }, 1, "Doctor created successfully.", null);
                }
                else
                {
                    response = new BaseResponse<Doctor>((int)StatusEnum.Failure, null, null, 0, "Failed to create doctor.", null);
                }
            }
            catch (Exception ex)
            {
                response = new BaseResponse<Doctor>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
            return response;
        }

        public BaseResponse<Doctor> GetDoctorById(int id)
        {
            var result = _doctorRepository.GetDoctorById(id);
            if (result == null)
            {
                return new BaseResponse<Doctor>((int)StatusEnum.Failure, null, null, 0, "Doctor not found", null);
            }

            return new BaseResponse<Doctor>((int)StatusEnum.Success, null, new List<Doctor> { result }, 1, "Doctor retrieved successfully", null);
        }

        public BaseResponse<Doctor> UpdateDoctor(Doctor doctor)
        {
            try
            {
                bool isUpdated = _doctorRepository.UpdateDoctor(doctor);
                if (!isUpdated)
                {
                    return new BaseResponse<Doctor>((int)StatusEnum.NotFound, null, null, 0, "Doctor not found", null);
                }

                return new BaseResponse<Doctor>((int)StatusEnum.Success, null, null, 0, "Doctor updated successfully", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<Doctor>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
        }

        public BaseResponse<bool> DeleteDoctor(int id)
        {
            try
            {
                bool isDeleted = _doctorRepository.DeleteDoctor(id);

                if (isDeleted)
                {
                    return new BaseResponse<bool>((int)StatusEnum.Success, true, new List<bool> { true }, 1, "Doctor deleted successfully", null);
                }
                else
                {
                    return new BaseResponse<bool>((int)StatusEnum.Failure, false, new List<bool> { false }, 0, "Doctor not found", null);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>((int)StatusEnum.Exception, false, new List<bool> { false }, 0, ex.Message, ex);
            }
        }
    }
}
