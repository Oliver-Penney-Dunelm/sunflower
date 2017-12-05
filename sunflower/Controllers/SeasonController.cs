using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;

namespace sunflower.Controllers
{
    public class SeasonController : Controller
    {
        // GET: Season
        public ActionResult Index()
        {
            SeasonBusinessLayer sbl = new SeasonBusinessLayer();
            List<Season> ListOfSeasons = sbl.Seasons.OrderBy(i => i.FirstLaunchDate).ToList();
            return View(ListOfSeasons);
        }


        // GET: Season/Create
        public ActionResult Create()
        {
            var Items = new HashSet<SelectListItem>();
            Items.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = true });
            Items.Add(new SelectListItem { Text = "No", Value = "0", Selected = false });
            ViewData["ddActive"] = Items;
            return View();
        }

        // POST: Season/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Create";
            try
            {
                Season a = new Season();

                #region Pull from Form Collection
                a.SeasonID = Convert.ToInt32(collection["SeasonID"]);
                a.SeasonDesc = (string)collection["SeasonDesc"];
                a.SeasonActive = Convert.ToInt32(collection["ddActive"]);
                a.FirstLaunchDate = Convert.ToDateTime(collection["FirstLaunchDate"]);
                a.VAT = Convert.ToDecimal(collection["VAT"]);
                a.GBPUSD = Convert.ToDecimal(collection["GBPUSD"]);
                a.GBPEUR = Convert.ToDecimal(collection["GBPEUR"]);
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

        // GET: Season/Edit/5
        public ActionResult Edit(int id)
        {
            SeasonBusinessLayer bl = new SeasonBusinessLayer();
            Season o = bl.Seasons.Where(p => p.SeasonID == id).Single();

            var Items = new HashSet<SelectListItem>();
            Items.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = o.SeasonActive == 1 });
            Items.Add(new SelectListItem { Text = "No", Value = "0", Selected = o.SeasonActive == 0 });
            ViewData["ddActive"] = Items;

            ViewBag.ID = id;
            return View(o);
        }

        // POST: Season/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Edit";
            try
            {
                SeasonBusinessLayer bl = new SeasonBusinessLayer();
                Season a = bl.Seasons.Where(p => p.SeasonID == id).Single();

                #region Pull from Form Collection
                a.SeasonDesc = (string)collection["SeasonDesc"];
                a.SeasonActive = Convert.ToInt32(collection["ddActive"]);
                a.FirstLaunchDate = Convert.ToDateTime(collection["FirstLaunchDate"]);
                a.VAT = Convert.ToDecimal(collection["VAT"]);
                a.GBPUSD = Convert.ToDecimal(collection["GBPUSD"]);
                a.GBPEUR = Convert.ToDecimal(collection["GBPEUR"]);
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

        // GET: Season/Delete/5
        public ActionResult Delete(int id)
        {
            SeasonBusinessLayer bl = new SeasonBusinessLayer();
            Season o = bl.Seasons.Where(p => p.SeasonID == id).Single();
            ViewBag.ID = id;
            return View(o);
        }

        // POST: Season/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Delete";
            try
            {
                SeasonBusinessLayer bl = new SeasonBusinessLayer();
                Season a = bl.Seasons.Where(p => p.SeasonID == id).Single();

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
