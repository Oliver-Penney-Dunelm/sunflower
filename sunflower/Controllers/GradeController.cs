using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;

namespace sunflower.Controllers
{
    public class GradeController : Controller
    {
        // GET: Grade
        public ActionResult Index()
        {
            GradeBusinessLayer bl = new GradeBusinessLayer();
            List<Grade> ListOfGrades = bl.Grades.OrderBy(o => o.GradeOrder).ToList();
            return View(ListOfGrades);
        }

        // GET: Grade/Create
        public ActionResult Create()
        {
            var Items = new HashSet<SelectListItem>();
            Items.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = true });
            Items.Add(new SelectListItem { Text = "No", Value = "0", Selected = false });
            ViewData["ddPAG"] = Items;
            ViewData["ddWhole"] = Items;
            ViewData["ddSubCat"] = Items;

            return View();
        }

        // POST: Grade/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Create";
            try
            {
                Grade a = new Grade();

                #region Pull from Form Collection
                a.GradeDescription = (string)collection["GradeDescription"];
                a.GradeOrder = Convert.ToInt32(collection["GradeOrder"]);
                a.PreArticleGrade = Convert.ToInt32(collection["ddPAG"]);
                a.WholeGrade = Convert.ToInt32(collection["ddWhole"]);
                a.SubCatGrade = Convert.ToInt32(collection["ddSubCat"]);
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

        // GET: Grade/Edit/5
        public ActionResult Edit(int id)
        {
            GradeBusinessLayer bl = new GradeBusinessLayer();
            Grade o = bl.Grades.Where(p => p.GradeID == id).Single();

            var PAGItems = new HashSet<SelectListItem>();
            PAGItems.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = o.PreArticleGrade == 1 });
            PAGItems.Add(new SelectListItem { Text = "No", Value = "0", Selected = o.PreArticleGrade == 0 });

            var WholeItems = new HashSet<SelectListItem>();
            WholeItems.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = o.WholeGrade == 1 });
            WholeItems.Add(new SelectListItem { Text = "No", Value = "0", Selected = o.WholeGrade == 0 });

            var SubCatItems = new HashSet<SelectListItem>();
            SubCatItems.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = o.SubCatGrade == 1 });
            SubCatItems.Add(new SelectListItem { Text = "No", Value = "0", Selected = o.SubCatGrade == 0 });

            ViewData["ddPag"] = PAGItems;
            ViewData["ddWhole"] = WholeItems;
            ViewData["ddSubCat"] = SubCatItems;

            return View(o);
        }

        // POST: Grade/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Edit";
            try
            {
                GradeBusinessLayer bl = new GradeBusinessLayer();
                Grade a = bl.Grades.Where(p => p.GradeID == id).Single();

                #region Pull from Form Collection
                a.GradeDescription = (string)collection["GradeDescription"];
                a.GradeOrder = Convert.ToInt32(collection["GradeOrder"]);
                a.PreArticleGrade = Convert.ToInt32(collection["ddPAG"]);
                a.WholeGrade = Convert.ToInt32(collection["ddWhole"]);
                a.SubCatGrade = Convert.ToInt32(collection["ddSubCat"]);
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

        // GET: Grade/Delete/5
        public ActionResult Delete(int id)
        {
            GradeBusinessLayer bl = new GradeBusinessLayer();
            Grade a = bl.Grades.Where(p => p.GradeID == id).Single();
            return View(a);
        }

        // POST: Grade/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Delete";
            try
            {
                GradeBusinessLayer bl = new GradeBusinessLayer();
                Grade a = bl.Grades.Where(p => p.GradeID == id).Single();

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
