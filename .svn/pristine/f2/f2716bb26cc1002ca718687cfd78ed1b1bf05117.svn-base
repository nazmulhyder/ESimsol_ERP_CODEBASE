using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    
    public class RMClosingStockDetailService : MarshalByRefObject, IRMClosingStockDetailService
    {
        #region Private functions and declaration
        private RMClosingStockDetail MapObject(NullHandler oReader)
        {
            RMClosingStockDetail oRMClosingStockDetail = new RMClosingStockDetail();
            oRMClosingStockDetail.RMClosingStockDetailID = oReader.GetInt32("RMClosingStockDetailID");
            oRMClosingStockDetail.RMClosingStockID = oReader.GetInt32("RMClosingStockID");
            oRMClosingStockDetail.RMAccountHeadID = oReader.GetInt32("RMAccountHeadID");
            oRMClosingStockDetail.Amount = oReader.GetDouble("Amount");
            oRMClosingStockDetail.AccountHeadName = oReader.GetString("AccountHeadName");
            oRMClosingStockDetail.ParentHeadName = oReader.GetString("ParentHeadName");
            oRMClosingStockDetail.AccountCode = oReader.GetString("AccountCode");
            
            return oRMClosingStockDetail;
        }

        private RMClosingStockDetail CreateObject(NullHandler oReader)
        {
            RMClosingStockDetail oRMClosingStockDetail = new RMClosingStockDetail();
            oRMClosingStockDetail = MapObject(oReader);
            return oRMClosingStockDetail;
        }

        private List<RMClosingStockDetail> CreateObjects(IDataReader oReader)
        {
            List<RMClosingStockDetail> oRMClosingStockDetails = new List<RMClosingStockDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RMClosingStockDetail oItem = CreateObject(oHandler);
                oRMClosingStockDetails.Add(oItem);
            }
            return oRMClosingStockDetails;
        }
        #endregion

        #region Interface implementation
        public RMClosingStockDetailService() { }

        public string Delete(RMClosingStockDetail oRMClosingStockDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                RMClosingStockDetailDA.Delete(tc, oRMClosingStockDetail, EnumDBOperation.Delete, nUserID, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Bank. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }
       
        public RMClosingStockDetail Get(int id, Int64 nUserId)
        {
            RMClosingStockDetail oRMClosingStockDetail = new RMClosingStockDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RMClosingStockDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRMClosingStockDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get RMClosingStockDetail", e);
                #endregion
            }

            return oRMClosingStockDetail;
        }

        public List<RMClosingStockDetail> Gets(int nLCBillID, Int64 nUserId)
        {
            List<RMClosingStockDetail> oRMClosingStockDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RMClosingStockDetailDA.Gets(tc, nLCBillID);
                oRMClosingStockDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RMClosingStockDetails", e);
                #endregion
            }

            return oRMClosingStockDetails;
        }
        public List<RMClosingStockDetail> GetsBySQL(string sSQL, Int64 nUserId)
        {
            List<RMClosingStockDetail> oRMClosingStockDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RMClosingStockDetailDA.GetsBySQL(tc, sSQL);
                oRMClosingStockDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RMClosingStockDetails", e);
                #endregion
            }

            return oRMClosingStockDetails;
        }
        #endregion
    }
}