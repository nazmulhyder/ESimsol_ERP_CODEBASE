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

    public class AttendanceSchemeService : MarshalByRefObject, IAttendanceSchemeService
    {
        #region Private functions and declaration
        private AttendanceScheme MapObject(NullHandler oReader)
        {
            AttendanceScheme oAttendanceScheme = new AttendanceScheme();
            oAttendanceScheme.AttendanceSchemeID = oReader.GetInt32("AttendanceSchemeID");
            oAttendanceScheme.CompanyID = oReader.GetInt32("CompanyID");
            oAttendanceScheme.RosterPlanID = oReader.GetInt32("RosterPlanID");
            oAttendanceScheme.AttendanceCalenderID = oReader.GetInt32("AttendanceCalenderID");
            oAttendanceScheme.Name = oReader.GetString("Name");
            oAttendanceScheme.Code = oReader.GetString("Code");
            oAttendanceScheme.OvertimeCalculateInMinuteAfter = oReader.GetInt32("OvertimeCalculateInMinuteAfter");
            oAttendanceScheme.Accomodation = oReader.GetBoolean("Accomodation");
            oAttendanceScheme.AccommodationDeferredDay = oReader.GetInt32("AccommodationDeferredDay");
            oAttendanceScheme.AccommodationActivationAfter = (EnumRecruitmentEvent)oReader.GetInt16("AccommodationActivationAfter");
            oAttendanceScheme.EnforceMonthClosingRoster = oReader.GetBoolean("EnforceMonthClosingRoster");
            oAttendanceScheme.OverTime = oReader.GetBoolean("OverTime");
            oAttendanceScheme.OverTimeDeferredDay = oReader.GetInt32("OverTimeDeferredDay");
            oAttendanceScheme.OverTimeActivationAfter = (EnumRecruitmentEvent)oReader.GetInt32("OverTimeActivationAfter");
            oAttendanceScheme.IsOTCalTimeStartFromShiftStart = oReader.GetBoolean("IsOTCalTimeStartFromShiftStart");
            oAttendanceScheme.MaxOTInMinutePerDay = oReader.GetInt32("MaxOTInMinutePerDay");
            oAttendanceScheme.BreakageTimeInMinute = oReader.GetInt32("BreakageTimeInMinute");
            oAttendanceScheme.IsActive = oReader.GetBoolean("IsActive");
            
            oAttendanceScheme.AttendanceCalendar = oReader.GetString("AttendanceCalendar");
            oAttendanceScheme.DayOff = oReader.GetString("DayOff");
            oAttendanceScheme.RosterPlanDescription = oReader.GetString("RosterPlanDescription");
            oAttendanceScheme.RosterCycle = oReader.GetDouble("RosterCycle");

            return oAttendanceScheme;
        }

        private AttendanceScheme CreateObject(NullHandler oReader)
        {
            AttendanceScheme oAttendanceScheme = MapObject(oReader);
            return oAttendanceScheme;
        }

        private List<AttendanceScheme> CreateObjects(IDataReader oReader)
        {
            List<AttendanceScheme> oAttendanceScheme = new List<AttendanceScheme>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceScheme oItem = CreateObject(oHandler);
                oAttendanceScheme.Add(oItem);
            }
            return oAttendanceScheme;
        }

        #endregion

        #region Interface implementation
        public AttendanceSchemeService() { }

        public AttendanceScheme IUD(AttendanceScheme oAttendanceScheme, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            AttendanceScheme oAS = new AttendanceScheme();
            try
            {
                tc = TransactionContext.Begin(true);
                List<AttendanceSchemeLeave> AttendanceSchemeLeaves = new List<AttendanceSchemeLeave>();
                AttendanceSchemeLeave oAttendanceSchemeLeave = new AttendanceSchemeLeave();
                AttendanceSchemeLeaves = oAttendanceScheme.AttendanceSchemeLeaves;

                List<AttendanceSchemeHoliday> AttendanceSchemeHolidays = new List<AttendanceSchemeHoliday>();
                AttendanceSchemeHoliday oAttendanceSchemeHoliday = new AttendanceSchemeHoliday();
                AttendanceSchemeHolidays = oAttendanceScheme.AttendanceSchemeHolidays;

                List<AttendanceSchemeDayOff> AttendanceSchemeDayOffs = new List<AttendanceSchemeDayOff>();
                AttendanceSchemeDayOff oAttendanceSchemeDayOff = new AttendanceSchemeDayOff();
                AttendanceSchemeDayOffs = oAttendanceScheme.AttendanceSchemeDayOffs;

                IDataReader reader;
                reader = AttendanceSchemeDA.IUD(tc, oAttendanceScheme, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oAS = CreateObject(oReader);
                }
                reader.Close();

                

                if (oAS.AttendanceSchemeID > 0)
                {
                    #region Attendance Scheme Leave Part
                    if (AttendanceSchemeLeaves != null)
                    {
                        foreach (AttendanceSchemeLeave oItem in AttendanceSchemeLeaves)
                        {
                            IDataReader readerDetail;
                            oItem.AttendanceSchemeID = oAS.AttendanceSchemeID;
                            if (oItem.AttendanceSchemeLeaveID > 0)
                            {
                                readerDetail = AttendanceSchemeLeaveDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Update);
                            }
                            else
                            {
                                readerDetail = AttendanceSchemeLeaveDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Insert);
                            }
                            readerDetail.Close();
                        }
                    }
                    #endregion

                    # region Attendance Scheme Holiday
                    if (AttendanceSchemeHolidays != null)
                    {
                        foreach (AttendanceSchemeHoliday oItem in AttendanceSchemeHolidays)
                        {
                            IDataReader readerDetail;
                            oItem.AttendanceSchemeID = oAS.AttendanceSchemeID;
                            if (oItem.AttendanceSchemeHolidayID > 0)
                            {
                                readerDetail = AttendanceSchemeHolidayDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Update);
                            }
                            else
                            {
                                readerDetail = AttendanceSchemeHolidayDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Insert);
                            }
                            readerDetail.Close();
                        }
                    }
                    #endregion

                    #region Attendance Scheme Day Off
                    if (AttendanceSchemeDayOffs != null)
                    {

                        AttendanceSchemeDayOffDA.Delete(tc, oAS.AttendanceSchemeID,"0");
                        foreach (AttendanceSchemeDayOff oItem in AttendanceSchemeDayOffs)
                        {
                            IDataReader readerDetail;
                            oItem.AttendanceSchemeID = oAS.AttendanceSchemeID;
                            //if (oItem.AttendanceSchemeDayOffID > 0)
                            //{
                            //    readerDetail = AttendanceSchemeDayOffDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Update);
                            //}
                            //else {
                            readerDetail = AttendanceSchemeDayOffDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Insert);
                            //}
                            readerDetail.Close();
                        }
                    }

                    #endregion
                }
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete) { oAS.ErrorMessage = Global.DeleteMessage; }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAS.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oAS.AttendanceSchemeID = 0;
                #endregion
            }
            return oAS;
        }

        public AttendanceScheme Get(int id, Int64 nUserId)
        {
            AttendanceScheme aAttendanceScheme = new AttendanceScheme();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AttendanceSchemeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aAttendanceScheme = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get AttendanceScheme", e);
                #endregion
            }

            return aAttendanceScheme;
        }

        public List<AttendanceScheme> Gets(Int64 nUserID)
        {
            List<AttendanceScheme> oAttendanceScheme = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceSchemeDA.Gets(tc);
                oAttendanceScheme = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceScheme", e);
                #endregion
            }
            return oAttendanceScheme;
        }
        public List<AttendanceScheme> Gets(string sSQL, Int64 nUserID)
        {
            List<AttendanceScheme> oAttendanceScheme = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceSchemeDA.Gets(tc, sSQL);
                oAttendanceScheme = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceScheme", e);
                #endregion
            }
            return oAttendanceScheme;
        }

        public AttendanceScheme ActiveInActive(AttendanceScheme oAttendanceScheme, Int64 nUserID)
        {
            TransactionContext tc = null;
            AttendanceScheme oAS = new AttendanceScheme();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                //reader = AttendanceSchemeDA.IUD(tc, oAttendanceScheme, nUserID, (int)EnumDBOperation.Update);
                reader = AttendanceSchemeDA.IUD(tc, oAttendanceScheme, nUserID, (int)EnumDBOperation.Active);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAS = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAS.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oAS.AttendanceSchemeID = 0;
                #endregion
            }
            return oAS;
        }

        #endregion
    }
   
}
