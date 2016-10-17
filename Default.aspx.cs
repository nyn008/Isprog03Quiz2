using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;

public partial class Admin_Products_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetSuppliers();
        }
    }

    void GetSuppliers()
    {
        using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        {
            con.Open();
            string SQL = @"SELECT p.ProductID, p.Name, c.Category, p.Description, 
            p.Image, p.Price, p.IsFeatured, p.DateAdded, p.DateModified FROM Products p
            INNER JOIN Categories c ON p.CatID = c.CatID";
            using (SqlCommand cmd = new SqlCommand(SQL, con))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, "Products");

                    lvSuppliers.DataSource = ds;
                    lvSuppliers.DataBind();
                }
            }
        }
    }

    void GetSuppliers(string keyword)
    {
        using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        {
            con.Open();
            string SQL = @"SELECT p.ProductID, p.Name, c.Category, p.Description, 
                                p.Image, p.Price, p.IsFeatured, p.DateAdded, p.DateModified 
                                FROM Products p INNER JOIN Categories c ON p.CatID = c.CatID WHERE 
                                ProductID Like @keyword OR
                                Name Like @keyword OR
                                Category Like @keyword OR
                                Description Like @keyword OR
                                Image Like @keyword OR
                                Price Like @keyword OR
                                IsFeatured Like @keyword";

            using (SqlCommand cmd = new SqlCommand(SQL, con))
            {
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, "Products");

                    lvSuppliers.DataSource = ds;
                    lvSuppliers.DataBind();
                }
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtKeyword.Text == "")
        {
            GetSuppliers();
        }
        else
        {
            GetSuppliers(txtKeyword.Text);
        }
    }
    protected void lvSuppliers_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {
        dpSuppliers.SetPageProperties(e.StartRowIndex,
            e.MaximumRows, false);
        if (txtKeyword.Text == "")
        {
            GetSuppliers();
        }
        else
        {
            GetSuppliers(txtKeyword.Text);
        }
    }
}