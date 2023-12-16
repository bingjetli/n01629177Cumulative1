using MySql.Data.MySqlClient;
using n01629177Cumulative1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace n01629177Cumulative1.Controllers
{
  public class TeacherDataController : ApiController
  {
    private SchoolDbContext school_db = new SchoolDbContext();

    /// <summary>
    /// Runs an SQL query on the `teachers` database and returns a list of 
    /// all the teachers found in an array of `Teacher` objects.
    /// </summary>
    /// <example>
    /// GET /api/teacherdata/listteachers => [Teacher, Teacher, ..., Teacher];
    /// </example>
    /// <returns>
    /// A list of `Teacher` objects with the following schema :
    /// [
    ///   {
    ///     teacherId : int,
    ///     teacherFName : string,
    ///     teacherLName : string,
    ///     employeeNumber : string,
    ///     hireDate : dateTime,
    ///     salary : decimal
    ///   },
    ///   {teacherId...},
    ///   ...
    ///   {teacherId...}
    /// ]
    /// </returns>
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
    /// Takes a specified `teacher_id` and runs an SQL query to find
    /// the teacher that corresponds to that specified id. It then returns
    /// a `Teacher` object containing the resulting data.
    /// </summary>
    /// <example>
    /// GET /api/teacherdata/findteacher?teacher_id={int} => Teacher
    /// </example>
    /// <param name="teacher_id">Integer value of the primary key `teacherid`.</param>
    /// <returns>
    /// A `Teacher` object with the following schema :
    /// {
    ///   teacherId : int,
    ///   teacherFName : string,
    ///   teacherLName : string,
    ///   employeeNumber : string,
    ///   hireDate : dateTime,
    ///   salary : decimal
    /// }
    /// </returns>
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
    /// Takes a `Teacher` object and runs an SQL query to insert a
    /// new entry containing data inside the `Teacher` object into the 
    /// `teachers` database.
    /// </summary>
    /// <example>
    /// POST /api/teacherdata/createteacher => void
    /// {
    ///   teacherId: 1,
    ///   teacherFName: Tom,
    ///   teacherLName: Bob,
    ///   employeeNumber: 'T00',
    ///   hireDate: '1995-01-01',
    ///   salary: 1.0
    /// }
    /// </example>
    /// <param name="teacher">
    /// A `Teacher` object of the following form : 
    /// {
    ///   teacherId: 1,
    ///   teacherFName: Tom,
    ///   teacherLName: Bob,
    ///   employeeNumber: 'T00',
    ///   hireDate: '1995-01-01',
    ///   salary: 1.0
    /// }
    /// </param>
    [HttpPost]
    public void CreateTeacher([FromBody]Teacher new_teacher)
    {
      //Server-side validation
      bool is_valid = true;

      is_valid = Regex.IsMatch(new_teacher.teacherFName, @"[A-z]{3, 255}");
      is_valid = Regex.IsMatch(new_teacher.teacherLName, @"[A-z]{3, 255}");
      is_valid = Regex.IsMatch(new_teacher.employeeNumber, @"T[0-9]+");
      is_valid = new_teacher.salary > 0;
      is_valid = new_teacher.hireDate != DateTime.MinValue;

      //Debug.WriteLine(teacher.teacherFName);
      //Debug.WriteLine(teacher.teacherLName);
      //Debug.WriteLine(teacher.employeeNumber);
      //Debug.WriteLine(teacher.hireDate);
      //Debug.WriteLine(teacher.salary);
      //Debug.WriteLine("Is the data passed to /Teacher/Create valid? : " + is_valid);

      //Skip the update query if the data passed is invalid.
      if (is_valid == false) return;

      //Create the teacher if it passes server-side validation.
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
    /// Takes a specified `teacher_id` and deletes the corresponding
    /// entry in the SQL Database if it exists.
    /// </summary>
    /// <example>
    /// GET /api/teacherdata/deleteteacher?teacher_id={int} => void
    /// </example>
    /// <param name="teacher_id">Integer value of the primary key `teacherid`.</param>
    [HttpGet]
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


    /// <summary>
    /// Takes a specified `teacher_id` and a `Teacher` object containing the updated
    /// values for the specified teacher and runs an SQL query to update the values
    /// inside the `teachers` database.
    /// </summary>
    /// <example>
    /// POST /api/teacherdata/updateteacher?teacher_id={int} => void
    /// {
    ///   teacherId: 1,
    ///   teacherFName: Tom,
    ///   teacherLName: Bob,
    ///   employeeNumber: 'T00',
    ///   hireDate: '1995-01-01',
    ///   salary: 1.0
    /// }
    /// </example>
    /// <param name="teacher_id">Integer value of the primary key `teacherid`.</param>
    /// <param name="teacher">
    /// A `Teacher` object of the following form : 
    /// {
    ///   teacherId: 1,
    ///   teacherFName: Tom,
    ///   teacherLName: Bob,
    ///   employeeNumber: 'T00',
    ///   hireDate: '1995-01-01',
    ///   salary: 1.0
    /// }
    /// </param>
    [HttpPost]
    public void UpdateTeacher(int teacher_id, [FromBody]Teacher teacher)
    {
      /** SERVER-SIDE VALIDATION CURL TESTING
       * Using `curl2` instead of `curl` because apparently on windows, `curl` is an alias
       * for `Invoke-WebRequest` and so the method shown in class fails when using powershell.
       * 
       * I wasn't sure if it would count towards the initiative if I used `Invoke-WebRequest` 
       * instead of curl so I downloaded `curl`, set my PATH variables, and renamed the 
       * .exe to `curl2`.
       *
       * curl2 -H "Content-Type:application/json" -d @invalid-data-1.json "http://localhost:62788/api/TeacherData/UpdateTeacher?teacher_id=12"
       * curl2 -H "Content-Type:application/json" -d @invalid-data-2.json "http://localhost:62788/api/TeacherData/UpdateTeacher?teacher_id=12"
       * curl2 -H "Content-Type:application/json" -d @valid-data.json "http://localhost:62788/api/TeacherData/UpdateTeacher?teacher_id=12"
       */

      //Server-side validation
      bool is_valid = true;

      is_valid = Regex.IsMatch(teacher.teacherFName, @"[A-z]{3, 255}");
      is_valid = Regex.IsMatch(teacher.teacherLName, @"[A-z]{3, 255}");
      is_valid = Regex.IsMatch(teacher.employeeNumber, @"T[0-9]+");
      is_valid = teacher.salary > 0;
      is_valid = teacher.hireDate != DateTime.MinValue;

      //Debug.WriteLine(teacher.teacherFName);
      //Debug.WriteLine(teacher.teacherLName);
      //Debug.WriteLine(teacher.employeeNumber);
      //Debug.WriteLine(teacher.hireDate);
      //Debug.WriteLine(teacher.salary);
      //Debug.WriteLine("Is the data passed to /Teacher/Create valid? : " + is_valid);

      //Skip the update query if the data passed is invalid.
      if (is_valid == false) return;

      //Update the data after server-side validation.
      MySqlConnection connection_to_school_db = school_db.AccessDatabase();
      connection_to_school_db.Open();

      MySqlCommand command = connection_to_school_db.CreateCommand();
      command.CommandText = "update teachers set teacherfname=@teacher_f_name, teacherlname=@teacher_l_name, employeenumber=@employee_number, hiredate=@hire_date, salary=@salary where teacherid=@teacher_id";

      command.Parameters.AddWithValue("teacher_id", teacher_id);
      command.Parameters.AddWithValue("teacher_f_name", teacher.teacherFName);
      command.Parameters.AddWithValue("teacher_l_name", teacher.teacherLName);
      command.Parameters.AddWithValue("employee_number", teacher.employeeNumber);
      command.Parameters.AddWithValue("hire_date", teacher.hireDate);
      command.Parameters.AddWithValue("salary", teacher.salary);

      command.Prepare();

      command.ExecuteNonQuery();

      connection_to_school_db.Close();
    }
  }
}
