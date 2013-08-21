using AutoDoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using System.Web.Security;

namespace AutoDoc.Controllers
{
    public class LayoutController : Controller
    {
        AutoDocContext _db = new AutoDocContext();

        public ActionResult LoginPartial()
        {
            var model = new User();
            var roles = (SimpleRoleProvider)Roles.Provider;
            if (WebSecurity.IsAuthenticated) {
                model = _db.Users.Find(WebSecurity.CurrentUserId);
                model.Role = roles.GetRolesForUser(model.UserLogin)[0];
            }
            return PartialView(model);
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
