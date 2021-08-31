﻿using AppointmentScheduling.Models;
using AppointmentScheduling.Utility;
using AppointmentScheduling.Models.ViewModels;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _db;

        public AppointmentService(ApplicationDbContext db)
        {
            _db = db;
        }
        public List<DoctorVM> GetDoctorList()
        {
            var doctors = (from user in _db.Users
                           join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                           join roles in _db.Roles.Where(x => x.Name == Helper.Doctor) on userRoles.RoleId equals roles.Id
                           select new DoctorVM
                           {
                               Id = user.Id,
                               Name = user.Name
                           }).ToList();
            return doctors;
        }

        public List<PatientVM> GetPatientList()
        {
            var patients = (from user in _db.Users
                           join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                           join roles in _db.Roles.Where(x => x.Name == Helper.Patient) on userRoles.RoleId equals roles.Id
                           select new PatientVM
                           {
                               Id = user.Id,
                               Name = user.Name
                           }).ToList();
            return patients;
        }

        public async Task<int> AddUpdate(AppointmentVM model)
        {
            var startDate = DateTime.Parse(model.StartDate, CultureInfo.InvariantCulture);
            var endDate = DateTime.Parse(model.StartDate, CultureInfo.InvariantCulture).AddMinutes(Convert.ToDouble(model.Duration));

            if(model != null && model.Id > 0)
            {
                //update
                return 1;
            }
            else
            {
                //create
                Appointment appointment = new Appointment
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = model.Duration,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                    AdminId = model.AdminId,
                    IsDoctorApproved = false
                };
                _db.Appointments.Add(appointment);
                _db.SaveChanges();
                await _db.SaveChangesAsync();
                return 2;
            }
        }

        public int AddUpdate2(AppointmentVM model)
        {
            CultureInfo culture = new CultureInfo("de-DE");
            var startDate = DateTime.Parse(model.StartDate, CultureInfo.InvariantCulture);
            var endDate = DateTime.Parse(model.StartDate, CultureInfo.InvariantCulture).AddMinutes(Convert.ToDouble(model.Duration));

            if (model != null && model.Id > 0)
            {
                //update
                return 1;
            }
            else
            {
                //create
                Appointment appointment = new Appointment
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = model.Duration,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                    AdminId = model.AdminId,
                    IsDoctorApproved = false
                };
                _db.Appointments.Add(appointment);
                _db.SaveChangesAsync();
                return 2;
            }
        }

        public List<AppointmentVM> DoctorsEventsById(string doctorId)
        {
            return _db.Appointments.Where(appointment => appointment.DoctorId == doctorId)
                .ToList().Select(appointment => new AppointmentVM
                {
                    Id = appointment.Id,
                    Description = appointment.Description,
                    StartDate = appointment.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndDate = appointment.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Title = appointment.Title,
                    Duration = appointment.Duration,
                    IsDoctorApproved = appointment.IsDoctorApproved
                }).ToList();
        }

        public List<AppointmentVM> PatientEventsById(string patientId)
        {
            return _db.Appointments.Where(appointment => appointment.PatientId == patientId)
                .ToList().Select(appointment => new AppointmentVM
                {
                    Id = appointment.Id,
                    Description = appointment.Description,
                    StartDate = appointment.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndDate = appointment.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Title = appointment.Title,
                    Duration = appointment.Duration,
                    IsDoctorApproved = appointment.IsDoctorApproved
                }).ToList();
        }
    }
}
