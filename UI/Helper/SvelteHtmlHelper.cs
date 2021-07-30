using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BExIS.UI.Helpers
{
    public static class BexisVatSvelte
    {
        public static IHtmlString SvelteVatScript(this HtmlHelper helper)
        {
            string script = "<script src=\"/Areas/VAT/UI/Scripts/svelte/vat.js\" type=\"text/javascript\"></script>";

            StringBuilder sb = new StringBuilder();
            //sb.AppendLine(link);
            sb.AppendLine(script);

            return new MvcHtmlString(sb.ToString());
        }

        public static IHtmlString SvelteVatForm(this HtmlHelper helper, long? datasetId)
        {
            TagBuilder tb = new TagBuilder("div");
            tb.Attributes.Add("id", "SvelteVatForm");
            tb.Attributes.Add("datasetId", datasetId.ToString());

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(tb.ToString());

            return new MvcHtmlString(sb.ToString());
        }
    }
}

