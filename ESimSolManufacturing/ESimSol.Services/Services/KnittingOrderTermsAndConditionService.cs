using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class KnittingOrderTermsAndConditionService : MarshalByRefObject, IKnittingOrderTermsAndConditionService
    {
        #region Private functions and declaration

        private KnittingOrderTermsAndCondition MapObject(NullHandler oReader)
        {
            KnittingOrderTermsAndCondition oKnittingOrderTermsAndCondition = new KnittingOrderTermsAndCondition();
            oKnittingOrderTermsAndCondition.KnittingOrderTermsAndConditionID = oReader.GetInt32("KnittingOrderTermsAndConditionID");
            oKnittingOrderTermsAndCondition.KnittingOrderID = oReader.GetInt32("KnittingOrderID");
            oKnittingOrderTermsAndCondition.ClauseType = oReader.GetInt32("ClauseType");
            oKnittingOrderTermsAndCondition.TermsAndCondition = oReader.GetString("TermsAndCondition");
            return oKnittingOrderTermsAndCondition;
        }

        private KnittingOrderTermsAndCondition CreateObject(NullHandler oReader)
        {
            KnittingOrderTermsAndCondition oKnittingOrderTermsAndCondition = new KnittingOrderTermsAndCondition();
            oKnittingOrderTermsAndCondition = MapObject(oReader);
            return oKnittingOrderTermsAndCondition;
        }

        private List<KnittingOrderTermsAndCondition> CreateObjects(IDataReader oReader)
        {
            List<KnittingOrderTermsAndCondition> oKnittingOrderTermsAndCondition = new List<KnittingOrderTermsAndCondition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                KnittingOrderTermsAndCondition oItem = CreateObject(oHandler);
                oKnittingOrderTermsAndCondition.Add(oItem);
            }
            return oKnittingOrderTermsAndCondition;
        }

        #endregion

        #region Interface implementation
        public KnittingOrderTermsAndCondition Save(KnittingOrderTermsAndCondition oKnittingOrderTermsAndCondition, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oKnittingOrderTermsAndCondition.KnittingOrderTermsAndConditionID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "KnittingOrderTermsAndCondition", EnumRoleOperationType.Add);
                    reader = KnittingOrderTermsAndConditionDA.InsertUpdate(tc, oKnittingOrderTermsAndCondition, EnumDBOperation.Insert, nUserID, "");
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "KnittingOrderTermsAndCondition", EnumRoleOperationType.Edit);
                    reader = KnittingOrderTermsAndConditionDA.InsertUpdate(tc, oKnittingOrderTermsAndCondition, EnumDBOperation.Update, nUserID, "");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingOrderTermsAndCondition = new KnittingOrderTermsAndCondition();
                    oKnittingOrderTermsAndCondition = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oKnittingOrderTermsAndCondition = new KnittingOrderTermsAndCondition();
                    oKnittingOrderTermsAndCondition.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnittingOrderTermsAndCondition;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                KnittingOrderTermsAndCondition oKnittingOrderTermsAndCondition = new KnittingOrderTermsAndCondition();
                oKnittingOrderTermsAndCondition.KnittingOrderTermsAndConditionID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "KnittingOrderTermsAndCondition", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "KnittingOrderTermsAndCondition", id);
                KnittingOrderTermsAndConditionDA.Delete(tc, oKnittingOrderTermsAndCondition, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }

        public KnittingOrderTermsAndCondition Get(int id, Int64 nUserId)
        {
            KnittingOrderTermsAndCondition oKnittingOrderTermsAndCondition = new KnittingOrderTermsAndCondition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = KnittingOrderTermsAndConditionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingOrderTermsAndCondition = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KnittingOrderTermsAndCondition", e);
                #endregion
            }
            return oKnittingOrderTermsAndCondition;
        }

        public List<KnittingOrderTermsAndCondition> Gets(Int64 nUserID)
        {
            List<KnittingOrderTermsAndCondition> oKnittingOrderTermsAndConditions = new List<KnittingOrderTermsAndCondition>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingOrderTermsAndConditionDA.Gets(tc);
                oKnittingOrderTermsAndConditions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnittingOrderTermsAndCondition oKnittingOrderTermsAndCondition = new KnittingOrderTermsAndCondition();
                oKnittingOrderTermsAndCondition.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnittingOrderTermsAndConditions;
        }

        public List<KnittingOrderTermsAndCondition> Gets(int id, Int64 nUserID)
        {
            List<KnittingOrderTermsAndCondition> oKnittingOrderTermsAndConditions = new List<KnittingOrderTermsAndCondition>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingOrderTermsAndConditionDA.Gets(tc, id);
                oKnittingOrderTermsAndConditions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnittingOrderTermsAndCondition oKnittingOrderTermsAndCondition = new KnittingOrderTermsAndCondition();
                oKnittingOrderTermsAndCondition.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnittingOrderTermsAndConditions;
        }

        public List<KnittingOrderTermsAndCondition> Gets(string sSQL, Int64 nUserID)
        {
            List<KnittingOrderTermsAndCondition> oKnittingOrderTermsAndConditions = new List<KnittingOrderTermsAndCondition>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingOrderTermsAndConditionDA.Gets(tc, sSQL);
                oKnittingOrderTermsAndConditions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KnittingOrderTermsAndCondition", e);
                #endregion
            }
            return oKnittingOrderTermsAndConditions;
        }

        #endregion
    }

}
