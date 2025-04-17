using Medpharm.Models;
using System.Collections.Generic;

namespace Medpharm.DataAccess
{
    public interface IContactFormRepository
    {
        List<ContactForm> GetAllContactForms();
        bool CreateContactForm(ContactForm contactForm);
        bool UpdateContactForm(ContactForm contactForm);
        ContactForm GetContactFormById(int id);
        bool DeleteContactForm(int id);
    }
}