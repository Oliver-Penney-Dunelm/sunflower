using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;


namespace sunflower.Controllers
{
    public class MerchCatController : Controller
    {
        // GET: MerchCat
        public ActionResult Index()
        {
            MerchCatBusinessLayer bl = new MerchCatBusinessLayer();
            List<MerchCat> ListOfMerchCats = bl.MerchCats.OrderBy(o => o.MerchCatID).ToList();
            return View(ListOfMerchCats);
        }

        // GET: MerchCat/Create
        public ActionResult Create()
        {
            DeptBusinessLayer dbl = new DeptBusinessLayer();
            List<Dept> ListOfDepts = dbl.Depts.Where(j=>j.DeptActive==1).OrderBy(o => o.DeptOrder).ToList();

            CatBusinessLayer bl = new CatBusinessLayer();
            List<Cat> ListOfCats = bl.Cats.Where(j => j.CatActive == 1).OrderBy(o => o.CatOrder).ToList();

            var Items = new HashSet<SelectListItem>();
            Items.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = true });
            Items.Add(new SelectListItem { Text = "No", Value = "0", Selected = false });
            ViewData["ddActive"] = Items;

            ViewData["ddDept"] = ListOfDepts.Select(m => new SelectListItem { Value = m.DeptID.ToString(), Text = m.DeptName + " (" + m.DeptID.ToString() + ")" });
            ViewData["ddCat"] = ListOfCats.Select(m => new SelectListItem { Value = m.CatID.ToString(), Text = m.CatDesc + " (" + m.CatID.ToString() + ")" });

            return View();
        }

        // POST: MerchCat/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Create";
            try
            {
                MerchCat a = new MerchCat();

                #region Pull from Form Collection
                a.MerchCatID = Convert.ToInt32(collection["MerchCatID"]);
                a.MerchCatDesc = (string)collection["MerchCatDesc"];
                a.DeptID = Convert.ToInt32(collection["ddDept"]);
                a.CatID = Convert.ToInt32(collection["ddCat"]);
                a.MerchCatActive = Convert.ToInt32(collection["ddActive"]);
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

        // GET: MerchCat/Edit/5
        public ActionResult Edit(int id)
        {
            MerchCatBusinessLayer bl = new MerchCatBusinessLayer();
            MerchCat o = bl.MerchCats.Where(p => p.MerchCatID == id).Single();

            DeptBusinessLayer dbl = new DeptBusinessLayer();
            List<Dept> ListOfDepts = dbl.Depts.Where(j => j.DeptActive == 1).OrderBy(z => z.DeptOrder).ToList();

            CatBusinessLayer cbl = new CatBusinessLayer();
            List<Cat> ListOfCats = cbl.Cats.Where(j => j.CatActive == 1).OrderBy(z => z.CatOrder).ToList();
            
            var Items = new HashSet<SelectListItem>();
            Items.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = o.MerchCatActive==1 });
            Items.Add(new SelectListItem { Text = "No", Value = "0", Selected = o.MerchCatActive == 0 });
            ViewData["ddActive"] = Items;

            ViewData["ddDept"] = ListOfDepts.Select(m => new SelectListItem { Value = m.DeptID.ToString(), Text = m.DeptName + " (" + m.DeptID.ToString() + ")",Selected=o.DeptID==m.DeptID });
            ViewData["ddCat"] = ListOfCats.Select(m => new SelectListItem { Value = m.CatID.ToString(), Text = m.CatDesc + " (" + m.CatID.ToString() + ")" ,Selected=o.CatID==m.CatID});

            ViewBag.ID = id;
            return View(o);
        }

        // POST: MerchCat/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Edit";
            try
            {
                MerchCatBusinessLayer bl = new MerchCatBusinessLayer();
                MerchCat a = bl.MerchCats.Where(p => p.MerchCatID == id).Single();

                #region Pull from Form Collection
                a.MerchCatDesc = (string)collection["MerchCatDesc"];
                a.DeptID = Convert.ToInt32(collection["ddDept"]);
                a.CatID = Convert.ToInt32(collection["ddCat"]);
                a.MerchCatActive = Convert.ToInt32(collection["ddActive"]);
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

        // GET: MerchCat/Delete/5
        public ActionResult Delete(int id)
        {
            MerchCatBusinessLayer bl = new MerchCatBusinessLayer();
            MerchCat o = bl.MerchCats.Where(p => p.MerchCatID == id).Single();
            ViewBag.ID = id;
            return View(o);
        }

        // POST: MerchCat/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Delete";
            try
            {
                MerchCatBusinessLayer bl = new MerchCatBusinessLayer();
                MerchCat a = bl.MerchCats.Where(p => p.MerchCatID == id).Single();

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
