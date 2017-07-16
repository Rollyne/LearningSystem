using System.Collections.Generic;
using System.Web.Mvc;

namespace LearningSystem.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString BooleanCheckbox(this HtmlHelper helper, bool? state, string name,
            string value = "true", object htmlAttributes = null)
        {
            var tag = new TagBuilder("input");
            tag.Attributes.Add("type", "checkbox");
            tag.Attributes.Add("name", name);
            tag.Attributes.Add("value", value);
            if (state ?? false)
            {
                tag.Attributes.Add("checked", "checked");
            }

            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                tag.MergeAttributes(attributes);
            }
            
            return new MvcHtmlString(tag.ToString());
        }
    }
}