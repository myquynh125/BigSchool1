using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigSchool1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Net;
using System.ComponentModel.DataAnnotations;
namespace BigSchool1.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Create()
        {
            BigSchoolContext context = new BigSchoolContext();
            Cours objCourse = new Cours();
            objCourse.listcategory = context.Categories.ToList();
            return View(objCourse);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cours objCourse)
        {
            BigSchoolContext context = new BigSchoolContext();
            ModelState.Remove("LectureId");
            if (!ModelState.IsValid)
            {
                objCourse.listcategory = context.Categories.ToList();
                return View("Create", objCourse);
            }
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LectureId = user.Id;
            context.Courses.Add(objCourse);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Attending()
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser currenUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById
                (System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = context.Attendences.Where(p => p.Attendee == currenUser.Id).ToList();
            var courses = new List<Cours>();
            foreach (Attendence temp in listAttendances)
            {
                Cours objCourse = temp.Cours;

                objCourse.LectureName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LectureId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }
        public ActionResult Mine()
        {
            ApplicationUser currenUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById
                (System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            var courses = context.Courses.Where(c => c.LectureId == currenUser.Id && c.DateTime > DateTime.Now).ToList();
            foreach (Cours i in courses)
            {
                i.LectureId = currenUser.Name;
            }
            return View(courses);
        }
        public ActionResult Edit(int? id)
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().
            GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours course = context.Courses.Where(c => c.id == id && c.LectureId == currentUser.Id).FirstOrDefault();
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(context.Categories, "Id", "Name", course.CategoryId);
            return View(course);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,LectureId,Place,DateTime,CategoryId")] Cours course)
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().
            GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            course.LectureId = currentUser.Id;
            if (ModelState.IsValid)
            {
                context.Entry(course).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Mine");
            }
            ViewBag.CategoryId = new SelectList(context.Categories, "Id", "Name", course.CategoryId);
            return View(course);
        }
        public ActionResult Delete(int? id)
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().
            GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours course = context.Courses.Where(c => c.id == id && c.LectureId == currentUser.Id).FirstOrDefault();
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().
            GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            Cours course = context.Courses.Where(c => c.id == id && c.LectureId == currentUser.Id).FirstOrDefault();
            Attendence attendance = context.Attendences.Where(a => a.CourseId == id).FirstOrDefault();
            context.Courses.Remove(course);
            if (attendance != null)
            {
                context.Attendences.Remove(attendance);

            }
            context.SaveChanges();
            return RedirectToAction("Mine");
        }
        public ActionResult LectureIamGoing()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().
            GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            var listFollowee = context.Followings.Where(p => p.FollowerId == currentUser.Id).ToList();
            var listAttendances = context.Attendences.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Cours>();
            foreach(var course in listAttendances)
            {
                foreach(var item in listFollowee)
                {
                    if (item.FolloweeId == course.Cours.LectureId)
                    {
                        Cours objCourse = course.Cours;
                        objCourse.LectureName = System.Web.HttpContext.Current.GetOwinContext().
                        GetUserManager<ApplicationUserManager>().FindById(objCourse.LectureId).Name;
                        courses.Add(objCourse);
                    }
                }
            }
            return View(courses);
        }
    }
}