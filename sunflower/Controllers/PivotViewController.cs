using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;

namespace sunflower.Controllers
{
    public class PivotViewController : Controller
    {
        // GET: PivotView
        public ActionResult Index()
        {
            PivotViewBusinessLayer sbl = new PivotViewBusinessLayer();
            List<PivotView> ListOfPivotViews = sbl.PivotViews(String.Empty).OrderBy(i => i.ViewID).ToList();

            return View(ListOfPivotViews);
        }

        // GET: PivotView/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PivotView/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Create";
            try
            {
                PivotView a = new PivotView();

                #region Pull from Form Collection
                a.ViewID = -1;
                a.ViewDesc = (string)collection["ViewDesc"];
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

        // GET: PivotView/Edit/5
        public ActionResult Edit(int id)
        {
            PivotViewBusinessLayer bl = new PivotViewBusinessLayer();
            PivotView o = bl.PivotViews(String.Empty).Where(p => p.ViewID == id).Single();

            ViewBag.ID = id;
            return View(o);
        }

        // POST: PivotView/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Edit";
            try
            {
                PivotViewBusinessLayer bl = new PivotViewBusinessLayer();
                PivotView a = bl.PivotViews(String.Empty).Where(p => p.ViewID == id).Single();

                #region Pull from Form Collection
                a.ViewDesc = (string)collection["ViewDesc"];
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

        // GET: PivotView/Delete/5
        public ActionResult Delete(int id)
        {
            PivotViewBusinessLayer bl = new PivotViewBusinessLayer();
            PivotView o = bl.PivotViews(String.Empty).Where(p => p.ViewID == id).Single();

            ViewBag.ID = id;
            return View(o);
        }

        // POST: PivotView/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Delete";
            try
            {
                PivotViewBusinessLayer bl = new PivotViewBusinessLayer();
                PivotView a = bl.PivotViews(String.Empty).Where(p => p.ViewID == id).Single();

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
