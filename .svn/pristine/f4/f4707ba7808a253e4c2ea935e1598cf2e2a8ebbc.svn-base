using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    public class DUProductionStatus : BusinessObject
    {
        public DUProductionStatus()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            QtyDyeing = 0;
            QtyDyeing_ReP = 0;
            QtyDyeing_Sweater = 0;
            QtyDyeing_Knit = 0;
            QtyDyeing_White = 0;
            White_Percentage = 0;
            QtyDyeing_Total = 0;
            ReProcess_InHouse = 0;
            ReProcess_OutSide = 0;
            ReProcess_Percentage = 0;
            ReProcess_Total = 0;
            Remarks = "";
            RSState = EnumRSState.None;
            DateType = 0;
            RefID = 0;
            RefName = "";
            Month = DateTime.Now;
            Year = 0;
            OrderType = 0;
            ErrorMessage = string.Empty;
            OrderTypeSt = string.Empty;
        }
        #region properties
        public int BUID { get; set; }
        public double QtyDyeing { get; set; }
        public double QtyDyeing_ReP { get; set; }
        public double QtyRecycle { get; set; }
        public double QtyWestage { get; set; }
        public double QtyRecycle_ReP { get; set; }
        public double QtyWestage_ReP { get; set; }
        public double QtyPacking { get; set; }
        public double QtyPacking_ReP { get; set; }

        public double QtyDyeing_Sweater { get; set; }
        public double QtyDyeing_Knit { get; set; }
        public double QtyDyeing_White { get; set; }
        public double White_Percentage { get; set; }
        public double QtyDyeing_Total { get; set; }
        public double ReProcess_InHouse { get; set; }
        public double ReProcess_OutSide { get; set; }
        public double ReProcess_Percentage { get; set; }
        public double ReProcess_Total { get; set; }
        public double QtyDC { get; set; }
        public double QtyDC_ReP { get; set; }
        public double QtyRC { get; set; }
        public double QtyRC_ReP { get; set; }
        public EnumRSState RSState { get; set; }
        public int DateType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Month { get; set; }
        public int Year { get; set; }
        public string Remarks { get; set; }
        public string OrderTypeSt { get; set; }
        public int OrderType { get; set; }
        public int RefID { get; set; }
        public string RefName { get; set; }
        public string Params { get; set; }
        public int nReportLayout { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        #region Derived Properties
        public string StartDateStr { get { return (this.StartDate == DateTime.MinValue) ? "" : this.StartDate.ToString("dd MMM yyyy"); } }
        public string StartDateStr_MMYY { get { return (this.StartDate == DateTime.MinValue) ? "" : this.StartDate.ToString("MMMM yy"); } }
        public string EndDateStr { get { return (this.EndDate == DateTime.MinValue) ? "" : this.EndDate.ToString("dd MMM yyyy"); } }

        public string StartDateMonthStr { get { return (this.StartDate == DateTime.MinValue) ? "" : this.StartDate.ToString("dd MMMM yy"); } }

        #endregion
        #region Functions
        public static List<DUProductionStatus> GetsDUProductionStatus(int nBUID, int nLayout, DateTime StartDate, DateTime EndDate, string sSQL, EnumRSState nRSState, long nUserID)
        {
            return DUProductionStatus.Service.GetsDUProductionStatus(nBUID, nLayout, StartDate, EndDate,sSQL, nRSState, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDUProductionStatusService Service
        {
            get { return (IDUProductionStatusService)Services.Factory.CreateService(typeof(IDUProductionStatusService)); }
        }

        #endregion

    }
    #region  TipsType interface
    public interface IDUProductionStatusService
    {
        List<DUProductionStatus> GetsDUProductionStatus(int nBUID, int nLayout, DateTime StartDate, DateTime EndDate, string sSQL, EnumRSState nRSState, long nUserID);
    }
    #endregion
}
