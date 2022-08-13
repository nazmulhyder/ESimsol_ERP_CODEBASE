using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.Services.DataAccess.ReportingDA;


namespace ESimSol.Services.Services.ReportingService
{
    public class ExportLCReportService : MarshalByRefObject, IExportLCReportService
    {
        #region Private functions and declaration
        
        #region Detail
        private ExportLCReport MapObject(NullHandler oReader)
        {
            ExportLCReport oExportLCReport = new ExportLCReport();
            oExportLCReport.BUID = oReader.GetInt32("BUID");
            oExportLCReport.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportLCReport.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportLCReport.LCNo = oReader.GetString("LCNo");
            oExportLCReport.VersionNo = oReader.GetInt32("VersionNo");
            oExportLCReport.LCOpenDate = oReader.GetDateTime("LCOpenDate");
            oExportLCReport.AmendmentDate = oReader.GetDateTime("AmendmentDate");
            oExportLCReport.LCReceiveDate = oReader.GetDateTime("LCReceiveDate");
            oExportLCReport.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oExportLCReport.ExpiryDate = oReader.GetDateTime("ExpiryDate");
            oExportLCReport.LCStatus = (EnumExportLCStatus)oReader.GetInt32("LCStatus");
            oExportLCReport.PINo = oReader.GetString("PINo");
            oExportLCReport.PIIssueDate = oReader.GetDateTime("PIIssueDate");
            oExportLCReport.ContractorID = oReader.GetInt32("BuyerID");
            oExportLCReport.MKTEmpID = oReader.GetInt32("MKTEmpID");
            oExportLCReport.ProductID = oReader.GetInt32("ProductID");
            oExportLCReport.MUnitID = oReader.GetInt32("MUnitID");
            oExportLCReport.Qty = oReader.GetDouble("Qty");
            oExportLCReport.UnitPrice = oReader.GetDouble("UnitPrice");
            oExportLCReport.Amount = oReader.GetDouble("Amount");
            oExportLCReport.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportLCReport.ProductCode = oReader.GetString("ProductCode");
            oExportLCReport.MUnitName = oReader.GetString("MUnitName");
            oExportLCReport.BuyerName = oReader.GetString("BuyerName");
            oExportLCReport.ProductName = oReader.GetString("ProductName");
            oExportLCReport.MUSymbol = oReader.GetString("MUSymbol");
            oExportLCReport.ContractorName = oReader.GetString("ContractorName");
            oExportLCReport.BuyerName = oReader.GetString("BuyerName");
            oExportLCReport.LAStDeliveryDate = oReader.GetDateTime("LAStDeliveryDate");
            oExportLCReport.MKTPersonName = oReader.GetString("MKTPersonName");
            oExportLCReport.MKTPName = oReader.GetString("MKTPName");
            oExportLCReport.Currency = oReader.GetString("Currency");
            oExportLCReport.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oExportLCReport.BankName_Nego = oReader.GetString("BankName_Nego");
            oExportLCReport.BankName_Issue = oReader.GetString("BankName_Issue");
            oExportLCReport.BUName = oReader.GetString("BUName");
            oExportLCReport.MasterLCNos = oReader.GetString("MasterLCNos");
            oExportLCReport.DateYear = oReader.GetDouble("DateYear");
            oExportLCReport.DateMonth = oReader.GetDouble("DateMonth");


            oExportLCReport.BankBranchID_Advice = oReader.GetInt32("BankBranchID_Advice");
            oExportLCReport.BankBranchID_Issue = oReader.GetInt32("BankBranchID_Issue");
            oExportLCReport.BankBranchID_NegoTiation = oReader.GetInt32("BankBranchID_NegoTiation");
            oExportLCReport.FileNo = oReader.GetString("FileNo");
            oExportLCReport.BuyerID = oReader.GetInt32("BuyerID");
            oExportLCReport.Qty_DC = oReader.GetDouble("Qty_DC");
            oExportLCReport.Amount_DC = oReader.GetDouble("Amount_DC");
            oExportLCReport.Qty_Bill = oReader.GetDouble("Qty_Bill");
            oExportLCReport.Amount_Bill = oReader.GetDouble("Amount_Bill");
            oExportLCReport.BuyerAcc = oReader.GetInt32("BuyerAcc");
            oExportLCReport.BankAcc = oReader.GetInt32("BankAcc");
            oExportLCReport.NoteQuery = oReader.GetString("NoteQuery");
            oExportLCReport.NoteUD = oReader.GetString("NoteUD");
            oExportLCReport.UDRcvType = oReader.GetInt32("UDRcvType");
            oExportLCReport.GetOriginalCopy = oReader.GetBoolean("GetOriginalCopy");
            oExportLCReport.LCTermsName = oReader.GetString("LCTermsName");
            








            return oExportLCReport;
        }
        private ExportLCReport CreateObject(NullHandler oReader)
        {
            ExportLCReport oExportLCReport = new ExportLCReport();
            oExportLCReport = MapObject(oReader);
            return oExportLCReport;
        }       
        private List<ExportLCReport> CreateObjects(IDataReader oReader)
        {
            List<ExportLCReport> oExportLCReports = new List<ExportLCReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportLCReport oItem = CreateObject(oHandler);
                oExportLCReports.Add(oItem);
            }
            return oExportLCReports;
        }

#endregion
        #region LC Wise
        private ExportLCReport MapObject_LC(NullHandler oReader)
        {
            ExportLCReport oExportLCReport = new ExportLCReport();
            oExportLCReport.BUID = oReader.GetInt32("BUID");
            oExportLCReport.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportLCReport.LCNo = oReader.GetString("LCNo");
            oExportLCReport.LCOpenDate = oReader.GetDateTime("LCOpenDate");
            oExportLCReport.LCStatus = (EnumExportLCStatus)oReader.GetInt32("LCStatus");
            oExportLCReport.ContractorID = oReader.GetInt32("ContractorID");
            oExportLCReport.Qty = oReader.GetDouble("Qty");
            //oExportLCReport.UnitPrice = oReader.GetDouble("UnitPrice");
            oExportLCReport.Amount = oReader.GetDouble("Amount");
            oExportLCReport.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportLCReport.MUSymbol = oReader.GetString("MUSymbol");
            oExportLCReport.ContractorName = oReader.GetString("ContractorName");
            oExportLCReport.MKTPersonName = oReader.GetString("MKTPersonName");
            oExportLCReport.MKTPName = oReader.GetString("MKTPName");
            oExportLCReport.Currency = oReader.GetString("Currency");
            oExportLCReport.BankName_Nego = oReader.GetString("BankName_Nego");
            oExportLCReport.BankName_Issue = oReader.GetString("BankName_Issue");
            oExportLCReport.BUName = oReader.GetString("BUName");
            oExportLCReport.MasterLCNos = oReader.GetString("MasterLCNos");
            oExportLCReport.Currency = oReader.GetString("Currency");
            oExportLCReport.MUnitName = oReader.GetString("MUnitName");
            return oExportLCReport;
        }
        private ExportLCReport CreateObject_LC(NullHandler oReader)
        {
            ExportLCReport oExportLCReport = new ExportLCReport();
            oExportLCReport = MapObject_LC(oReader);
            return oExportLCReport;
        }
        private List<ExportLCReport> CreateObjects_LC(IDataReader oReader)
        {
            List<ExportLCReport> oExportLCReports = new List<ExportLCReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportLCReport oItem = CreateObject_LC(oHandler);
                oExportLCReports.Add(oItem);
            }
            return oExportLCReports;
        }

        #endregion
        #region LC Version  Wise
        private ExportLCReport MapObject_LCVersion(NullHandler oReader)
        {
            ExportLCReport oExportLCReport = new ExportLCReport();
            oExportLCReport.BUID = oReader.GetInt32("BUID");
            oExportLCReport.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportLCReport.LCNo = oReader.GetString("LCNo");
            oExportLCReport.VersionNo = oReader.GetInt32("VersionNo");
            oExportLCReport.LCOpenDate = oReader.GetDateTime("LCOpenDate");
            oExportLCReport.AmendmentDate = oReader.GetDateTime("AmendmentDate");
            oExportLCReport.LCReceiveDate = oReader.GetDateTime("LCReceiveDate");
            oExportLCReport.LCStatus = (EnumExportLCStatus)oReader.GetInt32("LCStatus");
            oExportLCReport.ContractorID = oReader.GetInt32("ContractorID");
            oExportLCReport.MKTEmpID = oReader.GetInt32("MKTEmpID");
            oExportLCReport.Qty = oReader.GetDouble("Qty");
            //oExportLCReport.UnitPrice = oReader.GetDouble("UnitPrice");
            oExportLCReport.Amount = oReader.GetDouble("Amount");
            oExportLCReport.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportLCReport.MUSymbol = oReader.GetString("MUSymbol");
            oExportLCReport.ContractorName = oReader.GetString("ContractorName");
            oExportLCReport.MKTPersonName = oReader.GetString("MKTPersonName");
            oExportLCReport.MKTPName = oReader.GetString("MKTPName");
            oExportLCReport.Currency = oReader.GetString("Currency");
            oExportLCReport.MasterLCNos = oReader.GetString("MasterLCNos");
            oExportLCReport.BankName_Nego = oReader.GetString("BankName_Nego");
            oExportLCReport.BankName_Issue = oReader.GetString("BankName_Issue");
            oExportLCReport.BUName = oReader.GetString("BUName");
            return oExportLCReport;
        }
        private ExportLCReport CreateObject_LCVersion(NullHandler oReader)
        {
            ExportLCReport oExportLCReport = new ExportLCReport();
            oExportLCReport = MapObject_LCVersion(oReader);
            return oExportLCReport;
        }
        private List<ExportLCReport> CreateObjects_LCVersion(IDataReader oReader)
        {
            List<ExportLCReport> oExportLCReports = new List<ExportLCReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportLCReport oItem = CreateObject_LCVersion(oHandler);
                oExportLCReports.Add(oItem);
            }
            return oExportLCReports;
        }

        #endregion
        #region PI
        private ExportLCReport MapObject_PI(NullHandler oReader)
        {
            ExportLCReport oExportLCReport = new ExportLCReport();
            oExportLCReport.BUID = oReader.GetInt32("BUID");
            oExportLCReport.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportLCReport.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportLCReport.LCNo = oReader.GetString("LCNo");
            oExportLCReport.VersionNo = oReader.GetInt32("VersionNo");
           oExportLCReport.LCOpenDate = oReader.GetDateTime("LCOpenDate");
            oExportLCReport.AmendmentDate = oReader.GetDateTime("AmendmentDate");
            oExportLCReport.LCReceiveDate = oReader.GetDateTime("LCReceiveDate");
            oExportLCReport.LCStatus = (EnumExportLCStatus)oReader.GetInt32("LCStatus");
            oExportLCReport.PINo = oReader.GetString("PINo");
            oExportLCReport.PIIssueDate = oReader.GetDateTime("PIIssueDate");
            oExportLCReport.ContractorID = oReader.GetInt32("BuyerID");
            oExportLCReport.MKTEmpID = oReader.GetInt32("MKTEmpID");
            //oExportLCReport.ProductID = oReader.GetInt32("ProductID");
            oExportLCReport.MUnitID = oReader.GetInt32("MUnitID");
            oExportLCReport.Qty = oReader.GetDouble("Qty");
            //oExportLCReport.UnitPrice = oReader.GetDouble("UnitPrice");
            oExportLCReport.Amount = oReader.GetDouble("Amount");
            oExportLCReport.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportLCReport.ProductCode = oReader.GetString("ProductCode");
            oExportLCReport.ProductName = oReader.GetString("ProductName");
            oExportLCReport.MUSymbol = oReader.GetString("MUSymbol");
            oExportLCReport.ContractorName = oReader.GetString("ContractorName");
            //oExportLCReport.LAStDeliveryDate = oReader.GetDateTime("LAStDeliveryDate");
            oExportLCReport.MKTPersonName = oReader.GetString("MKTPersonName");
            oExportLCReport.Currency = oReader.GetString("Currency");
            oExportLCReport.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oExportLCReport.BankName_Nego = oReader.GetString("BankName_Nego");
            oExportLCReport.BankName_Issue = oReader.GetString("BankName_Issue");
            oExportLCReport.BUName = oReader.GetString("BUName");
            oExportLCReport.MasterLCNos = oReader.GetString("MasterLCNos");
            return oExportLCReport;
        }
        private ExportLCReport CreateObject_PI(NullHandler oReader)
        {
            ExportLCReport oExportLCReport = new ExportLCReport();
            oExportLCReport = MapObject_PI(oReader);
            return oExportLCReport;
        }
        private List<ExportLCReport> CreateObjects_PI(IDataReader oReader)
        {
            List<ExportLCReport> oExportLCReports = new List<ExportLCReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportLCReport oItem = CreateObject_PI(oHandler);
                oExportLCReports.Add(oItem);
            }
            return oExportLCReports;
        }

        #endregion
        #endregion

        #region Interface implementation
        public ExportLCReportService() { }
        public ExportLCReport Get(int id, Int64 nUserId)
        {
            ExportLCReport oExportLCReport = new ExportLCReport();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ExportLCReportDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportLCReport = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportLCReport", e);
                #endregion
            }
            return oExportLCReport;
        }
        public List<ExportLCReport> Gets(Int64 nUserId)
        {
            List<ExportLCReport> oExportLCReports = new List<ExportLCReport>(); ;
            ExportLCReport oExportLCReport = new ExportLCReport();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportLCReportDA.Gets(tc);
                oExportLCReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oExportLCReport.ErrorMessage = ex.Message;
                oExportLCReports = new List<ExportLCReport>();
                oExportLCReports.Add(oExportLCReport);
                #endregion
            }
            return oExportLCReports;
        }

        public List<ExportLCReport> Gets(string sSQL, EnumLCReportLevel eLCReportLevel, Int64 nUserId)
        {
            List<ExportLCReport> oExportLCReports = new List<ExportLCReport>(); ;
            ExportLCReport oExportLCReport= new ExportLCReport();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportLCReportDA.Gets(tc, sSQL, eLCReportLevel);
                if (eLCReportLevel == EnumLCReportLevel.LCLevel)
                {
                    oExportLCReports = CreateObjects_LC(reader);
                }
                else if (eLCReportLevel == EnumLCReportLevel.LCVersionLevel)
                {
                    oExportLCReports = CreateObjects_LCVersion(reader);
                }
                else if (eLCReportLevel == EnumLCReportLevel.PILevel)
                {
                    oExportLCReports = CreateObjects_PI(reader);
                }
                else 
                {
                    oExportLCReports = CreateObjects(reader);
                }
                
                
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oExportLCReport.ErrorMessage = ex.Message;
                oExportLCReports = new List<ExportLCReport>();
                oExportLCReports.Add(oExportLCReport);
                #endregion
            }
            return oExportLCReports;
        }

        public List<ExportLCReport> Gets(string sSQL, Int64 nUserId)
        {
            List<ExportLCReport> oExportLCReports = new List<ExportLCReport>(); ;
            ExportLCReport oExportLCReport = new ExportLCReport();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportLCReportDA.Gets(tc, sSQL);
                oExportLCReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oExportLCReport.ErrorMessage = ex.Message;
                oExportLCReports = new List<ExportLCReport>();
                oExportLCReports.Add(oExportLCReport);
                #endregion
            }
            return oExportLCReports;
        }

        public List<ExportLCReport> GetsReportProduct(ExportLCReport oExportLCReport, Int64 nUserId)
        {
            List<ExportLCReport> oExportLCReports = new List<ExportLCReport>(); 
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportLCReportDA.GetsReportProduct(tc, oExportLCReport);
                oExportLCReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oExportLCReport= new ExportLCReport();
                oExportLCReport.ErrorMessage = ex.Message;
                oExportLCReports = new List<ExportLCReport>();
                oExportLCReports.Add(oExportLCReport);
                #endregion
            }
            return oExportLCReports;
        }

        public List<ExportLCReport> GetsReport(string sSQL, int nReportType, long nUserID)
        {
            List<ExportLCReport> oExportLCReports = new List<ExportLCReport>(); ;
            ExportLCReport oExportLCReport = new ExportLCReport();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportLCReportDA.GetsReport(tc, sSQL, nReportType, nUserID);
                oExportLCReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oExportLCReport.ErrorMessage = ex.Message;
                oExportLCReports = new List<ExportLCReport>();
                oExportLCReports.Add(oExportLCReport);
                #endregion
            }
            return oExportLCReports;
        }
        #endregion
    }
}
