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
    public IEnumerable<Teacher> ListTeachers(
            string name = null
        )
    {
      MySqlConnection connection_to_school_db = school_db.AccessDatabase();
      connection_to_school_db.Open();

      //Build the SQL Query.
      MySqlCommand command = connection_to_school_db.CreateCommand();

      if (name != null)
      {
        command.CommandText = "select * from teachers where lower(teacherfname) like lower(@name) or lower(teacherlname) like lower(@name) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@name)";
        command.Parameters.AddWithValue("@name", name);
        command.Prepare();
      }
      else
      {
        command.CommandText = "select * from teachers";
      }


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


    /// <summary>
    /// Inserts a new teacher into the `teachers` table.
    /// </summary>
    /// <example>
    /// GET /api/teacherdata/createteacher?new_teacher={Teacher} => void
    /// </example>
    /// <param name="new_teacher">A `Teacher` object containing information for the new teacher to be added.</param>
    public void CreateTeacher(Teacher new_teacher)
    {
      MySqlConnection connection_to_school_db = school_db.AccessDatabase();
      connection_to_school_db.Open();

      MySqlCommand command = connection_to_school_db.CreateCommand();
      command.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@teacher_f_name, @teacher_l_name, @employee_number, @hire_date, @salary);";

      command.Parameters.AddWithValue("teacher_f_name", new_teacher.teacherFName);
      command.Parameters.AddWithValue("teacher_l_name", new_teacher.teacherLName);
      command.Parameters.AddWithValue("employee_number", new_teacher.employeeNumber);
      command.Parameters.AddWithValue("hire_date", new_teacher.hireDate);
      command.Parameters.AddWithValue("salary", new_teacher.salary);

      command.Prepare();

      command.ExecuteNonQuery();

      connection_to_school_db.Close();
    }

    /// <summary>
    /// Permanently deletes a teacher from the database and updates the courses associated with that teacher id to NULL.
    /// </summary>
    /// <example>
    /// GET /api/teacherdata/deleteteacher?teacher_id={int} => void
    /// </example>
    /// <param name="teacher_id">The integer id of the teacher to delete.</param>
    public void DeleteTeacher(int teacher_id)
    {
      MySqlConnection connection_to_school_db = school_db.AccessDatabase();
      connection_to_school_db.Open();

      //First, update the courses associated with this `teacher_id`. So these courses are not pointing to
      //an invalid teacher_id when the teacher is deleted from the database.
      MySqlCommand update_command = connection_to_school_db.CreateCommand();
      update_command.CommandText = "update classes set teacherid = NULL where teacherid = @teacher_id;";
      update_command.Parameters.AddWithValue("@teacher_id", teacher_id);
      update_command.Prepare();
      update_command.ExecuteNonQuery();

      //Then, delete the teacher associated with this `teacher_id`.
      MySqlCommand delete_command = connection_to_school_db.CreateCommand();
      delete_command.CommandText = "delete from teachers where teacherid = @teacher_id;";
      delete_command.Parameters.AddWithValue("@teacher_id", teacher_id);
      delete_command.Prepare();
      delete_command.ExecuteNonQuery();

      connection_to_school_db.Close();
    }
  }
}
