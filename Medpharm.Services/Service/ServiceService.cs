using Medpharm.Common;
using Medpharm.DataAccess.DBConnection;
using ServiceModel = Medpharm.BusinessModels.Models.Service; // Alias added for Service
using Medpharm.DataAccess.Repository;
using System;
using System.Collections.Generic;
using Medpharm.Services.IService;

namespace Medpharm.Services.Service
{
    public class ServiceService : IServiceService
    {
        readonly IServiceRepository _serviceRepository;

        public ServiceService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public BaseResponse<ServiceModel> GetAllServices()
        {
            var results = new List<ServiceModel>();
            BaseResponse<ServiceModel> response;
            try
            {
                results = _serviceRepository.GetAllServices();
                response = new BaseResponse<ServiceModel>((int)StatusEnum.Success, null, results, results.Count, "Services retrieved successfully", null);
            }
            catch (Exception ex)
            {
                response = new BaseResponse<ServiceModel>((int)StatusEnum.Exception, null, results, 0, ex.Message, ex);
            }
            return response;
        }

        public BaseResponse<ServiceModel> CreateService(ServiceModel service)
        {
            BaseResponse<ServiceModel> response;
            try
            {
                bool isCreated = _serviceRepository.CreateService(service);
                if (isCreated)
                {
                    response = new BaseResponse<ServiceModel>((int)StatusEnum.Success, null, new List<ServiceModel> { service }, 1, "Service created successfully.", null);
                }
                else
                {
                    response = new BaseResponse<ServiceModel>((int)StatusEnum.Failure, null, null, 0, "Failed to create service.", null);
                }
            }
            catch (Exception ex)
            {
                response = new BaseResponse<ServiceModel>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
            return response;
        }

        public BaseResponse<ServiceModel> GetServiceById(int id)
        {
            var result = _serviceRepository.GetServiceById(id);
            if (result == null)
            {
                return new BaseResponse<ServiceModel>((int)StatusEnum.Failure, null, null, 0, "Service not found", null);
            }

            return new BaseResponse<ServiceModel>((int)StatusEnum.Success, null, new List<ServiceModel> { result }, 1, "Service retrieved successfully", null);
        }

        public BaseResponse<ServiceModel> UpdateService(ServiceModel service)
        {
            try
            {
                bool isUpdated = _serviceRepository.UpdateService(service);
                if (!isUpdated)
                {
                    return new BaseResponse<ServiceModel>((int)StatusEnum.NotFound, null, null, 0, "Service not found", null);
                }

                return new BaseResponse<ServiceModel>((int)StatusEnum.Success, null, null, 0, "Service updated successfully", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<ServiceModel>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
        }

        public BaseResponse<bool> DeleteService(int id)
        {
            try
            {
                bool isDeleted = _serviceRepository.DeleteService(id);

                if (isDeleted)
                {
                    return new BaseResponse<bool>((int)StatusEnum.Success, true, new List<bool> { true }, 1, "Service deleted successfully", null);
                }
                else
                {
                    return new BaseResponse<bool>((int)StatusEnum.Failure, false, new List<bool> { false }, 0, "Service not found", null);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>((int)StatusEnum.Exception, false, new List<bool> { false }, 0, ex.Message, ex);
            }
        }
    }
}
