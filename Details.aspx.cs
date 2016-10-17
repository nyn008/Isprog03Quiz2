using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.IO;
public partial class Admin_Products_Details : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["ID"] != null) // query string is existing
        {
            int ProductID = 0; // initial value
            bool validID = int.TryParse(Request.QueryString["ID"].ToString(), out ProductID);

            if (validID)
            {
                if (!IsPostBack)
                {
                    GetData(ProductID);
                    GetCategory();
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        else // query is not existing
        {
            Response.Redirect("Default.aspx");
        }
    }


    void GetCategory()
    {
        using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        {
            con.Open();
            string SQL = @"SELECT CatID, Category FROM Categories";

            using (SqlCommand cmd = new SqlCommand(SQL, con))
            {
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    ddlCategory.DataSource = dr;
                    ddlCategory.DataTextField = "Category";
                    ddlCategory.DataValueField = "CatID";
                    ddlCategory.DataBind();

                    ddlCategory.Items.Insert(0, new ListItem("Select from the list", ""));
                }
            }
        }
    }

    void GetData(int ID)
    {
        using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        {
            con.Open();
            string Sql = "SELECT ProductID, Name, CatID, Code, Description, Image, Price, IsFeatured,CriticalLevel, Maximum From Products Where ProductID=@ProductID";
            using (SqlCommand cmd = new SqlCommand(Sql, con))
            {
                cmd.Parameters.AddWithValue("@ProductID", ID);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ltID.Text = dr["ProductID"].ToString();
                            txtName.Text = dr["Name"].ToString();
                            ddlCategory.SelectedValue = dr["CatID"].ToString();
                            txtCode.Text = dr["Code"].ToString();
                            txtDescription.Text = dr["Description"].ToString();
                            Session["image"] = dr["Image"].ToString();
                            txtPrice.Text = dr["Price"].ToString();
                            ddlFeatured.SelectedValue = dr["IsFeatured"].ToString();
                            txtCritical.Text = dr["CriticalLevel"].ToString();
                            txtMaximum.Text = dr["Maximum"].ToString();
                        }
                        imgProduct.ImageUrl = "~/img/products/" + Session["image"].ToString();
                    }
                    else
                    {
                        Response.Redirect("Default.aspx");
                    }
                }
            }
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
            using (SqlConnection con = new SqlConnection(Util.GetConnection()))
            {
                con.Open();
                string SQL = @"UPDATE Products SET Name=@Name, CatID=@CatID, 
                Code=@Code, Description=@Description, Image=@Image, Price=@Price, 
                IsFeatured=@IsFeatured, CriticalLevel=@CriticalLevel, Maximum=@Maximum, 
                DateModified=@DateModified WHERE ProductID=@ProductID";

            using (SqlCommand cmd = new SqlCommand(SQL, con))
            {
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@CatID", ddlCategory.SelectedValue);
                cmd.Parameters.AddWithValue("@Code", txtCode.Text);
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                if (fulimage.HasFile)
                {
                    string file = Path.GetExtension(fulimage.FileName);
                    string id = Guid.NewGuid().ToString();
                    cmd.Parameters.AddWithValue("@Image", id + file);
                    fulimage.SaveAs(Server.MapPath("~/img/products/" + id + file));
                }

                else
                {
                    cmd.Parameters.AddWithValue("@Image", Session["image"].ToString());
                }
                cmd.Parameters.AddWithValue("@Price", txtPrice.Text);
                cmd.Parameters.AddWithValue("@IsFeatured", ddlFeatured.SelectedValue);
                cmd.Parameters.AddWithValue("@Available", 0);
                cmd.Parameters.AddWithValue("@CriticalLevel", txtCritical.Text);
                cmd.Parameters.AddWithValue("@Maximum", txtMaximum.Text);
                cmd.Parameters.AddWithValue("@DateModified", DateTime.Now);
                cmd.Parameters.AddWithValue("@ProductID", Request.QueryString["ID"].ToString());
                cmd.ExecuteNonQuery();

                Response.Redirect("Default.aspx");
            }
        }
    }
}
