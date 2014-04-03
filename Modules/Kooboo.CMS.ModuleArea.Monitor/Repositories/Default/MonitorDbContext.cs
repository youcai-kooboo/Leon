using Kooboo.CMS.ModuleArea.Monitor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor.Repositories.Default
{
    public class MonitorDbContext:DbContext
    {
        static System.Data.Entity.Infrastructure.DbCompiledModel _dbCompiledModel = null;

        static MonitorDbContext()
        {
            var builder = new DbModelBuilder();
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !String.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                builder.Configurations.Add(configurationInstance);
            }
            _dbCompiledModel = builder.Build(new System.Data.Entity.Infrastructure.DbProviderInfo("System.Data.SqlClient", "2008")).Compile();
        }

        public MonitorDbContext(string connStr)
            : base(connStr, _dbCompiledModel)
        {
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }


        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
        //        .Where(type => !String.IsNullOrEmpty(type.Namespace))
        //        .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
        //    foreach (var type in typesToRegister)
        //    {
        //        dynamic configurationInstance = Activator.CreateInstance(type);
        //        modelBuilder.Configurations.Add(configurationInstance);
        //    }
        //    base.OnModelCreating(modelBuilder);
        //}

        public DbSet<Log> Logs { get; set; }
        public DbSet<Hyperlink> Hyperlinks { get; set; }
    }
}
