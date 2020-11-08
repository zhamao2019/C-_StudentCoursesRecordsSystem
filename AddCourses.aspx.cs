using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lab_4
{
    public partial class AddCourses : System.Web.UI.Page
    {
        protected void CourseCodeDBValidate(object source, ServerValidateEventArgs args)
        {
            // validate if the course ID is existed in database
            List<Course> courses = (List<Course>)Session["courses"];

            using (StudentRecordEntities entities = new StudentRecordEntities())
            {
                Course course = (from c in entities.Courses
                                 where c.Code == txtCourseNumber.Text
                                 select c).FirstOrDefault<Course>();

                if (course != null)
                {
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
            DisplayCourseTable(courses);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            LinkButton btnHome = (LinkButton)Master.FindControl("btnHome");
            BulletedList btnTopMenu = (BulletedList)Master.FindControl("topMenu");
            //bool sortCodeDesc = false;
            //bool sortTitleDesc = false;

            btnHome.Click += (s, a) => Response.Redirect("Default.aspx");
            btnTopMenu.Click += (s, a) => Response.Redirect("AddStudent.aspx");

            if (!IsPostBack)
            {
                btnTopMenu.Items.Add(new ListItem("Add Student Records"));


                if (Session["courses"] == null)
                {
                    // create a courses session to save and display the initial data list from database
                    using (StudentRecordEntities entities = new StudentRecordEntities())
                    {
                        List<Course> courses = entities.Courses.ToList<Course>();
                        Session["courses"] = courses;
                        DisplayCourseTable(courses);
                    }
                }
                else
                {
                    // retrieve the courses list form session
                    List<Course> courses = (List<Course>)Session["courses"];
                    string sort = Request.Params["sort"];

                    if (!string.IsNullOrEmpty(sort))
                    {
                        SortCourses(courses, sort);
                    }

                    DisplayCourseTable(courses);
                }

                
                //if (Session["sortCode"] == null)
                //{
                //    Session["sortCode"] = sortCodeDesc;
                //}
                //else
                //{
                //    sortCodeDesc = (bool)Session["sortCode"];
                //}

                //if (Session["sortTitle"] == null)
                //{
                //    Session["sortTitle"] = sortTitleDesc;
                //}
                //else
                //{
                //    sortTitleDesc = (bool)Session["sortTitle"];
                //} 
            }
        }

        public void SortCourses(List<Course> courses, string sort)
        {
            if (sort == "code")
            {
                courses.Sort((x, y) => x.Code.CompareTo(y.Code));
                sortCode.Attributes["href"] = "AddCourses.aspx?sort=code_desc";
            }
            else if (sort == "code_desc")
            {
                courses.Sort((x, y) => (x.Code.CompareTo(y.Code)) * (-1));
                sortCode.Attributes["href"] = "AddCourses.aspx?sort=code";
            }

            if (sort == "title")
            {
                courses.Sort((x, y) => x.Title.CompareTo(y.Title));
                sortTitle.Attributes["href"] = "AddCourses.aspx?sort=title_desc";
            }
            else if (sort == "title_desc")
            {
                courses.Sort((x, y) => (x.Title.CompareTo(y.Title)) * (-1));
                sortTitle.Attributes["href"] = "AddCourses.aspx?sort=title";
            }
        }

        protected void btnAddCourse_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            List<Course> courses = (List<Course>)Session["courses"];

            using (StudentRecordEntities entities = new StudentRecordEntities()) 
            {
                Course course = new Course();

                course.Code = txtCourseNumber.Text;
                course.Title = txtCourseName.Text;
                entities.Courses.Add(course);
                entities.SaveChanges();

                //courses.Add(course);
                // add new course to the session, use insert to display the course at top
                courses.Insert(0, course);
            }

            Session["courses"] = courses;
            DisplayCourseTable(courses);
        }

        public void DisplayCourseTable(List<Course> courses)
        {
            if (courses.Count == 0 || courses == null)
            {
                //create the empty table
                Label NoData = new Label();
                TableRow row = new TableRow();
                TableCell cell = new TableCell();

                NoData.CssClass = "text-muted";
                NoData.Text = "No Courses";
                cell.Controls.Add(NoData);
                cell.ColumnSpan = 2;
                row.HorizontalAlign = HorizontalAlign.Center;

                row.Cells.Add(cell);
                tblCourses.Rows.Add(row);
            }
            else
            {
                // clear rest table
                for (int i = tblCourses.Rows.Count - 1; i > 0; i--)
                {
                    tblCourses.Rows.RemoveAt(i);
                }

                foreach (Course course in courses)
                {
                    // display courses
                    TableRow row = new TableRow();

                    for (int j = 0; j < 2; j++)
                    {
                        TableCell cell = new TableCell();

                        if (j == 0) { cell.Text = course.Code; }
                        if (j == 1) { cell.Text = course.Title; }

                        row.Cells.Add(cell);
                    }
                    tblCourses.Rows.Add(row);
                }
            }
        }
    }
}