using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebStore.TagHelpers
{
    [HtmlTargetElement(Attributes = _ACTIVE_ROUTE)]
    public class ActiveRoute : TagHelper
    {
        private const string _ACTIVE_ROUTE = "ws-is-active-route";
        private const string _IGNORE_ACTION = "ws-ignore-action";

        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public Dictionary<string, string> RouteValues { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var isIgnoreAction = output.Attributes.ContainsName(_IGNORE_ACTION);

            if (IsActive(isIgnoreAction))
                MakeActive(output);

            output.Attributes.RemoveAll(_ACTIVE_ROUTE);
        }

        private bool IsActive(bool isIgnoreAction)
        {
            var route = ViewContext.RouteData.Values;
            var currentController = route["controller"]?.ToString();
            var currentAction = route["action"]?.ToString();

            const StringComparison strCmp = StringComparison.OrdinalIgnoreCase;

            if (Controller is { Length: > 0 } controller && !string.Equals(controller, currentController, strCmp))
                return false;

            if (isIgnoreAction)
                return true;

            if (Action is { Length: > 0 } action && !string.Equals(action, currentAction, strCmp))
                return false;

            foreach (var (key, value) in route)
                if (!route.ContainsKey(key) || route[key]?.ToString() != value)
                    return false;

            return true;
        }

        private static void MakeActive(TagHelperOutput output)
        {
            var classAttribute = output.Attributes.FirstOrDefault(a => a.Name == "class");

            if (classAttribute is null)
                output.Attributes.Add("class", "active");

            else
            {
                if (classAttribute.Value.ToString() == "active")
                    return;

                output.Attributes.SetAttribute("class", classAttribute.Value.ToString() + "active");
            }
        }
    }
}
