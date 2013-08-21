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
    [Authorize(Roles="Администратор")]
    public class UserController : Controller
    {
        AutoDocContext _db = new AutoDocContext();

        public ActionResult Create()
        {
            var model = new User();

            model.PossibleRoles = new List<SelectListItem>();
            foreach (var item in Roles.GetAllRoles())
            {
                var role = new SelectListItem();
                role.Text = item;
                role.Value = item;
                if (role.Value == "Преподаватель") {
                    model.Role = role.Value;
                    role.Selected = true;
                }
                model.PossibleRoles.Add(role);
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserLogin, model.Password,
                        new { FirstName = model.FirstName, MiddleName = model.MiddleName, LastName = model.LastName });
                    Roles.AddUserToRole(model.UserLogin, model.Role);
                    return RedirectToAction("List");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", AccountController.ErrorCodeToString(e.StatusCode));
                }
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var user = _db.Users.Find(id);
            var model = new User()
            {
                UserLogin = user.UserLogin,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                UserId = user.UserId,
            };
            if (model == null) return HttpNotFound();
            if (model.UserId == WebSecurity.CurrentUserId) return RedirectToAction("List");
            return View(model);         
        }

        [HttpPost]
        public ActionResult Delete(int id, int fake = 0)
        {
            var model = _db.Users.Find(id);
            if (model == null) return HttpNotFound();
            if (model.UserId == WebSecurity.CurrentUserId) return RedirectToAction("List");

            ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(model.UserLogin);
            Roles.RemoveUserFromRoles(model.UserLogin, Roles.GetRolesForUser(model.UserLogin));
            ((SimpleMembershipProvider)Membership.Provider).DeleteUser(model.UserLogin, true);

            return RedirectToAction("List");             
        }

        public ActionResult Edit(int id)
        {
            var user = _db.Users.Find(id);
            if (user == null) return HttpNotFound();

            var model = new User()
            {
                UserLogin = user.UserLogin,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                UserId = user.UserId,
                Password = "qwerty",
                ConfirmPassword = "qwerty",
                Role = Roles.GetRolesForUser(user.UserLogin)[0]
            };

            model.PossibleRoles = new List<SelectListItem>();
            foreach (var item in Roles.GetAllRoles().OrderByDescending(x => x))
            {
                var role = new SelectListItem();
                role.Text = item;
                role.Value = item;
                if (role.Value == model.Role)
                    role.Selected = true;
                model.PossibleRoles.Add(role);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(User model)
        {
            if (ModelState.IsValid)
            {
                var user = _db.Users.Find(model.UserId);
                if (user == null) return HttpNotFound();

                user.LastName = model.LastName;
                user.MiddleName = model.MiddleName;
                user.FirstName = model.FirstName;
                user.Password = model.Password;
                user.ConfirmPassword = model.ConfirmPassword;
                if (model.UserId != WebSecurity.CurrentUserId)
                    user.UserLogin = model.UserLogin;
                _db.Entry(user).State = System.Data.EntityState.Modified;
                _db.SaveChanges();

                if (model.UserId != WebSecurity.CurrentUserId)
                {
                    Roles.RemoveUserFromRoles(user.UserLogin, Roles.GetRolesForUser(user.UserLogin));
                    Roles.AddUserToRole(user.UserLogin, model.Role);
                    _db.Entry(user).State = System.Data.EntityState.Modified;
                    _db.SaveChanges();
                }

                return RedirectToAction("List");
            }
            return View(model);
        }

        public ActionResult ChangePassword(int id)
        {
            var user = _db.Users.Find(id);
            if (user == null) return HttpNotFound();

            var model = new User()
            {
                UserLogin = user.UserLogin,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                UserId = user.UserId,
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(User model, int id)
        {
            var user = _db.Users.Find(id);
            if (user == null) return HttpNotFound();
            var token = WebSecurity.GeneratePasswordResetToken(user.UserLogin);
            WebSecurity.ResetPassword(token, model.Password);
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            var model = _db.Users.OrderBy(x => x.LastName).ToList();
            foreach (var item in model)
            {
                item.Role = Roles.GetRolesForUser(item.UserLogin)[0];
            }

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
