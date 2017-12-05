using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;

namespace sunflower.Controllers
{
    public class CatController : Controller
    {
        // GET: Cat
        public ActionResult Index()
        {
            CatBusinessLayer sbl = new CatBusinessLayer();
            List<Cat> ListOfCats = sbl.Cats.ToList();
            return View(ListOfCats);
        }


        // GET: Cat/Create
        public ActionResult Create()
        {
            var ItemsY = new HashSet<SelectListItem>();
            ItemsY.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = true });
            ItemsY.Add(new SelectListItem { Text = "No", Value = "0", Selected = false });

            var ItemsN = new HashSet<SelectListItem>();
            ItemsN.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = false });
            ItemsN.Add(new SelectListItem { Text = "No", Value = "0", Selected = true });

            ViewData["ddActive"] = ItemsY;
            ViewData["ddConcept"] = ItemsN;
            return View();
        }

        // POST: Cat/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Create";
            try
            {
                Cat a = new Cat();

                #region Pull from Form Collection
                a.CatID = Convert.ToInt32(collection["CatID"]);
                a.CatDesc = (string)collection["CatDesc"];
                a.CatActive = Convert.ToInt32(collection["ddActive"]);
                a.CatOrder = Convert.ToInt32(collection["CatOrder"]);
                a.Concept = Convert.ToInt32(collection["ddConcept"]);
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

        // GET: Cat/Edit/5
        public ActionResult Edit(int id)
        {
            CatBusinessLayer bl = new CatBusinessLayer();
            Cat o = bl.Cats.Where(p => p.CatID == id).Single();

            var Items = new HashSet<SelectListItem>();
            Items.Add(new SelectListItem { Text = "Yes", Value = "1" });
            Items.Add(new SelectListItem { Text = "No", Value = "0" });
            ViewData["ddActive"] = Items.Select(m => new SelectListItem { Value = m.Value, Text = m.Text, Selected = o.CatActive.ToString() == m.Value });
            ViewData["ddConcept"] = Items.Select(m => new SelectListItem { Value = m.Value, Text = m.Text, Selected = o.Concept.ToString() == m.Value });

            ViewBag.ID = id;
            return View(o);
        }

        // POST: Cat/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Edit";
            try
            {
                CatBusinessLayer bl = new CatBusinessLayer();
                Cat a = bl.Cats.Where(p => p.CatID == id).Single();

                #region Pull from Form Collection
                a.CatDesc = (string)collection["CatDesc"];
                a.CatActive = Convert.ToInt32(collection["ddActive"]);
                a.CatOrder = Convert.ToInt32(collection["CatOrder"]);
                a.Concept = Convert.ToInt32(collection["ddConcept"]);
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

        // GET: Cat/Delete/5
        public ActionResult Delete(int id)
        {
            CatBusinessLayer bl = new CatBusinessLayer();
            Cat o = bl.Cats.Where(p => p.CatID == id).Single();
            ViewBag.ID = id;
            return View(o);
        }

        // POST: Cat/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Delete";
            try
            {
                CatBusinessLayer bl = new CatBusinessLayer();
                Cat a = bl.Cats.Where(p => p.CatID == id).Single();

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
