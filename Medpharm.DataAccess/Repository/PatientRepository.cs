using Medpharm.DataAccess.DBConnection;
using Medpharm.Models;
using System.Collections.Generic;
using System.Linq;

namespace Medpharm.DataAccess.Repository
{
    public class PatientRepository : IPatientRepository
    {
        public List<Patient> GetAllPatients()
        {
            using (var context = new DBConnectionFactory())
            {
                return context.patients.Select(x => new Patient
                {
                    Id = x.Id,
                    Name = x.Name,
                    Phone = x.Phone,
                    Diseases = x.Diseases,
                    History = x.History,
                    Medicine = x.Medicine,
                    LabReport = x.LabReport,
                    Surgery = x.Surgery,
                    Gender = x.Gender,
                    Weight = x.Weight,
                    Prescription = x.Prescription,
                    Reports = x.Reports,
                    CreatedAt = x.CreatedAt
                }).ToList();
            }
        }

        public Patient GetPatientById(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                return context.patients
                    .Where(p => p.Id == id)
                    .Select(x => new Patient
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Phone = x.Phone,
                        Diseases = x.Diseases,
                        History = x.History,
                        Medicine = x.Medicine,
                        LabReport = x.LabReport,
                        Surgery = x.Surgery,
                        Gender = x.Gender,
                        Weight = x.Weight,
                        Prescription = x.Prescription,
                        Reports = x.Reports,
                        CreatedAt = x.CreatedAt
                    })
                    .FirstOrDefault();
            }
        }

        public bool CreatePatient(Patient patient)
        {
            using (var context = new DBConnectionFactory())
            {
                var newPatient = new DBConnectionFactory.Patient
                {
                    Name = patient.Name,
                    Phone = patient.Phone,
                    Diseases = patient.Diseases,
                    History = patient.History,
                    Medicine = patient.Medicine,
                    LabReport = patient.LabReport,
                    Surgery = patient.Surgery,
                    Gender = patient.Gender,
                    Weight = patient.Weight,
                    Prescription = patient.Prescription,
                    Reports = patient.Reports,
                    CreatedAt = patient.CreatedAt
                };

                context.patients.Add(newPatient);
                return context.SaveChanges() > 0;
            }
        }

        public bool UpdatePatient(Patient patient)
        {
            using (var context = new DBConnectionFactory())
            {
                var existingPatient = context.patients.FirstOrDefault(p => p.Id == patient.Id);
                if (existingPatient == null)
                {
                    return false; // Patient not found
                }

                // Update fields
                existingPatient.Name = patient.Name;
                existingPatient.Phone = patient.Phone;
                existingPatient.Diseases = patient.Diseases;
                existingPatient.History = patient.History;
                existingPatient.Medicine = patient.Medicine;
                existingPatient.LabReport = patient.LabReport;
                existingPatient.Surgery = patient.Surgery;
                existingPatient.Gender = patient.Gender;
                existingPatient.Weight = patient.Weight;
                existingPatient.Prescription = patient.Prescription;
                existingPatient.Reports = patient.Reports;

                context.SaveChanges();
                return true; // Update successful
            }
        }

        public bool DeletePatient(int id)
        {
            using (var context = new DBConnectionFactory())
            {
                var patient = context.patients.FirstOrDefault(p => p.Id == id);
                if (patient == null)
                {
                    return false;
                }

                context.patients.Remove(patient);
                return context.SaveChanges() > 0;
            }
        }
    }
}
