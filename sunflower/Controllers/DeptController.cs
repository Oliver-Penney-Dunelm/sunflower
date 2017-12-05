using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;


namespace sunflower.Controllers
{
    public class DeptController : Controller
    {
        // GET: Dept
        public ActionResult Index()
        {
            DeptBusinessLayer dbl = new DeptBusinessLayer();
            List<Dept> ListOfDepts = dbl.Depts.Where(i=>i.DeptActive==1).OrderBy(j=>j.DeptOrder).ToList();
            return View(ListOfDepts);
        }

        public ActionResult Admin()
        {
            DeptBusinessLayer dbl = new DeptBusinessLayer();
            List<Dept> ListOfDepts = dbl.Depts.OrderBy(j => j.DeptOrder).ToList();
            return View(ListOfDepts);
        }

        // GET: Dept/Create
        public ActionResult Create()
        {
            var Items = new HashSet<SelectListItem>();
            Items.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = true });
            Items.Add(new SelectListItem { Text = "No", Value = "0", Selected = false });
            ViewData["ddActive"] = Items;
            return View();
        }

        // POST: Dept/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Create";
            try
            {
                Dept a = new Dept();

                #region Pull from Form Collection
                a.DeptID = Convert.ToInt32(collection["DeptID"]);
                a.DeptName = (string)collection["DeptName"];
                a.DeptOrder = Convert.ToInt32(collection["DeptOrder"]);
                a.DeptActive = Convert.ToInt32(collection["Active"]);
                #endregion

                StoredProcedureBusinessLayer spbl = new StoredProcedureBusinessLayer();
                DidItWork = spbl.ExecuteStoredProcedure(a, CrudAction, User.Identity.Name);
                if (DidItWork == false)
                {
                    return Content(string.Format("Error on {0} of {1}. Press back to return and try again", CrudAction, a.GetType().Name));
                }
                else
                {
                    return RedirectToAction("Admin");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Dept/Edit/5
        public ActionResult Edit(int id)
        {
            DeptBusinessLayer bl = new DeptBusinessLayer();
            Dept o = bl.Depts.Where(p => p.DeptID == id).Single();

            var Items = new HashSet<SelectListItem>();
            Items.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = o.DeptActive==1 });
            Items.Add(new SelectListItem { Text = "No", Value = "0", Selected = o.DeptActive == 0 });
            ViewData["ddActive"] = Items;

            ViewBag.ID = id;
            return View(o);
        }

        // POST: Dept/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Edit";
            try
            {
                DeptBusinessLayer bl = new DeptBusinessLayer();
                Dept a = bl.Depts.Where(p => p.DeptID == id).Single();

                #region Pull from Form Collection
                a.DeptName = (string)collection["DeptName"];
                a.DeptOrder = Convert.ToInt32(collection["DeptOrder"]);
                a.DeptActive = Convert.ToInt32(collection["Active"]);
                #endregion

                StoredProcedureBusinessLayer spbl = new StoredProcedureBusinessLayer();
                DidItWork = spbl.ExecuteStoredProcedure(a, CrudAction, User.Identity.Name);
                if (DidItWork == false)
                {
                    return Content(string.Format("Error on {0} of {1}. Press back to return and try again", CrudAction, a.GetType().Name));
                }
                else
                {
                    return RedirectToAction("Admin");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Dept/Delete/5
        public ActionResult Delete(int id)
        {
            DeptBusinessLayer bl = new DeptBusinessLayer();
            Dept o = bl.Depts.Where(p => p.DeptID == id).Single();

            ViewBag.ID = id;
            return View(o);
        }

        // POST: Dept/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Delete";
            try
            {
                DeptBusinessLayer bl = new DeptBusinessLayer();
                Dept a = bl.Depts.Where(p => p.DeptID == id).Single();

                StoredProcedureBusinessLayer spbl = new StoredProcedureBusinessLayer();
                DidItWork = spbl.ExecuteStoredProcedure(a, CrudAction, User.Identity.Name);
                if (DidItWork == false)
                {
                    return Content(string.Format("Error on {0} of {1}. Press back to return and try again", CrudAction, a.GetType().Name));
                }
                else
                {
                    return RedirectToAction("Admin");
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
