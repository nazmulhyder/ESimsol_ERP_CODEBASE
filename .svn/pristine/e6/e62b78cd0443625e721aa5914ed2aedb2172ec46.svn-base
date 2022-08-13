using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ServiceSchedule
    
    public class ServiceSchedule : BusinessObject
    {
        public ServiceSchedule()
        {
            ServiceScheduleID = 0;
            PreInvoiceID = 0;
            ServiceDate = DateTime.Now;
            ChargeType = EnumServiceILaborChargeType.Complementary;
            IsDone = false;
            DoneDate =DateTime.Today;
		    IsSMSSend  = false;
		    IsEmailSend =false;  
		    EmailBody ="";
            Status = EnumServiceScheduleStatus.None;
            Remarks = "";
            InvoiceNo = "";
            InvoiceDate = DateTime.Now;
            ContractorID = 0;
            CustomerName = "";
            CustomerShortName = "";
            ModelNo = "";
            CRM = "";
            CustomerPhoneNo = "";
            LastServiceDone= DateTime.MinValue;
            UpcomingServiceDate = DateTime.MinValue;
            TotalFreeService = 0;
            RemainingFreeService = 0;
            Warrenty = 0;
            RemaingWarrenty = 0;
            VinNo = "";
            RegistrationNo = "";
            kommNo = "";
            SalesPersonName = "";
            ChassisNo = "";
            IsPhoneCall = false;
            ServiceInterval = 0;
            ServiceDurationInMonth = 0;
            ServiceSchedules = new List<ServiceSchedule>();
            ErrorMessage = "";
        }

        #region Properties
        
        public int ServiceScheduleID { get; set; }
        public int PreInvoiceID { get; set; }
        public DateTime ServiceDate { get; set; }
        public EnumServiceILaborChargeType ChargeType { get; set; }
        public bool IsDone { get; set; }
        public DateTime   DoneDate { get; set; }
		public bool  IsSMSSend { get; set; }
		public bool  IsEmailSend { get; set; }
        public bool IsPhoneCall { get; set; }
		 public string  EmailBody { get; set; }
        public   EnumServiceScheduleStatus Status { get; set; }
        public string Remarks { get; set; }
        public string   InvoiceNo{ get; set; }
        public string ModelNo { get; set; }
	    public DateTime  InvoiceDate{ get; set; }
        public int ContractorID { get; set; }
		public string CustomerName{ get; set; }
        public string CustomerShortName { get; set; }
        public int ServiceInterval { get; set; }
        public int ServiceDurationInMonth { get; set; }
        public string CRM { get; set; }
        public string CustomerPhoneNo { get; set; }
        public DateTime LastServiceDone { get; set; }
        public DateTime UpcomingServiceDate { get; set; }
        public int TotalFreeService { get; set; }
        public int RemainingFreeService { get; set; }
        public double Warrenty { get; set; }
        public double RemaingWarrenty { get; set; }
        public string VinNo { get; set; }
        public string RegistrationNo { get; set; }
        public string kommNo { get; set; }
        public string SalesPersonName { get; set; }
        public string ChassisNo { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string Param { get; set; }
        public string TempServiceDateSt { get; set; }
        public List<ServiceSchedule> ServiceSchedules { get; set; }
        public string ServiceDateSt
        {
            get
            {
                return this.ServiceDate.ToString("dd MMM yyyy hh:mm tt");
            }
     
        }
        public string DayOfWeekSt
        {
            get
            {
                return this.ServiceDate.DayOfWeek.ToString().Substring(0, 3);
            }

        }
        public string DoneDateSt
        {
            get
            {
                return this.IsDone? this.DoneDate.ToString("dd MMM yyyy"):"-";
            }

        }
        public string ChargeTypeSt
        {
            get
            {
                return this.ChargeType.ToString();
            }
        }
        public string IsSMSSendSt
        {
            get
            {
                return this.IsSMSSend ? "Yes" : "No";
            }
        }
        public string IsEmailSendSt
        {
            get
            {
                return this.IsEmailSend ? "Yes" : "No";
            }
        }
        public string IsPhoneCallSt
        {
            get
            {
                return this.IsPhoneCall ? "Yes" : "No";
            }
        }
     
        public string StatusSt
        {
            get
            {
                return EnumObject.jGet(this.Status);
            }
        }
     
        #endregion

        #region Functions

        public static List<ServiceSchedule> Gets(long nUserID)
        {
            return ServiceSchedule.Service.Gets(nUserID);
        }
        public static List<ServiceSchedule> Gets(string sSQL, Int64 nUserID)
        {
            return ServiceSchedule.Service.Gets(sSQL, nUserID);
        }
     
        public ServiceSchedule Get(int nId, long nUserID)
        {
            return ServiceSchedule.Service.Get(nId,nUserID);
        }
        public ServiceSchedule Done(long nUserID)
        {
            return ServiceSchedule.Service.Done(this, nUserID);
        }
        public ServiceSchedule SendEmailOrSMS(bool bIsEmail, long nUserID)
        {
            return ServiceSchedule.Service.SendEmailOrSMS(this, bIsEmail, nUserID);
        }
        public ServiceSchedule PhoneCall( long nUserID)
        {
            return ServiceSchedule.Service.PhoneCall(this,  nUserID);
        }  
        public ServiceSchedule Save(long nUserID)
        {
            return ServiceSchedule.Service.Save(this, nUserID);
        }

        public ServiceSchedule ReSchedule(long nUserID)
        {
            return ServiceSchedule.Service.ReSchedule(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return ServiceSchedule.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IServiceScheduleService Service
        {
            get { return (IServiceScheduleService)Services.Factory.CreateService(typeof(IServiceScheduleService)); }
        }
        #endregion
    }
    #endregion

    #region IServiceSchedule interface
    
    public interface IServiceScheduleService
    {
        ServiceSchedule Get(int id, long nUserID);
        List<ServiceSchedule> Gets(long nUserID);
        List<ServiceSchedule> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        ServiceSchedule Save(ServiceSchedule oServiceSchedule, long nUserID);
        ServiceSchedule ReSchedule(ServiceSchedule oServiceSchedule, long nUserID);
        ServiceSchedule PhoneCall(ServiceSchedule oServiceSchedule, long nUserID);
        
        
        ServiceSchedule Done(ServiceSchedule oServiceSchedule, long nUserID);
        ServiceSchedule SendEmailOrSMS(ServiceSchedule oServiceSchedule,bool bIsSendEmail, long nUserID);
        
    }
    #endregion
}
