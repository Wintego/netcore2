using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.TagHelpers
{
    [HtmlTargetElement(Attributes = AttributeName)]
    public class ActiveRouteTagHelper : TagHelper
    {
        public const string IgnoreActionName = "ignore-action";
        public const string AttributeName = "is-active-route";
        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }
        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }
        public IDictionary<string, string> _RouteValues;
        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix ="asp-route-")]
        public IDictionary<string, string> RouteValues
        {
            get
            {
                return _RouteValues ??
                    (_RouteValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
            }
            set => _RouteValues = value;
        }
        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            var ignore_action = context.AllAttributes.TryGetAttribute(IgnoreActionName, out _);
            if (isActive(ignore_action))
                MakeActive(output);

            output.Attributes.RemoveAll(AttributeName);
        }
        private bool isActive(bool ignore_action)
        {
            var route_values = ViewContext.RouteData.Values;
            var current_controller = route_values["Controller"].ToString();
            var current_action = route_values["Action"].ToString();

            const StringComparison ignore_case = StringComparison.CurrentCultureIgnoreCase;
            if (string.IsNullOrWhiteSpace(Controller) || !string.Equals(Controller, current_controller, ignore_case))
                return false;

            if(!ignore_action && !string.IsNullOrWhiteSpace(Action) && !string.Equals(Action, current_action, ignore_case))
                return false;

            foreach (var (key, value) in RouteValues)
            {
                if (route_values.ContainsKey(key) || route_values[key].ToString() != value)
                    return false;
            }
            return true;
        }
        private static void MakeActive(TagHelperOutput output)
        {
            var class_attribute = output.Attributes.FirstOrDefault(a => a.Name == "class");
            if (class_attribute is null)
            {
                class_attribute = new TagHelperAttribute("class", "active");
                output.Attributes.Add(class_attribute);
            }
            else if(class_attribute.Value?.ToString().Contains("active", StringComparison.Ordinal)!=null)
            {
                output.Attributes.SetAttribute("class", class_attribute.Value is null ? "class" : class_attribute.Value + " class");
            }
        }
    }
}
