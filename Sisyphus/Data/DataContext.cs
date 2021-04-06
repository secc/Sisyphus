using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sisyphus.Models;
using Sisyphus.Utilities;

namespace Sisyphus.Data
{
    public class DataContext : DbContext
    {
        public DataContext() : base()
        {
        }

        public DataContext( DbContextOptions<DataContext> options ) : base( options )
        {
        }
        public DbSet<Bundle> Bundles { get; set; }
        public DbSet<BundleOperation> BundleOpperations { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<AppLog> AppLogs { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }

        public DbDataReader RawSqlQuery( string query, int commandTimeout = 300 )
        {

            using ( var command = this.Database.GetDbConnection().CreateCommand() )
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                this.Database.OpenConnection();
                this.Database.SetCommandTimeout( commandTimeout );

                command.CommandTimeout = commandTimeout;

                return command.ExecuteReader();
            }
        }

        public List<T> RawSqlQuery<T>( string query, Func<DbDataReader, T> map, int commandTimeout = 300 )
        {

            using ( var command = this.Database.GetDbConnection().CreateCommand() )
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                this.Database.OpenConnection();
                this.Database.SetCommandTimeout( commandTimeout );

                using ( var result = command.ExecuteReader() )
                {
                    var entities = new List<T>();

                    while ( result.Read() )
                    {
                        entities.Add( map( result ) );
                    }

                    return entities;
                }
            }
        }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(
                ConfigurationHelper.Configuration.GetConnectionString( "DefaultConnection" ), o => o.CommandTimeout( 3600 ) );

            base.OnConfiguring( optionsBuilder );
        }
        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
        }
    }
}
