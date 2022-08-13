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

    #region FabricBatchLoomDetail
    public class FabricBatchLoomDetail : BusinessObject
    {
        public FabricBatchLoomDetail()
        {

            FBLDetailID = 0;
            FabricBatchLoomID = 0;
            EmployeeID = 0;
            ShiftID = 0;
            Qty = 0;
            FinishDate = DateTime.Now;
            Note = "";
            ShiftName = "";
            ShiftStartTime = DateTime.Now;
            ShiftEndTime = DateTime.Now;
            EmployeeName = "";
            FabricBatchLoomDetails = new List<FabricBatchLoomDetail>();
            FBPBreakages = new List<FabricBatchProductionBreakage>();
            ErrorMessage = "";
            FBLDetailIDs = "";
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
            MachineCode = "";
            Color = "";
            FinishType = "";
            FabricType = "";
            Warp = 0;
            Weft = 0;
        }

        #region Properties
        public int TSUID { get; set; }
        public int FBLDetailID { get; set; }
        public int FabricBatchLoomID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string FinishType { get; set; }
        public string FabricType { get; set; }
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public string FBLDetailIDs { get; set; }
        public string ReceiveByName { get; set; }
        public double Qty { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ApproveName { get; set; }
        public int Warp { get; set; }
        public int Weft { get; set; }
        public double QtyInM
        {
            get
            {
                return Global.GetMeter(this.Qty, 2);
            }
        }
        public double Efficiency { get; set; }
        public int RPM { get; set; }
        public string ErrorMessage { get; set; }

        public string Note { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public DateTime ShiftEndTime { get; set; }

        public DateTime FinishDate { get; set; }
        public List<FabricBatchLoomDetail> FabricBatchLoomDetails { get; set; }
        public FabricBatch FabricProductionBatch { get; set; }
        public int TotalFBPBreakage { get; set; }
        public int TotalNoOfBreakage { get; set; }
        public int TotalColor { get; set; }
        public string ApproveDateStr
        {
            get
            {

                return (this.ApproveDate == DateTime.MinValue) ? "-" : this.ApproveDate.ToString("dd MMM yyyy");

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
        public string FEONo { get; set; }
        public string BuyerName { get; set; }
        public string Construction { get; set; }
        public string MachineCode { get; set; }
        public string Color { get; set; }

        #endregion

        #region Derived Properties
        public string BeamNo { get; set; }
        public FabricBatch FabricBatch { get; set; }
        public List<FabricBatchProductionBreakage> FBPBreakages { get; set; }
        public double QtyinMeter { get { return Global.GetMeter(this.Qty, 2); } }
        #endregion

        #region Functions
        public FabricBatchLoomDetail Get(int nId, long nUserID)
        {
            return FabricBatchLoomDetail.Service.Get(nId, nUserID);
        }
        public static List<FabricBatchLoomDetail> Gets(int nFabricBatchLoomID, long nUserID)
        {
            return FabricBatchLoomDetail.Service.Gets(nFabricBatchLoomID, nUserID);
        }
        public static List<FabricBatchLoomDetail> GetsBySql(string sSql, long nUserID)
        {
            return FabricBatchLoomDetail.Service.GetsBySql(sSql, nUserID);
        }
        public FabricBatchLoomDetail Save(long nUserID)
        {
            return FabricBatchLoomDetail.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricBatchLoomDetail.Service.Delete(nId, nUserID);
        }
        public string MultipleApprove(String FBLDetailIDS, long nUserID)
        {
            return FabricBatchLoomDetail.Service.MultipleApprove(FBLDetailIDS, nUserID);
        }
        public string MultipleDelete(String FBLDetailIDS, long nUserID)
        {
            return FabricBatchLoomDetail.Service.MultipleDelete(FBLDetailIDS, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFabricBatchLoomDetailService Service
        {
            get { return (IFabricBatchLoomDetailService)Services.Factory.CreateService(typeof(IFabricBatchLoomDetailService)); }
        }
        #endregion
    }
    #endregion


    #region IFabricBatchLoomDetail interface

    public interface IFabricBatchLoomDetailService
    {
        FabricBatchLoomDetail Get(int id, long nUserID);
        List<FabricBatchLoomDetail> Gets(int nFabricBatchLoomID, long nUserID);
        List<FabricBatchLoomDetail> GetsBySql(string ssql, long nUserID);
        FabricBatchLoomDetail Save(FabricBatchLoomDetail oFabricBatchLoomDetail, long nUserID);
        string Delete(int id, long nUserID);
        string MultipleApprove(String FBLDetailIDS, long nUserID);
        string MultipleDelete(String FBLDetailIDS, long nUserID);
    }
    #endregion


}
