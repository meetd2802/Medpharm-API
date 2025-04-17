using Medpharm.DataAccess.DBConnection;
using Medpharm.Models;
using System.Collections.Generic;
using System.Linq;

namespace Medpharm.DataAccess.Repository
{
    public class ContactFormRepository : IContactFormRepository
    {
        public List<ContactForm> GetAllContactForms()
        {
            using (var context = new DBConnectionFactory())
            {
                return context.contactForms.Select(x => new ContactForm
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Email = x.Email,
                    Subject = x.Subject,
                    Message = x.Message,
                    CreatedAt = x.CreatedAt
                }).ToList();
            }
        }

        public ContactForm GetContactFormById(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                return context.contactForms
                    .Where(c => c.Id == id)
                    .Select(x => new ContactForm
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        Email = x.Email,
                        Subject = x.Subject,
                        Message = x.Message,
                        CreatedAt = x.CreatedAt
                    })
                    .FirstOrDefault();
            }
        }

        public bool CreateContactForm(ContactForm contactForm)
        {
            using (var context = new DBConnectionFactory())
            {
                var newContactForm = new DBConnectionFactory.ContactForm
                {
                    FullName = contactForm.FullName,
                    Email = contactForm.Email,
                    Subject = contactForm.Subject,
                    Message = contactForm.Message,
                    CreatedAt = contactForm.CreatedAt
                };

                context.contactForms.Add(newContactForm);
                return context.SaveChanges() > 0;
            }
        }

        public bool UpdateContactForm(ContactForm contactForm)
        {
            using (var context = new DBConnectionFactory())
            {
                var existingContactForm = context.contactForms.FirstOrDefault(c => c.Id == contactForm.Id);
                if (existingContactForm == null)
                {
                    return false; // Contact form entry not found
                }

                // Update fields
                existingContactForm.FullName = contactForm.FullName;
                existingContactForm.Email = contactForm.Email;
                existingContactForm.Subject = contactForm.Subject;
                existingContactForm.Message = contactForm.Message;

                context.SaveChanges();
                return true; // Update successful
            }
        }

        public bool DeleteContactForm(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                var contactFormEntry = context.contactForms.FirstOrDefault(c => c.Id == id);
                if (contactFormEntry == null)
                {
                    return false;
                }

                context.contactForms.Remove(contactFormEntry);
                return context.SaveChanges() > 0;
            }
        }
    }
}
