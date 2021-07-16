using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;

namespace Audire_Admin
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class AudireAdminService : IAudireAdminService
    {
        public CompanyDetail GetServiceURL(Paramerters param)
        {
            CompanyDetail obj = new CompanyDetail();
            obj.Company_code = param.company_code;
            try
            {
                obj = GetServiceURLFromDB(obj);
            }
            catch (Exception ex)
            {
                ErrorLog(ex);
            }

            return obj;
        }

        private CompanyDetail GetServiceURLFromDB(CompanyDetail param)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["csAudire"].ToString());
            SqlCommand command;
            CompanyDetail obj = new CompanyDetail();
            try
            {

                JavaScriptSerializer ser = new JavaScriptSerializer();
                string json = ser.Serialize(param);
                TestLog(json, "GetTasksFromDB");

                command = new SqlCommand("sp_getServiceURL", connection);
                connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@code", SqlDbType.VarChar, 100).Value = param.Company_code;
                command.Parameters.Add("@Company", SqlDbType.VarChar, -1).Direction = ParameterDirection.Output;
                command.Parameters.Add("@ServiceURL", SqlDbType.VarChar, -1).Direction = ParameterDirection.Output;
                command.Parameters.Add("@Error_code", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@Error_msg", SqlDbType.VarChar, -1).Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                param.Company_Name = Convert.ToString(command.Parameters["@Company"].Value);
                param.Service_URL = Convert.ToString(command.Parameters["@ServiceURL"].Value);
                param.Error_msg = Convert.ToString(command.Parameters["@Error_msg"].Value);
                param.Error_Code = Convert.ToInt32(command.Parameters["@Error_code"].Value);

            }
            catch (Exception ex)
            {
                param.Error_Code = 202;
                param.Error_msg = ex.Message;
                ErrorLog(ex);
            }

            return param;
        }

        internal void ErrorLog(Exception ex)
        {
            StreamWriter log;
            DateTime dateTime = DateTime.UtcNow.Date;
            string ErrorLogFile = Convert.ToString(ConfigurationManager.AppSettings["ErrorLogFile"]) + "_" + dateTime.ToString("dd_MM_yyyy") + ".txt";
            string ErrorFile = Convert.ToString(ConfigurationManager.AppSettings["ErrorFile"]) + "_" + dateTime.ToString("dd_MM_yyyy") + ".txt";

            try
            {

                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += string.Format("Message: {0}", ex.Message);
                message += Environment.NewLine;
                message += string.Format("StackTrace: {0}", ex.StackTrace);
                message += Environment.NewLine;
                message += string.Format("Source: {0}", ex.Source);
                message += Environment.NewLine;
                message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;

                if (!File.Exists(ErrorFile))
                {
                    log = new StreamWriter(ErrorLogFile, true);
                }
                else
                {
                    log = File.AppendText(ErrorLogFile);
                }

                // Write to the file:
                log.WriteLine("Date Time:" + DateTime.Now.ToString());
                log.WriteLine(message);
                // Close the stream:
                log.Close();

            }
            catch (Exception)
            {

                if (!File.Exists(ErrorFile))
                {
                    log = new StreamWriter(ErrorLogFile, true);
                }
                else
                {
                    log = File.AppendText(ErrorLogFile);
                }
                log.Close();
            }
        }
        internal void TestLog(string text, string fn_name)
        {
            StreamWriter log;
            DateTime dateTime = DateTime.UtcNow.Date;
            string ErrorLogFile = Convert.ToString(ConfigurationManager.AppSettings["ErrorLogFile"]) + "_" + dateTime.ToString("dd_MM_yyyy") + ".txt";
            string ErrorFile = Convert.ToString(ConfigurationManager.AppSettings["ErrorFile"]) + "_" + dateTime.ToString("dd_MM_yyyy") + ".txt";

            try
            {
                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += string.Format("Function: {0}", fn_name);
                message += Environment.NewLine;
                message += string.Format("Message: {0}", text);
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;



                if (!File.Exists(ErrorFile))
                {
                    log = new StreamWriter(ErrorLogFile, true);
                }
                else
                {
                    log = File.AppendText(ErrorLogFile);
                }

                // Write to the file:
                log.WriteLine("Date Time:" + DateTime.Now.ToString());
                log.WriteLine(message);
                // Close the stream:
                log.Close();
            }
            catch (Exception)
            {

                if (!File.Exists(ErrorFile))
                {
                    log = new StreamWriter(ErrorLogFile, true);
                }
                else
                {
                    log = File.AppendText(ErrorLogFile);
                }
                log.Close();
            }
        }

    }
}
