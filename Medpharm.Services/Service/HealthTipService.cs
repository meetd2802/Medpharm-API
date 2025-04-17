using Medpharm.Common;
using Medpharm.DataAccess;
using Medpharm.Models;
using Medpharm.Services.IService;
using System;
using System.Collections.Generic;

namespace Medpharm.Services.Service
{
    public class HealthTipService : IHealthTipService
    {
        private readonly IHealthTipRepository _healthTipRepository;

        public HealthTipService(IHealthTipRepository healthTipRepository)
        {
            _healthTipRepository = healthTipRepository;
        }

        public BaseResponse<HealthTip> GetAllHealthTips()
        {
            var results = new List<HealthTip>();
            try
            {
                results = _healthTipRepository.GetAllHealthTips();
                return new BaseResponse<HealthTip>((int)StatusEnum.Success, null, results, results.Count, "Health tips retrieved successfully.", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<HealthTip>((int)StatusEnum.Exception, null, results, 0, ex.Message, ex);
            }
        }

        public BaseResponse<HealthTip> CreateHealthTip(HealthTip healthTip)
        {
            try
            {
                bool isCreated = _healthTipRepository.CreateHealthTip(healthTip);
                if (isCreated)
                {
                    return new BaseResponse<HealthTip>((int)StatusEnum.Success, null, new List<HealthTip> { healthTip }, 1, "Health tip created successfully.", null);
                }
                return new BaseResponse<HealthTip>((int)StatusEnum.Failure, null, null, 0, "Failed to create health tip.", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<HealthTip>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
        }

        public BaseResponse<HealthTip> GetHealthTipById(int id)
        {
            var result = _healthTipRepository.GetHealthTipById(id);
            if (result == null)
            {
                return new BaseResponse<HealthTip>((int)StatusEnum.Failure, null, null, 0, "Health tip not found", null);
            }

            return new BaseResponse<HealthTip>((int)StatusEnum.Success, null, new List<HealthTip> { result }, 1, "Health tip retrieved successfully", null);
        }

        public BaseResponse<HealthTip> UpdateHealthTip(HealthTip healthTip)
        {
            try
            {
                bool isUpdated = _healthTipRepository.UpdateHealthTip(healthTip);
                if (!isUpdated)
                {
                    return new BaseResponse<HealthTip>((int)StatusEnum.NotFound, null, null, 0, "Health tip not found", null);
                }

                return new BaseResponse<HealthTip>((int)StatusEnum.Success, null, null, 0, "Health tip updated successfully", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<HealthTip>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
        }

        public BaseResponse<bool> DeleteHealthTip(int id)
        {
            try
            {
                bool isDeleted = _healthTipRepository.DeleteHealthTip(id);

                if (isDeleted)
                {
                    return new BaseResponse<bool>((int)StatusEnum.Success, true, new List<bool> { true }, 1, "Health tip deleted successfully", null);
                }
                return new BaseResponse<bool>((int)StatusEnum.Failure, false, new List<bool> { false }, 0, "Health tip not found", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>((int)StatusEnum.Exception, false, new List<bool> { false }, 0, ex.Message, ex);
            }
        }
    }
}
