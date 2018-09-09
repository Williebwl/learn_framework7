using System;
using BIStudio.Framework.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BIFramework.Test
{
    [TestClass]
    public class CacheTest
    {
        [TestMethod]
        public void LocalCacheTest()
        {
            string moduleName = "BIFramework.Local";
            string key = "testKey";
            CacheService.Default.Clear(moduleName);
            CacheItem item = CacheService.Default.GetOrAdd("testKey",
                () => new CacheItem { Id = 0, Name = "测试", CreateTime = DateTime.Now }, module: moduleName);
            CacheService.Default.Remove(moduleName, key);
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void RedisLogTest()
        {
            string moduleName = "BIFramework.Remoting";
            string key = "testKey";
            CacheService.Default.Clear(moduleName);
            CacheItem item = CacheService.Default.GetOrAdd("testKey",
                () => new CacheItem { Id = 0, Name = "测试", CreateTime = DateTime.Now }, module: moduleName);
            CacheService.Default.Remove(moduleName, key);
            Assert.IsNotNull(item);
        }

        public class CacheItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime CreateTime { get; set; }
        }
    }
}