using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    public class EmailConfig
    {

        public EmailConfig()
        {
            EmailConfigID=0;
            BUID = 0;
            BUName = "";
            EmailAddress = "";
            EmailPassword = "";
            Remarks = "";
            EmailDisplayName = "";
            PortNumber = "";
            HostName = "";
            SSLRequired = false;
            ErrorMessage = "";
            EmailConfigs = new List<EmailConfig>();
        }

        #region Properties
         
        public int EmailConfigID{ get; set; }   
        public int BUID{ get; set; }
        public string BUName { get; set; }
        public string EmailAddress{ get; set; }
        public string EmailPassword{ get; set; }     
        public string Remarks { get; set; }
        public string EmailDisplayName { get; set; }   
        public string PortNumber { get; set; }
        public string HostName { get; set; }
        public bool SSLRequired { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string SSLRequiredSt
        {
            get
            {
                if (this.SSLRequired)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public bool Selected { get; set; }
       
        public List<EmailConfig> EmailConfigs { get; set; }
        #endregion

        #region Functions

        public static List<EmailConfig> Gets(long nUserID)
        {
            
            return EmailConfig.Service.Gets( nUserID);
        }
        public EmailConfig Get(int id, long nUserID)
        {
            return EmailConfig.Service.Get(id, nUserID);
        }
        public EmailConfig GetByBU(int nBUID, long nUserID)
        {
            return EmailConfig.Service.GetByBU(nBUID, nUserID);
        }
        public EmailConfig Save(long nUserID)
        {
            return EmailConfig.Service.Save(this, nUserID);
        }

        public static List<EmailConfig> Gets(string sSQL, long nUserID)
        {
            return EmailConfig.Service.Gets(sSQL, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return EmailConfig.Service.Delete(id, nUserID);
        }
      
        #endregion

        #region ServiceFactory

 
        internal static IEmailConfigService Service
        {
            get { return (IEmailConfigService)Services.Factory.CreateService(typeof(IEmailConfigService)); }
        }

        #endregion
    }
    

    #region IEmailConfig interface
     
    public interface IEmailConfigService
    {         
        EmailConfig Get(int id, Int64 nUserID);
        EmailConfig GetByBU(int nBUID, Int64 nUserID);         
        List<EmailConfig> Gets(Int64 nUserID);         
        List<EmailConfig> Gets(string sSQL, Int64 nUserID);               
        string Delete(int id, Int64 nUserID);         
        EmailConfig Save(EmailConfig oEmailConfig, Int64 nUserID);
    }
    #endregion
}
