using AutoDoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace AutoDoc.Controllers
{
    [Authorize(Roles="Администратор, Учебная часть")]
    public class SubjectController : Controller
    {
        AutoDocContext _db = new AutoDocContext();

        [AllowAnonymous, ChildActionOnly]
        public ActionResult _subjects(IEnumerable<Subject> model)
        {
            return PartialView(model);
        }

        public ActionResult Create(int GroupId)
        {
            var group = _db.Groups.Find(GroupId);
            if (group == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupName = group.GroupName;
            var model = new Subject();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Subject model)
        {
            if (ModelState.IsValid)
            {
                _db.Subjects.Add(model);
                _db.SaveChanges();
                _db.Sections.Add(new Section() { Name = "Раздел 1", SubjectId = model.Id });
                _db.Sections.Add(new Section() { Name = "Раздел 2", SubjectId = model.Id });
                _db.Sections.Add(new Section() { Name = "Раздел 3", SubjectId = model.Id });
                _db.Sections.Add(new Section() { Name = "Раздел 4", SubjectId = model.Id });
                _db.SaveChanges();

                return RedirectToAction("Index", "Group", new { id = model.GroupId });
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var model = _db.Subjects.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int fake = 0)
        {
            var model = _db.Subjects.Find(id);
            _db.Subjects.Remove(model);
            _db.SaveChanges();
            return RedirectToAction("Index", "Group", new { id = model.GroupId });
        }

        public ActionResult Edit(int id)
        {
            var model = _db.Subjects.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Subject model)
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
        public ActionResult Index(int id)
        {
            var subject = _db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            var group = _db.Groups.Find(subject.GroupId);

            var model = new DisplaySubject();
            model.Id = subject.Id;
            model.SubjectName = subject.SubjectName;
            model.GroupName = group.GroupName;
            model.Sections = new List<DisplaySection>();

            foreach (var sectItem in subject.Sections)
            {
                var sect = new DisplaySection();
                sect.id = sectItem.Id;
                sect.Name = sectItem.Name;
                sect.ControlForms = new List<ControlForm>();

                foreach (var item in sectItem.ControlForms)
                {
                    sect.ControlForms.Add(item);
                    sect.MaxScore += item.MaxScore;
                }

                sect.AllowedScore = (int)(sect.MaxScore * 0.6);

                model.Sections.Add(sect);

                model.MaxScore += sect.MaxScore;
            }

            model.Students = new List<DisplayStudent>();

            foreach (var studItem in group.Students)
            {
                var stud = new DisplayStudent();
                stud.Name = string.Format("{0} {1}.{2}.", studItem.LastName, studItem.FirstName[0], studItem.MiddleName[0]);
                stud.FullName = studItem.LastName + studItem.FirstName + studItem.MiddleName;
                stud.Sections = new List<DisplaySectionStudent>();
                foreach (var sectItem in subject.Sections)
                {
                    var sect = new DisplaySectionStudent();
                    sect.Id = sectItem.Id;
                    sect.SubjectId = sectItem.SubjectId;
                    sect.StudentId = stud.Id;
                    sect.Marks = new List<Mark>();
                    foreach (var formItem in sectItem.ControlForms)
                    {
                        var mark = _db.Marks.FirstOrDefault(x => x.FormId == formItem.Id && x.StudentId == studItem.Id);
                        sect.Marks.Add(mark);
                        sect.TotalScore += mark.Score;
                    }
                    stud.Sections.Add(sect);
                    stud.TotalScore += sect.TotalScore;
                }

                model.AllowedScore = (int)(model.MaxScore * 0.6);

                if (stud.TotalScore >= model.AllowedScore && stud.TotalScore != 0)
                {
                    stud.IsAllowed = true;
                }
                else 
                {
                    stud.IsAllowed = false;
                }

                model.Students.Add(stud);
            }

            model.Lectors = new List<User>();
            foreach (var item in subject.Lectors.ToList())
            {
                model.Lectors.Add(item);
            }

            ViewBag.GroupId = subject.GroupId;
            return View(model);
        }

        public ActionResult Grant(int id)
        {
            var subject = _db.Subjects.Find(id);
            if (subject == null)
                return HttpNotFound();

            var model = new GrantSubject();
            model.Lectors = new List<User>();
            foreach (var item in _db.Users.ToList())
            {
                if (Roles.IsUserInRole(item.UserLogin, "Преподаватель"))
                    model.Lectors.Add(item);
            }

            model.Lectors.OrderBy(x => x.LastName);
            model.Subject = subject;
            return View(model);
        }

        [HttpPost]
        public ActionResult Grant(int id, int userId)
        {
            var subject = _db.Subjects.Find(id);
            var lector = _db.Users.Find(userId);

            if (subject == null || lector == null)
                return HttpNotFound();

            if (subject.Lectors.Contains(lector))
                subject.Lectors.Remove(lector);
            else
                subject.Lectors.Add(lector);

            _db.SaveChanges();
            return RedirectToAction("Grant", "Subject", new { id = id });
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
