using n01629177Cumulative1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace n01629177Cumulative1.Controllers
{
    public class ClassController : Controller
    {
        // GET: Class
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
        public ActionResult List()
        {
            ClassDataController controller = new ClassDataController();
            IEnumerable<Class> classes = controller.ListClasses();
            return View(classes);
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
        public ActionResult Show(int class_id)
        {
            ClassDataController controller = new ClassDataController();
            Class school_class = controller.FindClass(class_id);
            return View(school_class);
        }
    }
}