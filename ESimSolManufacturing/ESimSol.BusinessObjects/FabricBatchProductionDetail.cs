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

    #region FabricBatchProductionDetail
    public class FabricBatchProductionDetail : BusinessObject
    {
        public FabricBatchProductionDetail()
        {

            FBPDetailID = 0;
            FBPID = 0;
            EmployeeID = 0;
            ShiftID = 0;
            Qty = 0;
            ProductionDate = DateTime.Now;
            Note = "";
            ShiftName = "";
            ShiftStartTime = DateTime.Now;
            ShiftEndTime = DateTime.Now;
            EmployeeName = "";
            MachineName = "";
            //FabricBatchProductionDetails = new List<FabricBatchProductionDetail>();
            ErrorMessage = "";
            NoOfBreakage = 0;
            FabricBatchProductionBeams = new List<FabricBatchProductionBeam>();
            TotalEnds = 0;
            QtyBatch = 0;
            BatchNo = "";
            FBID = 0;
            FMID = 0;
            NoOfFrame = 0;
            ProductionStatus = EnumProductionStatus.Initialize;
        }

        #region Properties
        public int FBPDetailID { get; set; }
        public int FBPID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime ProductionDate { get; set; }
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public double Qty { get; set; }
        public int FMID { get; set; }
        public DateTime EntryDate { get; set; }
        public string UserName { get; set; }
        public double NoOfBreakage { get; set; }
        public double NoOfFrame { get; set; }
        public string ErrorMessage { get; set; }
        public string Note { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public DateTime ShiftEndTime { get; set; }
        public double QtyM
        {
            get
            {
                return Global.GetMeter(this.Qty, 2);
            }
        }
        public EnumProductionStatus ProductionStatus { get; set; }
        public double TotalEnds { get; set; }
        public double QtyBatch { get; set; }
        public string BatchNo { get; set; }
        public int FBID { get; set; }
        public int FEOSID { get; set; }
        public double ReedCount { get; set; }
        public List<FabricBatchProductionBeam> FabricBatchProductionBeams { get; set; }
        public string EntryDateSt
        {
            get
            {

                return (this.EntryDate == DateTime.MinValue) ? "-" : this.EntryDate.ToString("dd MMM yyyy");

            }
        }
        public string ProductionDateSt
        {
            get
            {

                return this.ProductionDate.ToString("dd MMM yyyy");

            }
        }
        public string ShiftNameWithDuration
        {
            get
            {
                return this.ShiftName + "(" + this.ShiftStartTime.ToString("HH:mm") + "-" + this.ShiftEndTime.ToString("HH:mm") + ")";
            }
        }
        public string EmployeeName { get; set; }
        public string MachineName { get; set; }
        
        #endregion


        #region Functions
        public FabricBatchProductionDetail Get(int nId, long nUserID)
        {
            return FabricBatchProductionDetail.Service.Get(nId, nUserID);
        }
        public static List<FabricBatchProductionDetail> Gets(int nFBPID, long nUserID)
        {
            return FabricBatchProductionDetail.Service.Gets(nFBPID, nUserID);
        }
    
        public FabricBatchProductionDetail Save(long nUserID)
        {
            return FabricBatchProductionDetail.Service.Save(this, nUserID);
        }
        public FabricBatchProductionDetail SaveDetailWithBeam(long nUserID)
        {
            return FabricBatchProductionDetail.Service.SaveDetailWithBeam(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricBatchProductionDetail.Service.Delete(nId, nUserID);
        }
        public static List<FabricBatchProductionDetail> Gets(string sSQL, long nUserID)
        {
            return FabricBatchProductionDetail.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFabricBatchProductionDetailService Service
        {
            get { return (IFabricBatchProductionDetailService)Services.Factory.CreateService(typeof(IFabricBatchProductionDetailService)); }
        }
        #endregion
    }
    #endregion


    #region IFabricBatchProductionDetail interface

    public interface IFabricBatchProductionDetailService
    {
        FabricBatchProductionDetail Get(int id, long nUserID);
        List<FabricBatchProductionDetail> Gets(int nFBPID, long nUserID);
        FabricBatchProductionDetail Save(FabricBatchProductionDetail oFabricBatchProductionDetail, long nUserID);
        FabricBatchProductionDetail SaveDetailWithBeam(FabricBatchProductionDetail oFabricBatchProductionDetail, long nUserID);
        List<FabricBatchProductionDetail> Gets(string sSQL, long nUserID);
        string Delete(int id, long nUserID);
        //string MultipleApprove(String FBPDetailIDS, long nUserID);
        //string MultipleDelete(String FBPDetailIDS, long nUserID);
    }
    #endregion


}
