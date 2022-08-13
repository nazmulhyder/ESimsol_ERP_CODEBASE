using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class MeetingSummaryService : MarshalByRefObject, IMeetingSummaryService
    {
        #region Private functions and declaration
        private MeetingSummary MapObject(NullHandler oReader)
        {
            MeetingSummary oMeetingSummary = new MeetingSummary();
            oMeetingSummary.MeetingSummaryID = (oReader.GetInt32("MeetingSummaryID") == null) ? 0 : oReader.GetInt32("MeetingSummaryID");
            oMeetingSummary.MarketingScheduleID = (oReader.GetInt32("MarketingScheduleID") == null) ? 0 : oReader.GetInt32("MarketingScheduleID");
            oMeetingSummary.MeetingSummarizeBy = (oReader.GetInt32("MeetingSummarizeBy") == null) ? 0 : oReader.GetInt32("MeetingSummarizeBy");
            oMeetingSummary.MeetingSummaryText = (oReader.GetString("MeetingSummaryText") == null) ? "" : oReader.GetString("MeetingSummaryText");
            oMeetingSummary.MarketingScheduleNo = (oReader.GetString("MarketingScheduleNo") == null) ? "" : oReader.GetString("MarketingScheduleNo");
            oMeetingSummary.MeetingSummarizeByName = (oReader.GetString("MeetingSummarizeByName") == null) ? "" : oReader.GetString("MeetingSummarizeByName");
            oMeetingSummary.DBServerDateTime = (oReader.GetDateTime("DBServerDateTime") == null) ? DateTime.Now : oReader.GetDateTime("DBServerDateTime");
            oMeetingSummary.BuyerID = (oReader.GetInt32("BuyerID") == null) ? 0 : oReader.GetInt32("BuyerID");
            oMeetingSummary.BuyerName = (oReader.GetString("BuyerName") == null) ? "": oReader.GetString("BuyerName");
            oMeetingSummary.BuyerShortName = (oReader.GetString("BuyerShortName") == null) ? "" : oReader.GetString("BuyerShortName");

            oMeetingSummary.RefID = oReader.GetInt32("RefID");
            oMeetingSummary.RefType = (EnumMKTRef)oReader.GetInt32("RefType");
            return oMeetingSummary;
        }

        private MeetingSummary CreateObject(NullHandler oReader)
        {
            MeetingSummary oMeetingSummary = new MeetingSummary();
            oMeetingSummary = MapObject(oReader);
            return oMeetingSummary;
        }

        private List<MeetingSummary> CreateObjects(IDataReader oReader)
        {
            List<MeetingSummary> oMeetingSummary = new List<MeetingSummary>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MeetingSummary oItem = CreateObject(oHandler);
                oMeetingSummary.Add(oItem);
            }
            return oMeetingSummary;
        }

        #endregion

        #region Interface implementation
        public MeetingSummaryService() { }

        public MeetingSummary Save(MeetingSummary oMeetingSummary, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oMeetingSummary.MeetingSummaryID <= 0)
                {
                    reader = MeetingSummaryDA.InsertUpdate(tc, oMeetingSummary, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = MeetingSummaryDA.InsertUpdate(tc, oMeetingSummary, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMeetingSummary = new MeetingSummary();
                    oMeetingSummary = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save MeetingSummary. Because of " + e.Message, e);
                #endregion
            }
            return oMeetingSummary;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MeetingSummary oMeetingSummary = new MeetingSummary();
                oMeetingSummary.MeetingSummaryID = id;
                DBTableReferenceDA.HasReference(tc, "MeetingSummary", id);
                MeetingSummaryDA.Delete(tc, oMeetingSummary, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public MeetingSummary Get(int id, Int64 nUserId)
        {
            MeetingSummary oMeetingSummary = new MeetingSummary();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = MeetingSummaryDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMeetingSummary = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get MeetingSummary", e);
                #endregion
            }
            return oMeetingSummary;
        }
        public MeetingSummary SaveFromFabric(MeetingSummary oMeetingSummary, int nMKTPID, int nBuyerID, Int64 nUserId)
        {
            MeetingSummary objMeetingSummary = new MeetingSummary();
            objMeetingSummary = oMeetingSummary;
            List<MeetingSummary> oMeetingSummarys = new List<MeetingSummary>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                string sSQL = "SELECT top(1)* FROM View_MeetingSummary WHERE RefID = " + oMeetingSummary.RefID + " AND RefType = " + (int)oMeetingSummary.RefType;
                DateObject.CompareDateQuery(ref sSQL,"DBServerDateTime",(int)EnumCompareOperator.EqualTo,DateTime.Now,DateTime.Now);
                IDataReader reader = MeetingSummaryDA.Gets(tc, sSQL);
                oMeetingSummarys = CreateObjects(reader);
                reader.Close();
                
                if(oMeetingSummarys.Count>0)
                {
                    oMeetingSummary = oMeetingSummarys[0];
                    objMeetingSummary.MarketingScheduleID = oMeetingSummary.MarketingScheduleID;
                }
                else
                {
                     MarketingSchedule oMarketingSchedule = new MarketingSchedule();
                     oMarketingSchedule.BuyerID = nBuyerID;
                     oMarketingSchedule.MKTPersonID = nMKTPID;
                     oMarketingSchedule.MeetingLocation = "H/O";
                     oMarketingSchedule.ScheduleAssignBy = (int)nUserId;
                     oMarketingSchedule.MeetingDuration = "One Hour";
                     oMarketingSchedule.Remarks = "N/A";
                     IDataReader readerSchedule = MarketingScheduleDA.InsertUpdate(tc, oMarketingSchedule, EnumDBOperation.Insert, nUserId);

                    NullHandler oReader = new NullHandler(readerSchedule);
                    if (readerSchedule.Read())
                    {
                        MarketingScheduleService oMarketingScheduleService = new MarketingScheduleService();

                        oMarketingSchedule = oMarketingScheduleService.CreateObject(oReader);
                        objMeetingSummary.MarketingScheduleID = oMarketingSchedule.MarketingScheduleID;
                        objMeetingSummary.MeetingLocationAndTime = oMarketingSchedule.MeetingLocation + "@" + oMeetingSummary.DBServerDateTimeInString;
                    }
                    readerSchedule.Close();
                }
                IDataReader readerMeeting = MeetingSummaryDA.SaveFromFabric(tc, objMeetingSummary, EnumDBOperation.Insert, nUserId);
                NullHandler oReaderMeeting = new NullHandler(readerMeeting);
                if (readerMeeting.Read())
                {
                    MeetingSummaryService oMeetingSummaryService = new MeetingSummaryService();
                    oMeetingSummary = oMeetingSummaryService.CreateObject(oReaderMeeting);
                }
                readerMeeting.Close();
                tc.End();

                    
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MeetingSummary", e);
                #endregion
            }
            return oMeetingSummary;
        }



        
        public List<MeetingSummary> Gets(int nMarketingScheduleID, long nUserID)
        {
            List<MeetingSummary> oMeetingSummarys = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MeetingSummaryDA.Gets(tc,nMarketingScheduleID);
                oMeetingSummarys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MeetingSummary", e);
                #endregion
            }
            return oMeetingSummarys;
        }
        public List<MeetingSummary> GetsByRef(int nFabricID, int nRefType, long nUserID)
        {
            List<MeetingSummary> oMeetingSummarys = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MeetingSummaryDA.GetsByRef(tc, nFabricID, nRefType);
                oMeetingSummarys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MeetingSummary", e);
                #endregion
            }
            return oMeetingSummarys;
        }
        public List<MeetingSummary> Gets(Int64 nUserID)
        {
            List<MeetingSummary> oMeetingSummarys = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MeetingSummaryDA.Gets(tc);
                oMeetingSummarys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MeetingSummary", e);
                #endregion
            }
            return oMeetingSummarys;
        }
        public List<MeetingSummary> Gets(string sSQL, Int64 nUserID)
        {
            List<MeetingSummary> oMeetingSummarys = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MeetingSummaryDA.Gets(tc, sSQL);
                oMeetingSummarys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MeetingSummary", e);
                #endregion
            }
            return oMeetingSummarys;
        }

        
        #endregion
    }   
}