using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;

namespace sunflower.Controllers
{
    public class TeamController : Controller
    {
        // GET: Team
        public ActionResult Index(int SeasonID = 0)
        {
            TeamBusinessLayer sbl = new TeamBusinessLayer();
            List<Team> ListOfTeams = sbl.Teams.OrderBy(i => i.TeamDesc).ToList();

            return View(ListOfTeams);
        }

        // GET: Team/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Team/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Create";
            try
            {
                Team a = new Team();

                #region Pull from Form Collection
                a.TeamID = -1;
                a.TeamDesc = (string)collection["TeamDesc"];
                a.TeamEmail = (string)collection["TeamEmail"];
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

        // GET: Team/Edit/5
        public ActionResult Edit(int id)
        {
            TeamBusinessLayer bl = new TeamBusinessLayer();
            Team o = bl.Teams.Where(p => p.TeamID == id).Single();

            ViewBag.ID = id;
            return View(o);
        }

        // POST: Team/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Edit";
            try
            {
                TeamBusinessLayer bl = new TeamBusinessLayer();
                Team a = bl.Teams.Where(p => p.TeamID == id).Single();

                #region Pull from Form Collection
                a.TeamDesc = (string)collection["TeamDesc"];
                a.TeamEmail = (string)collection["TeamEmail"];
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

        // GET: Team/Delete/5
        public ActionResult Delete(int id)
        {
            TeamBusinessLayer bl = new TeamBusinessLayer();
            Team o = bl.Teams.Where(p => p.TeamID == id).Single();

            ViewBag.ID = id;
            return View(o);
        }

        // POST: Team/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Delete";
            try
            {
                TeamBusinessLayer bl = new TeamBusinessLayer();
                Team a = bl.Teams.Where(p => p.TeamID == id).Single();

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
