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
    public partial class ExamCam : System.Web.UI.Page
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
                try
                {
                    ScheduleId = "0";
                    CourseId = Session["CAM_CourseId"].ToString();
                    OrderId = Session["CAM_OrderId"].ToString();
                    ScheduleId = Session["CAM_ScheduleId"].ToString();
                }
                catch (Exception eexx)
                {
                    strlog = " 1- eexx: " + eexx.Message + " <br/>";
                    Logging(strlog);
                }
                //------------------
                //Session["B_UserId"] = "d3ec3a0e-d60f-4e40-9e4f-ae7e072225bf";
                //Session["B_UserName"] = "mbakr_2005@yahoo.com";

                //Session["B_UserId"] = "e6ff2ed1-133a-4ed8-a4cf-f7e39350b8b4";
                //Session["B_UserName"] = "Mbakr_2016@yahoo.com";

                strlog = " 2- if (Session[\"B_UserId\"] == null) < br/>";
                Logging(strlog);
                if (Session["B_UserId"] == null)
                {
                    return;
                }
                strlog = " 3- if (Session[\"B_UserName\"] == null) < br/>";
                Logging(strlog);
                if (Session["B_UserName"] == null)
                {
                    return;
                }
                UserId = Session["B_UserId"].ToString();
                Session["UserId"] = UserId;
                //----------
                string sql = "SET DATEFORMAT dmy; Select * from view_UserCoursesExams where ScheduleId=@ScheduleId and OrderId=@OrderId and CourseId=@CourseId and UserId=@UserId and CONVERT(date, FromDateTime)=CONVERT(date,GETDATE());";
                List<SqlParameter> spl = new List<SqlParameter>();
                spl.Add(new SqlParameter("UserId", UserId));
                spl.Add(new SqlParameter("ScheduleId", ScheduleId));
                spl.Add(new SqlParameter("CourseId", CourseId));
                spl.Add(new SqlParameter("OrderId", OrderId));
                strlog = " 4- if (ScheduleId==0) = " + ScheduleId + "< br/>";
                Logging(strlog);
                if (ScheduleId == "0")
                {
                    sql = "SET DATEFORMAT dmy; Select * from B_UserExam where ScheduleId=0 and OrderId=@OrderId and CourseId=@CourseId and UserId=@UserId and CONVERT(date, ReservationDateTime)=CONVERT(date,GETDATE());";
                }
                DataTable tbl_UserExamSchedule = DBManager.PopulateList(sql, spl);
                sql = "";
                spl.Clear();

                // ------ Get course,Exam data (exam time, # of questions) --------
                strlog = " 5- if (tbl_UserExamSchedule.Rows.Count == 1) , " + tbl_UserExamSchedule.Rows.Count + "< br/>";
                Logging(strlog);
                if (tbl_UserExamSchedule.Rows.Count == 1)
                {
                    UserExamID = tbl_UserExamSchedule.Rows[0]["UserExamID"].ToString();
                    CrntExamStatus = tbl_UserExamSchedule.Rows[0]["ExamStatus"].ToString();
                    strlog = " 6- CrntExamStatus " + CrntExamStatus + "< br/>";
                    Logging(strlog);
                    if (CrntExamStatus == "Reserved" || CrntExamStatus == "FinishedMCQ" || CrntExamStatus == "Finished")
                        return;
                    strlog = " 7- if (ScheduleId != 0) " + ScheduleId + "< br/>";
                    Logging(strlog);
                    if (ScheduleId != "0")
                    {
                        DateTime FromDateTime = (DateTime)tbl_UserExamSchedule.Rows[0]["FromDateTime"];
                        DateTime ToDateTime = (DateTime)tbl_UserExamSchedule.Rows[0]["ToDateTime"];
                        strlog = " 8- if (DateTime.Now > ToDateTime.AddMinutes(30)) " + ToDateTime.AddMinutes(30).ToString() + "< br/>";
                        Logging(strlog);
                        if (DateTime.Now > ToDateTime.AddMinutes(30))
                            return;
                    }
                    strlog = " 9- if (Request.InputStream.Length > 0) " + Request.InputStream.Length + "< br/>";
                    Logging(strlog);
                    if (Request.InputStream.Length > 0)
                    {
                        using (StreamReader reader = new StreamReader(Request.InputStream))
                        {
                            string hexString = Server.UrlEncode(reader.ReadToEnd());
                            string imageName = ASPHelper.GetUniqueDateTimeStamp();
                            string imagePath = string.Format("~/Exams/Captures/{0}.png", imageName);
                            File.WriteAllBytes(Server.MapPath(imagePath), ConvertHexToBytes(hexString));
                            Session["CapturedImage"] = ResolveUrl(imagePath);
                            strlog = " 9.1 - imagePath " + imagePath + "< br/>";
                            Logging(strlog);
                            //--- save image to table B_UserExamImages ------
                            sql = "SET DATEFORMAT dmy; insert into B_UserExamImages(UserExamID,ImageURL,CaptureDateTime,isCertificateImg) values(@UserExamID,@ImageURL,GetDate(),@isCertificateImg); SELECT SCOPE_IDENTITY() AS UExamImageId";
                            spl.Clear();
                            spl.Add(new SqlParameter("UserExamID", UserExamID));
                            spl.Add(new SqlParameter("ImageURL", imagePath));
                            spl.Add(new SqlParameter("isCertificateImg", false));

                            DataTable tbl0 = DBManager.PopulateList(sql, spl);
                            strlog = " 10- if (tbl0.Rows.Count==1)" + tbl0.Rows.Count + "< br/>";
                            Logging(strlog);
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
                strlog = " 11- ex= " + ex.Message + "< br/>";
                Logging(strlog);
            }
        }

        private static void Logging(string strlog)
        {
            try
            {
                string username = HttpContext.Current.Session["B_UserName"].ToString();
                strlog = "[ " + username + " ] " + strlog;
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
            string strlog = "";
            try
            {
                strlog = "80- GetAutoCapturedImage() called < br/>";
                Logging(strlog);
                //string url = HttpContext.Current.Session["CapturedImage"].ToString();
                string url = "";
                if (HttpContext.Current.Session["CapturedImage"] != null)
                    url = HttpContext.Current.Session["CapturedImage"].ToString();
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
            string strlog = "";
            try
            {
                string url = HttpContext.Current.Session["CapturedImage"].ToString();
                strlog = " 70- url " + url + "< br/>";
                Logging(strlog);

                if (HttpContext.Current.Session["LastUExamImageId"] != null)
                {
                    strlog = "71- if (HttpContext.Current.Session[LastUExamImageId] != null) , " + HttpContext.Current.Session["LastUExamImageId"].ToString() + "< br/>";
                    Logging(strlog);

                    string LastUExamImageId = HttpContext.Current.Session["LastUExamImageId"].ToString();
                    if (!string.IsNullOrEmpty(LastUExamImageId))
                    {
                        string crntUserExamID = HttpContext.Current.Session["crntUserExamID"].ToString();
                        string sql = "Update B_UserExamImages set isCertificateImg=@NotCertificateImg where UserExamID=@UserExamID; Update B_UserExamImages set isCertificateImg=@isCertificateImg where UExamImageId=@UExamImageId; ";
                        List<SqlParameter> spl = new List<SqlParameter>();
                        spl.Add(new SqlParameter("NotCertificateImg", false));
                        spl.Add(new SqlParameter("isCertificateImg", true));
                        spl.Add(new SqlParameter("UserExamID", crntUserExamID));
                        spl.Add(new SqlParameter("UExamImageId", LastUExamImageId));
                        int x = DBManager.ExecuteTransactionQuery(sql, spl, false);
                        strlog = "72- x , " + x + "< br/>";
                        Logging(strlog);
                    }
                }
                HttpContext.Current.Session["CapturedImage"] = null;
                strlog = "73- return url , " + url + "< br/>";
                Logging(strlog);
                return url;
            }
            catch (Exception ex)
            {
                strlog = "74- ex , " + ex.Message + "< br/>";
                Logging(strlog);
            }
            return "";
        }
    }
}