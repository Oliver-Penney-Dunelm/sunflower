using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;

namespace sunflower.Controllers
{
    public class AttributeValuePivotController : Controller
    {
        // GET: AttributeValuePivot
        public ActionResult Index(int ViewID=0,int SeasonID=0)
        {
            #region StaticDropdowns
            SeasonBusinessLayer rbl = new SeasonBusinessLayer();
            List<Season> ListOfSeasons = rbl.Seasons.Where(r => r.SeasonActive == 1).OrderBy(o => o.FirstLaunchDate).ToList();
            
            PivotViewBusinessLayer tbl = new PivotViewBusinessLayer();
            List<PivotView> ListOfPivotViews = tbl.PivotViews(User.Identity.Name).OrderBy(o => o.ViewDesc).ToList();
            
            if (ViewID == 0 && ListOfPivotViews.Count>0)
            {
                ViewID = ListOfPivotViews.First().ViewID;
            }
            if (SeasonID == 0 && ListOfSeasons.Count > 0)
            {
                SeasonID = ListOfSeasons.First().SeasonID;
            }

            //work out what team the user is in
            //int TeamID = 0;
            //UserBusinessLayer ubl = new UserBusinessLayer();
            //if (ubl.Users.Any(o => o.NetworkID == User.Identity.Name))
            //{
            //    TeamID = ubl.Users.Where(o => o.NetworkID == User.Identity.Name).Single().TeamID;
            //}        

            ViewAttributePivotBusinessLayer vapbl = new ViewAttributePivotBusinessLayer();
            List<ViewAttributePivot> ListOfVAP = vapbl.ViewAttributePivots.Where(r => r.ViewID == ViewID).OrderBy(s=>s.PivotOrder).ToList();

            int AttributeCount = 1;

            foreach (ViewAttributePivot vap in ListOfVAP)
            {
                ViewData["att" + AttributeCount.ToString("00")] = vap.AttributeDescription;
                AttributeCount++;
                if (AttributeCount > 12)
                {
                    break;
                }
            }

            #endregion

            AttributeValuePivotBusinessLayer sbl = new AttributeValuePivotBusinessLayer();
            List<AttributeValuePivot> ListOfAttributeValuePivots = sbl.AttributeValuePivots(ViewID, SeasonID).OrderBy(i => i.SFID).ToList();

            ViewData["ddSeason"] = ListOfSeasons.Select(m => new SelectListItem { Value = m.SeasonID.ToString(), Text = m.SeasonDesc + " (" + m.SeasonID.ToString() + ")", Selected = m.SeasonID == SeasonID });
            ViewData["ddView"] = ListOfPivotViews.Select(m => new SelectListItem { Value = m.ViewID.ToString(), Text = m.ViewDesc + " (" + m.ViewID.ToString() + ")", Selected = m.ViewID == ViewID });
            ViewBag.SeasonID = SeasonID;
            return View(ListOfAttributeValuePivots);
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            int ViewID = Convert.ToInt32(collection["ddView"]);
            int SeasonID = Convert.ToInt32(collection["ddSeason"]);

            #region StaticDropdowns
            SeasonBusinessLayer rbl = new SeasonBusinessLayer();
            List<Season> ListOfSeasons = rbl.Seasons.Where(r => r.SeasonActive == 1).OrderBy(o => o.FirstLaunchDate).ToList();

            PivotViewBusinessLayer tbl = new PivotViewBusinessLayer();
            List<PivotView> ListOfPivotViews = tbl.PivotViews(User.Identity.Name).OrderBy(o => o.ViewDesc).ToList();

            if (ViewID == 0 && ListOfPivotViews.Count > 0)
            {
                ViewID = ListOfPivotViews.First().ViewID;
            }
            if (SeasonID == 0 && ListOfSeasons.Count > 0)
            {
                SeasonID = ListOfSeasons.First().SeasonID;
            }       

            ViewAttributePivotBusinessLayer vapbl = new ViewAttributePivotBusinessLayer();
            List<ViewAttributePivot> ListOfVAP = vapbl.ViewAttributePivots.Where(r => r.ViewID == ViewID).OrderBy(s => s.PivotOrder).ToList();

            int AttributeCount = 1;

            foreach (ViewAttributePivot vap in ListOfVAP)
            {
                ViewData["att" + AttributeCount.ToString("00")] = vap.AttributeDescription;
                AttributeCount++;
                if (AttributeCount > 12)
                {
                    break;
                }
            }

            #endregion
            AttributeValuePivotBusinessLayer sbl = new AttributeValuePivotBusinessLayer();
            List<AttributeValuePivot> ListOfAttributeValuePivots = sbl.AttributeValuePivots(ViewID, SeasonID).OrderBy(i => i.SFID).ToList();

            ViewData["ddSeason"] = ListOfSeasons.Select(m => new SelectListItem { Value = m.SeasonID.ToString(), Text = m.SeasonDesc + " (" + m.SeasonID.ToString() + ")", Selected = m.SeasonID == SeasonID });
            ViewData["ddView"] = ListOfPivotViews.Select(m => new SelectListItem { Value = m.ViewID.ToString(), Text = m.ViewDesc + " (" + m.ViewID.ToString() + ")", Selected = m.ViewID == ViewID });

            #region Filter
            string FilterMessage = "";
            string SapArticleID_f = (string)(collection["SapArticleID_f"]);
            string MasterDescription_f = (string)(collection["MasterDescription_f"]);
            string DeptName_f = (string)(collection["DeptName_f"]);
            string CatDesc_f = (string)(collection["CatDesc_f"]);
            string MerchCatDesc_f = (string)(collection["MerchCatDesc_f"]);
            string VendorDesc_f = (string)(collection["VendorDesc_f"]);
            string ProposedPreArticleGradeDesc_f = (string)(collection["ProposedPreArticleGradeDesc_f"]);
            string ConfirmedPreArticleGradeDesc_f = (string)(collection["ConfirmedPreArticleGradeDesc_f"]);

            string[] AttributeFilter = new string[18];
            for(int i = 1; i <= 18; i++)
            {
                AttributeFilter[i-1]= (string)(collection["Att" + i.ToString("00") + "Fragment"]);
            }

            if (SapArticleID_f!="")
            {
                ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.SapArticleID.ToString().Contains(SapArticleID_f)).ToList();
                FilterMessage = " filtered by Sap Article";
            }
            if (MasterDescription_f != "")
            {
                ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.MasterDescription.ToString().Contains(MasterDescription_f)).ToList();
                FilterMessage = FilterMessage + " filtered by Master Description";
            }
            if (DeptName_f != "")
            {
                ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.DeptName.ToString().Contains(DeptName_f)).ToList();
                FilterMessage = FilterMessage + " filtered by Dept Name";
            }
            if (CatDesc_f != "")
            {
                ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.CatDesc.ToString().Contains(CatDesc_f)).ToList();
                FilterMessage = FilterMessage + " filtered by Cat Name";
            }
            if (MerchCatDesc_f != "")
            {
                ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.MerchCatDesc.ToString().Contains(MerchCatDesc_f)).ToList();
                FilterMessage = " filtered by Merch Cat";
            }
            if (VendorDesc_f != "")
            {
                ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.VendorDesc.ToString().Contains(VendorDesc_f)).ToList();
                FilterMessage = FilterMessage + " filtered by Vendor Description";
            }
            if (ProposedPreArticleGradeDesc_f != "")
            {
                ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.ProposedPreArticleGradeDesc.ToString().Contains(ProposedPreArticleGradeDesc_f)).ToList();
                FilterMessage = FilterMessage + " filtered by Proposed Grade";
            }
            if (ConfirmedPreArticleGradeDesc_f != "")
            {
                ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.ConfirmedPreArticleGradeDesc.ToString().Contains(ConfirmedPreArticleGradeDesc_f)).ToList();
                FilterMessage = FilterMessage + " filtered by Confirmed Grade";
            }
            for (int i = 1; i <= 18; i++)
            {
                if (AttributeFilter[i-1] != "")
                {
                    //ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.Att01.ToString().Contains(AttributeFilter[0])).ToList();
                    ListOfAttributeValuePivots = sbl.AttributeValuePivotWhereQuery(ListOfAttributeValuePivots, "Att" + i.ToString("00"), AttributeFilter[i-1]).ToList();
                    FilterMessage = FilterMessage + " filtered by attribute " + i.ToString("00");
                }
            }   

            ViewBag.FilterMessage = FilterMessage;
            ViewBag.SapArticleID_f = SapArticleID_f;
            ViewBag.MasterDescription_f = MasterDescription_f;
            ViewBag.DeptName_f = DeptName_f;
            ViewBag.CatDesc_f = CatDesc_f;
            ViewBag.MerchCatDesc_f = MerchCatDesc_f;
            ViewBag.VendorDesc_f = VendorDesc_f;
            ViewBag.ProposedPreArticleGradeDesc_f = ProposedPreArticleGradeDesc_f;
            ViewBag.ConfirmedPreArticleGradeDesc_f = ConfirmedPreArticleGradeDesc_f;
            for (int i = 1; i <= 18; i++)
            {
                ViewData["Att" + i.ToString("00") + "Fragment"] = AttributeFilter[i - 1];
            }
            #endregion


            return View(ListOfAttributeValuePivots);

        }

        // GET: AttributeValuePivot/Details/5
        public ActionResult Details(int id)
        {
            
            return View();
        }

        // GET: AttributeValuePivot/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AttributeValuePivot/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AttributeValuePivot/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AttributeValuePivot/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AttributeValuePivot/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AttributeValuePivot/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
