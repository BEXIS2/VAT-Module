<!-- VAT.stories.svelte -->
<script>
import Form from "./ConfigForm.svelte";
import { Treeview}  from '@bexis2/svelte-bexis2-core-ui';
import { treeviewData } from './store.js'

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
 showcount:false
}

function click()
{
 window.open("/ddm/home/index","_self")
}

</script>

<h1>test </h1>
<b> button and tree from ui core</b>

<Treeview {...args}></Treeview>

<b> form from modul</b>
<Form></Form>
