using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;

public partial class Admin_Products_Delete : System.Web.UI.Page
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
                    DeleteRecord(ProductID);
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

    void DeleteRecord(int ID)
    {
        using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        {
            con.Open();
            string SQL = @"DELETE FROM Products WHERE ProductID=@ProductID";

            using (SqlCommand cmd = new SqlCommand(SQL, con))
            {
                cmd.Parameters.AddWithValue("@ProductID", ID);
                cmd.ExecuteNonQuery();
                Response.Redirect("Default.aspx");
            }
        }
    }
}