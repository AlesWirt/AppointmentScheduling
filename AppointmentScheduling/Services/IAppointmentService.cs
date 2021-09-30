using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentScheduling.Models.ViewModels;
using AppointmentScheduling.Models;

namespace AppointmentScheduling.Services
{
    public interface IAppointmentService
    {
        public List<DoctorVM> GetDoctorList();
        public List<PatientVM> GetPatientList();
        public Task<int> AddUpdate(AppointmentVM model);
        public List<AppointmentVM> DoctorsEventsById(string doctorId);
        public List<AppointmentVM> PatientEventsById(string patientId);
    }
}
