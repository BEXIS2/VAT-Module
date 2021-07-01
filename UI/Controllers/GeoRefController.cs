using BExIS.App.Bootstrap.Attributes;
using BExIS.Dlm.Services.Data;
using BExIS.Utils.Route;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using Vaiona.Utils.Cfg;

namespace BExIS.Modules.VAT.UI.Controllers
{
    public class GeoRefController : ApiController
    {
        //[BExISApiAuthorize]
        //[GetRoute("api/dataset/georef/")]
        [GetRoute("api/georef/")]
        // GET api/<controller>
        public IEnumerable<GeoRefViewObject> Get()
        {
            List<GeoRefViewObject> ids = new List<GeoRefViewObject>();

            using (var datasetManager = new DatasetManager())
            {
                var datasetIds = datasetManager.GetDatasetLatestIds();

                foreach (var id in datasetIds)
                {
                    var filepath = Path.Combine(AppConfiguration.DataPath, "Datasets", id.ToString(), "geoengine.json");

                    if (File.Exists(filepath)) ids.Add(new GeoRefViewObject() { DatasetId = id, hasGeoReference = true });
                    else ids.Add(new GeoRefViewObject() { DatasetId = id, hasGeoReference = false });
                }

            }


            return ids;
        }

        [BExISApiAuthorize]
        //[GetRoute("api/dataset/{id}/georef/")]
        [GetRoute("api/georef/{id}")]
        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            var filepath = Path.Combine(AppConfiguration.DataPath, "Datasets", id.ToString(), "geoengine.json");

            if (File.Exists(filepath))
            {
                var response = Request.CreateResponse();
                response.Content = new StringContent(File.ReadAllText(filepath), System.Text.Encoding.UTF8, "application/json");

                return response;
            }
        
            else
            {
                var request = Request.CreateResponse();
                request.Content = new StringContent("Georeference is not available.");
                return request;
            }

            
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }

    public class GeoRefViewObject
    {
        public long DatasetId { get; set; }
        public bool hasGeoReference { get; set; }
    }
}