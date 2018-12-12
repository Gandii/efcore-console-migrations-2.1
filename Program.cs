using System;
using System.IO;
using System.Linq;
using CommandLine;
using ConsoleApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;

namespace ConsoleApp
{
    class Program
    {
        public class Options
        {


            [Option('l', "list", Required = false, HelpText = "List applied Migrations.")]
            public bool List { get; set;}

            [Option('c', "check", Required = false, HelpText = "Check and list missing Migrations.")]
            public bool Check { get; set; }

            [Option('u', "update", Required = false, HelpText = "Apply Migrations.")]
            public bool Update { get; set; }
        }

        public static IConfigurationRoot config;
        public static DbContextOptionsBuilder<MyContext> dbConfig;
        static void Main(string[] args)
        {
            Console.WriteLine("Migration Console App Sample");

            // Get config ffrom appsettings.json.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            config = builder.Build();

            
            // Build an DbContextOptionsBuilder option. This is normally setup using DI and injected .
            /*
            services.AddDbContextPool<mycontext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("defautl"))
                    .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.QueryClientEvaluationWarning))
             );
             */
            dbConfig  = new DbContextOptionsBuilder<MyContext>()
                .UseSqlServer(config.GetConnectionString("default"));


                            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       if (o.Check)
                       {
                           GetPendingMigrations();
                       }
                       else if(o.List)
                       {
                           GetAppliedMigrations();
                       }
                       else if(o.Update){
                           ApplyMigrations();
                       }
  
                   });

        }

        public static void GetAppliedMigrations(){
            using (var db = new MyContext(dbConfig.Options))
            {
                    Console.WriteLine("Applied Migrations:");

                   var migs =  db.Database.GetAppliedMigrations();

                   foreach(var m in migs){
                        Console.WriteLine(m);
                  
                   }
            }
        }
        public static void GetPendingMigrations(){
            using (var db = new MyContext(dbConfig.Options))
            {
             

                   var migs =  db.Database.GetPendingMigrations();

                    if(!migs.Any()){

                 Console.WriteLine("No Pending Migrations");
                        return;
                    }
                 Console.WriteLine("Pending Migrations:");
                   foreach(var m in migs){
                        Console.WriteLine(m);
                   }
            }
        }
            public static void ApplyMigrations(){
            using (var db = new MyContext(dbConfig.Options))
            {
                    Console.WriteLine("Applying Migrations!");

                   db.Database.Migrate();
            }
        }
    }
}
