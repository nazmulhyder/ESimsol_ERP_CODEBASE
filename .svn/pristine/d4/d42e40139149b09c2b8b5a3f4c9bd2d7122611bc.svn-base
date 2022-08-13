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
    public class EmployeeSalaryV2Service : MarshalByRefObject, IEmployeeSalaryV2Service
    {
        private EmployeeSalaryV2 MapObject(NullHandler oReader)
        {
            EmployeeSalaryV2 oEmployeeSalaryV2 = new EmployeeSalaryV2();
            oEmployeeSalaryV2.EmployeeSalaryID = oReader.GetInt32("EmployeeSalaryID");
            oEmployeeSalaryV2.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeSalaryV2.LocationID = oReader.GetInt32("LocationID");
            oEmployeeSalaryV2.DepartmentID = oReader.GetInt32("DepartmentID");
            oEmployeeSalaryV2.DesignationID = oReader.GetInt32("DesignationID");
            oEmployeeSalaryV2.GrossAmount = oReader.GetDouble("GrossAmount");
            oEmployeeSalaryV2.NetAmount = oReader.GetDouble("NetAmount");
            oEmployeeSalaryV2.CurrencyID = oReader.GetInt32("CurrencyID");
            oEmployeeSalaryV2.SalaryDate = oReader.GetDateTime("SalaryDate");
            oEmployeeSalaryV2.JoiningDate = oReader.GetDateTime("JoiningDate");
            oEmployeeSalaryV2.SalaryReceiveDate = oReader.GetDateTime("SalaryReceiveDate");
            oEmployeeSalaryV2.PayrollProcessID = oReader.GetInt32("PayrollProcessID");
            oEmployeeSalaryV2.IsManual = oReader.GetBoolean("IsManual");
            oEmployeeSalaryV2.StartDate = oReader.GetDateTime("StartDate");
            oEmployeeSalaryV2.EndDate = oReader.GetDateTime("EndDate");
            oEmployeeSalaryV2.IsLock = oReader.GetBoolean("IsLock");
            oEmployeeSalaryV2.ProductionAmount = oReader.GetDouble("ProductionAmount");
            oEmployeeSalaryV2.ProductionBonus = oReader.GetDouble("ProductionBonus");
            oEmployeeSalaryV2.OTHour = oReader.GetDouble("OTHour");
            oEmployeeSalaryV2.OTRatePerHour = oReader.GetDouble("OTRatePerHour");
            oEmployeeSalaryV2.TotalWorkingDay = oReader.GetInt32("TotalWorkingDay");
            oEmployeeSalaryV2.TotalAbsent = oReader.GetInt32("TotalAbsent");
            oEmployeeSalaryV2.TotalLate = oReader.GetInt32("TotalLate");
            oEmployeeSalaryV2.TotalEarlyLeaving = oReader.GetInt32("TotalEarlyLeaving");
            oEmployeeSalaryV2.TotalDayOff = oReader.GetInt32("TotalDayOff");
            oEmployeeSalaryV2.TotalHoliday = oReader.GetInt32("TotalHoliday");
            oEmployeeSalaryV2.TotalUpLeave = oReader.GetInt32("TotalUpLeave");
            oEmployeeSalaryV2.TotalPLeave = oReader.GetInt32("TotalPLeave");
            oEmployeeSalaryV2.RevenueStemp = oReader.GetDouble("RevenueStemp");
            oEmployeeSalaryV2.TotalNoWorkDay = oReader.GetInt32("TotalNoWorkDay");
            oEmployeeSalaryV2.TotalNoWorkDayAllowance = oReader.GetDouble("TotalNoWorkDayAllowance");
            oEmployeeSalaryV2.AddShortFall = oReader.GetDouble("AddShortFall");
            oEmployeeSalaryV2.IsProductionBase = oReader.GetBoolean("IsProductionBase");
            oEmployeeSalaryV2.IsAllowOverTime = oReader.GetBoolean("IsAllowOverTime");
            oEmployeeSalaryV2.EmployeeTypeName = oReader.GetString("EmployeeTypeName");
            oEmployeeSalaryV2.PaymentType = oReader.GetString("PaymentType");
            oEmployeeSalaryV2.Gender = oReader.GetString("Gender");
            oEmployeeSalaryV2.CashAmount = oReader.GetDouble("CashAmount");
            oEmployeeSalaryV2.BankAmount = oReader.GetDouble("BankAmount");
            oEmployeeSalaryV2.BUID = oReader.GetInt32("BUID");
            oEmployeeSalaryV2.BUName = oReader.GetString("BUName");
            oEmployeeSalaryV2.BMMID = oReader.GetInt32("BMMID");
            oEmployeeSalaryV2.BlockName = oReader.GetString("BlockName");
            oEmployeeSalaryV2.LateInMin = oReader.GetInt32("LateInMin");
            oEmployeeSalaryV2.IsOutSheet = oReader.GetBoolean("IsOutSheet");
            oEmployeeSalaryV2.MonthID = oReader.GetInt32("MonthID");
            oEmployeeSalaryV2.ETIN = oReader.GetString("ETIN");
            oEmployeeSalaryV2.EmployeeCode = oReader.GetString("EmployeeCode");
            oEmployeeSalaryV2.EmployeeName = oReader.GetString("EmployeeName");
            oEmployeeSalaryV2.DepartmentName = oReader.GetString("DepartmentName");
            oEmployeeSalaryV2.DesignationName = oReader.GetString("DesignationName");
            oEmployeeSalaryV2.BankName = oReader.GetString("BankName");
            oEmployeeSalaryV2.AccountNo = oReader.GetString("AccountNo");
            oEmployeeSalaryV2.BUShortName = oReader.GetString("BUShortName");
            oEmployeeSalaryV2.PresentSalary = oReader.GetDouble("PresentSalary");
            oEmployeeSalaryV2.LastGross = oReader.GetDouble("LastGross");
            oEmployeeSalaryV2.LastIncrement = oReader.GetDouble("LastIncrement");
            oEmployeeSalaryV2.IncrementEffectDate = oReader.GetDateTime("IncrementEffectDate");
            oEmployeeSalaryV2.DefineOTHour = oReader.GetDouble("DefineOTHour");
            oEmployeeSalaryV2.ExtraOTHour = oReader.GetDouble("ExtraOTHour");
            oEmployeeSalaryV2.Grade = oReader.GetString("Grade");
            oEmployeeSalaryV2.EarlyOutInMin = oReader.GetInt32("EarlyOutInMin");
            oEmployeeSalaryV2.DayOfRegularOTHour = oReader.GetDouble("DayOfRegularOTHour");
            oEmployeeSalaryV2.LocationName = oReader.GetString("LocationName");
            oEmployeeSalaryV2.ContactNo = oReader.GetString("ContactNo");
            return oEmployeeSalaryV2;
        }
        private EmployeeSalaryV2 CreateObject(NullHandler oReader)
        {
            EmployeeSalaryV2 oEmployeeSalaryV2 = MapObject(oReader);
            return oEmployeeSalaryV2;
        }

        private List<EmployeeSalaryV2> CreateObjects(IDataReader oReader)
        {
            List<EmployeeSalaryV2> oEmployeeSalaryV2 = new List<EmployeeSalaryV2>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeSalaryV2 oItem = CreateObject(oHandler);
                oEmployeeSalaryV2.Add(oItem);
            }
            return oEmployeeSalaryV2;
        }
        public List<EmployeeSalaryV2> CompGets(string sSQL,DateTime SalaryStartDate, DateTime SalaryEndDate, Int64 nUserID)
        {
            List<EmployeeSalaryV2> oEmployeeSalaryV2 = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryV2DA.CompGets(sSQL,SalaryStartDate,SalaryEndDate, tc);
                oEmployeeSalaryV2 = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oEmployeeSalaryV2;
        }
        public List<EmployeeSalaryV2> Gets(string sSQL,DateTime SalaryStartDate, DateTime SalaryEndDate, Int64 nUserID)
        {
            List<EmployeeSalaryV2> oEmployeeSalaryV2 = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryV2DA.Gets(sSQL, SalaryStartDate, SalaryEndDate, tc);
                oEmployeeSalaryV2 = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oEmployeeSalaryV2;
        }
    }
}
