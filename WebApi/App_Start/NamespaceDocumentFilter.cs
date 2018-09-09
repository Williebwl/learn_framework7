using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Description;

namespace WebApi
{
    public class NamespaceDocumentFilter : IDocumentFilter
    {
        static Regex rx = new Regex(@"\{namespace\}/(\w+)\.(\w+)/", RegexOptions.Compiled);

        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            swaggerDoc.paths = swaggerDoc.paths.ToDictionary(path =>
            {
                var match = rx.Match(path.Key);
                if (match.Success)
                    return path.Key.Replace(match.Value, match.Groups[1] + "/" + match.Groups[2] + "/");
                return path.Key;
            }, path => path.Value);
        }
    }
}