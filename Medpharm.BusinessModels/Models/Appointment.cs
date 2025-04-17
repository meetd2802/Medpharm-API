using System;
using System.Collections.Generic;
using System.Text;


namespace Medpharm.BusinessModels.Models
{
    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }
    
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public string Name { get; set; }   
        public string Phone { get; set; }
        public string Diseases { get; set; }
        public string History { get; set; }
        public string Medicine { get; set; }   
        public string LabReport { get; set; }
        public string Doctor { get; set; }
        public DateTime AppointmentTime { get; set; }
        
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
        public string Signature { get; set; }
        
        public string Status { get; set; }


    }
}
