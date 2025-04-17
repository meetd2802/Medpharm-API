using Medpharm.Common;
using Medpharm.DataAccess;
using Medpharm.Models;
using Medpharm.Services.IService;
using System;
using System.Collections.Generic;

namespace Medpharm.Services.Service
{
    public class ContactFormService : IContactFormService
    {
        private readonly IContactFormRepository _contactFormRepository;

        public ContactFormService(IContactFormRepository contactFormRepository)
        {
            _contactFormRepository = contactFormRepository;
        }

        public BaseResponse<ContactForm> GetAllContactForms()
        {
            var results = new List<ContactForm>();
            try
            {
                results = _contactFormRepository.GetAllContactForms();
                return new BaseResponse<ContactForm>((int)StatusEnum.Success, null, results, results.Count, "Contact forms retrieved successfully.", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<ContactForm>((int)StatusEnum.Exception, null, results, 0, ex.Message, ex);
            }
        }

        public BaseResponse<ContactForm> CreateContactForm(ContactForm contactForm)
        {
            try
            {
                bool isCreated = _contactFormRepository.CreateContactForm(contactForm);
                if (isCreated)
                {
                    return new BaseResponse<ContactForm>((int)StatusEnum.Success, null, new List<ContactForm> { contactForm }, 1, "Contact form entry created successfully.", null);
                }
                return new BaseResponse<ContactForm>((int)StatusEnum.Failure, null, null, 0, "Failed to create contact form entry.", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<ContactForm>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
        }

        public BaseResponse<ContactForm> GetContactFormById(int id)
        {
            var result = _contactFormRepository.GetContactFormById(id);
            if (result == null)
            {
                return new BaseResponse<ContactForm>((int)StatusEnum.Failure, null, null, 0, "Contact form entry not found", null);
            }

            return new BaseResponse<ContactForm>((int)StatusEnum.Success, null, new List<ContactForm> { result }, 1, "Contact form entry retrieved successfully", null);
        }

        public BaseResponse<ContactForm> UpdateContactForm(ContactForm contactForm)
        {
            try
            {
                bool isUpdated = _contactFormRepository.UpdateContactForm(contactForm);
                if (!isUpdated)
                {
                    return new BaseResponse<ContactForm>((int)StatusEnum.NotFound, null, null, 0, "Contact form entry not found", null);
                }

                return new BaseResponse<ContactForm>((int)StatusEnum.Success, null, null, 0, "Contact form entry updated successfully", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<ContactForm>((int)StatusEnum.Exception, null, null, 0, ex.Message, ex);
            }
        }

        public BaseResponse<bool> DeleteContactForm(int id)
        {
            try
            {
                bool isDeleted = _contactFormRepository.DeleteContactForm(id);

                if (isDeleted)
                {
                    return new BaseResponse<bool>((int)StatusEnum.Success, true, new List<bool> { true }, 1, "Contact form entry deleted successfully", null);
                }
                return new BaseResponse<bool>((int)StatusEnum.Failure, false, new List<bool> { false }, 0, "Contact form entry not found", null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>((int)StatusEnum.Exception, false, new List<bool> { false }, 0, ex.Message, ex);
            }
        }
    }
}
