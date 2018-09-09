using BIStudio.Framework.Data.Adapter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Security.Organization
{
    [NotMapped]
    public class ModuleDTO : ModuleInfo
    {
        public IList<ModuleDTO> Infos { get; set; }
    }
}
