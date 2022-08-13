using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 
namespace ESimSol.Services.Services
{


    public class PQTermsAndConditionService : MarshalByRefObject, IPQTermsAndConditionService
    {
        #region Private functions and declaration
        private PQTermsAndCondition MapObject(NullHandler oReader)
        {
            PQTermsAndCondition oPQTermsAndCondition = new PQTermsAndCondition();
            oPQTermsAndCondition.PQTermsAndConditionLogID = oReader.GetInt32("PQTermsAndConditionLogID");
            oPQTermsAndCondition.PQTermsAndConditionID = oReader.GetInt32("PQTermsAndConditionID");
            oPQTermsAndCondition.PurchaseQuotationID = oReader.GetInt32("PurchaseQuotationID");
            oPQTermsAndCondition.TermsAndCondition = oReader.GetString("TermsAndCondition");
            return oPQTermsAndCondition;
        }

        private PQTermsAndCondition CreateObject(NullHandler oReader)
        {
            PQTermsAndCondition oPQTermsAndCondition = new PQTermsAndCondition();
            oPQTermsAndCondition = MapObject(oReader);
            return oPQTermsAndCondition;
        }

        private List<PQTermsAndCondition> CreateObjects(IDataReader oReader)
        {
            List<PQTermsAndCondition> oPQTermsAndCondition = new List<PQTermsAndCondition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PQTermsAndCondition oItem = CreateObject(oHandler);
                oPQTermsAndCondition.Add(oItem);
            }
            return oPQTermsAndCondition;
        }

        #endregion

        #region Interface implementation
        public PQTermsAndConditionService() { }

        public PQTermsAndCondition Save(PQTermsAndCondition oPQTermsAndCondition, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPQTermsAndCondition.PQTermsAndConditionID <= 0)
                {
                    reader = PQTermsAndConditionDA.InsertUpdate(tc, oPQTermsAndCondition, EnumDBOperation.Insert, nUserId, "");
                }
                else
                {
                    reader = PQTermsAndConditionDA.InsertUpdate(tc, oPQTermsAndCondition, EnumDBOperation.Update, nUserId, "");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPQTermsAndCondition = new PQTermsAndCondition();
                    oPQTermsAndCondition = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPQTermsAndCondition.ErrorMessage = e.Message;
                #endregion
            }
            return oPQTermsAndCondition;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PQTermsAndCondition oPQTermsAndCondition = new PQTermsAndCondition();
                oPQTermsAndCondition.PQTermsAndConditionID = id;
                PQTermsAndConditionDA.Delete(tc, oPQTermsAndCondition, EnumDBOperation.Delete, nUserId, "");
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
            return "Delete sucessfully";
        }

        public string PQTermsAndConditionSave(List<PQTermsAndCondition> oPQTermsAndConditions, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<PQTermsAndCondition> _oPQTermsAndConditions = new List<PQTermsAndCondition>();
            PQTermsAndCondition oPQTermsAndCondition = new PQTermsAndCondition();
            oPQTermsAndCondition.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (PQTermsAndCondition oItem in oPQTermsAndConditions)
                {
                    IDataReader reader;
                    reader = PQTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oPQTermsAndCondition = new PQTermsAndCondition();
                        oPQTermsAndCondition = CreateObject(oReader);
                    }
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPQTermsAndCondition.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save PQTermsAndCondition. Because of " + e.Message, e);
                #endregion
            }
            return "Save Successfully !";
        }

        public PQTermsAndCondition Get(int id, Int64 nUserId)
        {
            PQTermsAndCondition oAccountHead = new PQTermsAndCondition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PQTermsAndConditionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PQTermsAndCondition", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<PQTermsAndCondition> Gets(int id, Int64 nUserID)
        {
            List<PQTermsAndCondition> oPQTermsAndCondition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PQTermsAndConditionDA.Gets(tc, id);
                oPQTermsAndCondition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PQTermsAndCondition", e);
                #endregion
            }

            return oPQTermsAndCondition;
        }

        public List<PQTermsAndCondition> GetsByLog(int id, Int64 nUserID)
        {
            List<PQTermsAndCondition> oPQTermsAndCondition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PQTermsAndConditionDA.GetsByLog(tc, id);
                oPQTermsAndCondition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PQTermsAndCondition", e);
                #endregion
            }

            return oPQTermsAndCondition;
        }


        public List<PQTermsAndCondition> Gets(string sSQL, Int64 nUserID)
        {
            List<PQTermsAndCondition> oPQTermsAndCondition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PQTermsAndConditionDA.Gets(tc, sSQL);
                oPQTermsAndCondition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PQTermsAndCondition", e);
                #endregion
            }

            return oPQTermsAndCondition;
        }
        #endregion
    }
    
}

