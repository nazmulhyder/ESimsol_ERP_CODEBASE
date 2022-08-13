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
    #region DUHardWindingReport
    public class DUHardWindingReport : BusinessObject
    {
        public DUHardWindingReport()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            QtyHWIn = 0;
            QtyHWOut = 0;
            QtyReHWIn = 0;
            QtyReHWOut = 0;
            QtyGreige = 0;
            Qty_LO = 0;

            BeamCom = 0;
            BeamTF = 0;
            Warping = 0;
            BeamStock = 0;
            BUID = 0;
            ReportLayout = 0;
            ReportType = 0;
            ErrorMessage = "";
        }

        #region Property
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double QtyHWIn { get; set; }
        public double QtyHWOut { get; set; }
        public double QtyReHWIn { get; set; }
        public double QtyReHWOut { get; set; }
        public double QtyGreige { get; set; }
        public double Qty_LO { get; set; }

        public double BeamCom { get; set; }
        public double BeamTF { get; set; }
        public double Warping { get; set; }
        public double BeamStock { get; set; }

        public int BUID { get; set; }
        public int ReportLayout { get; set; }
        public int ReportType { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public string StartDateInString
        {
            get
            {
                if (StartDate == DateTime.MinValue) return "";
                return StartDate.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string EndDateInString
        {
            get
            {
                if (EndDate == DateTime.MinValue) return "";
                return EndDate.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public double TotalQty
        {
            get
            {
                return this.QtyHWIn+this.QtyHWOut+this.QtyReHWIn+this.QtyReHWOut+this.QtyGreige;
            }
        }
        public double GrandTotalQty
        {
            get
            {
                return this.QtyHWIn + this.QtyHWOut + this.QtyReHWIn + this.QtyReHWOut + this.QtyGreige+this.Qty_LO;
            }
        }
        #endregion

        #region Functions

        public static List<DUHardWindingReport> Gets(DUHardWindingReport oDUHardWindingReport, long nUserID)
        {
            return DUHardWindingReport.Service.Gets(oDUHardWindingReport, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDUHardWindingReportService Service
        {
            get { return (IDUHardWindingReportService)Services.Factory.CreateService(typeof(IDUHardWindingReportService)); }
        }
        #endregion

        public List<DUHardWindingReport> DUHardWindingReports { get; set; }
    }
    #endregion

    #region IDUHardWindingReport interface
    public interface IDUHardWindingReportService
    {
        List<DUHardWindingReport> Gets(DUHardWindingReport oDUHardWindingReport, Int64 nUserID);
    }
    #endregion
}
