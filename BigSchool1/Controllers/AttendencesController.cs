using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BigSchool1.Models;
using Microsoft.AspNet.Identity;
namespace BigSchool1.Controllers
{
    public class AttendencesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Attend(Cours attdanceDto)
        {

            var userID = User.Identity.GetUserId();
            BigSchoolContext context = new BigSchoolContext();
            if (context.Attendences.Any(p => p.Attendee == userID && p.CourseId == attdanceDto.id))
            {
                return BadRequest("The attendance already exists");
            }
            var attendance = new Attendence() { CourseId = attdanceDto.id, Attendee = User.Identity.GetUserId() };
            context.Attendences.Add(attendance);
            context.SaveChanges();
            return Ok();
        }
    }
}
