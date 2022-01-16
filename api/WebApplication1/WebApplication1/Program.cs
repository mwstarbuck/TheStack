using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class Program
    {
        /*
         * 1.launchsettings.json: contains all the details about how the program should be started
         * 2. Controllers folder: contains controllers in which are written the api methods
         * 3. appsettings.json: keeps configuration details such as Db setails
         * 4. Progrram.cs contains the Main() program which is the entry point for the app and the creates the webhost
         * 5. startup.cs configures all the services needed by the app.
         *      Services are reusable components that can be used across the app using dependency injection.
         *      Also contains the configure method which creates the apps request processing pipeline 
         * 6. If connecting to a Db, the connection string needs to be added to the appsettings.json file
         *  Ex: "ConnectionStrings": {
                    "EmployeeAppCon": "Data Source=DESKTOP-TC5URPS;Initial Catalog=mytestdb;Integrated Security=SSPI;"
                 },
         * 7. You need to install system.Data.sql client through Nuget then add namespace in using statement for each controller -- using System.Data.SqlClient;
         * 8. To read connection strings you need to use dependency injection in controller
         *      in controller class use:
         *              creating variable of type IConfiguration
                        private readonly IConfiguration _configuration;

                        //Create a constructor in controller and instatniate variable in the constructor at runtime
                        public DepartmentController(IConfiguration configuration)
                        {
                            
                                _configuration = configuration;
                        }
         * If  you are using a Folder containing static files, you must give instructions to use folder in in 
         * the Configure() method in Startup.cs

         */

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // creates web host that enable host to listen to http requests
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
