# Bexis to VAT Concept


## VAT

### csv import steps

| step | description | User/Sytem from Bexis | Json
|---|---|---| --- |
| Data | CSV Stettings | System | operator/columns/x, operator/columns/y 
| Spatial | In this step you can specify the spatial columns of your CSV file. | User |
| Temporal | This step allows you to specify temporal columns of your CSV file. | User | operator/time
| Type | You can specify the data types of the remaining columns here. | System | operator/dataTypes
| Layer | Choose on error behavior and layer name. | System | symbology


### Attributes Import vs Json

|step | attribute | Json |
|---|---|---|
Data |    Delimeter | ?
Data|     Decimal seperator| ?
Data|     Text qualifier| ?
Data|     Header Row| ?
Spatial|  Coordinate Format| ?
Spatial|  Coordinate column| operator/operationType/numeric
Spatial|  longitude-Coordinate column| operator/operationType/x
Spatial|  latitude-Coordinate column| operator/operationType/y
Spatial|  Spatial Reference System| operator/projection
Spatial|  Result Type| operator/resultType
Spatial|  WKT| ?
Temporal| the file inlcudes temporal data| operator/time = none - true or false?
Temporal| interval type| operator/time/intervalType ?
Temporal| Start Column| operator/time/startColumn ?
Temporal| Start Format| operator/time/startFormat ?
Temporal| End Column| operator/time/endColumn ?
Temporal| End Format| operator/time/endFormat ?
Type|     KEY Value Pair, Attr Name - Type|
Layer| Layer Name| name
Layer| On error behavior| operator/on_error



## BEXIS 2

### Spatial

- the spatial information can come from metadata or primary data.
- as coordinates or as name, example city
- at coordinaten is currently EPSG:4326 WGS 84
- conversion may be necessary




## Examples

### Json example to send via api to vat

```json

 {
        "name": "new feature layer",
        "operator": {
                "id": 4,
                "resultType": "points",
                "operatorType": {
                        "operatorType": "ogr_raw_source",
                        "filename": "http://www.sharecsv.com/dl/8e3433d6d5ac99edd2892c0c2b20b696/covid19_points_stripped.csv",
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
        "symbology": {
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
        "expanded": false,
        "visible": true,
        "editSymbology": false,
        "type": "vector",
        "typeOptions": {
                "clustered": false
        }
 }


```