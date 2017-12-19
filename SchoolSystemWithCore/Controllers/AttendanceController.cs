using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolSystemWithCore.Data;
using SchoolSystemWithCore.Models.ViewModels;

namespace SchoolSystemWithCore.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        private readonly string _baseUrl = "http://attendanceazuretryschoolmanagementmorascorpions.azurewebsites.net";
        private readonly ApplicationDbContext _context;
        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAttendance()
        {
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> GetAttendance([Bind("Date", "ClassName")]AttendanceViewModel model)
        {
            List<StudentAttendance> Students = new List<StudentAttendance>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/AttendanceDetails/" + model.Date + "/" + model.ClassName);

                if (Res.IsSuccessStatusCode)
                {
                    var AttendanceResponse = Res.Content.ReadAsStringAsync().Result;
                    Students = JsonConvert.DeserializeObject<List<StudentAttendance>>(AttendanceResponse);
                }
            }
            
            return View(Students);
        }
    }
}