using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoDoc.Models;

namespace AutoDoc.Controllers
{
    [Authorize(Roles="Администратор, Учебная часть")]
    public class StudentController : Controller
    {
        AutoDocContext _db = new AutoDocContext();

        [ChildActionOnly, AllowAnonymous]
        public ActionResult _students(IEnumerable<Student> model)
        {
            return PartialView(model);
        }

        public ActionResult Create(int GroupId)
        {
            var model = new Student();
            ViewBag.GroupName = _db.Groups.Find(GroupId).GroupName;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Student model)
        {
            if (ModelState.IsValid)
            {
                _db.Students.Add(model);
                _db.SaveChanges();

                var group = _db.Groups.Find(model.GroupId);

                foreach (var subject in group.Subjects)
                {
                    foreach (var section in subject.Sections)
                    {
                        foreach (var item in section.ControlForms)
                        {
                            var mark = new Mark();
                            mark.FormId = item.Id;
                            mark.StudentId = model.Id;
                            _db.Marks.Add(mark);
                        }
                    }
                }

                _db.SaveChanges();

                return RedirectToAction("Index", "Group", new { id = model.GroupId });
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var model = _db.Students.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int fake = 0)
        {
            var model = _db.Students.Find(id);
            _db.Students.Remove(model);
            _db.SaveChanges();
            return RedirectToAction("Index", "Group", new { id = model.GroupId });
        }

        public ActionResult Edit(int id)
        {
            var model = _db.Students.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Student model)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(model).State = System.Data.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index", "Group", new { id = model.GroupId });
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Search(string search)
        {
            var model = new List<Student>();
            var parts = search.Split(' ');
            foreach (var item in parts)
            {
                var students = _db.Students.Where(x => x.LastName.StartsWith(item)).ToList();
                foreach (var stud in students)
                {
                    stud.GroupName = _db.Groups.Find(stud.GroupId).GroupName;
                    if (!model.Contains(stud)) model.Add(stud);
                }
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
