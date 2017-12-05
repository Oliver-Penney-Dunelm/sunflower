using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;

namespace sunflower.Controllers
{
    public class AttributeValueController : Controller
    {
        // GET: AttributeValue
        public ActionResult Index(int Sfid=0,int SeasonID=0)
        {
            AttributeValueBusinessLayer rbl = new AttributeValueBusinessLayer();
            List<AttributeValue> ListOfAttributeValues = rbl.AttributeValues.Where(r => r.SFID == Sfid && r.SeasonID == SeasonID).OrderBy(o => o.AttributeOrder).ToList();

            ViewBag.Sfid = Sfid;
            ViewBag.SeasonID = SeasonID;
            return View(ListOfAttributeValues);
        }

        // GET: AttributeValue/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AttributeValue/Create
        public ActionResult Create(int Sfid = 0, int SeasonID = 0)
        {
            #region StaticDropdowns
            SeasonBusinessLayer sbl = new SeasonBusinessLayer();
            List<Season> ListOfSeasons= sbl.Seasons.Where(r => r.SeasonActive == 1).OrderBy(o => o.FirstLaunchDate).ToList();
            ViewData["ddSeason"] = ListOfSeasons.Select(m => new SelectListItem { Value = m.SeasonID.ToString(), Text = m.SeasonDesc + " (" + m.SeasonID.ToString() + ")", Selected = m.SeasonID == SeasonID });

            AttributeBusinessLayer abl = new AttributeBusinessLayer();
            List<BusinessLayerLibrary.Attribute> ListOfAttributes = abl.Attributes.Where(w=>w.Calculated==0).OrderBy(o => o.AttributeOrder).ToList();

            //exclude attributes that already have values
            AttributeValueBusinessLayer rbl = new AttributeValueBusinessLayer();
            List<AttributeValue> ListOfAttributeValues = rbl.AttributeValues.Where(r => r.SFID == Sfid && r.SeasonID == SeasonID).ToList();
            List<BusinessLayerLibrary.Attribute> ListOfPopulatedAttributes = ListOfAttributeValues.Select(q => new BusinessLayerLibrary.Attribute { AttributeID = q.AttributeID }).ToList();
            ListOfAttributes = ListOfAttributes.Except(ListOfPopulatedAttributes, new Compare.LambdaComparer<BusinessLayerLibrary.Attribute>((x, y) => x.AttributeID == y.AttributeID)).ToList();

            //include only attributes that the user has write permissions for
            //work out what team the user is in
            int TeamID = 0;
            UserBusinessLayer ubl = new UserBusinessLayer();
            string ShortNetworkID = User.Identity.Name.Split('\\')[1].ToLower();
            if (ubl.Users.Any(o => o.NetworkID == ShortNetworkID))
            {
                TeamID = ubl.Users.Where(o => o.NetworkID == ShortNetworkID).Single().TeamID;
            }

            //TeamBusinessLayer tbl = new TeamBusinessLayer();
            //List<Team> WhatTeamsAmIIn = tbl.Teams(User.Identity.Name).ToList();
            TeamAttributePermissionBusinessLayer tapbl = new TeamAttributePermissionBusinessLayer();
            //List<TeamAttributePermission> WhatAttributesCanIWriteTo = tapbl.TeamAttributePermissions.Join(WhatTeamsAmIIn,a=> a.TeamID,b=>b.TeamID,(a,b) => new { a, b }).Select(z => z.a).ToList();
            List<TeamAttributePermission> WhatAttributesCanIWriteTo = tapbl.TeamAttributePermissions.Where(a=>a.TeamID==TeamID).ToList();

            ListOfAttributes = ListOfAttributes.Join(WhatAttributesCanIWriteTo, a => a.AttributeID, b => b.AttributeID, (a, b) => new { a, b }).Select(z => z.a).ToList();

            ViewData["ddAttribute"] = ListOfAttributes.Select(m => new SelectListItem { Value = m.AttributeID.ToString(), Text = m.AttributeDesc + " (" + m.AttributeID.ToString() + ")"});

            ViewBag.Sfid = Sfid;
            ViewBag.SeasonID = SeasonID;

            #endregion

            return View();
        }

        // POST: AttributeValue/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool DidItWork = false;
            try
            {
                int SFID = Convert.ToInt32(collection["SFID"]);
                int SeasonID = Convert.ToInt32(collection["ddSeason"]);
                int AttributeID = Convert.ToInt32(collection["ddAttribute"]);
                string AttributeValueEntry = (string)collection["AttributeValueEntry"];

                AttributeValue NewAV = new AttributeValue();
                NewAV.AttributeID = AttributeID;
                NewAV.SFID = SFID;
                NewAV.SeasonID = SeasonID;
                NewAV.AttributeValueEntry = AttributeValueEntry;

                //AttributeValueBusinessLayer sbl = new AttributeValueBusinessLayer();
                //DidItWork = sbl.InsertAttributeValue(NewAV, User.Identity.Name);
                StoredProcedureBusinessLayer spbl = new StoredProcedureBusinessLayer();
                DidItWork = spbl.ExecuteStoredProcedure(NewAV, "Create", User.Identity.Name);
                if (DidItWork == false)
                {
                    return Content("Error on creation of Attribute Value. Press back to return and try again");
                }
                else
                {
                    return RedirectToAction("Index", new { Sfid = SFID, SeasonID = SeasonID });
                }
            }
            catch (Exception x)
            {
                return Content("Error on creation of Attribute Value. Press back to return and try again");
            }
        }

        // GET: AttributeValue/Edit/5
        public ActionResult Edit(string id)
        {
            AttributeValueBusinessLayer sbl = new AttributeValueBusinessLayer();
            AttributeValue a = sbl.AttributeValues.Where(i => i.ID == id).Single();
            return View(a);
        }

        // POST: AttributeValue/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            bool DidItWork = false;
            try
            {
                AttributeValueBusinessLayer sbl = new AttributeValueBusinessLayer();
                AttributeValue a = sbl.AttributeValues.Where(i => i.ID == id).Single();
                a.AttributeValueEntry = (string)collection["AttributeValueEntry"];

                StoredProcedureBusinessLayer spbl = new StoredProcedureBusinessLayer();
                DidItWork = spbl.ExecuteStoredProcedure(a, "Edit", User.Identity.Name);
                if (DidItWork == false)
                {
                    return Content("Error on edit of attribute value. Press back to return and try again");
                }
                else
                {
                    return RedirectToAction("Index", new { SeasonID = a.SeasonID, SFID = a.SFID });
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: AttributeValue/Delete/5
        public ActionResult Delete(string id)
        {
            AttributeValueBusinessLayer sbl = new AttributeValueBusinessLayer();
            AttributeValue a = sbl.AttributeValues.Where(i => i.ID == id).Single();
            return View(a);
        }

        // POST: AttributeValue/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Delete";
            try
            {
                AttributeValueBusinessLayer sbl = new AttributeValueBusinessLayer();
                AttributeValue a = sbl.AttributeValues.Where(i => i.ID == id).Single();

                //pull data from FormCollection-----------------------------------------------
                //a.AttributeValueEntry = (string)collection["AttributeValueEntry"];

                //--------------------------------------------------------------------------- -

                StoredProcedureBusinessLayer spbl = new StoredProcedureBusinessLayer();
                DidItWork = spbl.ExecuteStoredProcedure(a, CrudAction, User.Identity.Name);
                if (DidItWork == false)
                {
                    return Content(string.Format("Error on {0} of {1}. Press back to return and try again", CrudAction, a.GetType().Name));
                }
                else
                {
                    return RedirectToAction("Index", new { SeasonID = a.SeasonID, SFID = a.SFID });
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
