using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects.ReportingObject
{
    #region RptProductionMonthlyInspection
    public class RptProductionMonthlyInspection : BusinessObject
    {
        public RptProductionMonthlyInspection()
        {
            LockDate = DateTime.Today;
            AGrade = 0;
            BGrade = 0;
            TotalReject = 0;
            RAShift = 0;
            RBShift = 0;
            RCShift = 0;
            YarnFault = 0;
            YDFault = 0;
            Weaving = 0;
            Remarks = "";
            ErrorMessage = "";
        }

        #region Properties
        public DateTime LockDate { get; set; }
        public double AGrade { get; set; }
        public double TotalReject { get; set; }
        public double RAShift { get; set; }
        public double RBShift { get; set; }
        public double RCShift { get; set; }
        public double BGrade { get; set; }
        public double YarnFault { get; set; }
        public double YDFault { get; set; }
        public double Weaving { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derive Properties
        public string RejectPercentage
        {
            get
            {
                double nTotal = this.AGrade + this.BGrade + this.TotalReject;
                double nCal = (100 * this.TotalReject / nTotal);
                if (double.IsNaN(nCal))
                {
                    nCal = 0; 
                }
                return nCal.ToString("0.00") + "%";
            }
        }
        public double TotalFolProduction
        {
            get 
            {
                return this.AGrade + this.BGrade + this.TotalReject;
            }
        }


        public double AGradeInMtr
        {
            get
            {
                return Global.GetMeter(this.AGrade, 2);
            }
        }

        public double BGradeInMtr
        {
            get
            {
                return Global.GetMeter(this.BGrade, 2);
            }
        }

        public double TotalRejectInMtr
        {
            get
            {
                return Global.GetMeter(this.TotalReject, 2);
            }
        }

        public double RAShiftInMtr
        {
            get
            {
                return Global.GetMeter(this.RAShift, 2);
            }
        }

        public double RBShiftInMtr
        {
            get
            {
                return Global.GetMeter(this.RBShift, 2);
            }
        }
        public double RCShiftInMtr
        {
            get
            {
                return Global.GetMeter(this.RCShift, 2);
            }
        }

        public double YarnFaultInMtr
        {
            get
            {
                return Global.GetMeter(this.YarnFault, 2);
            }
        }

        public double YDFaultInMtr
        {
            get
            {
                return Global.GetMeter(this.YDFault, 2);
            }
        }
      
        public double WeavingInMtr
        {
            get
            {
                return Global.GetMeter(this.Weaving, 2);
            }
        }

        public double TotalFolProductionInMtr
        {
            get
            {

                return this.AGradeInMtr + this.BGradeInMtr + this.TotalRejectInMtr;
            }
        }

        public string RejectPercentageInMtr
        {
            get
            {
                double nTotal = this.TotalFolProductionInMtr;
                double nCal = (100 * this.TotalRejectInMtr / nTotal);
                if (double.IsNaN(nCal))
                {
                    nCal = 0;
                }
                return nCal.ToString("0.00") + "%";
            }
        }



        public string WeavingSt
        {
            get
            {
                return this.GetQty(this.Weaving);
            }
        }
        public string YDFaultSt
        {
            get
            {
                return this.GetQty(this.YDFault);
            }
        }
        public string YarnFaultSt
        {
            get
            {
                return this.GetQty(this.YarnFault);
            }
        }
        public string RAShiftSt
        {
            get
            {
                return this.GetQty(this.RAShift);
            }
        }
        public string RBShiftSt
        {
            get
            {
                return this.GetQty(this.RBShift);
            }
        }
        public string RCShiftSt
        {
            get
            {
                return this.GetQty(this.RCShift);
            }
        }
        public string TotalRejectSt
        {
            get
            {
                return this.GetQty(this.TotalReject);
            }
        }
        public string BGradeSt
        {
            get
            {
                return this.GetQty(this.BGrade);
            }
        }
        public string AGradeSt
        {
            get
            {
                return this.GetQty(this.AGrade);
            }
        }
        public string LockDateSt
        {
            get
            {
                return this.GetDate(this.LockDate);
            }
        }
        private string GetQty(double nVal)
        {
            if (nVal < 0) return "(" + Global.MillionFormat(nVal * (-1)) + ")";
            else if (nVal == 0) return "-";
            else return Global.MillionFormat(nVal);
        }
        private string GetDate(DateTime dVal)
        {
            DateTime MinValue = new DateTime(1900, 01, 01);
            DateTime MinValue1 = new DateTime(0001, 01, 01);
            if (dVal == MinValue || dVal == MinValue1)
            {
                return "-";
            }
            else
            {
                return dVal.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<RptProductionMonthlyInspection> Gets(DateTime dFromDate, DateTime dToDate, string sFEOIDs, string sBuyerIDs, string  sFMIDs, long nUserID)
        {
            return RptProductionMonthlyInspection.Service.Gets(dFromDate, dToDate, sFEOIDs, sBuyerIDs, sFMIDs, nUserID);
        }
        public static List<RptProductionMonthlyInspection> Gets(string sSQL, long nUserID)
        {
            return RptProductionMonthlyInspection.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IRptProductionMonthlyInspectionService Service
        {
            get { return (IRptProductionMonthlyInspectionService)Services.Factory.CreateService(typeof(IRptProductionMonthlyInspectionService)); }
        }
        #endregion
    }
    #endregion

    #region IRptProductionMonthlyInspection interface
    public interface IRptProductionMonthlyInspectionService
    {
        List<RptProductionMonthlyInspection> Gets(DateTime dFromDate, DateTime dToDate, string sFEOIDs, string sBuyerIDs, string  sFMIDs, long nUserID);
        List<RptProductionMonthlyInspection> Gets(string sSQL, long nUserID);
    }
    #endregion
}
