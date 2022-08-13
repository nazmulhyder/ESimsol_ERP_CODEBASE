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
namespace ESimSol.BusinessObjects
{
    public class ReportRow
    {
        public ReportRow()
        {
            RowID = 0;
            RowObjTypeID = 0;
            RowObjName = "";
            TotalValue = 0;
        }

        #region Properties
        public int RowID { get; set; }
        public int RowObjTypeID { get; set; }
        public string RowObjName { get; set; }
        public double TotalValue { get; set; }
        #endregion

        #region Non DB Functions
        public static List<ReportRow> GetObjects(DataRowCollection oDataRows)
        {
            ReportRow oReportRow = new ReportRow();
            List<ReportRow> oReportRows = new List<ReportRow>();
            if (oDataRows != null)
            {
                foreach (DataRow oDataRow in oDataRows)
                {
                    oReportRow = new ReportRow();
                    oReportRow = ReportRow.GetObject(oDataRow);
                    oReportRows.Add(oReportRow);
                }
            }
            return oReportRows;
        }

        public static ReportRow GetObject(DataRow oDataRow)
        {
            ReportRow oReportRow = new ReportRow();
            oReportRow.RowID = (oDataRow.Table.Columns.Contains("RowID") && !oDataRow.IsNull("RowID")) ? Convert.ToInt32(oDataRow["RowID"]) : 0;
            oReportRow.RowObjTypeID = (oDataRow.Table.Columns.Contains("RowObjTypeID") && !oDataRow.IsNull("RowObjTypeID")) ? Convert.ToInt32(oDataRow["RowObjTypeID"]) : 0;
            oReportRow.RowObjName = (oDataRow.Table.Columns.Contains("RowObjName") && !oDataRow.IsNull("RowObjName")) ? Convert.ToString(oDataRow["RowObjName"]) : "";
            oReportRow.TotalValue = (oDataRow.Table.Columns.Contains("TotalValue") && !oDataRow.IsNull("TotalValue")) ? Convert.ToDouble(oDataRow["TotalValue"]) : 0;
            return oReportRow;
        }
        #endregion
    }


    public class ReportData
    {
        public ReportData()
        {
            DataID = 0;
            RowObjTypeID = 0;
            MonthID = 0;
            YearID = 0;
            MonthStartDate = DateTime.Today;
            MonthEndDate = DateTime.Today;
            ReportValue = 0;
        }

        #region Properties
        public int DataID { get; set; }
        public int RowObjTypeID { get; set; }
        public int MonthID { get; set; }
        public int YearID { get; set; }
        public DateTime MonthStartDate { get; set; }
        public DateTime MonthEndDate { get; set; }
        public double ReportValue { get; set; }
        #endregion

        #region Non DB Functions
        public static List<ReportData> GetObjects(DataRowCollection oDataRows)
        {
            ReportData oReportData = new ReportData();
            List<ReportData> oReportDatas = new List<ReportData>();
            if (oDataRows != null)
            {
                foreach (DataRow oDataRow in oDataRows)
                {
                    oReportData = new ReportData();
                    oReportData = ReportData.GetObject(oDataRow);
                    oReportDatas.Add(oReportData);
                }
            }
            return oReportDatas;
        }

        public static ReportData GetObject(DataRow oDataRow)
        {
            ReportData oReportData = new ReportData();
            oReportData.DataID = (oDataRow.Table.Columns.Contains("DataID") && !oDataRow.IsNull("DataID")) ? Convert.ToInt32(oDataRow["DataID"]) : 0;
            oReportData.RowObjTypeID = (oDataRow.Table.Columns.Contains("RowObjTypeID") && !oDataRow.IsNull("RowObjTypeID")) ? Convert.ToInt32(oDataRow["RowObjTypeID"]) : 0;
            oReportData.MonthID = (oDataRow.Table.Columns.Contains("MonthID") && !oDataRow.IsNull("MonthID")) ? Convert.ToInt32(oDataRow["MonthID"]) : 0;
            oReportData.YearID = (oDataRow.Table.Columns.Contains("YearID") && !oDataRow.IsNull("YearID")) ? Convert.ToInt32(oDataRow["YearID"]) : 0;
            oReportData.MonthStartDate = (oDataRow.Table.Columns.Contains("MonthStartDate") && !oDataRow.IsNull("MonthStartDate")) ? Convert.ToDateTime(oDataRow["MonthStartDate"]) : DateTime.MinValue;
            oReportData.MonthEndDate = (oDataRow.Table.Columns.Contains("MonthEndDate") && !oDataRow.IsNull("MonthEndDate")) ? Convert.ToDateTime(oDataRow["MonthEndDate"]) : DateTime.MinValue;
            oReportData.ReportValue = (oDataRow.Table.Columns.Contains("ReportValue") && !oDataRow.IsNull("ReportValue")) ? Convert.ToDouble(oDataRow["ReportValue"]) : 0;
            return oReportData;
        }
        #endregion
    }


    public class ReportMonth
    {
        public ReportMonth()
        {
            MonthPKID = 0;
            MonthID = 0;
            YearID = 0;
            NameofMonth = "";
            MonthDate = DateTime.Today;
            MonthStartDate = DateTime.Today;
            MonthEndDate = DateTime.Today;
            ReportColHeaderTex = "";
            TotalValue = 0;
        }

        #region Properties
        public int MonthPKID { get; set; }
        public int MonthID { get; set; }
        public int YearID { get; set; }
        public string NameofMonth { get; set; }
        public DateTime MonthDate { get; set; }
        public DateTime MonthStartDate { get; set; }
        public DateTime MonthEndDate { get; set; }
        public string ReportColHeaderTex { get; set; }
        public double TotalValue { get; set; }
        #endregion


        #region Non DB Functions
        public static List<ReportMonth> GetObjects(DataRowCollection oDataRows)
        {
            ReportMonth oReportMonth = new ReportMonth();
            List<ReportMonth> oReportMonths = new List<ReportMonth>();
            if (oDataRows != null)
            {
                foreach (DataRow oDataRow in oDataRows)
                {
                    oReportMonth = new ReportMonth();
                    oReportMonth = ReportMonth.GetObject(oDataRow);
                    oReportMonths.Add(oReportMonth);
                }
            }
            return oReportMonths;
        }

        public static ReportMonth GetObject(DataRow oDataRow)
        {
            ReportMonth oReportMonth = new ReportMonth();
            oReportMonth.MonthPKID = (oDataRow.Table.Columns.Contains("MonthPKID") && !oDataRow.IsNull("MonthPKID")) ? Convert.ToInt32(oDataRow["MonthPKID"]) : 0;
            oReportMonth.MonthID = (oDataRow.Table.Columns.Contains("MonthID") && !oDataRow.IsNull("MonthID")) ? Convert.ToInt32(oDataRow["MonthID"]) : 0;
            oReportMonth.YearID = (oDataRow.Table.Columns.Contains("YearID") && !oDataRow.IsNull("YearID")) ? Convert.ToInt32(oDataRow["YearID"]) : 0;
            oReportMonth.NameofMonth = (oDataRow.Table.Columns.Contains("NameofMonth") && !oDataRow.IsNull("NameofMonth")) ? Convert.ToString(oDataRow["NameofMonth"]) : "";
            oReportMonth.MonthDate = (oDataRow.Table.Columns.Contains("MonthDate") && !oDataRow.IsNull("MonthDate")) ? Convert.ToDateTime(oDataRow["MonthDate"]) : DateTime.MinValue;
            oReportMonth.MonthStartDate = (oDataRow.Table.Columns.Contains("MonthStartDate") && !oDataRow.IsNull("MonthStartDate")) ? Convert.ToDateTime(oDataRow["MonthStartDate"]) : DateTime.MinValue;
            oReportMonth.MonthEndDate = (oDataRow.Table.Columns.Contains("MonthEndDate") && !oDataRow.IsNull("MonthEndDate")) ? Convert.ToDateTime(oDataRow["MonthEndDate"]) : DateTime.MinValue;
            oReportMonth.ReportColHeaderTex = (oDataRow.Table.Columns.Contains("ReportColHeaderTex") && !oDataRow.IsNull("ReportColHeaderTex")) ? Convert.ToString(oDataRow["ReportColHeaderTex"]) : "";
            oReportMonth.TotalValue = (oDataRow.Table.Columns.Contains("TotalValue") && !oDataRow.IsNull("TotalValue")) ? Convert.ToDouble(oDataRow["TotalValue"]) : 0;
            return oReportMonth;
        }
        #endregion

    }
}
