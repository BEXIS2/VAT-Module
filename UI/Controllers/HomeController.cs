using BExIS.Modules.VAT.UI.Helper;
using BExIS.Modules.VAT.UI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;
using Vaiona.Web.Extensions;
using Vaiona.Web.Mvc.Models;

namespace BExIS.Modules.VAT.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(long id)
        {
            ViewBag.Title = PresentationModel.GetViewTitleForTenant("VAT", this.Session.GetTenant());

            EditModel model = new EditModel();

            //set all important ViewData 
            setViewData(id);

            // load geoData if exist and set it to the model
            GeoConfigHelper configHelper = new GeoConfigHelper();
            model = configHelper.Read(id);


            return View(model);

        }

        // GET: Help
        public ActionResult IndexPartial(long id)
        {
            ViewBag.Title = PresentationModel.GetViewTitleForTenant("VAT", this.Session.GetTenant());

            EditModel model = new EditModel();

            //set all important ViewData 
            setViewData(id);

            // load geoData if exist and set it to the model
            GeoConfigHelper configHelper = new GeoConfigHelper();
            model = configHelper.Read(id);


            return PartialView("Index",model);

        }

        public ActionResult View(long id)
        {

            ViewData["Title"] = PresentationModel.GetViewTitleForTenant("VAT", this.Session.GetTenant());

            EditModel model = new EditModel();

            //set all important ViewData 
            setViewData(id);

            // load geoData if exist and set it to the model
            GeoConfigHelper configHelper = new GeoConfigHelper();
            model = configHelper.Read(id);

            return PartialView("_view", model);

        }

        public ActionResult Edit(long id)
        {

            ViewData["Title"] = PresentationModel.GetViewTitleForTenant("VAT", this.Session.GetTenant());

            EditModel model = new EditModel();

            //set all important ViewData 
            setViewData(id);

            // load geoData if exist and set it to the model
            GeoConfigHelper configHelper = new GeoConfigHelper();
            model = configHelper.Read(id);

            //JsonConvert.SerializeObject(model);

            return PartialView("_edit", model);

        }

        [HttpPost]
        public ActionResult Edit(EditModel model)
        {
            setViewData(model.Id);

            GeoConfigHelper configHelper = new GeoConfigHelper();
            configHelper.Save(model);

            return PartialView("_view", model);
        }



        [HttpGet]
        public JsonResult Variables(long id)
        {
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

        private void setViewData(long id)
        {
            GeoConfigHelper helper = new GeoConfigHelper();

            var variables = helper.GetVariables(id);

            ViewData["Id"] = id;

            // load ViewData
            ViewData["Longitude"] = variables;
            ViewData["Latitude"] = variables;
            ViewData["Time"] = variables;

            ViewData["DataType"] = helper.GetDataTypes();
            ViewData["SpatialReference"] = helper.GetSpatialReference();
        }
    }
}
