using BExIS.App.Bootstrap.Attributes;
using BExIS.Dlm.Entities.Data;
using BExIS.Dlm.Services.Data;
using BExIS.Security.Entities.Authorization;
using BExIS.Security.Entities.Subjects;
using BExIS.Security.Services.Authorization;
using BExIS.Security.Services.Objects;
using BExIS.Security.Services.Subjects;
using BExIS.Utils.Route;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Vaiona.Utils.Cfg;

namespace BExIS.Modules.VAT.UI.Controllers
{
    public class GeoRefController : ApiController
    {

        /// <summary>
        /// return only ids of datasets that have a geo reference and the user hav acces to read
        /// </summary>
        /// <returns></returns>
        [GetRoute("api/georef/")]
        public List<long> Get()
        {
            // get token from the request
            string token = this.Request.Headers.Authorization?.Parameter;

            // flag for the public dataset check
            bool isPublic = false;

            List<long> ids = new List<long>();

            using (var datasetManager = new DatasetManager())
            {
                var datasetIds = datasetManager.GetDatasetLatestIds();

                foreach (var id in datasetIds)
                {
                    using (EntityPermissionManager entityPermissionManager = new EntityPermissionManager())
                    using (EntityManager entityManager = new EntityManager())
                    using (UserManager userManager = new UserManager())
                    {
                        // load the entity id of the e.g. is it a sample or dataset or publication
                        long? entityTypeId = entityManager.FindByName(typeof(Dataset).Name)?.Id;
                        entityTypeId = entityTypeId.HasValue ? entityTypeId.Value : -1;

                        // if the subject is null and one entry exist, means this dataset is public
                        isPublic = entityPermissionManager.Exists(null, entityTypeId.Value, id);

                        // load user based on token
                        User user = userManager.Users.Where(u => u.Token.Equals(token)).FirstOrDefault();

                        if (isPublic || user != null)
                        {
                            if (isPublic || entityPermissionManager.HasEffectiveRight(user.Name, typeof(Dataset), id, RightType.Read))
                            {
                                var filepath = Path.Combine(AppConfiguration.DataPath, "Datasets", id.ToString(), "geoengine.json");

                                if (File.Exists(filepath))
                                {
                                    ids.Add(id);
                                }
                            }
                        }
                    }
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
            // get token from the request
            string token = this.Request.Headers.Authorization?.Parameter;

            // flag for the public dataset check
            bool isPublic = false;

            using (EntityPermissionManager entityPermissionManager = new EntityPermissionManager())
            using (EntityManager entityManager = new EntityManager())
            using (UserManager userManager = new UserManager())
            {
                // load the entity id of the e.g. is it a sample or dataset or publication
                long? entityTypeId = entityManager.FindByName(typeof(Dataset).Name)?.Id;
                entityTypeId = entityTypeId.HasValue ? entityTypeId.Value : -1;

                // if the subject is null and one entry exist, means this dataset is public
                isPublic = entityPermissionManager.Exists(null, entityTypeId.Value, id);

                // if its not public and no token exist - fire exception
                if (!isPublic && String.IsNullOrEmpty(token))

                {
                    var request = Request.CreateResponse();
                    request.Content = new StringContent("Bearer token not exist.");

                    return request;
                }

                // load user based on token
                User user = userManager.Users.Where(u => u.Token.Equals(token)).FirstOrDefault();

                if (isPublic || user != null)
                {
                    if (isPublic || entityPermissionManager.HasEffectiveRight(user.Name, typeof(Dataset), id, RightType.Read))
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
                    else // has rights?
                    {
                        var request = Request.CreateResponse();
                        request.Content = new StringContent("User has no read right.");

                        return request;
                    }
                }
                else
                {
                    var request = Request.CreateResponse();
                    request.Content = new StringContent("User is not available.");

                    return request;
                }

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

}