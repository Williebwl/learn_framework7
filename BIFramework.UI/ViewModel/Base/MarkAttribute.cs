using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.UI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MarkAttribute : Attribute
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        public int Level { get; set; }

        public TypeInfo Type { get; set; }
    }
}
