using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BExIS.VAT.Entities.Data
{
    public enum Type
    { 
        vector
    }

    public enum ResultType
    {
        points,
        lines,
        polygons
    }

    public enum IntervalType
    { 
        Start,
        StartEnd,
        StartDuration,
        StartConstant
    }

    public class ConfigDataJson
    {
        public string name { get; set; }
        public Operator Operator { get; set; }
        public Symbology symbology { get; set; }

        public bool expanded { get; set; }
        public bool visible { get; set; }
        public bool editSymbology { get; set; }
        public Type type { get; set; }

        public TypeOptions typeOptions { get; set; }
    }

    public class TypeOptions
    {
        public bool clustered { get; set; }
    }

    #region symbology

    /*
     * "symbology": {
                "symbologyType": "COMPLEX_POINT",
                "fillRGBA": [255, 0, 0, 0.8],
                "strokeRGBA": [0, 0, 0, 1],
                "strokeWidth": 1,
                "fillColorizer": {
                        "breakpoints": [],
                        "type": "gradient"
                },
                "strokeColorizer": {
                        "breakpoints": [],
                        "type": "gradient"
                },
                "radiusFactor": 1,
                "radius": 5,
                "textAttribute": "id",
                "textColor": [255, 255, 255, 1],
                "textStrokeWidth": null,
                "clustered": false
        },
     * 
    */
    public class Symbology
    {
        public string symbologyType { get; set; }
        //"fillRGBA": [255, 0, 0, 0.8],
        public Color fillRGBA { get; set; }
        //"strokeRGBA": [0, 0, 0, 1],
        public Color strokeRGBA { get; set; }
        public int strokeWidth { get; set; }

        public Colorizer fillColorizer { get; set; }
        public Colorizer strokeColorizer { get; set; }

        public int radiusFactor { get; set; }
        public int radius { get; set; }
        public string textAttribute { get; set; }
        public Color textColor { get; set; }
    }

    public class Colorizer
    {
        public double[] breakPoints { get; set; }
        public string type { get; set; }
    }

    #endregion

    #region operation

    /**
     * "operator": {
                "id": 4,
                "resultType": "points",
                "operatorType": {
                        "operatorType": "ogr_raw_source",
                        "filename": "...",
                        "time": "none",
                        "columns": {
                                "x": "longitude",
                                "y": "latitude",
                                "numeric": [],
                                "textual": ["country"]
                        },
                        "on_error": "abort"
                },
                "projection": "EPSG:4326",
                "attributes": ["id"],
                "dataTypes": [
                        ["country", "Alphanumeric"]
                ],
                "units": [
                        ["id", {
                                "measurement": "unknown",
                                "unit": "unknown",
                                "interpolation": 1,
                                "classes": {}
                        }]
                ],
                "rasterSources": [],
                "pointSources": [],
                "lineSources": [],
                "polygonSources": []
        },
     */
    public class Operator
    {
        //??
        public long id { get; set; }
        public ResultType resultType { get; set; }
        public OperatorType operatorType { get; set; }

        //"EPSG:4326"
        public string projection { get; set; }
        public string[] attributes{ get; set; }

        //column name + datatype
        public KeyValuePair<string,string>[] dataTypes { get; set; }

        public KeyValuePair<string, Unit>[] units { get; set; }

        //??
        public string[] rasterSources { get; set; }
        //??
        public string[] pointSources { get; set; }
        //??
        public string[] lineSources { get; set; }
        //??
        public string[] polygonSources { get; set; }

    }

    public class Unit
    {
        //default:unknown
        public string measurement { get; set; }
        public string unit { get; set; }
        //?? default = 1
        public string interpolation { get; set; }
        //??
        public string classes { get; set; }
    }

    public class OperatorType
    {
        //??
        public String operationType { get {return "ogr_raw_source";}}
        public String fileName { get; set; }
        //time varaiable names ??
        public Time time { get; set; }

        public Columns columns { get; set; }

        public string on_error { get { return "abort"; }}
    }

    public class Columns
    {
        public string x { get; set; }
        public string y { get; set; }
        //??
        public string[] numeric{ get; set; }
        //"textual": ["country"]
        public string[] textual { get; set; }
    }

    public class Time
    {
        //values??
        public IntervalType intervalType { get; set; }
        public String start { get; set; }
        public String startFormat { get { return "yyyy-MM-ddTHH:mm:ss"; } }
        public String end { get; set; }
        public String endFormat { get { return "yyyy-MM-ddTHH:mm:ss"; } }

    }

    #endregion
}
