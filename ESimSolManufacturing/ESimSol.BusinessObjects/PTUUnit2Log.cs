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
    #region PTUUnit2Log
    public class PTUUnit2Log : BusinessObject
    {
        public PTUUnit2Log()
        {
            PTUUnit2LogID = 0;
            PTUUnit2ID = 0;
            BalanceQty = 0;
            Qty = 0;
            RefNo = "";
            PTUUnit2RefID = 0;
            Note = "";
            PTUUnit2RefType = EnumPTUUnit2Ref.Production_Order_Detail;
            PTUUnit2RefTypeInInt = 0;
            SheetWiseActualFinish = 0;
            SheetWiseReject = 0;
            POApproveByName = "";
            PODate = DateTime.Now;
            DBServerDateTime = DateTime.Now;
            DeliveryDate = DateTime.Now;
            DOApprovedByName = "";
            EntryPerson = "";
            MUSymbol = "";
            WorkingUnitName = "";
            ContractBUName = "";
            ContractDate = DateTime.Now;
            ErrorMessage = "";
        }

        #region Property
        public int PTUUnit2LogID { get; set; }
        public int PTUUnit2ID { get; set; }
        public double BalanceQty { get; set; }
        public double Qty { get; set; }
        public string RefNo { get; set; }
        public string Note { get; set; }
        public int PTUUnit2RefID { get; set; }
        public EnumPTUUnit2Ref PTUUnit2RefType { get; set; }
        public int PTUUnit2RefTypeInInt { get; set; }
        public double SheetWiseActualFinish { get; set; }
        public double SheetWiseReject { get; set; }
        public string POApproveByName { get; set; }
        public DateTime PODate { get; set; }
        public string EntryPerson { get; set; }
        public string MUSymbol { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string WorkingUnitName { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DOApprovedByName { get; set; }
        public string ContractBUName { get; set; }
        public DateTime ContractDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ContractDateInstring
        {
            get
            {
                if (this.ContractDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.ContractDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string DeliveryDateInString
        {
            get
            {
                return this.DeliveryDate.ToString("dd MMM yyyy hh:ss:t");
            }
        }
        public string DBServerDateTimeInString
        {
            get
            {
                return this.DBServerDateTime.ToString("dd MMM yyyy hh:ss:t");
            }
        }
        public string PODateInString
        {
            get
            {
                return this.PODate.ToString("dd MMM yyyy");
            }
        }
        public string PipeLineQtyInString
        {
            get
            {
                return Global.MillionFormatActualDigit( this.Qty - (this.SheetWiseActualFinish + this.SheetWiseReject));
            }            
        }
        public string QtyInString
        {
            get
            {
                return Global.MillionFormatActualDigit(this.Qty)+" "+this.MUSymbol;
            }
        }
        #endregion

        #region Functions
        public static List<PTUUnit2Log> Gets(int nPTUUnit2ID, int eType, long nUserID)
        {
            return PTUUnit2Log.Service.Gets(nPTUUnit2ID, eType, nUserID);
        }
        public static List<PTUUnit2Log> Gets(string sSQL, long nUserID)
        {
            return PTUUnit2Log.Service.Gets(sSQL, nUserID);
        }
        public PTUUnit2Log Get(int id, long nUserID)
        {
            return PTUUnit2Log.Service.Get(id, nUserID);
        }
      

        #endregion

        #region ServiceFactory
        internal static IPTUUnit2LogService Service
        {
            get { return (IPTUUnit2LogService)Services.Factory.CreateService(typeof(IPTUUnit2LogService)); }
        }
        #endregion
    }
    #endregion

    #region IPTUUnit2Log interface
    public interface IPTUUnit2LogService
    {
        PTUUnit2Log Get(int id,  Int64 nUserID);
        List<PTUUnit2Log> Gets(int nID, int nEType, Int64 nUserID);
        List<PTUUnit2Log> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
    
   
}
