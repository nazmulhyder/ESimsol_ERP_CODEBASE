 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    public class SalesReport
    {
        public SalesReport()
        {
            RefName = "";
            GroupName = "";
            Value = 0;
            Name = "";
            PrintName = "";
            Query = "";
            ReportType = 0;
            ErrorMessage = "";
            SalesReportID = 0;
            LastUpdateBy = 0;
            DateYear = 0;
            LastUpdateDateTime = DateTime.Now;
            LastUpdateByName = "";
            Day = 0;
            Month = 0;
            Year = 0;
            RefID = 0;
            Total = 0;
            BusinessUnitType = EnumBusinessUnitType.None;
            IDs = "";
            GrpByQ = "";
            BUID = 0;
            Activity = true;
            IsDouble = 0;
            ParentID = 0;
            ViewType = 0;
            MUnitName = "";
            Currency = "";
            Qty = 0;
            Count = 0;
            AllocationHeader = "";
            DispoTargetQuery = "";
            QueryLayerTwo = "";
            QueryLayerThree = "";
            Note = "";
            Symbol = "";

            //LAYER 3
            RefDate = DateTime.Now;
            RefNo = "";
            QTYDC = 0;
            Amount = 0;
            ID = 0;

            oSalesReports = new List<SalesReport>();
            _oSalesReports = new List<SalesReport>();


        }
        #region Properties
        public int SalesReportID { get; set; }
        public string Name { get; set; }
        public string PrintName { get; set; }
        public string Query { get; set; }
        public string IDs { get; set; } 
        public string GrpByQ { get; set; }
        public int ReportType { get; set; }
        public int ViewType { get; set; }
        public int BUID { get; set; }
        public EnumBusinessUnitType BusinessUnitType { get; set; }
        public int LastUpdateBy { get; set; }
        public int DateYear { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string LastUpdateByName { get; set; }
        public bool Activity { get; set; }
        public int IsDouble { get; set; }
        public int ParentID { get; set; }
        public string AllocationHeader { get; set; }
        public string DispoTargetQuery { get; set; }
        public string QueryLayerTwo { get; set; }
        public string QueryLayerThree { get; set; }
        public string MUnitName { get; set; }
        public string Currency { get; set; }
        public double Qty { get; set; }
        public int Count { get; set; }

        public int RefID { get; set; }
        public string RefName { get; set; }
        public string GroupName { get; set; }
        public double Value { get; set; }
        public int Day { get; set; }
        public int  Month { get; set; }
        public int Year { get; set; }
        public string ErrorMessage { get; set; }
        public double Total { get; set; }
        public string Note { get; set; }
        public string Symbol { get; set; }

        #region Layer3 Property
        public DateTime RefDate { get; set; }
        public string RefNo { get; set; }
        public double QTYDC { get; set; }
        public double Amount { get; set; }
        public int ID { get; set; }
        #endregion

        #endregion
        #region Functions
        public static List<SalesReport> Gets(long nUserID)
        {
            return SalesReport.Service.Gets(nUserID);
        }
        public static List<SalesReport> Gets(string sSQL, long nUserID)
        {
            return SalesReport.Service.Gets(sSQL, nUserID);
        }
        public SalesReport Get(int id, long nUserID)
        {
            return SalesReport.Service.Get(id, nUserID);
        }
        public SalesReport Save(long nUserID)
        {
            return SalesReport.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return SalesReport.Service.Delete(id, nUserID);
        }
        public static List<SalesReport> BUWiseGets(int nBUID, Int64 nUserID)
        {
            return SalesReport.Service.BUWiseGets(nBUID, nUserID);
        }
        public static SalesReport GetByParent(int parentID, Int64 nUserID)
        {
            return SalesReport.Service.GetByParent(parentID, nUserID);
        }
        public SalesReport Activate(long nUserID)
        {
            return SalesReport.Service.Activate(this, nUserID);
        }
        public SalesReport InActivate(long nUserID)
        {
            return SalesReport.Service.InActivate(this, nUserID);
        }
        public List<SalesReport> oSalesReports { get; set; }
        public List<SalesReport> _oSalesReports { get; set; }


        #endregion
        #region Derived Property
        public string MonthInST
        {
            get
            {
                if (this.Month == 1)
                {
                    return "January";
                }
                if (this.Month == 2)
                {
                    return "February";
                }
                if (this.Month == 3)
                {
                    return "March";
                }
                if (this.Month == 4)
                {
                    return "April";
                }
                if (this.Month == 5)
                {
                    return "May";
                }
                if (this.Month == 6)
                {
                    return "June";
                }
                if (this.Month == 7)
                {
                    return "July";
                }
                if (this.Month == 8)
                {
                    return "August";
                }
                if (this.Month == 9)
                {
                    return "September";
                }
                if (this.Month == 10)
                {
                    return "October";
                }
                 if (this.Month == 11)
                {
                    return "November";
                }
                 if (this.Month == 12)
                 {
                     return "December";
                 }
                 else
                 {
                     return "";
                 }
            }
        }
        public string DayInST
        {
            get
            {
                return "Day " + this.Day;
            }
        }
        public string ActivityInString
        {
            get
            {
                if (this.Activity == true)
                {
                    return "Active";
                }
                else
                {
                    return "InActive";
                }
            }
        }
        public string IsDoubleInString
        {
            get
            {
                if (this.IsDouble == 1)
                {
                    return "Single";
                }
                else if (this.IsDouble == 2)
                {
                    return "Double";
                }
                else
                {
                    return "";
                }
            }
        }
        public string ValueST
        {
            get
            {
                return this.Currency + "" + this.Value.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string LastUpdateDateTimeInString
        {
            get
            {
                return LastUpdateDateTime.ToString("dd MMM yyyy");
            }
        }
        public string RefDateInString
        {
            get
            {
                return RefDate.ToString("dd MMM yyyy");
            }
        }
        public string BUType
        {
            get
            {
             
               return EnumObject.jGet(this.BusinessUnitType);
              
            }
        }
        public string TotalST { get; set; }
      
        #endregion
        #region ServiceFactory
        internal static ISalesReportService Service
        {
            get { return (ISalesReportService)Services.Factory.CreateService(typeof(ISalesReportService)); }
        }
        #endregion
    }
    #region ISalesReport interface
    public interface ISalesReportService
    {
        SalesReport Get(int id, Int64 nUserID);
        List<SalesReport> Gets(Int64 nUserID);
        List<SalesReport> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        SalesReport Save(SalesReport oSalesReport, Int64 nUserID);
        List<SalesReport> BUWiseGets(int nBUID, Int64 nUserID);
        SalesReport GetByParent(int parentID, Int64 nUserID);
        SalesReport Activate(SalesReport oSalesReport, Int64 nUserID);
        SalesReport InActivate(SalesReport oSalesReport, Int64 nUserID);
    }
    #endregion
}
