using AutoDoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AutoDoc.Controllers
{
    [Authorize(Roles="Администратор, Учебная часть")]
    public class GroupController : Controller
    {
        AutoDocContext _db = new AutoDocContext();

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Group model)
        {
            if (ModelState.IsValid)
            {
                _db.Groups.Add(model);
                _db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var model = _db.Groups.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int fake=0)
        {
            var model = _db.Groups.Find(id);

            _db.Groups.Remove(model);
            _db.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult Edit(int id)
        {
            var model = _db.Groups.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Group model)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(model).State = System.Data.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index", "Group", new {id = model.Id});
            }
            return View(model);
        }
        
        [AllowAnonymous]
        public ActionResult List()
        {
            var model = _db.Groups;
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Index(int id)
        {
            var model = _db.Groups.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Rating(int id)
        {
            var group = _db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }

            var model = new RatingGroup();
            model.Id = id;
            model.Name = group.GroupName;

            model.Subjects = new List<RatingSubject>();
            foreach (var item in group.Subjects)
            {
                var subject = new RatingSubject();
                subject.Id = item.Id;
                subject.Name = item.SubjectName;
                subject.ShortName = item.ShortName;

                int s = 0;
                foreach (var mark in _db.Marks.ToList())
                {
                    if (_db.Sections.Find(_db.ControlForms.Find(mark.FormId).SectionId).SubjectId == item.Id)
                        s += mark.Score;
                }
                subject.AverageScore = (double)s / (double)group.Students.Count;

                model.Subjects.Add(subject);
            }

            model.Students = new List<RatingStudent>();
            foreach (var item in group.Students)
            {
                var student = new RatingStudent();
                student.FullName = item.LastName + item.FirstName + item.MiddleName;
                student.Name = string.Format("{0} {1}.{2}.", item.LastName, item.FirstName[0], item.MiddleName[0]);
                student.Id = item.Id;

                student.Marks = new List<RatingMark>();
                int t = 0;
                foreach (var subjItem in group.Subjects)
                {
                    var mark = new RatingMark();
                    mark.SubjectId = subjItem.Id;

                    int s = 0;
                    foreach (var markItem in _db.Marks.ToList())
                    {
                        if (_db.Sections.Find(_db.ControlForms.Find(markItem.FormId).SectionId).SubjectId == subjItem.Id
                            && markItem.StudentId == item.Id)
                            s += markItem.Score;
                    }
                    t += s;
                    mark.Score = s;
                    student.Marks.Add(mark);
                }

                student.Score = t;
                model.Students.Add(student);
            }

            foreach (var subject in model.Subjects)
            {
                var marklist = new List<double>();
                foreach (var student in model.Students)
                {
                    var mark = student.Marks.FirstOrDefault(x => x.SubjectId == subject.Id);
                    marklist.Add(mark.Score);
                }

                foreach (var student in model.Students)
                {
                    var mark = student.Marks.FirstOrDefault(x => x.SubjectId == subject.Id);
                    mark.Rating = GetRating(marklist, mark.Score);
                }
            }

            var marks = new List<double>();
            foreach (var student in model.Students)
            {
                marks.Add(student.Score);
            }

            foreach (var student in model.Students)
            {
                student.Rating = GetRating(marks, student.Score);
            }

            ViewBag.GroupId = id;
            return View(model);
        }

        private int GetRating(List<double> list, double result)
        {
            var newlist = new List<double>();
            foreach (var item in list)
            {
                if (!newlist.Contains(item))
                    newlist.Add(item);
            }

            newlist.Sort();
            newlist.Reverse();

            for (int i = 0; i < newlist.Count; i++)
            {
                if (result == newlist[i])
                    return i+1;
            }

            return -1;
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
