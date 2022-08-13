using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Linq;

namespace ESimSol.Services.Services
{
    public class EmployeeSalary_MAMIYAService : MarshalByRefObject, IEmployeeSalary_MAMIYAService
    {
        #region Private functions and declaration
        private EmployeeSalary_MAMIYA MapObject(NullHandler oReader)
        {
            EmployeeSalary_MAMIYA oEmployeeSalary_MAMIYA = new EmployeeSalary_MAMIYA();
            oEmployeeSalary_MAMIYA.EmployeeSalaryID = oReader.GetInt32("ESID");
            oEmployeeSalary_MAMIYA.EmpCategory = (EnumEmployeeCategory)oReader.GetInt16("EmpCategory");
            oEmployeeSalary_MAMIYA.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeSalary_MAMIYA.EmployeeCode = oReader.GetString("EmployeeCode");
            oEmployeeSalary_MAMIYA.EmployeeName = oReader.GetString("Name");
            oEmployeeSalary_MAMIYA.DepartmentName = oReader.GetString("DepartmentName");
            oEmployeeSalary_MAMIYA.DesignationName = oReader.GetString("DesignationName");
            oEmployeeSalary_MAMIYA.DateOfJoin = oReader.GetDateTime("DateOfJoin");
            oEmployeeSalary_MAMIYA.ConfirmationDate = oReader.GetDateTime("ConfirmationDate");
            oEmployeeSalary_MAMIYA.DateOfConfirmation = oReader.GetDateTime("DateOfConfirmation");
            oEmployeeSalary_MAMIYA.LastMonthLoanBalance = oReader.GetDouble("LastMonthLoanBalance");
            oEmployeeSalary_MAMIYA.LastMonthPFSub = oReader.GetDouble("LastMonthPFSub");
            oEmployeeSalary_MAMIYA.NightAllowDays = oReader.GetInt32("NightAllowDays");
            oEmployeeSalary_MAMIYA.TotalWorkingDay = oReader.GetInt32("TotalWorkingDay");
            oEmployeeSalary_MAMIYA.SickLeaveDeduction = oReader.GetInt32("SickLeaveDeduction");
            oEmployeeSalary_MAMIYA.NightAllow = oReader.GetDouble("NightAllow");
            oEmployeeSalary_MAMIYA.Basic = oReader.GetDouble("Basic");            
            oEmployeeSalary_MAMIYA.HRent = oReader.GetDouble("HRent");
            oEmployeeSalary_MAMIYA.Med = oReader.GetDouble("Med");
            oEmployeeSalary_MAMIYA.Conveyance = oReader.GetDouble("Conveyance");
            oEmployeeSalary_MAMIYA.GrossSalary = oReader.GetDouble("GrossSalary");
            oEmployeeSalary_MAMIYA.AbsentHr_Sick = oReader.GetDouble("AbsentHr_Sick");
            oEmployeeSalary_MAMIYA.AbsentHr_WOPay = oReader.GetDouble("AbsentHr_WOPay");
            oEmployeeSalary_MAMIYA.TotalAbsentAmount = oReader.GetDouble("TotalAbsentAmount");            
            oEmployeeSalary_MAMIYA.EarnedPay = oReader.GetDouble("EarnedPay");
            oEmployeeSalary_MAMIYA.ShiftAmount = oReader.GetDouble("ShiftAmount");
            oEmployeeSalary_MAMIYA.OT_NHR = oReader.GetDouble("OT_NHR");
            oEmployeeSalary_MAMIYA.OT_HHR = oReader.GetDouble("OT_HHR");
            oEmployeeSalary_MAMIYA.FHOT = oReader.GetDouble("OT_FHR");
            oEmployeeSalary_MAMIYA.OTAmount = oReader.GetDouble("OTAmount");
            oEmployeeSalary_MAMIYA.AttendanceBonus = oReader.GetDouble("AttendanceBonus");
            oEmployeeSalary_MAMIYA.ADJCR = oReader.GetDouble("ADJCR");
            oEmployeeSalary_MAMIYA.OtherAll = oReader.GetDouble("OtherAll");
            oEmployeeSalary_MAMIYA.FB = oReader.GetDouble("FB");
            oEmployeeSalary_MAMIYA.MobileBill = oReader.GetDouble("MobileBill");
            oEmployeeSalary_MAMIYA.GrossPay = oReader.GetDouble("GrossPay");
            oEmployeeSalary_MAMIYA.PF = oReader.GetDouble("PF");
            oEmployeeSalary_MAMIYA.TRNS = oReader.GetDouble("TRNS");
            oEmployeeSalary_MAMIYA.DORM = oReader.GetDouble("DORM");
            oEmployeeSalary_MAMIYA.ADV = oReader.GetDouble("ADV");
            oEmployeeSalary_MAMIYA.ADJDR = oReader.GetDouble("ADJDR");
            oEmployeeSalary_MAMIYA.RS = oReader.GetDouble("RS");
            oEmployeeSalary_MAMIYA.InterestAmt = oReader.GetDouble("InterestAmt");
            oEmployeeSalary_MAMIYA.InstallmentAmt = oReader.GetDouble("InstallmentAmt");
            oEmployeeSalary_MAMIYA.PLoan = oReader.GetDouble("PLoan");
            oEmployeeSalary_MAMIYA.IncomeTax = oReader.GetDouble("Tax");
            oEmployeeSalary_MAMIYA.DeductionTotal = oReader.GetDouble("DeductionTotal");
            oEmployeeSalary_MAMIYA.Fracretained = oReader.GetDouble("Fracretained");
            oEmployeeSalary_MAMIYA.NetPay = oReader.GetDouble("NetPay");
            oEmployeeSalary_MAMIYA.ShiftAmount = oReader.GetDouble("ShiftAmount");
            oEmployeeSalary_MAMIYA.BankAccountNo = oReader.GetString("ACNo");
            oEmployeeSalary_MAMIYA.HolidayAll = oReader.GetDouble("HolidayAllowance");
            oEmployeeSalary_MAMIYA.EmployeeTypeID = oReader.GetInt32("EmployeeTypeID");

            return oEmployeeSalary_MAMIYA;
        }

        private EmployeeSalary_MAMIYA CreateObject(NullHandler oReader)
        {
            EmployeeSalary_MAMIYA oEmployeeSalary_MAMIYA = MapObject(oReader);
            return oEmployeeSalary_MAMIYA;
        }

        private List<EmployeeSalary_MAMIYA> CreateObjects(IDataReader oReader)
        {
            List<EmployeeSalary_MAMIYA> oEmployeeSalary_MAMIYA = new List<EmployeeSalary_MAMIYA>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeSalary_MAMIYA oItem = CreateObject(oHandler);
                oEmployeeSalary_MAMIYA.Add(oItem);
            }
            return oEmployeeSalary_MAMIYA;
        }

        #endregion

        #region Interface implementation
        public EmployeeSalary_MAMIYAService() { }

        public List<EmployeeSalary_MAMIYA> Gets_MAMIYA(DateTime dtDateFrom, DateTime dtDateTo, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, int nPayType, long nUserID)
        {
            List<EmployeeSalary_MAMIYA> oEmployeeSalary_MAMIYAs = new List<EmployeeSalary_MAMIYA>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeSalary_MAMIYADA.Gets(dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs, nPayType,tc);
                oEmployeeSalary_MAMIYAs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Employee Salary", e);
                #endregion
            }
            return oEmployeeSalary_MAMIYAs;
        }

        public List<EmployeeSalary_MAMIYA> Gets_PaySlip(DateTime dtDateFrom, DateTime dtDateTo, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs,int nPayType, long nUserID)
        {
            List<EmployeeSalary_MAMIYA> oEmployeeSalary_MAMIYAs = new List<EmployeeSalary_MAMIYA>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeSalary_MAMIYADA.GetForPaySlip(dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs, nPayType, tc);
                //oEmployeeSalary_MAMIYAs = CreateObjects(reader);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    EmployeeSalary_MAMIYA oItem = new EmployeeSalary_MAMIYA();
                    oItem.EmpCategory = (EnumEmployeeCategory)oreader.GetInt16("EmpCategory");
                    oItem.DateOfConfirmation = oreader.GetDateTime("DateOfConfirmation");
                    oItem.LastMonthLoanBalance = oreader.GetDouble("LastMonthLoanBalance");
                    oItem.LastMonthPFSub = oreader.GetDouble("LastMonthPFSub");
                    oItem.NightAllowDays = oreader.GetInt32("NightAllowDays");
                    oItem.TotalWorkingDay = oreader.GetInt32("TotalWorkingDay");
                    oItem.SickLeaveDeduction = oreader.GetInt32("SickLeaveDeduction");
                    oItem.NightAllow = oreader.GetDouble("NightAllow");
                    oItem.EmployeeSalaryID = oreader.GetInt32("ESID");
                    oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                    oItem.EmployeeCode = oreader.GetString("EmployeeCode");
                    oItem.EmployeeName = oreader.GetString("Name");
                    oItem.LocationName = oreader.GetString("LocationName");
                    oItem.DepartmentName = oreader.GetString("DepartmentName");
                    oItem.DesignationName = oreader.GetString("DesignationName");
                    oItem.DateOfJoin = oreader.GetDateTime("DateOfJoin");
                    oItem.GrossSalary = oreader.GetDouble("GrossSalary");
                    oItem.TotalPresent = oreader.GetInt32("TotalPresent");
                    oItem.TotalAbsent = oreader.GetInt32("TotalAbsent");
                    oItem.AbsentHr_Sick = oreader.GetDouble("AbsentHr_Sick");
                    oItem.AbsentHr_WOPay = oreader.GetDouble("AbsentHr_WOPay");
                    oItem.TotalDayOff = oreader.GetInt32("TotalDayOff");
                    oItem.TotalLeave = oreader.GetInt32("TotalLeave");
                    oItem.OT_NHR = oreader.GetDouble("OT_NHR");
                    oItem.OT_HHR = oreader.GetDouble("OT_HHR");
                    oItem.FHOT = oreader.GetDouble("FHOT");
                    oItem.Basic = oreader.GetDouble("Basic");
                    oItem.HRent = oreader.GetDouble("HRent");
                    oItem.Med = oreader.GetDouble("Med");
                    oItem.Conveyance = oreader.GetDouble("Conveyance");
                    oItem.ShiftAmount = oreader.GetDouble("ShiftAmount");
                    oItem.AttendanceBonus = oreader.GetDouble("AttendanceBonus");
                    oItem.OTAmount = oreader.GetDouble("OTAmount");
                    oItem.EarningsTotal = oreader.GetDouble("EarningsTotal");
                    oItem.PF = oreader.GetDouble("PF");
                    oItem.PFProfit = oreader.GetDouble("PFProfit");
                    oItem.PLoan = oreader.GetDouble("PLoan");
                    oItem.TotalAbsentAmount = oreader.GetDouble("TotalAbsentAmount");
                    oItem.ADV = oreader.GetDouble("ADV");
                    oItem.RS = oreader.GetDouble("RS");
                    oItem.DeductionTotal = oreader.GetDouble("DeductionTotal");
                    oItem.NetPay = oreader.GetDouble("NetPay");
                    oItem.BankAccountNo = oreader.GetString("BankAccountNo");
                    oItem.BankName = oreader.GetString("BankName");
                    oItem.CasualLeave = oreader.GetDouble("CasualLeave");
                    oItem.EarnLeave = oreader.GetDouble("EarnLeave");
                    oItem.InterestAmt = oreader.GetDouble("InterestAmt");
                    oItem.InstallmentAmt = oreader.GetDouble("InstallmentAmt");
                    oItem.FriDay = oreader.GetInt32("FriDay");
                    oItem.HoliDay = oreader.GetInt32("HoliDay");
                    oItem.IncomeTax = oreader.GetDouble("IncomeTax");
                    oItem.ADJCR = oreader.GetDouble("ADJCR");
                    oItem.ADJDR = oreader.GetDouble("ADJDR");
                    oItem.FB = oreader.GetDouble("FB");
                    oItem.MobileBill = oreader.GetDouble("MobileBill");
                    oItem.OtherAll = oreader.GetDouble("OtherAll");
                    oItem.LoanBalance = oreader.GetDouble("LoanBalance");//Add date 21 jul 2016
                    oItem.TRNS = oreader.GetDouble("TRNS");
                    oItem.HolidayAll = oreader.GetInt32("HolidayAllowance");
                    oItem.Allowance = oreader.GetDouble("Allowance");

                    oEmployeeSalary_MAMIYAs.Add(oItem);
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
                throw new ServiceException(e.Message);
                #endregion
            }
            return oEmployeeSalary_MAMIYAs;
        }

        public List<EmployeeSalary_MAMIYA> Gets_SalarySummery_MAMIYA(DateTime dtDateFrom, DateTime dtDateTo, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, int nPayType, long nUserID)
        {
            List<EmployeeSalary_MAMIYA> oEmployeeSalary_MAMIYAs = new List<EmployeeSalary_MAMIYA>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeSalary_MAMIYADA.Gets_SalarySummery(dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs,nPayType, tc);
                //oEmployeeSalary_MAMIYAs = CreateObjects(reader);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    EmployeeSalary_MAMIYA oItem = new EmployeeSalary_MAMIYA();
                    oItem.DepartmentID = oreader.GetInt32("DepartmentID");
                    oItem.DepartmentName = oreader.GetString("DepartmentName");
                    oItem.NoOfEmp = oreader.GetInt32("NoOfEmp");
                    oItem.Basic = oreader.GetDouble("Basic");
                    oItem.HRent = oreader.GetDouble("HRent");
                    oItem.Med = oreader.GetDouble("Med");
                    oItem.Conveyance = oreader.GetDouble("Conveyance");
                    oItem.GrossSalary = oreader.GetDouble("GrossSalary");
                    oItem.AbsentHr_Sick = oreader.GetDouble("AbsentHr_Sick");
                    oItem.AbsentHr_WOPay = oreader.GetDouble("AbsentHr_WOPay");
                    oItem.TotalAbsentAmount = oreader.GetDouble("TotalAbsentAmount");
                    oItem.EarnedPay = oreader.GetDouble("EarnedPay");
                    oItem.ShiftAmount = oreader.GetDouble("ShiftAmount");
                    oItem.OT_NHR = oreader.GetDouble("OT_NHR");
                    oItem.OT_HHR = oreader.GetDouble("OT_HHR");
                    oItem.FHOT = oreader.GetDouble("FHOT");
                    oItem.OTAmount = oreader.GetDouble("OTAmount");
                    oItem.AttendanceBonus = oreader.GetDouble("AttendanceBonus");
                    oItem.OtherAll = oreader.GetDouble("OtherAll");
                    oItem.FB = oreader.GetDouble("FB");
                    oItem.MobileBill = oreader.GetDouble("MobileBill");
                    oItem.ADJCR = oreader.GetDouble("ADJCR");
                    oItem.GrossPay = oreader.GetDouble("GrossPay");
                    oItem.PF = oreader.GetDouble("PF");
                    oItem.TRNS = oreader.GetDouble("TRNS");
                    oItem.DORM = oreader.GetDouble("DORM");
                    oItem.ADV = oreader.GetDouble("ADV");
                    oItem.ADJDR = oreader.GetDouble("ADJDR");
                    oItem.RS = oreader.GetDouble("RS");
                    oItem.IncomeTax = oreader.GetDouble("Tax");
                    oItem.PLoan = oreader.GetDouble("PLoan");
                    oItem.DeductionTotal = oreader.GetDouble("DeductionTotal");
                    //oItem.Fracretained = oreader.GetDouble("Fracretained");
                    oItem.NetPay = oreader.GetDouble("NetPay");
                    oItem.ShiftAmount = oreader.GetDouble("ShiftAmount");
                    oItem.InterestAmt = oreader.GetDouble("InterestAmt");

                    oEmployeeSalary_MAMIYAs.Add(oItem);
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
                throw new ServiceException(e.Message);
                #endregion
            }
            return oEmployeeSalary_MAMIYAs;
        }

        public List<EmployeeSalary_MAMIYA> Gets_FinalSettlementOfResig(int nEmployeeSettlementID, long nUserID)
        {
            List<EmployeeSalary_MAMIYA> oEmployeeSalary_MAMIYAs = new List<EmployeeSalary_MAMIYA>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeSalary_MAMIYADA.Gets_FinalSettlementOfResig(nEmployeeSettlementID, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    EmployeeSalary_MAMIYA oItem = new EmployeeSalary_MAMIYA();
                    oItem.EmployeeSalaryID = oreader.GetInt32("ESID");
                    oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                    oItem.EmployeeCode = oreader.GetString("EmployeeCode");
                    oItem.EmployeeName = oreader.GetString("Name");
                    oItem.LocationName = oreader.GetString("LocationName");
                    oItem.DepartmentName = oreader.GetString("DepartmentName");
                    oItem.DesignationName = oreader.GetString("DesignationName");
                    oItem.DateOfJoin = oreader.GetDateTime("DateOfJoin");
                    oItem.GrossSalary = oreader.GetDouble("GrossSalary");
                    oItem.TotalPresent = oreader.GetInt32("TotalPresent");
                    oItem.TotalAbsent = oreader.GetInt32("TotalAbsent");
                    oItem.AbsentHr_Sick = oreader.GetDouble("AbsentHr_Sick");
                    oItem.SLDeduction = oreader.GetDouble("SLDeduction");
                    oItem.AbsentHr_WOPay = oreader.GetDouble("AbsentHr_WOPay");
                    oItem.TotalDayOff = oreader.GetInt32("TotalDayOff");
                    oItem.TotalUpLeave = oreader.GetInt32("TotalUpLeave");
                    oItem.TotalPLeave = oreader.GetInt32("TotalPLeave");
                    oItem.OT_NHR = oreader.GetDouble("OT_NHR");
                    oItem.OT_HHR = oreader.GetDouble("OT_HHR");
                    oItem.Basic = oreader.GetDouble("Basic");
                    oItem.HRent = oreader.GetDouble("HRent");
                    oItem.Med = oreader.GetDouble("Med");
                    oItem.Conveyance = oreader.GetDouble("Conveyance");
                    oItem.OtherAll = oreader.GetDouble("OtherAll");
                    oItem.ShiftAmount = oreader.GetDouble("ShiftAmount");
                    oItem.TotalNoWorkDayAllowance = oreader.GetDouble("TotalNoWorkDayAllowance");
                    oItem.AttendanceBonus = oreader.GetDouble("AttendanceBonus");
                    oItem.FB = oreader.GetDouble("FB");
                    oItem.ADJCR = oreader.GetDouble("ADJCR");
                    oItem.OT_NHR_AMT = oreader.GetDouble("OT_NHR_AMT");
                    oItem.OT_HHR_AMT = oreader.GetDouble("OT_HHR_AMT");
                    oItem.NoticePayAddition = oreader.GetDouble("NoticePayAddition");
                    oItem.Gratuity = oreader.GetDouble("Gratuity");
                    oItem.ELAmount = oreader.GetDouble("ELAmount");
                    oItem.EarningsTotal = oreader.GetDouble("EarningsTotal");
                    oItem.PF = oreader.GetDouble("PF");
                    oItem.TRNS = oreader.GetDouble("TRNS");
                    oItem.DORM = oreader.GetDouble("DORM");
                    oItem.PLoan = oreader.GetDouble("PLoan");
                    oItem.TotalAbsentAmount = oreader.GetDouble("TotalAbsentAmount");
                    oItem.ADV = oreader.GetDouble("ADV");
                    oItem.RS = oreader.GetDouble("RS");
                    oItem.DeductionTotal = oreader.GetDouble("DeductionTotal");
                    oItem.NetPay = oreader.GetDouble("NetPay");
                    oItem.BankAccountNo = oreader.GetString("BankAccountNo");
                    oItem.BankName = oreader.GetString("BankName");
                    oItem.CasualLeave = oreader.GetDouble("CasualLeave");
                    oItem.EarnLeave = oreader.GetDouble("EarnLeave");
                    oItem.ELBalance = oreader.GetDouble("ELBalance");
                    oItem.InterestAmt = oreader.GetDouble("InterestAmt");
                    oItem.InstallmentAmt = oreader.GetDouble("InstallmentAmt");
                    oItem.FriDay = oreader.GetInt32("FriDay");
                    oItem.HoliDay = oreader.GetInt32("HoliDay");
                    oItem.ShiftAllDay = oreader.GetInt32("ShiftAllDay");
                    oItem.IncomeTax = oreader.GetDouble("IncomeTax");
                    oItem.ADJDR = oreader.GetDouble("ADJDR");
                    oItem.DateOfConfirmation = oreader.GetDateTime("DateOfConfirmation");
                    oItem.DateOfResigEffect = oreader.GetDateTime("DateOfResigEffect");
                    oItem.DateOfBirth = oreader.GetDateTime("DateOfBirth");
                    oItem.SettlementType = (EnumSettleMentType)oreader.GetInt16("SettlementType");
                    oItem.SalaryStartDate = oreader.GetDateTime("SalaryStartDate");
                    oItem.SalaryEndDate = oreader.GetDateTime("SalaryEndDate");
                    oItem.HolidayAll = oreader.GetDouble("HolidayAll");
                    oItem.NoticePayDeduction = oreader.GetDouble("NoticePayDeduction");
                    oEmployeeSalary_MAMIYAs.Add(oItem);
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
                oEmployeeSalary_MAMIYAs = new List<EmployeeSalary_MAMIYA>();
                EmployeeSalary_MAMIYA oEmployeeSalary_MAMIYA = new EmployeeSalary_MAMIYA();
                oEmployeeSalary_MAMIYA.ErrorMessage = e.Message.Split('!')[0];
                oEmployeeSalary_MAMIYAs.Add(oEmployeeSalary_MAMIYA);
                //throw new ServiceException(e.Message);
                #endregion
            }
            return oEmployeeSalary_MAMIYAs;
        }

        public List<EmployeeSalary_MAMIYA> Gets_OTSheet(DateTime dtStartDate, DateTime dtEndDate, long nUserID)
        {
            List<EmployeeSalary_MAMIYA> oEmployeeSalary_MAMIYAs = new List<EmployeeSalary_MAMIYA>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeSalary_MAMIYADA.Gets_OTSheet( dtStartDate,  dtEndDate, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    EmployeeSalary_MAMIYA oItem = new EmployeeSalary_MAMIYA();

                    oItem.EmployeeCode = oreader.GetString("EmployeeCode");
                    oItem.EmployeeName = oreader.GetString("EmployeeName");
                    oItem.DepartmentName = oreader.GetString("DepartmentName");
                    oItem.DesignationName = oreader.GetString("DesignationName");
                    oItem.DateOfJoin = oreader.GetDateTime("DOJ");
                    oItem.Basic = oreader.GetDouble("Basic");
                    oItem.HRent = oreader.GetDouble("HRent");
                    oItem.Med = oreader.GetDouble("Medical");
                    oItem.GrossSalary = oreader.GetDouble("GrossAmount");
                    oItem.OT_NHR = oreader.GetDouble("OT_NHR");
                    oItem.OT_HHR = oreader.GetDouble("OT_HHR");
                    oItem.FHOT = oreader.GetDouble("FHOT");
                    oItem.OTAmount = oreader.GetDouble("OTAmount");
                    oItem.IsActive = oreader.GetBoolean("IsActive");
                    oEmployeeSalary_MAMIYAs.Add(oItem);
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
                throw new ServiceException(e.Message);
                #endregion
            }
            return oEmployeeSalary_MAMIYAs;
        }

        public List<EmployeeSalary_MAMIYA> GetsSettlementDetailList(string sParam, long nUserID, int FinancialUserType)
        {
             DateTime dtStartDate = Convert.ToDateTime(sParam.Split('~')[0]);
             DateTime dtEndDate = Convert.ToDateTime(sParam.Split('~')[1]);
             Int16 nSettlementType = Convert.ToInt16(sParam.Split('~')[2]);
             string sDepartmentIds = sParam.Split('~')[3];
             string sDesignationIds =sParam.Split('~')[4];
             Int16 nClearanceStatus =  Convert.ToInt16(sParam.Split('~')[5]);
             Int16 nApproveStatus = Convert.ToInt16(sParam.Split('~')[6]);


             List<EmployeeSalary_MAMIYA> oEmployeeSalary_MAMIYAs = new List<EmployeeSalary_MAMIYA>();
             List<EmployeeSettlement> oEmployeeSettlements = new List<EmployeeSettlement>();
             string sSql = "";


            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                sSql = "SELECT * FROM View_EmployeeSettlement WHERE EmployeeSettlementID<>0 AND EffectDate BETWEEN '" + dtStartDate.ToString("dd MMM yyyy") + "' AND '" + dtEndDate.ToString("dd MMM yyyy") + "' ";
                if (sDepartmentIds != "")
                {
                    sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE DepartmentID IN(" + sDepartmentIds + "))";
                }
                if (sDesignationIds != "")
                {
                    sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM EmployeeOfficial WHERE DesignationID IN(" + sDesignationIds + "))";
                }

                if (nSettlementType > 0)
                {
                    sSql = sSql + " AND SettlementType=" + nSettlementType;
                }

                if (nApproveStatus == 1)
                {
                    sSql = sSql + " AND ApproveBy>0";
                }
                if (nApproveStatus == 2)
                {
                    sSql = sSql + " AND ApproveBy<=0";
                }
                if (nClearanceStatus > 0)
                {
                    sSql = sSql + " AND EmployeeSettlementID IN(SELECT EmployeeSettlementID  FROM EmployeeSettlementClearance WHERE CurrentStatus=" + nClearanceStatus + ")";
                }
                if (FinancialUserType != (int)EnumFinancialUserType.GroupAccounts)
                {
                    sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE DRPID IN( "
                                + " SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + nUserID + "))";
                }
                IDataReader readerSettID = null;
                readerSettID = EmployeeSettlementDA.Gets(sSql, tc);
                NullHandler oreaderSettID = new NullHandler(readerSettID);
                while (readerSettID.Read())
                {
                    EmployeeSettlement oItem = new EmployeeSettlement();

                    oItem.EmployeeSettlementID = oreaderSettID.GetInt32("EmployeeSettlementID");

                    oEmployeeSettlements.Add(oItem);
                }
                readerSettID.Close();
                tc.End();

                string[] sEmployeeSettlementIDs = string.Join(",", oEmployeeSettlements.Select(x=>x.EmployeeSettlementID.ToString())).Split(',');

                foreach (string sEmployeeSettlementID in sEmployeeSettlementIDs)
                {
                    int nEmployeeSettlementID = Convert.ToInt32(sEmployeeSettlementID);

                    string sSalarySql = "SELECT ISNULL( COUNT(*),0) as TotalCount FROM EmployeeSettlementSalary WHERE EmployeeSettlementID=" + nEmployeeSettlementID;
                    tc = TransactionContext.Begin(true);
                    bool IsExists = EmployeeSalary_MAMIYADA.IsExists(sSalarySql, tc);
                    tc.End();
                    if (IsExists)
                    {
                        tc = TransactionContext.Begin(true);
                        IDataReader reader = null;
                        reader = EmployeeSalary_MAMIYADA.Gets_FinalSettlementOfResig(nEmployeeSettlementID, tc);
                        NullHandler oreader = new NullHandler(reader);
                        while (reader.Read())
                        {
                            EmployeeSalary_MAMIYA oItem = new EmployeeSalary_MAMIYA();
                            oItem.EmployeeSalaryID = oreader.GetInt32("ESID");
                            oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                            oItem.EmployeeCode = oreader.GetString("EmployeeCode");
                            oItem.EmployeeName = oreader.GetString("Name");
                            oItem.LocationName = oreader.GetString("LocationName");
                            oItem.DepartmentName = oreader.GetString("DepartmentName");
                            oItem.DesignationName = oreader.GetString("DesignationName");
                            oItem.DateOfJoin = oreader.GetDateTime("DateOfJoin");
                            oItem.GrossSalary = oreader.GetDouble("GrossSalary");
                            oItem.TotalPresent = oreader.GetInt32("TotalPresent");
                            oItem.TotalAbsent = oreader.GetInt32("TotalAbsent");
                            oItem.AbsentHr_Sick = oreader.GetDouble("AbsentHr_Sick");
                            oItem.SLDeduction = oreader.GetDouble("SLDeduction");
                            oItem.AbsentHr_WOPay = oreader.GetDouble("AbsentHr_WOPay");
                            oItem.TotalDayOff = oreader.GetInt32("TotalDayOff");
                            oItem.TotalUpLeave = oreader.GetInt32("TotalUpLeave");
                            oItem.TotalPLeave = oreader.GetInt32("TotalPLeave");
                            oItem.OT_NHR = oreader.GetDouble("OT_NHR");
                            oItem.OT_HHR = oreader.GetDouble("OT_HHR");
                            oItem.Basic = oreader.GetDouble("Basic");
                            oItem.HRent = oreader.GetDouble("HRent");
                            oItem.Med = oreader.GetDouble("Med");
                            oItem.Conveyance = oreader.GetDouble("Conveyance");
                            oItem.OtherAll = oreader.GetDouble("OtherAll");
                            oItem.ShiftAmount = oreader.GetDouble("ShiftAmount");
                            oItem.TotalNoWorkDayAllowance = oreader.GetDouble("TotalNoWorkDayAllowance");
                            oItem.AttendanceBonus = oreader.GetDouble("AttendanceBonus");
                            oItem.FB = oreader.GetDouble("FB");
                            oItem.ADJCR = oreader.GetDouble("ADJCR");
                            oItem.OT_NHR_AMT = oreader.GetDouble("OT_NHR_AMT");
                            oItem.OT_HHR_AMT = oreader.GetDouble("OT_HHR_AMT");
                            oItem.NoticePayAddition = oreader.GetDouble("NoticePayAddition");
                            oItem.Gratuity = oreader.GetDouble("Gratuity");
                            oItem.ELAmount = oreader.GetDouble("ELAmount");
                            oItem.EarningsTotal = oreader.GetDouble("EarningsTotal");
                            oItem.PF = oreader.GetDouble("PF");
                            oItem.TRNS = oreader.GetDouble("TRNS");
                            oItem.DORM = oreader.GetDouble("DORM");
                            oItem.PLoan = oreader.GetDouble("PLoan");
                            oItem.TotalAbsentAmount = oreader.GetDouble("TotalAbsentAmount");
                            oItem.ADV = oreader.GetDouble("ADV");
                            oItem.RS = oreader.GetDouble("RS");
                            oItem.DeductionTotal = oreader.GetDouble("DeductionTotal");
                            oItem.NetPay = oreader.GetDouble("NetPay");
                            oItem.BankAccountNo = oreader.GetString("BankAccountNo");
                            oItem.BankName = oreader.GetString("BankName");
                            oItem.CasualLeave = oreader.GetDouble("CasualLeave");
                            oItem.EarnLeave = oreader.GetDouble("EarnLeave");
                            oItem.ELBalance = oreader.GetDouble("ELBalance");
                            oItem.InterestAmt = oreader.GetDouble("InterestAmt");
                            oItem.InstallmentAmt = oreader.GetDouble("InstallmentAmt");
                            oItem.FriDay = oreader.GetInt32("FriDay");
                            oItem.HoliDay = oreader.GetInt32("HoliDay");
                            oItem.ShiftAllDay = oreader.GetInt32("ShiftAllDay");
                            oItem.IncomeTax = oreader.GetDouble("IncomeTax");
                            oItem.ADJDR = oreader.GetDouble("ADJDR");
                            oItem.DateOfConfirmation = oreader.GetDateTime("DateOfConfirmation");
                            oItem.DateOfResigEffect = oreader.GetDateTime("DateOfResigEffect");
                            oItem.DateOfBirth = oreader.GetDateTime("DateOfBirth");
                            oItem.SettlementType = (EnumSettleMentType)oreader.GetInt16("SettlementType");
                            oItem.SalaryStartDate = oreader.GetDateTime("SalaryStartDate");
                            oItem.SalaryEndDate = oreader.GetDateTime("SalaryEndDate");
                            oItem.HolidayAll = oreader.GetDouble("HolidayAll");
                            oItem.NoticePayDeduction = oreader.GetDouble("NoticePayDeduction");

                            oItem.StructureGross = oreader.GetDouble("StructureGross");
                            oItem.StructureBasic = oreader.GetDouble("StructureBasic");
                            oItem.StructureHR = oreader.GetDouble("StructureHR");
                            oItem.StructureMedical = oreader.GetDouble("StructureMedical");

                            oEmployeeSalary_MAMIYAs.Add(oItem);
                        }
                        reader.Close();

                        //int nEmployeeID = oEmployeeSettlements.Where(x => x.EmployeeSettlementID == nEmployeeSettlementID).ToList()[0].EmployeeID;

                        //List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();

                        //IDataReader readerESSD = null;
                        //readerESSD = EmployeeSalaryStructureDetailDA.Gets("SELECT * FROM VIEW_EmployeeSalaryStructureDetail WHERE ESSID IN(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=" + nEmployeeID + ")", tc);


                        //List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructures = new List<EmployeeSalaryStructureDetail>();

                        //IDataReader readerESS = null;
                        //readerESS = EmployeeSalaryStructureDA.Gets("SELECT *  FROM VIEW_EmployeeSalaryStructure WHERE EmployeeID=" + nEmployeeID, tc);


                        tc.End();
                    }
                }
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
            return oEmployeeSalary_MAMIYAs;
        }
        #endregion
    }
}
