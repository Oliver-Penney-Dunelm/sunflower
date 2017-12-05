using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;

namespace sunflower.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            UserBusinessLayer bl = new UserBusinessLayer();
            List<User> ListOfUsers = bl.Users.OrderBy(o => o.NetworkID).ToList();
            return View(ListOfUsers);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            TeamBusinessLayer tbl = new TeamBusinessLayer();
            List<Team> ListOfTeams = tbl.Teams.ToList();
            ViewData["ddTeam"] = ListOfTeams.Select(m => new SelectListItem { Value = m.TeamID.ToString(), Text = m.TeamDesc + " (" + m.TeamID.ToString() + ")" });
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Create";
            try
            {
                User a = new User();

                #region Pull from Form Collection
                a.NetworkID = (string)collection["NetworkID"];
                a.TeamID = Convert.ToInt32(collection["ddTeam"]);
                #endregion

                StoredProcedureBusinessLayer spbl = new StoredProcedureBusinessLayer();
                DidItWork = spbl.ExecuteStoredProcedure(a, CrudAction, User.Identity.Name);
                if (DidItWork == false)
                {
                    return Content(string.Format("Error on {0} of {1}. Press back to return and try again", CrudAction, a.GetType().Name));
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(string id="")
        {
            UserBusinessLayer bl = new UserBusinessLayer();
            User o = bl.Users.Where(p => p.NetworkID == id).Single();
            TeamBusinessLayer tbl = new TeamBusinessLayer();
            List<Team> ListOfTeams = tbl.Teams.ToList();
            ViewData["ddTeam"] = ListOfTeams.Select(m => new SelectListItem { Value = m.TeamID.ToString(), Text = m.TeamDesc + " (" + m.TeamID.ToString() + ")" ,Selected=m.TeamID==o.TeamID});
            ViewBag.NetworkID = o.NetworkID;
            return View(o);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection, string id = "")
        {
            bool DidItWork = false;
            string CrudAction = "Edit";
            try
            {
                UserBusinessLayer bl = new UserBusinessLayer();
                User a = bl.Users.Where(p => p.NetworkID == id).Single();

                #region Pull from Form Collection
                a.TeamID = Convert.ToInt32(collection["ddTeam"]);
                #endregion

                StoredProcedureBusinessLayer spbl = new StoredProcedureBusinessLayer();
                DidItWork = spbl.ExecuteStoredProcedure(a, CrudAction, User.Identity.Name);
                if (DidItWork == false)
                {
                    return Content(string.Format("Error on {0} of {1}. Press back to return and try again", CrudAction, a.GetType().Name));
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(string id="")
        {
            UserBusinessLayer bl = new UserBusinessLayer();
            User o = bl.Users.Where(p => p.NetworkID == id).Single();
            ViewBag.NetworkID = o.NetworkID;
            return View(o);
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(FormCollection collection, string id = "")
        {
            bool DidItWork = false;
            string CrudAction = "Delete";
            try
            {
                UserBusinessLayer bl = new UserBusinessLayer();
                User a = bl.Users.Where(p => p.NetworkID == id).Single();

                StoredProcedureBusinessLayer spbl = new StoredProcedureBusinessLayer();
                DidItWork = spbl.ExecuteStoredProcedure(a, CrudAction, User.Identity.Name);
                if (DidItWork == false)
                {
                    return Content(string.Format("Error on {0} of {1}. Press back to return and try again", CrudAction, a.GetType().Name));
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
