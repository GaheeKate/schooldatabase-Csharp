﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Assignment3.Models; 
using MySql.Data.MySqlClient;
using System.Web.Http.Cors;



namespace Assignment3.Controllers
{
    public class ClassDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext School = new SchoolDbContext();

        //This Controller Will access the Class table of our school database.
        /// <summary>
        /// Returns a list of Classes in the system if input is null or return an individual class object by SearchKey
        /// </summary>
        /// <example>GET api/ClassData/ListClasses/pr
        /// <returns>
        /// <Class>
        /// <ClassCode>http5102</ClassCode>
        /// <ClassId>2</ClassId>
        /// <ClassName>Project Management</ClassName>
        /// <FisihiDate>2018-12-14T00:00:00</FisihiDate>
        /// <StartDate>2018-09-04T00:00:00</StartDate>
        /// <TeacherId>2</TeacherId>
        /// </Class>
        /// <Class>
        /// <ClassCode>http5103</ClassCode>
        /// <ClassId>3</ClassId>
        /// <ClassName>Web Programming</ClassName>
        /// <FisihiDate>2018-12-14T00:00:00</FisihiDate>
        /// <StartDate>2018-09-04T00:00:00</StartDate>
        /// <TeacherId>5</TeacherId>
        /// </Class>
        /// </returns>
        
        [HttpGet]
        [Route("api/ClassData/ListClasses/{SearchKey?}")]

        public IEnumerable<Class> ListClasses(string SearchKey = null) //method read only
        {
            //Create an instance of a connection
            //Access database through access database method which we created in the Models 
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
       
            cmd.CommandText = "Select * from classes where lower(ClassName) like lower(@key)";
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();
            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teacher
            List<Class> Classes = new List<Class> {};

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {

                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                int ClassId = (int)ResultSet["classid"];
                string ClassCode = (string)ResultSet["classcode"];
                DateTime StartDate = (DateTime)ResultSet["startdate"];
                DateTime FisihiDate = (DateTime)ResultSet["finishdate"];
                string ClassName = (string)ResultSet["classname"];

                Class NewClass = new Class();
                NewClass.TeacherId = TeacherId;
                NewClass.ClassId = ClassId;
                NewClass.ClassCode = ClassCode;
                NewClass.StartDate = StartDate;
                NewClass.FisihiDate = FisihiDate;
                NewClass.ClassName = ClassName;



                //Add the Teacher to the List
                Classes.Add(NewClass);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of teachers
            return Classes;
        }


        /// <summary>
        /// Returns all matching classes from the database by specifying the primary key classId
        /// </summary>
        /// <param name="id">the Class ID in the database</param>
        /// <returns>class objects by the class ID</returns>
        /// <example>GET api/ClassData/FindClass/2
        /// <returns>
        ///<Class>
        ///<ClassCode>http5102</ClassCode>
        ///<ClassId>2</ClassId>
        ///<ClassName>Project Management</ClassName>
        ///<FisihiDate>2018-12-14T00:00:00</FisihiDate>
        ///<StartDate>2018-09-04T00:00:00</StartDate>
        ///<TeacherId>2</TeacherId>
        ///</Class>
        ///<Class>
        ///<ClassCode>http5201</ClassCode>
        ///<ClassId>6</ClassId>
        ///<ClassName>Security & Quality Assurance</ClassName>
        ///<FisihiDate>2019-04-27T00:00:00</FisihiDate>
        ///<StartDate>2019-01-08T00:00:00</StartDate>
        ///<TeacherId>2</TeacherId>
        ///</Class>
        ///</returns>
    


        [HttpGet]
        public Class FindClass (int id)
        {
            Class NewClass = new Class();
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from classes where classid = " + id;


            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                int ClassId = (int)ResultSet["classid"];
                string ClassCode = (string)ResultSet["classcode"];
                DateTime StartDate = (DateTime)ResultSet["startdate"];
                DateTime FisihiDate = (DateTime)ResultSet["finishdate"];
                string ClassName = (string)ResultSet["classname"];


                NewClass.TeacherId = TeacherId;
                NewClass.ClassId = ClassId;
                NewClass.ClassCode = ClassCode;
                NewClass.StartDate = StartDate;
                NewClass.FisihiDate = FisihiDate;
                NewClass.ClassName = ClassName;

            }

            return NewClass;
        }


        //find teacher from class

        /// <summary>
        /// Returns all matching classes from the database by specifying the primary key classId
        /// </summary>
        /// <param name="id">the Class ID in the database</param>
        /// <returns>teacher id from class by the class ID</returns>
        /// <example>GET api/ClassData/FindClass/2
        /// <returns>   
        /// <Teacher>
        /// <Employeenumber>T381</Employeenumber>
        /// <HireDate>2014-06-10T00:00:00</HireDate>
        /// <Salary>10000.00</Salary>
        /// <TeacherFname>Caitlinead</TeacherFname>
        /// <TeacherId>2</TeacherId>
        /// <TeacherLname>Cummings</TeacherLname>
        /// </Teacher>
        /// </returns>
        [HttpGet]
        public Teacher FindTeacherfromclass(int id)
        {
            Teacher NewTeacher = new Teacher();
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from teachers join classes on teachers.teacherid = classes.teacherid where classid = " + id;


            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = (string)ResultSet["teacherFname"];
                string TeacherLName = (string)ResultSet["teacherlname"];
                string Employeenumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                decimal Salary = (decimal)ResultSet["salary"];

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLName;
                NewTeacher.Employeenumber = Employeenumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;

            }

            return NewTeacher;
        }



   
        /// <summary>
        /// Updates an Teacher on the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="TeacherInfo">An object with fields that map to the columns of the class's table.</param>
        /// <example>
        /// POST api/ClassData/UpdateTeacher/10
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherId":"5"
        /// }
        /// </example>
        [HttpPost]

        public void UpdateTeacher(int id, [FromBody] Class TeacherInfo)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();


            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY

            cmd.CommandText = "update classes set teacherid=@TeacherId where classid=@ClassId";
            cmd.Parameters.AddWithValue("@ClassId", id);
            cmd.Parameters.AddWithValue("@TeacherId", TeacherInfo.TeacherId);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();

        }

         }
}
