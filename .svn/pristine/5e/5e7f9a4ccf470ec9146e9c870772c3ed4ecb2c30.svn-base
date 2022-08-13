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

    #region PTUUnit2Distribution
    public class PTUUnit2Distribution : BusinessObject
    {
        public PTUUnit2Distribution()
        {
            PTUUnit2DistributionID = 0;
            PTUUnit2ID = 0;
            LotID = 0;
            Qty = 0;
            LotNo = "";
            WorkingUnitID = 0;
            BUID = 0;
            MUName = "";
            WorkingUnitName = "";
            ProductName = "";
            ColorName = "";
            LotBalance = 0;
            TransactionTime = DateTime.Now;
            PINo = "";
            BuyerName = "";
            ErrorMessage = "";
        }
        #region Properties
        public int PTUUnit2DistributionID { get; set; }
        public int LotID { get; set; }
        public int PTUUnit2ID { get; set; }
        public double Qty { get; set; }
        public string LotNo { get; set; }
        public int WorkingUnitID { get; set; }
        public int BUID { get; set; }
        public string MUName { get; set; }
        public string WorkingUnitName { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public double LotBalance { get; set; }
        public DateTime TransactionTime { get; set; }
        public string PINo { get; set; }
        public string BuyerName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived propertiy
        public string LotNoWithWorkingUnitName
        {
            get
            {
                return this.WorkingUnitName + "[" + this.LotNo + "]";
            }
            
        }
        public string TransactionTimeSt
        {
            get
            {
                return this.TransactionTime.ToString("dd MMM yyyy");
            }

        }
        public string QtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.Qty) + " " + this.MUName;
            }
        }
        #endregion

        #region Functions


        public static List<PTUUnit2Distribution> Gets(int nUserID)
        {
            return PTUUnit2Distribution.Service.Gets(nUserID);
        }
        public  PTUUnit2Distribution PTUTransfer(int nUserID)
        {
            return PTUUnit2Distribution.Service.PTUTransfer(this, nUserID);
        }
        public PTUUnit2Distribution ReceiveInReadyeStock(int nUserID)
        {
            return PTUUnit2Distribution.Service.ReceiveInReadyeStock(this, nUserID);
        }
        public PTUUnit2Distribution PTUTransferSubContract(int nUserID)
        {
            return PTUUnit2Distribution.Service.PTUTransferSubContract(this, nUserID);
        }
        public PTUUnit2Distribution ReceiveInAvilableStock(int nUserID)
        {
            return PTUUnit2Distribution.Service.ReceiveInAvilableStock(this, nUserID);
        }
        public static List<PTUUnit2Distribution> Gets(int nShelfID, int nUserID)
        {
            return PTUUnit2Distribution.Service.Gets(nShelfID, nUserID);
        }
        public static List<PTUUnit2Distribution> Gets(string sSQL, int nUserID)
        {
            return PTUUnit2Distribution.Service.Gets(sSQL, nUserID);
        }
        public static List<PTUUnit2Distribution> ConfirmPTUUnit2Distribution(List<PTUUnit2Distribution> oPTUUnit2Distributions, int nLotID, Int64 nUserID)
        {
            return PTUUnit2Distribution.Service.ConfirmPTUUnit2Distribution(oPTUUnit2Distributions, nLotID, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IPTUUnit2DistributionService Service
        {
            get { return (IPTUUnit2DistributionService)Services.Factory.CreateService(typeof(IPTUUnit2DistributionService)); }
        }
        #endregion
    }
    #endregion



    #region IPTUUnit2Distribution interface
    public interface IPTUUnit2DistributionService
    {
        PTUUnit2Distribution PTUTransfer(PTUUnit2Distribution oPTUUnit2Distribution, int nUserID);
        PTUUnit2Distribution ReceiveInReadyeStock(PTUUnit2Distribution oPTUUnit2Distribution, int nUserID);
        PTUUnit2Distribution PTUTransferSubContract(PTUUnit2Distribution oPTUUnit2Distribution, int nUserID);
        PTUUnit2Distribution ReceiveInAvilableStock(PTUUnit2Distribution oPTUUnit2Distribution, int nUserID);
        List<PTUUnit2Distribution> Gets(int nUserID);
        List<PTUUnit2Distribution> Gets(int nShelfID, int nUserID);
        List<PTUUnit2Distribution> Gets(string sSQL, int nUserID);
        List<PTUUnit2Distribution> ConfirmPTUUnit2Distribution(List<PTUUnit2Distribution> oPTUUnit2Distributions, int nLotID, Int64 nUserID);


        
    }
    #endregion
    
}
