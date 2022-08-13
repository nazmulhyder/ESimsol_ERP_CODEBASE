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
    public class AMGSalarySheetService : MarshalByRefObject, IAMGSalarySheetService
    {
        #region Private functions and declaration
        private AMGSalarySheet MapObject(NullHandler oReader)
        {
            AMGSalarySheet oAMGSS = new AMGSalarySheet();
            oAMGSS.EmployeeSalaryID = oReader.GetInt32("EmployeeSalaryID");
            oAMGSS.EmployeeID = oReader.GetInt32("EmployeeID");
            oAMGSS.BUID = oReader.GetInt32("BUID");
            oAMGSS.LocationID = oReader.GetInt32("LocationID");
            oAMGSS.DepartmentID = oReader.GetInt32("DepartmentID");
            oAMGSS.DesignationID = oReader.GetInt32("DesignationID");
            oAMGSS.TotalDays = oReader.GetInt32("TotalDays");
            oAMGSS.DayOffHoliday = oReader.GetInt32("DayOffHoliday");
            oAMGSS.EWD = oReader.GetInt32("EWD");
            oAMGSS.LWP = oReader.GetInt32("LWP");
            oAMGSS.CL = oReader.GetInt32("CL");
            oAMGSS.EL = oReader.GetInt32("EL");
            oAMGSS.SL = oReader.GetInt32("SL");
            oAMGSS.ML = oReader.GetInt32("ML");
            oAMGSS.Absent = oReader.GetInt32("A");
            oAMGSS.Present = oReader.GetInt32("P");
            oAMGSS.Gross = oReader.GetDouble("Gross");
            oAMGSS.Basics = oReader.GetDouble("Basics");
            oAMGSS.HR = oReader.GetDouble("HR");
            oAMGSS.Med = oReader.GetDouble("Med");
            oAMGSS.Food = oReader.GetDouble("Food");
            oAMGSS.Conv = oReader.GetDouble("Conv");
            oAMGSS.Earning = oReader.GetDouble("Earning");
            oAMGSS.AttBonus = oReader.GetDouble("AttBonus");
            oAMGSS.OT_HR = oReader.GetDouble("OT_HR");
            oAMGSS.OT_Rate = oReader.GetDouble("OT_Rate");
            oAMGSS.OT_Amount = oReader.GetDouble("OT_Amount");
            oAMGSS.ExtraOTHR = oReader.GetDouble("ExtraOTHR");
            oAMGSS.ExtraOTAmount = oReader.GetDouble("ExtraOTAmount");
            oAMGSS.GrossEarning = oReader.GetDouble("GrossEarning");
            oAMGSS.AbsentAmount = oReader.GetDouble("AbsentAmount");
            oAMGSS.Advance = oReader.GetDouble("Advance");
            oAMGSS.Stemp = oReader.GetInt32("Stemp");
            oAMGSS.Welfare = oReader.GetInt32("Welfare");
            oAMGSS.TotalDeduction = oReader.GetDouble("TotalDeduction");
            oAMGSS.NetAmount = oReader.GetDouble("NetAmount");
            oAMGSS.Code = oReader.GetString("Code");
            oAMGSS.Name = oReader.GetString("Name");
            oAMGSS.DOJ = oReader.GetDateTime("DOJ");
            oAMGSS.Grade = oReader.GetString("Grade");
            oAMGSS.BUName = oReader.GetString("BUName");
            oAMGSS.LocName = oReader.GetString("LocName");
            oAMGSS.DptName = oReader.GetString("DptName");
            oAMGSS.BlockName = oReader.GetString("BlockName");
            oAMGSS.DsgName = oReader.GetString("DsgName");
            oAMGSS.AccountNo = oReader.GetString("AccountNo");
            oAMGSS.BankName = oReader.GetString("BankName");
            oAMGSS.BUAddress = oReader.GetString("BUAddress");
            oAMGSS.EmpNameInBangla = oReader.GetString("EmpNameInBangla");
            oAMGSS.DsgNameInBangla = oReader.GetString("DsgNameInBangla");
            oAMGSS.GroupByID = oReader.GetInt32("GroupByID");
            oAMGSS.StartDate = oReader.GetDateTime("StartDate");
            oAMGSS.EndDate = oReader.GetDateTime("EndDate");
            return oAMGSS;
        }

        private AMGSalarySheet CreateObject(NullHandler oReader)
        {
            AMGSalarySheet oAMGSalarySheet = new AMGSalarySheet();
            oAMGSalarySheet = MapObject(oReader);
            return oAMGSalarySheet;
        }

        private List<AMGSalarySheet> CreateObjects(IDataReader oReader)
        {
            List<AMGSalarySheet> oAMGSalarySheet = new List<AMGSalarySheet>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AMGSalarySheet oItem = CreateObject(oHandler);
                oAMGSalarySheet.Add(oItem);
            }
            return oAMGSalarySheet;
        }
        #endregion

        #region Interface implementation
        public AMGSalarySheetService() { }

        public List<AMGSalarySheet> Gets(string BUIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int nMonthID, int nYear, bool IsNewJoin, int IsOutSheet, double nMinSalary, double nMaxSalary, string sGroupIDs, string sBlockIDs, bool IsComp, Int64 nUserId)
        {
            List<AMGSalarySheet> oAMGSalarySheets = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AMGSalarySheetDA.Gets(tc, BUIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, nMonthID, nYear, IsNewJoin, IsOutSheet, nMinSalary, nMaxSalary, sGroupIDs, sBlockIDs, IsComp, nUserId);
                oAMGSalarySheets = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new ServiceException("Failed to get salary sheet info.", e);
                #endregion
            }

            return oAMGSalarySheets;
        }
        public List<AMGSalarySheet> GetsComp(string BUIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int nMonthID, int nYear, bool IsNewJoin, int IsOutSheet, double nMinSalary, double nMaxSalary, string sGroupIDs, string sBlockIDs, int nTimeCardID, Int64 nUserId)
        {
            List<AMGSalarySheet> oAMGSalarySheets = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AMGSalarySheetDA.GetsComp(tc, BUIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, nMonthID, nYear, IsNewJoin, IsOutSheet, nMinSalary, nMaxSalary, sGroupIDs, sBlockIDs, nTimeCardID, nUserId);
                oAMGSalarySheets = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new ServiceException("Failed to get salary sheet info.", e);
                #endregion
            }

            return oAMGSalarySheets;
        }
        public List<AMGSalarySheet> GetsPaySlip(string BUIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int nMonthID, int nYear, bool IsNewJoin, int IsOutSheet, double nMinSalary, double nMaxSalary, string sGroupIDs, string sBlockIDs, int nMOCID, Int64 nUserId)
        {
            List<AMGSalarySheet> oAMGSalarySheets = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AMGSalarySheetDA.GetsPaySlip(tc, BUIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, nMonthID, nYear, IsNewJoin, IsOutSheet, nMinSalary, nMaxSalary, sGroupIDs, sBlockIDs, nMOCID);
                oAMGSalarySheets = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new ServiceException("Failed to get salary sheet info.", e);
                #endregion
            }

            return oAMGSalarySheets;
        }


        #endregion
    }


}
