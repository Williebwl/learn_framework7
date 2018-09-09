using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BIFramework.Test
{
    public static class AssertExtend
    {
        public static void IsNotEmpty<T>(IEnumerable<T> items)
        {
            Assert.IsNotNull(items);
            Assert.IsTrue(items.Any());
        }
        public static void IsNullOrEmpty<T>(IEnumerable<T> items)
        {
            if (items != null)
                Assert.IsFalse(items.Any());
            else
                Assert.IsNull(items);
        }
    }
}
