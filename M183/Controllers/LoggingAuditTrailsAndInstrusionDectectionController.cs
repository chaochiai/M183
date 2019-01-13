using M183.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.Controllers
{
    public class LoggingAuditTrailsAndInstrusionDectectionController : Controller
    {
        // GET: LoggingAuditTrailsAndInstrusionDectection
        /// <summary>
        /// Shows the index view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// shows the log in view
        /// checks if user is already logged in
        /// creates session
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            if (Request.Cookies["UserProfile"] != null && !String.IsNullOrEmpty(Request.Cookies["UserProfile"].Value))
            {
                CreateUserProfileSession();
                ViewBag.Message = "You are currently logged in";
            }
            return View();
        }

        /// <summary>
        /// log in functions
        /// logs the user's log in behavior
        /// creates sessions and cookies
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            //request information
            string ip = Request.ServerVariables["REMOTE_ADDR"];
            string platform = Request.Browser.Platform;
            string browser = Request.UserAgent;

            //connect to the database
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                AttachDbFilename=C:\M183\M183\Database\SQLLoggingIntrusionDetection.mdf;
                Integrated Security=True;Connect Timeout=30";

            //check if the user exists
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataReader sqlDataReader;
            sqlCommand.CommandText = "SELECT [Id], [Username] " +
                "FROM [dbo].[User] " +
                "WHERE [Username] = '" + loginViewModel.Username + "' AND [Password] = '" + loginViewModel.Password + "'";
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            sqlDataReader = sqlCommand.ExecuteReader();

            //if the user exists
            if (sqlDataReader.HasRows)
            {
                var userId = 0;
                string username = "";

                //get username and id
                while (sqlDataReader.Read())
                {
                    userId = sqlDataReader.GetInt32(0);
                    username = sqlDataReader.GetString(1);
                    break;
                }

                sqlConnection.Close();
                sqlConnection.Open();

                //check whether user uses a known browser
                SqlCommand browserSqlCommand = new SqlCommand();
                browserSqlCommand.CommandText = "SELECT [Id] " +
                "FROM [dbo].[UserLog] " +
                "WHERE [UserId] = '" + userId + "' AND [IP] LIKE '" + ip.Substring(0,2) + "%' AND browser LIKE '" + platform + "%'";
                browserSqlCommand.Connection = sqlConnection;
                SqlDataReader browserSqlDataReader = browserSqlCommand.ExecuteReader();

                if (!browserSqlDataReader.HasRows)
                {
                    //inform the user that she is not using a usual browser
                    sqlConnection.Close();
                    sqlConnection.Open();

                    //log user's behavior
                    SqlCommand logSqlCommand = new SqlCommand();
                    logSqlCommand.CommandText = "INSERT INTO [dbo].[UserLog] " +
                        "(UserId, IP, Action, Result, CreatedOn, Browser) " +
                        "VALUES('" + userId + "', '" + ip + "', 'login', 'success', GETDATE(), '" + platform + "')";
                    logSqlCommand.Connection = sqlConnection;
                    logSqlCommand.ExecuteReader();

                    //create session and cookies
                    CreateHttpCookie("UserProfile", "notStayLoggedIn", DateTime.Now.AddDays(14));
                    this.CreateUserProfileSession();
                    ViewBag.Message = "You are logged in successfully, " + username;

                }
                else
                {
                    sqlConnection.Close();
                    sqlConnection.Open();

                    //log user's behavior
                    SqlCommand logSqlCommand = new SqlCommand();
                    logSqlCommand.CommandText = "INSERT INTO [dbo].[UserLog] " +
                        "(UserId, IP, Action, Result, CreatedOn, Browser, AdditionalInformation) " +
                        "VALUES('" + userId + "', '" + ip + "', 'login', 'success', GETDATE(), '" + platform + "', 'other browser')";
                    logSqlCommand.Connection = sqlConnection;
                    logSqlCommand.ExecuteReader();

                    //create session and cookies
                    CreateHttpCookie("UserProfile", "notStayLoggedIn", DateTime.Now.AddDays(14));
                    this.CreateUserProfileSession();
                    ViewBag.Message = "You are logged in successfully, " + username;
                }
            }
            //credentials do not match
            else
            {
                //check if user can be found by username
                SqlCommand getUserIdByNameSqlCommand = new SqlCommand();
                getUserIdByNameSqlCommand.CommandText = "SELECT [Id] " +
                    "FROM [dbo].[User] " +
                    "WHERE [Username] = '" + loginViewModel.Username + "'";
                getUserIdByNameSqlCommand.Connection = sqlConnection;
                SqlDataReader getUserIdByNameSqlDataReader = getUserIdByNameSqlCommand.ExecuteReader();

                //user found
                if (getUserIdByNameSqlDataReader.HasRows)
                {
                    //get user id
                    var userId = 0;
                    while (getUserIdByNameSqlDataReader.Read())
                    {
                        userId = getUserIdByNameSqlDataReader.GetInt32(0);
                        break;
                    }

                    sqlConnection.Close();
                    sqlConnection.Open();

                    //get login attempts by the user
                    SqlCommand failedLogSqlCommand = new SqlCommand();
                    failedLogSqlCommand.CommandText = "SELECT COUNT(ID) " +
                        "FROM [dbo].[UserLog] " +
                        "WHERE [UserId] = '" + userId + "' AND Result = 'failed' AND CAST(CreatedOn AS date) = '" + DateTime.Now.ToShortDateString().Substring(0, 10) + "'";
                    failedLogSqlCommand.Connection = sqlConnection;
                    SqlDataReader failedLogInCountSqlDataReader = failedLogSqlCommand.ExecuteReader();

                    //get attempts
                    var attempts = 0;
                    if (failedLogInCountSqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            attempts = sqlDataReader.GetInt32(0);
                            break;
                        }
                    }

                    //if user attemtps 5 times
                    if (attempts >= 5 || loginViewModel.Password.Length < 4 || loginViewModel.Password.Length > 20)
                    {
                        //block user
                    }

                    sqlConnection.Close();
                    sqlConnection.Open();

                    //log user's behaviour
                    SqlCommand logSqlCommand = new SqlCommand();
                    logSqlCommand.CommandText = "INSERT INTO [dbo].[UserLog] " +
                        "(UserId, IP, Action, Result, CreatedOn, Browser) " +
                        "VALUES('" + userId + "', '" + ip + "', 'login', 'failed', GETDATE(), '" + platform + "')";
                    logSqlCommand.Connection = sqlConnection;
                    logSqlCommand.ExecuteReader();

                    ViewBag.Message = "User not found!";
                }
                else
                {
                    //log user's behavior
                    SqlCommand logSqlCommand = new SqlCommand();
                    logSqlCommand.CommandText = "INSERT INTO [dbo].[UserLog] " +
                        "(UserId, IP, Action, Result, CreatedOn, AdditionalInformation, Browser) " +
                        "VALUES(0, '" + ip + "', 'login', 'failed', GETDATE(), 'No User Found', '" + platform + "')";
                    logSqlCommand.Connection = sqlConnection;
                    logSqlCommand.ExecuteReader();

                    ViewBag.Message = "User not found!";
                }

                ViewBag.Message = "The entered credentials are incorrect";
            }
            sqlConnection.Close();
            return View(loginViewModel);
        }

        /// <summary>
        /// Shows the user log view
        /// </summary>
        /// <returns></returns>
        public ActionResult UserLog()
        {
            //connect to the db
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                AttachDbFilename=C:\M183\M183\Database\SQLLoggingIntrusionDetection.mdf;
                Integrated Security=True;Connect Timeout=30";

            //get all userlogs
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "SELECT * " +
                "FROM [dbo].[UserLog] ul " +
                "JOIN [dbo].[User] u " +
                "ON ul.UserId = u.Id " +
                "ORDER BY ul.CreatedOn DESC";
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            List<UserLogViewModel> userLogViewModels = new List<UserLogViewModel>();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    //10 = id
                    //0 = log id
                    //7 = createdon
                    UserLogViewModel userLogViewModel = new UserLogViewModel();
                    userLogViewModel.UserId = sqlDataReader.GetValue(10).ToString();
                    userLogViewModel.LogId = sqlDataReader.GetValue(0).ToString();
                    userLogViewModel.LogCreatedOn = sqlDataReader.GetValue(7).ToString();

                    userLogViewModels.Add(userLogViewModel);
                }
            }
            else
            {
                ViewBag.Message = "No results found!";
            }
            return View(userLogViewModels);
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