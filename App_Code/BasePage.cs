using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Lab_4
{
    public class BasePage: System.Web.UI.Page
    {
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            LinkButton btnHome = (LinkButton)Master.FindControl("btnHome");
            BulletedList btnTopMenu = (BulletedList)Master.FindControl("topMenu");

            btnHome.Enabled = false;

            if (!IsPostBack)
            {
                btnTopMenu.Items.Add(new ListItem("Add Courses"));
                btnTopMenu.Items.Add(new ListItem("Add Student Records"));
            }

            btnTopMenu.Click += BtnTopMenuClick;
        }
        protected void BtnTopMenuClick(object sender, BulletedListEventArgs e)
        {
            switch (e.Index)
            {
                case 0:
                    Response.Redirect("AddCourses.aspx");
                    break;
                case 1:
                    Response.Redirect("AddStudent.aspx");
                    break;
            }
        }
    }
}