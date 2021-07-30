using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BExIS.Modules.VAT.UI.Models
{
    public class GeoEngine
    {
        public loadingInfo loadingInfo { get; set; }
        public resultDescriptor resultDescriptor { get; set; }

        public GeoEngine(
            string x, 
            string y, 
            string datatype,
            string spatialReference,
            string layerName,
            Dictionary<string, List<string>> loadingInfoColumns,
            Dictionary<string, string> resultDescriptorColumns,
            TimeObject start = null,
            TimeObject end = null
            )
        {
            loadingInfo = new loadingInfo(x,y,loadingInfoColumns, datatype, layerName, start, end);
            resultDescriptor = new resultDescriptor(resultDescriptorColumns,datatype,spatialReference);
        }
    }

    public class loadingInfo
    {
        /// <summary>
        /// keys is one of this types
        /// e.g. float, int, text
        /// avlue is the list of variables in the structure with the same type
        /// type,list of columns
        /// 
        /// </summary>
        public Dictionary<string, List<string>> columns;

        public string x { get; set; }
        public string y { get; set; }

        /// <summary>
        /// e.g. MultiPoint
        /// </summary>
        public string dataType { get; set; }

        /// <summary>
        /// filename from the data file
        /// </summary>
        public string fileName { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        public bool forceOgrTimeFilter { get; set; }

        /// <summary>
        /// most like filename without extentions
        /// </summary>
        public string layerName { get; set; }

        public string onError { get; set; }
        public Time time { get; set; }


        public loadingInfo()
        {
            columns = new Dictionary<string, List<string>>();
            dataType = "";
            fileName = "";
            forceOgrTimeFilter = false;
            layerName = "";
            onError = "ignore";
            time = new Time();
        }

        public loadingInfo(string _x, string _y, Dictionary<string, List<string>> _columns, string _datatype, string _layername, TimeObject start = null, TimeObject end = null)
        {
            x = _x;
            y = _y;
            columns = _columns;
            dataType = _datatype;
            layerName = _layername;
            fileName = layerName + ".txt;";
            forceOgrTimeFilter = false;
            onError = "ignore";
            time = new Time();

            if (start!=null) time.start = start;
            if (end!=null) time.end = end;
        }
    }

    public class Time
    {
        public TimeObject start { get; set; }
        public TimeObject end { get; set; }

        public Time()
        {
            start = new TimeObject();
            end = new TimeObject();
        }

        public Time(TimeObject _start, TimeObject _end)
        {
            start = _start;
            end = _end;
        }
    }

    public class TimeObject
    {
        public int duration { get; set; }
        public string startField { get; set; }
        public Format startFormat { get; set; }

        public TimeObject()
        { 
            duration = 800000;
            startField = "";
            startFormat = new Format();
        }

        public TimeObject(int _duration, string _startField, Format _startFormat)
        {
            duration = _duration;
            startField = _startField;
            startFormat = _startFormat;
        }
    }

    public class Format
    {
        public string customFormat { get; set; }
        public string format { get; set; }

        public Format()
        {
            customFormat = "";
            format = "";
        }
    }

    public class resultDescriptor
    {
        /// <summary>
        /// keys is one of this types
        /// e.g. float, int, text
        /// avlue is the list of variables in the structure with the same type
        /// type,list of columns
        /// 
        /// 
        /// </summary>
        public Dictionary<string, string> columns;

        public string dataType { get; set; }

        public string spatialReference { get; set; }


        public resultDescriptor()
        {
            columns = new Dictionary<string, string>();
            dataType = "";
            spatialReference = "";
        }

        public resultDescriptor(Dictionary<string, string>  _columns, string _dataType, string _spatialReference)
        {
            columns = _columns;
            dataType = _dataType;
            spatialReference = _spatialReference;
        }
    }


}