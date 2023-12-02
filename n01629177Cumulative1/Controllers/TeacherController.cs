using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using n01629177Cumulative1.Models;

namespace n01629177Cumulative1.Controllers
{
  public class TeacherController : Controller
  {
    // GET: Teacher
    public ActionResult Index()
    {
      return View();
    }

    /// <summary>
    /// Dynamic view page to list all the teachers in the `teachers` table for the school's database.
    /// </summary>
    /// <example>
    /// GET: /Teacher/List
    /// </example>
    /// <returns>
    /// A page containing a list of all the teachers in the `teachers` table for the school's database.
    /// </returns>
    public ActionResult List(string name = null)
    {
      TeacherDataController controller = new TeacherDataController();
      IEnumerable<Teacher> teachers = controller.ListTeachers(name);
      return View(teachers);
    }

    /// <summary>
    /// Dynamic view page to show information about the specified `teacher_id`.
    /// </summary>
    /// <example>
    /// GET: /Teacher/Show?teacher_id={int}
    /// </example>
    /// <param name="teacher_id">Corresponds to the `teacherid` field in the database.</param>
    /// <returns>
    /// A page containing all the data retreivable from the row in the `teachers` table for the specified `teacher_id`.
    /// </returns>
    public ActionResult Show(int teacher_id)
    {
      TeacherDataController controller = new TeacherDataController();
      Teacher teacher = controller.FindTeacher(teacher_id);
      return View(teacher);
    }

    /// <summary>
    /// Dynamic view page to confirm that the user wants to delete the specified `teacher_id`.
    /// </summary>
    /// <example>
    /// GET: /Teacher/DeleteConfirm?teacher_id={int}
    /// </example>
    /// <param name="teacher_id">Corresponds to the `teacherid` field in the database.</param>
    /// <returns>A page that prompts the user to confirm their decision to delete the specified `teacher_id`.</returns>
    public ActionResult DeleteConfirm(int teacher_id)
    {
      TeacherDataController controller = new TeacherDataController();
      Teacher teacher = controller.FindTeacher(teacher_id);
      return View(teacher);
    }

    /// <summary>
    /// Deletes the specified `teacher_id` from the database and redirects the user to the list of teachers.
    /// </summary>
    /// <example>
    /// GET: /Teacher/Delete?teacher_id={int}
    /// </example>
    /// <param name="teacher_id">Corresponds to the `teacherid` field in the database.</param>
    /// <returns>Redirects back to the list of teachers in the database.</returns>
    public ActionResult Delete(int teacher_id){
      TeacherDataController controller = new TeacherDataController();
      controller.DeleteTeacher(teacher_id);

      return RedirectToAction("List");
    }

    /// <summary>
    /// Dynamic view page containing a form for the user to insert a new teacher into the database.
    /// </summary>
    /// <example>
    /// GET: /Teacher/New
    /// </example>
    /// <returns>A page containing a client-side validated form for the user to insert a new teacher into the database.</returns>
    public ActionResult New(){
      return View();
    }

    /// <summary>
    /// Internal test page for testing server-side validation, it's the same as /Teachers/New, except it has no client-side validation on the form.
    /// </summary>
    /// <example>
    /// GET: /Teacher/NewUnvalidated
    /// </example>
    /// <returns>Same as /Teacher/New without client-side validation.</returns>
    public ActionResult NewUnvalidated(){
      return View();
    }

    /// <summary>
    /// Same as /Teachers/New, except it sends a POST request using AJAX.
    /// </summary>
    /// <example>
    /// GET: /Teacher/NewAjax
    /// </example>
    /// <returns>Same as /Teacher/New but uses AJAX to send the form data.</returns>
    public ActionResult NewAjax(){
      return View();
    }

    /// <summary>
    /// Inserts a new teacher into the database using the provided data and redirects back to the list of teachers
    /// </summary>
    /// <param name="teacherFName">String value containing the first name of the teacher.</param>
    /// <param name="teacherLName">String value containing the last name of the teacher.</param>
    /// <param name="employeeNumber">String value containing the employee number of the teacher.</param>
    /// <param name="hireDate">String value containing the hire date of the teacher, must be in a standard date format.</param>
    /// <param name="salary">String value containing the salary of the teacher.</param>
    /// <example>
    /// POST: /Teacher/Create => void
    /// Payload = {
    ///   teacherFName : string,
    ///   teacherLName : string,
    ///   employeeNumber : string,
    ///   hireDate : string,
    ///   salary : string,
    /// }
    /// </example>
    /// <returns>Redirects back to the list of teachers.</returns>
    [HttpPost]
    public ActionResult Create(
      string teacherFName,
      string teacherLName,
      string employeeNumber,
      string hireDate,
      string salary
    ){
      //Server-side validation
      bool is_valid = true;

      is_valid = Regex.IsMatch(teacherFName, @"[A-z]{3, 255}");
      is_valid = Regex.IsMatch(teacherLName, @"[A-z]{3, 255}");
      is_valid = Regex.IsMatch(employeeNumber, @"T[0-9]+");
      is_valid = Regex.IsMatch(salary, @"[0-9]+\.?[0-9]*");

      DateTime hire_date_parsed;
      is_valid = DateTime.TryParse(hireDate, out hire_date_parsed);

      Debug.WriteLine("Is the data passed to /Teacher/Create valid? : " + is_valid);

      //If validation fails, then just redirect them back to the List view without
      //making any changes.
      if(is_valid == false) return RedirectToAction("List");

      //Proceed with creation if it passes validation.
      Teacher teacher = new Teacher();
      teacher.teacherFName = teacherFName;
      teacher.teacherLName = teacherLName;
      teacher.employeeNumber = employeeNumber;
      teacher.hireDate = hire_date_parsed;
      teacher.salary = Decimal.Parse(salary);

      TeacherDataController controller = new TeacherDataController();
      controller.CreateTeacher(teacher);

      return RedirectToAction("List");
    }

    /// <summary>
    /// Child-action partial view that returns the list of courses this teacher teaches.
    /// </summary>
    /// <param name="teacher_id">Corresponds to the `teacherid` field in the database.</param>
    /// <returns>A DeleteConfirm specific page that returns a component detailing the affected courses as a result of deleting this teacher from the database.</returns>
    [ChildActionOnly]
    public ActionResult TeacherCourses(int teacher_id){
      CourseDataController controller = new CourseDataController();
      IEnumerable<Course> courses = controller.ListCourses(teacher_id);
      return PartialView(courses);
    }
  }
}
