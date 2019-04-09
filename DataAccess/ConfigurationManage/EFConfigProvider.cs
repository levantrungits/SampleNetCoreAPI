using DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.ConfigurationManage
{
    public class EFConfigProvider : ConfigurationProvider
    {
        Action<DbContextOptionsBuilder> OptionsAction { get; }

        public EFConfigProvider(Action<DbContextOptionsBuilder> optionsAction) => OptionsAction = optionsAction; 

        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<SampleNetCoreAPIContext>();
            OptionsAction(builder);

            using (var dbContext = new SampleNetCoreAPIContext(builder.Options))
            {
                dbContext.Database.EnsureCreated();
                Data = dbContext.SampleNetCoreConfig.ToDictionary(c => c.Key, c => c.Value);
            }
        }
    }
}
