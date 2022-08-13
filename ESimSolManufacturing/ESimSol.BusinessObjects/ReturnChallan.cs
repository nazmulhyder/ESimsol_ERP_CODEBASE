using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region ReturnChallan
    public class ReturnChallan : BusinessObject
    {
        public ReturnChallan()
        {
            ReturnChallanID = 0;
            BUID = 0;
            ContractorID = 0;
            ReturnDate = DateTime.Today;
            ContractorName = "";
            ReceivedByName = string.Empty;
            Note = string.Empty;
            ApprovedBy = 0;
            ReceivedBy = 0;
            ExportSCID = 0;
            PINo = "";
            WorkingUnitID = 0;
            StoreName = "";
            ErrorMessage = string.Empty;
            ReturnChallanDetails = new List<ReturnChallanDetail>();
        }

        #region Property
        public int ReturnChallanID { get; set; }
        public int BUID {get; set;}
        public string ReturnChallanNo { get; set; }
        public DateTime ReturnDate { get; set; }
        public int ContractorID { get; set; }
        public string ReceivedByName {get; set;}
        public string Note {get; set;}
        public int ApprovedBy { get; set; }
        public int ReceivedBy { get; set; }
        public EnumProductNature ProductNature { get; set; }
        public int ProductNatureInInt { get; set; }
        public int ExportSCID { get; set; }
        public string PINo { get; set; }
        public int WorkingUnitID { get; set; }
        public string StoreName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<ReturnChallanDetail> ReturnChallanDetails { get; set; }
        public List<ReturnChallan> ReturnChallans { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public Company Company { get; set; }
        public string BUName { get; set; }
        public string DONo { get; set; }
        public string ContractorName { get; set; }
        public string ApprovedByName { get; set; }
        
        public string WUName { get; set; }
        public string ReturnDateStr
        {
            get
            {
                return this.ReturnDate.ToString("dd MMM yyyy");
            }
        }

        #endregion

        #region Functions

        public ReturnChallan Get(int id, long nUserID)
        {
            return ReturnChallan.Service.Get(id, nUserID);
        }
        public static List<ReturnChallan> Gets(string sSQL, long nUserID)
        {
            return ReturnChallan.Service.Gets(sSQL, nUserID);
        }
        public ReturnChallan IUD(short nDBOperation, long nUserID)
        {
            return ReturnChallan.Service.IUD(this, nDBOperation, nUserID);
        }

        public ReturnChallan Approve(long nUserID)
        {
            return ReturnChallan.Service.Approve(this, nUserID);
        }
        public ReturnChallan Receive(long nUserID)
        {
            return ReturnChallan.Service.Receive(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IReturnChallanService Service
        {
            get { return (IReturnChallanService)Services.Factory.CreateService(typeof(IReturnChallanService)); }
        }
        #endregion
    }
    #endregion

    #region IReturnChallan interface
    public interface IReturnChallanService
    {
        ReturnChallan Get(int id, Int64 nUserID);
        List<ReturnChallan> Gets(string sSQL, Int64 nUserID);
        ReturnChallan IUD(ReturnChallan oReturnChallan, short nDBOperation, Int64 nUserID);
        ReturnChallan Receive(ReturnChallan oReturnChallan, Int64 nUserID);
        ReturnChallan Approve(ReturnChallan oReturnChallan, Int64 nUserID);
        
    }
    #endregion
}
