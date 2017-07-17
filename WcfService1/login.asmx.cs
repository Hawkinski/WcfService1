using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml;
using System.IO;
using System.Drawing;
using System.Configuration;
using System.Diagnostics;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Helpers;
using System.Transactions;

namespace WcfService1
{
    /// <summary>
    /// Summary description for login
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class login : System.Web.Services.WebService
    {



        string loginConString = ConfigurationManager.ConnectionStrings["Login_GFM_DB"].ConnectionString;
        string gfmConString = ConfigurationManager.ConnectionStrings["GFM_DB"].ConnectionString;


        [WebMethod]
        public Message notificationLogin(string empcode, string id, string compcode, string flag)
        {
            Message message = new Message();
            bool isExist = false;
            int f = Convert.ToInt32(flag);

            try
            {
                using (SqlConnection con = new SqlConnection(loginConString))
                {
                    con.Open();

                    string check = "select * from GLPUSHNOTIFICATION where EMP_CODE=@empcode and COMP_CODE=@compcode";
                    using (SqlCommand comm = new SqlCommand(check, con))
                    {
                        comm.Parameters.AddWithValue("@empcode", empcode);
                        comm.Parameters.AddWithValue("@compcode", compcode);

                        SqlDataReader reader = comm.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader[0] != DBNull.Value)
                            {
                                isExist = true;
                            }

                        }
                    }
                    con.Close();

                }
            }
            catch (Exception)
            {
                message.message = "error";

            }


            if (isExist)
            {
                try
                {

                    using (SqlConnection con = new SqlConnection(loginConString))
                    {
                        con.Open();
                        using (SqlCommand comm = new SqlCommand("SP_GLPUSHNOTIFICATION_Save", con))
                        {
                            comm.CommandType = CommandType.StoredProcedure;
                            comm.Parameters.AddWithValue("@empcode", empcode);
                            comm.Parameters.AddWithValue("@compcode", compcode);
                            comm.Parameters.AddWithValue("@id", id);
                            comm.Parameters.AddWithValue("@flag", f);
                            comm.Parameters.AddWithValue("@exist", 1);

                            int i = comm.ExecuteNonQuery();

                            if (i == 0)
                            {
                                message.message = "error";

                            }
                            else
                            {
                                message.message = "inserted";
                            }

                        }

                        con.Close();


                    }

                }
                catch (Exception)
                {
                    message.message = "error";

                }
            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(loginConString))
                    {
                        con.Open();
                        using (SqlCommand comm = new SqlCommand("SP_GLPUSHNOTIFICATION_Save", con))
                        {
                            comm.CommandType = CommandType.StoredProcedure;
                            comm.Parameters.AddWithValue("@empcode", empcode);
                            comm.Parameters.AddWithValue("@id", id);
                            comm.Parameters.AddWithValue("@compcode", compcode);
                            comm.Parameters.AddWithValue("@flag", f);
                            comm.Parameters.AddWithValue("@exist", 0);

                            int i = comm.ExecuteNonQuery();

                            if (i == 0)
                            {
                                message.message = "error";

                            }
                            else
                            {
                                message.message = "inserted";
                            }

                        }

                        con.Close();


                    }

                }
                catch (Exception)
                {
                    message.message = "error";

                }

            }





            return message;



        }


        [WebMethod]
        public Message notificationLogout(string empcode, string compcode, string flag)
        {
            Message message = new Message();
            bool isExist = false;
            int f = Int32.Parse(flag);

            try
            {
                using (SqlConnection con = new SqlConnection(loginConString))
                {
                    con.Open();

                    string check = "select * from GLPUSHNOTIFICATION where EMP_CODE=@empcode and COMP_CODE=@compcode";
                    using (SqlCommand comm = new SqlCommand(check, con))
                    {
                        comm.Parameters.AddWithValue("@empcode", empcode);
                        comm.Parameters.AddWithValue("@compcode", compcode);

                        SqlDataReader reader = comm.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader[0] != DBNull.Value)
                            {
                                isExist = true;
                            }

                        }
                    }
                    con.Close();

                }
            }
            catch (Exception)
            {
                message.message = "error";

            }


            if (isExist)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(loginConString))
                    {
                        con.Open();
                        using (SqlCommand comm = new SqlCommand("SP_GLPUSHNOTIFICATION_Save", con))
                        {
                            comm.CommandType = CommandType.StoredProcedure;
                            comm.Parameters.AddWithValue("@empcode", empcode);
                            comm.Parameters.AddWithValue("@compcode", compcode);
                            comm.Parameters.AddWithValue("@flag", f);
                            comm.Parameters.AddWithValue("@id", " ");

                            comm.Parameters.AddWithValue("@exist", 2);

                            int i = comm.ExecuteNonQuery();

                            if (i == 0)
                            {
                                message.message = "error";

                            }
                            else
                            {
                                message.message = "inserted";
                            }

                        }

                        con.Close();


                    }

                }
                catch (Exception ex)
                {
                    message.message = "error";

                }
            }
            return message;



        }


        /// <summary>
        /// Method to check login credantial
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        [WebMethod]
        public XmlDocument Login(string userName, string password, string deviceId)
        {
            DataSet ds = new DataSet();
            employee emp = new employee();
            string pwd = null;
            try
            {
                using (SqlConnection con = new SqlConnection(loginConString))
                {
                    using (SqlCommand comm = new SqlCommand("SP_GLMOBUSERH_GET", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@userCode", userName.ToUpper());
                        comm.Parameters.AddWithValue("@flag", 1);
                        comm.Parameters.AddWithValue("@deviceId", deviceId);

                        SqlDataReader reader = comm.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["MOBUSR_PWD"] != DBNull.Value)
                            {
                                pwd = reader["MOBUSR_PWD"].ToString();

                            }
                        }

                    }

                }
                if (pwd != null)
                {

                    decrypt d = new decrypt();
                    string pass = d.Flc_Decrypt(pwd);
                    Debug.Print("PASSWORD" + pass);

                    if (pass.Equals(password))
                    {
                        using (SqlConnection con = new SqlConnection(loginConString))
                        {
                            using (SqlCommand comm = new SqlCommand("SP_GLMOBUSERH_GET", con))
                            {
                                con.Open();
                                comm.CommandType = CommandType.StoredProcedure;
                                comm.Parameters.AddWithValue("@userCode", userName.ToUpper());
                                comm.Parameters.AddWithValue("@flag", 0);
                                comm.Parameters.AddWithValue("@deviceId", deviceId);

                                SqlDataAdapter ad = new SqlDataAdapter(comm);
                                ad.Fill(ds);
                            }

                        }


                    }

                }


                //SqlCommand comm = new SqlCommand("SELECT ,,,, FROM HRZ_WEBUSER where (USER_CD= @user or WUSER_MAIL= @user) and USER_PWD= @pass", con);




                /* if (hasData)
                 {
                     String path = Server.MapPath("Images");

                     con2.Open();
                     String q = "update GLCOMPANY set COMP_IMG_PATH =@path";
                     SqlCommand com = new SqlCommand(q, con2);
                     com.Parameters.AddWithValue("@path", path);
                     com.ExecuteNonQuery();
                     con2.Close();


                 }*/
            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());

            return xdoc;
        }



        [WebMethod]
        public Message getMaterialList(string compCode)
        {
            DataSet ds = new DataSet();
            Message msg = new Message();

            try
            {
                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();

                    // get Maintenance Task
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_INMAST_GET", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@compCode", compCode);
                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                    }



                }

            }
            catch (Exception ex)
            {
                throw ex;
            }



            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());
            string path= string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    string query = "select COMP_IMG_PATH from GLCOMPANY where COMPCD = @compcode ";

                    using (SqlCommand com = new SqlCommand(query, con))
                    {
                        con.Open();
                        com.Parameters.AddWithValue("@compcode", compCode);

                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            path = reader["COMP_IMG_PATH"].ToString();

                        }

                    }
                }


                if (!string.IsNullOrEmpty(path))
                {
                    string newPath = path + @"\XML";

                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);

                    }

                    string file = path + @"\XML\fms_materials.xml";
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                    xdoc.Save(newPath + @"\fms_materials.xml");
                    msg.message = "fms_materials.xml";

                }
            }
            catch (Exception ex)
            {
                msg.message = "error";
                throw ex;

            }




            return msg;
        }


        [WebMethod]
        public XmlDocument getUnitList(string compCode,string code)
        {
            DataSet ds = new DataSet();
            Message msg = new Message();
            try
            {
                if (code.Equals("MISC") || code.Equals("MSC") || code == null)
                {
                    using (SqlConnection con = new SqlConnection(gfmConString))
                    {
                        con.Open();

                        // get Maintenance Task
                        using (SqlCommand comm = new SqlCommand("SP_FMSMOB_INUNIT_GET", con))
                        {
                            comm.CommandType = CommandType.StoredProcedure;
                            comm.Parameters.AddWithValue("@compCode", compCode);
                            SqlDataAdapter ad = new SqlDataAdapter(comm);
                            ad.Fill(ds);
                        }



                    }
                }
                else
                {
                    using (SqlConnection con = new SqlConnection(gfmConString))
                    {
                        con.Open();

                        // get Maintenance Task
                        using (SqlCommand comm = new SqlCommand("SP_FMSMOB_INUMST_GET", con))
                        {
                            comm.CommandType = CommandType.StoredProcedure;

                            comm.Parameters.AddWithValue("@compCode", compCode);
                            comm.Parameters.AddWithValue("@code", code);
                            SqlDataAdapter ad = new SqlDataAdapter(comm);
                            ad.Fill(ds);
                        }



                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }



            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());         
           


            return xdoc;
        }



        // Dtat contract to store the basic information about each logged in user

        [DataContract]
        public class employee
        {
            [DataMember]
            public string cd { get; set; }

            [DataMember]
            public string pass { get; set; }

            [DataMember]
            public string name { get; set; }

            [DataMember]
            public string email { get; set; }

            [DataMember]
            public string empcode { get; set; }

            [DataMember]
            public string error { get; set; }

        }

        //------------------------END------------------------------------//



        /// <summary>
        /// Method to get all the status code from sql databse
        /// 
        /// </summary>
        /// <param name="comcode"></param>
        /// <returns></returns>


        [WebMethod]
        public statusCode StatusClass(string comcode)
        {

            statusCode scode = new statusCode();

            string message = "";
            try
            {
                using (SqlConnection con = new SqlConnection(gfmConString))
                {

                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetStatusCode", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@compCode", comcode);
                        SqlDataReader reader = comm.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {

                                scode.assignedCode = reader["JFMS_ASSGST"].ToString();
                                scode.workCompleted = reader["JFMS_WORKCOMP"].ToString();
                                scode.workProgressCode = reader["JFMS_WIP"].ToString();
                            }

                        }

                    }

                }

            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }
            return scode;
        }


        // Data contract to store all the status code

        [DataContract]
        public class statusCode
        {
            [DataMember]
            public string assignedCode { get; set; }

            [DataMember]
            public string workProgressCode { get; set; }

            [DataMember]
            public string workCompleted { get; set; }

            [DataMember]
            public string updatedStatus { get; set; }

            [DataMember]
            public string reading { get; set; }

            
            [DataMember]
            public string defaultUOM { get; set; }

        }

     
        [WebMethod]
        public XmlDocument getTaskCount(string compcode, string assignedcode, string completedcode, string workprogresscode, string empcode,int flag,int option)
        {

            CommonModel model = new CommonModel();
            Helper helpers = new Helper();
            DataSet ds = new DataSet();
            string message = "";
            try
            {
                using (SqlConnection con = new SqlConnection(gfmConString))
                {

                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetTaskCount", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@compCode", compcode);
                        comm.Parameters.AddWithValue("@assignedCode", assignedcode);
                        comm.Parameters.AddWithValue("@completedCode", completedcode);
                        comm.Parameters.AddWithValue("@inProgressCode", workprogresscode);
                        comm.Parameters.AddWithValue("@empCode", empcode);
                        comm.Parameters.AddWithValue("@flag", flag);
                        comm.Parameters.AddWithValue("@isViewed", option);

                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                        //SqlDataReader reader = comm.ExecuteReader();
                        //if (reader.HasRows)
                        //{
                        //    while (reader.Read())
                        //    {
                        //        if(flag == 4)
                        //        {
                        //            model.regularCount = helpers.NZ(reader["REGULAR_COUNT"]);
                        //            model.InspectionCount = helpers.NZ(reader["INSP_COUNT"]);
                        //            model.PPMCount = helpers.NZ(reader["PPM_COUNT"]);
                        //            model.completedCount = helpers.NZ(reader["COMP_COUNT"]);
                        //        }
                        //        else
                        //        {
                        //            model.taskCount = helpers.NZ(reader["TASK_COUNT"]);

                        //        }



                        //    }

                        //}

                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());

            return xdoc;
        }


        //------------------------END------------------------------------//



        /// <summary>
        /// Method to get the number of new task assigned and task status update
        /// </summary>
        /// <param name="comcode"></param>
        /// <param name="assignedcode"></param>
        /// <param name="completedcode"></param>
        /// <param name="workprogresscode"></param>
        /// <param name="empcode"></param>
        /// <returns></returns>

        [WebMethod]
        public XmlDocument TaskCountMethod(string compcode, string assignedcode, string completedcode, string workprogresscode, string empcode)
        {
            string message = "";
            DataSet ds = new DataSet();
            taskCount count = new taskCount();
            try
            {
                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();

                    // get task count
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetAssignedTaskData", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@compCode", compcode);
                        comm.Parameters.AddWithValue("@assignedCode", assignedcode);
                        comm.Parameters.AddWithValue("@completedCode", completedcode);
                        comm.Parameters.AddWithValue("@inProgressCode", workprogresscode);
                        comm.Parameters.AddWithValue("@empCode", empcode);
                        comm.Parameters.AddWithValue("@flag", 0);

                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                    }



                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());

            return xdoc;


        }


        // data contract to store count of task

        [DataContract]
        public class taskCount
        {
            [DataMember]
            public string newTask { get; set; }

            public string viewedTask { get; set; }


        }


        //------------------------END------------------------------------//



        /// <summary>
        /// Method to get the list of tasks
        /// </summary>
        /// <param name="option"></param>
        /// <param name="assignedcode"></param>
        /// <param name="completedcode"></param>
        /// <param name="workprogresscode"></param>
        /// <param name="empcode"></param>
        /// <returns></returns>
        /// 
        [WebMethod]
        public XmlDocument getTaskList(int option, string assignedcode, string completedcode, string workprogresscode, string empcode, string compcode, string pageNo, string taskType)
        {
            string message = "";
            List<taskListItems> test = new List<taskListItems>();
            DataSet ds = new DataSet();
            int pg = Convert.ToInt32(pageNo);
            try
            {

                switch (taskType)
                {

                    case "M":
                        using (SqlConnection con = new SqlConnection(gfmConString))
                        {
                            con.Open();

                            // get Maintenance Task
                            using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetAssignedTaskData", con))
                            {
                                comm.CommandType = CommandType.StoredProcedure;

                                comm.Parameters.AddWithValue("@compCode", compcode);
                                comm.Parameters.AddWithValue("@assignedCode", assignedcode);
                                comm.Parameters.AddWithValue("@completedCode", completedcode);
                                comm.Parameters.AddWithValue("@inProgressCode", workprogresscode);
                                comm.Parameters.AddWithValue("@empCode", empcode);
                                comm.Parameters.AddWithValue("@isViewed", option);
                                comm.Parameters.AddWithValue("@flag", 6);
                                comm.Parameters.AddWithValue("@pageNo", pg);

                                SqlDataAdapter ad = new SqlDataAdapter(comm);
                                ad.Fill(ds);
                            }



                        }
                        break;
                    case "INSP":
                        using (SqlConnection con = new SqlConnection(gfmConString))
                        {
                            con.Open();

                            // get Inspection TASK
                            using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetAssignedTaskData", con))
                            {
                                comm.CommandType = CommandType.StoredProcedure;

                                comm.Parameters.AddWithValue("@compCode", compcode);
                                comm.Parameters.AddWithValue("@assignedCode", assignedcode);
                                comm.Parameters.AddWithValue("@completedCode", completedcode);
                                comm.Parameters.AddWithValue("@inProgressCode", workprogresscode);
                                comm.Parameters.AddWithValue("@empCode", empcode);
                                comm.Parameters.AddWithValue("@isViewed", option);
                                comm.Parameters.AddWithValue("@flag", 4);
                                comm.Parameters.AddWithValue("@pageNo", pg);

                                SqlDataAdapter ad = new SqlDataAdapter(comm);
                                ad.Fill(ds);
                            }



                        }
                        break;

                    case "PPM":

                        // Get PPM TASK
                        using (SqlConnection con = new SqlConnection(gfmConString))
                        {
                            con.Open();

                            // get task count
                            using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetAssignedTaskData", con))
                            {
                                comm.CommandType = CommandType.StoredProcedure;

                                comm.Parameters.AddWithValue("@compCode", compcode);
                                comm.Parameters.AddWithValue("@assignedCode", assignedcode);
                                comm.Parameters.AddWithValue("@completedCode", completedcode);
                                comm.Parameters.AddWithValue("@inProgressCode", workprogresscode);
                                comm.Parameters.AddWithValue("@empCode", empcode);
                                comm.Parameters.AddWithValue("@isViewed", option);
                                comm.Parameters.AddWithValue("@flag", 5);
                                comm.Parameters.AddWithValue("@pageNo", pg);
                                SqlDataAdapter ad = new SqlDataAdapter(comm);
                                ad.Fill(ds);
                            }



                        }
                        break;
                    case "OT":

                        // Get PPM TASK
                        using (SqlConnection con = new SqlConnection(gfmConString))
                        {
                            con.Open();

                            // get task count
                            using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetAssignedTaskData", con))
                            {
                                comm.CommandType = CommandType.StoredProcedure;

                                comm.Parameters.AddWithValue("@compCode", compcode);
                                comm.Parameters.AddWithValue("@assignedCode", assignedcode);
                                comm.Parameters.AddWithValue("@completedCode", completedcode);
                                comm.Parameters.AddWithValue("@inProgressCode", workprogresscode);
                                comm.Parameters.AddWithValue("@empCode", empcode);
                                comm.Parameters.AddWithValue("@isViewed", option);
                                comm.Parameters.AddWithValue("@flag", 7);
                                comm.Parameters.AddWithValue("@pageNo", pg);
                                SqlDataAdapter ad = new SqlDataAdapter(comm);
                                ad.Fill(ds);
                            }



                        }
                        break;

                    case "CT":

                        // Get PPM TASK
                        using (SqlConnection con = new SqlConnection(gfmConString))
                        {
                            con.Open();

                            // get task count
                            using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetAssignedTaskData", con))
                            {
                                comm.CommandType = CommandType.StoredProcedure;

                                comm.Parameters.AddWithValue("@compCode", compcode);
                                comm.Parameters.AddWithValue("@assignedCode", assignedcode);
                                comm.Parameters.AddWithValue("@completedCode", completedcode);
                                comm.Parameters.AddWithValue("@inProgressCode", workprogresscode);
                                comm.Parameters.AddWithValue("@empCode", empcode);
                                comm.Parameters.AddWithValue("@isViewed", option);
                                comm.Parameters.AddWithValue("@flag", 8);
                                comm.Parameters.AddWithValue("@pageNo", pg);
                                SqlDataAdapter ad = new SqlDataAdapter(comm);
                                ad.Fill(ds);
                            }



                        }
                        break;
                    default:

                        // Get ALL task
                        using (SqlConnection con = new SqlConnection(gfmConString))
                        {
                            con.Open();

                            // get task count
                            using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetAssignedTaskData", con))
                            {
                                comm.CommandType = CommandType.StoredProcedure;

                                comm.Parameters.AddWithValue("@compCode", compcode);
                                comm.Parameters.AddWithValue("@assignedCode", assignedcode);
                                comm.Parameters.AddWithValue("@completedCode", completedcode);
                                comm.Parameters.AddWithValue("@inProgressCode", workprogresscode);
                                comm.Parameters.AddWithValue("@empCode", empcode);
                                comm.Parameters.AddWithValue("@isViewed", option);
                                comm.Parameters.AddWithValue("@flag", 1);
                                comm.Parameters.AddWithValue("@pageNo", pg);

                                SqlDataAdapter ad = new SqlDataAdapter(comm);
                                ad.Fill(ds);
                            }



                        }

                        break;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());
            return xdoc;

        }




        [WebMethod]
        public XmlDocument getTaskListWithCount(int option, string assignedcode, string completedcode, string workprogresscode, string empcode, string compcode, string pageNo, string taskType)
        {
            string message = "";
            List<taskListItems> test = new List<taskListItems>();
            DataSet ds = new DataSet();
            int pg = Convert.ToInt32(pageNo);
            try
            {

                switch (taskType)
                {

                    case "M":
                        using (SqlConnection con = new SqlConnection(gfmConString))
                        {
                            con.Open();

                            // get Maintenance Task
                            using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetTaskListWithCount", con))
                            {
                                comm.CommandType = CommandType.StoredProcedure;

                                comm.Parameters.AddWithValue("@compCode", compcode);
                                comm.Parameters.AddWithValue("@assignedCode", assignedcode);
                                comm.Parameters.AddWithValue("@completedCode", completedcode);
                                comm.Parameters.AddWithValue("@inProgressCode", workprogresscode);
                                comm.Parameters.AddWithValue("@empCode", empcode);
                                comm.Parameters.AddWithValue("@isViewed", option);
                                comm.Parameters.AddWithValue("@flag", 2);
                                comm.Parameters.AddWithValue("@pageNo", pg);

                                SqlDataAdapter ad = new SqlDataAdapter(comm);
                                ad.Fill(ds);
                            }



                        }
                        break;
                    case "INSP":
                        using (SqlConnection con = new SqlConnection(gfmConString))
                        {
                            con.Open();

                            // get Inspection TASK
                            using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetTaskListWithCount", con))
                            {
                                comm.CommandType = CommandType.StoredProcedure;

                                comm.Parameters.AddWithValue("@compCode", compcode);
                                comm.Parameters.AddWithValue("@assignedCode", assignedcode);
                                comm.Parameters.AddWithValue("@completedCode", completedcode);
                                comm.Parameters.AddWithValue("@inProgressCode", workprogresscode);
                                comm.Parameters.AddWithValue("@empCode", empcode);
                                comm.Parameters.AddWithValue("@isViewed", option);
                                comm.Parameters.AddWithValue("@flag", 0);
                                comm.Parameters.AddWithValue("@pageNo", pg);

                                SqlDataAdapter ad = new SqlDataAdapter(comm);
                                ad.Fill(ds);
                            }



                        }
                        break;

                    case "PPM":

                        // Get PPM TASK
                        using (SqlConnection con = new SqlConnection(gfmConString))
                        {
                            con.Open();

                            // get task count
                            using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetTaskListWithCount", con))
                            {
                                comm.CommandType = CommandType.StoredProcedure;

                                comm.Parameters.AddWithValue("@compCode", compcode);
                                comm.Parameters.AddWithValue("@assignedCode", assignedcode);
                                comm.Parameters.AddWithValue("@completedCode", completedcode);
                                comm.Parameters.AddWithValue("@inProgressCode", workprogresscode);
                                comm.Parameters.AddWithValue("@empCode", empcode);
                                comm.Parameters.AddWithValue("@isViewed", option);
                                comm.Parameters.AddWithValue("@flag", 1);
                                comm.Parameters.AddWithValue("@pageNo", pg);
                                SqlDataAdapter ad = new SqlDataAdapter(comm);
                                ad.Fill(ds);
                            }



                        }
                        break;
                    case "OT":

                        // Get PPM TASK
                        using (SqlConnection con = new SqlConnection(gfmConString))
                        {
                            con.Open();

                            // get task count
                            using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetTaskListWithCount", con))
                            {
                                comm.CommandType = CommandType.StoredProcedure;

                                comm.Parameters.AddWithValue("@compCode", compcode);
                                comm.Parameters.AddWithValue("@assignedCode", assignedcode);
                                comm.Parameters.AddWithValue("@completedCode", completedcode);
                                comm.Parameters.AddWithValue("@inProgressCode", workprogresscode);
                                comm.Parameters.AddWithValue("@empCode", empcode);
                                comm.Parameters.AddWithValue("@isViewed", option);
                                comm.Parameters.AddWithValue("@flag", 3);
                                comm.Parameters.AddWithValue("@pageNo", pg);
                                SqlDataAdapter ad = new SqlDataAdapter(comm);
                                ad.Fill(ds);
                            }



                        }
                        break;

                    case "CT":

                        // Get PPM TASK
                        using (SqlConnection con = new SqlConnection(gfmConString))
                        {
                            con.Open();

                            // get task count
                            using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetTaskListWithCount", con))
                            {
                                comm.CommandType = CommandType.StoredProcedure;

                                comm.Parameters.AddWithValue("@compCode", compcode);
                                comm.Parameters.AddWithValue("@assignedCode", assignedcode);
                                comm.Parameters.AddWithValue("@completedCode", completedcode);
                                comm.Parameters.AddWithValue("@inProgressCode", workprogresscode);
                                comm.Parameters.AddWithValue("@empCode", empcode);
                                comm.Parameters.AddWithValue("@isViewed", option);
                                comm.Parameters.AddWithValue("@flag", 4);
                                comm.Parameters.AddWithValue("@pageNo", pg);
                                SqlDataAdapter ad = new SqlDataAdapter(comm);
                                ad.Fill(ds);
                            }



                        }
                        break;
                    default:

                        // Get ALL task
                        using (SqlConnection con = new SqlConnection(gfmConString))
                        {
                            con.Open();

                            // get task count
                            using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetAssignedTaskData", con))
                            {
                                comm.CommandType = CommandType.StoredProcedure;

                                comm.Parameters.AddWithValue("@compCode", compcode);
                                comm.Parameters.AddWithValue("@assignedCode", assignedcode);
                                comm.Parameters.AddWithValue("@completedCode", completedcode);
                                comm.Parameters.AddWithValue("@inProgressCode", workprogresscode);
                                comm.Parameters.AddWithValue("@empCode", empcode);
                                comm.Parameters.AddWithValue("@isViewed", option);
                                comm.Parameters.AddWithValue("@flag", 1);
                                comm.Parameters.AddWithValue("@pageNo", pg);

                                SqlDataAdapter ad = new SqlDataAdapter(comm);
                                ad.Fill(ds);
                            }



                        }

                        break;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());
            return xdoc;

        }


        // Data contract to store task list values
        [DataContract]
        public class taskListItems
        {
            [DataMember]
            public string task_id;
            public string call_no;
            public string date;
            public string location;
            public string des;
            public string priority;


            public taskListItems()
            {

            }



            public taskListItems(string task_id, string call_no, string date, string location, string des, string priority)
            {
                this.task_id = task_id;
                this.call_no = call_no;
                this.date = date;
                this.location = location;
                this.des = des;
                this.priority = priority;

            }
        }



        //------------------------END------------------------------------//


        /// <summary>
        ///  Method to update the view status and get the work start status
        /// </summary>
        [WebMethod]
        public int getWorkStatus(string task_no, string compcode, string call_no)
        {
            string message = " ";
            int start = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();

                    // get task count
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetWorkStatus", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@taskNo", task_no);
                        comm.Parameters.AddWithValue("@callNo", call_no);
                        comm.Parameters.AddWithValue("@compcode", compcode);
                        comm.Parameters.AddWithValue("@flag", 0);

                        SqlDataReader reader = comm.ExecuteReader();
                        while (reader.Read())
                        {
                            start = Convert.ToInt32(reader["TD_WORKSTART"].ToString());

                        }
                    }



                }



            }
            catch (Exception ex)
            {
                message = ex.ToString();
                throw ex;

            }


            return start;
        }



        //------------------------END------------------------------------//





        /// <summary>
        /// 
        /// 
        /// Web method to get all the details of a particular task 
        /// </summary>
        /// <param name="task_no"></param>
        /// <param name="empcode"></param>
        /// <param name="compcode"></param>
        /// <param name="assignedcode"></param>
        /// <param name="completedcode"></param>
        /// <param name="workprogresscode"></param>
        /// <returns></returns>

        [WebMethod]
        public TaskDetailModel TaskDetails(string task_no, string empcode, string compcode, string assignedcode, string completedcode, string workprogresscode)
        {

            DataSet ds = new DataSet();
            TaskDetailModel details = new TaskDetailModel();
            try
            {
                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();
                    // get task count
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetAssignedTaskData", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@compCode", compcode);
                        comm.Parameters.AddWithValue("@assignedCode", assignedcode);
                        comm.Parameters.AddWithValue("@completedCode", completedcode);
                        comm.Parameters.AddWithValue("@inProgressCode", workprogresscode);
                        comm.Parameters.AddWithValue("@empCode", empcode);
                        comm.Parameters.AddWithValue("@taskNo", task_no);
                        comm.Parameters.AddWithValue("@flag", 2);

                        SqlDataReader reader = comm.ExecuteReader();
                        while (reader.Read())
                        {
                            details.date = reader[1].ToString();
                            details.location = reader[2].ToString();
                            details.building = reader[3].ToString();
                            details.unit = reader[4].ToString();
                            details.contract = reader[5].ToString();
                            details.person = reader[6].ToString();
                            details.mobile_no = reader[7].ToString();
                            details.landline_no = reader[8].ToString();
                            details.complain = reader[9].ToString();
                            details.priority = reader[10].ToString();
                            details.s_date = reader[11].ToString();
                            details.d_date = reader[12].ToString();
                            details.asset = reader[13].ToString();
                            details.scope = reader[14].ToString();
                            details.latitude = reader[15].ToString();
                            details.longitude = reader[16].ToString();


                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            /// XmlDocument xdoc = new XmlDocument();
            /// xdoc.LoadXml(ds.GetXml());
            return details;
        }


        // data contract for TaskDetailModel 

        [DataContract]

        public class TaskDetailModel
        {
            [DataMember]
            public string date { get; set; }

            [DataMember]
            public string location { get; set; }

            [DataMember]
            public string building { get; set; }

            [DataMember]
            public string unit { get; set; }

            [DataMember]
            public string contract { get; set; }

            [DataMember]
            public string person { get; set; }

            [DataMember]
            public string mobile_no { get; set; }

            [DataMember]
            public string landline_no { get; set; }

            [DataMember]
            public string complain { get; set; }

            [DataMember]
            public string priority { get; set; }

            [DataMember]
            public string s_date { get; set; }

            [DataMember]
            public string d_date { get; set; }
            [DataMember]
            public string asset { get; set; }
            [DataMember]
            public string scope { get; set; }

            [DataMember]
            public string latitude { get; set; }
            [DataMember]
            public string longitude { get; set; }

        }

        //------------------------END------------------------------------//


        /// <summary>
        /// F
        /// 
        /// Web method to get all the details of a particular task 
        /// </summary>
        /// <param name="task_no"></param>
        /// <param name="empcode"></param>
        /// <param name="compcode"></param>
        /// <param name="assignedcode"></param>
        /// <param name="completedcode"></param>
        /// <param name="workprogresscode"></param>
        /// <returns></returns>

        [WebMethod]
        public XmlDocument checkTaskStatus(string task_no, string empcode, string compcode, string assignedcode, string completedcode, string workprogresscode)
        {

            DataSet ds = new DataSet();

            try
            {
                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();
                    // get task count
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GetAssignedTaskData", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@compCode", compcode);
                        comm.Parameters.AddWithValue("@assignedCode", assignedcode);
                        comm.Parameters.AddWithValue("@completedCode", completedcode);
                        comm.Parameters.AddWithValue("@inProgressCode", workprogresscode);
                        comm.Parameters.AddWithValue("@empCode", empcode);
                        comm.Parameters.AddWithValue("@taskNo", task_no);
                        comm.Parameters.AddWithValue("@flag", 3);


                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                    }
                }



            }

            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());



            return xdoc;
        }




        [WebMethod]
        public Message insert(string compcode, string imagetype, string doc_date, string call_no, string reported_date, string building, string location, string task_id, string workcompleted)
        {
            Message message = new Message();

            Doc_no doc_no = new Doc_no();
            bool hasRow = false;
            bool generate = false;
            bool inserted = false;
            try
            {
                SqlConnection con = new SqlConnection(gfmConString);


                if (imagetype.Equals("B"))
                {

                    try
                    {
                        con.Open();
                        string q = "select TH_DOC_NO from FMCALL_INSP where TH_CALLREFNO =@call_no AND TH_CompCd =@compcode AND TH_TASK_NO = @task_id";
                        SqlCommand comm = new SqlCommand(q, con);
                        comm.Parameters.AddWithValue("@call_no", call_no);
                        comm.Parameters.AddWithValue("@compcode", compcode);
                        comm.Parameters.AddWithValue("@task_id", task_id);

                        SqlDataReader reader = comm.ExecuteReader();

                        while (reader.Read())
                        {

                            if (reader["TH_DOC_NO"] != DBNull.Value)
                            {
                                doc_no.doc_no = reader["TH_DOC_NO"].ToString();
                                hasRow = true;

                            }
                            else
                            {
                                hasRow = false;

                            }


                        }

                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        message.message = "error";

                        throw ex;

                    }

                    try
                    {
                        if (!hasRow)
                        {
                            con.Open();
                            string query_doc_number = "exec [GEN_FN_GETANDROIDDOCNOFORINSP] @compcode";
                            SqlCommand comm_1 = new SqlCommand(query_doc_number, con);
                            comm_1.Parameters.AddWithValue("@compcode", compcode);
                            SqlDataReader r = comm_1.ExecuteReader();
                            while (r.Read())
                            {
                                doc_no.doc_no = r[0].ToString();
                                generate = true;
                            }



                            con.Close();


                        }
                    }
                    catch (Exception ex)
                    {
                        message.message = "error";

                        throw ex;

                    }

                    try
                    {
                        if (generate && !hasRow)
                        {
                            string doc_type = null;

                            try
                            {
                                con.Open();
                                string get_doc = "select  JFMS_INSPDEFDOC from JBCONT where JJCOMPCD=@compcode";
                                SqlCommand comm = new SqlCommand(get_doc, con);
                                comm.Parameters.AddWithValue("@compcode", compcode);

                                SqlDataReader reader = comm.ExecuteReader();

                                while (reader.Read())
                                {

                                    if (reader["JFMS_INSPDEFDOC"] != DBNull.Value)
                                    {
                                        doc_type = reader["JFMS_INSPDEFDOC"].ToString();

                                    }


                                }

                                con.Close();
                            }
                            catch (Exception ex)
                            {
                                message.message = "error";

                                throw ex;

                            }

                            if (doc_type != null)
                            {
                                con.Open();


                                imagetype = "I";

                                // Store the remarks in TH_OBSERVATION column of FMCALL_INSP table.

                                string update = "insert into FMCALL_INSP (TH_CompCd,TH_TYPE,TH_DOC_TY,TH_DOC_NO,TH_DOC_DATE,TH_CALLREFNO,TH_REPORTED_DATE,TH_BUILDING,TH_LOCATION,TH_WORKTYPE,TH_TASK_NO ) " +
                                    " values(@compcode, @imagetype, @doc_type, @doc_no, @doc_date, @call_no, @repoeted_date, @building, @location, ' ' , @task_id) ";

                                SqlCommand com = new SqlCommand(update, con);
                                com.Parameters.AddWithValue("@compcode", compcode);
                                com.Parameters.AddWithValue("@imagetype", imagetype);
                                com.Parameters.AddWithValue("@doc_type", doc_type);
                                com.Parameters.AddWithValue("@doc_no", doc_no.doc_no);
                                com.Parameters.AddWithValue("@doc_date", doc_date);
                                com.Parameters.AddWithValue("@call_no", call_no);
                                com.Parameters.AddWithValue("@repoeted_date", reported_date);
                                com.Parameters.AddWithValue("@building", building);
                                com.Parameters.AddWithValue("@location", location);
                                com.Parameters.AddWithValue("@task_id", task_id);

                                int rows = com.ExecuteNonQuery();
                                if (rows != 0)
                                {
                                    inserted = true;

                                    message.message = "inserted";


                                }
                                else
                                {
                                    inserted = false;

                                    message.message = "error";

                                }


                                con.Close();
                            }
                            else
                            {
                                message.message = "error";

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        message.message = "error";

                        throw ex;

                    }
                }

                /////// Insert for After picture

                else if (imagetype.Equals("A"))
                {

                    try
                    {
                        con.Open();
                        string q = "select TH_DOC_NO from FMCALL_WIP where TH_CALLREFNO =@call_no AND TH_CompCd =@compcode AND TH_TASK_NO = @task_id";
                        SqlCommand comm = new SqlCommand(q, con);
                        comm.Parameters.AddWithValue("@call_no", call_no);
                        comm.Parameters.AddWithValue("@compcode", compcode);
                        comm.Parameters.AddWithValue("@task_id", task_id);

                        SqlDataReader reader = comm.ExecuteReader();

                        while (reader.Read())
                        {

                            if (reader["TH_DOC_NO"] != DBNull.Value)
                            {
                                doc_no.doc_no = reader["TH_DOC_NO"].ToString();
                                hasRow = true;

                            }
                            else
                            {
                                hasRow = false;

                            }


                        }

                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        message.message = "error";

                        throw ex;

                    }

                    try
                    {
                        if (!hasRow)
                        {
                            con.Open();
                            string query_doc_number = "exec [GEN_FN_GETANDROIDDOCNOFORWIP] @compcode";
                            SqlCommand comm_1 = new SqlCommand(query_doc_number, con);
                            comm_1.Parameters.AddWithValue("@compcode", compcode);
                            SqlDataReader r = comm_1.ExecuteReader();
                            while (r.Read())
                            {
                                doc_no.doc_no = r[0].ToString();
                                generate = true;
                            }



                            con.Close();


                        }
                    }
                    catch (Exception ex)
                    {
                        message.message = "error";

                        throw ex;

                    }

                    try
                    {

                        string insp = "";

                        if (generate && !hasRow)
                        {
                            con.Open();
                            string q2 = "select TH_DOC_NO from FMCALL_INSP where TH_CompCd=@compcode AND TH_CALLREFNO =@call_no AND TH_TASK_NO =@task_id ";
                            SqlCommand comm2 = new SqlCommand(q2, con);
                            comm2.Parameters.AddWithValue("@compcode", compcode);
                            comm2.Parameters.AddWithValue("@task_id", task_id);
                            comm2.Parameters.AddWithValue("@call_no", call_no);
                            SqlDataReader reader2 = comm2.ExecuteReader();
                            while (reader2.Read())
                            {
                                if (reader2["TH_DOC_NO"] != DBNull.Value)
                                {
                                    insp = reader2["TH_DOC_NO"].ToString();

                                }
                                else
                                {
                                    insp = null;
                                }
                            }

                            con.Close();

                            string doc_type = null;

                            try
                            {
                                con.Open();
                                string get_doc = "select  JFMS_WIPDEFDOC from JBCONT where JJCOMPCD=@compcode";
                                SqlCommand comm = new SqlCommand(get_doc, con);
                                comm.Parameters.AddWithValue("@compcode", compcode);

                                SqlDataReader reader = comm.ExecuteReader();

                                while (reader.Read())
                                {

                                    if (reader["JFMS_WIPDEFDOC"] != DBNull.Value)
                                    {
                                        doc_type = reader["JFMS_WIPDEFDOC"].ToString();

                                    }


                                }

                                con.Close();
                            }
                            catch (Exception ex)
                            {
                                message.message = "error";

                                throw ex;

                            }


                            if (doc_type != null)
                            {
                                con.Open();

                                imagetype = "W";



                                string update = "insert into FMCALL_WIP (TH_CompCd,TH_TYPE,TH_DOC_TY,TH_DOC_NO,TH_DOC_DATE,TH_CALLREFNO,TH_REPORTED_DATE,TH_BUILDING,TH_LOCATION,TH_WORKTYPE,TH_TASK_NO ,TH_INSP_NO) " +
                                    " values(@compcode, @imagetype, @doc_type, @doc_no, @doc_date, @call_no, @repoeted_date, @building, @location, ' ' , @task_id , @insp) ";

                                SqlCommand com = new SqlCommand(update, con);
                                com.Parameters.AddWithValue("@compcode", compcode);
                                com.Parameters.AddWithValue("@imagetype", imagetype);
                                com.Parameters.AddWithValue("@doc_type", doc_type);
                                com.Parameters.AddWithValue("@doc_no", doc_no.doc_no);
                                com.Parameters.AddWithValue("@doc_date", doc_date);
                                com.Parameters.AddWithValue("@call_no", call_no);
                                com.Parameters.AddWithValue("@repoeted_date", reported_date);
                                com.Parameters.AddWithValue("@building", building);
                                com.Parameters.AddWithValue("@location", location);
                                com.Parameters.AddWithValue("@task_id", task_id);
                                com.Parameters.AddWithValue("@insp", insp);

                                int rows = com.ExecuteNonQuery();
                                if (rows != 0)
                                {
                                    inserted = true;

                                    message.message = "inserted";


                                }
                                else
                                {
                                    inserted = false;

                                    message.message = "error";

                                }



                                con.Close();
                            }
                            else
                            {
                                message.message = "error";

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        message.message = "error";

                        throw ex;

                    }
                }

                //////////////// insert for close picture 


                else if (imagetype.Equals("C"))
                {
                    string insp = "D";
                    string wip = "D";

                    try
                    {
                        con.Open();
                        string q = "select TH_DOC_NO from FMCALL_INSP where TH_CompCd=@compcode AND TH_CALLREFNO =@call_no AND TH_TASK_NO =@task_id ";
                        SqlCommand comm = new SqlCommand(q, con);
                        comm.Parameters.AddWithValue("@compcode", compcode);
                        comm.Parameters.AddWithValue("@task_id", task_id);
                        comm.Parameters.AddWithValue("@call_no", call_no);
                        SqlDataReader reader = comm.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["TH_DOC_NO"] != DBNull.Value)
                            {
                                insp = reader["TH_DOC_NO"].ToString();

                            }
                            else
                            {
                                insp = null;
                            }
                        }

                        con.Close();


                        con.Open();
                        string q2 = "select TH_DOC_NO from FMCALL_WIP where TH_CompCd=@compcode AND TH_CALLREFNO =@call_no AND TH_TASK_NO =@task_id ";
                        SqlCommand comm2 = new SqlCommand(q2, con);
                        comm2.Parameters.AddWithValue("@compcode", compcode);
                        comm2.Parameters.AddWithValue("@task_id", task_id);
                        comm2.Parameters.AddWithValue("@call_no", call_no);
                        SqlDataReader reader2 = comm2.ExecuteReader();
                        while (reader2.Read())
                        {
                            if (reader2["TH_DOC_NO"] != DBNull.Value)
                            {
                                wip = reader2["TH_DOC_NO"].ToString();

                            }
                            else
                            {
                                wip = null;
                            }
                        }

                        con.Close();

                    }
                    catch (Exception ex)
                    {
                        message.message = "error";

                        throw ex;

                    }

                    if (insp != "D" && wip != "D")
                    {
                        try
                        {
                            con.Open();
                            string q = "select TH_DOC_NO from FMCALL_INSPCOMP where TH_CALLREFNO =@call_no AND TH_CompCd =@compcode AND TH_TASK_NO = @task_id";
                            SqlCommand comm = new SqlCommand(q, con);
                            comm.Parameters.AddWithValue("@call_no", call_no);
                            comm.Parameters.AddWithValue("@compcode", compcode);
                            comm.Parameters.AddWithValue("@task_id", task_id);

                            SqlDataReader reader = comm.ExecuteReader();

                            while (reader.Read())
                            {

                                if (reader["TH_DOC_NO"] != DBNull.Value)
                                {
                                    doc_no.doc_no = reader["TH_DOC_NO"].ToString();
                                    hasRow = true;

                                }
                                else
                                {
                                    hasRow = false;

                                }


                            }

                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            message.message = "error";

                            throw ex;

                        }

                        try
                        {

                            if (!hasRow)
                            {
                                con.Open();
                                string query_doc_number = "exec [GEN_FN_GETANDROIDDOCNOFORINSPCOMP] @compcode";
                                SqlCommand comm_1 = new SqlCommand(query_doc_number, con);
                                comm_1.Parameters.AddWithValue("@compcode", compcode);
                                SqlDataReader r = comm_1.ExecuteReader();
                                while (r.Read())
                                {
                                    doc_no.doc_no = r[0].ToString();
                                    generate = true;
                                }



                                con.Close();


                            }
                        }
                        catch (Exception ex)
                        {
                            message.message = "error";

                            throw ex;

                        }

                        try
                        {

                            if (generate && !hasRow)
                            {
                                string doc_type = null;

                                try
                                {
                                    con.Open();
                                    string get_doc = "select  JFMS_COMPDEFDOC from JBCONT where JJCOMPCD=@compcode";
                                    SqlCommand comm = new SqlCommand(get_doc, con);
                                    comm.Parameters.AddWithValue("@compcode", compcode);

                                    SqlDataReader reader = comm.ExecuteReader();

                                    while (reader.Read())
                                    {

                                        if (reader["JFMS_COMPDEFDOC"] != DBNull.Value)
                                        {
                                            doc_type = reader["JFMS_COMPDEFDOC"].ToString();

                                        }


                                    }

                                    con.Close();
                                }
                                catch (Exception ex)
                                {
                                    message.message = "error";

                                    throw ex;

                                }



                                if (doc_type != null)
                                {
                                    con.Open();


                                    imagetype = "C";

                                    string update = "insert into FMCALL_INSPCOMP (TH_COMPCD,TH_TYPE,TH_DOC_TY,TH_DOC_NO,TH_DOC_DATE,TH_CALLREFNO,TH_INSP_NO ,TH_WIP_NO ,TH_BUILDING,TH_LOCATION ,TH_TASK_NO,TH_COMPLETIONDATE) " +
                                        "values(@compcode, @imagetype, @doc_type, @doc_no, @doc_date, @call_no, @insp, @wip, @building, @location , @task_id ,@doc_date)";

                                    SqlCommand com = new SqlCommand(update, con);
                                    com.Parameters.AddWithValue("@compcode", compcode);
                                    com.Parameters.AddWithValue("@imagetype", imagetype);
                                    com.Parameters.AddWithValue("@doc_type", doc_type);
                                    com.Parameters.AddWithValue("@doc_no", doc_no.doc_no);
                                    com.Parameters.AddWithValue("@doc_date", doc_date);
                                    com.Parameters.AddWithValue("@call_no", call_no);
                                    com.Parameters.AddWithValue("@building", building);
                                    com.Parameters.AddWithValue("@location", location);
                                    com.Parameters.AddWithValue("@task_id", task_id);
                                    com.Parameters.AddWithValue("@insp", insp);
                                    com.Parameters.AddWithValue("@wip", wip);

                                    int rows = com.ExecuteNonQuery();
                                    if (rows != 0)
                                    {
                                        inserted = true;

                                        message.message = "inserted";


                                    }
                                    else
                                    {
                                        inserted = false;

                                        message.message = "error";

                                    }



                                    con.Close();

                                    con.Open();
                                    string query1 = "UPDATE FMTASKH SET TH_STATUS=@workcompleted  WHERE TH_COMPCD=@compcode AND TH_TASK_NO=@task_id";
                                    SqlCommand comm3 = new SqlCommand(query1, con);
                                    comm3.Parameters.AddWithValue("@compcode", compcode);
                                    comm3.Parameters.AddWithValue("@task_id", task_id);
                                    comm3.Parameters.AddWithValue("@workcompleted", workcompleted);

                                    rows = comm3.ExecuteNonQuery();
                                    if (rows != 0)
                                    {
                                        inserted = true;

                                        message.message = "inserted";


                                    }
                                    else
                                    {
                                        inserted = false;

                                        message.message = "error";

                                    }

                                    con.Close();

                                }
                                else
                                {
                                    message.message = "error";

                                }







                            }
                        }
                        catch (Exception ex)
                        {
                            message.message = "error";

                            throw ex;

                        }
                    }

                    else if (insp == "D" || wip == "D")
                    {
                        try
                        {
                            con.Open();
                            string q = "select TH_DOC_NO from FMCALL_INSP_DIRECT where TH_CALLREFNO =@call_no AND TH_CompCd =@compcode AND TH_TASK_NO = @task_id";
                            SqlCommand comm = new SqlCommand(q, con);
                            comm.Parameters.AddWithValue("@call_no", call_no);
                            comm.Parameters.AddWithValue("@compcode", compcode);
                            comm.Parameters.AddWithValue("@task_id", task_id);

                            SqlDataReader reader = comm.ExecuteReader();

                            while (reader.Read())
                            {

                                if (reader["TH_DOC_NO"] != DBNull.Value)
                                {
                                    doc_no.doc_no = reader["TH_DOC_NO"].ToString();
                                    hasRow = true;

                                }
                                else
                                {
                                    hasRow = false;

                                }


                            }

                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            message.message = "error";

                            throw ex;

                        }

                        try
                        {

                            if (!hasRow)
                            {
                                con.Open();
                                string query_doc_number = "exec [GEN_FN_GETANDROIDDOCNOFORINSPDIRECT] @compcode";
                                SqlCommand comm_1 = new SqlCommand(query_doc_number, con);
                                comm_1.Parameters.AddWithValue("@compcode", compcode);
                                SqlDataReader r = comm_1.ExecuteReader();
                                while (r.Read())
                                {
                                    doc_no.doc_no = r[0].ToString();
                                    generate = true;
                                }



                                con.Close();


                            }
                        }
                        catch (Exception ex)
                        {
                            message.message = "error";

                            throw ex;

                        }

                        try
                        {

                            if (generate && !hasRow)
                            {
                                string doc_type = null;

                                try
                                {
                                    con.Open();
                                    string get_doc = "select  JFMS_DIRDOC from JBCONT where JJCOMPCD=@compcode";
                                    SqlCommand comm = new SqlCommand(get_doc, con);
                                    comm.Parameters.AddWithValue("@compcode", compcode);

                                    SqlDataReader reader = comm.ExecuteReader();

                                    while (reader.Read())
                                    {

                                        if (reader["JFMS_DIRDOC"] != DBNull.Value)
                                        {
                                            doc_type = reader["JFMS_DIRDOC"].ToString();

                                        }


                                    }

                                    con.Close();
                                }
                                catch (Exception ex)
                                {
                                    message.message = "error";

                                    throw ex;

                                }



                                if (doc_type != null)
                                {
                                    con.Open();





                                    imagetype = "D";

                                    string update = "insert into FMCALL_INSP_DIRECT (TH_COMPCD,TH_TYPE,TH_DOC_TY,TH_DOC_NO,TH_DOC_DATE,TH_CALLREFNO,TH_BUILDING,TH_LOCATION ,TH_TASK_NO,TH_COMPLETIONDATE) " +
                                        "values(@compcode, @imagetype, @doc_type, @doc_no, @doc_date, @call_no, @building, @location , @task_id,@comp_date)";

                                    SqlCommand com = new SqlCommand(update, con);
                                    com.Parameters.AddWithValue("@compcode", compcode);
                                    com.Parameters.AddWithValue("@imagetype", imagetype);
                                    com.Parameters.AddWithValue("@doc_type", doc_type);
                                    com.Parameters.AddWithValue("@doc_no", doc_no.doc_no);
                                    com.Parameters.AddWithValue("@doc_date", doc_date);
                                    com.Parameters.AddWithValue("@call_no", call_no);
                                    com.Parameters.AddWithValue("@building", building);
                                    com.Parameters.AddWithValue("@location", location);
                                    com.Parameters.AddWithValue("@task_id", task_id);
                                    com.Parameters.AddWithValue("@comp_date", doc_date);

                                    int rows = com.ExecuteNonQuery();
                                    if (rows != 0)
                                    {
                                        inserted = true;

                                        message.message = "inserted";


                                    }
                                    else
                                    {
                                        inserted = false;

                                        message.message = "error";

                                    }



                                    con.Close();

                                    con.Open();
                                    string query1 = "UPDATE FMTASKH SET TH_STATUS=@workcompleted  WHERE TH_COMPCD=@compcode AND TH_TASK_NO=@task_id";
                                    SqlCommand comm3 = new SqlCommand(query1, con);
                                    comm3.Parameters.AddWithValue("@compcode", compcode);
                                    comm3.Parameters.AddWithValue("@task_id", task_id);
                                    comm3.Parameters.AddWithValue("@workcompleted", workcompleted);

                                    rows = comm3.ExecuteNonQuery();
                                    if (rows != 0)
                                    {
                                        inserted = true;

                                        message.message = "inserted";


                                    }
                                    else
                                    {
                                        inserted = false;

                                        message.message = "error";

                                    }

                                    con.Close();


                                }
                                else
                                {
                                    message.message = "error";

                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            message.message = "error";

                            throw ex;

                        }
                    }




                }






            }
            catch (Exception ex)
            {
                throw ex;

            }
            return message;

        }



        [WebMethod]
        public Message headerInsert(string compcode, string call_no, string task_id, string userCode)
        {
            Message message = new Message();
            try
            {
                string doc_date = string.Empty;
                doc_date = DateTime.Now.ToString("yyyy-MM-dd");

                using (SqlConnection con = new SqlConnection(gfmConString))
                {

                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMCALL_INSP_DIRECT_SAVE", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@hId", compcode);

                        comm.Parameters.AddWithValue("@compCode", compcode);
                        comm.Parameters.AddWithValue("@docDate", doc_date);
                        comm.Parameters.AddWithValue("@callNo", call_no);
                        comm.Parameters.AddWithValue("@subject", " ");
                        comm.Parameters.AddWithValue("@description", " ");
                        comm.Parameters.AddWithValue("@completionDate", null);
                        comm.Parameters.AddWithValue("@taskNo", task_id);
                        comm.Parameters.AddWithValue("@observation", " ");
                        comm.Parameters.AddWithValue("@reason", " ");
                        comm.Parameters.AddWithValue("@userCode", userCode);

                        message.value = Convert.ToInt32(comm.ExecuteScalar());
                    }
                }               
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return message;



        }




        [WebMethod]
        public Message udpateWorkStatus(string compcode, string date, string call_no, string task_id, string remarks,string userCode)
        {
            Message message = new Message();
            try
            {




                using (SqlConnection con = new SqlConnection(gfmConString))
                {

                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMTASKH_UPDATE", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@compCode", compcode);
                        comm.Parameters.AddWithValue("@taskNo", task_id);
                        comm.Parameters.AddWithValue("@remarks", remarks);
                        comm.Parameters.AddWithValue("@callNo", call_no);
                        comm.Parameters.AddWithValue("@date", date);
                        comm.Parameters.AddWithValue("@flag", 0);
                        comm.Parameters.AddWithValue("@userCode", userCode);


                        int rows = comm.ExecuteNonQuery();
                        if (rows != 0)
                        {
                            message.message = "inserted";

                        }
                        else
                        {
                            message.message = "error";

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return message;
        }



        /*  [WebMethod]
          public Message saveImage(string call_no, string compcode, string imagetype, string remarks, string Value, string task_id)
          {
              SqlConnection con = new SqlConnection(gfmConString);
              DataSet ds = new DataSet();
              string hid = "";
              string doc_no = "";
              bool stored = false;
              string pic_path = "";
              int count = 0;
              Message message = new Message();
              string path = null;

              try
              {
                  con.Open();

                  string query = "select COMP_IMG_PATH from GLCOMPANY where COMPCD = @compcode ";
                  SqlCommand com = new SqlCommand(query, con);
                  com.Parameters.AddWithValue("@compcode", compcode);

                  SqlDataReader reader = com.ExecuteReader();
                  while (reader.Read())
                  {
                      path = reader["COMP_IMG_PATH"].ToString();

                  }

                  con.Close();

              }
              catch (Exception ex)
              {
                  message.message = "error";

                  throw ex;

              }




              ////////////////////  insert for After picture 

              if (path != null)
              {
                  if (imagetype.Equals("B"))
                  {
                      try
                      {
                          con.Open();

                          string query = "select TH_HID ,TH_DOC_NO from FMCALL_INSP_DIRECT where TH_CALLREFNO = @call_no AND TH_CompCd = @compcode AND TH_TASK_NO = @task_id";
                          SqlCommand com = new SqlCommand(query, con);
                          com.Parameters.AddWithValue("@compcode", compcode);
                          com.Parameters.AddWithValue("@call_no", call_no);
                          com.Parameters.AddWithValue("@task_id", task_id);

                          SqlDataReader reader = com.ExecuteReader();
                          while (reader.Read())
                          {
                              hid = reader["TH_HID"].ToString();
                              doc_no = reader["TH_DOC_NO"].ToString();

                          }

                          con.Close();

                          if (doc_no != "")
                          {

                              string newPath = path + @"\" + doc_no;
                              if (!Directory.Exists(newPath))
                              {
                                  Directory.CreateDirectory(newPath);

                              }


                              string base64 = Value;
                              // for (int i = 0; i < base64.Length; i++)
                              {
                                  byte[] imageBytes = Convert.FromBase64String(base64);
                                  MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                  ms.Write(imageBytes, 0, imageBytes.Length);
                                  System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);


                                  string finalPath = newPath + @"\" + Guid.NewGuid() + ".jpg";


                                  image.Save(finalPath, System.Drawing.Imaging.ImageFormat.Jpeg);


                                  pic_path = finalPath;
                                  stored = true;

                              }

                          }
                          else
                          {
                              message.message = "error";

                          }






                      }
                      catch (Exception ex)
                      {
                          throw ex;
                      }

                      if (stored)
                      {
                          try
                          {
                              string line = "0";
                              con.Open();
                              string query = "select TOP 1 TD_LNO from FMCALL_PIC where TD_DOC_NO = @doc_no AND TD_COMPCD = @compcode AND TH_HID =@hid order by TD_LNO DESC ";
                              SqlCommand comm = new SqlCommand(query, con);
                              comm.Parameters.AddWithValue("@hid", hid);
                              comm.Parameters.AddWithValue("@compcode", compcode);
                              comm.Parameters.AddWithValue("@doc_no", doc_no);
                              SqlDataReader read = comm.ExecuteReader();
                              while (read.Read())
                              {
                                  if (read["TD_LNO"] != DBNull.Value)
                                  {
                                      line = read["TD_LNO"].ToString();
                                  }
                                  else
                                  {
                                      line = "0";
                                  }


                              }
                              con.Close();


                              con.Open();

                              int temp = Int32.Parse(line);
                              temp = temp + 1;

                              string q = "insert into FMCALL_PIC (TH_HID,TD_TYPE,TD_COMPCD,TD_DOC_NO,TD_LNO,TD_PATH) " +
                                  " values(@hid, @imagetype, @compcode, @doc_no, @lno, @path)";
                              SqlCommand com = new SqlCommand(q, con);
                              com.Parameters.AddWithValue("@hid", hid);
                              com.Parameters.AddWithValue("@imagetype", "I");
                              com.Parameters.AddWithValue("@compcode", compcode);
                              com.Parameters.AddWithValue("@doc_no", doc_no);
                              com.Parameters.AddWithValue("@lno", temp.ToString());
                              com.Parameters.AddWithValue("@path", pic_path);
                              int rows = com.ExecuteNonQuery();
                              if (rows != 0)
                              {
                                  message.message = "Saved";


                              }
                              else
                              {
                                  message.message = "error";

                              }
                              con.Close();




                              con.Open();



                              string r = "update FMCALL_INSP SET TH_OBSERVATION =@remark where TH_CALLREFNO = @call_no AND TH_CompCd = @compcode AND TH_TASK_NO = @task_id";
                              SqlCommand comm3 = new SqlCommand(r, con);
                              comm3.Parameters.AddWithValue("@compcode", compcode);
                              comm3.Parameters.AddWithValue("@task_id", task_id);
                              comm3.Parameters.AddWithValue("@remark", remarks);
                              comm3.Parameters.AddWithValue("@call_no", call_no);

                              rows = comm3.ExecuteNonQuery();
                              if (rows != 0)
                              {
                                  message.message = "Saved";


                              }
                              else
                              {
                                  message.message = "error";

                              }
                              con.Close();


                          }
                          catch (Exception ex)
                          {
                              message.message = "error";

                              throw ex;
                          }
                      }
                      else
                      {
                          message.message = "error";

                      }






                  }

                  ////////////////////  insert for before picture 

                  else if (imagetype.Equals("A"))
                  {


                      try
                      {
                          con.Open();

                          string query = "select TH_HID ,TH_DOC_NO from FMCALL_WIP where TH_CALLREFNO = @call_no AND TH_CompCd = @compcode AND TH_TASK_NO = @task_id";
                          SqlCommand com = new SqlCommand(query, con);
                          com.Parameters.AddWithValue("@compcode", compcode);
                          com.Parameters.AddWithValue("@call_no", call_no);
                          com.Parameters.AddWithValue("@task_id", task_id);

                          SqlDataReader reader = com.ExecuteReader();
                          while (reader.Read())
                          {
                              hid = reader["TH_HID"].ToString();
                              doc_no = reader["TH_DOC_NO"].ToString();

                          }

                          con.Close();


                          if (doc_no != "")
                          {
                              string newPath = path + @"\" + doc_no;
                              if (!Directory.Exists(newPath))
                              {
                                  Directory.CreateDirectory(newPath);

                              }




                              string base64 = Value;
                              //for (int i = 0; i < base64.Length; i++)
                              {
                                  byte[] imageBytes = Convert.FromBase64String(base64);
                                  MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                  ms.Write(imageBytes, 0, imageBytes.Length);
                                  System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);


                                  string finalPath = newPath + @"\" + Guid.NewGuid() + ".jpg";


                                  image.Save(finalPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                                  pic_path = finalPath;
                                  stored = true;
                              }
                          }
                          else
                          {
                              message.message = "error";

                          }









                      }
                      catch (Exception ex)
                      {
                          message.message = "error";

                          throw ex;
                      }

                      if (stored)
                      {
                          try
                          {
                              string line = "0";
                              con.Open();
                              string query = "select TOP 1 TD_LNO from FMCALL_PIC where TD_DOC_NO = @doc_no AND TD_COMPCD = @compcode AND TH_HID =@hid order by TD_LNO DESC ";
                              SqlCommand comm = new SqlCommand(query, con);
                              comm.Parameters.AddWithValue("@hid", hid);
                              comm.Parameters.AddWithValue("@compcode", compcode);
                              comm.Parameters.AddWithValue("@doc_no", doc_no);

                              SqlDataReader read = comm.ExecuteReader();
                              while (read.Read())
                              {
                                  if (read["TD_LNO"] != DBNull.Value)
                                  {
                                      line = read["TD_LNO"].ToString();
                                  }
                                  else
                                  {
                                      line = "0";
                                  }


                              }
                              con.Close();


                              con.Open();

                              int temp = Int32.Parse(line);
                              temp = temp + 1;
                              string l = Convert.ToString(temp);


                              string q = "insert into FMCALL_PIC (TH_HID,TD_TYPE,TD_COMPCD,TD_DOC_NO,TD_LNO,TD_PATH) " +
                                  " values(@hid, @imagetype, @compcode, @doc_no, @lno, @path)";
                              SqlCommand com = new SqlCommand(q, con);
                              com.Parameters.AddWithValue("@hid", hid);
                              com.Parameters.AddWithValue("@imagetype", "W");
                              com.Parameters.AddWithValue("@compcode", compcode);
                              com.Parameters.AddWithValue("@doc_no", doc_no);
                              com.Parameters.AddWithValue("@lno", l);
                              com.Parameters.AddWithValue("@path", pic_path);
                              int rows = com.ExecuteNonQuery();
                              if (rows != 0)
                              {
                                  message.message = "Saved";


                              }
                              else
                              {
                                  message.message = "error";

                              }
                              con.Close();



                              con.Open();



                              string r = "update FMCALL_WIP SET TH_OBSERVATION =@remark where TH_CALLREFNO = @call_no AND TH_CompCd = @compcode AND TH_TASK_NO = @task_id";
                              SqlCommand comm3 = new SqlCommand(r, con);
                              comm3.Parameters.AddWithValue("@compcode", compcode);
                              comm3.Parameters.AddWithValue("@task_id", task_id);
                              comm3.Parameters.AddWithValue("@remark", remarks);
                              comm3.Parameters.AddWithValue("@call_no", call_no);

                              rows = comm3.ExecuteNonQuery();
                              if (rows != 0)
                              {
                                  message.message = "Saved";


                              }
                              else
                              {
                                  message.message = "error";

                              }
                              con.Close();


                          }
                          catch (Exception ex)
                          {
                              throw ex;
                          }
                      }
                      else
                      {
                          message.message = "error";

                      }




                  }

                  ////////////////////  insert for CLose picture 

                  else if (imagetype.Equals("C"))
                  {

                      bool inInspComp = false;
                      bool inInspDirect = false;
                      try
                      {

                          con.Open();

                          string query = "select TH_HID ,TH_DOC_NO from FMCALL_INSPCOMP where TH_CALLREFNO = @call_no AND TH_CompCd = @compcode AND TH_TASK_NO = @task_id";
                          SqlCommand com = new SqlCommand(query, con);
                          com.Parameters.AddWithValue("@compcode", compcode);
                          com.Parameters.AddWithValue("@call_no", call_no);
                          com.Parameters.AddWithValue("@task_id", task_id);

                          SqlDataReader reader = com.ExecuteReader();
                          while (reader.Read())
                          {
                              if (reader["TH_HID"] != DBNull.Value)
                              {
                                  hid = reader["TH_HID"].ToString();
                                  doc_no = reader["TH_DOC_NO"].ToString();
                                  inInspComp = true;
                              }


                          }

                          con.Close();


                          con.Open();

                          string query3 = "select TH_HID ,TH_DOC_NO from FMCALL_INSP_DIRECT where TH_CALLREFNO = @call_no AND TH_CompCd = @compcode AND TH_TASK_NO = @task_id";
                          SqlCommand com3 = new SqlCommand(query3, con);
                          com3.Parameters.AddWithValue("@compcode", compcode);
                          com3.Parameters.AddWithValue("@call_no", call_no);
                          com3.Parameters.AddWithValue("@task_id", task_id);

                          SqlDataReader reader3 = com3.ExecuteReader();
                          while (reader3.Read())
                          {
                              if (reader3["TH_HID"] != DBNull.Value)
                              {
                                  hid = reader3["TH_HID"].ToString();
                                  doc_no = reader3["TH_DOC_NO"].ToString();
                                  inInspDirect = true;
                              }


                          }

                          con.Close();

                          if (doc_no != "")
                          {
                              string newPath = path + @"\" + doc_no;
                              if (!Directory.Exists(newPath))
                              {
                                  Directory.CreateDirectory(newPath);

                              }

                              byte[] imageBytes = Convert.FromBase64String(Value);

                              MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                              ms.Write(imageBytes, 0, imageBytes.Length);
                              System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                              string finalPath = newPath + @"\" + Guid.NewGuid() + ".jpg";

                              image.Save(finalPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                              pic_path = finalPath;
                              stored = true;

                          }




                      }
                      catch (Exception ex)
                      {
                          message.message = "error";

                          throw ex;
                      }



                      if (stored)
                      {
                          try
                          {
                              string line = "0";
                              con.Open();
                              string query = "select TOP 1 TD_LNO from FMCALL_PIC where TD_DOC_NO = @doc_no AND TD_COMPCD = @compcode AND TH_HID =@hid order by TD_LNO DESC ";
                              SqlCommand comm = new SqlCommand(query, con);
                              comm.Parameters.AddWithValue("@hid", hid);
                              comm.Parameters.AddWithValue("@compcode", compcode);
                              comm.Parameters.AddWithValue("@doc_no", doc_no);
                              SqlDataReader read = comm.ExecuteReader();
                              while (read.Read())
                              {
                                  if (read["TD_LNO"] != DBNull.Value)
                                  {
                                      line = read["TD_LNO"].ToString();
                                  }
                                  else
                                  {
                                      line = "0";
                                  }


                              }
                              con.Close();


                              con.Open();

                              int temp = Int32.Parse(line);
                              temp = temp + 1;

                              string q = "insert into FMCALL_PIC (TH_HID,TD_TYPE,TD_COMPCD,TD_DOC_NO,TD_LNO,TD_PATH) " +
                                  " values(@hid, @imagetype, @compcode, @doc_no, @lno, @path)";
                              SqlCommand com = new SqlCommand(q, con);
                              com.Parameters.AddWithValue("@hid", hid);
                              com.Parameters.AddWithValue("@imagetype", "C");
                              com.Parameters.AddWithValue("@compcode", compcode);
                              com.Parameters.AddWithValue("@doc_no", doc_no);
                              com.Parameters.AddWithValue("@lno", temp.ToString());
                              com.Parameters.AddWithValue("@path", pic_path);
                              int rows = com.ExecuteNonQuery();

                              if (rows != 0)
                              {
                                  message.message = "Saved";


                              }
                              else
                              {
                                  message.message = "error";

                              }
                              con.Close();


                              if (inInspDirect)
                              {
                                  con.Open();



                                  string r = "update FMCALL_INSP_DIRECT SET TH_DESCRIPTION =@remark where TH_CALLREFNO = @call_no AND TH_COMPCD = @compcode AND TH_TASK_NO = @task_id";
                                  SqlCommand comm3 = new SqlCommand(r, con);
                                  comm3.Parameters.AddWithValue("@compcode", compcode);
                                  comm3.Parameters.AddWithValue("@task_id", task_id);
                                  comm3.Parameters.AddWithValue("@remark", remarks);
                                  comm3.Parameters.AddWithValue("@call_no", call_no);

                                  rows = comm3.ExecuteNonQuery();
                                  if (rows != 0)
                                  {
                                      message.message = "Saved";


                                  }
                                  else
                                  {
                                      message.message = "error";

                                  }
                                  con.Close();
                              }
                              else if (inInspComp)
                              {
                                  con.Open();



                                  string r = "update FMCALL_INSPCOMP SET TH_DESCRIPTION =@remark where TH_CALLREFNO = @call_no AND TH_COMPCD = @compcode AND TH_TASK_NO = @task_id";
                                  SqlCommand comm3 = new SqlCommand(r, con);
                                  comm3.Parameters.AddWithValue("@compcode", compcode);
                                  comm3.Parameters.AddWithValue("@task_id", task_id);
                                  comm3.Parameters.AddWithValue("@remark", remarks);
                                  comm3.Parameters.AddWithValue("@call_no", call_no);

                                  rows = comm3.ExecuteNonQuery();
                                  if (rows != 0)
                                  {
                                      message.message = "Saved";


                                  }
                                  else
                                  {
                                      message.message = "error";

                                  }
                                  con.Close();
                              }



                          }
                          catch (Exception ex)
                          {
                              message.message = "error";

                              throw ex;
                          }
                      }
                      else
                      {
                          message.message = "error";

                      }









                  }
              }
              else
              {
                  message.message = "error";
              }

              return message;

          }*/


        [WebMethod]
        public Message saveImage(string call_no, string compcode, string imagetype, string remarks, string Value, string task_id)
        {
            DataSet ds = new DataSet();
            string hid = "";
            string doc_no = null;
            bool stored = false;
            string pic_path = "";
            int count = 0;
            Message message = new Message();
            string path = null;

            try
            {
                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    string query = "select COMP_IMG_PATH from GLCOMPANY where COMPCD = @compcode ";

                    using (SqlCommand com = new SqlCommand(query, con))
                    {
                        con.Open();
                        com.Parameters.AddWithValue("@compcode", compcode);

                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            path = reader["COMP_IMG_PATH"].ToString();

                        }

                    }
                }



            }
            catch (Exception ex)
            {
                message.message = "error";

                throw ex;

            }

            try
            {

                // get header id and document no from FMCALL_INSP_DIRECT
                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMCALL_INSP_DIRECT_GET", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@flag", 0);
                        comm.Parameters.AddWithValue("@compcode", compcode);
                        comm.Parameters.AddWithValue("@callNo", call_no);
                        comm.Parameters.AddWithValue("@taskNo", task_id);
                        SqlDataReader reader = comm.ExecuteReader();
                        while (reader.Read())
                        {
                            hid = reader["TH_HID"].ToString();
                            doc_no = reader["TH_DOC_NO"].ToString();

                        }

                    }
                }
            }

            catch (Exception ex)
            {
                message.message = "error";

                throw ex;

            }
            ////////////////////  insert for After picture 
            if (path != null && doc_no != null)
            {
                if (imagetype.Equals("B"))
                {
                    try
                    {


                        string newPath = path + @"\" + doc_no+ @"\"+ "Images";
                        if (!Directory.Exists(newPath))
                        {
                            Directory.CreateDirectory(newPath);

                        }

                        /*String after = newPath + @"\After\";
                        if (!Directory.Exists(after))
                        {
                            Directory.CreateDirectory(after);

                        }*/


                        string base64 = Value;
                        // for (int i = 0; i < base64.Length; i++)
                        {
                            byte[] imageBytes = Convert.FromBase64String(base64);
                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            ms.Write(imageBytes, 0, imageBytes.Length);
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);


                            string finalPath = newPath + @"\" + Guid.NewGuid() + ".jpg";


                            image.Save(finalPath, System.Drawing.Imaging.ImageFormat.Jpeg);


                            pic_path = finalPath;
                            stored = true;

                        }


                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    if (stored)
                    {
                        try
                        {
                            // insert into FMCALL_PIC 
                            using (SqlConnection con = new SqlConnection(gfmConString))
                            {
                                using (SqlCommand comm = new SqlCommand("SP_FMSWEB_FMCALL_PIC_SAVE", con))
                                {
                                    con.Open();
                                    comm.CommandType = CommandType.StoredProcedure;
                                    comm.Parameters.AddWithValue("@hid", hid);
                                    comm.Parameters.AddWithValue("@imagetype", "B");
                                    comm.Parameters.AddWithValue("@compcode", compcode);
                                    comm.Parameters.AddWithValue("@doc_no", doc_no);
                                    comm.Parameters.AddWithValue("@path", pic_path);
                                    comm.Parameters.AddWithValue("@TD_REMARKS", remarks);
                                    int rows = Convert.ToInt32(comm.ExecuteScalar());

                                    if (rows != 0)
                                    {
                                        message.message = "Saved";
                                    }
                                    else
                                    {
                                        message.message = "error";
                                    }

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            message.message = "error";

                            throw ex;
                        }
                    }
                    else
                    {
                        message.message = "error";

                    }

                }

                ////////////////////  insert for before picture 

                else if (imagetype.Equals("A"))
                {


                    try
                    {
                        string newPath = path + @"\" + doc_no + @"\" + "Images";
                        if (!Directory.Exists(newPath))
                        {
                            Directory.CreateDirectory(newPath);

                        }
                        /* String after = newPath + @"\Before\";
                         if (!Directory.Exists(after))
                         {
                             Directory.CreateDirectory(after);

                         }*/




                        string base64 = Value;
                        //for (int i = 0; i < base64.Length; i++)
                        {
                            byte[] imageBytes = Convert.FromBase64String(base64);
                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            ms.Write(imageBytes, 0, imageBytes.Length);
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);


                            string finalPath = newPath + @"\" + Guid.NewGuid() + ".jpg";


                            image.Save(finalPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                            pic_path = finalPath;
                            stored = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        message.message = "error";

                        throw ex;
                    }

                    if (stored)
                    {
                        try
                        {

                            // insert into FMCALL_PIC 
                            using (SqlConnection con = new SqlConnection(gfmConString))
                            {
                                using (SqlCommand comm = new SqlCommand("SP_FMSWEB_FMCALL_PIC_SAVE", con))
                                {
                                    con.Open();
                                    comm.CommandType = CommandType.StoredProcedure;
                                    comm.Parameters.AddWithValue("@hid", hid);
                                    comm.Parameters.AddWithValue("@imagetype", "A");
                                    comm.Parameters.AddWithValue("@compcode", compcode);
                                    comm.Parameters.AddWithValue("@doc_no", doc_no);
                                    comm.Parameters.AddWithValue("@path", pic_path);
                                    comm.Parameters.AddWithValue("@TD_REMARKS", remarks);
                                    int rows = Convert.ToInt32(comm.ExecuteScalar());

                                    if (rows != 0)
                                    {
                                        message.message = "Saved";
                                    }
                                    else
                                    {
                                        message.message = "error";
                                    }

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        message.message = "error";

                    }




                }

                ////////////////////  insert for CLose picture 
                else if (imagetype.Equals("C"))
                {
                    try
                    {

                        string newPath = path + @"\" + doc_no + @"\" + "Images";
                        if (!Directory.Exists(newPath))
                        {
                            Directory.CreateDirectory(newPath);

                        }
                        /* String after = newPath + @"\Close\";
                         if (!Directory.Exists(after))
                         {
                             Directory.CreateDirectory(after);

                         }*/

                        byte[] imageBytes = Convert.FromBase64String(Value);

                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        ms.Write(imageBytes, 0, imageBytes.Length);
                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                        string finalPath = newPath + @"\" + Guid.NewGuid() + ".jpg";

                        image.Save(finalPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        pic_path = finalPath;
                        stored = true;

                    }
                    catch (Exception ex)
                    {
                        message.message = "error";

                        throw ex;
                    }

                    if (stored)
                    {
                        try
                        {
                            // insert into FMCALL_PIC 
                            using (SqlConnection con = new SqlConnection(gfmConString))
                            {
                                using (SqlCommand comm = new SqlCommand("SP_FMSWEB_FMCALL_PIC_SAVE", con))
                                {
                                    con.Open();
                                    comm.CommandType = CommandType.StoredProcedure;
                                    comm.Parameters.AddWithValue("@hid", hid);
                                    comm.Parameters.AddWithValue("@imagetype", "C");
                                    comm.Parameters.AddWithValue("@compcode", compcode);
                                    comm.Parameters.AddWithValue("@doc_no", doc_no);
                                    comm.Parameters.AddWithValue("@path", pic_path);
                                    comm.Parameters.AddWithValue("@TD_REMARKS", remarks);
                                    int rows = Convert.ToInt32(comm.ExecuteScalar());

                                    if (rows != 0)
                                    {
                                        message.message = "Saved";
                                    }
                                    else
                                    {
                                        message.message = "error";
                                    }

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            message.message = "error";

                            throw ex;
                        }
                    }
                    else
                    {
                        message.message = "error";

                    }
                }
                // store pictures for inspection task
                else if (imagetype.Equals("TP"))
                {


                    try
                    {
                        string newPath = path + @"\" + doc_no + @"\" + "Images";
                        if (!Directory.Exists(newPath))
                        {
                            Directory.CreateDirectory(newPath);

                        }
                        /* String after = newPath + @"\Before\";
                         if (!Directory.Exists(after))
                         {
                             Directory.CreateDirectory(after);

                         }*/




                        string base64 = Value;
                        //for (int i = 0; i < base64.Length; i++)
                        {
                            byte[] imageBytes = Convert.FromBase64String(base64);
                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            ms.Write(imageBytes, 0, imageBytes.Length);
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);


                            string finalPath = newPath + @"\" + Guid.NewGuid() + ".jpg";


                            image.Save(finalPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                            pic_path = finalPath;
                            stored = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        message.message = "error";

                        throw ex;
                    }

                    if (stored)
                    {
                        try
                        {

                            // insert into FMCALL_PIC 
                            using (SqlConnection con = new SqlConnection(gfmConString))
                            {
                                using (SqlCommand comm = new SqlCommand("SP_FMSWEB_FMCALL_PIC_SAVE", con))
                                {
                                    con.Open();
                                    comm.CommandType = CommandType.StoredProcedure;
                                    comm.Parameters.AddWithValue("@hid", hid);
                                    comm.Parameters.AddWithValue("@imagetype", "T");
                                    comm.Parameters.AddWithValue("@compcode", compcode);
                                    comm.Parameters.AddWithValue("@doc_no", doc_no);
                                    comm.Parameters.AddWithValue("@path", pic_path);
                                    comm.Parameters.AddWithValue("@TD_REMARKS", remarks);
                                    int rows = Convert.ToInt32(comm.ExecuteScalar());

                                    if (rows != 0)
                                    {
                                        message.message = "Saved";
                                    }
                                    else
                                    {
                                        message.message = "error";
                                    }

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        message.message = "error";

                    }




                }

            }
            else
            {
                message.message = "error";
            }

            return message;

        }

        [WebMethod]
        public Doc_no check(string compcode, string task_id, string call_no, string imagetype)
        {

            SqlConnection con = new SqlConnection(gfmConString);
            DataSet ds = new DataSet();
            Doc_no status_code = new Doc_no();


            try
            {

                if (imagetype.Equals("B"))
                {
                    con.Open();
                    string q = "select TH_DOC_NO from FMCALL_INSP where TH_CALLREFNO =@call_no AND TH_CompCd =@compcode AND TH_TASK_NO = @task_id";
                    SqlCommand comm = new SqlCommand(q, con);
                    comm.Parameters.AddWithValue("@task_id", task_id);
                    comm.Parameters.AddWithValue("@compcode", compcode);
                    comm.Parameters.AddWithValue("@call_no", call_no);

                    SqlDataReader reader = comm.ExecuteReader();

                    while (reader.Read())
                    {

                        if (reader["TH_DOC_NO"] != DBNull.Value)
                        {
                            status_code.doc_no = reader["TH_DOC_NO"].ToString();


                        }



                    }
                }
                else if (imagetype.Equals("A"))
                {
                    con.Open();
                    string q = "select TH_DOC_NO from FMCALL_WIP where TH_CALLREFNO =@call_no AND TH_CompCd =@compcode AND TH_TASK_NO = @task_id";
                    SqlCommand comm = new SqlCommand(q, con);
                    comm.Parameters.AddWithValue("@task_id", task_id);
                    comm.Parameters.AddWithValue("@compcode", compcode);
                    comm.Parameters.AddWithValue("@call_no", call_no);

                    SqlDataReader reader = comm.ExecuteReader();

                    while (reader.Read())
                    {

                        if (reader["TH_DOC_NO"] != DBNull.Value)
                        {
                            status_code.doc_no = reader["TH_DOC_NO"].ToString();


                        }



                    }
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status_code;

        }








        [WebMethod]
        public Doc_no generateMaterialDocNo(string taskNo, string callNo, string compCode)
        {          
            Doc_no doc_no = new Doc_no(); 
            try
            {

                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_INMREQH_GEN_DOCNO", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;                
                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@taskNo", taskNo);
                        comm.Parameters.AddWithValue("@callNo", callNo);

                        SqlDataReader reader = comm.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                doc_no.doc_no = reader[0].ToString();

                            }
                        }
                        else
                        {
                            doc_no.doc_no = "0";
                        }
                       
                    }
                }

            }
            catch (Exception ex)
            {
                doc_no.doc_no = "0";
                throw ex;
            }     
            
            return doc_no;
            
        }


        [WebMethod]
        public Message resetMaterialDocNo(string taskNo, string callNo, string docNo,string compCode)
        {
            Message message = new Message();
            try
            {

                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_INMREQH_Reset_DOCNO", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@taskNo", taskNo);
                        comm.Parameters.AddWithValue("@callNo", callNo);
                        comm.Parameters.AddWithValue("@docNo", docNo);
                        int hid = comm.ExecuteNonQuery();
                        message.message = "updated";


                    }
                }

            }
            catch (Exception ex)
            {
                message.message = "error";
                throw ex;
            }

            return message;

        }


        [DataContract]
        public class Doc_no
        {
            [DataMember]
            public string doc_no { get; set; }

        }



        [WebMethod]
        public StatusCode checkWorkStatus(string compcode, string task_id)
        {

            SqlConnection con = new SqlConnection(gfmConString);
            DataSet ds = new DataSet();
            StatusCode status_code = new StatusCode();


            try
            {
                con.Open();
                string q = "Select TH_STATUS from FMTASKH WHERE TH_COMPCD=@compcode AND TH_TASK_NO=@task_id";
                SqlCommand comm = new SqlCommand(q, con);
                comm.Parameters.AddWithValue("@task_id", task_id);
                comm.Parameters.AddWithValue("@compcode", compcode);
                SqlDataReader reader = comm.ExecuteReader();

                while (reader.Read())
                {

                    if (reader["TH_STATUS"] != DBNull.Value)
                    {
                        status_code.status_code = reader["TH_STATUS"].ToString();


                    }


                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status_code;

        }


        [DataContract]
        public class StatusCode
        {
            [DataMember]
            public string status_code { get; set; }

        }


        [WebMethod]
        public XmlDocument getMaterialData(string docNo, string compCode)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();

                    // get Maintenance Task
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_INMREQH_GET", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@docNo", docNo);                    
                        comm.Parameters.AddWithValue("@flag", 0);
                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                    }



                }

            }
            catch (Exception ex)
            {
                throw ex;
            }



            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());


            return xdoc;
        }


        [WebMethod]
        public XmlDocument getUsedMaterialData(string docNo, string compCode,string callNo,string taskNo)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();

                    // get Maintenance Task
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMCALL_DIR_MATERIAL_GET", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@callNo", callNo);
                        comm.Parameters.AddWithValue("@taskNo", taskNo);
                        comm.Parameters.AddWithValue("@flag", 0);
                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                    }



                }

            }
            catch (Exception ex)
            {
                throw ex;
            }



            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());


            return xdoc;
        }



        [WebMethod]
        public XmlDocument getLocation(string compcode)
        {

            SqlConnection con = new SqlConnection(gfmConString);
            DataSet ds = new DataSet();

            try
            {
                string query = "SELECT LC_CD,LC_DESC FROM dbo.INLOCN WHERE LC_CompCd=@compcode AND LC_ALLOW_MOBILTEAM = 1 ";
                SqlCommand comm = new SqlCommand(query, con);
                comm.Parameters.AddWithValue("@compcode", compcode);
                SqlDataAdapter ad = new SqlDataAdapter(comm);
                ad.Fill(ds);


            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());
            return xdoc;
        }



        [WebMethod]
        public Message saveMaterial(string compCode,string callNo, string task_id, string loc_des, string docNo, string c, string name, string qt, string u, string empcode,string userCode)
        {
            string code = c;
            string p_name = name;
            string qtlist = qt;
            string uom = u;

            Message message = new Message();
            DataSet ds = new DataSet();
            int hid = 0;
            int line = 0;
            ////// query to get Document type 

            using(TransactionScope trans = new TransactionScope())
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(gfmConString))
                    {
                        con.Open();

                        // get Maintenance Task
                        using (SqlCommand comm = new SqlCommand("SP_FMSMOB_INMREQH_SAVE", con))
                        {
                            comm.CommandType = CommandType.StoredProcedure;

                            comm.Parameters.AddWithValue("@compCode", compCode);
                            comm.Parameters.AddWithValue("@docNo", docNo);
                            comm.Parameters.AddWithValue("@callNo", callNo);
                            comm.Parameters.AddWithValue("@taskNo", task_id);
                            comm.Parameters.AddWithValue("@empCode", empcode);
                            comm.Parameters.AddWithValue("@userCode", userCode);
                
                            hid = Convert.ToInt32(comm.ExecuteScalar());



                        }



                    }


                    if(hid != 0)
                    {
                        using (SqlConnection con = new SqlConnection(gfmConString))
                        {
                            con.Open();

                            // get Maintenance Task
                            using (SqlCommand comm = new SqlCommand("SP_FMSMOB_INMREQD_SAVE", con))
                            {
                                comm.CommandType = CommandType.StoredProcedure;

                                comm.Parameters.AddWithValue("@compCode", compCode);
                                comm.Parameters.AddWithValue("@callNo", callNo);
                                comm.Parameters.AddWithValue("@docNo", docNo);
                                comm.Parameters.AddWithValue("@taskNo", task_id);
                                comm.Parameters.AddWithValue("@qty", qtlist);
                                comm.Parameters.AddWithValue("@hId", hid);
                                comm.Parameters.AddWithValue("@code", code);
                                comm.Parameters.AddWithValue("@uom", uom);
                                comm.Parameters.AddWithValue("@name", p_name);

                                hid = Convert.ToInt32(comm.ExecuteScalar());


                                if (hid != 0)
                                {
                                    message.message = "Added";
                                }
                                else
                                {
                                    message.message = "error";
                                }
                            }



                        }
                    }
                    trans.Complete();

                }
                catch (Exception ex)
                {
                    message.message = "error";
                    trans.Dispose();
                    throw ex;
                }
            }  
            ////////// insert into INSIND table



            return message;

        }



        [WebMethod]
        public Message saveUsedMaterial(string compCode, string callNo, string task_id, string c, string name, string qt, string u)
        {
            string code = c;
            string p_name = name;
            string qtlist = qt;
            string uom = u;

            Message message = new Message();
            DataSet ds = new DataSet();
            int hid = 0;
            int line = 0;
            ////// query to get Document type 

            using (TransactionScope trans = new TransactionScope())
            {
                try
                {
                  using (SqlConnection con = new SqlConnection(gfmConString))
                        {
                            con.Open();

                            // get Maintenance Task
                            using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMCALL_DIR_MATERIAL_SAVE", con))
                            {
                                comm.CommandType = CommandType.StoredProcedure;

                                comm.Parameters.AddWithValue("@compCode", compCode);
                                comm.Parameters.AddWithValue("@callNo", callNo);
                                comm.Parameters.AddWithValue("@taskNo", task_id);
                                comm.Parameters.AddWithValue("@qty", qtlist);
                                comm.Parameters.AddWithValue("@code", code);
                                comm.Parameters.AddWithValue("@uom", uom);
                            comm.Parameters.AddWithValue("@name", p_name);

                            hid = Convert.ToInt32(comm.ExecuteScalar());


                                if (hid != 0)
                                {
                                    message.message = "Added";
                                }
                                else
                                {
                                    message.message = "error";
                                }
                            }



                        }
                    trans.Complete();

                }
                catch (Exception ex)
                {
                    message.message = "error";
                    trans.Dispose();
                    throw ex;
                }
            }
            ////////// insert into INSIND table



            return message;

        }



        [DataContract]
        public class Message
        {
            [DataMember]
            public string message { get; set; }
            [DataMember]
            public int value { get; set; }
        }


        [WebMethod]
        public Message saveSafety(string compCode, string taskNo, string callNo, string empCode, string check)
        {

            DataSet ds = new DataSet();
            Helper helpers = new Helper();
            Message message = new Message();
            try
            {
                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMTASK_SAFREQ_SAVE", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@taskNo", taskNo);
                        comm.Parameters.AddWithValue("@callNo", callNo);
                        comm.Parameters.AddWithValue("@empCode", empCode);
                        comm.Parameters.AddWithValue("@check", check);

                        int temp = Convert.ToInt32(comm.ExecuteScalar());

                        if (temp != 0)
                        {
                            message.message = "Saved";
                        }
                        else
                        {
                            message.message = "error";
                        }


                    }

                }

             
            }
            catch (Exception ex)
            {
                message.message = "error";

                throw ex;
            }


            return message;
        }

        [WebMethod]
        public XmlDocument getSafety()
        {

            SqlConnection con = new SqlConnection(gfmConString);
            DataSet ds = new DataSet();
            Message message = new Message();
            try
            {
                con.Open();

                string q = "select * from SAFETY_REQ";
                SqlCommand comm = new SqlCommand(q, con);

                SqlDataAdapter ad = new SqlDataAdapter(comm);
                ad.Fill(ds);
                con.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());


            return xdoc;
        }

        [WebMethod]
        public XmlDocument savedSafety(string compCode, string taskNo, string callNo)
        {

            DataSet ds = new DataSet();
            Message message = new Message();
            try
            {
                // get header id and document no from FMCALL_INSP_DIRECT
                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMTASK_SAFREQ_GET", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@taskNo", taskNo);
                        comm.Parameters.AddWithValue("@callNo", callNo);
                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                        con.Close();


                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());


            return xdoc;
        }


        [WebMethod]
        public Message getImages(string taskNo, string compCode, string callNo,string type)
        {
            DataSet ds = new DataSet();
            Message message = new Message();
            string doc_no= string.Empty;     
            try
            {


                // get header id and document no from FMCALL_INSP_DIRECT
                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMCALL_INSP_DIRECT_GET", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@flag", 0);
                        comm.Parameters.AddWithValue("@compcode", compCode);
                        comm.Parameters.AddWithValue("@callNo", callNo);
                        comm.Parameters.AddWithValue("@taskNo", taskNo);
                        SqlDataReader reader = comm.ExecuteReader();
                        while (reader.Read())
                        {
                            doc_no = reader["TH_DOC_NO"].ToString();

                        }

                    }
                }

                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMCALL_PIC_GET", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@taskNo", taskNo);
                        comm.Parameters.AddWithValue("@type", type);
                        comm.Parameters.AddWithValue("@callNo", callNo);
                        comm.Parameters.AddWithValue("@flag", 0);
                        SqlDataReader reader = comm.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                message.message = reader[0].ToString();

                            }
                            message.message = doc_no+"/Images/"+Path.GetFileName(message.message);

                        }
                        else
                        {
                            message.message = "error";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return message;
        }


        [WebMethod]
        public Message workStart(string task_id, string compcode, string call_no, string workprogresscode)
        {
            Message message = new Message();
            SqlConnection con = new SqlConnection(gfmConString);
            try
            {
                con.Open();
                string query = "UPDATE FMTASKD SET TD_WORKSTART=1  WHERE TD_CALL_NO= @call_no AND TD_COMPCD=@compcode AND TD_TASK_NO=@task_id ";

                SqlCommand comm = new SqlCommand(query, con);
                comm.Parameters.AddWithValue("@task_id", task_id);
                comm.Parameters.AddWithValue("@call_no", call_no);
                comm.Parameters.AddWithValue("@compcode", compcode);
                comm.ExecuteNonQuery();


                con.Close();

                con.Open();

                string query1 = "UPDATE FMTASKH SET TH_STATUS= @workprogresscode  WHERE TH_CALL_NO=@call_no AND TH_COMPCD=@compcode AND TH_TASK_NO=@task_id";
                SqlCommand comm_1 = new SqlCommand(query1, con);
                comm_1.Parameters.AddWithValue("@task_id", task_id);
                comm_1.Parameters.AddWithValue("@workprogresscode", workprogresscode);
                comm_1.Parameters.AddWithValue("@compcode", compcode);
                comm_1.Parameters.AddWithValue("@call_no", call_no);

                int temp = comm_1.ExecuteNonQuery();
                if (temp != 0)
                {
                    message.message = "updated";
                }
                else
                {
                    message.message = "error";
                }

                con.Close();

            }
            catch (Exception ex)
            {
                message.message = "error";

                throw ex;

            }


            return message;
        }



        [WebMethod]
        public XmlDocument getPriority(string compcode)
        {

            SqlConnection con = new SqlConnection(gfmConString);
            DataSet ds = new DataSet();
            try
            {
                con.Open();

                string q1 = "SELECT * FROM dbo.FMPRIOR where PRCOMPCD = @compcode";
                SqlCommand comm = new SqlCommand(q1, con);
                comm.Parameters.AddWithValue("@compcode", compcode);

                SqlDataAdapter ad = new SqlDataAdapter(comm);
                ad.Fill(ds);
                con.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());


            return xdoc;
        }

        [WebMethod]
        public XmlDocument getAllStatus(string compCode, bool isInspection)
        {

            DataSet ds = new DataSet();
            try
            {

                using(SqlConnection con = new SqlConnection(gfmConString))
                {
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMSTATUS_GET", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@isInspection", isInspection);
                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                        con.Close();
                    }
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());


            return xdoc;

        }

        [WebMethod]
        public Message updateStatus(string compCode, string taskNo, string status, string remarks, string callNo, string reading,string userCode)
        {
            Message message = new Message();
            bool updated = false;
            Helper helpers = new Helper();
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMTASKH_UPDATE", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@taskNo", taskNo);
                        comm.Parameters.AddWithValue("@taskStatus", status);
                        comm.Parameters.AddWithValue("@remarks", remarks);
                        comm.Parameters.AddWithValue("@callNo", callNo);
                        comm.Parameters.AddWithValue("@reading", helpers.NDO(reading));
                        comm.Parameters.AddWithValue("@flag", 3);
                        comm.Parameters.AddWithValue("@userCode", userCode);


                        int rows = Convert.ToInt32(comm.ExecuteScalar());
                        message.message = "updated";
                        message.value = rows;
                    }
                }
            }
            catch (Exception)
            {
                message.message = "Error";
                throw;
            }
            return message;
        }



        [WebMethod]
        public statusCode getUpdatedStatus(string compCode, string taskNo, string callNo)
        {
            statusCode code = new statusCode();
            bool updated = false;

            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMTASKH_GET", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@taskNo", taskNo);
                        comm.Parameters.AddWithValue("@callNo", callNo);
                        comm.Parameters.AddWithValue("@flag", 1);
                        SqlDataReader reader = comm.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                code.updatedStatus = reader[0].ToString();
                                code.reading = reader[1].ToString();
                                code.defaultUOM = reader[2].ToString();


                            }
                        }
                        else
                        {
                            code.updatedStatus = "Error";
                            code.reading = "Error";
                            code.defaultUOM = "Error";
                        }
                        
                    }
                }
               
            }
            catch (Exception ex)
            {
                code.updatedStatus = "Error";
                throw ex;
            }
            return code;
        }


        [WebMethod]
        public Message closeTask(string call_no, string compcode, string imagetype, string remarks, string Value, string task_id, string feedback,string userCode)
        {
            DataSet ds = new DataSet();
            string hid = "";
            string doc_no = null;
            bool stored = false;
            string pic_path = "";
            Message message = new Message();
            string path = null;

            using (TransactionScope trans = new TransactionScope())
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(gfmConString))
                    {
                        string query = "select COMP_IMG_PATH from GLCOMPANY where COMPCD = @compcode ";

                        using (SqlCommand com = new SqlCommand(query, con))
                        {
                            con.Open();
                            com.Parameters.AddWithValue("@compcode", compcode);

                            SqlDataReader reader = com.ExecuteReader();
                            while (reader.Read())
                            {
                                path = reader["COMP_IMG_PATH"].ToString();

                            }

                        }
                    }


                    using (SqlConnection con = new SqlConnection(gfmConString))
                    {
                        using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMCALL_INSP_DIRECT_GET", con))
                        {
                            con.Open();
                            comm.CommandType = CommandType.StoredProcedure;
                            comm.Parameters.AddWithValue("@flag", 0);
                            comm.Parameters.AddWithValue("@compcode", compcode);
                            comm.Parameters.AddWithValue("@callNo", call_no);
                            comm.Parameters.AddWithValue("@taskNo", task_id);
                            SqlDataReader reader = comm.ExecuteReader();
                            while (reader.Read())
                            {
                                hid = reader["TH_HID"].ToString();
                                doc_no = reader["TH_DOC_NO"].ToString();

                            }

                        }
                    }



                    if (path != null && doc_no != null)
                    {

                        try
                        {

                            string newPath = path + @"\" + doc_no + @"\" + "Images";
                            if (!Directory.Exists(newPath))
                            {
                                Directory.CreateDirectory(newPath);

                            }
                            /* String after = newPath + @"\Close\";
                             if (!Directory.Exists(after))
                             {
                                 Directory.CreateDirectory(after);

                             }*/

                            byte[] imageBytes = Convert.FromBase64String(Value);

                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            ms.Write(imageBytes, 0, imageBytes.Length);
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                            string finalPath = newPath + @"\" + Guid.NewGuid() + ".jpg";

                            image.Save(finalPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                            pic_path = finalPath;
                            stored = true;

                        }
                        catch (Exception ex)
                        {
                            message.message = "error";

                            throw ex;
                        }

                        if (stored)
                        {
                            try
                            {
                                // insert into FMCALL_PIC 
                                using (SqlConnection con = new SqlConnection(gfmConString))
                                {
                                    using (SqlCommand comm = new SqlCommand("SP_FMSWEB_FMCALL_PIC_SAVE", con))
                                    {
                                        con.Open();
                                        comm.CommandType = CommandType.StoredProcedure;
                                        comm.Parameters.AddWithValue("@hid", hid);
                                        comm.Parameters.AddWithValue("@imagetype", "C");
                                        comm.Parameters.AddWithValue("@compcode", compcode);
                                        comm.Parameters.AddWithValue("@doc_no", doc_no);
                                        comm.Parameters.AddWithValue("@path", pic_path);
                                        comm.Parameters.AddWithValue("@TD_REMARKS", remarks);
                                        int rows = Convert.ToInt32(comm.ExecuteScalar());

                                        if (rows != 0)
                                        {
                                            message.message = "Saved";
                                        }
                                        else
                                        {
                                            message.message = "error";
                                        }

                                    }
                                }
                                // update task status and TH_THROUGH_MOB_YN in FMCALL_INSP_DIRECT table
                                using (SqlConnection con = new SqlConnection(gfmConString))
                                {
                                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMTASKH_UPDATE", con))
                                    {
                                        con.Open();
                                        comm.CommandType = CommandType.StoredProcedure;
                                        comm.Parameters.AddWithValue("@flag", 0);
                                        comm.Parameters.AddWithValue("@compcode", compcode);
                                        comm.Parameters.AddWithValue("@callNo", call_no);
                                        comm.Parameters.AddWithValue("@taskNo", task_id);                                        
                                        comm.Parameters.AddWithValue("@userCode", userCode);

                                        int rows = comm.ExecuteNonQuery();
                                        if (rows != 0)
                                        {
                                            message.message = "Saved";
                                        }
                                        else
                                        {
                                            message.message = "error";
                                        }

                                    }
                                }
                                List<string> feedbackList = new List<string>();

                                if (feedback != null)
                                {
                                    string[] str = feedback.Split(',');
                                    feedbackList = new List<string>(str);
                                }

                                List<CommonModel> tempList = new List<CommonModel>();
                                tempList.Add(new CommonModel()
                                {
                                    code = "QL",
                                    status = feedbackList[0]

                                });
                                tempList.Add(new CommonModel()
                                {
                                    code = "SP",
                                    status = feedbackList[1]

                                });
                                tempList.Add(new CommonModel()
                                {
                                    code = "RS",
                                    status = feedbackList[2]

                                });
                                tempList.Add(new CommonModel()
                                {
                                    code = "AT",
                                    status = feedbackList[3]

                                });

                                DataTable feedBackTable = new DataTable();
                                feedBackTable.Columns.Add("code", typeof(string));
                                feedBackTable.Columns.Add("status", typeof(string));
                                feedBackTable.Columns.Add("compCode", typeof(string));
                                feedBackTable.Columns.Add("hId", typeof(int));
                                foreach (var item in tempList)
                                {

                                    DataRow row = feedBackTable.NewRow();
                                    row["code"] = item.code;
                                    row["status"] = item.status;
                                    row["compCode"] = compcode;
                                    row["hId"] = hid;                                   
                                    feedBackTable.Rows.Add(row);
                                }
                                using (SqlConnection con = new SqlConnection(gfmConString))
                                {
                                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMTASKCUSTFDB_SAVE", con))
                                    {
                                        con.Open();
                                        comm.CommandType = CommandType.StoredProcedure;
                                        comm.Parameters.AddWithValue("@feedback", feedBackTable);
                                        int rows = comm.ExecuteNonQuery();
                                        if (rows != 0)
                                        {                                         

                                            message.message = "Saved";
                                        }
                                        else
                                        {
                                            message.message = "Error";

                                        }
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                message.message = "error";

                                throw ex;
                            }
                        }
                        else
                        {
                            message.message = "error";

                        }



                    }
                    else
                    {
                        message.message = "error";
                    }

                    trans.Complete();

                }
                catch (Exception ex)
                {
                    message.message = "error";
                    trans.Dispose();

                    throw ex;

                }
               
            }
           

            return message;

        }



        //[WebMethod]
        //public Message saveFeedback(string compcode, string task_id, string feedback_code, string call_no)
        //{
        //    Message message = new Message();
        //    bool updated = false;
        //    DataSet ds = new DataSet();
        //    try
        //    {

        //        using (SqlConnection con = new SqlConnection(gfmConString))
        //        {
        //            using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMTASKH_UPDATE", con))
        //            {
        //                con.Open();
        //                comm.CommandType = CommandType.StoredProcedure;
        //                comm.Parameters.AddWithValue("@probCode", probCode);
        //                comm.Parameters.AddWithValue("@causeCode", causeCode);
        //                comm.Parameters.AddWithValue("@solutionCode", solutionCode);
        //                comm.Parameters.AddWithValue("@compCode", compCode);
        //                comm.Parameters.AddWithValue("@taskNo", taskNo);
        //                comm.Parameters.AddWithValue("@callNo", callNo);
        //                comm.Parameters.AddWithValue("@flag", 2);

        //                int rows = comm.ExecuteNonQuery();
        //                if (rows != 0)
        //                {
        //                    updated = true;

        //                    message.message = "updated";
        //                }
        //                else
        //                {
        //                    message.message = "Error";

        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        message.message = "Error";
        //        throw ex;
        //    }
        //    return message;

        //}



        [WebMethod]
        public XmlDocument getPPMWorkOrderList(string compcode, string work_order_no)
        {

            string wo_id = null;
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();

                    string query = "select * from FMWORKORDERH where TH_WO_NO =@orderno and TH_COMPCD =@compcode";
                    using (SqlCommand comm = new SqlCommand(query, con))
                    {

                        comm.Parameters.AddWithValue("@orderno", work_order_no);
                        comm.Parameters.AddWithValue("compcode", compcode);
                        SqlDataReader reader = comm.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["TH_WO_ID"] != DBNull.Value)
                            {
                                wo_id = reader["TH_WO_ID"].ToString();

                            }
                        }

                    }
                    con.Close();

                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            //check whether ppm document no exist or not 
            if (wo_id != null)
            {
                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();

                    string query = "select * from FMWOTASK where TH_WO_ID=@id and TSKD_COMPCD =@compcode";
                    using (SqlCommand comm = new SqlCommand(query, con))
                    {
                        comm.Parameters.AddWithValue("@id", wo_id);
                        comm.Parameters.AddWithValue("@compcode", compcode);
                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);

                    }
                    con.Close();

                }
            }




            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());

            return xdoc;



        }



        [WebMethod]
        public Message savePPMWorkOrder(string compCode, int woId)
        {
            int i = 0;
            Message message = new Message();

            using (SqlConnection con = new SqlConnection(gfmConString))
            {
                con.Open();

                using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMWOTASK_UPDATE", con))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@compCode", compCode);
                    comm.Parameters.AddWithValue("@woId", woId);
                    i = comm.ExecuteNonQuery();
                }
            }
            if (i > 0)
            {
                message.message = "saved";
            }
            else
            {
                message.message = "error";
            }


            return message;
        }

        [WebMethod]
        public string getMobileId(string empcode, string compcode)
        {
            int i = 0;
            string rid = "";
            using (SqlConnection con = new SqlConnection(loginConString))
            {
                con.Open();

                string query = "select MOBILE_RID from GLPUSHNOTIFICATION where EMP_CODE=@empcode and COMP_CODE =@compcode ";
                using (SqlCommand comm = new SqlCommand(query, con))
                {
                    comm.Parameters.AddWithValue("@compcode", compcode);
                    comm.Parameters.AddWithValue("@empcode", empcode);
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        rid = reader[0].ToString();
                    }


                }
                con.Close();

            }


            return rid;
        }


        [WebMethod]
        public XmlDocument getProblemList(string scope, string compCode)
        {
            string message = "";
            Helper helpers = new Helper();

            List<taskListItems> test = new List<taskListItems>();
            DataSet ds = new DataSet();
            try
            {


                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();

                    // get Maintenance Task
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMTASKM_PROBLEM_GET", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@scope", helpers.convertToP(scope));

                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                    }



                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());
            return xdoc;

        }

        [WebMethod]
        public XmlDocument getCauseList(string probCode, string compCode)
        {
            string message = "";
            Helper helpers = new Helper();

            List<taskListItems> test = new List<taskListItems>();
            DataSet ds = new DataSet();
            try
            {


                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();

                    // get Maintenance Task
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMTASKM_ROOTCAUSE_GET", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@probCode", helpers.convertToP(probCode));

                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                    }



                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());
            return xdoc;

        }

        [WebMethod]
        public XmlDocument getSolutionList(string scope, string compCode)
        {
            string message = "";
            List<taskListItems> test = new List<taskListItems>();
            DataSet ds = new DataSet();
            try
            {


                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();

                    // get Maintenance Task
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMTASKM_SOLUTION_GET", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@compCode", compCode);

                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                    }



                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());
            return xdoc;

        }

        [WebMethod]
        public Message updatePRS(string probCode, string causeCode, string solutionCode, string compCode, string callNo, string taskNo)
        {
            Message message = new Message();
            bool updated = false;
            DataSet ds = new DataSet();
            try
            {

                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMTASKH_UPDATE", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@probCode", probCode);
                        comm.Parameters.AddWithValue("@causeCode", causeCode);
                        comm.Parameters.AddWithValue("@solutionCode", solutionCode);
                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@taskNo", taskNo);
                        comm.Parameters.AddWithValue("@callNo", callNo);
                        comm.Parameters.AddWithValue("@flag", 2);


                        int rows = comm.ExecuteNonQuery();
                        if (rows != 0)
                        {
                            updated = true;

                            message.message = "updated";
                        }
                        else
                        {
                            message.message = "Error";

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                message.message = "Error";
                throw ex;
            }
            return message;
        }

        [WebMethod]
        public CommonModel getSavedPRS(string compCode, string callNo, string taskNo)
        {
            CommonModel model = new CommonModel();
            bool updated = false;
            DataSet ds = new DataSet();
            try
            {

                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMTASKH_GET", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@taskNo", taskNo);
                        comm.Parameters.AddWithValue("@callNo", callNo);
                        comm.Parameters.AddWithValue("@flag", 0);

                        SqlDataReader reader = comm.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                model.probCode = reader[0].ToString();
                                model.causeCode = reader[1].ToString();
                                model.solutionCode = reader[2].ToString();

                                model.probDesc = reader[3].ToString();
                                model.causeDesc = reader[4].ToString();
                                model.solutionDesc = reader[5].ToString();

                            }
                        }
                        else
                        {
                            model.probCode = null;
                            model.causeCode = null;
                            model.solutionCode = null;

                            model.probDesc = null;
                            model.causeDesc = null;
                            model.solutionDesc = null;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }


        [WebMethod]
        public Message updateOR(string taskNo, string callNo, string observation, string reason, string compCode)
        {
            Message message = new Message();
            bool updated = false;
            DataSet ds = new DataSet();
            try
            {

                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMCALL_INSP_DIRECT_UPDATE", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@observation", observation);
                        comm.Parameters.AddWithValue("@reason", reason);
                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@taskNo", taskNo);
                        comm.Parameters.AddWithValue("@callNo", callNo);
                        comm.Parameters.AddWithValue("@flag", 0);
                        int rows = comm.ExecuteNonQuery();
                        if (rows != 0)
                        {
                            updated = true;

                            message.message = "updated";
                        }
                        else
                        {
                            message.message = "Error";

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                message.message = "Error";
                throw ex;
            }
            return message;


        }

        [WebMethod]
        public CommonModel getSavedOR(string compCode, string callNo, string taskNo)
        {
            CommonModel model = new CommonModel();
            bool updated = false;
            DataSet ds = new DataSet();
            try
            {

                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMCALL_INSP_DIRECT_GET", con))
                    {
                        con.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@taskNo", taskNo);
                        comm.Parameters.AddWithValue("@callNo", callNo);
                        comm.Parameters.AddWithValue("@flag", 1);

                        SqlDataReader reader = comm.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                model.observation = reader[0].ToString();
                                model.reason = reader[1].ToString();


                            }
                        }
                        else
                        {
                            model.observation = null;
                            model.reason = null;

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }


        [WebMethod]
        public XmlDocument getTaskLocation(string compCode)
        {
            string message = "";
            Helper helpers = new Helper();

            DataSet ds = new DataSet();
            try
            {


                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();

                    // get Maintenance Task
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GET_Loc_Build_Unit_Cont", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@flag", 0);

                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                    }



                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());
            return xdoc;
        }

        [WebMethod]
        public XmlDocument getBuilding(string compCode, string locationCode)
        {
            string message = "";
            Helper helpers = new Helper();

            DataSet ds = new DataSet();
            try
            {


                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();

                    // get Maintenance Task
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GET_Loc_Build_Unit_Cont", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@locationCode", locationCode);
                        comm.Parameters.AddWithValue("@flag", 1);

                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                    }



                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());
            return xdoc;
        }

        [WebMethod]
        public XmlDocument getUnit(string compCode, string locationCode, string buildingNo)
        {
            string message = "";
            Helper helpers = new Helper();

            DataSet ds = new DataSet();
            try
            {


                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();

                    // get Maintenance Task
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GET_Loc_Build_Unit_Cont", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@locationCode", locationCode);
                        comm.Parameters.AddWithValue("@buildingNo", buildingNo);
                        comm.Parameters.AddWithValue("@flag", 2);

                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                    }



                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());
            return xdoc;
        }

        [WebMethod]
        public XmlDocument getContract(string compCode, string buildingNo, string locationCode)
        {
            string message = "";
            Helper helpers = new Helper();

            DataSet ds = new DataSet();
            try
            {


                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();

                    // get Maintenance Task
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GET_Loc_Build_Unit_Cont", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@locationCode", locationCode);
                        comm.Parameters.AddWithValue("@buildingNo", buildingNo);
                        comm.Parameters.AddWithValue("@flag", 2);

                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                    }



                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());
            return xdoc;
        }


        [WebMethod]
        public XmlDocument getScope(string compCode, string locationCode, string buildingNo)
        {
            string message = "";
            Helper helpers = new Helper();

            DataSet ds = new DataSet();
            try
            {


                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();

                    // get Maintenance Task
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GET_Loc_Build_Unit_Cont", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@locationCode", locationCode);
                        comm.Parameters.AddWithValue("@buildingNo", buildingNo);
                        comm.Parameters.AddWithValue("@flag", 2);

                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                    }



                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());
            return xdoc;
        }

        [WebMethod]
        public XmlDocument getAsset(string compCode, string buildingNo, string locationCode)
        {
            string message = "";
            Helper helpers = new Helper();

            DataSet ds = new DataSet();
            try
            {


                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();

                    // get Maintenance Task
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_GET_Loc_Build_Unit_Cont", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@compCode", compCode);
                        comm.Parameters.AddWithValue("@locationCode", locationCode);
                        comm.Parameters.AddWithValue("@buildingNo", buildingNo);
                        comm.Parameters.AddWithValue("@flag", 2);

                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                    }



                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());
            return xdoc;
        }


        [WebMethod]
        public XmlDocument getUnitDetailsByQRCode(string qrCode)
        {
            string message = "";
            Helper helpers = new Helper();

            List<taskListItems> test = new List<taskListItems>();
            DataSet ds = new DataSet();
            try
            {


                using (SqlConnection con = new SqlConnection(gfmConString))
                {
                    con.Open();

                    // get Maintenance Task
                    using (SqlCommand comm = new SqlCommand("SP_FMSMOB_FMTASKM_PROBLEM_GET", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.AddWithValue("@compCode", qrCode);

                        SqlDataAdapter ad = new SqlDataAdapter(comm);
                        ad.Fill(ds);
                    }



                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ds.GetXml());
            return xdoc;
        }

    }

    [DataContract]
    public class CommonModel
    {
        [DataMember]
        public string probCode { get; set; }

        [DataMember]
        public string probDesc { get; set; }

        [DataMember]
        public string causeCode { get; set; }

        [DataMember]
        public string causeDesc { get; set; }

        [DataMember]
        public string solutionCode { get; set; }

        [DataMember]
        public string solutionDesc { get; set; }

        [DataMember]
        public string observation { get; set; }
        [DataMember]
        public string reason { get; set; }

        [DataMember]
        public string code { get; set; }

        [DataMember]
        public string description { get; set; }
        
        [DataMember]
        public string status { get; set; }


        [DataMember]
        public int regularCount { get; set; }

        [DataMember]
        public int PPMCount { get; set; }

        [DataMember]
        public int InspectionCount { get; set; }
        [DataMember]
        public int completedCount { get; set; }
        [DataMember]
        public int taskCount { get; set; }




    }
}
