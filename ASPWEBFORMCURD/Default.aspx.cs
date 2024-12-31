using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPWEBFORMCURD
{
    public partial class _Default : Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    PopulateCategories();
                    BindProducts();
                }
            }
            catch(Exception ex)
            {
                throw;
            }
           
         
        }
        private void BindProducts()
         {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"SELECT p.ProductID, p.ProductName, c.CategoryName, p.ImagePath 
                                 FROM Products p 
                                 JOIN Categories c ON p.CategoryID = c.CategoryID";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvProducts.DataSource = dt;
                    gvProducts.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }
        private void PopulateCategories()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Categories";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ddlEditCategory.DataSource = dt;
                    ddlEditCategory.DataTextField = "CategoryName";
                    ddlEditCategory.DataValueField = "CategoryID";
                    ddlEditCategory.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
         
        }
        private void ClearEditControls()
        {
            try
            {
                txtEditProductName.Text = string.Empty;
                ddlEditCategory.SelectedIndex = 0;
                imgEditProduct.ImageUrl = string.Empty;
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }
        private string GetCategoryIDByName(string categoryName)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT CategoryID FROM Categories WHERE CategoryName = @CategoryName", con);
                    cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                    con.Open();
                    return cmd.ExecuteScalar()?.ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }
    
        protected void btnadd_Click(object sender, EventArgs e)
        {
            // Get user inputs

            try
            {
                string productName = txtEditProductName.Text;
                string categoryID = ddlEditCategory.SelectedValue;
                string imagePath = string.Empty;

                // Handle file upload
                if (fuEditProductImage.HasFile)
                {
                    string folderPath = Server.MapPath("~/UploadedImages/");
                    if (!System.IO.Directory.Exists(folderPath))
                    {
                        System.IO.Directory.CreateDirectory(folderPath);
                    }
                    imagePath = "~/UploadedImages/" + fuEditProductImage.FileName;
                    fuEditProductImage.SaveAs(folderPath + fuEditProductImage.FileName);
                }

                // Save data to the database
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Products (ProductName, CategoryID, ImagePath) VALUES (@ProductName, @CategoryID, @ImagePath)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    cmd.Parameters.AddWithValue("@ImagePath", imagePath);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                BindProducts();
                ClearEditControls();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Form submitted successfully!');", true);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string productID = ViewState["EditProductID"].ToString();
                string productName = txtEditProductName.Text;
                string categoryID = ddlEditCategory.SelectedValue;
                string imagePath = imgEditProduct.ImageUrl;

                if (fuEditProductImage.HasFile)
                {
                    string folderPath = Server.MapPath("~/UploadedImages/");
                    if (!System.IO.Directory.Exists(folderPath))
                    {
                        System.IO.Directory.CreateDirectory(folderPath);
                    }
                    imagePath = "~/UploadedImages/" + fuEditProductImage.FileName;
                    fuEditProductImage.SaveAs(folderPath + fuEditProductImage.FileName);
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Products SET ProductName = @ProductName, CategoryID = @CategoryID, ImagePath = @ImagePath WHERE ProductID = @ProductID", con);
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    cmd.Parameters.AddWithValue("@ImagePath", imagePath);
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Form Updated successfully!');", true);

                }
                BindProducts();
                ClearEditControls();

            }
            catch (Exception ex)
            {
                throw;
            }
           
        }

        protected void gvProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                int rowIndex = e.NewEditIndex;
                GridViewRow row = gvProducts.Rows[rowIndex];

                // Retrieve product information
                string productID = gvProducts.DataKeys[rowIndex]["ProductID"].ToString();
                string productName = row.Cells[1].Text;
                string categoryName = row.Cells[2].Text;
                string imagePath = ((Image)row.FindControl("imgProduct")).ImageUrl;

                // Populate edit form
                txtEditProductName.Text = productName;
                imgEditProduct.ImageUrl = imagePath;

                PopulateCategories();
                ddlEditCategory.SelectedValue = GetCategoryIDByName(categoryName);

                // Store the ProductID for later update
                ViewState["EditProductID"] = productID;
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                throw;
            }
         

            // Cancel default edit mode of GridView
          
        }

        protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                string productID = gvProducts.DataKeys[rowIndex]["ProductID"].ToString();

                // Delete the product
                DeleteProduct(productID);

                // Rebind the GridView
                BindProducts();
            }
            catch (Exception ex)
            {
                throw;
            }
          
        }
        private void DeleteProduct(string productID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE ProductID = @ProductID", con);
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Form Deleted successfully!');", true);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        protected void cvImage_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (fuEditProductImage.HasFile)
            {
                string[] validFileTypes = { ".jpg", ".jpeg", ".png", ".gif" };
                string fileExtension = System.IO.Path.GetExtension(fuEditProductImage.FileName).ToLower();

                if (Array.Exists(validFileTypes, ext => ext == fileExtension))
                {
                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                    cvImage.ErrorMessage = "Only .jpg, .jpeg, .png, or .gif files are allowed.";
                }
            }
            else
            {
                args.IsValid = false;
                cvImage.ErrorMessage = "Please upload an image file.";
            }
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            ClearEditControls();
        }
    }
}