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
        /// <summary>
        /// shows the index view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// shows the log in page
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            //checks if user is already logged in and creates the session
            if (Request.Cookies["UserProfile"] != null && !String.IsNullOrEmpty(Request.Cookies["UserProfile"].Value))
            {
                CreateUserProfileSession();
                ViewBag.Message = "You are currently logged in";
            }
            return View();
        }

        /// <summary>
        /// log in function
        /// checks if the user exists on the database
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            //connect to the db
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                AttachDbFilename=C:\M183\M183\Database\SQLXSSInjection.mdf;
                Integrated Security=True;
                Connect Timeout=30";

            SqlCommand sqlCommand = new SqlCommand();
            SqlDataReader sqlDataReader;

            //sql query select the user's id, username and password
            sqlCommand.CommandText = "SELECT [Id], [Username], [Password] " +
                "FROM [dbo].[User] " +
                "WHERE [Username] = '" + loginViewModel.Username + "' AND [Password] = '" + loginViewModel.Password + "'";
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            sqlDataReader = sqlCommand.ExecuteReader();

            //if the user exists
            if (sqlDataReader.HasRows)
            {
                string username = "";
                while (sqlDataReader.Read())
                {
                    ViewBag.Message += sqlDataReader.GetInt32(0) + " " + sqlDataReader.GetString(1) + " " + sqlDataReader.GetString(2);
                    username = sqlDataReader.GetString(1);
                }

                //creates a session and cookie
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

        /// <summary>
        /// shows the add a message view
        /// </summary>
        /// <returns></returns>
        public ActionResult Message()
        {
            return View();
        }

        /// <summary>
        /// adds the message to the database
        /// </summary>
        /// <param name="messageViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Message(MessageViewModel messageViewModel)
        {
            if (ModelState.IsValid)
            {
                //connect to the db
                SqlConnection sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                    AttachDbFilename=C:\M183\M183\Database\SQLXSSInjection.mdf;
                    Integrated Security=True;
                    Connect Timeout=30";

                //sql query
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = "INSERT INTO [dbo].[Message] ([Value]) VALUES ('" + messageViewModel.Message + "')";
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();

                //if the message was added
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