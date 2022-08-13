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


    public class MasterLCTermsAndConditionService : MarshalByRefObject, IMasterLCTermsAndConditionService
    {
        #region Private functions and declaration
        private MasterLCTermsAndCondition MapObject(NullHandler oReader)
        {
            MasterLCTermsAndCondition oMasterLCTermsAndCondition = new MasterLCTermsAndCondition();
            oMasterLCTermsAndCondition.MasterLCTermsAndConditionID = oReader.GetInt32("MasterLCTermsAndConditionID");
            oMasterLCTermsAndCondition.MasterLCID = oReader.GetInt32("MasterLCID");
            oMasterLCTermsAndCondition.MasterLCLogID = oReader.GetInt32("MasterLCLogID");
            oMasterLCTermsAndCondition.MasterLCTermsAndConditionLogID = oReader.GetInt32("MasterLCTermsAndConditionLogID");
            oMasterLCTermsAndCondition.TermsAndCondition = oReader.GetString("TermsAndCondition");
            return oMasterLCTermsAndCondition;
        }

        private MasterLCTermsAndCondition CreateObject(NullHandler oReader)
        {
            MasterLCTermsAndCondition oMasterLCTermsAndCondition = new MasterLCTermsAndCondition();
            oMasterLCTermsAndCondition = MapObject(oReader);
            return oMasterLCTermsAndCondition;
        }

        private List<MasterLCTermsAndCondition> CreateObjects(IDataReader oReader)
        {
            List<MasterLCTermsAndCondition> oMasterLCTermsAndCondition = new List<MasterLCTermsAndCondition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MasterLCTermsAndCondition oItem = CreateObject(oHandler);
                oMasterLCTermsAndCondition.Add(oItem);
            }
            return oMasterLCTermsAndCondition;
        }

        #endregion

        #region Interface implementation
        public MasterLCTermsAndConditionService() { }

        public MasterLCTermsAndCondition Save(MasterLCTermsAndCondition oMasterLCTermsAndCondition, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oMasterLCTermsAndCondition.MasterLCTermsAndConditionID <= 0)
                {
                    reader = MasterLCTermsAndConditionDA.InsertUpdate(tc, oMasterLCTermsAndCondition, EnumDBOperation.Insert, "", nUserId);
                }
                else
                {
                    reader = MasterLCTermsAndConditionDA.InsertUpdate(tc, oMasterLCTermsAndCondition, EnumDBOperation.Update, "", nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMasterLCTermsAndCondition = new MasterLCTermsAndCondition();
                    oMasterLCTermsAndCondition = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oMasterLCTermsAndCondition.ErrorMessage = e.Message;
                #endregion
            }
            return oMasterLCTermsAndCondition;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MasterLCTermsAndCondition oMasterLCTermsAndCondition = new MasterLCTermsAndCondition();
                oMasterLCTermsAndCondition.MasterLCTermsAndConditionID = id;
                MasterLCTermsAndConditionDA.Delete(tc, oMasterLCTermsAndCondition, EnumDBOperation.Delete, "", nUserId);
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

        public string MasterLCTermsAndConditionSave(List<MasterLCTermsAndCondition> oMasterLCTermsAndConditions, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<MasterLCTermsAndCondition> _oMasterLCTermsAndConditions = new List<MasterLCTermsAndCondition>();
            MasterLCTermsAndCondition oMasterLCTermsAndCondition = new MasterLCTermsAndCondition();
            oMasterLCTermsAndCondition.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (MasterLCTermsAndCondition oItem in oMasterLCTermsAndConditions)
                {
                    IDataReader reader;
                    reader = MasterLCTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, "", nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oMasterLCTermsAndCondition = new MasterLCTermsAndCondition();
                        oMasterLCTermsAndCondition = CreateObject(oReader);
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

                oMasterLCTermsAndCondition.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save MasterLCTermsAndCondition. Because of " + e.Message, e);
                #endregion
            }
            return "Save Successfully !";
        }

        public MasterLCTermsAndCondition Get(int id, Int64 nUserId)
        {
            MasterLCTermsAndCondition oAccountHead = new MasterLCTermsAndCondition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MasterLCTermsAndConditionDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get MasterLCTermsAndCondition", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<MasterLCTermsAndCondition> Gets(int id, Int64 nUserID)
        {
            List<MasterLCTermsAndCondition> oMasterLCTermsAndCondition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MasterLCTermsAndConditionDA.Gets(tc, id);
                oMasterLCTermsAndCondition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MasterLCTermsAndCondition", e);
                #endregion
            }

            return oMasterLCTermsAndCondition;
        }

        public List<MasterLCTermsAndCondition> GetsMasterLCLog(int id, Int64 nUserID)
        {
            List<MasterLCTermsAndCondition> oMasterLCTermsAndCondition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MasterLCTermsAndConditionDA.GetsMasterLCLog(tc, id);
                oMasterLCTermsAndCondition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MasterLCTermsAndCondition", e);
                #endregion
            }

            return oMasterLCTermsAndCondition;
        }


        public List<MasterLCTermsAndCondition> Gets(string sSQL, Int64 nUserID)
        {
            List<MasterLCTermsAndCondition> oMasterLCTermsAndCondition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MasterLCTermsAndConditionDA.Gets(tc, sSQL);
                oMasterLCTermsAndCondition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MasterLCTermsAndCondition", e);
                #endregion
            }

            return oMasterLCTermsAndCondition;
        }
        #endregion
    }
    
}
