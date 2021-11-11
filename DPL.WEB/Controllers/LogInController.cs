using DPL.WEB.App_Start;
using DPL.WEB.Models;
using Dutility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace DPL.WEB.Controllers
{
    public class LogInController : Controller
    {
        MPO Obj = new MPO();
        string u = "";
        string p = "";
        //
        // GET: /LogIn/
        public ActionResult LogIn()
        {
            LogInMo login = new LogInMo();
            return View("LogIn");
        }

        public ActionResult Logout()
        {
            LogInMo login = new LogInMo();
            Session["USERID"] = null;
            //Removes all keys and values from the session-state collection.
            Session.Clear();

            //Cancels the current session.
            Session.Abandon();
            return RedirectToAction("LogIn", "LogIn");
            //return View("LogIn");
        }
     

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LogInMo login)
        {

            string name = "";
             u = login.username;
             p = login.password;


             //if (login.userLevel == 0)
             //{
                 Obj = mGetUserPassword("0001", u, Convert.ToInt16(login.userLevel));
                 string databaseUser = Obj.strUserID;
                 string databasePass = Obj.strUserPassword;
                  name = AuthUser(databaseUser, databasePass);
             //}

            if (!(String.IsNullOrEmpty(name)))
            {

                if (login.userLevel == 0)
                {
                    string UserID = login.username;
                    Session["USERID"] = login.username;
                    Session["userLevel"] = login.userLevel;
                    return RedirectToAction("AdminUserView", "SalesOrder", new { Area = "Transaction" });
                }
                else if (login.userLevel == 1)
                {
                    string UserID = login.username;
                    Session["USERID"] = login.username;
                    Session["userLevel"] = login.userLevel;
                    return RedirectToAction("AdminUserView", "SalesOrder", new { Area = "Transaction" });
                }
                else if (login.userLevel == 2)
                {
                    string UserID = login.username;
                    Session["USERID"] = login.username;
                    Session["userLevel"] = login.userLevel;
                    return RedirectToAction("MpoView", "SalesOrder", new { Area = "Transaction" });
                }
               
                else
                {
                    string UserID = login.username;
                    Session["USERID"] = login.username;
                    Session["userLevel"] = login.userLevel;
                    return RedirectToAction("AreaHeadView", "SalesOrder", new { Area = "Transaction" });
                }
            }
            ViewBag.checkfals = "ok";
            return RedirectToAction("LogIn", "LogIn", new { returnUrl = UrlParameter.Optional });


        }




        public string AuthUser(string username, string password)
        {
            if (password.Equals(p) && username.Equals(u))
                return username;
            else
                return null;
        }


        public MPO  mGetUserPassword(string strDeComID, string strUserID,Int16 intUserlevel)
        {
            string strSQL = "";

            SqlDataReader drGetGroup;
        
         
            SqlCommand cmd = new SqlCommand();

            string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection gcnMain = new SqlConnection(connectionString))
            {
                if (gcnMain.State == ConnectionState.Open)
                {
                    gcnMain.Close();
                }
                gcnMain.Open();
                cmd.Connection = gcnMain;

                if ( (intUserlevel == 1) || (intUserlevel == 0))
                {
                    strSQL = "SELECT USER_LOGIN_SERIAL,USER_LOGIN_NAME,USER_PASS,USER_LEBEL,USER_STATUS FROM USER_CONFIG ";
                    strSQL = strSQL + "WHERE USER_LOGIN_NAME = '" + strUserID.Trim().Replace("'", "''") + "' ";
                }
                else
                {
                    strSQL = "SELECT USER_ID,PASSWORD FROM USER_ONLILE_SECURITY WHERE STATUS=0 AND USER_ID ='" + strUserID + "' ";

                }
                cmd.CommandText = strSQL;
                drGetGroup = cmd.ExecuteReader();
                if (drGetGroup.Read())
                {

                    if ( (intUserlevel == 1) || (intUserlevel == 0))
                    {
                        Obj.strUserID = drGetGroup["USER_LOGIN_NAME"].ToString();
                        Obj.strUserPassword = Utility.Decrypt(drGetGroup["USER_PASS"].ToString(), drGetGroup["USER_LOGIN_NAME"].ToString()).ToString();
                    }
                    else
                    {
                        Obj.strUserID = drGetGroup["USER_ID"].ToString();
                        Obj.strUserPassword = drGetGroup["PASSWORD"].ToString();
                    }

                 
                }
                else
                {
                    Obj.strUserID = "";
                    Obj.strUserPassword = "";
                }
                drGetGroup.Close();

                return Obj;

            }
        }


    }
}