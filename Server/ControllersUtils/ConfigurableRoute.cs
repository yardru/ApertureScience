using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApertureScience
{
    public class ConfigurableRoute : Attribute, IRouteTemplateProvider
    {
        public string Template => routs[Name];
        public int? Order => 2;
        public string Name { get; set; }
        public ConfigurableRoute(string name) { Name = name; }

        static public void AddRoute(string name, string route) { routs[name] = route; }
        static private readonly Dictionary<string, string> routs = new Dictionary<string, string>();
    }
}
