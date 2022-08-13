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
    public class RosterPlanService : MarshalByRefObject, IRosterPlanService
    {
        #region Private functions and declaration
        private RosterPlan MapObject(NullHandler oReader)
        {
            RosterPlan oRosterPlan = new RosterPlan();
            oRosterPlan.RosterPlanID = oReader.GetInt32("RosterPlanID");
            oRosterPlan.CompanyID = oReader.GetInt32("CompanyID");
            oRosterPlan.Description = oReader.GetString("Description");
            oRosterPlan.RosterCycle = oReader.GetInt16("RosterCycle");
            oRosterPlan.IsActive = oReader.GetBoolean("IsActive");
            oRosterPlan.EncryptRPID = Global.Encrypt(oRosterPlan.RosterPlanID.ToString());
            return oRosterPlan;
        }

        private RosterPlan CreateObject(NullHandler oReader)
        {
            RosterPlan oRosterPlan = MapObject(oReader);
            return oRosterPlan;
        }

        private List<RosterPlan> CreateObjects(IDataReader oReader)
        {
            List<RosterPlan> oRosterPlan = new List<RosterPlan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RosterPlan oItem = CreateObject(oHandler);
                oRosterPlan.Add(oItem);
            }
            return oRosterPlan;
        }

        #endregion

        #region Interface implementation
        public RosterPlanService() { }

        public RosterPlan IUD(RosterPlan oRosterPlan, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            RosterPlan oNewRosterPlan = new RosterPlan();
            try
            {
                tc = TransactionContext.Begin(true);
                List<RosterPlanDetail> oRosterPlanDetails = new List<RosterPlanDetail>();
                RosterPlanDetail oRosterPlanDetail = new RosterPlanDetail();
                oRosterPlanDetails = oRosterPlan.RosterPlanDetails;
                IDataReader reader;
                reader = RosterPlanDA.IUD(tc, oRosterPlan, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oNewRosterPlan = CreateObject(oReader);
                }
                reader.Close();

                if (nDBOperation != 3)
                {
                    #region RosterPlanDetail Part
                    foreach (RosterPlanDetail oItem in oRosterPlanDetails)
                    {
                        IDataReader readerDetail  ;
                        oItem.RosterPlanID = oNewRosterPlan.RosterPlanID;
                        if (oItem.RosterPlanDetailID <= 0)
                        {
                            readerDetail = RosterPlanDetailDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Insert);
                            NullHandler oReaderDetail = new NullHandler(readerDetail);
                            readerDetail.Close();
                        }  
                    }
                    #endregion    
                }
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oNewRosterPlan.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oNewRosterPlan.RosterPlanID = 0;
                #endregion
            }
            return oNewRosterPlan;
        }

        public RosterPlan Get(int id, Int64 nUserId)
        {
            RosterPlan aRosterPlan = new RosterPlan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RosterPlanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aRosterPlan = CreateObject(oReader);
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

            return aRosterPlan;
        }

        public List<RosterPlan> Gets(Int64 nUserID)
        {
            List<RosterPlan> oRosterPlan = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RosterPlanDA.Gets(tc);
                oRosterPlan = CreateObjects(reader);
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
            return oRosterPlan;
        }
        public List<RosterPlan> Gets(string sSQL, Int64 nUserID)
        {
            List<RosterPlan> oRosterPlan = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RosterPlanDA.Gets(tc, sSQL);
                oRosterPlan = CreateObjects(reader);
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
            return oRosterPlan;
        }

        public string ChangeActiveStatus(RosterPlan oRosterPlan, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                RosterPlanDA.ChangeActiveStatus(tc, oRosterPlan);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Product. Because of " + e.Message, e);
                #endregion
            }
            return "Update sucessfully";
        }
        #endregion
    }
}
