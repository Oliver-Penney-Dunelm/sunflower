using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;

namespace sunflower.Controllers
{
    public class MeTypeController : Controller
    {
        // GET: MeType
        public ActionResult Index()
        {
            MeTypeBusinessLayer sbl = new MeTypeBusinessLayer();
            List<MeType> ListOfMeTypes = sbl.MeTypes.OrderBy(i => i.MeTypeID).ToList();

            return View(ListOfMeTypes);
        }

        // GET: MeType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MeType/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Create";
            try
            {
                MeType a = new MeType();

                #region Pull from Form Collection
                a.MeTypeID = -1;
                a.MeTypeDesc = (string)collection["MeTypeDesc"];
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

        // GET: MeType/Edit/5
        public ActionResult Edit(int id)
        {
            MeTypeBusinessLayer bl = new MeTypeBusinessLayer();
            MeType o = bl.MeTypes.Where(p => p.MeTypeID == id).Single();

            ViewBag.ID = id;
            return View(o);
        }

        // POST: MeType/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Edit";
            try
            {
                MeTypeBusinessLayer bl = new MeTypeBusinessLayer();
                MeType a = bl.MeTypes.Where(p => p.MeTypeID == id).Single();

                #region Pull from Form Collection
                a.MeTypeDesc = (string)collection["MeTypeDesc"];
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

        // GET: MeType/Delete/5
        public ActionResult Delete(int id)
        {
            MeTypeBusinessLayer bl = new MeTypeBusinessLayer();
            MeType o = bl.MeTypes.Where(p => p.MeTypeID == id).Single();

            ViewBag.ID = id;
            return View(o);
        }

        // POST: MeType/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Delete";
            try
            {
                MeTypeBusinessLayer bl = new MeTypeBusinessLayer();
                MeType a = bl.MeTypes.Where(p => p.MeTypeID == id).Single();

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
