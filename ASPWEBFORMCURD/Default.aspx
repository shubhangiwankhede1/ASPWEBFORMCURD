<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ASPWEBFORMCURD._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <div>
              <h2> Product</h2>
    <table class="table table-info">
        <tr>
            <td>Product Name:</td>
            <td><asp:TextBox class="form-control" ID="txtEditProductName" runat="server"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="rfvProductName" runat="server" 
                ControlToValidate="txtEditProductName" ErrorMessage="Product Name is required."
                ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>Category:</td>
            <td>
                <asp:DropDownList class="form-control" ID="ddlEditCategory" runat="server"></asp:DropDownList>
                 <asp:RequiredFieldValidator ID="rfvCategory" runat="server" 
                ControlToValidate="ddlEditCategory" InitialValue="0" ErrorMessage="Please select a category."
                ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>Current Image:</td>
            <td><asp:Image class="form-control" ID="imgEditProduct" runat="server" Width="100px" Height="100px" /></td>
        </tr>
        <tr>
            <td>Change Image:</td>
            <td><asp:FileUpload class="form-control"  ID="fuEditProductImage" runat="server" />
                <asp:CustomValidator ID="cvImage" runat="server" 
                ErrorMessage="Please upload an image file." ForeColor="Red"
                 OnServerValidate="cvImage_ServerValidate">*</asp:CustomValidator>
            </td>
        </tr>
        <%-- <tr>
        <td>Email:</td>
        <td>
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                ControlToValidate="txtEmail" ErrorMessage="Email is required."
                ForeColor="Red">*</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                ControlToValidate="txtEmail" 
                ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$" 
                ErrorMessage="Invalid email format."
                ForeColor="Red">*</asp:RegularExpressionValidator>
        </td>
    </tr>--%>

     
         <%--<tr>
        <td>Password:</td>
        <td>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                ControlToValidate="txtPassword" ErrorMessage="Password is required."
                ForeColor="Red">*</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revPassword" runat="server" 
                ControlToValidate="txtPassword" 
                ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$" 
                ErrorMessage="Password must be at least 8 characters, including letters and numbers."
                ForeColor="Red">*</asp:RegularExpressionValidator>
        </td>
    </tr>--%>
        <tr>
            <td colspan="2">
                 <asp:Button ID="btnadd" CssClass="btn btn-info" runat="server" Text="Save" OnClick="btnadd_Click"  />
                <asp:Button ID="btnUpdate" CssClass="btn btn-success" runat="server" Text="Update" OnClick="btnUpdate_Click"  />
                <asp:Button ID="btncancel" CssClass="btn btn-dark" runat="server" Text="Cancel" OnClick="btncancel_Click" />

            </td>
        </tr>
    </table> 
            <br />
            <br />
            <br />
        <h2>Product List</h2>
        <asp:GridView ID="gvProducts" runat="server" class="table table-responsive" AutoGenerateColumns="False" DataKeyNames="ProductID" OnRowEditing="gvProducts_RowEditing" OnRowDeleting="gvProducts_RowDeleting"
            >
            <Columns>
                <asp:BoundField DataField="ProductID" HeaderText="ID" ReadOnly="True" />
                <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                <asp:TemplateField HeaderText="Image">
                    <ItemTemplate>
                        <asp:Image ID="imgProduct" runat="server" ImageUrl='<%# Eval("ImagePath") %>' Width="50px" Height="50px" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-success"  CommandName="Edit" Text="Edit" />
                <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-danger"  CommandName="Delete" Text="Delete" />
            </Columns>
        </asp:GridView>

    
  
        </div>
    

    </main>

</asp:Content>
