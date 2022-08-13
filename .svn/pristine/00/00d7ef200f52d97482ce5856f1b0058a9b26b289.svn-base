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
    public class ChangesEquitySetupDetailService : MarshalByRefObject, IChangesEquitySetupDetailService
    {
        #region Private functions and declaration
        private ChangesEquitySetupDetail MapObject(NullHandler oReader)
        {
            ChangesEquitySetupDetail oChangesEquitySetupDetail = new ChangesEquitySetupDetail();
            oChangesEquitySetupDetail.ChangesEquitySetupDetailID = oReader.GetInt32("ChangesEquitySetupDetailID");
            oChangesEquitySetupDetail.ChangesEquitySetupID = oReader.GetInt32("ChangesEquitySetupID");
            oChangesEquitySetupDetail.EffectedAccountID = oReader.GetInt32("EffectedAccountID");
            oChangesEquitySetupDetail.AccountCode = oReader.GetString("AccountCode");
            oChangesEquitySetupDetail.AccountHeadName = oReader.GetString("AccountHeadName");
            return oChangesEquitySetupDetail;
        }

        private ChangesEquitySetupDetail CreateObject(NullHandler oReader)
        {
            ChangesEquitySetupDetail oChangesEquitySetupDetail = new ChangesEquitySetupDetail();
            oChangesEquitySetupDetail = MapObject(oReader);
            return oChangesEquitySetupDetail;
        }

        private List<ChangesEquitySetupDetail> CreateObjects(IDataReader oReader)
        {
            List<ChangesEquitySetupDetail> oChangesEquitySetupDetail = new List<ChangesEquitySetupDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ChangesEquitySetupDetail oItem = CreateObject(oHandler);
                oChangesEquitySetupDetail.Add(oItem);
            }
            return oChangesEquitySetupDetail;
        }

        #endregion

        #region Interface implementation
        public ChangesEquitySetupDetailService() { }

        public ChangesEquitySetupDetail Save(ChangesEquitySetupDetail oChangesEquitySetupDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                if (oChangesEquitySetupDetail.ChangesEquitySetupDetailID <= 0)
                {
                    reader = ChangesEquitySetupDetailDA.InsertUpdate(tc, oChangesEquitySetupDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ChangesEquitySetupDetailDA.InsertUpdate(tc, oChangesEquitySetupDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChangesEquitySetupDetail = new ChangesEquitySetupDetail();
                    oChangesEquitySetupDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ChangesEquitySetupDetail. Because of " + e.Message, e);
                #endregion
            }
            return oChangesEquitySetupDetail;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ChangesEquitySetupDetail oChangesEquitySetupDetail = new ChangesEquitySetupDetail();
                oChangesEquitySetupDetail.ChangesEquitySetupDetailID = id;
                ChangesEquitySetupDetailDA.Delete(tc, oChangesEquitySetupDetail, EnumDBOperation.Delete, nUserId,"");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ChangesEquitySetupDetail. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public ChangesEquitySetupDetail Get(int id, int nUserId)
        {
            ChangesEquitySetupDetail oAccountHead = new ChangesEquitySetupDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ChangesEquitySetupDetailDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ChangesEquitySetupDetail", e);
                #endregion
            }

            return oAccountHead;
        }

       

        public List<ChangesEquitySetupDetail> Gets(int nUserID)
        {
            List<ChangesEquitySetupDetail> oChangesEquitySetupDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChangesEquitySetupDetailDA.Gets(tc);
                oChangesEquitySetupDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChangesEquitySetupDetail", e);
                #endregion
            }

            return oChangesEquitySetupDetail;
        }
        public List<ChangesEquitySetupDetail> Gets(int nChangesEquitySetupID, int nUserID)
        {
            List<ChangesEquitySetupDetail> oChangesEquitySetupDetail = new List<ChangesEquitySetupDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChangesEquitySetupDetailDA.Gets(tc, nChangesEquitySetupID);
                oChangesEquitySetupDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChangesEquitySetupDetail", e);
                #endregion
            }

            return oChangesEquitySetupDetail;
        }
       
        public List<ChangesEquitySetupDetail> Gets(string sSQL,int nUserID)
        {
            List<ChangesEquitySetupDetail> oChangesEquitySetupDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                
                reader = ChangesEquitySetupDetailDA.Gets(tc, sSQL);
                oChangesEquitySetupDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChangesEquitySetupDetail", e);
                #endregion
            }

            return oChangesEquitySetupDetail;
        }

       
        #endregion
    }   
}