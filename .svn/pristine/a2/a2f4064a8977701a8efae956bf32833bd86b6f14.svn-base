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
    public class RPTSalarySheetService : MarshalByRefObject, IRPTSalarySheetService
    {
        #region Private functions and declaration
        private RPTSalarySheet MapObject(NullHandler oReader)
        {
            RPTSalarySheet oEmployeeSalary = new RPTSalarySheet();
            oEmployeeSalary.EmployeeSalaryID = oReader.GetInt32("EmployeeSalaryID");
            oEmployeeSalary.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeSalary.LocationID = oReader.GetInt32("LocationID");
            oEmployeeSalary.DepartmentID = oReader.GetInt32("DepartmentID");
            oEmployeeSalary.DesignationID = oReader.GetInt32("DesignationID");
            oEmployeeSalary.GrossAmount = oReader.GetDouble("GrossAmount");
            oEmployeeSalary.SalaryDate = oReader.GetDateTime("SalaryDate");
            oEmployeeSalary.StartDate = oReader.GetDateTime("StartDate");
            oEmployeeSalary.EndDate = oReader.GetDateTime("EndDate");

            oEmployeeSalary.OTHour = oReader.GetDouble("OT_HR");
            oEmployeeSalary.OTRatePerHour = oReader.GetDouble("OT_Rate");
            oEmployeeSalary.TotalAbsent = oReader.GetInt32("TotalAbsent");
            oEmployeeSalary.TotalLate = oReader.GetInt32("TotalLate");
            oEmployeeSalary.TotalEarlyLeaving = oReader.GetInt32("TotalEarlyLeaving");
            oEmployeeSalary.TotalDayOff = oReader.GetInt32("TotalDayOff");
            oEmployeeSalary.TotalHoliday = oReader.GetInt32("TotalHoliday");
            oEmployeeSalary.TotalUpLeave = oReader.GetInt32("TotalUpLeave");
            oEmployeeSalary.TotalPLeave = oReader.GetInt32("TotalPLeave");
            oEmployeeSalary.RevenueStemp = oReader.GetInt32("RevenueStemp");

            oEmployeeSalary.LateInMin = oReader.GetInt32("LateInMin");
            //derive
            oEmployeeSalary.EmployeeName = oReader.GetString("EmployeeName");
            oEmployeeSalary.EmployeeNameInBangla = oReader.GetString("EmployeeNameInBangla");
            oEmployeeSalary.EmployeeCode = oReader.GetString("EmployeeCode");
            oEmployeeSalary.LocationName = oReader.GetString("LocationName");
            oEmployeeSalary.DepartmentName = oReader.GetString("DepartmentName");
            oEmployeeSalary.ParentDepartmentName = oReader.GetString("ParentDepartmentName");
            oEmployeeSalary.DesignationName = oReader.GetString("DesignationName");
            oEmployeeSalary.JoiningDate = oReader.GetDateTime("JoiningDate");
            oEmployeeSalary.DateOfConfirmation = oReader.GetDateTime("DateOfConfirmation");
            oEmployeeSalary.EmployeeTypeName = oReader.GetString("EmployeeTypeName");
            oEmployeeSalary.Gender = oReader.GetString("Gender");
            oEmployeeSalary.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oEmployeeSalary.BUName = oReader.GetString("BUName");
            oEmployeeSalary.BlockName = oReader.GetString("BlockName");

            oEmployeeSalary.ContactNo = oReader.GetString("ContactNo");


            oEmployeeSalary.CashAmount = oReader.GetDouble("CashAmount");

            return oEmployeeSalary;
        }

        private RPTSalarySheet CreateObject(NullHandler oReader)
        {
            RPTSalarySheet oEmployeeSalary = MapObject(oReader);
            return oEmployeeSalary;
        }

        private List<RPTSalarySheet> CreateObjects(IDataReader oReader)
        {
            List<RPTSalarySheet> oEmployeeSalary = new List<RPTSalarySheet>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RPTSalarySheet oItem = CreateObject(oHandler);
                oEmployeeSalary.Add(oItem);
            }
            return oEmployeeSalary;
        }

        #endregion

        #region Interface implementation
        public RPTSalarySheetService() { }

        public RPTSalarySheet Get(int nEmployeeSalaryID, Int64 nUserId)
        {
            RPTSalarySheet oEmployeeSalary = new RPTSalarySheet();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RPTSalarySheetDA.Get(nEmployeeSalaryID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalary = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeSalary", e);
                oEmployeeSalary.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSalary;
        }

        public RPTSalarySheet Get(string sSql, Int64 nUserId)
        {
            RPTSalarySheet oEmployeeSalary = new RPTSalarySheet();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RPTSalarySheetDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalary = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeSalary", e);
                oEmployeeSalary.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSalary;
        }

        public List<RPTSalarySheet> Gets(Int64 nUserID)
        {
            List<RPTSalarySheet> oEmployeeSalary = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RPTSalarySheetDA.Gets(tc);
                oEmployeeSalary = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_EmployeeSalary", e);
                #endregion
            }
            return oEmployeeSalary;
        }

        public List<RPTSalarySheet> Gets(string sSQL, Int64 nUserID)
        {
            List<RPTSalarySheet> oEmployeeSalary = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RPTSalarySheetDA.Gets(sSQL, tc);
                oEmployeeSalary = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_EmployeeSalary", e);
                #endregion
            }
            return oEmployeeSalary;
        }

        public List<RPTSalarySheet> GetEmployeesSalary(string sBU, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, string sBlockIDs, string sGroupIDs, string sEmpIDs, int nMonthID, int nYear, bool bNewJoin, int IsOutSheet, double nStartSalaryRange, double nEndSalaryRange, bool IsCompliance, int nPayType, bool IsMacthExact, int BankAccID)
        {
            List<RPTSalarySheet> oEmployeeSalarys = new List<RPTSalarySheet>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RPTSalarySheetDA.GetEmployeesSalary(tc, sBU, sLocationID, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, sBlockIDs, sGroupIDs, sEmpIDs, nMonthID, nYear, bNewJoin, IsOutSheet, nStartSalaryRange, nEndSalaryRange, IsCompliance, nPayType, IsMacthExact, BankAccID);
                
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {

                    RPTSalarySheet oESS = new RPTSalarySheet();
                    oESS.EmployeeSalaryID = oreader.GetInt32("EmployeeSalaryID");
                    oESS.EmployeeID = oreader.GetInt32("EmployeeID");
                    oESS.BusinessUnitID = oreader.GetInt32("BUID");
                    oESS.LocationID = oreader.GetInt32("LocationID");
                    oESS.DepartmentID = oreader.GetInt32("DepartmentID");
                    oESS.DesignationID = oreader.GetInt32("DesignationID");

                    oESS.TotalDayOff = oreader.GetInt32("DayOffHoliday");
                    oESS.TotalAbsent = oreader.GetInt32("A");
                    oESS.Present = oreader.GetInt32("P");
                    oESS.TotalLate = oreader.GetInt32("TotalLate");
                    oESS.TotalLeave = oreader.GetInt32("TotalLeave");
                    oESS.TotalEarlyLeaving = oreader.GetInt32("TotalEarly");
                    oESS.GrossAmount = oreader.GetDouble("Gross");
                    oESS.OTHour = oreader.GetDouble("OT_HR");
                    oESS.OTRatePerHour = oreader.GetDouble("OT_Rate");
                    oESS.OTAmount = oreader.GetDouble("OT_Amount");
                    oESS.NetAmount = oreader.GetDouble("NetAmount");
                    oESS.TotalDays = oreader.GetInt32("TotalDays");
                    oESS.EarlyInMin = oreader.GetInt32("EarlyInMin");
                    oESS.LateInMin = oreader.GetInt32("LateInMin");
                    oESS.EWD = oreader.GetInt32("EWD");
                    oESS.TotalPLeave = oreader.GetInt32("TotalPLeave");
                    oESS.TotalUpLeave = oreader.GetInt32("TotalUpLeave");
                    oESS.TotalHoliday = oreader.GetInt32("TotalHoliday");
                    oESS.TotalDOff = oreader.GetInt32("TotalDOff");
                    oESS.LastGross = oreader.GetDouble("LastGross");
                    oESS.IncrementAmount = oreader.GetDouble("IncrementAmount");
                    oESS.EffectedDate = oreader.GetDateTime("EffectedDate");
                    oESS.StartDate = oreader.GetDateTime("StartDate");
                    oESS.EndDate = oreader.GetDateTime("EndDate");


                    oESS.ContactNo = oreader.GetString("ContactNo");
                    //derive
                    oESS.EmployeeName = oreader.GetString("Name");
                    oESS.Gender = oreader.GetString("GENDER");                    
                    oESS.EmployeeCode = oreader.GetString("Code");
                    oESS.EmployeeTypeName = oreader.GetString("Grade");
                    oESS.EmpGroup = oreader.GetString("EmpGroup");
                    oESS.PaymentType = oreader.GetString("PaymentType");
                    oESS.LocationName = oreader.GetString("LocName");
                    oESS.DepartmentName = oreader.GetString("DptName");
                    oESS.ParentDepartmentName = oreader.GetString("ParentDeptName");
                    oESS.BUName = oreader.GetString("BUName");
                    oESS.DesignationName = oreader.GetString("DsgName");
                    oESS.BlockName = oreader.GetString("BlockName");

                    oESS.AccountNo = oreader.GetString("AccountNo");
                    oESS.BankName = oreader.GetString("BankName");
                    oESS.EmpNameInBangla = oreader.GetString("EmpNameInBangla");
                    oESS.DsgNameInBangla = oreader.GetString("DsgNameInBangla");
                    oESS.BUAddress = oreader.GetString("BUAddress");



                    oESS.JoiningDate = oreader.GetDateTime("DOJ");
                    oESS.DateOfConfirmation = oreader.GetDateTime("DOC");

                    oESS.CashAmount = oreader.GetDouble("CashAmount");
                    oESS.BankAmount = oreader.GetDouble("BankAmount");
                    oEmployeeSalarys.Add(oESS);
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
                throw new ServiceException("Failed to Get View_EmployeeSalary", e);
                #endregion
            }
            return oEmployeeSalarys;
        }

        #endregion

    }
}

