using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using BIStudio.Framework;
using BIStudio.Framework.Domain;
using BIStudio.Framework.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApi.Controllers.Core
{
    public class FormController : ApplicationService
    {
        protected static MarkAttribute[] ValidatableObject = null;


        protected static readonly JsonSerializerSettings jsonSeting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        [HttpGet]
        public JObject GetFormValidInfo([FromUri]string mark)
        {
            mark = (mark ?? string.Empty).Trim(',', ' ');

            if (string.IsNullOrEmpty(mark)) return null; ;

            if (ValidatableObject == null) GetValidatableObject();

            return mark.Split(',').AsParallel().Aggregate(new JObject(), (jo, d) =>
            {
                var vo = ValidatableObject.AsParallel().Where(vm => vm.Name == d).FirstOrDefault();

                if (vo == null) goto End;

                jo.Add(d, JToken.FromObject(ModelValidator.Default.GetPropertyValidationJson(vo.Type)));

                End: return jo;
            });
        }

        [NonAction]
        protected void GetValidatableObject()
        {
            ValidatableObject = CFConfig.Default.ParallelGetTypes(t => typeof(IValidatableObject).IsAssignableFrom(t))
                                                .AsParallel().Select(d =>
                                                {
                                                    var attr = d.GetCustomAttribute<MarkAttribute>(false) ?? new MarkAttribute();

                                                    attr.Name = (attr.Name ?? string.Empty).Trim();

                                                    if (string.IsNullOrEmpty(attr.Name)) attr.Name = d.Name;

                                                    attr.FullName = d.FullName;
                                                    attr.Type = d;

                                                    return attr;
                                                }).OrderByDescending(d => d.Level).ToArray();
        }
    }
}