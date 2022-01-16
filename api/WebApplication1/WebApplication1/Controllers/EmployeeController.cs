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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        //creating variable of type IConfiguration
        private readonly IConfiguration _configuration;
        
        private readonly IWebHostEnvironment _environment; // use dependency injection to get path to Photos folder
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        // =================== GETS =====================================

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            SELECT EmployeeId, EmployeeName, Department,
                            convert(varchar(10),DateOfJoining, 120) AS DateOfJoining,
                            PhotoFileName
                            FROM dbo.Employee
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            // use data reader to fill data table with data that comes from the Db query 
            SqlDataReader myReader;


            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                // first open the connection
                myCon.Open();

                // using the query above we create a command
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {

                    myReader = myCommand.ExecuteReader();

                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            // return the data in JSON format
            return new JsonResult(table);
        }

        // ================= INSERTS ===========================
        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string query = @"
                            INSERT INTO dbo.Employee
                            (EmployeeName, Department, DateOfJoining, PhotoFileName)
                            values (@EmployeeName, @Department, @DateOfJoining, @PhotoFileName)
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                // first open the connection
                myCon.Open();

                // using the query above we create a command
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    //Passing the values to the command here using command parameters !!! This will avoid sql injections
                    myCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", employee.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);

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
        public JsonResult Put(Employee employee)
        {
            string query = @"
                            UPDATE dbo.Employee
                            SET EmployeeName = @EmployeeName,
                            Department = @Department,
                            DateOfJoining = @DateOfJoining,
                            PhotoFileName = @PhotoFileName
                            WHERE EmployeeId = @EmployeeId
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                // first open the connection
                myCon.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    myCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", employee.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);

                    // execute command using the ExecuteReader method since we are expecting a return value from th select query
                    myReader = myCommand.ExecuteReader();

                    // Fill the table with the returned data
                    table.Load(myReader);

                    // Close the reader and the connection
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Update = Successful");
        }

        // ==================== DELETE ===========================
        //[HttpDelete("{id}")] Use this if you want the Id to come over at the  end of the url: api/Department/4
        [HttpDelete]
        public JsonResult Delete(Employee employee)
        {
            string query = @"
                            DELETE FROM dbo.Employee
                            WHERE EmployeeId = @EmployeeId
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                // first open the connection
                myCon.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {

                    myCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);

                    myReader = myCommand.ExecuteReader();

                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deletion = Successful");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];

                // extract the file name from form data
                string fileName = postedFile.FileName;

                // Creating physical path
                var physicalPath = _environment.ContentRootPath + "/Photos/" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(fileName);
            }
            catch (Exception)
            {
                // If ther was an exception, just reurn an annonymous image
                return new JsonResult("anonymous.png");    
            }
        }
    }
}
