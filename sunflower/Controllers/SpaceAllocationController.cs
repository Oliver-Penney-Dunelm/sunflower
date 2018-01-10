using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;

namespace sunflower.Controllers
{
    public class SpaceAllocationController : Controller
    {
        // GET: SpaceAllocation
        public ActionResult Index(int DeptID = 0, int SeasonID = 0, int GradeID = 0, int CatID=0,int FO=0)
        {
            string DeptName="";
            string CatName="";
            string GradeName="";

            SpaceAllocationBusinessLayer sbl = new SpaceAllocationBusinessLayer();
            List<SpaceAllocation> ListOfSpaceAllocations = sbl.SpaceAllocations.ToList();

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
            DeptName=ListOfDepts.Where(q => q.DeptID== DeptID).Single().DeptName;

            if (SeasonID == 0)
            {
                SeasonID = ListOfSeasons.Where(q => q.SeasonActive == 1).First().SeasonID;
            }
            
            
            MerchCatBusinessLayer mbl = new MerchCatBusinessLayer();
            List<MerchCat> ListOfMerchCats = mbl.MerchCats.Where(w=>w.DeptID==DeptID).ToList();

            CatBusinessLayer cbl = new CatBusinessLayer();
            List<Cat> ListOfCats = cbl.Cats.Where(w => w.CatActive == 1).ToList();

            if (CatID == 0 && ListOfMerchCats.Any(q => q.MerchCatActive == 1))
            {
                CatID = ListOfMerchCats.Where(q => q.MerchCatActive == 1).First().CatID;
            }
            ListOfCats = ListOfCats.Join(ListOfMerchCats, a => a.CatID, b => b.CatID, (a, b) => new { a, b }).Select(z => z.a).Distinct().ToList();

            if (ListOfCats.Count>0)
            {
                CatName = ListOfCats.Where(c => c.CatID == CatID).Single().CatDesc;
            }

            //ListOfSpaceAllocations = ListOfSpaceAllocations.Join(ListOfMerchCats, a => a.CatID, b => b.CatID, (a, b) => new { a, b }).Select(z => z.a).ToList();

            ListOfSpaceAllocations = ListOfSpaceAllocations.Where(z => z.SpaceGradeID == GradeID && z.SeasonID == SeasonID && z.CatID== CatID).ToList();

            List<SelectListItem> ListofSelects = new List<SelectListItem>();
            ListofSelects = ListOfSpaceAllocations.Select(m => new SelectListItem { Value = m.FixtureOrdinal.ToString(), Text = m.FixtureName + " (" + m.FixtureOrdinal.ToString() + ")", Selected = m.FixtureOrdinal == FO }).ToList();
            IEnumerable<SelectListItem> ListofSelects2  = ListofSelects;
            ListofSelects = ListofSelects.GroupBy(p => p.Value).Select(g => g.First()).ToList();

            if (FO==0 && ListOfSpaceAllocations.Count>0)
            {
                FO = ListOfSpaceAllocations.Min(a => a.FixtureOrdinal);
            }
            ListOfSpaceAllocations = ListOfSpaceAllocations.Where(z => z.FixtureOrdinal == FO).OrderBy(w=>w.OrdinalSequence).ToList();
            string FixtureName = "";
            if (ListOfSpaceAllocations.Count > 0)
            {
                FixtureName = ListOfSpaceAllocations.First().FixtureName;
            }

            #region StaticData
            ViewData["ddSeason"] = ListOfSeasons.Select(m => new SelectListItem { Value = m.SeasonID.ToString(), Text = m.SeasonDesc + " (" + m.SeasonID.ToString() + ")", Selected = m.SeasonID == SeasonID });
            ViewData["ddGrade"] = ListOfGrades.Select(m => new SelectListItem { Value = m.GradeID.ToString(), Text = m.GradeDescription , Selected = m.GradeID == GradeID });
            ViewData["ddDept"] = ListOfDepts.Select(m => new SelectListItem { Value = m.DeptID.ToString(), Text = m.DeptName + " (" + m.DeptID.ToString() + ")", Selected = m.DeptID == DeptID });
            ViewData["ddCat"] = ListOfCats.Select(m => new SelectListItem { Value = m.CatID.ToString(), Text = m.CatDesc + " (" + m.CatID.ToString() + ")", Selected = m.CatID == CatID });
            
            ViewData["ddFO"]= ListofSelects;

            ViewBag.DeptID = DeptID;
            ViewBag.DeptName = DeptName;

            ViewBag.CatID = CatID;
            ViewBag.CatName = CatName;
            ViewBag.GradeName = GradeName;
            ViewBag.FixtureName = FixtureName;

            #endregion

            return View(ListOfSpaceAllocations);
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection, string submitButton, IEnumerable<string> keys, IEnumerable<decimal> amounts)
        {
            SpaceAllocationBusinessLayer sbl = new SpaceAllocationBusinessLayer();

            if (submitButton== "Retazz LocationID" && keys !=null)
            {
                //read the values for retazz and attempt to realign
                for(int i = 0; i < keys.Count(); i++)
                {
                    int NewOrder;
                    NewOrder = i + 1000 * Convert.ToInt32(amounts.ElementAt(i));
                    if (sbl.SpaceAllocations.Where(y => y.ID == keys.ElementAt(i) && y.AutoID != 0).Any())
                    {
                        SpaceAllocation LoopSA = sbl.SpaceAllocations.Where(y => y.ID == keys.ElementAt(i) && y.AutoID != 0).Single();
                        LoopSA.OrdinalSequence = NewOrder;
                        StoredProcedureBusinessLayer spbl = new StoredProcedureBusinessLayer();
                        Boolean DidUpdateWork = spbl.ExecuteStoredProcedure(LoopSA, "Edit", User.Identity.Name);
                        //Boolean DidUpdateWork = sbl.ActionSpaceAllocation(LoopSA, "Edit", User.Identity.Name);
                    }
                }
                Boolean DidRetazzWork=sbl.RetazzSpaceAllocation();

            }
            int DeptID = Convert.ToInt32(collection["ddDept"]);
            int SeasonID = Convert.ToInt32(collection["ddSeason"]);
            int GradeID = Convert.ToInt32(collection["ddGrade"]);
            int CatID = Convert.ToInt32(collection["ddCat"]);
            int FO = Convert.ToInt32(collection["ddFO"]);

            string DeptName;
            string CatName = "";

            
            List<SpaceAllocation> ListOfSpaceAllocations = sbl.SpaceAllocations.ToList();

            GradeBusinessLayer gbl = new GradeBusinessLayer();
            List<Grade> ListOfGrades = gbl.Grades.ToList();
            string GradeName = ListOfGrades.Where(q => q.GradeID == GradeID).Single().GradeDescription;

            DeptBusinessLayer dbl = new DeptBusinessLayer();
            List<Dept> ListOfDepts = dbl.Depts.ToList();

            SeasonBusinessLayer ebl = new SeasonBusinessLayer();
            List<Season> ListOfSeasons = ebl.Seasons.ToList();

            MerchCatBusinessLayer mbl = new MerchCatBusinessLayer();
            List<MerchCat> ListOfMerchCats = mbl.MerchCats.Where(w => w.DeptID == DeptID).ToList();

            CatBusinessLayer cbl = new CatBusinessLayer();
            List<Cat> ListOfCats = cbl.Cats.Where(w => w.CatActive == 1).ToList();
            List<Cat> ListOfCatsForDept = ListOfCats.Join(ListOfMerchCats, a => a.CatID, b => b.CatID, (a, b) => new { a, b }).Select(z => z.a).Distinct().ToList();

            if (ListOfCatsForDept.Count==0)
            {
                CatID = 0;
                FO = 0;
            }
            else
            {
                if (ListOfCatsForDept.Any(c => c.CatID == CatID) == false)
                {
                    CatID = ListOfCatsForDept.First().CatID;
                    CatName = ListOfCatsForDept.Where(c => c.CatID == CatID).Single().CatDesc;
                }
            }

            DeptName = ListOfDepts.Where(q => q.DeptID == DeptID).Single().DeptName;

            #region StaticData

            ViewData["ddSeason"] = ListOfSeasons.Select(m => new SelectListItem { Value = m.SeasonID.ToString(), Text = m.SeasonDesc + " (" + m.SeasonID.ToString() + ")", Selected = m.SeasonID == SeasonID });
            ViewData["ddGrade"] = ListOfGrades.Select(m => new SelectListItem { Value = m.GradeID.ToString(), Text = m.GradeDescription, Selected = m.GradeID == GradeID });
            ViewData["ddCat"] = ListOfCatsForDept.Select(m => new SelectListItem { Value = m.CatID.ToString(), Text = m.CatDesc + " (" + m.CatID.ToString() + ")", Selected = m.CatID == CatID });
            ViewData["ddDept"] = ListOfDepts.Select(m => new SelectListItem { Value = m.DeptID.ToString(), Text = m.DeptName + " (" + m.DeptID.ToString() + ")", Selected = m.DeptID == DeptID });

            ViewBag.DeptID = DeptID;
            ViewBag.DeptName = DeptName;

            ViewBag.CatID = CatID;
            ViewBag.CatName = CatName;

            ViewBag.GradeName = GradeName;

            ListOfSpaceAllocations = ListOfSpaceAllocations.Where(z => z.SpaceGradeID == GradeID && z.SeasonID == SeasonID && z.CatID == CatID).OrderBy(w => w.OrdinalSequence).ToList();

            if (FO == 0 && ListOfSpaceAllocations.Count>0)
            {
                FO = ListOfSpaceAllocations.First().FixtureOrdinal;
            }

            List<SelectListItem> ListofSelects = new List<SelectListItem>();
            ListofSelects = ListOfSpaceAllocations.Select(m => new SelectListItem { Value = m.FixtureOrdinal.ToString(), Text = m.FixtureName + " (" + m.FixtureOrdinal.ToString() + ")", Selected = m.FixtureOrdinal == FO }).ToList();
            IEnumerable<SelectListItem> ListofSelects2 = ListofSelects;
            ListofSelects = ListofSelects.GroupBy(p => p.Value).Select(g => g.First()).ToList();

            #endregion

            ListOfSpaceAllocations = ListOfSpaceAllocations.Where(z=>z.FixtureOrdinal == FO).ToList();
            string FixtureName = "";
            if (ListOfSpaceAllocations.Count > 0)
            {
                FixtureName = ListOfSpaceAllocations.First().FixtureName;
            }

            //fixture ordinals are slightly different to the other drop downs...they are made from the existing data
            ViewData["ddFO"] = ListofSelects;
            ViewBag.FixtureName = FixtureName;

            return View(ListOfSpaceAllocations);

        }

        // GET: SpaceAllocation/Edit/5
        public ActionResult Edit(string id)
        {
            SpaceAllocationBusinessLayer sbl = new SpaceAllocationBusinessLayer();
            SpaceAllocation sa = sbl.SpaceAllocations.Where(i=>i.ID==id).Single();

            MeTypeBusinessLayer m = new MeTypeBusinessLayer();
            List<MeType> ListOfMeTypes = m.MeTypes.ToList();

            ViewData["ddMeType"] = ListOfMeTypes.Select(k => new SelectListItem { Value = k.MeTypeID.ToString(), Text = k.MeTypeDesc,Selected=k.MeTypeID==sa.MeTypeID });

            PreArticleSeasonalBusinessLayer pbl = new PreArticleSeasonalBusinessLayer();
            List<PreArticleSeasonal> ListOfPreArticleSeasonals = pbl.PreArticleSeasonals.Where(j => j.SeasonID == sa.SeasonID || j.SFID == sa.SFID).ToList();

            ViewData["ddPAS"] = ListOfPreArticleSeasonals.Select(k => new SelectListItem { Value = k.SFID.ToString(), Text = k.SunflowerDesc + " (" + k.SFID + ")", Selected = k.SFID == sa.SFID });

            return View(sa);
        }

        // POST: SpaceAllocation/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            bool DidItWork = false;
            try
            {
                SpaceAllocationBusinessLayer sbl = new SpaceAllocationBusinessLayer();
                SpaceAllocation sa = sbl.SpaceAllocations.Where(i => i.ID == id).Single();

                sa.Fill = Convert.ToInt32(collection["Fill"]);
                sa.DisplayQuantity = Convert.ToInt32(collection["DisplayQuantity"]);
                sa.SFID = Convert.ToInt32(collection["ddPas"]);
                sa.MeTypeID = Convert.ToInt32(collection["ddMeType"]);

                StoredProcedureBusinessLayer spbl = new StoredProcedureBusinessLayer();
                DidItWork = spbl.ExecuteStoredProcedure(sa, "upsert", User.Identity.Name);
                //DidItWork = sbl.ActionSpaceAllocation(sa,"upsert", User.Identity.Name);
                if (DidItWork == false)
                {
                    return Content("Error on edit of space allocation. Press back to return and try again");
                }
                else
                {
                    return RedirectToAction("Index", new { SeasonID = sa.SeasonID, GradeID = sa.SpaceGradeID, CatID = sa.CatID, FO = sa.FixtureOrdinal });
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: SpaceAllocation/Edit/5
        public ActionResult AddNew(string id)
        {
            SpaceAllocationBusinessLayer sbl = new SpaceAllocationBusinessLayer();
            SpaceAllocation sa = sbl.SpaceAllocations.Where(i => i.ID == id).Single();

            int MaxOrdinalSequence = sbl.SpaceAllocations.Where(t => t.SeasonID == sa.SeasonID && t.SpaceID == sa.SpaceID && t.CatID == sa.CatID && t.SpaceGradeID == sa.SpaceGradeID && t.FixtureOrdinal == sa.FixtureOrdinal).Max(s => s.OrdinalSequence);

            sa.OrdinalSequence = MaxOrdinalSequence + 1;

            MeTypeBusinessLayer m = new MeTypeBusinessLayer();
            List<MeType> ListOfMeTypes = m.MeTypes.ToList();

            ViewData["ddMeType"] = ListOfMeTypes.Select(k => new SelectListItem { Value = k.MeTypeID.ToString(), Text = k.MeTypeDesc, Selected = k.MeTypeID == sa.MeTypeID });

            PreArticleSeasonalBusinessLayer pbl = new PreArticleSeasonalBusinessLayer();
            List<PreArticleSeasonal> ListOfPreArticleSeasonals = pbl.PreArticleSeasonals.Where(j => j.SeasonID == sa.SeasonID || j.SFID==sa.SFID).ToList();

            ViewData["ddPAS"] = ListOfPreArticleSeasonals.Select(k => new SelectListItem { Value = k.SFID.ToString(), Text = k.SunflowerDesc + " (" + k.SFID + ")", Selected = k.SFID == sa.SFID });

            return View(sa);
        }

        // POST: SpaceAllocation/Edit/5
        [HttpPost]
        public ActionResult AddNew(string id, FormCollection collection)
        {
            bool DidItWork = false;
            try
            {
                SpaceAllocationBusinessLayer sbl = new SpaceAllocationBusinessLayer();
                SpaceAllocation sa = sbl.SpaceAllocations.Where(i => i.ID == id).Single();
                //SpaceAllocation sa = new SpaceAllocation();

                
                sa.Fill = Convert.ToInt32(collection["Fill"]);
                sa.DisplayQuantity = Convert.ToInt32(collection["DisplayQuantity"]);
                sa.SFID = Convert.ToInt32(collection["ddPas"]);
                sa.MeTypeID = Convert.ToInt32(collection["ddMeType"]);
                sa.OrdinalSequence = Convert.ToInt32(collection["OrdinalSequence"]);

                StoredProcedureBusinessLayer spbl = new StoredProcedureBusinessLayer();
                DidItWork = spbl.ExecuteStoredProcedure(sa, "insert", User.Identity.Name);
                //DidItWork = sbl.ActionSpaceAllocation(sa, "insert", User.Identity.Name);
                if (DidItWork == false)
                {
                    return Content("Error on edit of space allocation. Press back to return and try again");
                }
                else
                {
                    return RedirectToAction("Index", new { SeasonID = sa.SeasonID, GradeID = sa.SpaceGradeID, CatID = sa.CatID, FO = sa.FixtureOrdinal });
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: SpaceAllocation/Delete/5
        public ActionResult Delete(int Autoid)
        {
            SpaceAllocationBusinessLayer sbl = new SpaceAllocationBusinessLayer();
            SpaceAllocation sa = sbl.SpaceAllocations.Where(i => i.AutoID == Autoid).Single();
            return View(sa);
        }

        // POST: SpaceAllocation/Delete/5
        [HttpPost]
        public ActionResult Delete(int Autoid, FormCollection collection)
        {
            bool DidItWork = false;
            try
            {
                SpaceAllocationBusinessLayer sbl = new SpaceAllocationBusinessLayer();
                SpaceAllocation sa = sbl.SpaceAllocations.Where(i => i.AutoID == Autoid).Single();

                StoredProcedureBusinessLayer spbl = new StoredProcedureBusinessLayer();
                DidItWork = spbl.ExecuteStoredProcedure(sa, "delete", User.Identity.Name);
                //DidItWork = sbl.ActionSpaceAllocation(sa, "delete", User.Identity.Name);
                if (DidItWork == false)
                {
                    return Content("Error on deletion of space allocation. Press back to return and try again");
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
