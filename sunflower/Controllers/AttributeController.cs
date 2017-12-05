using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayerLibrary;

namespace sunflower.Controllers
{
    public class AttributeController : Controller
    {
        // GET: Attribute
        public ActionResult Index()
        {
            AttributeBusinessLayer bl = new AttributeBusinessLayer();
            List<BusinessLayerLibrary.Attribute> ListOfAttributes = bl.Attributes.OrderBy(o => o.AttributeOrder).ToList();
            return View(ListOfAttributes);
        }

        // GET: Attribute/Create
        public ActionResult Create()
        {
            DataTypeBusinessLayer dbl = new DataTypeBusinessLayer();
            List<SFDataType> ListOfDataTypes = dbl.DataTypes.ToList();

            var Items = new HashSet<SelectListItem>();
            Items.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = false });
            Items.Add(new SelectListItem { Text = "No", Value = "0", Selected = true });
            ViewData["ddSeasonal"] = Items;
            ViewData["ddCascade"] = Items;
            ViewData["ddCalculated"] = Items;

            ViewData["ddDataType"] = ListOfDataTypes.Select(m => new SelectListItem { Value = m.DataTypeID.ToString(), Text = m.DataTypeDescription + " (" + m.DataTypeID.ToString() + ")" });

            return View();
        }

        // POST: Attribute/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Create";
            try
            {

                BusinessLayerLibrary.Attribute a = new BusinessLayerLibrary.Attribute();

                #region Pull from Form Collection
                a.AttributeDesc = (string)collection["AttributeDesc"];
                a.Seasonal = Convert.ToInt32(collection["ddSeasonal"]);
                a.AttributeOrder = Convert.ToInt32(collection["AttributeOrder"]);
                a.SapName = (string)collection["SapName"];
                a.PlmName = (string)collection["PlmName"];
                a.DataTypeID = Convert.ToInt32(collection["ddDataType"]);
                a.FutureSeasonCascade = Convert.ToInt32(collection["ddCascade"]);
                a.Calculated = Convert.ToInt32(collection["ddCalculated"]);
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

        // GET: Attribute/Edit/5
        public ActionResult Edit(int id)
        {
            AttributeBusinessLayer bl = new AttributeBusinessLayer();
            BusinessLayerLibrary.Attribute o = bl.Attributes.Where(p => p.AttributeID==id).Single();

            DataTypeBusinessLayer dbl = new DataTypeBusinessLayer();
            List<SFDataType> ListOfDataTypes = dbl.DataTypes.ToList();

            var SeasonalItems = new HashSet<SelectListItem>();
            SeasonalItems.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = o.Seasonal==1});
            SeasonalItems.Add(new SelectListItem { Text = "No", Value = "0", Selected = o.Seasonal == 0 });

            var CascadeItems = new HashSet<SelectListItem>();
            CascadeItems.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = o.FutureSeasonCascade == 1 });
            CascadeItems.Add(new SelectListItem { Text = "No", Value = "0",  Selected = o.FutureSeasonCascade == 0 });

            var CalculatedItems = new HashSet<SelectListItem>();
            CalculatedItems.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = o.Calculated == 1 });
            CalculatedItems.Add(new SelectListItem { Text = "No", Value = "0", Selected = o.Calculated == 0 });

            ViewData["ddSeasonal"] = SeasonalItems;
            ViewData["ddCascade"] = CascadeItems;
            ViewData["ddCalculated"] = CalculatedItems;

            ViewData["ddDataType"] = ListOfDataTypes.Select(m => new SelectListItem { Value = m.DataTypeID.ToString(), Text = m.DataTypeDescription + " (" + m.DataTypeID.ToString() + ")" ,Selected = m.DataTypeID==o.DataTypeID});

            return View(o);
        }

        // POST: Attribute/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Edit";
            try
            {
                AttributeBusinessLayer bl = new AttributeBusinessLayer();
                BusinessLayerLibrary.Attribute a = bl.Attributes.Where(i => i.AttributeID == id).Single();

                #region Pull from Form Collection
                a.AttributeDesc = (string)collection["AttributeDesc"];
                a.Seasonal = Convert.ToInt32(collection["ddSeasonal"]);
                a.AttributeOrder = Convert.ToInt32(collection["AttributeOrder"]);
                a.SapName = (string)collection["SapName"];
                a.PlmName = (string)collection["PlmName"];
                a.DataTypeID = Convert.ToInt32(collection["ddDataType"]);
                a.FutureSeasonCascade = Convert.ToInt32(collection["ddCascade"]);
                a.Calculated = Convert.ToInt32(collection["ddCalculated"]);
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

        // GET: Attribute/Delete/5
        public ActionResult Delete(int id)
        {
            AttributeBusinessLayer bl = new AttributeBusinessLayer();
            BusinessLayerLibrary.Attribute o = bl.Attributes.Where(p => p.AttributeID == id).Single();

            return View(o);
        }

        // POST: Attribute/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            bool DidItWork = false;
            string CrudAction = "Delete";
            try
            {
                AttributeBusinessLayer bl = new AttributeBusinessLayer();
                BusinessLayerLibrary.Attribute a = bl.Attributes.Where(i => i.AttributeID == id).Single();

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
