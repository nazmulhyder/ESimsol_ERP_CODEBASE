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
    public class AttendanceProcessManagementDA
    {
        public AttendanceProcessManagementDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, AttendanceProcessManagement oAttendanceProcessManagement, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceProcessManagement] %n,%n,%n,%n,%n,%n,%n,%d,%n,%n,%n",
                   oAttendanceProcessManagement.APMID,
                   oAttendanceProcessManagement.CompanyID,
                   oAttendanceProcessManagement.LocationID,
                   oAttendanceProcessManagement.DepartmentID,
                   oAttendanceProcessManagement.ProcessType,
                   oAttendanceProcessManagement.ShiftID,
                   oAttendanceProcessManagement.Status,
                   oAttendanceProcessManagement.AttendanceDate,
                   nUserID,
                   oAttendanceProcessManagement.BusinessUnitID,
                   nDBOperation);

        }
        public static IDataReader IUD_V2(TransactionContext tc, AttendanceProcessManagement oAttendanceProcessManagement, Int64 nUserID, EnumDBOperation eDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceProcessManagementV2] %n,%n,%n,%n,%n,%n,%d,%s,%n,%n,%n",
                   oAttendanceProcessManagement.APMID,
                   oAttendanceProcessManagement.CompanyID,
                   oAttendanceProcessManagement.LocationID,
                   oAttendanceProcessManagement.DepartmentID,
                   oAttendanceProcessManagement.ProcessType,
                   oAttendanceProcessManagement.Status,
                   oAttendanceProcessManagement.AttendanceDate,
                   oAttendanceProcessManagement.ErrorMessage,
                   nUserID,
                   oAttendanceProcessManagement.BusinessUnitID,
                   (int)eDBOperation);

        }

        public static void Delete_V2(TransactionContext tc, AttendanceProcessManagement oAttendanceProcessManagement, Int64 nUserID, EnumDBOperation eDBOperation)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_AttendanceProcessManagementV2] %n,%n,%n,%n,%n,%n,%d,%s,%n,%n,%n",
               oAttendanceProcessManagement.APMID,
               oAttendanceProcessManagement.CompanyID,
               oAttendanceProcessManagement.LocationID,
               oAttendanceProcessManagement.DepartmentID,
               oAttendanceProcessManagement.ProcessType,
               oAttendanceProcessManagement.Status,
               oAttendanceProcessManagement.AttendanceDate,
               oAttendanceProcessManagement.ErrorMessage,
               nUserID,
               oAttendanceProcessManagement.BusinessUnitID,
               (int)eDBOperation);
        }
        public static void ProcessAttendanceDaily(TransactionContext tc, AttendanceProcessManagement oAttendanceProcessManagement, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_Process_AttendanceDaily] %n,%n,%n,%n,%n,%d,%n,%n",
                   oAttendanceProcessManagement.BusinessUnitID,
                   oAttendanceProcessManagement.CompanyID,
                   oAttendanceProcessManagement.LocationID,
                   oAttendanceProcessManagement.DepartmentID,
                   oAttendanceProcessManagement.ShiftID,
                   oAttendanceProcessManagement.AttendanceDate,
                   oAttendanceProcessManagement.APMID,
                   nUserID);

        }

        public static int ProcessCompAsPerEdit(TransactionContext tc, string EmployeeID, DateTime Startdate, DateTime EndDate, int MOCID, int nIndex, string sLocationIDs, string sBUIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            object oIndex = tc.ExecuteScalar("EXEC [SP_Process_ComplianceAttendanceProcessAsPerEdit] %d,%d,%s,%n,%n,%n, %s, %s",
                   Startdate,
                   EndDate,
                   EmployeeID,
                   MOCID,
                   nUserID,
                   nIndex,
                   sLocationIDs,
                   sBUIDs);
            if (oIndex != null)
            {
                nNewIndex = Convert.ToInt32(oIndex);
            }
            return nNewIndex;
        }
        public static int ProcessAttendanceDaily_V1(TransactionContext tc, AttendanceProcessManagement oAttendanceProcessManagement, int nIndex, Int64 nUserID)
        {
            int nNewIndex = 0;
            object oIndex = tc.ExecuteScalar("EXEC [SP_Process_AttendanceDaily_V1] %n,%n,%n,%n,%n,%d,%n,%n,%n",
                   oAttendanceProcessManagement.BusinessUnitID,
                   oAttendanceProcessManagement.CompanyID,
                   oAttendanceProcessManagement.LocationID,
                   oAttendanceProcessManagement.DepartmentID,
                   oAttendanceProcessManagement.ShiftID,
                   oAttendanceProcessManagement.AttendanceDate,
                   oAttendanceProcessManagement.APMID,
                   nUserID, nIndex);
            if (oIndex != null)
            {
                nNewIndex = Convert.ToInt32(oIndex);
            }
            return nNewIndex;
        }

        public static void ProcessDataCollectionRT(TransactionContext tc, RTPunchLog oRTP, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC SP_Process_DataCollectionRT %s,%s,%s",
                   oRTP.C_Date, oRTP.C_Time, oRTP.C_Unique);

        }

        public static IDataReader EmployeeWiseReprocess(int EmployeeID, DateTime Startdate, DateTime EndDate, TransactionContext tc, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_AttendanceDaily_EmployeeWise] %n,%d,%d,%n",
                   EmployeeID, Startdate, EndDate,
                   nUserID);
        }

        public static int ProcessBreezeAbsent(TransactionContext tc, int nIndex, DateTime StartDate, DateTime EndDate, Int64 nUserID)
        {
            int nNewIndex = 0;
            object oIndex = tc.ExecuteScalar("EXEC [SP_Process_BreezeAbsent] %n,%d,%d,%n",
            nIndex, StartDate, EndDate, nUserID);
            if (oIndex != null)
            {
                nNewIndex = Convert.ToInt32(oIndex);
            }
            return nNewIndex;
        }

        public static void ManualAttendanceProcess(TransactionContext tc, string sBUIDs, string sLocationIDs, string sDepartmentIDs, int nEmployeeID, DateTime dAttendanceDate, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_Process_AttendanceDaily_AMG] %s, %s, %s, %d, %n, %n, %n", sBUIDs, sLocationIDs, sDepartmentIDs, dAttendanceDate, nUserID, nEmployeeID, 3); //Here 3 Means SP Execute From manual Process
        }

        public static void ManualCompAttendanceProcess(TransactionContext tc, string sBUIDs, string sLocationIDs, string sDepartmentIDs, DateTime dAttendanceDate, int nMOCID, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_Process_CompAttendanceDaily_AMG] %s, %s, %s, %d, %n, %n", sBUIDs, sLocationIDs, sDepartmentIDs, dAttendanceDate, nMOCID, nUserID);
        }
        public static void APMProcess(TransactionContext tc, string sBUIDs, string sLocationIDs, string sDepartmentIDs, DateTime dStartDate, DateTime dEndDate, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_APM_Process_AMG] %d, %d, %s, %s, %s, %n", dStartDate, dEndDate, sBUIDs, sLocationIDs, sDepartmentIDs, nUserID);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(TransactionContext tc, int nAPMID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AttendanceProcessManagement WHERE APMID=%n", nAPMID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_AttendanceProcessManagement");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
