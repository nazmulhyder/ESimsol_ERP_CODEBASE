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
    public class EarnLeaveService : MarshalByRefObject, IEarnLeaveService
    {
        #region Private functions and declaration
        private EarnLeave MapObject(NullHandler oReader)
        {
            EarnLeave oEarnLeave = new EarnLeave();
            oEarnLeave.EmpLeaveLedgerID = oReader.GetInt32("EmpLeaveLedgerID");
            oEarnLeave.EmployeeID = oReader.GetInt32("EmployeeID");
            oEarnLeave.Code = oReader.GetString("Code");
            oEarnLeave.Name = oReader.GetString("Name");
            oEarnLeave.LocationName = oReader.GetString("LocationName");
            oEarnLeave.DPTName = oReader.GetString("DPTName");
            oEarnLeave.DSGName = oReader.GetString("DSGName");
            oEarnLeave.DateOfJoin = oReader.GetDateTime("DateOfJoin");
            oEarnLeave.PresentDayBalance = oReader.GetInt32("PresentDayBalance");
            oEarnLeave.LastProcessDate = oReader.GetDateTime("LastProcessDate");
            oEarnLeave.PreviousLastDate = oReader.GetDateTime("PreviousLastDate");
            oEarnLeave.ClassifiedEL = oReader.GetInt32("ClassifiedEL");
            oEarnLeave.PresencePerLeave = oReader.GetInt32("PresencePerLeave");
            oEarnLeave.Enjoyed = oReader.GetInt32("Enjoyed");
            //oEarnLeave.Present = oReader.GetInt32("Present");
            oEarnLeave.RunningEL = oReader.GetInt32("RunningEL");
            oEarnLeave.PresentONAtt = oReader.GetInt32("PresentONAtt");
            oEarnLeave.AbsentOnAtt = oReader.GetInt32("AbsentOnAtt");
            oEarnLeave.DayoffOnAtt = oReader.GetInt32("DayoffOnAtt");
            oEarnLeave.HolidayOnAtt = oReader.GetInt32("HolidayOnAtt");
            oEarnLeave.LeaveOnAtt = oReader.GetInt32("LeaveOnAtt");
            oEarnLeave.TotalLeave = oReader.GetInt32("TotalLeave");
            oEarnLeave.TotalDays = oReader.GetInt32("TotalDays");


            oEarnLeave.TotalHoliday = oReader.GetInt32("TotalHoliday");
            oEarnLeave.TotalDayOff = oReader.GetInt32("TotalDayOff");
            oEarnLeave.TotalAbsent = oReader.GetInt32("TotalAbsent");
            oEarnLeave.TotalPresent = oReader.GetInt32("TotalPresent");
            oEarnLeave.TotalLeaveOnAttendance = oReader.GetInt32("TotalLeaveOnAttendance");


            oEarnLeave.LocationID = oReader.GetInt32("LocationID");
            oEarnLeave.DesignationID = oReader.GetInt32("DesignationID");
            oEarnLeave.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oEarnLeave.DepartmentID = oReader.GetInt32("DepartmentID");

            return oEarnLeave;
        }

        private EarnLeave CreateObject(NullHandler oReader)
        {
            EarnLeave oEarnLeave = MapObject(oReader);
            return oEarnLeave;
        }

        private List<EarnLeave> CreateObjects(IDataReader oReader)
        {
            List<EarnLeave> oEarnLeave = new List<EarnLeave>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EarnLeave oItem = CreateObject(oHandler);
                oEarnLeave.Add(oItem);
            }
            return oEarnLeave;
        }

        #endregion

        #region Interface implementation
        public EarnLeaveService() { }
        public List<EarnLeave> ELGets(string EmployeeIDs, string sLocationID, string DepartmentIDs, string DesignationIDs, DateTime Date, string sBUnit, int nLoadRecordsS, int nRowLengthS, int isAll, Int64 nUserID)
        {
            List<EarnLeave> oEarnLeaves = new List<EarnLeave>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EarnLeaveDA.ELGets(tc, EmployeeIDs,sLocationID, DepartmentIDs, DesignationIDs, Date,sBUnit, nLoadRecordsS, nRowLengthS, isAll, nUserID);
                oEarnLeaves = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oEarnLeaves = new List<EarnLeave>();
                EarnLeave oEarnLeave = new EarnLeave();
                oEarnLeave.ErrorMessage = e.Message;
                oEarnLeaves.Add(oEarnLeave);
                #endregion
            }
            return oEarnLeaves;
        }


        public List<EarnLeave> ELGetsClassified(string sSQL, Int64 nUserId)
        {
            List<EarnLeave> oEarnLeave = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EarnLeaveDA.ELGetsClassified(tc, sSQL);
                oEarnLeave = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Earn Leave ", e);
                #endregion
            }

            return oEarnLeave;
        }
        public EarnLeave ELProcess(int EmpLeaveLedgerID, DateTime LastProcessDate, Int64 nUserID)
        {
            EarnLeave oEL = new EarnLeave();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = EarnLeaveDA.ELProcess(tc, EmpLeaveLedgerID, LastProcessDate, nUserID);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oEL = new EarnLeave();
                oEL.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oEL;
        }

        public List<EarnLeave> ELGetsToEncash(string sBusinessUnitIds, string sEmployeeIDs, string sLocationID, string sDepartmentIds, string sDesignationIDs, int nACSID, double nStartSalaryRange, double nEndSalaryRange, int nLoadRecordsS, int nRowLengthS, Int64 nUserID)
        {
            List<EarnLeave> oEarnLeaves = new List<EarnLeave>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                reader = EarnLeaveDA.ELGetsToEncash(tc, sBusinessUnitIds, sEmployeeIDs, sLocationID, sDepartmentIds, sDesignationIDs, nACSID, nStartSalaryRange, nEndSalaryRange, nLoadRecordsS, nRowLengthS, nUserID);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    EarnLeave oItem = new EarnLeave();
                    oItem.EmpLeaveLedgerID = oreader.GetInt32("EmpLeaveLedgerID");
                    oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                    oItem.Code = oreader.GetString("Code");
                    oItem.Name = oreader.GetString("Name");
                    oItem.DPTName = oreader.GetString("DPTName");
                    oItem.DSGName = oreader.GetString("DSGName");
                    oItem.DateOfJoin = oreader.GetDateTime("DateOfJoin");
                    oItem.PresentDayBalance = oreader.GetInt32("PresentDayBalance");
                    oItem.LastProcessDate = oreader.GetDateTime("LastProcessDate");
                    oItem.ClassifiedEL = oreader.GetInt32("ClassifiedEL");
                    oItem.PresencePerLeave = oreader.GetInt32("PresencePerLeave");
                    oItem.Enjoyed = oreader.GetInt32("Enjoyed");
                    oItem.Present = oreader.GetInt32("Present");
                    oItem.RunningEL = oreader.GetInt32("RunningEL");
                    //oItem.PresentONAtt = oreader.GetInt32("PresentONAtt");
                    //oItem.AbsentOnAtt = oreader.GetInt32("AbsentOnAtt");
                    //oItem.DayoffOnAtt = oreader.GetInt32("DayoffOnAtt");
                    //oItem.HolidayOnAtt = oreader.GetInt32("HolidayOnAtt");
                    //oItem.LeaveOnAtt = oreader.GetInt32("LeaveOnAtt");
                    //oItem.PreviousLastDate = oreader.GetDateTime("PreviousLastDate");
                    oEarnLeaves.Add(oItem);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oEarnLeaves = new List<EarnLeave>();
                EarnLeave oEarnLeave = new EarnLeave();
                oEarnLeave.ErrorMessage = e.Message;
                oEarnLeaves.Add(oEarnLeave);
                #endregion
            }
            return oEarnLeaves;
        }

        public int EncashEL(int nInxex, string sEmpLeaveLedgerIDs, DateTime DeclarationDate, int nACSID, int nConsiderEL, bool IsApplyforallemployee, bool IsEncashPresentBalance, Int64 nUserID)
        {
            EarnLeave oEarnLeave = new EarnLeave();
            int nNewIndex = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                //IDataReader reader = null;
                nNewIndex = EarnLeaveDA.EncashEL(tc, nInxex, sEmpLeaveLedgerIDs, DeclarationDate, nACSID, nConsiderEL, IsApplyforallemployee, IsEncashPresentBalance, nUserID);
                //oEarnLeaves = CreateObjects(reader);
                //reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oEarnLeave = new EarnLeave();
                oEarnLeave.ErrorMessage = e.Message;
                #endregion
            }
            return nNewIndex;
        }
        #endregion
    }
}
