using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class HolidayCalendarDetailService : MarshalByRefObject, IHolidayCalendarDetailService
    {
        private HolidayCalendarDetail MapObject(NullHandler oReader)
        {
            HolidayCalendarDetail oHolidayCalendarDetail = new HolidayCalendarDetail();
            oHolidayCalendarDetail.HolidayCalendarDetailID = oReader.GetInt32("HolidayCalendarDetailID");
            oHolidayCalendarDetail.HolidayCalendarID = oReader.GetInt32("HolidayCalendarID");
            oHolidayCalendarDetail.HolidayID = oReader.GetInt32("HolidayID");
            oHolidayCalendarDetail.HolidayName = oReader.GetString("HolidayName");
            oHolidayCalendarDetail.CalendarApply = (EnumCalendarApply)oReader.GetInt16("CalendarApply");
            oHolidayCalendarDetail.StartDate = oReader.GetDateTime("StartDate");
            oHolidayCalendarDetail.EndDate = oReader.GetDateTime("EndDate");
            oHolidayCalendarDetail.Remarks = oReader.GetString("Remarks");
            return oHolidayCalendarDetail;
        }
        private HolidayCalendarDetail CreateObject(NullHandler oReader)
        {
            HolidayCalendarDetail oHolidayCalendarDetail = new HolidayCalendarDetail();
            oHolidayCalendarDetail = MapObject(oReader);
            return oHolidayCalendarDetail;
        }

        private List<HolidayCalendarDetail> CreateObjects(IDataReader oReader)
        {
            List<HolidayCalendarDetail> oHolidayCalendarDetail = new List<HolidayCalendarDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                HolidayCalendarDetail oItem = CreateObject(oHandler);
                oHolidayCalendarDetail.Add(oItem);
            }
            return oHolidayCalendarDetail;
        }

        public HolidayCalendarDetail Save(HolidayCalendarDetail oHolidayCalendarDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sHolidayCalendarDRPIDs = "";
            List<HolidayCalendarDRP> oHolidayCalendarDRPs = new List<HolidayCalendarDRP>();
            HolidayCalendarDRP oHolidayCalendarDRP = new HolidayCalendarDRP();
            oHolidayCalendarDRPs = oHolidayCalendarDetail.HolidayCalendarDRPs;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oHolidayCalendarDetail.HolidayCalendarDetailID <= 0)
                {
                    reader = HolidayCalendarDetailDA.InsertUpdate(tc, oHolidayCalendarDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = HolidayCalendarDetailDA.InsertUpdate(tc, oHolidayCalendarDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHolidayCalendarDetail = new HolidayCalendarDetail();
                    oHolidayCalendarDetail = CreateObject(oReader);
                }
                reader.Close();

                #region HolidayCalendarDRP
                foreach (HolidayCalendarDRP oItem in oHolidayCalendarDRPs)
                {
                    IDataReader readerdetail;
                    if (oItem.HolidayCalendarDRPID != 0 && oItem.HolidayCalendarDetailID != oHolidayCalendarDetail.HolidayCalendarDetailID)
                    {
                        oItem.HolidayCalendarDetailID = oHolidayCalendarDetail.HolidayCalendarDetailID;
                        readerdetail = HolidayCalendarDRPDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        oItem.HolidayCalendarDetailID = oHolidayCalendarDetail.HolidayCalendarDetailID;

                        if (oItem.HolidayCalendarDRPID <= 0)
                        {
                            readerdetail = HolidayCalendarDRPDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = HolidayCalendarDRPDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                    }
                    
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sHolidayCalendarDRPIDs = sHolidayCalendarDRPIDs + oReaderDetail.GetString("HolidayCalendarDRPID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sHolidayCalendarDRPIDs.Length > 0)
                {
                    sHolidayCalendarDRPIDs = sHolidayCalendarDRPIDs.Remove(sHolidayCalendarDRPIDs.Length - 1, 1);
                }
                oHolidayCalendarDRP = new HolidayCalendarDRP();
                oHolidayCalendarDRP.HolidayCalendarDetailID = oHolidayCalendarDetail.HolidayCalendarDetailID;
                HolidayCalendarDRPDA.Delete(tc, oHolidayCalendarDRP, EnumDBOperation.Delete, nUserID, sHolidayCalendarDRPIDs);
                #endregion

                #region Get Holidat Calendar Detail
                reader = HolidayCalendarDetailDA.Get(tc, oHolidayCalendarDetail.HolidayCalendarDetailID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHolidayCalendarDetail = new HolidayCalendarDetail();
                    oHolidayCalendarDetail = CreateObject(oReader);
                }
                reader.Close();
                
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oHolidayCalendarDetail = new HolidayCalendarDetail();
                    oHolidayCalendarDetail.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oHolidayCalendarDetail;
        }

        public HolidayCalendarDetail Get(int id, Int64 nUserId)
        {
            HolidayCalendarDetail oHolidayCalendarDetail = new HolidayCalendarDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = HolidayCalendarDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHolidayCalendarDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get HolidayCalendar Detail", e);
                #endregion
            }

            return oHolidayCalendarDetail;
        }
        public List<HolidayCalendarDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<HolidayCalendarDetail> oHolidayCalendarDetail = new List<HolidayCalendarDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = HolidayCalendarDetailDA.Gets(tc, sSQL);
                oHolidayCalendarDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get HolidayCalendar Detail", e);
                #endregion
            }
            return oHolidayCalendarDetail;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                HolidayCalendarDetail oHolidayCalendarDetail = new HolidayCalendarDetail();
                oHolidayCalendarDetail.HolidayCalendarDetailID = id;
                HolidayCalendarDetailDA.Delete(tc, oHolidayCalendarDetail, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
    }
}
