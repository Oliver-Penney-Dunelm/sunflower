using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;

namespace sunflower.Controllers
{
    public class AvailableSpaceController : Controller
    {
        // GET: AvailableSpace
        public ActionResult Index(int DeptID = 0, int SeasonID = 0, int GradeID = 0, int CatID = 0)
        {
            string DeptName = "";
            string CatName = "";
            string GradeName = "";

            AvailableSpaceBusinessLayer sbl = new AvailableSpaceBusinessLayer();
            List<AvailableSpace> ListOfAvailableSpaces = sbl.AvailableSpaces.ToList();

            GradeBusinessLayer gbl = new GradeBusinessLayer();
            List<Grade> ListOfGrades = gbl.Grades.ToList();

            DeptBusinessLayer dbl = new DeptBusinessLayer();
            List<Dept> ListOfDepts = dbl.Depts.ToList();

            SeasonBusinessLayer ebl = new SeasonBusinessLayer();
            List<Season> ListOfSeasons = ebl.Seasons.ToList();

            //no parameters recieved, pick the first ones
            if (GradeID == 0)
            {
                GradeID = ListOfGrades.Where(q => q.PreArticleGrade == 1).First().GradeID;
            }
            GradeName = ListOfGrades.Where(q => q.GradeID == GradeID).Single().GradeDescription;
            if (DeptID == 0)
            {
                DeptID = ListOfDepts.Where(q => q.DeptActive == 1).First().DeptID;
            }
            DeptName = ListOfDepts.Where(q => q.DeptID == DeptID).Single().DeptName;

            if (SeasonID == 0)
            {
                SeasonID = ListOfSeasons.Where(q => q.SeasonActive == 1).First().SeasonID;
            }


            MerchCatBusinessLayer mbl = new MerchCatBusinessLayer();
            List<MerchCat> ListOfMerchCats = mbl.MerchCats.Where(w => w.DeptID == DeptID).ToList();

            CatBusinessLayer cbl = new CatBusinessLayer();
            List<Cat> ListOfCats = cbl.Cats.Where(w => w.CatActive == 1).ToList();

            if (CatID == 0 && ListOfMerchCats.Any(q => q.MerchCatActive == 1))
            {
                CatID = ListOfMerchCats.Where(q => q.MerchCatActive == 1).First().CatID;
            }
            ListOfCats = ListOfCats.Join(ListOfMerchCats, a => a.CatID, b => b.CatID, (a, b) => new { a, b }).Select(z => z.a).Distinct().ToList();

            if (ListOfCats.Count > 0)
            {
                CatName = ListOfCats.Where(c => c.CatID == CatID).Single().CatDesc;
            }

            ListOfAvailableSpaces = ListOfAvailableSpaces.Where(z => z.SpaceGradeID == GradeID && z.SeasonID == SeasonID && z.CatID == CatID).ToList();

            #region StaticData
            ViewData["ddSeason"] = ListOfSeasons.Select(m => new SelectListItem { Value = m.SeasonID.ToString(), Text = m.SeasonDesc + " (" + m.SeasonID.ToString() + ")", Selected = m.SeasonID == SeasonID });
            ViewData["ddGrade"] = ListOfGrades.Select(m => new SelectListItem { Value = m.GradeID.ToString(), Text = m.GradeDescription, Selected = m.GradeID == GradeID });
            //move department selection to department index
            //ViewData["ddDept"] = ListOfDepts.Select(m => new SelectListItem { Value = m.DeptID.ToString(), Text = m.DeptName + " (" + m.DeptID.ToString() + ")", Selected = m.DeptID == DeptID });
            ViewData["ddCat"] = ListOfCats.Select(m => new SelectListItem { Value = m.CatID.ToString(), Text = m.CatDesc + " (" + m.CatID.ToString() + ")", Selected = m.CatID == CatID });

            ViewBag.DeptID = DeptID;
            ViewBag.DeptName = DeptName;

            ViewBag.CatID = CatID;
            ViewBag.CatName = CatName;
            ViewBag.GradeName = GradeName;
            ViewBag.SeasonID = SeasonID;

            #endregion

            return View(ListOfAvailableSpaces);
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            AvailableSpaceBusinessLayer sbl = new AvailableSpaceBusinessLayer();

            int SeasonID = Convert.ToInt32(collection["ddSeason"]);
            int GradeID = Convert.ToInt32(collection["ddGrade"]);
            int CatID = Convert.ToInt32(collection["ddCat"]);

            string DeptName;
            string CatName = "";


            List<AvailableSpace> ListOfAvailableSpaces = sbl.AvailableSpaces.ToList();

            GradeBusinessLayer gbl = new GradeBusinessLayer();
            List<Grade> ListOfGrades = gbl.Grades.ToList();
            string GradeName = ListOfGrades.Where(q => q.GradeID == GradeID).Single().GradeDescription;

            DeptBusinessLayer dbl = new DeptBusinessLayer();
            List<Dept> ListOfDepts = dbl.Depts.ToList();

            SeasonBusinessLayer ebl = new SeasonBusinessLayer();
            List<Season> ListOfSeasons = ebl.Seasons.ToList();

            MerchCatBusinessLayer mbl = new MerchCatBusinessLayer();
            List<MerchCat> ListOfMerchCats = mbl.MerchCats.ToList();

            CatBusinessLayer cbl = new CatBusinessLayer();
            List<Cat> ListOfCats = cbl.Cats.Where(w => w.CatActive == 1).ToList();

            int DeptID = ListOfMerchCats.Where(m => m.CatID == CatID).First().DeptID;
            DeptName = ListOfDepts.Where(q => q.DeptID == DeptID).Single().DeptName;

            if (ListOfCats.Count > 0)
            {
                CatName = ListOfCats.Where(c => c.CatID == CatID).Single().CatDesc;
            }

            #region StaticData

            ViewData["ddSeason"] = ListOfSeasons.Select(m => new SelectListItem { Value = m.SeasonID.ToString(), Text = m.SeasonDesc + " (" + m.SeasonID.ToString() + ")", Selected = m.SeasonID == SeasonID });
            ViewData["ddGrade"] = ListOfGrades.Select(m => new SelectListItem { Value = m.GradeID.ToString(), Text = m.GradeDescription, Selected = m.GradeID == GradeID });
            ViewData["ddCat"] = ListOfCats.Select(m => new SelectListItem { Value = m.CatID.ToString(), Text = m.CatDesc + " (" + m.CatID.ToString() + ")", Selected = m.CatID == CatID });

            ViewBag.DeptID = DeptID;
            ViewBag.DeptName = DeptName;
            ViewBag.CatID = CatID;
            ViewBag.CatName = CatName;
            ViewBag.GradeName = GradeName;
            ViewBag.SeasonID = SeasonID;

            ListOfAvailableSpaces = ListOfAvailableSpaces.Where(z => z.SpaceGradeID == GradeID && z.SeasonID == SeasonID && z.CatID == CatID).OrderBy(w => w.FixtureOrdinal).ToList();
            #endregion

            return View(ListOfAvailableSpaces);

        }

        // GET: AvailableSpace/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AvailableSpace/Create
        public ActionResult Create(int SeasonID)
        {
            GradeBusinessLayer gbl = new GradeBusinessLayer();
            List<Grade> ListOfGrades = gbl.Grades.ToList();

            SeasonBusinessLayer ebl = new SeasonBusinessLayer();
            List<Season> ListOfSeasons = ebl.Seasons.ToList();

            SpaceBusinessLayer sbl = new SpaceBusinessLayer();
            List<Space> ListOfSpaces = sbl.Spaces.ToList();

            CatBusinessLayer cbl = new CatBusinessLayer();
            List<Cat> ListOfCats = cbl.Cats.Where(w => w.CatActive == 1).ToList();

            ViewData["ddSeason"] = ListOfSeasons.Select(m => new SelectListItem { Value = m.SeasonID.ToString(), Text = m.SeasonDesc + " (" + m.SeasonID.ToString() + ")", Selected = m.SeasonID == SeasonID });
            ViewData["ddGrade"] = ListOfGrades.Select(m => new SelectListItem { Value = m.GradeID.ToString(), Text = m.GradeDescription });
            ViewData["ddCat"] = ListOfCats.Select(m => new SelectListItem { Value = m.CatID.ToString(), Text = m.CatDesc + " (" + m.CatID.ToString() + ")"});
            ViewData["ddSpace"] = ListOfSpaces.Select(m => new SelectListItem { Value = m.SpaceID.ToString(), Text = m.SpaceDesc + " (" + m.SpaceID.ToString() + ")",Selected = m.SpaceID==1 });

            return View();
        }

        // POST: AvailableSpace/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool DidItWork = false;
            try
            {
                AvailableSpace a = new AvailableSpace();
                a.SeasonID = Convert.ToInt32(collection["ddSeason"]);
                a.SpaceID = Convert.ToInt32(collection["ddSpace"]);
                a.CatID = Convert.ToInt32(collection["ddCat"]);
                a.SpaceGradeID = Convert.ToInt32(collection["ddGrade"]);
                //AvailableSpaceBusinessLayer bl = new AvailableSpaceBusinessLayer();
                //DidItWork = bl.ActionAvailableSpace(a, "Create", User.Identity.Name);
                StoredProcedureBusinessLayer spbl = new StoredProcedureBusinessLayer();
                DidItWork = spbl.ExecuteStoredProcedure(a, "Create", User.Identity.Name);
                if (DidItWork == false)
                {
                    //something went wrong trying to action the PreArticalSeasonald procedure
                    //return View();
                    return Content("Error on creation of Available Space. Press back to return and try again");
                }
                else
                {
                    //return View();
                    return RedirectToAction("Index", "AvailableSpace", new { SeasonID = a.SeasonID,CatID=a.CatID,SpaceID=a.SpaceID,GradeID=a.SpaceGradeID });
                }
            }
            catch (Exception x)
            {
                return View();
            }
        }

        // GET: AvailableSpace/Edit/5
        public ActionResult Edit(string id)
        {
            AvailableSpaceBusinessLayer sbl = new AvailableSpaceBusinessLayer();
            AvailableSpace sa = sbl.AvailableSpaces.Where(i => i.ID == id).Single();

            GradeBusinessLayer gbl = new GradeBusinessLayer();
            List<Grade> ListOfGrades = gbl.Grades.ToList();

            SeasonBusinessLayer ebl = new SeasonBusinessLayer();
            List<Season> ListOfSeasons = ebl.Seasons.ToList();

            SpaceBusinessLayer spbl = new SpaceBusinessLayer();
            List<Space> ListOfSpaces = spbl.Spaces.ToList();

            CatBusinessLayer cbl = new CatBusinessLayer();
            List<Cat> ListOfCats = cbl.Cats.Where(w => w.CatActive == 1).ToList();

            ViewData["ddSeason"] = ListOfSeasons.Select(m => new SelectListItem { Value = m.SeasonID.ToString(), Text = m.SeasonDesc + " (" + m.SeasonID.ToString() + ")", Selected = m.SeasonID == sa.SeasonID });
            ViewData["ddGrade"] = ListOfGrades.Select(m => new SelectListItem { Value = m.GradeID.ToString(), Text = m.GradeDescription, Selected = m.GradeID == sa.SpaceGradeID });
            ViewData["ddCat"] = ListOfCats.Select(m => new SelectListItem { Value = m.CatID.ToString(), Text = m.CatDesc + " (" + m.CatID.ToString() + ")", Selected = m.CatID == sa.CatID });
            ViewData["ddSpace"] = ListOfSpaces.Select(m => new SelectListItem { Value = m.SpaceID.ToString(), Text = m.SpaceDesc + " (" + m.SpaceID.ToString() + ")", Selected = m.SpaceID == sa.SpaceID });

            return View(sa);
        }

        // POST: AvailableSpace/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            bool DidItWork = false;
            try
            {
                AvailableSpaceBusinessLayer sbl = new AvailableSpaceBusinessLayer();
                AvailableSpace sa = sbl.AvailableSpaces.Where(i => i.ID == id).Single();
                sa.FixtureName = (string)collection["FixtureName"];

                StoredProcedureBusinessLayer spbl = new StoredProcedureBusinessLayer();
                DidItWork = spbl.ExecuteStoredProcedure(sa, "Edit", User.Identity.Name);

                if (DidItWork == false)
                {
                    return Content("Error on edit of available space. Press back to return and try again");
                }
                else
                {
                    return RedirectToAction("Index", new { SeasonID = sa.SeasonID, GradeID = sa.SpaceGradeID, CatID = sa.CatID });
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: AvailableSpace/Delete/5
        public ActionResult Delete(string id)
        {
            AvailableSpaceBusinessLayer sbl = new AvailableSpaceBusinessLayer();
            AvailableSpace sa = sbl.AvailableSpaces.Where(i => i.ID == id).Single();
            int MaxFO = sbl.AvailableSpaces.Where(i => i.SeasonID == sa.SeasonID && i.SpaceID == sa.SpaceID && i.SpaceGradeID == sa.SpaceGradeID && i.CatID==sa.CatID).Max(e => e.FixtureOrdinal);
            sa.FixtureOrdinal = MaxFO;
            string RedoneID = sa.ID;
            AvailableSpace saRedone = sbl.AvailableSpaces.Where(i => i.ID == RedoneID).Single();
            return View(saRedone);
        }

        // POST: AvailableSpace/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            bool DidItWork = false;
            try
            {
                AvailableSpaceBusinessLayer sbl = new AvailableSpaceBusinessLayer();
                AvailableSpace sa = sbl.AvailableSpaces.Where(i => i.ID == id).Single();

                StoredProcedureBusinessLayer spbl = new StoredProcedureBusinessLayer();
                DidItWork = spbl.ExecuteStoredProcedure(sa, "Edit", User.Identity.Name);
                if (DidItWork == false)
                {
                    return Content("Error on deletion of available space. Press back to return and try again");
                }
                else
                {
                    return RedirectToAction("Index", new { SeasonID = sa.SeasonID, GradeID = sa.SpaceGradeID, CatID = sa.CatID });
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
