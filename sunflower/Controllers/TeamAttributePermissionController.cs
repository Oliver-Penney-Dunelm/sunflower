using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;

namespace sunflower.Controllers
{
    public class TeamAttributePermissionController : Controller
    {
        // GET: TeamAttributePermission
        public ActionResult Index()
        {
            TeamAttributePermissionBusinessLayer bl = new TeamAttributePermissionBusinessLayer();
            List<TeamAttributePermission> ListOfTeamAttributePermissions = bl.TeamAttributePermissions.OrderBy(o => o.TeamID).ToList();
            return View(ListOfTeamAttributePermissions);
        }


        // GET: TeamAttributePermission/Create
        public ActionResult Create()
        {
            TeamBusinessLayer tbl = new TeamBusinessLayer();
            List<Team> ListOfTeams = tbl.Teams.ToList();
            ViewData["ddTeam"] = ListOfTeams.Select(m => new SelectListItem { Value = m.TeamID.ToString(), Text = m.TeamDesc + " (" + m.TeamID.ToString() + ")" });

            AttributeBusinessLayer abl = new AttributeBusinessLayer();
            List<BusinessLayerLibrary.Attribute> ListOfAttributes = abl.Attributes.Where(w => w.Calculated == 0).OrderBy(o => o.AttributeOrder).ToList();
            ViewData["ddAttr"] = ListOfAttributes.Select(m => new SelectListItem { Value = m.AttributeID.ToString(), Text = m.AttributeDesc + " (" + m.AttributeID.ToString() + ")" });

            var Items = new HashSet<SelectListItem>();
            Items.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = false });
            Items.Add(new SelectListItem { Text = "No", Value = "0", Selected = true });
            ViewData["ddRead"] = Items;
            ViewData["ddWrite"] = Items;
            ViewData["ddNotify"] = Items;
            ViewData["ddView"] = Items;
            ViewData["ddPark"] = Items;

            return View();
        }

        // POST: TeamAttributePermission/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Create";
            try
            {
                TeamAttributePermission a = new TeamAttributePermission();

                #region Pull from Form Collection
                a.TeamID = Convert.ToInt32(collection["ddTeam"]);
                a.AttributeID = Convert.ToInt32(collection["ddAttr"]);
                a.ReadPermission = Convert.ToInt32(collection["ddRead"]);
                a.WritePermission = Convert.ToInt32(collection["ddWrite"]);
                a.Notify = Convert.ToInt32(collection["ddNotify"]);
                a.ViewChange = Convert.ToInt32(collection["ddView"]);
                a.ParkForQuarantine = Convert.ToInt32(collection["ddPark"]);
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

        // GET: TeamAttributePermission/Edit/5
        public ActionResult Edit(string id)
        {
            TeamAttributePermissionBusinessLayer bl = new TeamAttributePermissionBusinessLayer();
            TeamAttributePermission o = bl.TeamAttributePermissions.Where(p => p.ID == id).Single();

            TeamBusinessLayer tbl = new TeamBusinessLayer();
            List<Team> ListOfTeams = tbl.Teams.ToList();
            ViewData["ddTeam"] = ListOfTeams.Select(m => new SelectListItem { Value = m.TeamID.ToString(), Text = m.TeamDesc + " (" + m.TeamID.ToString() + ")",Selected=m.TeamID==o.TeamID });

            AttributeBusinessLayer abl = new AttributeBusinessLayer();
            List<BusinessLayerLibrary.Attribute> ListOfAttributes = abl.Attributes.Where(w => w.Calculated == 0).OrderBy(z => z.AttributeOrder).ToList();
            ViewData["ddAttr"] = ListOfAttributes.Select(m => new SelectListItem { Value = m.AttributeID.ToString(), Text = m.AttributeDesc + " (" + m.AttributeID.ToString() + ")",Selected=m.AttributeID==o.AttributeID });



            var Items = new HashSet<SelectListItem>();
            Items.Add(new SelectListItem { Text = "Yes", Value = "1"});
            Items.Add(new SelectListItem { Text = "No", Value = "0" });
            ViewData["ddRead"] = Items.Select(m => new SelectListItem { Value = m.Value, Text = m.Text, Selected = o.ReadPermission.ToString() == m.Value });
            ViewData["ddWrite"] = Items.Select(m => new SelectListItem { Value = m.Value, Text = m.Text, Selected = o.WritePermission.ToString() == m.Value });
            ViewData["ddNotify"] = Items.Select(m => new SelectListItem { Value = m.Value, Text = m.Text, Selected = o.Notify.ToString() == m.Value });
            ViewData["ddView"] = Items.Select(m => new SelectListItem { Value = m.Value, Text = m.Text, Selected = o.ViewChange.ToString() == m.Value });
            ViewData["ddPark"] = Items.Select(m => new SelectListItem { Value = m.Value, Text = m.Text, Selected = o.ParkForQuarantine.ToString() == m.Value });

            ViewBag.ID = id;
            return View(o);
        }

        // POST: TeamAttributePermission/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Edit";
            try
            {
                TeamAttributePermissionBusinessLayer bl = new TeamAttributePermissionBusinessLayer();
                TeamAttributePermission a = bl.TeamAttributePermissions.Where(p => p.ID == id).Single();

                #region Pull from Form Collection
                a.ReadPermission = Convert.ToInt32(collection["ddRead"]);
                a.WritePermission = Convert.ToInt32(collection["ddWrite"]);
                a.Notify = Convert.ToInt32(collection["ddNotify"]);
                a.ViewChange = Convert.ToInt32(collection["ddView"]);
                a.ParkForQuarantine = Convert.ToInt32(collection["ddPark"]);
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

        // GET: TeamAttributePermission/Delete/5
        public ActionResult Delete(string id)
        {
            TeamAttributePermissionBusinessLayer bl = new TeamAttributePermissionBusinessLayer();
            TeamAttributePermission o = bl.TeamAttributePermissions.Where(p => p.ID == id).Single();
            return View(o);
        }

        // POST: TeamAttributePermission/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Delete";
            try
            {
                TeamAttributePermissionBusinessLayer bl = new TeamAttributePermissionBusinessLayer();
                TeamAttributePermission a = bl.TeamAttributePermissions.Where(p => p.ID == id).Single();

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
