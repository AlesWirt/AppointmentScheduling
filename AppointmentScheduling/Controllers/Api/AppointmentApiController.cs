using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AppointmentScheduling.Services;
using AppointmentScheduling.Utility;
using AppointmentScheduling.Models.ViewModels;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Controllers.Api
{
    [Route("api/Appointment")]
    [ApiController]
    public class AppointmentApiController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _loginUserId;
        private readonly string _role;
        public AppointmentApiController(IAppointmentService appointmentService,
            IHttpContextAccessor httpContextAccessor)
        {
            _appointmentService = appointmentService;
            _httpContextAccessor = httpContextAccessor;
            _loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        [HttpPost]
        [Route("SaveCalendarData")]
        public IActionResult SaveCalendarData(AppointmentVM data)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                commonResponse.Status = _appointmentService.AddUpdate(data).Result;
                if (commonResponse.Status == 1)
                {
                    commonResponse.Message = Helper.appointmentUpdated;
                }
                if (commonResponse.Status == 2)
                {
                    commonResponse.Message = Helper.appointmentAdded;
                }
            }
            catch(Exception e)
            {
                commonResponse.Message = e.Message;
                commonResponse.Status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }

        [HttpGet]
        [Route("GetCalendarData")]
        public IActionResult GetCalendarData(string doctorId)
        {
            CommonResponse<List<AppointmentVM>> commonResponse = new CommonResponse<List<AppointmentVM>>();

            try
            {
                if(_role == Helper.Patient)
                {
                    commonResponse.DataEnum = _appointmentService.PatientEventsById(_loginUserId);
                    commonResponse.Status = Helper.success_code;
                }
                else if(_role == Helper.Doctor)
                {
                    commonResponse.DataEnum = _appointmentService.DoctorsEventsById(_loginUserId);
                    commonResponse.Status = Helper.success_code;
                }
                else
                {
                    commonResponse.DataEnum = _appointmentService.DoctorsEventsById(doctorId);
                    commonResponse.Status = Helper.success_code;
                }
            }
            catch(Exception e)
            {
                commonResponse.Message = e.Message;
                commonResponse.Status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }
    }
}
