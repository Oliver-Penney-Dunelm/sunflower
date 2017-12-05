using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;

namespace sunflower.Controllers
{
    public class PreArticleSeasonalController : Controller
    {
        // GET: PreArticleSeasonal
        public ActionResult Index(int SeasonID = 0)
        {
            
            PreArticleSeasonalBusinessLayer sbl = new PreArticleSeasonalBusinessLayer();
            List<PreArticleSeasonal> ListOfPreArticleSeasonals = sbl.PreArticleSeasonals.OrderBy(i => i.SFID).ToList();
            if (SeasonID!=0)
            {
                ListOfPreArticleSeasonals = ListOfPreArticleSeasonals.Where(j => j.SeasonID == SeasonID).ToList();
            }
          
            return View(ListOfPreArticleSeasonals);
        }

        // GET: PreArticleSeasonal/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PreArticleSeasonal/Create
        public ActionResult Create(int SeasonID = 0)
        {
            SeasonBusinessLayer sbl = new SeasonBusinessLayer();
            List<Season> ListOfSeasons = sbl.Seasons.Where(r => r.SeasonActive == 1).OrderBy(o => o.FirstLaunchDate).ToList();
            ViewData["ddSeason"] = ListOfSeasons.Select(m => new SelectListItem { Value = m.SeasonID.ToString(), Text = m.SeasonDesc + " (" + m.SeasonID.ToString() + ")", Selected = m.SeasonID == SeasonID });

            VendorBusinessLayer vbl = new VendorBusinessLayer();
            List<Vendor> ListOfVendors = vbl.Vendors.OrderBy(o => o.VendorID).ToList();

            GradeBusinessLayer gbl = new GradeBusinessLayer();
            List<Grade> ListOfGrades = gbl.Grades.Where(z => z.PreArticleGrade == 1).OrderBy(o => o.GradeOrder).ToList();

            MerchCatBusinessLayer mbl = new MerchCatBusinessLayer();
            List<MerchCat> ListOfMerchCats = mbl.MerchCats.OrderBy(o => o.MerchCatID).ToList();

            ViewData["ddGradeP"] = ListOfGrades.Select(m => new SelectListItem { Value = m.GradeID.ToString(), Text = m.GradeDescription });
            ViewData["ddGradeC"] = ListOfGrades.Select(m => new SelectListItem { Value = m.GradeID.ToString(), Text = m.GradeDescription });
            ViewData["ddMerchCat"] = ListOfMerchCats.Select(m => new SelectListItem { Value = m.MerchCatID.ToString(), Text = m.MerchCatDesc + " (" + m.MerchCatID.ToString() + ")" });
            ViewData["ddVendor"] = ListOfVendors.Select(m => new SelectListItem { Value = m.VendorID, Text = m.VendorDesc + " (" + m.VendorID + ")" });
            ViewBag.SeasonID = SeasonID;
            return View();
        }

        // POST: PreArticleSeasonal/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool DidItWork = false;
            try
            {
                PreArticleSeasonal SomePreArticalSeasonal = new PreArticleSeasonal();
                SomePreArticalSeasonal.SFID = Convert.ToInt32(collection["SFID"]);
                SomePreArticalSeasonal.SunflowerDesc = (string)collection["SunflowerDesc"];
                SomePreArticalSeasonal.PlmID = (string)collection["PlmID"];
                SomePreArticalSeasonal.PlmDescription = (string)collection["PlmDescription"];
                SomePreArticalSeasonal.SapArticleID = (string)collection["SapArticleID"];
                SomePreArticalSeasonal.SapDescription = (string)collection["SapDescription"];
                SomePreArticalSeasonal.SeasonID = Convert.ToInt32(collection["ddSeason"]);
                SomePreArticalSeasonal.ProposedPreArticleGradeID = Convert.ToInt32(collection["ddGradeP"]);
                SomePreArticalSeasonal.ConfirmedPreArticleGradeID = Convert.ToInt32(collection["ddGradeC"]);
                SomePreArticalSeasonal.MerchCatID = Convert.ToInt32(collection["ddMerchCat"]);
                SomePreArticalSeasonal.VendorID = (string)collection["ddVendor"];

                PreArticleSeasonalBusinessLayer bl = new PreArticleSeasonalBusinessLayer();
                DidItWork = bl.ActionPreArticleSeasonal(SomePreArticalSeasonal,"Create", User.Identity.Name);
                if (DidItWork == false)
                {
                    //something went wrong trying to action the PreArticalSeasonald procedure
                    //return View();
                    return Content("Error on creation of PreArticalSeasonal. Press back to return and try again");
                }
                else
                {
                    //return View();
                    return RedirectToAction("Index","AttributeValuePivot", new { SeasonID = SomePreArticalSeasonal.SeasonID });
                }
            }
            catch(Exception x)
            {
                return View();
            }
        }

        // GET: PreArticleSeasonal/Edit/5
        public ActionResult Edit(string id)
        {
            #region StaticData
            SeasonBusinessLayer sbl = new SeasonBusinessLayer();
            List<Season> ListOfSeasons = sbl.Seasons.Where(r => r.SeasonActive == 1).OrderBy(o => o.FirstLaunchDate).ToList();
            
            VendorBusinessLayer vbl = new VendorBusinessLayer();
            List<Vendor> ListOfVendors = vbl.Vendors.OrderBy(o => o.VendorID).ToList();

            GradeBusinessLayer gbl = new GradeBusinessLayer();
            List<Grade> ListOfGrades = gbl.Grades.Where(z => z.PreArticleGrade == 1).OrderBy(o => o.GradeOrder).ToList();

            MerchCatBusinessLayer mbl = new MerchCatBusinessLayer();
            List<MerchCat> ListOfMerchCats = mbl.MerchCats.OrderBy(o => o.MerchCatID).ToList();

            
            #endregion

            PreArticleSeasonalBusinessLayer pbl = new PreArticleSeasonalBusinessLayer();
            PreArticleSeasonal SomePreArticleSeasonal = pbl.PreArticleSeasonals.Where(i => i.ID == id).Single();

            ViewData["ddSeason"] = ListOfSeasons.Select(m => new SelectListItem { Value = m.SeasonID.ToString(), Text = m.SeasonDesc + " (" + m.SeasonID.ToString() + ")", Selected = m.SeasonID == SomePreArticleSeasonal.SeasonID });
            ViewData["ddGradeP"] = ListOfGrades.Select(m => new SelectListItem { Value = m.GradeID.ToString(), Text = m.GradeDescription, Selected = m.GradeID == SomePreArticleSeasonal.ProposedPreArticleGradeID });
            ViewData["ddGradeC"] = ListOfGrades.Select(m => new SelectListItem { Value = m.GradeID.ToString(), Text = m.GradeDescription, Selected = m.GradeID == SomePreArticleSeasonal.ConfirmedPreArticleGradeID });
            ViewData["ddMerchCat"] = ListOfMerchCats.Select(m => new SelectListItem { Value = m.MerchCatID.ToString(), Text = m.MerchCatDesc + " (" + m.MerchCatID.ToString() + ")", Selected = m.MerchCatID == SomePreArticleSeasonal.MerchCatID });
            ViewData["ddVendor"] = ListOfVendors.Select(m => new SelectListItem { Value = m.VendorID, Text = m.VendorDesc + " (" + m.VendorID + ")", Selected = m.VendorID == SomePreArticleSeasonal.VendorID });


            ViewBag.SeasonID = SomePreArticleSeasonal.SeasonID;
            return View(SomePreArticleSeasonal);
        }

        // POST: PreArticleSeasonal/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            bool DidItWork = false;
            try
            {
                PreArticleSeasonal SomePreArticalSeasonal = new PreArticleSeasonal();
                SomePreArticalSeasonal.SFID = Convert.ToInt32(collection["SFID"]);
                SomePreArticalSeasonal.SunflowerDesc = (string)collection["SunflowerDesc"];
                SomePreArticalSeasonal.PlmID = (string)collection["PlmID"];
                SomePreArticalSeasonal.PlmDescription = (string)collection["PlmDescription"];
                SomePreArticalSeasonal.SapArticleID = (string)collection["SapArticleID"];
                SomePreArticalSeasonal.SapDescription = (string)collection["SapDescription"];
                SomePreArticalSeasonal.SeasonID = Convert.ToInt32(collection["ddSeason"]);
                SomePreArticalSeasonal.ProposedPreArticleGradeID = Convert.ToInt32(collection["ddGradeP"]);
                SomePreArticalSeasonal.ConfirmedPreArticleGradeID = Convert.ToInt32(collection["ddGradeC"]);
                SomePreArticalSeasonal.MerchCatID = Convert.ToInt32(collection["ddMerchCat"]);
                SomePreArticalSeasonal.VendorID = (string)collection["ddVendor"];

                PreArticleSeasonalBusinessLayer bl = new PreArticleSeasonalBusinessLayer();
                DidItWork = bl.ActionPreArticleSeasonal(SomePreArticalSeasonal, "Edit", User.Identity.Name);
                if (DidItWork == false)
                {
                    //something went wrong trying to action the PreArticalSeasonald procedure
                    //return View();
                    return Content("Error on edit of PreArticalSeasonal. Press back to return and try again");
                }
                else
                {
                    //return View();
                    return RedirectToAction("Index", "AttributeValuePivot", new { SeasonID = SomePreArticalSeasonal.SeasonID });
                }
            }
            catch (Exception x)
            {
                return View();
            }
        }

        // GET: PreArticleSeasonal/Delete/5
        public ActionResult Delete(string id)
        {
            PreArticleSeasonalBusinessLayer pbl = new PreArticleSeasonalBusinessLayer();
            PreArticleSeasonal SomePreArticleSeasonal = pbl.PreArticleSeasonals.Where(i => i.ID == id).Single();
            ViewBag.SeasonID = SomePreArticleSeasonal.SeasonID;
            return View(SomePreArticleSeasonal);
        }

        // POST: PreArticleSeasonal/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            bool DidItWork = false;
            try
            {
                PreArticleSeasonalBusinessLayer sbl = new PreArticleSeasonalBusinessLayer();
                PreArticleSeasonal r = sbl.PreArticleSeasonals.Where(q => q.ID == id).Single();

                DidItWork = sbl.ActionPreArticleSeasonal(r, "Delete",User.Identity.Name);
                if (DidItWork == false)
                {
                    return Content("Error on deletion of PreArticleSeasonal. Press back to return and try again");
                }
                else
                {
                    return RedirectToAction("Index", "AttributeValuePivot", new { SeasonID = r.SeasonID });
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
