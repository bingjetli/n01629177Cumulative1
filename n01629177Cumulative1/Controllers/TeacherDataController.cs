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
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext school_db = new SchoolDbContext();

        /// <summary>
        /// Queries the database for a list of all the teachers in the teachers table.
        /// </summary>
        /// <example>
        /// GET /api/teacherdata/listteachers => [
        ///     {
        ///         teacherId : int,
        ///         teacherFName : string,
        ///         teacherLName : string,
        ///         employeeNumber : string,
        ///         hireDate : dateTime,
        ///         salary : decimal
        ///     },
        ///     {teacherId...},
        ///     ...
        ///     {teacherId...}
        /// ]
        /// </example>
        /// <returns>Enumerable list of Teacher objects.</returns>
        [HttpGet]
        public IEnumerable<Teacher> ListTeachers()
        {
            MySqlConnection connection_to_school_db = school_db.AccessDatabase();
            connection_to_school_db.Open();

            MySqlCommand command = connection_to_school_db.CreateCommand();
            command.CommandText = "select * from teachers";

            MySqlDataReader result_set = command.ExecuteReader();
            List<Teacher> teachers = new List<Teacher>();

            while (result_set.Read())
            {
                Teacher teacher = new Teacher();
                teacher.teacherId = (int)result_set["teacherid"];
                teacher.teacherFName = (string)result_set["teacherfname"];
                teacher.teacherLName = (string)result_set["teacherlname"];
                teacher.employeeNumber = (string)result_set["employeenumber"];
                teacher.hireDate = (DateTime)result_set["hiredate"];
                teacher.salary = (Decimal)result_set["salary"];

                teachers.Add(teacher);
            }

            connection_to_school_db.Close();
            return teachers;
        }


        /// <summary>
        /// Queries the database for a Teacher based on the given `teacher_id`.
        /// </summary>
        /// <example>
        /// GET /api/teacherdata/findteacher?teacher_id={int} => {
        ///     teacherId : int,
        ///     teacherFName : string,
        ///     teacherLName : string,
        ///     employeeNumber : string,
        ///     hireDate : dateTime,
        ///     salary : decimal
        /// }
        /// </example>
        /// <param name="teacher_id">Corresponds to the internal `teacherid` field in the database.</param>
        /// <returns>A `Teacher` object that corresponding to the specified `teacherid`.</returns>
        [HttpGet]
        public Teacher FindTeacher(int teacher_id)
        {
            MySqlConnection connection_to_school_db = school_db.AccessDatabase();
            connection_to_school_db.Open();

            MySqlCommand command = connection_to_school_db.CreateCommand();
            command.CommandText = "select * from teachers where teacherid = @teacher_id;";

            command.Parameters.AddWithValue("@teacher_id", teacher_id);
            command.Prepare();

            MySqlDataReader result_set = command.ExecuteReader();
            Teacher teacher = new Teacher();

            while (result_set.Read())
            {
                teacher.teacherId = (int)result_set["teacherid"];
                teacher.teacherFName = (string)result_set["teacherfname"];
                teacher.teacherLName = (string)result_set["teacherlname"];
                teacher.employeeNumber = (string)result_set["employeenumber"];
                teacher.hireDate = (DateTime)result_set["hiredate"];
                teacher.salary = (Decimal)result_set["salary"];
            }

            connection_to_school_db.Close();
            return teacher;
        }
    }
}
