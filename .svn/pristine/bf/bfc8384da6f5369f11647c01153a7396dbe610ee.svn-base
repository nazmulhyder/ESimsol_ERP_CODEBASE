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
    public class BDYEACDetailService : MarshalByRefObject, IBDYEACDetailService
    {
        #region Private functions and declaration
        private BDYEACDetail MapObject(NullHandler oReader)
        {
            BDYEACDetail oBDYEACDetail = new BDYEACDetail();
            oBDYEACDetail.BDYEACDetailID = oReader.GetInt32("BDYEACDetailID");
            oBDYEACDetail.BDYEACID = oReader.GetInt32("BDYEACID");
            oBDYEACDetail.Qty = oReader.GetDouble("Qty");
            oBDYEACDetail.ProductName = oReader.GetString("ProductName");
            return oBDYEACDetail;
        }

        private BDYEACDetail CreateObject(NullHandler oReader)
        {
            BDYEACDetail oBDYEACDetail = new BDYEACDetail();
            oBDYEACDetail = MapObject(oReader);
            return oBDYEACDetail;
        }

        private List<BDYEACDetail> CreateObjects(IDataReader oReader)
        {
            List<BDYEACDetail> oBDYEACDetail = new List<BDYEACDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BDYEACDetail oItem = CreateObject(oHandler);
                oBDYEACDetail.Add(oItem);
            }
            return oBDYEACDetail;
        }

        #endregion

        #region Interface implementation
        public BDYEACDetailService() { }

        public BDYEACDetail Save(BDYEACDetail oBDYEACDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBDYEACDetail.BDYEACDetailID <= 0)
                {
                    reader = BDYEACDetailDA.InsertUpdate(tc, oBDYEACDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = BDYEACDetailDA.InsertUpdate(tc, oBDYEACDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBDYEACDetail = new BDYEACDetail();
                    oBDYEACDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save BDYEACDetail. Because of " + e.Message, e);
                #endregion
            }
            return oBDYEACDetail;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BDYEACDetail oBDYEACDetail = new BDYEACDetail();
                oBDYEACDetail.BDYEACDetailID = id;
                BDYEACDetailDA.Delete(tc, oBDYEACDetail, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete BDYEACDetail. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public BDYEACDetail Get(int id, int nUserId)
        {
            BDYEACDetail oAccountHead = new BDYEACDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BDYEACDetailDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get BDYEACDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<BDYEACDetail> Gets(int nGRNID, int nUserID)
        {
            List<BDYEACDetail> oBDYEACDetail = new List<BDYEACDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BDYEACDetailDA.Gets(tc, nGRNID);
                oBDYEACDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BDYEACDetail", e);
                #endregion
            }

            return oBDYEACDetail;
        }
        public List<BDYEACDetail> Gets(int nUserID)
        {
            List<BDYEACDetail> oBDYEACDetail = new List<BDYEACDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BDYEACDetailDA.Gets(tc);
                oBDYEACDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BDYEACDetail", e);
                #endregion
            }

            return oBDYEACDetail;
        }
        public List<BDYEACDetail> Gets(string sSQL,int nUserID)
        {
            List<BDYEACDetail> oBDYEACDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if(sSQL=="")
                {
                sSQL = "Select * from BDYEACDetail where BDYEACDetailID in (1,2,80,272,347,370,60,45)";
                    }
                reader = BDYEACDetailDA.Gets(tc, sSQL);
                oBDYEACDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BDYEACDetail", e);
                #endregion
            }

            return oBDYEACDetail;
        }

       
        #endregion
    }   
}