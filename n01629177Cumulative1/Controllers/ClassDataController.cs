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
        /// Queries the database for a list of all the classes in the classes table.
        /// </summary>
        /// <example>
        /// ClassDataController controller = new ClassDataController();
        /// IEnumerable<Class> classes = controller.ListClasses();
        /// </example>
        /// <returns>Enumerable list of Class objects.</returns>
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
        /// Queries the database for a Class based on the given `class_id`.
        /// </summary>
        /// <example>
        /// ClassDataController controller = new ClassDataController();
        /// Class school_class = controller.FindClass(class_id);
        /// </example>
        /// <param name="class_id">Corresponds to the internal `classid` field in the database.</param>
        /// <returns>A `Class` object that corresponding to the specified `classid`.</returns>
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
