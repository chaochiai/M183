using M183.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.Controllers
{
    public class LabDatabaseXSSFilterSQLInjectionsController : Controller
    {
        // GET: LabDatabaseXSSFilterSQLInjections
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (Request.Cookies["UserProfile"] != null && !String.IsNullOrEmpty(Request.Cookies["UserProfile"].Value))
            {
                CreateUserProfileSession();
                ViewBag.Message = "You are currently logged in";
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                AttachDbFilename=C:\M183\M183\Database\SQLXSSInjection.mdf;
                Integrated Security=True;
                Connect Timeout=30";

            SqlCommand sqlCommand = new SqlCommand();
            SqlDataReader sqlDataReader;

            sqlCommand.CommandText = "SELECT [Id], [Username], [Password] " +
                "FROM [dbo].[User] " +
                "WHERE [Username] = '" + loginViewModel.Username + "' AND [Password] = '" + loginViewModel.Password + "'";
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.HasRows)
            {
                string username = "";
                while (sqlDataReader.Read())
                {
                    ViewBag.Message += sqlDataReader.GetInt32(0) + " " + sqlDataReader.GetString(1) + " " + sqlDataReader.GetString(2);
                    username = sqlDataReader.GetString(1);
                }

                CreateHttpCookie("UserProfile", "notStayLoggedIn", DateTime.Now.AddDays(14));
                this.CreateUserProfileSession();
                ViewBag.Message = "You are logged in successfully, " + username;
            }
            else
            {
                ViewBag.Message = "The entered credentials are incorrect";
            }
            return View(loginViewModel);
        }
        public ActionResult Message()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Message(MessageViewModel messageViewModel)
        {
            if (ModelState.IsValid)
            {
                SqlConnection sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
AttachDbFilename=C:\M183\M183\Database\SQLXSSInjection.mdf;
Integrated Security=True;
Connect Timeout=30";

                SqlCommand sqlCommand = new SqlCommand();
                //SqlDataReader sqlDataReader;

                sqlCommand.CommandText = "INSERT INTO [dbo].[Message] ([Value]) VALUES ('" + messageViewModel.Message + "')";
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                //sqlDataReader = sqlCommand.ExecuteReader();
                //int successful = ;
                //sqlConnection.Close();
                if (sqlCommand.ExecuteNonQuery() != 0)
                {
                    ViewBag.Message = " \"" + messageViewModel.Message + "\" was added successfully";
                }
            }
            
            return View(messageViewModel);
        }

        private void CreateUserProfileSession()
        {
            System.Web.HttpContext.Current.Session["IsLoggedIn"] = true;
        }

        
        public void CreateHttpCookie(string name, string value, DateTime expiration)
        {
            HttpCookie httpCookie = new HttpCookie(name);
            httpCookie.Value = value;
            httpCookie.Expires = DateTime.Now.AddDays(14);
            Response.Cookies.Add(httpCookie);
        }
    }
}