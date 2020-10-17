using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
namespace Cpanel.Exams
{
    public partial class ExamCam_Sim : System.Web.UI.Page
    {
        public string UserId
        {
            get
            {
                if (ViewState["UserId"] == null)
                    ViewState["UserId"] = string.Empty;
                return ViewState["UserId"].ToString();
            }
            set
            {
                ViewState["UserId"] = value;
            }
        }
        public string UserExamID
        {
            get
            {
                if (ViewState["UserExamID"] == null)
                    ViewState["UserExamID"] = string.Empty;
                return ViewState["UserExamID"].ToString();
            }
            set
            {
                ViewState["UserExamID"] = value;
            }
        }
        public string CrntExamStatus
        {
            get
            {
                if (ViewState["CrntExamStatus"] == null)
                    ViewState["CrntExamStatus"] = string.Empty;
                return ViewState["CrntExamStatus"].ToString();
            }
            set
            {
                ViewState["CrntExamStatus"] = value;
            }
        }
        public string ScheduleId
        {
            get
            {
                if (ViewState["ScheduleId"] == null)
                    ViewState["ScheduleId"] = string.Empty;
                return ViewState["ScheduleId"].ToString();
            }
            set
            {
                ViewState["ScheduleId"] = value;
            }
        }
        public string CourseId
        {
            get
            {
                if (ViewState["CourseId"] == null)
                    ViewState["CourseId"] = string.Empty;
                return ViewState["CourseId"].ToString();
            }
            set
            {
                ViewState["CourseId"] = value;
            }
        }
        public string OrderId
        {
            get
            {
                if (ViewState["OrderId"] == null)
                    ViewState["OrderId"] = string.Empty;
                return ViewState["OrderId"].ToString();
            }
            set
            {
                ViewState["OrderId"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string strlog = "";
            try
            {
                //------------------
                //Session["B_UserId"] = "d3ec3a0e-d60f-4e40-9e4f-ae7e072225bf";
                //Session["B_UserName"] = "mbakr_2005@yahoo.com";

                //Session["B_UserId"] = "e6ff2ed1-133a-4ed8-a4cf-f7e39350b8b4";
                //Session["B_UserName"] = "Mbakr_2016@yahoo.com";

                if (Session["B_UserId"] == null)
                {
                    return;
                }
                if (Session["B_UserName"] == null)
                {
                    return;
                }
                // strlog += " 1- if (Session[\"CAM_UserExamID\"] == null) <br/>";
                // Logging(strlog);
                if (Session["CAM_UserExamID"] == null)
                    return;
                UserId = Session["B_UserId"].ToString();
                Session["UserId"] = UserId;
                UserExamID = Session["CAM_UserExamID"].ToString();
                //----------
                string sql = "SET DATEFORMAT dmy; Select * from B_UserExam_Sim where UserExamID=@UserExamID;";
                List<SqlParameter> spl = new List<SqlParameter>();
                spl.Add(new SqlParameter("UserExamID", UserExamID));
                DataTable B_UserExam_Sim = DBManager.PopulateList(sql, spl);
                sql = "";
                spl.Clear();

                // ------ Get course,Exam data (exam time, # of questions) --------
                // strlog += " 2- if (B_UserExam_Sim.Rows.Count == 1) = "+ B_UserExam_Sim.Rows.Count + "<br/>";
                // Logging(strlog);
                if (B_UserExam_Sim.Rows.Count == 1)
                {

                    CrntExamStatus = B_UserExam_Sim.Rows[0]["ExamStatus"].ToString();
                    // strlog += " 2.1- CrntExamStatus = " + CrntExamStatus + "<br/>";
                    // Logging(strlog);
                    if (CrntExamStatus == "Reserved" || CrntExamStatus == "Finished")
                        return;
                    sql = "select top(1) CaptureDateTime from B_UserExamImages_Sim where UserExamID=@UserExamID order by CaptureDateTime";
                    spl.Add(new SqlParameter("UserExamID", UserExamID));
                    DataTable tbl00 = DBManager.PopulateList(sql, spl);
                    // strlog += " 2.2- if (tbl00.Rows.Count > 0) = " + tbl00.Rows.Count + "<br/>";
                    // Logging(strlog);
                    if (tbl00.Rows.Count > 0)
                    {
                        if (Session["FirstImageTime"] == null)
                            Session["FirstImageTime"] = DateTime.Now;
                        DateTime FirstImageTime = (DateTime)Session["FirstImageTime"];
                        // strlog += " 2.3 Now= "+DateTime.Now.ToString()+ " if (DateTime.Now > FirstImageTime.AddHours(2)) " + FirstImageTime.AddHours(2).ToString() + "<br/>";
                        // Logging(strlog);

                        if (DateTime.Now > FirstImageTime.AddHours(2))
                            return;
                    }
                    // strlog += " 3- if (Request.InputStream.Length > 0) = "+ Request.InputStream.Length + "<br/>";
                    // Logging(strlog);
                    if (Request.InputStream.Length > 0)
                    {
                        using (StreamReader reader = new StreamReader(Request.InputStream))
                        {
                            // strlog += " 4- using (StreamReader reader = new StreamReader(Request.InputStream)) <br/>";
                            // Logging(strlog);
                            string hexString = Server.UrlEncode(reader.ReadToEnd());
                            string imageName = ASPHelper.GetUniqueDateTimeStamp();
                            string imagePath = string.Format("~/Exams/Captures/{0}.png", imageName);
                            File.WriteAllBytes(Server.MapPath(imagePath), ConvertHexToBytes(hexString));
                            Session["CapturedImage"] = ResolveUrl(imagePath);

                            //--- save image to table B_UserExamImages ------
                            sql = "SET DATEFORMAT dmy; insert into B_UserExamImages_Sim(UserExamID,ImageURL,CaptureDateTime,isCertificateImg) values(@UserExamID,@ImageURL,GetDate(),@isCertificateImg); SELECT SCOPE_IDENTITY() AS UExamImageId";
                            spl.Clear();
                            spl.Add(new SqlParameter("UserExamID", UserExamID));
                            spl.Add(new SqlParameter("ImageURL", imagePath));
                            spl.Add(new SqlParameter("isCertificateImg", false));

                            DataTable tbl0 = DBManager.PopulateList(sql, spl);
                            // strlog += " 5- if (tbl0.Rows.Count==1) <br/>";
                            // Logging(strlog);
                            if (tbl0.Rows.Count == 1)
                            {
                                Session["LastUExamImageId"] = tbl0.Rows[0][0].ToString();
                                Session["crntUserExamID"] = UserExamID;
                            }

                        }// using
                    }// if
                }
            }
            catch (Exception ex)
            {
                // strlog += " 6- "+ ex.Message+"<br/>";
                // Logging(strlog);
            }


        }
        private void Logging(string strlog)
        {
            try
            {
                string sql_log = "insert into [log]([Date],[Exception],[Level],[Logger],[Machine],[Message],[Thread]) values(@DateNow,@msg,'Level','Logger','Machine',@msg,'Thread')";
                List<SqlParameter> spl_log = new List<SqlParameter>();
                spl_log.Add(new SqlParameter("DateNow", DateTime.Now));
                spl_log.Add(new SqlParameter("msg", strlog));
                int xlog = DBManager.ExecuteTransactionQuery(sql_log, spl_log, false);
            }
            catch { }
        }

        private static byte[] ConvertHexToBytes(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        [WebMethod(EnableSession = true)]
        public static string GetAutoCapturedImage()
        {
            try
            {
                //string url = HttpContext.Current.Session["CapturedImage"].ToString();
                string url = "";
                HttpContext.Current.Session["CapturedImage"] = null;
                return url;
            }
            catch
            { }
            return "";
        }

        [WebMethod(EnableSession = true)]
        public static string GetUserCapturedImage()
        {
            try
            {
                string url = HttpContext.Current.Session["CapturedImage"].ToString();
                if (HttpContext.Current.Session["LastUExamImageId"] != null)
                {
                    string LastUExamImageId = HttpContext.Current.Session["LastUExamImageId"].ToString();
                    if (!string.IsNullOrEmpty(LastUExamImageId))
                    {
                        string crntUserExamID = HttpContext.Current.Session["crntUserExamID"].ToString();
                        string sql = "Update B_UserExamImages_Sim set isCertificateImg=@NotCertificateImg where UserExamID=@UserExamID; ;Update B_UserExamImages_Sim set isCertificateImg=@isCertificateImg where UExamImageId=@UExamImageId; ";
                        List<SqlParameter> spl = new List<SqlParameter>();
                        spl.Add(new SqlParameter("NotCertificateImg", false));
                        spl.Add(new SqlParameter("isCertificateImg", true));
                        spl.Add(new SqlParameter("UserExamID", crntUserExamID));
                        spl.Add(new SqlParameter("UExamImageId", LastUExamImageId));
                        int x = DBManager.ExecuteTransactionQuery(sql, spl, false);
                    }
                }
                HttpContext.Current.Session["CapturedImage"] = null;
                return url;
            }
            catch
            { }
            return "";
        }
    }
}