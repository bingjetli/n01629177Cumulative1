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
    public class CourseDataController : ApiController
    {
        private SchoolDbContext school_db = new SchoolDbContext();

        /// <summary>
        /// Queries the database for a list of all the classes in the classes table.
        /// </summary>
        /// <example>
        /// GET /api/coursedata/listcourses => [
        ///     {
        ///         classId : int,
        ///         classCode : string,
        ///         teacherId : long,
        ///         startDate : dateTime,
        ///         finishDate : dateTime,
        ///         className : string,
        ///     },
        ///     {classId...},
        ///     ...
        ///     {classId...}
        /// ]
        /// </example>
        /// <returns>Enumerable list of Class objects.</returns>
        [HttpGet]
        public IEnumerable<Course> ListCourses()
        {
            MySqlConnection connection_to_school_db = school_db.AccessDatabase();
            connection_to_school_db.Open();

            MySqlCommand command = connection_to_school_db.CreateCommand();
            command.CommandText = "select * from classes";

            MySqlDataReader result_set = command.ExecuteReader();
            List<Course> courses = new List<Course>();

            while (result_set.Read())
            {
                Course course = new Course();
                course.classId = (int)result_set["classid"];
                course.classCode = (string)result_set["classcode"];
                course.teacherId = (long)result_set["teacherid"];
                course.startDate = (DateTime)result_set["startdate"];
                course.finishDate = (DateTime)result_set["finishdate"];
                course.className = (string)result_set["classname"];

                courses.Add(course);
            }

            connection_to_school_db.Close();
            return courses;
        }


        /// <summary>
        /// Queries the database for a Class based on the given `class_id`.
        /// </summary>
        /// <example>
        /// GET /api/coursedata/findcourse?class_id={int} => {
        ///     classId : int,
        ///     classCode : string,
        ///     teacherId : long,
        ///     startDate : dateTime,
        ///     finishDate : dateTime,
        ///     className : string
        /// }
        /// </example>
        /// <param name="class_id">Corresponds to the internal `classid` field in the database.</param>
        /// <returns>A `Class` object that corresponding to the specified `classid`.</returns>
        [HttpGet]
        public Course FindCourse(int class_id)
        {
            MySqlConnection connection_to_school_db = school_db.AccessDatabase();
            connection_to_school_db.Open();

            MySqlCommand command = connection_to_school_db.CreateCommand();
            command.CommandText = "select * from classes where classid = @class_id;";

            command.Parameters.AddWithValue("@class_id", class_id);
            command.Prepare();

            MySqlDataReader result_set = command.ExecuteReader();
            Course course = new Course();

            while (result_set.Read())
            {
                course.classId = (int)result_set["classid"];
                course.classCode = (string)result_set["classcode"];
                course.teacherId = (long)result_set["teacherid"];
                course.startDate = (DateTime)result_set["startdate"];
                course.finishDate = (DateTime)result_set["finishdate"];
                course.className = (string)result_set["classname"];
            }

            connection_to_school_db.Close();
            return course;
        }
    }
}
