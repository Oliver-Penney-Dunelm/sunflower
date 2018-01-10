using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;
using System.Web.Helpers;


namespace sunflower.Controllers
{
    public class AttributeValuePivotController : Controller
    {
        //// GET: AttributeValuePivot
        //public ActionResult Index(int ViewID=0,int SeasonID=0,int DeptID=0)
        //{
        //    #region StaticDropdowns
        //    SeasonBusinessLayer rbl = new SeasonBusinessLayer();
        //    List<Season> ListOfSeasons = rbl.Seasons.Where(r => r.SeasonActive == 1).OrderBy(o => o.FirstLaunchDate).ToList();
            
        //    PivotViewBusinessLayer tbl = new PivotViewBusinessLayer();
        //    List<PivotView> ListOfPivotViews = tbl.PivotViews(User.Identity.Name).OrderBy(o => o.ViewDesc).ToList();
            
        //    if (ViewID == 0 && ListOfPivotViews.Count>0)
        //    {
        //        ViewID = ListOfPivotViews.First().ViewID;
        //    }
        //    if (SeasonID == 0 && ListOfSeasons.Count > 0)
        //    {
        //        SeasonID = ListOfSeasons.First().SeasonID;
        //    }

        //    //work out what team the user is in
        //    //int TeamID = 0;
        //    //UserBusinessLayer ubl = new UserBusinessLayer();
        //    //if (ubl.Users.Any(o => o.NetworkID == User.Identity.Name))
        //    //{
        //    //    TeamID = ubl.Users.Where(o => o.NetworkID == User.Identity.Name).Single().TeamID;
        //    //}        

        //    ViewAttributePivotBusinessLayer vapbl = new ViewAttributePivotBusinessLayer();
        //    List<ViewAttributePivot> ListOfVAP = vapbl.ViewAttributePivots.Where(r => r.ViewID == ViewID).OrderBy(s=>s.PivotOrder).ToList();

        //    int AttributeCount = 1;

        //    foreach (ViewAttributePivot vap in ListOfVAP)
        //    {
        //        ViewData["att" + AttributeCount.ToString("00")] = vap.AttributeDescription;
        //        AttributeCount++;
        //        if (AttributeCount > 12)
        //        {
        //            break;
        //        }
        //    }

        //    DeptBusinessLayer dbl = new DeptBusinessLayer();
        //    List<Dept> ListOfDepts = dbl.Depts.ToList();
        //    string DeptName = ListOfDepts.Where(q => q.DeptID == DeptID).Single().DeptName;

        //    #endregion

        //    AttributeValuePivotBusinessLayer sbl = new AttributeValuePivotBusinessLayer();
        //    List<AttributeValuePivot> ListOfAttributeValuePivots = sbl.AttributeValuePivots(DeptID, ViewID, SeasonID).OrderBy(i => i.SFID).ToList();

        //    ViewData["ddSeason"] = ListOfSeasons.Select(m => new SelectListItem { Value = m.SeasonID.ToString(), Text = m.SeasonDesc + " (" + m.SeasonID.ToString() + ")", Selected = m.SeasonID == SeasonID });
        //    ViewData["ddView"] = ListOfPivotViews.Select(m => new SelectListItem { Value = m.ViewID.ToString(), Text = m.ViewDesc + " (" + m.ViewID.ToString() + ")", Selected = m.ViewID == ViewID });
        //    ViewBag.SeasonID = SeasonID;
        //    ViewBag.DeptID = DeptID;
        //    ViewBag.DeptName = DeptName;
        //    return View(ListOfAttributeValuePivots);
        //}

        //[HttpPost]
        //public ActionResult Index(FormCollection collection,int DeptID=0)
        //{
        //    int ViewID = Convert.ToInt32(collection["ddView"]);
        //    int SeasonID = Convert.ToInt32(collection["ddSeason"]);

        //    #region StaticDropdowns
        //    SeasonBusinessLayer rbl = new SeasonBusinessLayer();
        //    List<Season> ListOfSeasons = rbl.Seasons.Where(r => r.SeasonActive == 1).OrderBy(o => o.FirstLaunchDate).ToList();

        //    PivotViewBusinessLayer tbl = new PivotViewBusinessLayer();
        //    List<PivotView> ListOfPivotViews = tbl.PivotViews(User.Identity.Name).OrderBy(o => o.ViewDesc).ToList();

        //    if (ViewID == 0 && ListOfPivotViews.Count > 0)
        //    {
        //        ViewID = ListOfPivotViews.First().ViewID;
        //    }
        //    if (SeasonID == 0 && ListOfSeasons.Count > 0)
        //    {
        //        SeasonID = ListOfSeasons.First().SeasonID;
        //    }       

        //    ViewAttributePivotBusinessLayer vapbl = new ViewAttributePivotBusinessLayer();
        //    List<ViewAttributePivot> ListOfVAP = vapbl.ViewAttributePivots.Where(r => r.ViewID == ViewID).OrderBy(s => s.PivotOrder).ToList();

        //    int AttributeCount = 1;

        //    foreach (ViewAttributePivot vap in ListOfVAP)
        //    {
        //        ViewData["att" + AttributeCount.ToString("00")] = vap.AttributeDescription;
        //        AttributeCount++;
        //        if (AttributeCount > 12)
        //        {
        //            break;
        //        }
        //    }

        //    DeptBusinessLayer dbl = new DeptBusinessLayer();
        //    List<Dept> ListOfDepts = dbl.Depts.ToList();
        //    string DeptName = ListOfDepts.Where(q => q.DeptID == DeptID).Single().DeptName;


        //    #endregion
        //    AttributeValuePivotBusinessLayer sbl = new AttributeValuePivotBusinessLayer();
        //    List<AttributeValuePivot> ListOfAttributeValuePivots = sbl.AttributeValuePivots(DeptID,ViewID, SeasonID).OrderBy(i => i.SFID).ToList();

        //    ViewData["ddSeason"] = ListOfSeasons.Select(m => new SelectListItem { Value = m.SeasonID.ToString(), Text = m.SeasonDesc + " (" + m.SeasonID.ToString() + ")", Selected = m.SeasonID == SeasonID });
        //    ViewData["ddView"] = ListOfPivotViews.Select(m => new SelectListItem { Value = m.ViewID.ToString(), Text = m.ViewDesc + " (" + m.ViewID.ToString() + ")", Selected = m.ViewID == ViewID });

        //    #region Filter
        //    string FilterMessage = "";
        //    string SapArticleID_f = (string)(collection["SapArticleID_f"]);
        //    string MasterDescription_f = (string)(collection["MasterDescription_f"]);
        //    string CatDesc_f = (string)(collection["CatDesc_f"]);
        //    string MerchCatDesc_f = (string)(collection["MerchCatDesc_f"]);
        //    string VendorDesc_f = (string)(collection["VendorDesc_f"]);
        //    string ProposedPreArticleGradeDesc_f = (string)(collection["ProposedPreArticleGradeDesc_f"]);
        //    string ConfirmedPreArticleGradeDesc_f = (string)(collection["ConfirmedPreArticleGradeDesc_f"]);

        //    string[] AttributeFilter = new string[18];
        //    for(int i = 1; i <= 18; i++)
        //    {
        //        AttributeFilter[i-1]= (string)(collection["Att" + i.ToString("00") + "Fragment"]);
        //    }

        //    if (SapArticleID_f!="")
        //    {
        //        ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.SapArticleID.ToString().Contains(SapArticleID_f)).ToList();
        //        FilterMessage = " filtered by Sap Article";
        //    }
        //    if (MasterDescription_f != "")
        //    {
        //        ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.MasterDescription.ToString().Contains(MasterDescription_f)).ToList();
        //        FilterMessage = FilterMessage + " filtered by Master Description";
        //    }
        //    if (CatDesc_f != "")
        //    {
        //        ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.CatDesc.ToString().Contains(CatDesc_f)).ToList();
        //        FilterMessage = FilterMessage + " filtered by Cat Name";
        //    }
        //    if (MerchCatDesc_f != "")
        //    {
        //        ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.MerchCatDesc.ToString().Contains(MerchCatDesc_f)).ToList();
        //        FilterMessage = " filtered by Merch Cat";
        //    }
        //    if (VendorDesc_f != "")
        //    {
        //        ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.VendorDesc.ToString().Contains(VendorDesc_f)).ToList();
        //        FilterMessage = FilterMessage + " filtered by Vendor Description";
        //    }
        //    if (ProposedPreArticleGradeDesc_f != "")
        //    {
        //        ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.ProposedPreArticleGradeDesc.ToString().Contains(ProposedPreArticleGradeDesc_f)).ToList();
        //        FilterMessage = FilterMessage + " filtered by Proposed Grade";
        //    }
        //    if (ConfirmedPreArticleGradeDesc_f != "")
        //    {
        //        ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.ConfirmedPreArticleGradeDesc.ToString().Contains(ConfirmedPreArticleGradeDesc_f)).ToList();
        //        FilterMessage = FilterMessage + " filtered by Confirmed Grade";
        //    }
        //    for (int i = 1; i <= 18; i++)
        //    {
        //        if (AttributeFilter[i-1] != "")
        //        {
        //            //ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.Att01.ToString().Contains(AttributeFilter[0])).ToList();
        //            ListOfAttributeValuePivots = sbl.AttributeValuePivotWhereQuery(ListOfAttributeValuePivots, "Att" + i.ToString("00"), AttributeFilter[i-1]).ToList();
        //            FilterMessage = FilterMessage + " filtered by attribute " + i.ToString("00");
        //        }
        //    }

        //    ViewBag.DeptID = DeptID;
        //    ViewBag.DeptName = DeptName;

        //    ViewBag.FilterMessage = FilterMessage;
        //    ViewBag.SapArticleID_f = SapArticleID_f;
        //    ViewBag.MasterDescription_f = MasterDescription_f;
        //    ViewBag.CatDesc_f = CatDesc_f;
        //    ViewBag.MerchCatDesc_f = MerchCatDesc_f;
        //    ViewBag.VendorDesc_f = VendorDesc_f;
        //    ViewBag.ProposedPreArticleGradeDesc_f = ProposedPreArticleGradeDesc_f;
        //    ViewBag.ConfirmedPreArticleGradeDesc_f = ConfirmedPreArticleGradeDesc_f;
        //    for (int i = 1; i <= 18; i++)
        //    {
        //        ViewData["Att" + i.ToString("00") + "Fragment"] = AttributeFilter[i - 1];
        //    }
        //    #endregion


        //    return View(ListOfAttributeValuePivots);

        //}

        public ActionResult Index(int ViewID = 0, int SeasonID = 0, int DeptID = 0)
        {
            #region StaticDropdowns
            SeasonBusinessLayer rbl = new SeasonBusinessLayer();
            List<Season> ListOfSeasons = rbl.Seasons.Where(r => r.SeasonActive == 1).OrderBy(o => o.FirstLaunchDate).ToList();

            PivotViewBusinessLayer tbl = new PivotViewBusinessLayer();
            List<PivotView> ListOfPivotViews = tbl.PivotViews(User.Identity.Name).OrderBy(o => o.ViewDesc).ToList();

            DeptBusinessLayer dbl = new DeptBusinessLayer();
            List<Dept> ListOfDepts = dbl.Depts.ToList();

            if (ViewID == 0 && ListOfPivotViews.Count > 0)
            {
                bool parsed = int.TryParse(ViewBag.ViewID, out ViewID);
                if (!parsed)
                {
                    ViewID = ListOfPivotViews.First().ViewID;
                }

            }
            if (SeasonID == 0 && ListOfSeasons.Count > 0)
            {
                bool parsed = int.TryParse(ViewBag.SeasonID, out SeasonID);
                if (!parsed)
                {
                    SeasonID = ListOfSeasons.First().SeasonID;
                }     
            }

            ViewAttributePivotBusinessLayer vapbl = new ViewAttributePivotBusinessLayer();
            List<ViewAttributePivot> ListOfVAP = vapbl.ViewAttributePivots.Where(r => r.ViewID == ViewID).OrderBy(s => s.PivotOrder).ToList();

            int AttributeCount = 1;
            bool ShallWefilter = true;

            List<WebGridColumn> columns = new List<WebGridColumn>();
            columns.Add(new WebGridColumn() { ColumnName = "DeptID", Header = "DeptID", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "DeptName", Header = "Dept", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "ContinuationStatus", Header = "Continue", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "ReplacementStatus", Header = "Replace", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "SapArticleID", Header = "SapArticleID", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "MasterDescription", Header = "MasterDesc", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "CatDesc", Header = "Cat", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "MerchCatDesc", Header = "MerchCat", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "VendorDesc", Header = "Vendor", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "ProposedPreArticleGradeDesc", Header = "ProposedGrade", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "ConfirmedPreArticleGradeDesc", Header = "ConfirmedGrade", CanSort = ShallWefilter });

            foreach (ViewAttributePivot vap in ListOfVAP)
            {
                ViewData["att" + AttributeCount.ToString("00")] = vap.AttributeDescription;
                Object WebGridFormat = "#,##0.00";
                columns.Add(new WebGridColumn() {
                    ColumnName = "att" + AttributeCount.ToString("00"),
                    Header = vap.AttributeDescription,

                    CanSort = false});
                AttributeCount++;
                if (AttributeCount > 18)
                {
                    break;
                }
            }
            columns.Add(new WebGridColumn()
                {
                    ColumnName = "PreArticleSeasonal",
                    Header = "Edit",
                    CanSort = false,
                    Format = (item) => new MvcHtmlString("<a href='" + Url.Action("Edit", "PreArticleSeasonal", new { id = item.ID }, null) + "'>edit</a>")
                });
            columns.Add(new WebGridColumn()
                {
                    ColumnName = "Attr",
                    Header = "Attributes",
                    CanSort = false,
                    Format = (item) => new MvcHtmlString("<a href='" + Url.Action("Index", "AttributeValue", new { SeasonID = item.SeasonID, SfID = item.SFID }, null) + "'>attr</a>")
                });

            string DeptName = ListOfDepts.Where(q => q.DeptID == DeptID).Single().DeptName;

            #endregion

            AttributeValuePivotBusinessLayer sbl = new AttributeValuePivotBusinessLayer();
            List<AttributeValuePivot> ListOfAttributeValuePivots = sbl.AttributeValuePivots(DeptID, ViewID, SeasonID).OrderBy(i => i.SFID).ToList();

            ViewData["ddSeason"] = ListOfSeasons.Select(m => new SelectListItem { Value = m.SeasonID.ToString(), Text = m.SeasonDesc + " (" + m.SeasonID.ToString() + ")", Selected = m.SeasonID == SeasonID });
            ViewData["ddView"] = ListOfPivotViews.Select(m => new SelectListItem { Value = m.ViewID.ToString(), Text = m.ViewDesc + " (" + m.ViewID.ToString() + ")", Selected = m.ViewID == ViewID });
            ViewData["ddDept"] = ListOfDepts.Select(m => new SelectListItem { Value = m.DeptID.ToString(), Text = m.DeptName + " (" + m.DeptID.ToString() + ")", Selected = m.DeptID == DeptID });

            ViewBag.SeasonID = SeasonID;
            ViewBag.DeptID = DeptID;
            ViewBag.DeptName = DeptName;
            ViewBag.ViewID = ViewID;
            ViewBag.Columns = columns;

            return View(ListOfAttributeValuePivots);
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            int ViewID = Convert.ToInt32(collection["ddView"]);
            int SeasonID = Convert.ToInt32(collection["ddSeason"]);
            int DeptID = Convert.ToInt32(collection["ddDept"]);

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
            bool ShallWefilter = true;

            List<WebGridColumn> columns = new List<WebGridColumn>();
            columns.Add(new WebGridColumn() { ColumnName = "DeptID", Header = "DeptID", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "DeptName", Header = "Dept", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "ContinuationStatus", Header = "Continue", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "ReplacementStatus", Header = "Replace", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "SapArticleID", Header = "SapArticleID", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "MasterDescription", Header = "MasterDesc", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "CatDesc", Header = "Cat", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "MerchCatDesc", Header = "MerchCat", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "VendorDesc", Header = "Vendor", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "ProposedPreArticleGradeDesc", Header = "ProposedGrade", CanSort = ShallWefilter });
            columns.Add(new WebGridColumn() { ColumnName = "ConfirmedPreArticleGradeDesc", Header = "ConfirmedGrade", CanSort = ShallWefilter });

            foreach (ViewAttributePivot vap in ListOfVAP)
            {
                ViewData["att" + AttributeCount.ToString("00")] = vap.AttributeDescription;
                Object WebGridFormat = "#,##0.00" ;
                columns.Add(new WebGridColumn() { ColumnName = "att" + AttributeCount.ToString("00"), Header = vap.AttributeDescription, CanSort = false});
                AttributeCount++;
                if (AttributeCount > 18)
                {
                    break;
                }
            }
            columns.Add(new WebGridColumn()
            {
                ColumnName = "PreArticleSeasonal",
                Header = "Edit",
                CanSort = false,
                Format = (item) => new MvcHtmlString("<a href='" + Url.Action("Edit", "PreArticleSeasonal", new { id = item.ID }, null) + "'>edit</a>")
            });
            columns.Add(new WebGridColumn()
            {
                ColumnName = "Attr",
                Header = "Attributes",
                CanSort = false,
                Format = (item) => new MvcHtmlString("<a href='" + Url.Action("Index", "AttributeValue", new { SeasonID = item.SeasonID, SfID = item.SFID }, null) + "'>attr</a>")
            });

            DeptBusinessLayer dbl = new DeptBusinessLayer();
            List<Dept> ListOfDepts = dbl.Depts.ToList();
            string DeptName = ListOfDepts.Where(q => q.DeptID == DeptID).Single().DeptName;


            #endregion
            AttributeValuePivotBusinessLayer sbl = new AttributeValuePivotBusinessLayer();
            List<AttributeValuePivot> ListOfAttributeValuePivots = sbl.AttributeValuePivots(DeptID, ViewID, SeasonID).OrderBy(i => i.SFID).ToList();

            ViewData["ddSeason"] = ListOfSeasons.Select(m => new SelectListItem { Value = m.SeasonID.ToString(), Text = m.SeasonDesc + " (" + m.SeasonID.ToString() + ")", Selected = m.SeasonID == SeasonID });
            ViewData["ddView"] = ListOfPivotViews.Select(m => new SelectListItem { Value = m.ViewID.ToString(), Text = m.ViewDesc + " (" + m.ViewID.ToString() + ")", Selected = m.ViewID == ViewID });
            ViewData["ddDept"] = ListOfDepts.Select(m => new SelectListItem { Value = m.DeptID.ToString(), Text = m.DeptName + " (" + m.DeptID.ToString() + ")", Selected = m.DeptID == DeptID });

            #region Filter
            string FilterMessage = "";
            string DeptName_f = (string)(collection["DeptName_f"]);
            string SapArticleID_f = (string)(collection["SapArticleID_f"]);
            string ContinuationStatus_f = (string)(collection["ContinuationStatus_f"]);
            string ReplacementStatus_f = (string)(collection["ReplacementStatus_f"]);
            string MasterDescription_f = (string)(collection["MasterDescription_f"]);
            string CatDesc_f = (string)(collection["CatDesc_f"]);
            string MerchCatDesc_f = (string)(collection["MerchCatDesc_f"]);
            string VendorDesc_f = (string)(collection["VendorDesc_f"]);
            string ProposedPreArticleGradeDesc_f = (string)(collection["ProposedPreArticleGradeDesc_f"]);
            string ConfirmedPreArticleGradeDesc_f = (string)(collection["ConfirmedPreArticleGradeDesc_f"]);

            string[] AttributeFilter = new string[18];
            for (int i = 1; i <= 18; i++)
            {
                AttributeFilter[i - 1] = (string)(collection["Att" + i.ToString("00") + "Fragment"]);
            }
            if (DeptName_f != "")
            {
                int _DeptID = 0;
                bool parsed = int.TryParse(DeptName_f, out _DeptID);
                if (parsed)
                {
                    ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.DeptID == _DeptID).ToList();
                    FilterMessage = FilterMessage + " filtered by Dept ID";
                }
                else
                {
                    ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.DeptName.ToLower().Contains(DeptName_f.ToLower())).ToList();
                    FilterMessage = FilterMessage + " filtered by Dept Name";
                }
            }

            if (SapArticleID_f != "")
            {
                ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.SapArticleID.Contains(SapArticleID_f)).ToList();
                FilterMessage = " filtered by Sap Article";
            }
            if (MasterDescription_f != "")
            {
                ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.MasterDescription.ToLower().Contains(MasterDescription_f.ToLower())).ToList();
                FilterMessage = FilterMessage + " filtered by Master Description";
            }
            if (CatDesc_f != "")
            {
                ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.CatDesc.ToLower().Contains(CatDesc_f.ToLower())).ToList();
                FilterMessage = FilterMessage + " filtered by Cat Name";  
            }
            if (MerchCatDesc_f != "")
            {
                int MerchCatID = 0;
                bool parsed = int.TryParse(MerchCatDesc_f, out MerchCatID);
                if (parsed)
                {
                    ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.MerchCatID == MerchCatID).ToList();
                    FilterMessage = FilterMessage + " filtered by MerchCat ID";
                }
                else
                {
                    ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.MerchCatDesc.ToLower().Contains(MerchCatDesc_f)).ToList();
                    FilterMessage = FilterMessage + " filtered by MerchCat Name";
                }
            }
            if (VendorDesc_f != "")
            {
                if (ListOfAttributeValuePivots.Any(a => a.VendorID==VendorDesc_f))
                {
                    ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.VendorID==VendorDesc_f).ToList();
                    FilterMessage = FilterMessage + " filtered by Vendor ID";
                }
                else
                {
                    ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.VendorDesc.ToLower().Contains(VendorDesc_f.ToLower())).ToList();
                    FilterMessage = FilterMessage + " filtered by Vendor Description";
                }
                
            }
            if (ProposedPreArticleGradeDesc_f != "")
            {
                ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.ProposedPreArticleGradeDesc.ToLower().Contains(ProposedPreArticleGradeDesc_f.ToLower())).ToList();
                FilterMessage = FilterMessage + " filtered by Proposed Grade";
            }
            if (ConfirmedPreArticleGradeDesc_f != "")
            {
                ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.ConfirmedPreArticleGradeDesc.ToLower().Contains(ConfirmedPreArticleGradeDesc_f.ToLower())).ToList();
                FilterMessage = FilterMessage + " filtered by Confirmed Grade";
            }
            if (ContinuationStatus_f != "")
            {
                ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.ContinuationStatus.ToLower().Contains(ContinuationStatus_f.ToLower())).ToList();
                FilterMessage = FilterMessage + " filtered by Continuation Status";
            }
            if (ReplacementStatus_f != "")
            {
                ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.ReplacementStatus.ToLower().Contains(ReplacementStatus_f.ToLower())).ToList();
                FilterMessage = FilterMessage + " filtered by Replacement Grade";
            }
            for (int i = 1; i <= 18; i++)
            {
                if (AttributeFilter[i - 1] != "")
                {
                    //ListOfAttributeValuePivots = ListOfAttributeValuePivots.Where(a => a.Att01.ToString().Contains(AttributeFilter[0])).ToList();
                    ListOfAttributeValuePivots = sbl.AttributeValuePivotWhereQuery(ListOfAttributeValuePivots, "Att" + i.ToString("00"), AttributeFilter[i - 1]).ToList();
                    FilterMessage = FilterMessage + " filtered by attribute " + i.ToString("00");
                }
            }
            #endregion

            ViewBag.DeptID = DeptID;
            ViewBag.DeptName = DeptName;
            ViewBag.Columns = columns;

            ViewBag.FilterMessage = FilterMessage;
            ViewBag.DeptName_f = DeptName_f;
            ViewBag.SapArticleID_f = SapArticleID_f;
            ViewBag.MasterDescription_f = MasterDescription_f;
            ViewBag.CatDesc_f = CatDesc_f;
            ViewBag.MerchCatDesc_f = MerchCatDesc_f;
            ViewBag.VendorDesc_f = VendorDesc_f;
            ViewBag.ProposedPreArticleGradeDesc_f = ProposedPreArticleGradeDesc_f;
            ViewBag.ConfirmedPreArticleGradeDesc_f = ConfirmedPreArticleGradeDesc_f;
            ViewBag.ContinuationStatus_f = ContinuationStatus_f;
            ViewBag.ReplacementStatus_f = ReplacementStatus_f;
            for (int i = 1; i <= 18; i++)
            {
                ViewData["Att" + i.ToString("00") + "Fragment"] = AttributeFilter[i - 1];
            }
            


            return View(ListOfAttributeValuePivots);

        }
    }
}
