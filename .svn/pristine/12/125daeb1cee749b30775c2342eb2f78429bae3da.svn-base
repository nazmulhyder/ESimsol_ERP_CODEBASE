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
    public class GeneralWorkingDayShiftService : MarshalByRefObject, IGeneralWorkingDayShiftService
    {
        #region Private functions and declaration
        private GeneralWorkingDayShift MapObject(NullHandler oReader)
        {
            GeneralWorkingDayShift oGeneralWorkingDayShifts = new GeneralWorkingDayShift();
            oGeneralWorkingDayShifts.GWDSID = oReader.GetInt32("GWDSID");
            oGeneralWorkingDayShifts.GWDID = oReader.GetInt32("GWDID");
            oGeneralWorkingDayShifts.ShiftID = oReader.GetInt32("ShiftID");
            oGeneralWorkingDayShifts.Name = oReader.GetString("Name");
            oGeneralWorkingDayShifts.StartTime = oReader.GetDateTime("StartTime");
            oGeneralWorkingDayShifts.EndTime = oReader.GetDateTime("EndTime");
            oGeneralWorkingDayShifts.DayStartTime = oReader.GetDateTime("DayStartTime");
            oGeneralWorkingDayShifts.DayEndTime = oReader.GetDateTime("DayEndTime");
            return oGeneralWorkingDayShifts;
        }

        private GeneralWorkingDayShift CreateObject(NullHandler oReader)
        {
            GeneralWorkingDayShift oGeneralWorkingDayShifts = MapObject(oReader);
            return oGeneralWorkingDayShifts;
        }

        private List<GeneralWorkingDayShift> CreateObjects(IDataReader oReader)
        {
            List<GeneralWorkingDayShift> oGeneralWorkingDayShifts = new List<GeneralWorkingDayShift>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GeneralWorkingDayShift oItem = CreateObject(oHandler);
                oGeneralWorkingDayShifts.Add(oItem);
            }
            return oGeneralWorkingDayShifts;
        }

        #endregion

        #region Interface implementation
        public GeneralWorkingDayShiftService() { }

        //public List<GeneralWorkingDayShift> Save(GeneralWorkingDayShift oGeneralWorkingDayShift, int nDBOperation, Int64 nUserID)
        //{
        //    List<GeneralWorkingDayShift> oGeneralWorkingDayShifts = new List<GeneralWorkingDayShift>();
        //    TransactionContext tc = null;
        //    string[] ShiftIDs = oGeneralWorkingDayShift.ShiftIDs.Split(',');
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        IDataReader reader;
        //        reader = GeneralWorkingDayShiftDA.Gets("DELETE FROM GeneralWorkingDayShift WHERE GWDID=" + oGeneralWorkingDayShift.GWDID, tc);
        //        reader.Close();
        //        tc.End();
        //        tc = TransactionContext.Begin(true);
        //        foreach (string sShiftID in ShiftIDs)
        //        {
        //            GeneralWorkingDayShift oGWDS = new GeneralWorkingDayShift();
        //            int nShiftID = Convert.ToInt32(sShiftID);
        //            oGWDS.GWDSID = 0;
        //            oGWDS.GWDID = oGeneralWorkingDayShift.GWDID;
        //            oGWDS.ShiftID = nShiftID;


        //            reader = GeneralWorkingDayShiftDA.Save(tc, oGWDS, nDBOperation, nUserID);
        //            NullHandler oReader = new NullHandler(reader);
        //            if (reader.Read())
        //            {
        //                oGWDS = new GeneralWorkingDayShift();
        //                oGWDS = CreateObject(oReader);
        //            }
        //            reader.Close();
        //            oGeneralWorkingDayShifts.Add(oGWDS);
        //        }
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();
        //        oGeneralWorkingDayShift = new GeneralWorkingDayShift();
        //        oGeneralWorkingDayShift.ErrorMessage = e.Message.Split('!')[0];
        //        oGeneralWorkingDayShifts = new List<GeneralWorkingDayShift>();
        //        oGeneralWorkingDayShifts.Add(oGeneralWorkingDayShift);
        //        #endregion
        //    }
        //    return oGeneralWorkingDayShifts;
        //}

        public List<GeneralWorkingDayShift> Gets(int id, Int64 nUserID)
        {
            List<GeneralWorkingDayShift> oGeneralWorkingDayShifts = new List<GeneralWorkingDayShift>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GeneralWorkingDayShiftDA.Gets(tc, id);
                oGeneralWorkingDayShifts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GeneralWorkingDayShifts", e);
                #endregion
            }
            return oGeneralWorkingDayShifts;
        }
        #endregion
    }
}
