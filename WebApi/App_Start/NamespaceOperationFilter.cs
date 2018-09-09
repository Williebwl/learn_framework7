using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace WebApi
{
    public class NamespaceOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            operation.parameters
                ?.Where(item => item.@in == "path" && item.name == "namespace")
                .ToList()
                .ForEach(item => operation.parameters.Remove(item));
        }
    }
}