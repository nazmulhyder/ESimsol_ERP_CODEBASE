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
    public class RosterPlanDetailService : MarshalByRefObject, IRosterPlanDetailService
    {
        #region Private functions and declaration
        private RosterPlanDetail MapObject(NullHandler oReader)
        {
            RosterPlanDetail oRosterPlanDetail = new RosterPlanDetail();
            oRosterPlanDetail.RosterPlanDetailID = oReader.GetInt32("RosterPlanDetailID");
            oRosterPlanDetail.RosterPlanID = oReader.GetInt32("RosterPlanID");
            oRosterPlanDetail.ShiftID = oReader.GetInt32("ShiftID");
            oRosterPlanDetail.Shift = oReader.GetString("Shift");
            oRosterPlanDetail.NextShiftID = oReader.GetInt16("NextShiftID");
            oRosterPlanDetail.NextShift = oReader.GetString("NextShift");
            oRosterPlanDetail.ToleranceTime = oReader.GetDateTime("ToleranceTime");

            return oRosterPlanDetail;
        }

        private RosterPlanDetail CreateObject(NullHandler oReader)
        {
            RosterPlanDetail oRosterPlanDetail = MapObject(oReader);
            return oRosterPlanDetail;
        }

        private List<RosterPlanDetail> CreateObjects(IDataReader oReader)
        {
            List<RosterPlanDetail> oRosterPlanDetail = new List<RosterPlanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RosterPlanDetail oItem = CreateObject(oHandler);
                oRosterPlanDetail.Add(oItem);
            }
            return oRosterPlanDetail;
        }

        #endregion

        #region Interface implementation
        public RosterPlanDetailService() { }

        public RosterPlanDetail Get(int id, Int64 nUserId)
        {
            RosterPlanDetail aRosterPlanDetail = new RosterPlanDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RosterPlanDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aRosterPlanDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get RosterPlan", e);
                #endregion
            }

            return aRosterPlanDetail;
        }

        public List<RosterPlanDetail> Gets(int id,Int64 nUserID)
        {
            List<RosterPlanDetail> oRosterPlanDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RosterPlanDetailDA.Gets(tc);
                oRosterPlanDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RosterPlanDetail", e);
                #endregion
            }

            return oRosterPlanDetail;
        }
        public List<RosterPlanDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<RosterPlanDetail> oRosterPlanDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RosterPlanDetailDA.Gets(tc, sSQL);
                oRosterPlanDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RosterPlanDetail", e);
                #endregion
            }

            return oRosterPlanDetail;
        }
        public RosterPlanDetail IUD(RosterPlanDetail oRosterPlanDetail, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            RosterPlanDetail oNewRosterPlanDetail = new RosterPlanDetail();
            try
            {
                tc = TransactionContext.Begin(true);
                
                //RosterPlanDetail oRosterPlanDetail = new RosterPlanDetail();
                //oRosterPlanDetails = oRosterPlan.RosterPlanDetails;
                IDataReader reader;
                reader = RosterPlanDetailDA.IUD(tc, oRosterPlanDetail, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oNewRosterPlanDetail = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oNewRosterPlanDetail.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oNewRosterPlanDetail;
        }
        //public RosterPlanDetail IUD(RosterPlanDetail oRosterPlanDetail, Int64 nUserID, int nDBOperation)
        //{
        //    TransactionContext tc = null;
        //    RosterPlanDetail oNewCCCD = new RosterPlanDetail();
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        IDataReader reader;
        //        reader = RosterPlanDetailDA.IUD(tc, oRosterPlanDetail, nUserID, nDBOperation);

        //        NullHandler oReader = new NullHandler(reader);
        //        if (reader.Read())
        //        {
        //            oNewCCCD = CreateObject(oReader);
        //        }
        //        reader.Close();
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();
        //        oNewCCCD.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
        //        #endregion
        //    }
        //    return oNewCCCD;
        //}

        #endregion
    }
}
