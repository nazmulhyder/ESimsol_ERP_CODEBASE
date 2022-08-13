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
    public class ELEncashReportService : MarshalByRefObject, IELEncashReportService
    {
        #region Private functions and declaration
        private ELEncashReport MapObject(NullHandler oReader)
        {
            ELEncashReport oELEncashReport = new ELEncashReport();
            oELEncashReport.EmployeeID = oReader.GetInt32("EmployeeID");
            oELEncashReport.DepartmentID = oReader.GetInt32("DepartmentID");
            oELEncashReport.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oELEncashReport.LocationID = oReader.GetInt32("LocationID");
            oELEncashReport.EmployeeCode = oReader.GetString("EmployeeCode");
            oELEncashReport.EmployeeName = oReader.GetString("EmployeeName");
            oELEncashReport.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oELEncashReport.BusinessUnitAddress = oReader.GetString("BusinessUnitAddress");
            oELEncashReport.LocationName = oReader.GetString("LocationName");
            oELEncashReport.DepartmentName = oReader.GetString("DepartmentName");
            oELEncashReport.DesignationName = oReader.GetString("DesignationName");
            oELEncashReport.Joining = oReader.GetDateTime("Joining");
            oELEncashReport.Gross = oReader.GetDouble("Gross");
            oELEncashReport.TotalDays = oReader.GetDouble("TotalDays");
            oELEncashReport.Amount = oReader.GetDouble("Amount");
            oELEncashReport.DeclarationDate = oReader.GetDateTime("DeclarationDate");
            oELEncashReport.TotalEL = oReader.GetInt32("TotalEL");
            oELEncashReport.Enjoyed = oReader.GetInt32("Enjoyed");
            oELEncashReport.EncashedEL = oReader.GetDouble("EncashedEL");
            oELEncashReport.EncashedELAmount = oReader.GetDouble("EncashedELAmount");
            oELEncashReport.Stamp = oReader.GetDouble("Stamp");
            oELEncashReport.NetPayable = oReader.GetDouble("NetPayable");
           
            return oELEncashReport;
        }

        private ELEncashReport CreateObject(NullHandler oReader)
        {
            ELEncashReport oELEncashReport = MapObject(oReader);
            return oELEncashReport;
        }

        private List<ELEncashReport> CreateObjects(IDataReader oReader)
        {
            List<ELEncashReport> oELEncashReport = new List<ELEncashReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ELEncashReport oItem = CreateObject(oHandler);
                oELEncashReport.Add(oItem);
            }
            return oELEncashReport;
        }

        #endregion

        #region Interface implementation
        public ELEncashReportService() { }
        public List<ELEncashReport> Gets(string sELEncashIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIDs, string sEmployeeIDs, int nACSID, bool IsDeclarationDate, DateTime DeclarationDate, double nStartSalary, double nEndSalary, Int64 nUserID)
        {
            List<ELEncashReport> oELEncashReports = new List<ELEncashReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EarnLeaveReportDA.Gets(tc, sELEncashIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIDs, sEmployeeIDs, nACSID, IsDeclarationDate, DeclarationDate, nStartSalary, nEndSalary, nUserID);
                oELEncashReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oELEncashReports = new List<ELEncashReport>();
                ELEncashReport oELEncashReport = new ELEncashReport();
                oELEncashReport.ErrorMessage = e.Message;
                oELEncashReports.Add(oELEncashReport);
                #endregion
            }
            return oELEncashReports;
        }
        #endregion
    }
}
