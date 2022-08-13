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

    #region FabricBatchProductionBatchMan
    public class FabricBatchProductionBatchMan : BusinessObject
    {
        public FabricBatchProductionBatchMan()
        {
            
            FBPBID = 0;
            FBPID = 0;
            EmployeeID = 0;
            ShiftID = 0;
            Qty= 0;
            FinishDate = DateTime.Now;
            Note = "";
            ShiftName = "";
            ShiftStartTime= DateTime.Now;
            ShiftEndTime = DateTime.Now;
            EmployeeName = "";
            FabricBatchProductionBatchMans = new List<FabricBatchProductionBatchMan>();
            FBPBreakages = new List<FabricBatchProductionBreakage>();
            ErrorMessage = "";
            FBPBIDs = "";
            TotalFBPBreakage = 0;
            TotalNoOfBreakage = 0;
            TotalColor = 0;
            Efficiency = 0;
            RPM = 0;
            TSUID = 0;
            BeamNo = string.Empty;
            ApproveBy = 0;
            ApproveDate = DateTime.MinValue;
            ApproveName = "";
            FEONo = "";
            BuyerName = "";
            Construction = "";
            MachineCode  = "";
            Color = "";

        }

        #region Properties
        public int TSUID { get; set; }
        public int FBPBID { get; set; }
        public int FBPID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public string FBPBIDs { get; set; }
        public string ReceiveByName { get; set; }
        public double Qty { get; set; }
         public int ApproveBy { get; set; }
         public DateTime ApproveDate { get; set; }
         public string ApproveName { get; set; }
        public double QtyInM {
            get
            {
                return Global.GetMeter(this.Qty,2);
            }
        }
        public double Efficiency { get; set; }
        public int RPM { get; set; }
        public string ErrorMessage { get; set; }

        public string Note { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public DateTime ShiftEndTime { get; set; }

        public DateTime FinishDate { get; set; }
        public List<FabricBatchProductionBatchMan> FabricBatchProductionBatchMans { get; set; }
        public FabricBatch FabricProductionBatch { get; set; }
        public int TotalFBPBreakage { get; set; }
        public int TotalNoOfBreakage { get; set; }
        public int TotalColor { get; set; }
        public string ApproveDateStr
        {
            get
            {

                return (this.ApproveDate==DateTime.MinValue)?"-": this.ApproveDate.ToString("dd MMM yyyy");
                               
            }
        }
        public string FinishDateInString
        {
            get
            {
               
                    return this.FinishDate.ToString("dd MMM yyyy");
                               
            }
        }
        public string ShiftNameWithDuration
        {
            get
            {
                return this.ShiftName + "(" + this.ShiftStartTime.ToString("HH:mm") + "-" + this.ShiftEndTime.ToString("HH:mm") + ")";
            }
        }
        public string  FEONo{ get; set; }
        public string  BuyerName{ get; set; }
        public string  Construction{ get; set; }
        public string  MachineCode { get; set; }
        public string Color { get; set; }

        #endregion

        #region Derived Properties
        public string BeamNo { get; set; }
        public FabricBatch FabricBatch { get; set; }
        public List<FabricBatchProductionBreakage> FBPBreakages { get; set; }
        public double QtyinMeter { get { return Global.GetMeter(this.Qty, 2); } }
        #endregion

        #region Functions
        public FabricBatchProductionBatchMan Get(int nId, long nUserID)
        {
            return FabricBatchProductionBatchMan.Service.Get(nId, nUserID);
        }
        public static List<FabricBatchProductionBatchMan> Gets(int nFBPID,  long nUserID)
        {
            return FabricBatchProductionBatchMan.Service.Gets(nFBPID,  nUserID);
        }
        public static List<FabricBatchProductionBatchMan> GetsBySql(string sSql, long nUserID)
        {
            return FabricBatchProductionBatchMan.Service.GetsBySql(sSql, nUserID);
        }
        public FabricBatchProductionBatchMan Save(long nUserID)
        {
            return FabricBatchProductionBatchMan.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricBatchProductionBatchMan.Service.Delete(nId, nUserID);
        }
        public string MultipleApprove(String FBPBIDS, long nUserID)
        {
            return FabricBatchProductionBatchMan.Service.MultipleApprove(FBPBIDS, nUserID);
        }
        public string MultipleDelete(String FBPBIDS, long nUserID)
        {
            return FabricBatchProductionBatchMan.Service.MultipleDelete(FBPBIDS, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFabricBatchProductionBatchManService Service
        {
            get { return (IFabricBatchProductionBatchManService)Services.Factory.CreateService(typeof(IFabricBatchProductionBatchManService)); }
        }
        #endregion
    }
    #endregion


    #region IFabricBatchProductionBatchMan interface

    public interface IFabricBatchProductionBatchManService
    {
        FabricBatchProductionBatchMan Get(int id, long nUserID);
        List<FabricBatchProductionBatchMan> Gets(int nFBPID, long nUserID);
        List<FabricBatchProductionBatchMan> GetsBySql(string ssql, long nUserID);
        FabricBatchProductionBatchMan Save(FabricBatchProductionBatchMan oFabricBatchProductionBatchMan, long nUserID);
        string Delete(int id, long nUserID);
        string MultipleApprove(String FBPBIDS, long nUserID);
        string MultipleDelete(String FBPBIDS, long nUserID);
    }
    #endregion
    

}
