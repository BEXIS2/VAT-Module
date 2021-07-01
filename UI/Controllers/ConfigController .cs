using BExIS.Modules.VAT.UI.Helper;
using BExIS.Modules.VAT.UI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;
using Vaiona.Web.Extensions;
using Vaiona.Web.Mvc.Models;

namespace BExIS.Modules.VAT.UI.Controllers
{
    public class ConfigController : Controller
    {


        public JsonResult View(long id)
        {

            EditModel model = new EditModel();

            // load geoData if exist and set it to the model
            GeoConfigHelper configHelper = new GeoConfigHelper();
            model = configHelper.Read(id);

            return Json(model, JsonRequestBehavior.AllowGet);

        }

        //public ActionResult Edit(long id)
        //{

        //    ViewData["Title"] = PresentationModel.GetViewTitleForTenant("VAT", this.Session.GetTenant());

        //    EditModel model = new EditModel();

        //    //set all important ViewData 
        //    setViewData(id);

        //    // load geoData if exist and set it to the model
        //    GeoConfigHelper configHelper = new GeoConfigHelper();
        //    model = configHelper.Read(id);

        //    //JsonConvert.SerializeObject(model);

        //    return PartialView("_edit", model);

        //}
        [HttpGet]
        public JsonResult Edit(long id)
        {

            if (id == 0) return Json(new EditModel(), JsonRequestBehavior.AllowGet);

            EditModel model = new EditModel();

            // load geoData if exist and set it to the model
            GeoConfigHelper configHelper = new GeoConfigHelper();
            model = configHelper.Read(id);

            return Json(model, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Edit(EditModel model)
        {
            GeoConfigHelper configHelper = new GeoConfigHelper();
            configHelper.Save(model);

            return Json(true);
        }



        [HttpGet]
        public JsonResult Variables(long id)
        {
            if (id == 0)
            {
                return Json(new string[0], JsonRequestBehavior.AllowGet); 
            }

            GeoConfigHelper helper = new GeoConfigHelper();

            var variables = helper.GetVariables(id);

            return Json(variables.Items, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DataTypes()
        {
            GeoConfigHelper helper = new GeoConfigHelper();

            return Json(helper.GetDataTypes().Items, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SpatialReferences()
        {
            GeoConfigHelper helper = new GeoConfigHelper();

            return Json(helper.GetSpatialReference().Items, JsonRequestBehavior.AllowGet);
        }
    }
}
