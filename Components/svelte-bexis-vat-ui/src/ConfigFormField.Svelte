<script>
import {  FormGroup, Input, Label } from 'sveltestrap';

export let list = [];
export let name;
export let label;
export let value;
export let placeholder;

</script>

<FormGroup>
  <Label for="{label}">{label}</Label>
  <Input type="select" {placeholder} {name} id="{name}" bind:value>
    {#each list as item}
      <option value={item}>{item}</option>
    {/each}
  </Input>
</FormGroup>

