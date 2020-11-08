<%@ Page Language="C#" MasterPageFile="~/ACMasterPage.master" AutoEventWireup="true" CodeBehind="AddStudent.aspx.cs" Inherits="Lab_4.AddStudentRecords" %>

<asp:Content ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="mb-5">Add Student Records</h1>

    <div class="form-group row">
        <label for="drpCourseSelection" class="col-md-4 col-form-label">Course:</label>
        <div class="col-md-4">
            <asp:DropDownList  ID="drpCourseSelection" runat="server" CssClass="form-control" 
                OnSelectedIndexChanged="drpCourseSelection_SelectedIndexChanged" AutoPostBack="true" >
                <asp:ListItem Value="-1">Select a Course ... </asp:ListItem>
            </asp:DropDownList>
        </div>
        <asp:RequiredFieldValidator runat="server" ID="rfvCourseSelection" 
                ControlToValidate="drpCourseSelection" ErrorMessage="Must Select One" 
                 InitialValue="-1" Display="Dynamic" CssClass="text-danger">
        </asp:RequiredFieldValidator>
    </div>

    <div class="form-group row">
        <label for="txtStudentNumber" class="col-md-4 col-form-label">Student Number:</label>
        <div class="col-md-4">
            <asp:TextBox runat="server" ID="txtStudentNumber" CssClass="form-control"></asp:TextBox>
        </div>
        <asp:RequiredFieldValidator ID="rqvStuNumber" runat="server" Display="Dynamic"
            CssClass="text-danger" ControlToValidate="txtStudentNumber" ErrorMessage="Required!" EnableClientScript="true">
        </asp:RequiredFieldValidator>
        <asp:CustomValidator ID="cvCNumber" runat="server" Display="Dynamic" CssClass="text-danger"
            ControlToValidate="txtStudentNumber" ErrorMessage="This student record has already existed in the selected course" 
            OnServerValidate="StudentIdDBValidate">
        </asp:CustomValidator>
    </div>

    <div class="form-group row">
        <label for="txtStudentName" class="col-md-4 col-form-label">Student Name:</label>
        <div class="col-md-4">
            <asp:TextBox runat="server" ID="txtStudentName" CssClass="form-control"></asp:TextBox>
        </div>
        <asp:RequiredFieldValidator ID="rqvStuName" runat="server" Display="Dynamic"
            CssClass="text-danger" ControlToValidate="txtStudentName" ErrorMessage="Required!" EnableClientScript="true">
        </asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="revStuName"
                ValidationExpression="[a-zA-Z]+\s[a-zA-Z]+"
                ControlToValidate="txtStudentName" CssClass="text-danger" Display="Dynamic"
                ErrorMessage="Must be in first_name last_name!" runat="server">
        </asp:RegularExpressionValidator>

    </div>

    <div class="form-group row">
        <label for="txtGrade" class="col-md-4 col-form-label">Grade:</label>
        <div class="col-md-4">
            <asp:TextBox runat="server" ID="txtGrade" CssClass="form-control"></asp:TextBox>
        </div>
        <asp:RequiredFieldValidator ID="rqvGrade" runat="server" Display="Dynamic"
            CssClass="text-danger" ControlToValidate="txtGrade" ErrorMessage="Required!" EnableClientScript="true">
        </asp:RequiredFieldValidator>
        <asp:RangeValidator ID="rvGrade" runat="server" ControlToValidate="txtGrade" Type="Integer"
                ErrorMessage="Grade needs to be between 0 and 100 to apply"
                MaximumValue="100" MinimumValue="0" Display="Dynamic" CssClass="text-danger">
        </asp:RangeValidator>
    </div>

    <asp:Button runat="server" ID="btnAddStudentRecord" Text="Add to Course" OnClick="btnAddStudentRecord_Click" CssClass="btn btn-primary" />

    <p class="vertical-margin"><strong>The selected course has the following student records:</strong></p>

    <asp:Table runat="server" ID="tblStudentRecords" CssClass="table">
        <asp:TableHeaderRow CssClass="thead-dark">
            <asp:TableHeaderCell Scope="Column">ID</asp:TableHeaderCell>
            <asp:TableHeaderCell Scope="Column">Name</asp:TableHeaderCell>
            <asp:TableHeaderCell Scope="Column">Grade</asp:TableHeaderCell>
        </asp:TableHeaderRow>
    </asp:Table>

</asp:Content>
