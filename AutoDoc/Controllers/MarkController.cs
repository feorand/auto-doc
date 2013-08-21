using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoDoc.Models;
using WebMatrix.WebData;

namespace AutoDoc.Controllers
{
    [Authorize]
    public class MarkController : Controller
    {
        AutoDocContext _db = new AutoDocContext();

        public ActionResult Edit(int id)
        {
            var model = _db.Marks.Find(id);
            var cForm = _db.ControlForms.Find(model.FormId);

            var section = _db.Sections.Find(cForm.SectionId);
            if (section == null)
            {
                return HttpNotFound();
            }
            var user = _db.Users.Find(WebSecurity.CurrentUserId);
            var subject = _db.Subjects.Find(section.SubjectId);
            if (!user.Subjects.Contains(subject))
            {
                return HttpNotFound();
            }

            if (model == null) return HttpNotFound();

            var student = _db.Students.Find(model.StudentId);
            model.StudentName = string.Format("{0} {1}.{2}.", student.LastName, student.FirstName[0], student.MiddleName[0]);

            model.FormName = string.Format("{0} ({1})", cForm.Name, cForm.Description);
            model.MaxScore = cForm.MaxScore;

            model.SubjectId = subject.Id;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Mark model)
        {
            var cForm = _db.ControlForms.Find(model.FormId);

            var section = _db.Sections.Find(cForm.SectionId);
            if (section == null)
            {
                return HttpNotFound();
            }
            var user = _db.Users.Find(WebSecurity.CurrentUserId);
            var subject = _db.Subjects.Find(section.SubjectId);
            if (!user.Subjects.Contains(subject))
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Entry(model).State = System.Data.EntityState.Modified;
                _db.SaveChanges();

                return RedirectToAction("Index", "Subject", new { id = model.SubjectId });
            }
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (_db != null)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
