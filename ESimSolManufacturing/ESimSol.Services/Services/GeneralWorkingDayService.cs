using System;
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
    public class GeneralWorkingDayService : MarshalByRefObject, IGeneralWorkingDayService
    {
        #region Private functions and declaration
        private GeneralWorkingDay MapObject(NullHandler oReader)
        {
            GeneralWorkingDay oGeneralWorkingDay = new GeneralWorkingDay();
            oGeneralWorkingDay.GWDID = oReader.GetInt32("GWDID");
            oGeneralWorkingDay.GWDTitle = oReader.GetString("GWDTitle");
            oGeneralWorkingDay.AttendanceDate = oReader.GetDateTime("AttendanceDate");
            oGeneralWorkingDay.GWDApplyOn = (EnumGWDApplyOn)oReader.GetInt32("GWDApplyOn");
            oGeneralWorkingDay.GWDApplyOnInt = oReader.GetInt32("GWDApplyOn");
            oGeneralWorkingDay.Remarks = oReader.GetString("Remarks");
            oGeneralWorkingDay.DBUserID = oReader.GetInt32("DBUserID");
            oGeneralWorkingDay.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oGeneralWorkingDay.UserName = oReader.GetString("UserName");
            return oGeneralWorkingDay;

        }


        private GeneralWorkingDay CreateObject(NullHandler oReader)
        {
            GeneralWorkingDay oGeneralWorkingDay = MapObject(oReader);
            return oGeneralWorkingDay;
        }

        private List<GeneralWorkingDay> CreateObjects(IDataReader oReader)
        {
            List<GeneralWorkingDay> oGeneralWorkingDay = new List<GeneralWorkingDay>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GeneralWorkingDay oItem = CreateObject(oHandler);
                oGeneralWorkingDay.Add(oItem);
            }
            return oGeneralWorkingDay;
        }

        #endregion

        #region Interface implementation
        public GeneralWorkingDayService() { }

        public GeneralWorkingDay Save(GeneralWorkingDay oGeneralWorkingDay, long nUserID)
        {
            TransactionContext tc = null;
            oGeneralWorkingDay.ErrorMessage = "";

            string sGeneralWorkingDayDetailIDs = "";            
            List<GeneralWorkingDayDetail> oGeneralWorkingDayDetails = new List<GeneralWorkingDayDetail>();            
            GeneralWorkingDayDetail oGeneralWorkingDayDetail = new GeneralWorkingDayDetail();
            oGeneralWorkingDayDetails = oGeneralWorkingDay.GeneralWorkingDayDetails;           

            string sGeneralWorkingDayShiftIDs = "";
            List<GeneralWorkingDayShift> oGeneralWorkingDayShifts = new List<GeneralWorkingDayShift>();
            GeneralWorkingDayShift oGeneralWorkingDayShift = new GeneralWorkingDayShift();
            oGeneralWorkingDayShifts = oGeneralWorkingDay.GeneralWorkingDayShifts;

            try
            {
                tc = TransactionContext.Begin(true);

                #region GeneralWorkingDay
                IDataReader reader;
                if (oGeneralWorkingDay.GWDID <= 0)
                {
                    reader = GeneralWorkingDayDA.InsertUpdate(tc, oGeneralWorkingDay, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = GeneralWorkingDayDA.InsertUpdate(tc, oGeneralWorkingDay, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGeneralWorkingDay = new GeneralWorkingDay();
                    oGeneralWorkingDay = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region General Working Day Detail

                foreach (GeneralWorkingDayDetail oItem in oGeneralWorkingDayDetails)
                {
                    IDataReader readerdetail;
                    oItem.GWDID = oGeneralWorkingDay.GWDID;
                    if (oItem.GWDDID <= 0)
                    {
                        readerdetail = GeneralWorkingDayDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = GeneralWorkingDayDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sGeneralWorkingDayDetailIDs = sGeneralWorkingDayDetailIDs + oReaderDetail.GetString("GWDDID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sGeneralWorkingDayDetailIDs.Length > 0)
                {
                    sGeneralWorkingDayDetailIDs = sGeneralWorkingDayDetailIDs.Remove(sGeneralWorkingDayDetailIDs.Length - 1, 1);
                }
                oGeneralWorkingDayDetail = new GeneralWorkingDayDetail();
                oGeneralWorkingDayDetail.GWDID = oGeneralWorkingDay.GWDID;
                GeneralWorkingDayDetailDA.Delete(tc, oGeneralWorkingDayDetail, EnumDBOperation.Delete, nUserID, sGeneralWorkingDayDetailIDs);

                #endregion

                #region General Working Day Shift

                foreach (GeneralWorkingDayShift oItem in oGeneralWorkingDayShifts)
                {
                    IDataReader readershift;
                    oItem.GWDID = oGeneralWorkingDay.GWDID;
                    if (oItem.GWDSID <= 0)
                    {
                        readershift = GeneralWorkingDayShiftDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readershift = GeneralWorkingDayShiftDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readershift);
                    if (readershift.Read())
                    {
                        sGeneralWorkingDayShiftIDs = sGeneralWorkingDayShiftIDs + oReaderDetail.GetString("GWDSID") + ",";
                    }
                    readershift.Close();
                }
                if (sGeneralWorkingDayShiftIDs.Length > 0)
                {
                    sGeneralWorkingDayShiftIDs = sGeneralWorkingDayShiftIDs.Remove(sGeneralWorkingDayShiftIDs.Length - 1, 1);
                }
                oGeneralWorkingDayShift = new GeneralWorkingDayShift();
                oGeneralWorkingDayShift.GWDID = oGeneralWorkingDay.GWDID;
                GeneralWorkingDayShiftDA.Delete(tc, oGeneralWorkingDayShift, EnumDBOperation.Delete, nUserID, sGeneralWorkingDayShiftIDs);

                #endregion

                #region Get Salary Field Setup

                reader = GeneralWorkingDayDA.Get(tc, oGeneralWorkingDay.GWDID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGeneralWorkingDay = new GeneralWorkingDay();
                    oGeneralWorkingDay = CreateObject(oReader);
                }
                reader.Close();

                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oGeneralWorkingDay = new GeneralWorkingDay();
                oGeneralWorkingDay.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oGeneralWorkingDay;
        }


        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                GeneralWorkingDay oGeneralWorkingDay = new GeneralWorkingDay();
                oGeneralWorkingDay.GWDID = id;
                GeneralWorkingDayDA.Delete(tc, oGeneralWorkingDay, EnumDBOperation.Delete, nUserId);
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

        public GeneralWorkingDay Get(int id, Int64 nUserId)
        {
            GeneralWorkingDay oGeneralWorkingDay = new GeneralWorkingDay();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GeneralWorkingDayDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGeneralWorkingDay = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get GeneralWorkingDay", e);
                #endregion
            }

            return oGeneralWorkingDay;
        }
        public List<GeneralWorkingDay> Gets(long nUserID)
        {
            List<GeneralWorkingDay> oGeneralWorkingDays = new List<GeneralWorkingDay>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GeneralWorkingDayDA.Gets(tc);
                oGeneralWorkingDays = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GeneralWorkingDay", e);
                #endregion
            }
            return oGeneralWorkingDays;
        }

        public List<GeneralWorkingDay> Gets(string sSQL, Int64 nUserId)
        {
            List<GeneralWorkingDay> oGWD = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GeneralWorkingDayDA.Get(tc, sSQL);
                oGWD = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get General Working Days ", e);
                #endregion
            }

            return oGWD;
        }


        #endregion
    }
}
