<script>

import { Form, Button } from 'sveltestrap';
import Field from './ConfigFormField.svelte'
import { onMount } from 'svelte'; 

import { ApiUrl, variables, dataTypes, spatialReferences, model, fetchData, fetchVariables } from './store.js'
 
export let Id;
// export let FormIsValid = false;


let container

onMount(() => {

    console.log("inside svelte");

    container = document.getElementById("SvelteVatForm");
    if(container !== undefined)
    {
      Id = container.getAttribute("datasetId");
    }
    console.log(container);
    console.log("Id" + Id);
    fetchVariables(Id);
    fetchData(Id);
})

async function handleSubmit() {
  
  alert("click");

  let url = ApiUrl +"vat/config/edit"
  console.log(url);

    const options = {
      method: "POST",
      body: JSON.stringify($model),
      headers: {
        "Content-Type": "application/json",
      },
    };


    fetch(url, options).then((response) => { 
      alert("test");
    });
  }

</script>

<h1>test3</h1>



<Form> 

<Field 
  label="Longitude"
  name="Longitude"
  placeholder="select"
  bind:value={$model.Longitude}
  list={$variables}
  />

<Field 
  label="Latitude"
  name="Latitude"
  placeholder="select"
  bind:value={$model.Latitude}
  list={$variables}
  />

<Field 
  label="Time"
  name="Time"
  placeholder="select"
  bind:value={$model.Time}
  list={$variables}
  />

<Field 
  label="DataType"
  name={model.DataType}
  placeholder={model.DataType}
  bind:value={$model.DataType}
  list={$dataTypes}
  />

<Field 
  label="SpatialReference"
  name={model.SpatialReference}
  placeholder={model.SpatialReference}
  bind:value={$model.SpatialReference}
  list={$spatialReferences}
  />

</Form>

<Button on:click={handleSubmit}> <!-- disabled={!FormIsValid}> -->
 Save
</Button>
