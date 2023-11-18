using MySql.Data.MySqlClient;
using n01629177Cumulative1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace n01629177Cumulative1.Controllers
{
    public class ClassDataController : ApiController
    {
        private SchoolDbContext school_db = new SchoolDbContext();

        /// <summary>
        /// Queries the database for a list of all the teachers in the teachers table.
        /// </summary>
        /// <example>
        /// MySqlConnection connection_to_school_db = school_db.AccessDatabase();
        /// connection_to_school_db.Open();
        /// </example>
        /// <returns>Enumerable list of Teacher objects.</returns>
        [HttpGet]
        public IEnumerable<Class> ListClasses()
        {
            MySqlConnection connection_to_school_db = school_db.AccessDatabase();
            connection_to_school_db.Open();

            MySqlCommand command = connection_to_school_db.CreateCommand();
            command.CommandText = "select * from classes";

            MySqlDataReader result_set = command.ExecuteReader();
            List<Class> classes = new List<Class>();

            while (result_set.Read())
            {
                Class school_class = new Class();
                school_class.classId = (int)result_set["classid"];
                school_class.classCode = (string)result_set["classcode"];
                school_class.teacherId = (long)result_set["teacherid"];
                school_class.startDate = (DateTime)result_set["startdate"];
                school_class.finishDate = (DateTime)result_set["finishdate"];
                school_class.className = (string)result_set["classname"];

                classes.Add(school_class);
            }

            connection_to_school_db.Close();
            return classes;
        }


        /// <summary>
        /// Queries the database for a Teacher based on the given `teacher_id`.
        /// </summary>
        /// <example>
        /// MySqlConnection connection_to_school_db = school_db.AccessDatabase();
        /// connection_to_school_db.Open();
        /// </example>
        /// <param name="teacher_id">Corresponds to the internal `teacherid` field in the database.</param>
        /// <returns>A `Teacher` object that corresponding to the specified `teacherid`.</returns>
        [HttpGet]
        public Class FindClass(int class_id)
        {
            MySqlConnection connection_to_school_db = school_db.AccessDatabase();
            connection_to_school_db.Open();

            //NOTE: The command below is vulnerable to SQL injection attacks.
            //TODO: Fix it.
            MySqlCommand command = connection_to_school_db.CreateCommand();
            command.CommandText = "select * from classes where classid = " + class_id;

            MySqlDataReader result_set = command.ExecuteReader();
            Class school_class = new Class();

            while (result_set.Read())
            {
                school_class.classId = (int)result_set["classid"];
                school_class.classCode = (string)result_set["classcode"];
                school_class.teacherId = (long)result_set["teacherid"];
                school_class.startDate = (DateTime)result_set["startdate"];
                school_class.finishDate = (DateTime)result_set["finishdate"];
                school_class.className = (string)result_set["classname"];
            }

            connection_to_school_db.Close();
            return school_class;
        }
    }
}
