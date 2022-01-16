using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {   
        //creating variable of type IConfiguration
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            //instatniating variable here in the constructor at runtime
            _configuration = configuration;
        }

        // =================== GETS =====================================

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            SELECT DepartmentId, DepartmentName
                            FROM dbo.Department
                            ";

            DataTable table = new DataTable();

            // Getting connection string from appsettings.json using the configuration instance
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            // use data reader to fill data table with data that comes from the Db query 
            SqlDataReader myReader;

            // executing Db commands with the using class to make sure to close the connection even if there ar exceptions
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                // first open the connection
                myCon.Open();
                
                // using the query above we create a command
                using(SqlCommand myCommand=new SqlCommand(query, myCon))
                {
                    // execute command using the ExecuteReader method since we are expecting a return value from th select query
                    myReader = myCommand.ExecuteReader();

                   // Fill the table with the returned data
                    table.Load(myReader);

                    // Close the reader and the connection
                    myReader.Close();
                    myCon.Close();
                }
            }

            // return the data in JSON format
            return new JsonResult(table);
        }

        // ================= INSERTS ===========================
        [HttpPost]
        public JsonResult Post(Department dep)
        {
            string query = @"
                            INSERT INTO dbo.Department
                            values (@DepartmentName)
                            ";

            DataTable table = new DataTable();

            // Getting connection string from appsettings.json using the configuration instance
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            // use data reader to fill data table with data that comes from the Db query 
            SqlDataReader myReader;

            // executing Db commands with the using class to make sure to close the connection even if there ar exceptions
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                // first open the connection
                myCon.Open();

                // using the query above we create a command
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    //Passing the values to the command here using command parameters !!! This will avoid sql injections
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);

                    // execute command using the ExecuteReader method since we are expecting a return value from th select query
                    myReader = myCommand.ExecuteReader();

                    // Fill the table with the returned data
                    table.Load(myReader);

                    // Close the reader and the connection
                    myReader.Close();
                    myCon.Close();
                }
            }

            // returning Db return message
            return new JsonResult("Insert = Successful");
        }

        // ======= UPDATES =======================
        [HttpPut]
        public JsonResult Put(Department dep)
        {
            string query = @"
                            UPDATE dbo.Department
                            SET DepartmentName = @DepartmentName
                            WHERE DepartmentId = @DepartmentId
                            ";

            DataTable table = new DataTable();

            // Getting connection string from appsettings.json using the configuration instance
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            // use data reader to fill data table with data that comes from the Db query 
            SqlDataReader myReader;

            // executing Db commands with the using class to make sure to close the connection even if there ar exceptions
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                // first open the connection
                myCon.Open();

                // using the query above we create a command
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    //Passing the values to the command here using command parameters !!! This will avoid sql injections
                    myCommand.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);

                    // execute command using the ExecuteReader method since we are expecting a return value from th select query
                    myReader = myCommand.ExecuteReader();

                    // Fill the table with the returned data
                    table.Load(myReader);

                    // Close the reader and the connection
                    myReader.Close();
                    myCon.Close();
                }
            }

            // return message
            return new JsonResult("Update = Successful");
        }

        // ==================== DELETE ===========================
        //[HttpDelete("{id}")] Use this if you want the Id to come over at the  end of the url: api/Department/4
        [HttpDelete]
        public JsonResult Delete(Department dep)
        {
            string query = @"
                            DELETE FROM dbo.Department
                            WHERE DepartmentId = @DepartmentId
                            ";

            DataTable table = new DataTable();

            // Getting connection string from appsettings.json using the configuration instance
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            // use data reader to fill data table with data that comes from the Db query 
            SqlDataReader myReader;

            // executing Db commands with the using class to make sure to close the connection even if there ar exceptions
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                // first open the connection
                myCon.Open();

                // using the query above we create a command
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    //Passing the values to the command here using command parameters !!! This will avoid sql injections
                    myCommand.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);

                    // execute command using the ExecuteReader method since we are expecting a return value from th select query
                    myReader = myCommand.ExecuteReader();

                    // Fill the table with the returned data
                    table.Load(myReader);

                    // Close the reader and the connection
                    myReader.Close();
                    myCon.Close();
                }
            }

            // return message
            return new JsonResult("Deletion = Successful");
        }

    }
}
