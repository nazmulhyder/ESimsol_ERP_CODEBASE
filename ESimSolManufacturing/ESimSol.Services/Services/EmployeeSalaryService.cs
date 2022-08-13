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
    public class EmployeeSalaryService : MarshalByRefObject, IEmployeeSalaryService
    {
        #region Private functions and declaration
        private EmployeeSalary MapObject(NullHandler oReader)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            oEmployeeSalary.EmployeeSalaryID = oReader.GetInt32("EmployeeSalaryID");
            oEmployeeSalary.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeSalary.LocationID = oReader.GetInt32("LocationID");
            oEmployeeSalary.DepartmentID = oReader.GetInt32("DepartmentID");
            oEmployeeSalary.DesignationID = oReader.GetInt32("DesignationID");
            oEmployeeSalary.GrossAmount = oReader.GetDouble("GrossAmount");
            oEmployeeSalary.Allowance = oReader.GetDouble("Allowance");
            oEmployeeSalary.NetAmount = oReader.GetInt32("NetAmount");
            oEmployeeSalary.CurrencyID = oReader.GetInt32("CurrencyID");
            oEmployeeSalary.SalaryDate = oReader.GetDateTime("SalaryDate");
            oEmployeeSalary.SalaryReceiveDate = oReader.GetDateTime("SalaryReceiveDate");
            oEmployeeSalary.PayrollProcessID = oReader.GetInt32("PayrollProcessID");
            oEmployeeSalary.IsManual = oReader.GetBoolean("IsManual");
            oEmployeeSalary.IsOutSheet = oReader.GetBoolean("IsOutSheet");
            oEmployeeSalary.StartDate = oReader.GetDateTime("StartDate");
            oEmployeeSalary.EndDate = oReader.GetDateTime("EndDate");
            oEmployeeSalary.IsLock = oReader.GetBoolean("IsLock");

            oEmployeeSalary.ProductionAmount = oReader.GetDouble("ProductionAmount");
            oEmployeeSalary.ProductionBonus = oReader.GetDouble("ProductionBonus");
            oEmployeeSalary.OTHour = oReader.GetDouble("OTHour");
            oEmployeeSalary.OTRatePerHour = oReader.GetDouble("OTRatePerHour");
            oEmployeeSalary.TotalWorkingDay = oReader.GetInt32("TotalWorkingDay");
            oEmployeeSalary.TotalAbsent = oReader.GetInt32("TotalAbsent");
            oEmployeeSalary.TotalLate = oReader.GetInt32("TotalLate");
            oEmployeeSalary.TotalEarlyLeaving = oReader.GetInt32("TotalEarlyLeaving");
            oEmployeeSalary.EarlyLeavingMinute = oReader.GetInt32("EarlyLeavingMinute");
            oEmployeeSalary.TotalDayOff = oReader.GetInt32("TotalDayOff");
            oEmployeeSalary.TotalHoliday = oReader.GetInt32("TotalHoliday");
            oEmployeeSalary.TotalUpLeave = oReader.GetInt32("TotalUpLeave");
            oEmployeeSalary.TotalPLeave = oReader.GetInt32("TotalPLeave");
            oEmployeeSalary.RevenueStemp = oReader.GetInt32("RevenueStemp");
            oEmployeeSalary.TotalNoWorkDay = oReader.GetInt32("TotalNoWorkDay");
            oEmployeeSalary.TotalNoWorkDayAllowance = oReader.GetDouble("TotalNoWorkDayAllowance");
            oEmployeeSalary.AddShortFall = oReader.GetDouble("AddShortFall");

            oEmployeeSalary.CompOTHour = oReader.GetDouble("CompOTHour");
            oEmployeeSalary.CompOTRatePerHour = oReader.GetDouble("CompOTRatePerHour");
            oEmployeeSalary.CompNetAmount = oReader.GetDouble("CompNetAmount");

            oEmployeeSalary.CompGrossAmount = oReader.GetDouble("CompGrossAmount");
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
            oEmployeeSalary.IsProductionBase = oReader.GetBoolean("IsProductionBase");
            oEmployeeSalary.EmployeeTypeName = oReader.GetString("EmployeeTypeName");
            oEmployeeSalary.Gender = oReader.GetString("Gender");
            oEmployeeSalary.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oEmployeeSalary.BUName = oReader.GetString("BUName");
            oEmployeeSalary.BMMID = oReader.GetInt32("BMMID");
            oEmployeeSalary.BlockName = oReader.GetString("BlockName");
            oEmployeeSalary.ETIN = oReader.GetString("ETIN");


            oEmployeeSalary.CompTotalAbsent = oReader.GetInt32("CompTotalAbsent");
            oEmployeeSalary.CompTotalDayOff = oReader.GetInt32("CompTotalDayOff");
            oEmployeeSalary.CompTotalHoliday = oReader.GetInt32("CompTotalHoliday");
            oEmployeeSalary.CompTotalLeave = oReader.GetInt32("CompTotalLeave");
            oEmployeeSalary.CompTotalLate = oReader.GetInt32("CompTotalLate");
            oEmployeeSalary.CompTotalEarlyLeave = oReader.GetInt32("CompTotalEarlyLeave");
            oEmployeeSalary.CompLateInMin = oReader.GetInt32("CompLateInMin");
            oEmployeeSalary.CompTotalWorkingDay = oReader.GetInt32("CompTotalWorkingDay");

            oEmployeeSalary.CashAmount = oReader.GetDouble("CashAmount");
            oEmployeeSalary.BankAmount = oReader.GetDouble("BankAmount");

            oEmployeeSalary.MOCID = oReader.GetInt32("MOCID");

            return oEmployeeSalary;
        }

        private EmployeeSalary CreateObject(NullHandler oReader)
        {
            EmployeeSalary oEmployeeSalary = MapObject(oReader);
            return oEmployeeSalary;
        }

        private List<EmployeeSalary> CreateObjects(IDataReader oReader)
        {
            List<EmployeeSalary> oEmployeeSalary = new List<EmployeeSalary>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeSalary oItem = CreateObject(oHandler);
                oEmployeeSalary.Add(oItem);
            }
            return oEmployeeSalary;
        }

        #endregion

        #region Interface implementation
        public EmployeeSalaryService() { }

        public EmployeeSalary IUD(EmployeeSalary oEmployeeSalary, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeSalaryDA.IUD(tc, oEmployeeSalary, nUserID, nDBOperation);
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
                oEmployeeSalary.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeSalary.EmployeeSalaryID = 0;
                #endregion
            }
            return oEmployeeSalary;
        }


        public EmployeeSalary ProcessSalaryComp(EmployeeSalary oEmployeeSalary, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeSalaryDA.ProcessSalaryComp(tc, oEmployeeSalary, nUserID);
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
                oEmployeeSalary.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeSalary.EmployeeSalaryID = 0;
                #endregion
            }
            return oEmployeeSalary;
        }


        public EmployeeSalary ProcessSalary(EmployeeSalary oEmployeeSalary, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeSalaryDA.ProcessSalary(tc, oEmployeeSalary, nUserID);
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
                oEmployeeSalary.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeSalary.EmployeeSalaryID = 0;
                #endregion
            }
            return oEmployeeSalary;
        }


        public EmployeeSalary Get(int nEmployeeSalaryID, Int64 nUserId)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSalaryDA.Get(nEmployeeSalaryID, tc);
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

        public EmployeeSalary Get(string sSql, Int64 nUserId)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSalaryDA.Get(sSql, tc);
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

        public List<EmployeeSalary> Gets(Int64 nUserID)
        {
            List<EmployeeSalary> oEmployeeSalary = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryDA.Gets(tc);
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

        public List<EmployeeSalary> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeSalary> oEmployeeSalary = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryDA.Gets(sSQL, tc);
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

        public bool UpdateOutSheet(string sSQL, Int64 nUserID)
        {
            List<EmployeeSalary> oEmployeeSalary = null;
            bool bSuccess = false;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryDA.UpdateOutSheet(sSQL, tc);
                oEmployeeSalary = CreateObjects(reader);
                if (reader.RecordsAffected != -1)
                {
                    bSuccess = true;
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                bSuccess = false;
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Update", e);
                #endregion
            }
            return bSuccess;
        }

        public List<EmployeeSalary> Gets_ZN(string sEmpIDs, string sDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, Int64 nUserID)
        {
            List<EmployeeSalary> oEmployeeSalarys = new List<EmployeeSalary>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryDA.Gets_ZN(sEmpIDs, sDate, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {

                    EmployeeSalary oEmployeeSalary = new EmployeeSalary();
                    oEmployeeSalary.EmployeeSalaryID = oreader.GetInt32("EmployeeSalaryID");
                    oEmployeeSalary.EmployeeID = oreader.GetInt32("EmployeeID");
                    oEmployeeSalary.LocationID = oreader.GetInt32("LocationID");
                    oEmployeeSalary.DepartmentID = oreader.GetInt32("DepartmentID");
                    oEmployeeSalary.DesignationID = oreader.GetInt32("DesignationID");
                    oEmployeeSalary.GrossAmount = oreader.GetDouble("GrossAmount");
                    oEmployeeSalary.NetAmount = oreader.GetInt32("NetAmount");
                    oEmployeeSalary.NetAmount_ZN = oreader.GetInt32("NetAmount_ZN");
                    oEmployeeSalary.CurrencyID = oreader.GetInt32("CurrencyID");
                    oEmployeeSalary.SalaryDate = oreader.GetDateTime("SalaryDate");
                    oEmployeeSalary.SalaryReceiveDate = oreader.GetDateTime("SalaryReceiveDate");
                    oEmployeeSalary.PayrollProcessID = oreader.GetInt32("PayrollProcessID");
                    oEmployeeSalary.IsManual = oreader.GetBoolean("IsManual");
                    oEmployeeSalary.StartDate = oreader.GetDateTime("StartDate");
                    oEmployeeSalary.EndDate = oreader.GetDateTime("EndDate");
                    oEmployeeSalary.IsLock = oreader.GetBoolean("IsLock");

                    oEmployeeSalary.ProductionAmount = oreader.GetDouble("ProductionAmount");
                    oEmployeeSalary.ProductionBonus = oreader.GetDouble("ProductionBonus");
                    oEmployeeSalary.OTHour = oreader.GetDouble("OTHour");
                    oEmployeeSalary.OTHr_2ndPortion = oreader.GetDouble("OTHr_2ndPortion");
                    oEmployeeSalary.OTHr_Rest = oreader.GetDouble("OTHr_Rest");

                    oEmployeeSalary.OTRatePerHour = oreader.GetDouble("OTRatePerHour");
                    oEmployeeSalary.TotalWorkingDay = oreader.GetInt32("TotalWorkingDay");
                    oEmployeeSalary.TotalAbsent = oreader.GetInt32("TotalAbsent");
                    oEmployeeSalary.TotalLate = oreader.GetInt32("TotalLate");
                    oEmployeeSalary.TotalEarlyLeaving = oreader.GetInt32("TotalEarlyLeaving");
                    oEmployeeSalary.TotalDayOff = oreader.GetInt32("TotalDayOff");
                    oEmployeeSalary.TotalHoliday = oreader.GetInt32("TotalHoliday");
                    oEmployeeSalary.TotalUpLeave = oreader.GetInt32("TotalUpLeave");
                    oEmployeeSalary.TotalPLeave = oreader.GetInt32("TotalPLeave");
                    oEmployeeSalary.RevenueStemp = oreader.GetInt32("RevenueStemp");
                    oEmployeeSalary.TotalNoWorkDay = oreader.GetInt32("TotalNoWorkDay");
                    oEmployeeSalary.TotalNoWorkDayAllowance = oreader.GetDouble("TotalNoWorkDayAllowance");
                    oEmployeeSalary.AddShortFall = oreader.GetDouble("AddShortFall");

                    //derive
                    oEmployeeSalary.EmployeeName = oreader.GetString("EmployeeName");
                    oEmployeeSalary.EmployeeNameInBangla = oreader.GetString("EmployeeNameInBangla");
                    oEmployeeSalary.EmployeeCode = oreader.GetString("EmployeeCode");
                    oEmployeeSalary.LocationName = oreader.GetString("LocationName");
                    oEmployeeSalary.DepartmentName = oreader.GetString("DepartmentName");
                    oEmployeeSalary.DesignationName = oreader.GetString("DesignationName");
                    oEmployeeSalary.JoiningDate = oreader.GetDateTime("JoiningDate");
                    oEmployeeSalary.DateOfConfirmation = oreader.GetDateTime("DateOfConfirmation");
                    oEmployeeSalary.IsProductionBase = oreader.GetBoolean("IsProductionBase");
                    oEmployeeSalary.IsAllowOverTime = oreader.GetBoolean("IsAllowOverTime");
                    oEmployeeSalary.AllowanceNameWithDay = oreader.GetString("AllowanceNameWithDay");
                    oEmployeeSalary.EmployeeTypeName = oreader.GetString("EmployeeTypeName");
                    //oEmployeeSalary.CashAmount = oreader.GetDouble("CashAmount");
                    oEmployeeSalarys.Add(oEmployeeSalary);
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

        public List<EmployeeSalary> GetsComparisonReport(string BUIDs, string LocIDs, string DeptIDs, string DesignationIDs, string SchemeIDs, string EmpIDs, bool isMonthWise, int MonthFrom, int YearFrom, int MonthTo, int YearTo, int ComparisonYearFrom, int ComparisonYearTo, double MinSalary, double MaxSalary, string GroupIDs, string BlockIDs, int GroupBy, Int64 nUserID)
        {
            List<EmployeeSalary> oEmployeeSalarys = new List<EmployeeSalary>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryDA.GetsComparisonReport(BUIDs, LocIDs, DeptIDs, DesignationIDs, SchemeIDs, EmpIDs, isMonthWise, MonthFrom, YearFrom, MonthTo, YearTo, ComparisonYearFrom, ComparisonYearTo, MinSalary, MaxSalary, GroupIDs, BlockIDs, GroupBy, nUserID, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {

                    EmployeeSalary oEmployeeSalary = new EmployeeSalary();



                    oEmployeeSalary.BusinessUnitID = oreader.GetInt32("BusinessUnitID");
                    oEmployeeSalary.LocationID = oreader.GetInt32("LocationID");
                    oEmployeeSalary.DepartmentID = oreader.GetInt32("DepartmentID");

                    oEmployeeSalary.BUName = oreader.GetString("BusinessUnitName");
                    oEmployeeSalary.LocationName = oreader.GetString("LocationName");
                    oEmployeeSalary.DepartmentName = oreader.GetString("DepartmentName");

                    oEmployeeSalary.NoOfEmployee = oreader.GetInt32("NoOfEmp");
                    oEmployeeSalary.GrossAmount = oreader.GetDouble("GrossSalary");
                    oEmployeeSalary.GrossOnEWD = oreader.GetDouble("GrossSalaryOnPresent");
                    oEmployeeSalary.OTHour = oreader.GetDouble("OTHr");
                    oEmployeeSalary.OTAmount = oreader.GetDouble("OTAmount");
                    oEmployeeSalary.TotalPayable = oreader.GetDouble("TotalPayable");

                    oEmployeeSalary.MonthID = oreader.GetInt32("MonthID");
                    oEmployeeSalary.Year = oreader.GetInt32("Year");
                    

                    oEmployeeSalarys.Add(oEmployeeSalary);
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

        public List<EmployeeSalary> GetsPayRollBreakDown(DateTime StartDate, DateTime EndDate, bool IsDateSearch,int nLocationID, Int64 nUserId)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            List<EmployeeSalary> oEmployeeSalarys = new List<EmployeeSalary>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSalaryDA.GetPayRollBreakDown(StartDate,EndDate,IsDateSearch,nLocationID, tc);
                NullHandler oReader = new NullHandler(reader);
                while (reader.Read())
                {
                    oEmployeeSalary = new EmployeeSalary();
                    oEmployeeSalary.DepartmentID = Convert.ToInt16(reader["DepartmentID"]);
                    oEmployeeSalary.DepartmentName = Convert.ToString(reader["DepartmentName"]);
                    oEmployeeSalary.Salary = Convert.ToDouble(reader["Salary"]);
                    oEmployeeSalary.Allowance = Convert.ToDouble(reader["Allowance"]);
                    oEmployeeSalary.Overtime = Convert.ToDouble(reader["Overtime"]);
                    oEmployeeSalarys.Add(oEmployeeSalary);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oEmployeeSalarys = new List<EmployeeSalary>();
                oEmployeeSalary.ErrorMessage = (ex.Message.Contains("!"))? ex.Message.Split('!')[0]:ex.Message;
                oEmployeeSalarys.Add(oEmployeeSalary);
                #endregion
            }

            return oEmployeeSalarys;
        }

        public List<EmployeeSalary> GetsEmployeeSettleemtSalary(string sSQL, Int64 nUserID)
        {
            List<EmployeeSalary> oEmployeeSalarys = new List<EmployeeSalary>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeSalaryDA.Gets(sSQL, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    EmployeeSalary oItem = new EmployeeSalary();
                    oItem.EmployeeSalaryID = oreader.GetInt32("EmployeeSalaryID");
                    oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                    oItem.LocationID = oreader.GetInt32("LocationID");
                    oItem.DepartmentID = oreader.GetInt32("DepartmentID");
                    oItem.DesignationID = oreader.GetInt32("DesignationID");
                    oItem.GrossAmount = oreader.GetDouble("GrossAmount");
                    oItem.NetAmount = oreader.GetInt32("NetAmount");
                    oItem.CurrencyID = oreader.GetInt32("CurrencyID");
                    oItem.SalaryDate = oreader.GetDateTime("SalaryDate");
                    oItem.SalaryReceiveDate = oreader.GetDateTime("SalaryReceiveDate");
                    oItem.PayrollProcessID = oreader.GetInt32("EmployeeSettlementID");//return EmployeeSettlementID
                    oItem.StartDate = oreader.GetDateTime("StartDate");
                    oItem.EndDate = oreader.GetDateTime("EndDate");

                    oItem.ProductionAmount = oreader.GetDouble("ProductionAmount");
                    oItem.ProductionBonus = oreader.GetDouble("ProductionBonus");
                    oItem.OTHour = oreader.GetDouble("OTHour");
                    oItem.OTRatePerHour = oreader.GetDouble("OTRatePerHour");
                    oItem.TotalWorkingDay = oreader.GetInt32("TotalWorkingDay");
                    oItem.TotalAbsent = oreader.GetInt32("TotalAbsent");
                    oItem.TotalLate = oreader.GetInt32("TotalLate");
                    oItem.TotalEarlyLeaving = oreader.GetInt32("TotalEarlyLeaving");
                    oItem.EarlyLeavingMinute = oreader.GetInt32("EarlyLeavingMinute");
                    oItem.TotalDayOff = oreader.GetInt32("TotalDayOff");
                    oItem.TotalHoliday = oreader.GetInt32("TotalHoliday");
                    oItem.TotalUpLeave = oreader.GetInt32("TotalUpLeave");
                    oItem.TotalPLeave = oreader.GetInt32("TotalPLeave");
                    oItem.RevenueStemp = oreader.GetInt32("RevenueStemp");
                    oItem.TotalNoWorkDay = oreader.GetInt32("TotalNoWorkDay");
                    oItem.TotalNoWorkDayAllowance = oreader.GetDouble("TotalNoWorkDayAllowance");
                    oItem.AddShortFall = oreader.GetDouble("AddShortFall");

                    //derive
                    oItem.EmployeeName = oreader.GetString("EmployeeName");
                    oItem.EmployeeNameInBangla = oreader.GetString("EmployeeNameInBangla");
                    oItem.EmployeeCode = oreader.GetString("EmployeeCode");
                    oItem.LocationName = oreader.GetString("LocationName");
                    oItem.DepartmentName = oreader.GetString("DepartmentName");
                    oItem.ParentDepartmentName = oreader.GetString("ParentDepartmentName");
                    oItem.DesignationName = oreader.GetString("DesignationName");
                    oItem.JoiningDate = oreader.GetDateTime("JoiningDate");
                    oItem.DateOfConfirmation = oreader.GetDateTime("DateOfConfirmation");
                    oItem.IsProductionBase = oreader.GetBoolean("IsProductionBase");
                    oItem.EmployeeTypeName = oreader.GetString("EmployeeTypeName");

                    oEmployeeSalarys.Add(oItem);
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
