using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;

namespace sunflower.Controllers
{
    public class TeamViewController : Controller
    {
        // GET: TeamView
        public ActionResult Index()
        {
            TeamViewBusinessLayer t = new TeamViewBusinessLayer();
            List<TeamView> ListofTeamViews = t.TeamViews.ToList();

            return View(ListofTeamViews);
        }

        // GET: TeamView/Create
        public ActionResult Create()
        {
            TeamBusinessLayer t = new TeamBusinessLayer();
            List<Team> ListofTeams = t.Teams.ToList();

            PivotViewBusinessLayer p = new PivotViewBusinessLayer();
            List<PivotView> ListofPivotViews = p.PivotViews(String.Empty).ToList();

            ViewData["ddTeam"] = ListofTeams.Select(m => new SelectListItem { Value = m.TeamID.ToString(), Text = m.TeamDesc + " (" + m.TeamID.ToString() + ")"});
            ViewData["ddPivotView"] = ListofPivotViews.Select(m => new SelectListItem { Value = m.ViewID.ToString(), Text = m.ViewDesc + " (" + m.ViewID.ToString() + ")"});

            return View();
        }

        // POST: TeamView/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Create";
            try
            {
                TeamView a = new TeamView();

                #region Pull from Form Collection
                a.TeamID = Convert.ToInt16(collection["ddTeam"]);
                a.ViewID = Convert.ToInt16(collection["ddPivotView"]);
                a.ViewDescription = "";
                a.TeamDescription="";
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

        // GET: TeamView/Delete/5
        public ActionResult Delete(string id)
        {
            TeamViewBusinessLayer t = new TeamViewBusinessLayer();
            TeamView tv = t.TeamViews.Where(i => i.ID == id).Single();

            return View(tv);
        }

        // POST: TeamView/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Delete";
            try
            {
                TeamViewBusinessLayer bl = new TeamViewBusinessLayer();
                TeamView a = bl.TeamViews.Where(p => p.ID == id).Single();

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
