using System.Collections.Concurrent;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection;

namespace BIStudio.Framework.Domain.EntityFramework
{
    using BIStudio.Framework.Data;

    public class DynamicContext : DbContext
    {
        private static ConcurrentDictionary<string, DbCompiledModel> registeredModels = new ConcurrentDictionary<string, DbCompiledModel>();
        private static DbConnection GetConnection(string connectionName)
        {
            var connection = CFConfig.GetConnection(connectionName);
            var efConnection = DbProviderFactories.GetFactory(connection.ProviderName).CreateConnection();
            efConnection.ConnectionString = connection.ConnectionString;
            return efConnection;
        }
        internal static DbCompiledModel GetCompiledModel(string connectionName)
        {
            return registeredModels.GetOrAdd(connectionName, connectionString =>
            {
                var builder = new DbModelBuilder();
                CFConfig.Default.ParallelScanAttributes<TableAttribute>((type, attr) =>
                {
                    //注册实体类型
                    typeof(DynamicContext).GetMethod("ConfigMap", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(type).Invoke(null, new object[] { builder });
                    //设置TableAttribute属性
                    var config = builder.Types().Where(entity => entity == type);
                    config.Configure(entity =>
                    {
                        var entityConfig = DataEntityUtils.Entity(type);
                        entity.HasEntitySetName(entityConfig.TableAttribute.TableName);
                        entity.ToTable(entityConfig.TableAttribute.TableName);
                        entity.HasKey(entityConfig.TableAttribute.PrimaryKey);
                        foreach (var field in entityConfig.ColumnAttributes)
                        {
                            if (field.Value.IsExtend/* && !field.Value.IsInherit*/)
                                entity.Ignore(field.Key);
                        }
                    });
                });
                
                DbConnection connection = GetConnection(connectionString);
                DbModel model = builder.Build(connection);
                return model.Compile();
            });
        }
        private static void ConfigMap<T>(DbModelBuilder builder) where T : class
        {
            //Type type = typeof(T);
            builder.Entity<T>().Map(config =>
            {
                //将父类discriminator设置为-1，子类discriminator设置为1
                //config.Requires("discriminator").HasValue(1);
                config.MapInheritedProperties();
            });
        }

        static DynamicContext()
        {
            Database.SetInitializer(new NullDatabaseInitializer<DynamicContext>());
        }

        public DynamicContext(string connectionName)
            : base(GetConnection(connectionName), GetCompiledModel(connectionName), true)
        {
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
            this.Database.Initialize(false);
        }

    }
}
