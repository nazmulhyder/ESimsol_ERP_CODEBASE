using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class RosterPlanEmployeeDA
    {
        public RosterPlanEmployeeDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, RosterPlanEmployee oRosterPlanEmployee, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RosterPlanEmployee] %n, %n, %n, %b, %b, %D, %D, %d, %n, %n, %n, %s, %n",
                   oRosterPlanEmployee.RPEID, oRosterPlanEmployee.EmployeeID, oRosterPlanEmployee.ShiftID, oRosterPlanEmployee.IsDayOff, oRosterPlanEmployee.IsHoliday, oRosterPlanEmployee.InTime, oRosterPlanEmployee.OutTime, oRosterPlanEmployee.AttendanceDate,  nUserID, oRosterPlanEmployee.MaxOTInMin,oRosterPlanEmployee.IsGWD, oRosterPlanEmployee.Remarks, nDBOperation);

        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(int nRPEID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RosterPlanEmployee WHERE RPEID=%n", nRPEID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RosterPlanEmployee ");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Search_ShiftRostering] %s,%n,%d,%d",
                EmployeeIDs, ShiftID, StartDate, EndDate);
        }
        public static IDataReader Transfer(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate, Int64 nUserID, int nOT_In_Minute, bool IsDayOff, int nDBOperation, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ShiftRostering_Transfer] %s, %n, %d, %d, %n, %n, %b, %n",
                EmployeeIDs, ShiftID, StartDate, EndDate, nUserID, nOT_In_Minute, IsDayOff, nDBOperation);
        }
        public static IDataReader TransferDept(DateTime StartDate, DateTime EndDate, int BUID, string LocationIDs, string DepartmentIDs, int ShiftID, DateTime InTime, DateTime OutTime, bool IsGWD, bool IsDayOff, string Remarks, int MaxOTDateTime, string EmployeeIDs, string DesignationIDs, bool isOfficial, DateTime RosterDate, string GroupIDs, string BlockIDs, int TrsShiftID, Int64 nUserID, int nDBOperation, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ShiftRostering_Transfer_MultiDept] %n,%s,%s,%s,%s,%n,%b,%d,%s,%s ,%d,%d,%D,%D,%n,%b,%b,%s,%n,%n",
                BUID
	            ,LocationIDs
	            ,DepartmentIDs
	            ,DesignationIDs
	            ,EmployeeIDs
	            ,ShiftID
	            ,isOfficial
	            ,RosterDate
	            ,GroupIDs
	            ,BlockIDs
	            ,StartDate
	            ,EndDate
	            ,InTime
                ,OutTime
                ,TrsShiftID
	            ,IsGWD
	            ,IsDayOff
	            ,Remarks
	            ,MaxOTDateTime
	            ,nUserID
                );
        }
        public static IDataReader Swap(int RosterPlanID, DateTime StartDate, DateTime EndDate, Int64 nUserID, int nDBOperation, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ShiftRostering_Swap] %n,%d,%d,%n,%n",
                 RosterPlanID, StartDate, EndDate, nUserID, nDBOperation);
        }
        #endregion


        public static IDataReader Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_RosterPlanEmployee] %s,%s,%s,%s,%s,%s,%d,%d,%s,%n, %s, %s,%n,%n,%s, %s",
                  sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, nUserID, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds, sRemarks);
        }
        public static IDataReader UploadRosterEmpXL(TransactionContext tc, RosterPlanEmployee oRosterPlanEmployee, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Upload_RosterPlanEmployee_UploadXL]%s,%s,%d,%n",
                   oRosterPlanEmployee.EmployeeCode,
                   oRosterPlanEmployee.ErrorMessage,//Shift/Dayoff
                   oRosterPlanEmployee.AttendanceDate,// att start date
                   nUserID
                   );
        }
    }
}
