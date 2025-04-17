using Medpharm.Common;
using Medpharm.DataAccess;
using Medpharm.Models;
using Medpharm.Services.IService;
using System;
using System.Collections.Generic;

namespace Medpharm.Services.Service
{
    public class WaitlistService : IWaitlistService
    {
        private readonly IWaitlistRepository _waitlistRepository;

        public WaitlistService(IWaitlistRepository waitlistRepository)
        {
            _waitlistRepository = waitlistRepository;
        }

        public BaseResponse<Waitlist> GetAllWaitlist()
        {
            var results = new List<Waitlist>();
            try
            {
                results = _waitlistRepository.GetAllWaitlist();
                return new BaseResponse<Waitlist>((int)StatusEnum.Success, null, results, results.Count, "Waitlist retrieved successfully.", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<Waitlist>((int)StatusEnum.Exception, null, results, 0, ex.Message, ex);
            }
        }

        public BaseResponse<Waitlist> CreateWaitlist(Waitlist waitlist)
        {
            try
            {
                bool isCreated = _waitlistRepository.CreateWaitlist(waitlist);
                if (isCreated)
                {
                    return new BaseResponse<Waitlist>((int)StatusEnum.Success, null, new List<Waitlist> { waitlist }, 1, "Waitlist entry created successfully.", null);
                }
                return new BaseResponse<Waitlist>((int)StatusEnum.Failure, null, null, 0, "Failed to create waitlist entry.", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<Waitlist>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
        }

        public BaseResponse<Waitlist> GetWaitlistById(int id)
        {
            var result = _waitlistRepository.GetWaitlistById(id);
            if (result == null)
            {
                return new BaseResponse<Waitlist>((int)StatusEnum.Failure, null, null, 0, "Waitlist entry not found", null);
            }

            return new BaseResponse<Waitlist>((int)StatusEnum.Success, null, new List<Waitlist> { result }, 1, "Waitlist entry retrieved successfully", null);
        }

        public BaseResponse<Waitlist> UpdateWaitlist(Waitlist waitlist)
        {
            try
            {
                bool isUpdated = _waitlistRepository.UpdateWaitlist(waitlist);
                if (!isUpdated)
                {
                    return new BaseResponse<Waitlist>((int)StatusEnum.NotFound, null, null, 0, "Waitlist entry not found", null);
                }

                return new BaseResponse<Waitlist>((int)StatusEnum.Success, null, null, 0, "Waitlist entry updated successfully", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<Waitlist>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
        }

        public BaseResponse<bool> DeleteWaitlist(int id)
        {
            try
            {
                bool isDeleted = _waitlistRepository.DeleteWaitlist(id);

                if (isDeleted)
                {
                    return new BaseResponse<bool>((int)StatusEnum.Success, true, new List<bool> { true }, 1, "Waitlist entry deleted successfully", null);
                }
                return new BaseResponse<bool>((int)StatusEnum.Failure, false, new List<bool> { false }, 0, "Waitlist entry not found", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>((int)StatusEnum.Exception, false, new List<bool> { false }, 0, ex.Message, ex);
            }
        }
    }
}
