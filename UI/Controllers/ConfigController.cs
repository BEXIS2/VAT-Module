using BExIS.Dlm.Services.Data;
using BExIS.Dlm.Services.DataStructure;
using BExIS.IO.DataType.DisplayPattern;
using BExIS.Modules.VAT.UI.Helper;
using BExIS.Modules.VAT.UI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// this get returns a display pattern of a variable if exit otherwise returne false
        /// </summary>
        /// <param name="id">variable id</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DisplayPattern(long id, string variable)
        {
            //id less then 1 is not possible so return empty string
            if(id<=0) return Json("", JsonRequestBehavior.AllowGet);

            using (var datasetManager = new DatasetManager())
            using (var dataStructureManager = new DataStructureManager())
            using (var dataContainerManager = new DataContainerManager())
            using (var dataTypeManager = new DataTypeManager())
            {
                var dataset = datasetManager.GetDataset(id);
                if (dataset == null) return Json("", JsonRequestBehavior.AllowGet);

                var datastructureId = dataset.DataStructure.Id;
                var datastructure = dataStructureManager.StructuredDataStructureRepo.Get(datastructureId);

                var v = datastructure.Variables.Where(var=>var.Label.ToLower().Equals(variable.ToLower())).FirstOrDefault();

                //if variable not exit return false
                if(v==null) return Json("", JsonRequestBehavior.AllowGet);

                if (v.DataAttribute != null)
                {
                    var attr = dataContainerManager.DataAttributeRepo.Get(v.DataAttribute.Id);
                    if (attr.DataType.SystemType.Equals("DateTime"))
                    {
                        var dataType = dataTypeManager.Repo.Get(attr.DataType.Id);

                        if (dataType != null && dataType.Extra != null)
                        {
                            DataTypeDisplayPattern dp = DataTypeDisplayPattern.Materialize(dataType.Extra);
                            if (dp != null)
                            {
                                return Json(dp.StringPattern, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}
