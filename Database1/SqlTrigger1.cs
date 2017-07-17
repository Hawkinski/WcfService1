using Microsoft.SqlServer.Server;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;


public partial class Triggers
{

    // Enter existing table or view for the target and uncomment the attribute line
    // [Microsoft.SqlServer.Server.SqlTrigger (Name="SqlTrigger1", Target="Table1", Event="FOR UPDATE")]
    public static void SqlTrigger1()
    {
        // Replace with your own code
        SqlTriggerContext context = SqlContext.TriggerContext;
        // SqlConnection conn = new SqlConnection("Data Source=DEMO-LAP3;Initial Catalog=HRZGFM_MOB_NET_01;Integrated Security=False;User ID=sa;Password=KL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        SqlConnection conn = new SqlConnection(" context connection =true ");

        conn.Open();

        SqlCommand comm = conn.CreateCommand();
        SqlPipe pipe = SqlContext.Pipe;
        SqlDataReader dr;

        comm.CommandText = "Select TD_TASK_NO,TD_CALL_NO,TD_EMP,TD_COMPCD from INSERTED";
        dr = comm.ExecuteReader();
        while (dr.Read())
        {

            if (dr[0] != DBNull.Value)
            {
                pipe.Send((string)dr[0] + " , " + (string)dr[1] + " , " + (string)dr[2] + " ," + (string)dr[3]);
                getMobileId();


            }

        }
        SendNotification();

    }



    public string getMobileId()
    {

    }

    public static AndroidFCMPushNotificationStatus SendNotification()
    {
        AndroidFCMPushNotificationStatus result = new AndroidFCMPushNotificationStatus();
        string serverApiKey = "AIzaSyCFlMKvqgNdugTlySOgJHSBD34pHwSjzFE";
        string senderId = "243310446245";
        string deviceId = "dcbpC_fLx8g:APA91bGBaIsREc1xylL6PLfk2s_cLVv2n09any5uN5Ev_WfXp0ZcIj97DdRFHTsyJksvmZHQ5Bfzkw9sPstZea2tYSPWiiLXnV0cp3l1QjEybm-E49KjWNiQ5jhQwPvLg3eCJzUlhzKP";

        try
        {
            result.Successful = false;
            result.Error = null;

            var value = "You have one new task";
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            tRequest.Headers.Add(string.Format("Authorization: key={0}", serverApiKey));
            tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

            string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + value + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + deviceId + "";

            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;

            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);

                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String sResponseFromServer = tReader.ReadToEnd();
                            result.Response = sResponseFromServer;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            result.Successful = false;
            result.Response = null;
            result.Error = ex;
        }

        return result;
    }


    public class AndroidFCMPushNotificationStatus
    {
        public bool Successful
        {
            get;
            set;
        }

        public string Response
        {
            get;
            set;
        }
        public Exception Error
        {
            get;
            set;
        }
    }

}


