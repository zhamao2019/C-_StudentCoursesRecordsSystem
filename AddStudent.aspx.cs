using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lab_4
{
    public partial class AddStudentRecords : System.Web.UI.Page
    {
        protected void StudentIdDBValidate(object source, ServerValidateEventArgs args)
        {
            // validate if the student record is existed the selected course in database
            List<Course> courses = (List<Course>)Session["courses"];
            Course selectedCourse = courses.Find(c => c.Code == drpCourseSelection.SelectedValue);

            using (StudentRecordEntities entities = new StudentRecordEntities())
            {
                AcademicRecord academicRecord = (from r in entities.AcademicRecords
                                   where r.StudentId == txtStudentNumber.Text && r.CourseCode == selectedCourse.Code
                                   select r).FirstOrDefault<AcademicRecord>();

                if (academicRecord != null)
                {
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
            displayStudentRecordTable(selectedCourse);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            LinkButton btnHome = (LinkButton)Master.FindControl("btnHome");
            BulletedList btnTopMenu = (BulletedList)Master.FindControl("topMenu");

            btnHome.Click += (s, a) => Response.Redirect("Default.aspx");
            btnTopMenu.Click += (s, a) => Response.Redirect("AddCourses.aspx");

            if (!IsPostBack)
            {
                btnTopMenu.Items.Add(new ListItem("Add Courses"));

                using (StudentRecordEntities entities = new StudentRecordEntities())
                {
                    List<Course> courses = entities.Courses.ToList<Course>();

                    if (Session["courses"] == null)
                    {
                        Session["courses"] = courses;
                    }
                                        
                    foreach (Course course in courses)
                    {
                        drpCourseSelection.Items.Add(new ListItem(course.Code + " - " + course.Title, course.Code));
                    }
                    
                    displayStudentRecordTable(null);
                }
            }
        }
        protected void drpCourseSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Course> courses = (List<Course>)Session["courses"];
            Course selectedCourse = courses.Find(c => c.Code == drpCourseSelection.SelectedValue);

            displayStudentRecordTable(selectedCourse);
        }
        protected void btnAddStudentRecord_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            List<Course> courses = (List<Course>)Session["courses"];
            Course selectedCourse = courses.Find(c => c.Code == drpCourseSelection.SelectedValue);

            using (StudentRecordEntities entities = new StudentRecordEntities())
            {

                Student student = (from s in entities.Students
                                   where s.Id == txtStudentNumber.Text
                                   select s).FirstOrDefault<Student>();

                if(student == null)
                {
                    student = new Student();
                    student.Id = txtStudentNumber.Text;
                    student.Name = txtStudentName.Text;
                    entities.Students.Add(student);
                    entities.SaveChanges();
                }

                AcademicRecord academicRecord = new AcademicRecord();
                academicRecord.CourseCode = drpCourseSelection.SelectedValue;
                //academicRecord.StudentId = txtStudentNumber.Text;
                academicRecord.Grade = int.Parse(txtGrade.Text);
                //academicRecord.Course = selectedCourse;
                academicRecord.Student = student;

                Course course = (from c in entities.Courses
                                 where c.Code == selectedCourse.Code
                                 select c).FirstOrDefault<Course>();

                course.AcademicRecords.Add(academicRecord);
                entities.SaveChanges();
            }
            displayStudentRecordTable(selectedCourse);
        }
        public void displayStudentRecordTable(Course course)
        {
            if (drpCourseSelection.SelectedValue == "-1" || course == null)
            {
                //create the empty table
                Label NoData = new Label();
                TableRow row = new TableRow();
                TableCell cell = new TableCell();

                NoData.CssClass = "text-muted";
                NoData.Text = "No Student Records";

                cell.Controls.Add(NoData);
                cell.ColumnSpan = 3;
                row.HorizontalAlign = HorizontalAlign.Center;

                row.Cells.Add(cell);
                tblStudentRecords.Rows.Add(row);
            }
            else
            {
                using (StudentRecordEntities entities = new StudentRecordEntities())
                {
                    List<AcademicRecord> allAcademicRecords = entities.AcademicRecords.ToList<AcademicRecord>();

                    List<AcademicRecord> academicRecords = allAcademicRecords.FindAll(r => r.CourseCode == course.Code);

                    if (academicRecords.Count == 0)
                    {
                        //create the empty table
                        Label NoData = new Label();
                        TableRow row = new TableRow();
                        TableCell cell = new TableCell();

                        NoData.CssClass = "text-muted";
                        NoData.Text = "No Student Records";

                        cell.Controls.Add(NoData);
                        cell.ColumnSpan = 3;
                        row.HorizontalAlign = HorizontalAlign.Center;

                        row.Cells.Add(cell);
                        tblStudentRecords.Rows.Add(row);
                    }
                    else
                    {
                        // clear rest table
                        for (int i = tblStudentRecords.Rows.Count - 1; i > 0; i--)
                        {
                            tblStudentRecords.Rows.RemoveAt(i);
                        }

                        // sort student records by name, lastname then firstname
                        academicRecords.Sort((x, y) =>
                        {
                            var lhs = x.Student.Name.Split(' ');
                            var rhs = y.Student.Name.Split(' ');
                            var ret = lhs.LastOrDefault().CompareTo(rhs.LastOrDefault());
                            return ret != 0 ? lhs.LastOrDefault().CompareTo(rhs.LastOrDefault()) : lhs.FirstOrDefault().CompareTo(rhs.FirstOrDefault());
                        });

                        foreach (AcademicRecord record in academicRecords)
                        {
                            TableRow row = new TableRow();
                            tblStudentRecords.Rows.Add(row);

                            for (int j = 0; j < 3; j++)
                            {
                                TableCell cell = new TableCell();

                                if (j == 0) { cell.Text = record.Student.Id; }
                                if (j == 1) { cell.Text = record.Student.Name; }
                                if (j == 2) { cell.Text = record.Grade.ToString(); }

                                row.Cells.Add(cell);
                            }
                        }
                    }
                }
            }
        }
    }
}