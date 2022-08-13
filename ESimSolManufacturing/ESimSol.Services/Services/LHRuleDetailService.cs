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
    public class LHRuleDetailService : MarshalByRefObject, ILHRuleDetailService
    {
        #region Private functions and declaration
        public static LHRuleDetail MapObject(NullHandler oReader)
        {
            LHRuleDetail oLHRuleDetail = new LHRuleDetail();
            oLHRuleDetail.LHRuleDetailID = oReader.GetInt32("LHRuleDetailID");
            oLHRuleDetail.LHRuleID = oReader.GetInt32("LHRuleID");
            oLHRuleDetail.LHRuleValueType = (EnumLHRuleValueType)oReader.GetInt16("LHRuleValueType");
            oLHRuleDetail.LHRuleValueTypeInt = oReader.GetInt16("LHRuleValueTypeInt");
            oLHRuleDetail.LHRuleValue = oReader.GetString("LHRuleValue");
            oLHRuleDetail.Sequence = oReader.GetInt32("Sequence");
            return oLHRuleDetail;
        }

        public static LHRuleDetail CreateObject(NullHandler oReader)
        {
            LHRuleDetail oLHRuleDetail = new LHRuleDetail();
            oLHRuleDetail = MapObject(oReader);
            return oLHRuleDetail;
        }

        private List<LHRuleDetail> CreateObjects(IDataReader oReader)
        {
            List<LHRuleDetail> oLHRuleDetail = new List<LHRuleDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LHRuleDetail oItem = CreateObject(oHandler);
                oLHRuleDetail.Add(oItem);
            }
            return oLHRuleDetail;
        }

        #endregion

        #region Interface implementation
        public LHRuleDetailService() { }

        public LHRuleDetail Save(LHRuleDetail oLHRuleDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLHRuleDetail.LHRuleDetailID <= 0)
                {
                    reader = LHRuleDetailDA.InsertUpdate(tc, oLHRuleDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = LHRuleDetailDA.InsertUpdate(tc, oLHRuleDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLHRuleDetail = new LHRuleDetail();
                    oLHRuleDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save LHRuleDetail. Because of " + e.Message, e);
                #endregion
            }
            return oLHRuleDetail;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                LHRuleDetail oLHRuleDetail = new LHRuleDetail();
                oLHRuleDetail.LHRuleDetailID = id;
                LHRuleDetailDA.Delete(tc, oLHRuleDetail, EnumDBOperation.Delete, nUserId, "");
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
        public LHRuleDetail Get(int id, Int64 nUserId)
        {
            LHRuleDetail oLHRuleDetail = new LHRuleDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = LHRuleDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLHRuleDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LHRuleDetail", e);
                #endregion
            }
            return oLHRuleDetail;
        }
        public List<LHRuleDetail> Gets(Int64 nUserID)
        {
            List<LHRuleDetail> oLHRuleDetails = new List<LHRuleDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LHRuleDetailDA.Gets(tc);
                oLHRuleDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LHRuleDetail", e);
                #endregion
            }
            return oLHRuleDetails;
        }
        public List<LHRuleDetail> Gets(string sSQL,Int64 nUserID)
        {
            List<LHRuleDetail> oLHRuleDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LHRuleDetailDA.Gets(tc,sSQL);
                oLHRuleDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LHRuleDetail", e);
                #endregion
            }
            return oLHRuleDetails;
        }
        #endregion
    }   
}

