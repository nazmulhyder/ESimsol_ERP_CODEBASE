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

    public class HRMShiftService : MarshalByRefObject, IHRMShiftService
    {
        #region Private functions and declaration
        private HRMShift MapObject(NullHandler oReader)
        {
            HRMShift oHRMShift = new HRMShift();
            oHRMShift.ShiftID = oReader.GetInt32("ShiftID");

            oHRMShift.Code = oReader.GetString("Code");
            oHRMShift.Name = oReader.GetString("Name");
            oHRMShift.NameBangla = oReader.GetString("NameBangla");
            oHRMShift.ToleranceForEarlyInMin = oReader.GetInt32("ToleranceForEarlyInMin");
            oHRMShift.ReportTime = oReader.GetDateTime("ReportTime");
            oHRMShift.StartTime = oReader.GetDateTime("StartTime");
            oHRMShift.EndTime = oReader.GetDateTime("EndTime");
            oHRMShift.TotalWorkingTime = oReader.GetInt32("TotalWorkingTime");
            oHRMShift.MaxOTComplianceInMin = oReader.GetInt32("MaxOTComplianceInMin");
            oHRMShift.ToleranceTime = oReader.GetDateTime("ToleranceTime");
            oHRMShift.DayStartTime = oReader.GetDateTime("DayStartTime");
            oHRMShift.CompMaxEndTime = oReader.GetDateTime("CompMaxEndTime");
            oHRMShift.DayEndTime = oReader.GetDateTime("DayEndTime");
            oHRMShift.IsActive = oReader.GetBoolean("IsActive");
            oHRMShift.IsOT = oReader.GetBoolean("IsOT");
            oHRMShift.IsHalfDayOff = oReader.GetBoolean("IsHalfDayOff");
            oHRMShift.PStart = (oReader.GetDateTime("PStart") == DateTime.MinValue ? new DateTime(1950, 01, 01) : oReader.GetDateTime("PStart"));
            oHRMShift.PEnd = (oReader.GetDateTime("PEnd") == DateTime.MinValue ? new DateTime(1950, 01, 01) : oReader.GetDateTime("PEnd"));
            oHRMShift.OTStartTime = oReader.GetDateTime("OTStartTime");
            oHRMShift.OTEndTime = oReader.GetDateTime("OTEndTime");
            oHRMShift.IsOTOnActual = oReader.GetBoolean("IsOTOnActual");
            oHRMShift.OTCalculateAfterInMinute = oReader.GetInt32("OTCalculateAfterInMinute");
            return oHRMShift;
        }

        private HRMShift CreateObject(NullHandler oReader)
        {
            HRMShift oHRMShift = new HRMShift();
            oHRMShift = MapObject(oReader);
            return oHRMShift;
        }

        private List<HRMShift> CreateObjects(IDataReader oReader)
        {
            List<HRMShift> oHRMShifts = new List<HRMShift>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                HRMShift oItem = CreateObject(oHandler);
                oHRMShifts.Add(oItem);
            }
            return oHRMShifts;
        }

        #endregion

        #region Interface implementation
        public HRMShiftService() { }

        public HRMShift Save(HRMShift oHRMShift, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region HRM Shift Part
                IDataReader reader;
                if (oHRMShift.ShiftID <= 0)
                {
                    reader = HRMShiftDA.InsertUpdate(tc, oHRMShift, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = HRMShiftDA.InsertUpdate(tc, oHRMShift, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHRMShift = new HRMShift();
                    oHRMShift = CreateObject(oReader);
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

                oHRMShift = new HRMShift();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oHRMShift.ErrorMessage = Message;
                #endregion
            }
            return oHRMShift;
        }

        public HRMShift Copy(HRMShift oHRMShift, Int64 nUserId)
        {
            List<ShiftBreakSchedule> oShiftBreakSchedules = new List<ShiftBreakSchedule>();
            List<ShiftOTSlab> oShiftOTSlabs = new List<ShiftOTSlab>();
            TransactionContext tc = null;


            oShiftBreakSchedules = oHRMShift.ShiftBreakSchedules;
            oShiftOTSlabs = oHRMShift.ShiftOTSlabs;
            string sShiftBScIDs = "";
            string sShiftOTSlabIDs = "";
            try
            {
                tc = TransactionContext.Begin(true);

                #region HRM Shift Part
                IDataReader reader;
                if (oHRMShift.ShiftID <= 0)
                {
                    reader = HRMShiftDA.InsertUpdate(tc, oHRMShift, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = HRMShiftDA.InsertUpdate(tc, oHRMShift, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHRMShift = new HRMShift();
                    oHRMShift = CreateObject(oReader);
                }
                reader.Close();

                foreach (ShiftBreakSchedule oItem in oShiftBreakSchedules)
                {
                    IDataReader readerdetail;
                    oItem.ShiftID = oHRMShift.ShiftID;
                    if (oItem.ShiftBScID <= 0)
                    {
                        readerdetail = ShiftBreakScheduleDA.IUD(tc, oItem, nUserId, (int)EnumDBOperation.Insert);
                    }
                    else
                    {
                        readerdetail = ShiftBreakScheduleDA.IUD(tc, oItem, nUserId, (int)EnumDBOperation.Update);
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sShiftBScIDs = sShiftBScIDs + oReaderDetail.GetString("ShiftBScID") + ",";
                    }
                    readerdetail.Close();
                }

                foreach (ShiftOTSlab oItem in oShiftOTSlabs)
                {
                    IDataReader readerdetail;
                    oItem.ShiftID = oHRMShift.ShiftID;
                    if (oItem.ShiftOTSlabID <= 0)
                    {
                        readerdetail = ShiftOTSlabDA.IUD(tc, oItem, nUserId, (int)EnumDBOperation.Insert);
                    }
                    else
                    {
                        readerdetail = ShiftOTSlabDA.IUD(tc, oItem, nUserId, (int)EnumDBOperation.Update);
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sShiftOTSlabIDs = sShiftOTSlabIDs + oReaderDetail.GetString("ShiftOTSlabID") + ",";
                    }
                    readerdetail.Close();
                }


                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oHRMShift = new HRMShift();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oHRMShift.ErrorMessage = Message;
                #endregion
            }
            return oHRMShift;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                HRMShift oHRMShift = new HRMShift();
                oHRMShift.ShiftID = id;
                HRMShiftDA.Delete(tc, oHRMShift, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                #endregion
                return e.Message;
            }
            return Global.DeleteMessage;
        }

        public HRMShift Get(int id, Int64 nUserId)
        {
            HRMShift oHRMShift = new HRMShift();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = HRMShiftDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHRMShift = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oHRMShift = new HRMShift();
                oHRMShift.ErrorMessage = e.Message;
                #endregion
            }

            return oHRMShift;
        }

        public List<HRMShift> Gets(Int64 nUserID)
        {
            List<HRMShift> oHRMShifts = new List<HRMShift>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = HRMShiftDA.Gets(tc);
                oHRMShifts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                HRMShift oHRMShift = new HRMShift();
                oHRMShift.ErrorMessage = e.Message;
                oHRMShifts.Add(oHRMShift);
                #endregion
            }

            return oHRMShifts;
        }


        public List<HRMShift> BUWiseGets(int BUID, Int64 nUserID)
        {
            List<HRMShift> oHRMShifts = new List<HRMShift>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = HRMShiftDA.BUWiseGets(BUID, tc);
                oHRMShifts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                HRMShift oHRMShift = new HRMShift();
                oHRMShift.ErrorMessage = e.Message;
                oHRMShifts.Add(oHRMShift);
                #endregion
            }

            return oHRMShifts;
        }

        public List<HRMShift> Gets(string sSQL, Int64 nUserId)
        {
            List<HRMShift> oHRMShift = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = HRMShiftDA.Gets(tc, sSQL);
                oHRMShift = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get HRMShift", e);
                #endregion
            }

            return oHRMShift;
        }

        public HRMShift ShiftInActive(int nShiftID, int ntRShiftID, Int64 nUserId)
        {
            TransactionContext tc = null;
           HRMShift oHRMShift = new HRMShift();
            try
            {
                tc = TransactionContext.Begin(true);

                #region HRM Shift Part
                IDataReader reader;
                reader = HRMShiftDA.ShiftInActive(nShiftID, ntRShiftID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHRMShift = new HRMShift();
                    oHRMShift = CreateObject(oReader);
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

                oHRMShift = new HRMShift();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oHRMShift.ErrorMessage = Message;
                #endregion
            }
            return oHRMShift;
        }
        #endregion
    }


}
