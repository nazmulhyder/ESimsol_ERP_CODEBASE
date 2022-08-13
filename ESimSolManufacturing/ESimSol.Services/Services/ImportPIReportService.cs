using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class ImportPIReportService : MarshalByRefObject, IImportPIReportService
    {
        #region Private functions and declaration
        private ImportPIReport MapObject(NullHandler oReader)
        {
            ImportPIReport oImportPIReport = new ImportPIReport();
            oImportPIReport.ImportPIID = oReader.GetInt32("ImportPIID");
            oImportPIReport.ImportPINo = oReader.GetString("ImportPINo");
            oImportPIReport.ContractorID = oReader.GetInt32("ContractorID");
            oImportPIReport.ContractorName = oReader.GetString("ContractorName");
            oImportPIReport.ImportPIType = (EnumImportPIType)oReader.GetInt16("ImportPIType");
            oImportPIReport.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oImportPIReport.MUnit = oReader.GetString("MUnit");
            oImportPIReport.ImportPIDate = oReader.GetDateTime("ImportPIDate");
            oImportPIReport.ImportPIDetailID = oReader.GetInt32("ImportPIDetailID");
            oImportPIReport.ProductID = oReader.GetInt32("ProductID");
            oImportPIReport.ProductName = oReader.GetString("ProductName");
            oImportPIReport.ProductCode = oReader.GetString("ProductCode");
            oImportPIReport.UnitPrice = oReader.GetDouble("UnitPrice");
            oImportPIReport.Quantity = oReader.GetDouble("Quantity");
            oImportPIReport.PCStatus = (EnumImportPIState)oReader.GetInt16("PCStatus");
            
            return oImportPIReport;

        }

        private ImportPIReport CreateObject(NullHandler oReader)
        {
            ImportPIReport oImportPIReport = MapObject(oReader);
            return oImportPIReport;
        }

        private List<ImportPIReport> CreateObjects(IDataReader oReader)
        {
            List<ImportPIReport> oImportPIReport = new List<ImportPIReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportPIReport oItem = CreateObject(oHandler);
                oImportPIReport.Add(oItem);
            }
            return oImportPIReport;
        }

        #endregion

        #region Interface implementation
        public ImportPIReportService() { }


        public List<ImportPIReport> Gets(string sSQL, Int64 nUserId)
        {
            List<ImportPIReport> oImportPIReports = new List<ImportPIReport>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIReportDA.Gets(tc,sSQL);
                oImportPIReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                //#region Handle Exception
                //if (tc != null)
                //    tc.HandleError();
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get View_ImportPIReport", e);
                //#endregion
                oImportPIReports = new List<ImportPIReport>();
                ImportPIReport oImportPIReport = new ImportPIReport();
                oImportPIReport.ErrorMessage = e.Message;
                oImportPIReports.Add(oImportPIReport);
            }
            return oImportPIReports;
        }

        #endregion

    }
}
