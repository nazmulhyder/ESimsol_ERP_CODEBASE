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
    public class MarketingScheduleService : MarshalByRefObject, IMarketingScheduleService
    {
        #region Private functions and declaration
        private MarketingSchedule MapObject(NullHandler oReader)
        {
            MarketingSchedule oMarketingSchedule = new MarketingSchedule();
            oMarketingSchedule.MarketingScheduleID = (oReader.GetInt32("MarketingScheduleID") == null) ? 0 : oReader.GetInt32("MarketingScheduleID");
            oMarketingSchedule.ScheduleNo = (oReader.GetString("ScheduleNo") == null) ? "" : oReader.GetString("ScheduleNo");
            oMarketingSchedule.MKTPersonID = (oReader.GetInt32("MKTPersonID") == null) ? 0 : oReader.GetInt32("MKTPersonID");
            oMarketingSchedule.BuyerID = (oReader.GetInt32("BuyerID") == null) ? 0 : oReader.GetInt32("BuyerID");
            oMarketingSchedule.ScheduleDateTime = (oReader.GetDateTime("ScheduleDateTime") == null) ? DateTime.Now : oReader.GetDateTime("ScheduleDateTime");
            oMarketingSchedule.MeetingLocation = (oReader.GetString("MeetingLocation") == null) ? "" : oReader.GetString("MeetingLocation");
            oMarketingSchedule.MeetingDuration = (oReader.GetString("MeetingDuration") == null) ? "" : oReader.GetString("MeetingDuration");
            oMarketingSchedule.Remarks = (oReader.GetString("Remarks") == null) ? "" : oReader.GetString("Remarks");
            oMarketingSchedule.ScheduleAssignBy = (oReader.GetInt32("ScheduleAssignBy") == null) ? 0 : oReader.GetInt32("ScheduleAssignBy");
            oMarketingSchedule.MKTPersonName = (oReader.GetString("MKTPersonName") == null) ? "" : oReader.GetString("MKTPersonName");
            oMarketingSchedule.BuyerName = (oReader.GetString("BuyerName") == null) ? "" : oReader.GetString("BuyerName");
            oMarketingSchedule.BuyerShortName = (oReader.GetString("BuyerShortName") == null) ? "" : oReader.GetString("BuyerShortName");
            oMarketingSchedule.MeetingSummaryText = (oReader.GetString("MeetingSummaryText") == null) ? "" : oReader.GetString("MeetingSummaryText");
            oMarketingSchedule.ScheduleAssignByName = (oReader.GetString("ScheduleAssignByName") == null) ? "" : oReader.GetString("ScheduleAssignByName");
            return oMarketingSchedule;
        }

        public MarketingSchedule CreateObject(NullHandler oReader)
        {
            MarketingSchedule oMarketingSchedule = new MarketingSchedule();
            oMarketingSchedule = MapObject(oReader);
            return oMarketingSchedule;
        }

        private  List<MarketingSchedule> CreateObjects(IDataReader oReader)
        {
            List<MarketingSchedule> oMarketingSchedule = new List<MarketingSchedule>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MarketingSchedule oItem = CreateObject(oHandler);
                oMarketingSchedule.Add(oItem);
            }
            return oMarketingSchedule;
        }

        #endregion

        #region Interface implementation
        public MarketingScheduleService() { }

        public MarketingSchedule Save(MarketingSchedule oMarketingSchedule, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oMarketingSchedule.MarketingScheduleID <= 0)
                {
                    reader = MarketingScheduleDA.InsertUpdate(tc, oMarketingSchedule, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = MarketingScheduleDA.InsertUpdate(tc, oMarketingSchedule, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMarketingSchedule = new MarketingSchedule();
                    oMarketingSchedule = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save MarketingSchedule. Because of " + e.Message, e);
                #endregion
            }
            return oMarketingSchedule;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MarketingSchedule oMarketingSchedule = new MarketingSchedule();
                oMarketingSchedule.MarketingScheduleID = id;
                DBTableReferenceDA.HasReference(tc, "MarketingSchedule", id);
                MarketingScheduleDA.Delete(tc, oMarketingSchedule, EnumDBOperation.Delete, nUserId);
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

        public MarketingSchedule Get(int id, Int64 nUserId)
        {
            MarketingSchedule oMarketingSchedule = new MarketingSchedule();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = MarketingScheduleDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMarketingSchedule = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get MarketingSchedule", e);
                #endregion
            }
            return oMarketingSchedule;
        }
        //public MarketingSchedule GetByFabricID(int nBuyerID, int nFabricID, int nUserID)
        //{
        //    MarketingSchedule oMarketingSchedule = new MarketingSchedule();
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin();
        //        IDataReader reader = MarketingScheduleDA.GetByFabricID(tc, nBuyerID, nFabricID, nUserID);
        //        NullHandler oReader = new NullHandler(reader);
        //        if (reader.Read())
        //        {
        //            oMarketingSchedule = CreateObject(oReader);
        //        }


        //        reader.Close();
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();
        //        ExceptionLog.Write(e);
        //        throw new ServiceException("Failed to Get MarketingSchedule", e);
        //        #endregion
        //    }
        //    return oMarketingSchedule;
        //}
        public List<MarketingSchedule> GetsByCurrentMonth(DateTime dMonth, int nMKTPersonID, Int64 nUserID)
        {
            List<MarketingSchedule> oMarketingSchedules = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MarketingScheduleDA.GetsByCurrentMonth(tc, dMonth, nMKTPersonID);
                oMarketingSchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MarketingSchedule", e);
                #endregion
            }
            return oMarketingSchedules;
        }
        public List<MarketingSchedule> GetsByCurrentMonth(DateTime dMonth, Int64 nUserID)
        {
            List<MarketingSchedule> oMarketingSchedules = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MarketingScheduleDA.GetsByCurrentMonth(tc, dMonth);
                oMarketingSchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MarketingSchedule", e);
                #endregion
            }
            return oMarketingSchedules;
        }
        public List<MarketingSchedule> Gets(DateTime dScheduleDateTime, long nUserID)
        {
            List<MarketingSchedule> oMarketingSchedules = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MarketingScheduleDA.Gets(tc, dScheduleDateTime);
                oMarketingSchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MarketingSchedule", e);
                #endregion
            }
            return oMarketingSchedules;
        }
        public List<MarketingSchedule> Gets(int nMKTPersonID, DateTime dScheduleDateTime, long nUserID)
        {
            List<MarketingSchedule> oMarketingSchedules = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MarketingScheduleDA.Gets(tc, nMKTPersonID, dScheduleDateTime);
                oMarketingSchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MarketingSchedule", e);
                #endregion
            }
            return oMarketingSchedules;
        }
        public List<MarketingSchedule> Gets(Int64 nUserID)
        {
            List<MarketingSchedule> oMarketingSchedules = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MarketingScheduleDA.Gets(tc);
                oMarketingSchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MarketingSchedule", e);
                #endregion
            }
            return oMarketingSchedules;
        }
        public List<MarketingSchedule> Gets(string sSQL, Int64 nUserID)
        {
            List<MarketingSchedule> oMarketingSchedules = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MarketingScheduleDA.Gets(tc, sSQL);
                oMarketingSchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MarketingSchedule", e);
                #endregion
            }
            return oMarketingSchedules;
        }


       
        #endregion
    }   
}