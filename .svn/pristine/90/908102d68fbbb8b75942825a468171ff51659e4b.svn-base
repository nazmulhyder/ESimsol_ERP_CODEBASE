using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using System.Data;
using System.Dynamic;
namespace ESimSol.BusinessObjects
{
    public class ExportSummary : BusinessObject
    {
        public ExportSummary()
        {
            BUID = 0;
            ReportName = EnumExportSummaryReportName.None;
            ReportNameInt = 0;
            ReportType = EnumExportSummaryReportType.None;
            ReportTypeInt = 0;
            Reportlayout = EnumExportSummaryReportLayout.None;
            ReportlayoutInt = 0;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            CompareFromMonths = "";
            CompareWithMonths = "";
            ErrorMessage = "";
            FooterRow = new ExpandoObject();
            ExportSummarys = new List<ExpandoObject>();
            ReportMonths = new List<ReportMonth>();
            ReportRows = new List<ReportRow>();
            ReportDatas = new List<ReportData>();
            DataCarrier = null;
            BaseColumnCaption = "";
            TotalColumnCaption = "";
        }
        #region properties        
        public int BUID { get; set; }
        public EnumExportSummaryReportName ReportName { get; set; }
        public int ReportNameInt { get; set; }
        public EnumExportSummaryReportType ReportType { get; set; }
        public int ReportTypeInt { get; set; }
        public EnumExportSummaryReportLayout Reportlayout { get; set; }
        public int ReportlayoutInt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CompareFromMonths { get; set; }
        public string CompareWithMonths { get; set; }
        public string BaseColumnCaption { get; set; }
        public string TotalColumnCaption { get; set; }
        public string ErrorMessage { get; set; }        
        #endregion


        #region Derived Property
        public DataSet DataCarrier { get; set; }
        public ExpandoObject FooterRow { get; set; }
        public List<ExpandoObject> ExportSummarys { get; set; }
        public List<ReportMonth> ReportMonths { get; set; }
        public List<ReportData> ReportDatas { get; set; }
        public List<ReportRow> ReportRows { get; set; }
        public string StartDateSt
        {
            get
            {
                if (this.StartDate == DateTime.MinValue)
                {
                    return "";
                }
                else 
                {
                    return this.StartDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string EndDateSt
        {
            get
            {
                if (this.EndDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.EndDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion

        public static ExportSummary GetsExportSummary(ExportSummary oExportSummary, long nUserID)
        {
            return ExportSummary.Service.GetsExportSummary(oExportSummary, nUserID);
        }

        #region ServiceFactory

        internal static IExportSummaryService Service
        {
            get { return (IExportSummaryService)Services.Factory.CreateService(typeof(IExportSummaryService)); }
        }

        #endregion
    }
    public interface IExportSummaryService
    {
        ExportSummary GetsExportSummary(ExportSummary oExportSummary, long nUserID);
    }
}
