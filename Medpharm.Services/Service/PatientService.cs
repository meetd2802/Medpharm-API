using Medpharm.Common;
using Medpharm.DataAccess;
using Medpharm.Models;
using Medpharm.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Medpharm.Services.Service
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public BaseResponse<Patient> GetAllPatients()
        {
            var results = new List<Patient>();
            try
            {
                results = _patientRepository.GetAllPatients();
                return new BaseResponse<Patient>((int)StatusEnum.Success, null, results, results.Count, "Patients retrieved successfully.", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<Patient>((int)StatusEnum.Exception, null, results, 0, ex.Message, ex);
            }
        }

        public BaseResponse<Patient> CreatePatient(Patient patient)
        {
            try
            {
                bool isCreated = _patientRepository.CreatePatient(patient);
                if (isCreated)
                {
                    return new BaseResponse<Patient>((int)StatusEnum.Success, null, new List<Patient> { patient }, 1, "Patient created successfully.", null);
                }
                return new BaseResponse<Patient>((int)StatusEnum.Failure, null, null, 0, "Failed to create patient.", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<Patient>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
        }

        public BaseResponse<Patient> GetPatientById(int id)
        {
            var result = _patientRepository.GetPatientById(id);
            if (result == null)
            {
                return new BaseResponse<Patient>((int)StatusEnum.Failure, null, null, 0, "Patient not found", null);
            }

            return new BaseResponse<Patient>((int)StatusEnum.Success, null, new List<Patient> { result }, 1, "Patient retrieved successfully", null);
        }

        public BaseResponse<Patient> UpdatePatient(Patient patient)
        {
            try
            {
                bool isUpdated = _patientRepository.UpdatePatient(patient);
                if (!isUpdated)
                {
                    return new BaseResponse<Patient>((int)StatusEnum.NotFound, null, null, 0, "Patient not found", null);
                }

                return new BaseResponse<Patient>((int)StatusEnum.Success, null, null, 0, "Patient updated successfully", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<Patient>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
        }

        public BaseResponse<bool> DeletePatient(int id)
        {
            try
            {
                bool isDeleted = _patientRepository.DeletePatient(id);

                if (isDeleted)
                {
                    return new BaseResponse<bool>((int)StatusEnum.Success, true, new List<bool> { true }, 1, "Patient deleted successfully", null);
                }
                return new BaseResponse<bool>((int)StatusEnum.Failure, false, new List<bool> { false }, 0, "Patient not found", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>((int)StatusEnum.Exception, false, new List<bool> { false }, 0, ex.Message, ex);
            }
        }

        public BaseResponse<Patient> SearchPatients(string keyword, string gender)
        {
            try
            {
                var results = _patientRepository.GetAllPatients()
                    .Where(p => 
                        (string.IsNullOrEmpty(keyword) || 
                         p.Name.ToLower().Contains(keyword.ToLower()) ||
                         p.Phone.ToLower().Contains(keyword.ToLower())) &&
                        (string.IsNullOrEmpty(gender) || 
                         p.Gender.ToLower() == gender.ToLower()))
                    .ToList();

                return new BaseResponse<Patient>(
                    (int)StatusEnum.Success,
                    null,
                    results,
                    results.Count,
                    "Patients retrieved successfully",
                    null
                );
            }
            catch (Exception ex)
            {
                return new BaseResponse<Patient>(
                    (int)StatusEnum.Exception,
                    null,
                    new List<Patient>(),
                    0,
                    ex.Message,
                    ex
                );
            }
        }
    }
}
