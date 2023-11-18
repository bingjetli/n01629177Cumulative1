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
    public class StudentDataController : ApiController
    {
        private SchoolDbContext school_db = new SchoolDbContext();

        /// <summary>
        /// Queries the database for a list of all the students in the students table.
        /// </summary>
        /// <example>
        /// StudentDataController controller = new StudentDataController();
        /// IEnumerable<Student> student = controller.ListStudents();
        /// </example>
        /// <returns>Enumerable list of Student objects.</returns>
        [HttpGet]
        public IEnumerable<Student> ListStudents()
        {
            MySqlConnection connection_to_school_db = school_db.AccessDatabase();
            connection_to_school_db.Open();

            MySqlCommand command = connection_to_school_db.CreateCommand();
            command.CommandText = "select * from students";

            MySqlDataReader result_set = command.ExecuteReader();
            List<Student> students = new List<Student>();

            while (result_set.Read())
            {
                Student student = new Student();
                student.studentId = (uint)result_set["studentid"];
                student.studentFName = (string)result_set["studentfname"];
                student.studentLName = (string)result_set["studentlname"];
                student.studentNumber = (string)result_set["studentnumber"];
                student.enrollDate = (DateTime)result_set["enroldate"];

                students.Add(student);
            }

            connection_to_school_db.Close();
            return students;
        }


        /// <summary>
        /// Queries the database for a Student based on the given `student_id`.
        /// </summary>
        /// <example>
        /// StudentDataController controller = new StudentDataController();
        /// Student student = controller.FindStudent(student_id);
        /// </example>
        /// <param name="student_id">Corresponds to the internal `studentid` field in the database.</param>
        /// <returns>A `Student` object that corresponding to the specified `studentid`.</returns>
        [HttpGet]
        public Student FindStudent(int student_id)
        {
            MySqlConnection connection_to_school_db = school_db.AccessDatabase();
            connection_to_school_db.Open();

            //NOTE: The command below is vulnerable to SQL injection attacks.
            //TODO: Fix it.
            MySqlCommand command = connection_to_school_db.CreateCommand();
            command.CommandText = "select * from students where studentid = " + student_id;

            MySqlDataReader result_set = command.ExecuteReader();
            Student student = new Student();

            while (result_set.Read())
            {
                student.studentId = (uint)result_set["studentid"];
                student.studentFName = (string)result_set["studentfname"];
                student.studentLName = (string)result_set["studentlname"];
                student.studentNumber = (string)result_set["studentnumber"];
                student.enrollDate = (DateTime)result_set["enroldate"];
            }

            connection_to_school_db.Close();
            return student;
        }
    }
}
