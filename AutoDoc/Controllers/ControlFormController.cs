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
    public class ControlFormController : Controller
    {
        AutoDocContext _db = new AutoDocContext();

        public ActionResult Create(int SectionId)
        {
            var section = _db.Sections.Find(SectionId);
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

            var model = new ControlForm();
            model.SubjectId = section.SubjectId;
            return View(model);     
        }

        [HttpPost]
        public ActionResult Create(ControlForm model, int SectionId)
        {
            var section = _db.Sections.Find(SectionId);
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
                var group = _db.Groups.Find(subject.GroupId);
                _db.ControlForms.Add(model);
                _db.SaveChanges();

                model.Marks = new List<Mark>();
                foreach (var item in group.Students)
                {
                    var mark = new Mark();
                    mark.Score = 0;
                    mark.StudentId = item.Id;
                    mark.FormId = model.Id;
                    model.Marks.Add(mark);
                }

                _db.SaveChanges();

                return RedirectToAction("Index", "Subject", new { id = model.SubjectId});
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var model = _db.ControlForms.Find(id);

            var section = _db.Sections.Find(model.SectionId);
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

            if (model == null) 
            {
                return HttpNotFound();
            }

            model.SubjectId = subject.Id;
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int fake = 0)
        {
            var model = _db.ControlForms.Find(id);

            var section = _db.Sections.Find(model.SectionId);
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

            var marklist = model.Marks.ToList();

            foreach (var item in marklist)
            {
                _db.Marks.Remove(item);
            }
            _db.ControlForms.Remove(model);
            _db.SaveChanges();

            return RedirectToAction("Index", "Subject", new { id = subject.Id });
        }

        public ActionResult Edit(int id)
        {
            var model = _db.ControlForms.Find(id);

            var section = _db.Sections.Find(model.SectionId);
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

            if (model == null)
            {
                return HttpNotFound();
            } 

            model.SubjectId = subject.Id;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ControlForm model)
        {
            var section = _db.Sections.Find(model.SectionId);
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
