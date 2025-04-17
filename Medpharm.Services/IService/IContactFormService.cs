using Medpharm.Common;
using Medpharm.Models;

namespace Medpharm.Services.IService
{
    public interface IContactFormService
    {
        BaseResponse<ContactForm> GetAllContactForms();
        BaseResponse<ContactForm> CreateContactForm(ContactForm contactForm);
        BaseResponse<ContactForm> GetContactFormById(int id);
        BaseResponse<ContactForm> UpdateContactForm(ContactForm contactForm);
        BaseResponse<bool> DeleteContactForm(int id);
    }
}