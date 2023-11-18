using n01629177Cumulative1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace n01629177Cumulative1.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Dynamic view page to list all the students in the `students` table for the school's database.
        /// </summary>
        /// <example>
        /// GET: /Student/List
        /// </example>
        /// <returns>
        /// A page containing a list of all the students in the `students` table for the school's database.
        /// </returns>
        public ActionResult List()
        {
            StudentDataController controller = new StudentDataController();
            IEnumerable<Student> student = controller.ListStudents();
            return View(student);
        }

        /// <summary>
        /// Dynamic view page to show information about the specified `student_id`.
        /// </summary>
        /// <example>
        /// GET: /Student/Show?student_id={int}
        /// </example>
        /// <param name="student_id">Corresponds to the `studentid` field in the database.</param>
        /// <returns>
        /// A page containing all the data retreivable from the row in the `student` table for the specified `student_id`.
        /// </returns>
        public ActionResult Show(int student_id)
        {
            StudentDataController controller = new StudentDataController();
            Student student = controller.FindStudent(student_id);
            return View(student);
        }
    }
}