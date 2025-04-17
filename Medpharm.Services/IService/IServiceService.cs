using Medpharm.Common;
using Medpharm.DataAccess.DBConnection;
using Medpharm.BusinessModels.Models;
using ServiceModel = Medpharm.BusinessModels.Models.Service;


namespace Medpharm.Services.IService
{
    public interface IServiceService
    {
        BaseResponse<ServiceModel> GetAllServices();
        BaseResponse<ServiceModel> CreateService(ServiceModel service);
        BaseResponse<ServiceModel> GetServiceById(int id);
        BaseResponse<ServiceModel> UpdateService(ServiceModel service);
        BaseResponse<bool> DeleteService(int id);
    }
}