using BExIS.Dlm.Services.Data;
using BExIS.Dlm.Services.DataStructure;
using BExIS.IO;
using BExIS.Modules.VAT.UI.Models;
using BExIS.UI.Helpers;
using BExIS.Xml.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vaiona.Utils.Cfg;

namespace BExIS.Modules.VAT.UI.Helper
{
    public class GeoConfigHelper
    {
        /// <summary>
        /// load datatypes from settings xml of this modul
        /// </summary>
        /// <returns></returns>
        public SelectList GetDataTypes()
        {
            SettingsHelper settingsHelper = new SettingsHelper("Vat");
            string values = settingsHelper.GetValue("DataTypes");

            if (string.IsNullOrEmpty(values)) return new SelectList(new List<string>());

            return new SelectList(values.Split(','));
        }

        /// <summary>
        /// load Spatial Reference List from settings xml of this modul
        /// </summary>
        /// <returns></returns>
        public SelectList GetSpatialReference()
        {
            SettingsHelper settingsHelper = new SettingsHelper("Vat");
            string values = settingsHelper.GetValue("SpatialRefrences");

            if (string.IsNullOrEmpty(values)) return new SelectList(new List<string>());
         
            return new SelectList(values.Split(','));
        }

        /// <summary>
        /// load list of variables based on the dataset id 
        /// </summary>
        /// <param name="datasetId"></param>
        /// <returns></returns>
        public SelectList GetVariables(long datasetId)
        {
             if (datasetId <= 0) throw new Exception("Id is missing.");

            using (var datasetManager = new DatasetManager())
            using (var dataStructureManager = new DataStructureManager())
            {
                // load dataset and get datasturctureid
                var dataset = datasetManager.GetDataset(datasetId);

                if(dataset == null) throw new Exception("Dataset with id "+ datasetId + " not exist");

                var structure = dataStructureManager.StructuredDataStructureRepo.Get(dataset.DataStructure.Id);

                // check if datastrutcure is structured
                if(structure == null) throw new Exception("Datastructure is not structured");

                if (structure.Variables.Any())
                {
                    // return list of vairables als selection list
                    return new SelectList(structure.Variables.Select(v => v.Label));     
                }
            }

            return new SelectList(new List<string>());

        }


        public bool Save(EditModel model)
        {
            // validate incoming data
            if (model==null) throw new Exception("model is empty.");
            if (model.Id <= 0) throw new Exception("Id is missing.");


            // get current datasettitle
            string layerName = "";

            using (var datasetManager = new DatasetManager())
            {
                var dataset = datasetManager.GetDataset(model.Id);
                if (dataset == null) throw new Exception("Dataset with id "+model.Id+"not exist.");

                XmlDatasetHelper xmlHelpers = new XmlDatasetHelper();
                layerName = xmlHelpers.GetInformation(dataset, NameAttributeValues.title);

                TimeObject to = new TimeObject(80000,model.Time, new Format() { format = "custom", customFormat = model.TimeFormat });


                GeoEngine geoEngine = new GeoEngine(
                        model.Latitude,
                        model.Longitude,
                        model.DataType,
                        model.SpatialReference,
                        layerName,
                        GetTypeSortedVariables(dataset.DataStructure.Id),
                        GetVariablesWithType(dataset.DataStructure.Id),
                        to
                    );

                string path = Path.Combine(AppConfiguration.DataPath,"Datasets",model.Id.ToString(),"geoengine.json");

                // check if directory exist, if not create
                if(!Directory.Exists(Path.GetDirectoryName(path)))
                    FileHelper.CreateDicrectoriesIfNotExist(Path.GetDirectoryName(path));

                // if file exist -> delete
                if (FileHelper.FileExist(path)) File.Delete(path);

                // save json to file
                File.WriteAllText(path, JsonConvert.SerializeObject(geoEngine));

                return true;
            }
        }

        /// <summary>
        /// This function creates a dictionary based on the data structure with datatypes as keys and the corresponding variables.
        /// </summary>
        /// <param name="dataSturctureId"></param>
        /// <returns></returns>
        private Dictionary<string, List<string>> GetTypeSortedVariables(long dataSturctureId)
        {
            Dictionary<string, List<string>> tmp = new Dictionary<string, List<string>>();

            using (var dataStructureManager = new DataStructureManager())
            using (var dataTypeManager = new DataTypeManager())
            using (var dataContainerManager = new DataContainerManager())
            {
                // check if structure is sturctured or files only
                var structuredDataSturcture = dataStructureManager.StructuredDataStructureRepo.Get(dataSturctureId);

                if (structuredDataSturcture == null) throw new Exception("Datastructure is not structured");

                //check if variables exist
                if (structuredDataSturcture.Variables.Any())
                {
                    //go to each variable
                    foreach (var v in structuredDataSturcture.Variables)
                    {
                        // because of lazy loading data needs to be loaded directly
                        // load data attribute from varibale
                        var dataAttribute = dataContainerManager.DataAttributeRepo.Get(v.DataAttribute.Id);
                        if (dataAttribute == null) throw new Exception("DataAttribute with Id "+ v.DataAttribute.Id + "not exist.");

                        // load datatype from data attribute
                        var dataType = dataTypeManager.Repo.Get(dataAttribute.DataType.Id);
                        if (dataType == null) throw new Exception("DataType with Id " + dataType.Id + "not exist.");

                        // get the systemtype name of the datatype and use this as the key for the dictionary
                        var systemstype = dataType?.SystemType;

                        // when systemtype allready exist in the dictionary, update list
                        // otherwhise add a new key to the dictionary with the first variable label in a lis of strings
                        if (tmp.ContainsKey(systemstype)) //key exist
                        {
                            List<string> list = (List<string>)tmp[systemstype];
                            list.Add(v.Label);
                            tmp[systemstype] = list;
                        }
                        else //key not exist 
                        {
                            tmp.Add(systemstype, new List<string>() { v.Label });
                        }

                    }
                }
            }

            return tmp;
        }

        /// <summary>
        /// This function creates a dictionary based on the data structure with variable labels as keys and the corresponding datatype as value.
        /// </summary>
        /// <param name="dataSturctureId"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetVariablesWithType(long dataSturctureId)
        {
            Dictionary<string, string> tmp = new Dictionary<string, string>();

            using (var dataStructureManager = new DataStructureManager())
            using (var dataTypeManager = new DataTypeManager())
            using (var dataContainerManager = new DataContainerManager())
            {
                // check if structure is sturctured or files only
                var structuredDataSturcture = dataStructureManager.StructuredDataStructureRepo.Get(dataSturctureId);

                if (structuredDataSturcture == null) throw new Exception("Datastructure is not structured");

                //check if variables exist
                if (structuredDataSturcture.Variables.Any())
                {
                    //go to each variable
                    foreach (var v in structuredDataSturcture.Variables)
                    {
                        // because of lazy loading data needs to be loaded directly
                        // load data attribute from varibale
                        var dataAttribute = dataContainerManager.DataAttributeRepo.Get(v.DataAttribute.Id);
                        if (dataAttribute == null) throw new Exception("DataAttribute with Id " + v.DataAttribute.Id + "not exist.");

                        // load datatype from data attribute
                        var dataType = dataTypeManager.Repo.Get(dataAttribute.DataType.Id);
                        if (dataType == null) throw new Exception("DataType with Id " + dataType.Id + "not exist.");

                        // get the systemtype name of the datatype and use this as the key for the dictionary
                        var systemstype = dataType?.SystemType;

                        // add variable label and systemtype to dictionary
                        tmp.Add(v.Label, systemstype);

                    }
                }
            }

            return tmp;
        }

        public EditModel Read(long id)
        {
            string path = Path.Combine(AppConfiguration.DataPath, "Datasets", id.ToString(), "geoengine.json");

            EditModel model = new EditModel();


            if (File.Exists(path))
            {
                GeoEngine geoEngine = JsonConvert.DeserializeObject<GeoEngine>(System.IO.File.ReadAllText(path));

                model.Latitude = geoEngine.loadingInfo.x;
                model.Longitude = geoEngine.loadingInfo.y;
                model.DataType = geoEngine.resultDescriptor.dataType;
                model.SpatialReference = geoEngine.resultDescriptor.spatialReference;
                model.Time = geoEngine.loadingInfo.time?.start.startField;
                model.TimeFormat = geoEngine.loadingInfo.time?.start.startFormat.customFormat;
            }

            model.Id = id;
            
            return model;
        }
    }
}