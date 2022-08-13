using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.Services.Services
{
    public class CompliancePayrollProcessManagementService : MarshalByRefObject, ICompliancePayrollProcessManagementService
    {
        private CompliancePayrollProcessManagement MapObject(NullHandler oReader)
        {
            CompliancePayrollProcessManagement oCompliancePayrollProcessManagement = new CompliancePayrollProcessManagement();
            oCompliancePayrollProcessManagement.PPMID = oReader.GetInt32("PPMID");
            oCompliancePayrollProcessManagement.CompanyID = oReader.GetInt32("CompanyID");
            oCompliancePayrollProcessManagement.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oCompliancePayrollProcessManagement.LocationID = oReader.GetInt32("LocationID");
            oCompliancePayrollProcessManagement.EmpCount = oReader.GetInt32("EmpCount");
            oCompliancePayrollProcessManagement.DepartmentID = oReader.GetInt32("DepartmentID");
            oCompliancePayrollProcessManagement.YearID = oReader.GetInt32("YearID");
            oCompliancePayrollProcessManagement.MonthID = oReader.GetInt32("MonthID");            
            oCompliancePayrollProcessManagement.MOCID = oReader.GetInt32("MOCID");
            oCompliancePayrollProcessManagement.PaymentCycle = (EnumPaymentCycle)oReader.GetInt16("PaymentCycle");
            oCompliancePayrollProcessManagement.ProcessDate = oReader.GetDateTime("ProcessDate");
            oCompliancePayrollProcessManagement.SalaryFrom = oReader.GetDateTime("SalaryFrom");
            oCompliancePayrollProcessManagement.SalaryTo = oReader.GetDateTime("SalaryTo");
            oCompliancePayrollProcessManagement.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oCompliancePayrollProcessManagement.BUCode = oReader.GetString("BUCode");
            oCompliancePayrollProcessManagement.BUName = oReader.GetString("BUName");
            oCompliancePayrollProcessManagement.LocCode = oReader.GetString("LocCode");
            oCompliancePayrollProcessManagement.LocName = oReader.GetString("LocName");
            oCompliancePayrollProcessManagement.DeptCode = oReader.GetString("DeptCode");
            oCompliancePayrollProcessManagement.DeptName = oReader.GetString("DeptName");
            oCompliancePayrollProcessManagement.TimeCardName = oReader.GetString("TimeCardName");
            oCompliancePayrollProcessManagement.ApprovedByName = oReader.GetString("ApprovedByName");
            return oCompliancePayrollProcessManagement;
        }

        private CompliancePayrollProcessManagement CreateObject(NullHandler oReader)
        {
            CompliancePayrollProcessManagement oCompliancePayrollProcessManagement = new CompliancePayrollProcessManagement();
            oCompliancePayrollProcessManagement = MapObject(oReader);
            return oCompliancePayrollProcessManagement;
        }

        private List<CompliancePayrollProcessManagement> CreateObjects(IDataReader oReader)
        {
            List<CompliancePayrollProcessManagement> oCompliancePayrollProcessManagement = new List<CompliancePayrollProcessManagement>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CompliancePayrollProcessManagement oItem = CreateObject(oHandler);
                oCompliancePayrollProcessManagement.Add(oItem);
            }
            return oCompliancePayrollProcessManagement;
        }

        public List<CompliancePayrollProcessManagement> Gets(string sSQL, Int64 nUserID)
        {
            List<CompliancePayrollProcessManagement> oCompliancePayrollProcessManagement = new List<CompliancePayrollProcessManagement>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = CompliancePayrollProcessManagementDA.Gets(tc, sSQL);
                oCompliancePayrollProcessManagement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CompliancePayrollProcessManagement", e);
                #endregion
            }
            return oCompliancePayrollProcessManagement;
        }

        public List<CompliancePayrollProcessManagement> GetsRunningEmployeeBatchs(CompliancePayrollProcessManagement oCPPM, Int64 nUserID)
        {
            List<CompliancePayrollProcessManagement> oCompliancePayrollProcessManagements = new List<CompliancePayrollProcessManagement>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = CompliancePayrollProcessManagementDA.GetsRunningEmployeeBatchs(tc, oCPPM);
                oCompliancePayrollProcessManagements = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Gets Compliance Payroll Process Management", e);
                #endregion
            }
            return oCompliancePayrollProcessManagements;
        }

        public List<CompliancePayrollProcessManagement> GetsArchiveEmployeeBatchs(CompliancePayrollProcessManagement oCPPM, Int64 nUserID)
        {
            List<CompliancePayrollProcessManagement> oCompliancePayrollProcessManagements = new List<CompliancePayrollProcessManagement>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = CompliancePayrollProcessManagementDA.GetsArchiveEmployeeBatchs(tc, oCPPM);
                oCompliancePayrollProcessManagements = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Gets Compliance Payroll Process Management", e);
                #endregion
            }
            return oCompliancePayrollProcessManagements;
        }

        public CompliancePayrollProcessManagement CompPayRollProcess(CompliancePayrollProcessManagement oCPPM, string sBUIDs, string sLocationIDs, string sDepartmentIDs, Int64 nUserID)
        {
            CompliancePayrollProcessManagement oCompliancePayrollProcessManagement = new CompliancePayrollProcessManagement();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();                
                CompliancePayrollProcessManagementDA.CompPayRollProcess(tc, oCPPM, sBUIDs, sLocationIDs, sDepartmentIDs, nUserID);                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                tc.HandleError();                
                #endregion

                oCompliancePayrollProcessManagement = new CompliancePayrollProcessManagement();
                oCompliancePayrollProcessManagement.ErrorMessage = e.Message;
            }
            return oCompliancePayrollProcessManagement;
        }

        public string DeleteCompPayRollProcess(string sCPPMIDs, Int64 nUserID)
        {            
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                CompliancePayrollProcessManagementDA.DeleteCompPayRollProcess(tc, sCPPMIDs, nUserID, EnumDBOperation.Delete);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                #endregion

                return e.Message;
            }
            return Global.SuccessMessage;
        }
    }
}
