<!-- VAT.stories.svelte -->
<script>

import Form from "./ConfigForm.svelte";
import {Button} from '@bexis2/svelte-bexis-core-ui';
import {Treeview} from '@bexis2/svelte-bexis-core-ui';
import {
   ButtonDropdown,
   DropdownItem,
   DropdownMenu,
   DropdownToggle
 } from 'sveltestrap';

let Id = 10;

let args = {
 href:"api/test/id=",
 checkboxhref:"api/test/id=",
 showcheckbox:false,
 showcount:false,
 target:""}

function click()
{
 window.open("/ddm/home/index","_self")
}

</script>
<svelte:head>
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css">
</svelte:head>

<h1>test </h1>
<b> button and tree from ui core</b>

<Button on:click="{click}">open new page</Button>

<Button></Button>
<Treeview {...args}></Treeview>

<b> form from modul</b>
<Form></Form>


<select class="form-select bx-dropdown" aria-label="Default select example">
 <option selected>Open this select menu</option>
 <option value="1">One</option>
 <option value="2">Two</option>
 <option value="3">Three</option>
</select>



<ButtonDropdown>
 <DropdownToggle color="primary" caret>Click Me</DropdownToggle>
 <DropdownMenu>
   <DropdownItem header>Header</DropdownItem>
   <DropdownItem>Some Action</DropdownItem>
   <DropdownItem disabled>Action (disabled)</DropdownItem>
   <DropdownItem divider />
   <DropdownItem>Foo Action</DropdownItem>
   <DropdownItem>Bar Action</DropdownItem>
   <DropdownItem>Quo Action</DropdownItem>
 </DropdownMenu>
</ButtonDropdown>