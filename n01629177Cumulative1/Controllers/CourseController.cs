using n01629177Cumulative1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace n01629177Cumulative1.Controllers
{
    public class CourseController : Controller
    {
        // GET: Class
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Dynamic view page to list all the courses in the `courses` table for the school's database.
        /// </summary>
        /// <example>
        /// GET: /Class/List?by_teacher_id={int}
        /// </example>
        /// <returns>
        /// A page containing a list of all the courses in the `courses` table for the school's database.
        /// </returns>
        public ActionResult List(int by_teacher_id=-1)
        {
            CourseDataController controller = new CourseDataController();
            IEnumerable<Course> courses = controller.ListCourses(by_teacher_id);
            return View(courses);
        }

        /// <summary>
        /// Dynamic view page to show information about the specified `class_id`.
        /// </summary>
        /// <example>
        /// GET: /Class/Show?class_id={int}
        /// </example>
        /// <param name="class_id">Corresponds to the `classid` field in the database.</param>
        /// <returns>
        /// A page containing all the data retreivable from the row in the `courses` table for the specified `class_id`.
        /// </returns>
        public ActionResult Show(int class_id)
        {
            CourseDataController controller = new CourseDataController();
            Course course = controller.FindCourse(class_id);
            return View(course);
        }
    }
}