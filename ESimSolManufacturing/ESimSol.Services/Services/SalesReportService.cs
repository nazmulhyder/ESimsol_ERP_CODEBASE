using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class SalesReportService : MarshalByRefObject, ISalesReportService
    {
        #region Private functions and declaration
        private SalesReport MapObject(NullHandler oReader)
        {
            SalesReport oSalesReport = new SalesReport();
            
            oSalesReport.SalesReportID = oReader.GetInt32("SalesReportID");
            oSalesReport.Name = oReader.GetString("Name");
            oSalesReport.PrintName = oReader.GetString("PrintName");
            oSalesReport.Query = oReader.GetString("Query");
            oSalesReport.ReportType = oReader.GetInt32("ReportType");
            oSalesReport.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oSalesReport.LastUpdateDateTime = Convert.ToDateTime(oReader.GetDateTime("LastUpdateDateTime"));
            oSalesReport.LastUpdateByName = oReader.GetString("LastUpdateByName");
            oSalesReport.IDs = oReader.GetString("IDs");
            oSalesReport.GrpByQ = oReader.GetString("GrpByQ");
            oSalesReport.BUID = oReader.GetInt32("BUID");
            oSalesReport.BusinessUnitType = (EnumBusinessUnitType)oReader.GetInt32("BusinessUnitType");
            oSalesReport.Activity = oReader.GetBoolean("Activity");
            oSalesReport.IsDouble = oReader.GetInt32("IsDouble");
            oSalesReport.ParentID = oReader.GetInt32("ParentID");
            oSalesReport.AllocationHeader = oReader.GetString("AllocationHeader");
            oSalesReport.DispoTargetQuery = oReader.GetString("TargetQuery");
            oSalesReport.QueryLayerTwo = oReader.GetString("QueryLayerTwo");
            oSalesReport.QueryLayerThree = oReader.GetString("QueryLayerThree");
            oSalesReport.RefDate = oReader.GetDateTime("RefDate");
            oSalesReport.RefNo = oReader.GetString("RefNo");
            oSalesReport.QTYDC = oReader.GetDouble("QTYDC");
            oSalesReport.Amount = oReader.GetDouble("Amount");
            oSalesReport.ID = oReader.GetInt32("ID");
            oSalesReport.Note = oReader.GetString("Note");
           

            /// For Report
            oSalesReport.RefID = oReader.GetInt32("RefID");
            oSalesReport.RefName = oReader.GetString("RefName");
            oSalesReport.GroupName = oReader.GetString("GroupName");
            oSalesReport.Value = oReader.GetDouble("Value");
            oSalesReport.Day = oReader.GetInt32("Day");
            oSalesReport.Month = oReader.GetInt32("Month");
            oSalesReport.Year = oReader.GetInt32("Year");

            oSalesReport.Qty = oReader.GetDouble("Qty");
            oSalesReport.Count = oReader.GetInt32("Count");
            oSalesReport.MUnitName = oReader.GetString("MUnit");
            oSalesReport.Currency = oReader.GetString("Currency");
            oSalesReport.Symbol = oReader.GetString("Symbol");

            return oSalesReport;
        }
        private SalesReport CreateObject(NullHandler oReader)
        {
            SalesReport oSalesReport = new SalesReport();
            oSalesReport = MapObject(oReader);
            return oSalesReport;
        }

        private List<SalesReport> CreateObjects(IDataReader oReader)
        {
            List<SalesReport> oSalesReport = new List<SalesReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalesReport oItem = CreateObject(oHandler);
                oSalesReport.Add(oItem);
            }
            return oSalesReport;
        }
        #endregion
        #region Interface implementation
        public SalesReport Save(SalesReport oSalesReport, Int64 nUserID)
        {
            string sRefChildIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSalesReport.SalesReportID <= 0)
                {

                    reader = SalesReportDA.InsertUpdate(tc, oSalesReport, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = SalesReportDA.InsertUpdate(tc, oSalesReport, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesReport = new SalesReport();
                    oSalesReport = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oSalesReport = new SalesReport();
                    oSalesReport.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oSalesReport;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SalesReport oSalesReport = new SalesReport();
                oSalesReport.SalesReportID = id;
                DBTableReferenceDA.HasReference(tc, "SalesReport", id);
                SalesReportDA.Delete(tc, oSalesReport, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public SalesReport Get(int id, Int64 nUserId)
        {
            SalesReport oSalesReport = new SalesReport();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = SalesReportDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesReport = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalesReport", e);
                #endregion
            }
            return oSalesReport;
        }
        public List<SalesReport> Gets(Int64 nUserID)
        {
            List<SalesReport> oSalesReports = new List<SalesReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SalesReportDA.Gets(tc);
                oSalesReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                SalesReport oSalesReport = new SalesReport();
                oSalesReport.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oSalesReports;
        }
        public List<SalesReport> Gets(string sSQL, Int64 nUserID)
        {
            List<SalesReport> oSalesReports = new List<SalesReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SalesReportDA.Gets(tc, sSQL);
                oSalesReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalesReport", e);
                #endregion
            }
            return oSalesReports;
        }
        public List<SalesReport> BUWiseGets(int nBUID, Int64 nUserId)
        {
            List<SalesReport> oSalesReport = new List<SalesReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SalesReportDA.BUWiseGets(tc, nBUID);
                oSalesReport = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BUnit", e);
                #endregion
            }

            return oSalesReport;

        }
        public SalesReport GetByParent(int ParentID, Int64 nUserId)
        {
            SalesReport oSalesReport = new SalesReport();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = SalesReportDA.GetByParent(tc, ParentID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesReport = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalesReport", e);
                #endregion
            }
            return oSalesReport;
        }

        public SalesReport Activate(SalesReport oSalesReport, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = SalesReportDA.InsertUpdate(tc, oSalesReport, EnumDBOperation.Active, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesReport = new SalesReport();
                    oSalesReport = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oSalesReport = new SalesReport();
                    oSalesReport.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oSalesReport;
        }
        public SalesReport InActivate(SalesReport oSalesReport, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = SalesReportDA.InsertUpdate(tc, oSalesReport, EnumDBOperation.InActive, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesReport = new SalesReport();
                    oSalesReport = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oSalesReport = new SalesReport();
                    oSalesReport.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oSalesReport;
        }
        

        #endregion
    }
}
