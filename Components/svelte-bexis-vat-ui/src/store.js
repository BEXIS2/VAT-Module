import {writable} from 'svelte/store';

export const ApiUrl = "https://localhost:44345/";
//export const ApiUrl = "http://localhost:8081/";

// data

export const treeviewData = writable([]);

const fetchTreeviewData = async () => {
   const url = ApiUrl +'vat/test/GetTreeviewData';
   const res = await fetch(url);
   fetchTreeviewData.set(await res.json());

}
fetchTreeviewData();

export const model = writable({
  Id :0,
  Longitude :'',
  Latitude :'',
  DataType:'',
  Time:'',
  SpatialReference:''
});

export const fetchData = async (id) => {
 const url = ApiUrl +'vat/config/edit/'+id;
 const res = await fetch(url);
 model.set(await res.json());
}

//fetchData(0);

// variables

export const variables = writable([]);

export const fetchVariables = async (id) => {
   const url = ApiUrl +'vat/config/Variables/'+id;
   const res = await fetch(url);
   variables.set(await res.json());
}

//fetchVariables(0);

// datatypes

export const dataTypes = writable([]);

const fetchDataTypes = async () => {
   const url = ApiUrl +'vat/config/DataTypes';
   const res = await fetch(url);
   dataTypes.set(await res.json());
   
}

fetchDataTypes();

//spatial references

export const spatialReferences = writable([]);

const fetchSpatialReferences = async () => {
   const url = ApiUrl +'vat/config/spatialReferences';
   const res = await fetch(url);
   spatialReferences.set(await res.json());

}

fetchSpatialReferences();


