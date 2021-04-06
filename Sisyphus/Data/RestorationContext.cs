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
    public class RestorationContext : DbContext
    {
        public RestorationContext() : base()
        {
        }

        public RestorationContext( DbContextOptions<DataContext> options ) : base( options )
        { }
        public DbDataReader RawSqlQuery( string query, int commandTimeout = 60 )
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

        public List<T> RawSqlQuery<T>( string query, Func<DbDataReader, T> map )
        {
            using ( var command = this.Database.GetDbConnection().CreateCommand() )
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                this.Database.OpenConnection();

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
                ConfigurationHelper.Configuration.GetConnectionString( "RestorationConnection" ), o => o.CommandTimeout( 3600 ) );

            base.OnConfiguring( optionsBuilder );
        }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
        }
    }
}
