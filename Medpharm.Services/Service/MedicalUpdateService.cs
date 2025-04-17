using Medpharm.BusinessModels.Models;
using Medpharm.Common;
using Medpharm.DataAccess;
using Medpharm.Services.IService;
using System;
using System.Collections.Generic;

namespace Medpharm.Services.Service
{
    public class MedicalUpdateService : IMedicalUpdateService
    {
        readonly IMedicalUpdateRepository _medicalUpdateRepository;

        public MedicalUpdateService(IMedicalUpdateRepository medicalUpdateRepository)
        {
            _medicalUpdateRepository = medicalUpdateRepository;
        }

        public BaseResponse<MedicalUpdate> GetAllMedicalUpdates()
        {
            var results = new List<MedicalUpdate>();
            BaseResponse<MedicalUpdate> response;
            try
            {
                results = _medicalUpdateRepository.GetAllMedicalUpdates();
                response = new BaseResponse<MedicalUpdate>((int)StatusEnum.Success, null, results, results.Count, "Medical updates retrieved successfully", null);
            }
            catch (Exception ex)
            {
                response = new BaseResponse<MedicalUpdate>((int)StatusEnum.Exception, null, results, 0, ex.Message, ex);
            }
            return response;
        }

        public BaseResponse<MedicalUpdate> CreateMedicalUpdate(MedicalUpdate medicalUpdate)
        {
            BaseResponse<MedicalUpdate> response;
            try
            {
                bool isCreated = _medicalUpdateRepository.CreateMedicalUpdate(medicalUpdate);
                if (isCreated)
                {
                    response = new BaseResponse<MedicalUpdate>((int)StatusEnum.Success, null, new List<MedicalUpdate> { medicalUpdate }, 1, "Medical update created successfully.", null);
                }
                else
                {
                    response = new BaseResponse<MedicalUpdate>((int)StatusEnum.Failure, null, null, 0, "Failed to create medical update.", null);
                }
            }
            catch (Exception ex)
            {
                response = new BaseResponse<MedicalUpdate>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
            return response;
        }

        public BaseResponse<MedicalUpdate> GetMedicalUpdateById(int id)
        {
            var result = _medicalUpdateRepository.GetMedicalUpdateById(id);
            if (result == null)
            {
                return new BaseResponse<MedicalUpdate>((int)StatusEnum.Failure, null, null, 0, "Medical update not found", null);
            }

            return new BaseResponse<MedicalUpdate>((int)StatusEnum.Success, null, new List<MedicalUpdate> { result }, 1, "Medical update retrieved successfully", null);
        }

        public BaseResponse<MedicalUpdate> UpdateMedicalUpdate(MedicalUpdate medicalUpdate)
        {
            try
            {
                bool isUpdated = _medicalUpdateRepository.UpdateMedicalUpdate(medicalUpdate);
                if (!isUpdated)
                {
                    return new BaseResponse<MedicalUpdate>((int)StatusEnum.NotFound, null, null, 0, "Medical update not found", null);
                }

                return new BaseResponse<MedicalUpdate>((int)StatusEnum.Success, null, null, 0, "Medical update updated successfully", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<MedicalUpdate>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
        }

        public BaseResponse<bool> DeleteMedicalUpdate(int id)
        {
            try
            {
                bool isDeleted = _medicalUpdateRepository.DeleteMedicalUpdate(id);

                if (isDeleted)
                {
                    return new BaseResponse<bool>((int)StatusEnum.Success, true, new List<bool> { true }, 1, "Medical update deleted successfully", null);
                }
                else
                {
                    return new BaseResponse<bool>((int)StatusEnum.Failure, false, new List<bool> { false }, 0, "Medical update not found", null);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>((int)StatusEnum.Exception, false, new List<bool> { false }, 0, ex.Message, ex);
            }
        }
    }
}
