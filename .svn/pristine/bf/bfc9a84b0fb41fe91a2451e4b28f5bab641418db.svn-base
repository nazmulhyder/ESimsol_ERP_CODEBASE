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
    public class LHRuleService : MarshalByRefObject, ILHRuleService
    {
        #region Private functions and declaration
        public static LHRule MapObject(NullHandler oReader)
        {
            LHRule oLHRule = new LHRule();
            oLHRule.LHRuleID = oReader.GetInt32("LHRuleID");
            oLHRule.LeaveHeadID = oReader.GetInt32("LeaveHeadID");
            oLHRule.LHRuleType = (EnumLHRuleType)oReader.GetInt16("LHRuleType");
            oLHRule.LHRuleTypeInt = oReader.GetInt16("LHRuleType");
            oLHRule.Remarks = oReader.GetString("Remarks");
            return oLHRule;
        }

        public static LHRule CreateObject(NullHandler oReader)
        {
            LHRule oLHRule = new LHRule();
            oLHRule = MapObject(oReader);
            return oLHRule;
        }

        private List<LHRule> CreateObjects(IDataReader oReader)
        {
            List<LHRule> oLHRule = new List<LHRule>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LHRule oItem = CreateObject(oHandler);
                oLHRule.Add(oItem);
            }
            return oLHRule;
        }

        #endregion

        #region Interface implementation
        public LHRuleService() { }

        public LHRule Save(LHRule oLHRule, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLHRule.LHRuleID <= 0)
                {
                    reader = LHRuleDA.InsertUpdate(tc, oLHRule, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = LHRuleDA.InsertUpdate(tc, oLHRule, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLHRule = new LHRule();
                    oLHRule = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save LHRule. Because of " + e.Message, e);
                #endregion
            }
            return oLHRule;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                LHRule oLHRule = new LHRule();
                oLHRule.LHRuleID = id;
                LHRuleDA.Delete(tc, oLHRule, EnumDBOperation.Delete, nUserId, "");
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
        public LHRule Get(int id, Int64 nUserId)
        {
            LHRule oLHRule = new LHRule();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = LHRuleDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLHRule = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LHRule", e);
                #endregion
            }
            return oLHRule;
        }
        public List<LHRule> Gets(Int64 nUserID)
        {
            List<LHRule> oLHRules = new List<LHRule>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LHRuleDA.Gets(tc);
                oLHRules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LHRule", e);
                #endregion
            }
            return oLHRules;
        }
        public List<LHRule> Gets(string sSQL,Int64 nUserID)
        {
            List<LHRule> oLHRules = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LHRuleDA.Gets(tc,sSQL);
                oLHRules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LHRule", e);
                #endregion
            }
            return oLHRules;
        }
        #endregion
    }   
}
