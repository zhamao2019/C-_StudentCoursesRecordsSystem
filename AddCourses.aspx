<%@ Page Language="C#" MasterPageFile="~/ACMasterPage.master" AutoEventWireup="true" CodeBehind="AddCourses.aspx.cs" Inherits="Lab_4.AddCourses" %>

<asp:Content ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="mb-5">Add Student Records</h1>

    <div class="form-group row">
        <label for="txtCourseNumber" class="col-md-4 col-form-label">Course Number:</label>
        <div class="col-md-4">
            <asp:TextBox runat="server" ID="txtCourseNumber" CssClass="form-control"></asp:TextBox>
        </div>
        <asp:RequiredFieldValidator ID="rqvCNumber" runat="server" Display="Dynamic"
            CssClass="text-danger" ControlToValidate="txtcourseNumber" ErrorMessage="Required!" EnableClientScript="true">
        </asp:RequiredFieldValidator>
        <asp:CustomValidator ID="cvCNumber" runat="server" Display="Dynamic" CssClass="text-danger"
            ControlToValidate="txtcourseNumber" ErrorMessage="Course with this code already exist" 
            OnServerValidate="CourseCodeDBValidate">
        </asp:CustomValidator>
    </div>
    
    <div class="form-group row">
        <label for="txtCourseName" class="col-md-4 col-form-label">Course Name:</label>
        <div class="col-md-4">
            <asp:TextBox runat="server" ID="txtCourseName" CssClass="form-control"></asp:TextBox>
        </div>
        <asp:RequiredFieldValidator ID="rqvCName" runat="server" Display="Dynamic"
                CssClass="text-danger" ControlToValidate="txtcourseName" ErrorMessage="Required!" EnableClientScript="true">
        </asp:RequiredFieldValidator>
    </div>

    <asp:Button runat="server" ID="btnAddCourse" Text="Submit Course Information" OnClick="btnAddCourse_Click" CssClass="btn btn-primary" />

    <p class="vertical-margin"><strong>The following courses are currently in the system:</strong></p>

    <asp:Table runat="server" ID="tblCourses" CssClass="table">
        <asp:TableHeaderRow CssClass="thead-dark">
            <asp:TableHeaderCell Scope="Column"><a href="AddCourses.aspx?sort=code" id="sortCode" runat="server">Course Code</a></asp:TableHeaderCell>
            <asp:TableHeaderCell Scope="Column"><a href="AddCourses.aspx?sort=title" id="sortTitle" runat="server">Name</a></asp:TableHeaderCell>    
        </asp:TableHeaderRow>
    </asp:Table>

</asp:Content>
