using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;

namespace sunflower.Controllers
{
    public class SpaceController : Controller
    {
        // GET: Space
        public ActionResult Index()
        {
            SpaceBusinessLayer bl = new SpaceBusinessLayer();
            List<Space> ListOfSpaces = bl.Spaces.OrderBy(o => o.SpaceOrder).ToList();
            return View(ListOfSpaces);
        }

        // GET: Space/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Space/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Create";
            try
            {
                Space a = new Space();

                #region Pull from Form Collection
                a.SpaceID = -1;
                a.SpaceDesc = (string)collection["SpaceDesc"];
                a.SpaceOrder = Convert.ToInt32(collection["SpaceOrder"]);
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

        // GET: Space/Edit/5
        public ActionResult Edit(int id)
        {
            SpaceBusinessLayer bl = new SpaceBusinessLayer();
            Space o = bl.Spaces.Where(p => p.SpaceID == id).Single();
            ViewBag.ID = id;
            return View(o);
        }

        // POST: Space/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Edit";
            try
            {
                SpaceBusinessLayer bl = new SpaceBusinessLayer();
                Space a = bl.Spaces.Where(p => p.SpaceID == id).Single();

                #region Pull from Form Collection
                a.SpaceOrder = Convert.ToInt32(collection["SpaceOrder"]);
                a.SpaceDesc= (string)collection["SpaceDesc"];
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

        // GET: Space/Delete/5
        public ActionResult Delete(int id)
        {
            SpaceBusinessLayer bl = new SpaceBusinessLayer();
            Space o = bl.Spaces.Where(p => p.SpaceID == id).Single();
            ViewBag.ID = id;
            return View(o);
        }

        // POST: Space/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Delete";
            try
            {
                SpaceBusinessLayer bl = new SpaceBusinessLayer();
                Space a = bl.Spaces.Where(p => p.SpaceID == id).Single();

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
