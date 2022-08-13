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
    public class FinalSettlementService : MarshalByRefObject, IFinalSettlementService
    {
        #region Private functions and declaration
        private FinalSettlement MapObject(NullHandler oReader)
        {
            FinalSettlement oFinalSettlement = new FinalSettlement();
            oFinalSettlement.EmployeeSettlementID = oReader.GetInt32("EmployeeSettlementID");
            oFinalSettlement.EmployeeID = oReader.GetInt32("EmployeeID");
            oFinalSettlement.PaymentDate = oReader.GetDateTime("PaymentDate");
            oFinalSettlement.EmployeeName = oReader.GetString("EmployeeName");
            oFinalSettlement.EmployeeNameBangla = oReader.GetString("EmployeeNameBangla");
            oFinalSettlement.EmployeeCode = oReader.GetString("EmployeeCode");
            oFinalSettlement.EmployeeCodeBangla = oReader.GetString("EmployeeCodeBangla");
            oFinalSettlement.BUID = oReader.GetInt32("BUID");
            oFinalSettlement.BUName = oReader.GetString("BUName");
            oFinalSettlement.BUNameBangla = oReader.GetString("BUNameBangla");
            oFinalSettlement.DesignationID = oReader.GetInt32("DesignationID");
            oFinalSettlement.DesignationName = oReader.GetString("DesignationName");
            oFinalSettlement.DesignationNameBangla = oReader.GetString("DesignationNameBangla");
            oFinalSettlement.DepartmentID = oReader.GetInt32("DepartmentID");
            oFinalSettlement.DeptName = oReader.GetString("DeptName");
            oFinalSettlement.DrptNameBangla = oReader.GetString("DrptNameBangla");
            oFinalSettlement.DateOfJoin = oReader.GetDateTime("DateOfJoin");
            oFinalSettlement.DateOfSeparation = oReader.GetDateTime("DateOfSeparation");
            oFinalSettlement.JobDurationYears = oReader.GetInt32("JobDurationYears");
            oFinalSettlement.JobDurationMonths = oReader.GetInt32("JobDurationMonths");
            oFinalSettlement.JobDurationDays = oReader.GetInt32("JobDurationDays");
            oFinalSettlement.GrossSalary = oReader.GetDouble("GrossSalary");
            oFinalSettlement.BasicSalary = oReader.GetDouble("BasicSalary");
            oFinalSettlement.PerDayGross = oReader.GetDouble("PerDayGross");
            oFinalSettlement.PerDayBasic = oReader.GetDouble("PerDayBasic");
            oFinalSettlement.OTRatePerHour = oReader.GetDouble("OTRatePerHour");
            oFinalSettlement.PayableDays = oReader.GetInt32("PayableDays");
            oFinalSettlement.PayableAmount = oReader.GetDouble("PayableAmount");
            oFinalSettlement.OTHour = oReader.GetInt32("OTHour");
            oFinalSettlement.OTAmount = oReader.GetDouble("OTAmount");
            oFinalSettlement.AttendanceBonus = oReader.GetDouble("AttendanceBonus");
            oFinalSettlement.FestivalBonus = oReader.GetDouble("FestivalBonus");
            oFinalSettlement.EarnLeaveDays = oReader.GetInt32("EarnLeaveDays");
            oFinalSettlement.EarnLeaveAmount = oReader.GetDouble("EarnLeaveAmount");
            oFinalSettlement.ServiceBenefitDays = oReader.GetInt32("ServiceBenefitDays");
            oFinalSettlement.ServiceBenefitAmount = oReader.GetDouble("ServiceBenefitAmount");
            oFinalSettlement.TerminationBenefitDays = oReader.GetInt32("TerminationBenefitDays");
            oFinalSettlement.TerminationBenefitAmount = oReader.GetDouble("TerminationBenefitAmount");
            oFinalSettlement.TotalPayableAmount = oReader.GetDouble("TotalPayableAmount");
            oFinalSettlement.AbsentDays = oReader.GetInt32("AbsentDays");
            oFinalSettlement.AbsentAmount = oReader.GetDouble("AbsentAmount");
            oFinalSettlement.NoticePeriodDays = oReader.GetInt32("NoticePeriodDays");
            oFinalSettlement.NoticePeriodAmount = oReader.GetDouble("NoticePeriodAmount");
            oFinalSettlement.PaidEarnLeaveDays = oReader.GetInt32("PaidEarnLeaveDays");
            oFinalSettlement.PaidEarnLeaveAmount = oReader.GetDouble("PaidEarnLeaveAmount");
            oFinalSettlement.StampAmount = oReader.GetDouble("StampAmount");
            oFinalSettlement.OthersAmount = oReader.GetDouble("OthersAmount");
            oFinalSettlement.TotalDeductionAmount = oReader.GetDouble("TotalDeductionAmount");
            oFinalSettlement.TotalDuesAmount = oReader.GetDouble("TotalDuesAmount");
            oFinalSettlement.AdvPaidAmount = oReader.GetDouble("AdvPaidAmount");
            oFinalSettlement.ApprovedAmount = oReader.GetDouble("ApprovedAmount");
            return oFinalSettlement;

        }

        private FinalSettlement CreateObject(NullHandler oReader)
        {
            FinalSettlement oFinalSettlement = MapObject(oReader);
            return oFinalSettlement;
        }


        #endregion



        public FinalSettlement Get(int nEmployeeSettlementID, Int64 nUsEmployeeSettlementID)
        {
            FinalSettlement oFinalSettlement = new FinalSettlement();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FinalSettlementDA.Get(nEmployeeSettlementID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFinalSettlement = CreateObject(oReader);
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

                oFinalSettlement.ErrorMessage = e.Message;
                #endregion
            }

            return oFinalSettlement;
        }

    }
}
