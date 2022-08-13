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
    [Serializable]
    public class PurchaseInvoicePackingListDetailService : MarshalByRefObject, IPurchaseInvoicePackingListDetailService
    {
        #region Private functions and declaration
        private PurchaseInvoicePackingListDetail MapObject(NullHandler oReader)
        {
            PurchaseInvoicePackingListDetail oPurchaseInvoicePackingListDetail = new PurchaseInvoicePackingListDetail();
            oPurchaseInvoicePackingListDetail.PurchaseInvoicePackingListID = oReader.GetInt32("PurchaseInvoicePackingListID");
            oPurchaseInvoicePackingListDetail.PurchaseInvoicePackingListDetailID = oReader.GetInt32("PurchaseInvoicePackingListDetailID");
            oPurchaseInvoicePackingListDetail.NoOfBag = oReader.GetInt32("NoOfBag");
            oPurchaseInvoicePackingListDetail.WeightPerBag = oReader.GetInt32("WeightPerBag");
            oPurchaseInvoicePackingListDetail.BagDes = oReader.GetString("BagDes");
         
            return oPurchaseInvoicePackingListDetail;
        }

        private PurchaseInvoicePackingListDetail CreateObject(NullHandler oReader)
        {
            PurchaseInvoicePackingListDetail oPurchaseInvoicePackingListDetail = new PurchaseInvoicePackingListDetail();
            oPurchaseInvoicePackingListDetail = MapObject(oReader);
            return oPurchaseInvoicePackingListDetail;
        }

        private List<PurchaseInvoicePackingListDetail> CreateObjects(IDataReader oReader)
        {
            List<PurchaseInvoicePackingListDetail> lstPurchaseInvoicePackingListDetails = new List<PurchaseInvoicePackingListDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PurchaseInvoicePackingListDetail oItem = CreateObject(oHandler);
                lstPurchaseInvoicePackingListDetails.Add(oItem);
            }
            return lstPurchaseInvoicePackingListDetails;
        }
        #endregion

        #region Interface implementation
        public PurchaseInvoicePackingListDetailService() { }

        public PurchaseInvoicePackingListDetail Save(PurchaseInvoicePackingListDetail oPurchaseInvoicePackingListDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (oPurchaseInvoicePackingListDetail.PurchaseInvoicePackingListDetailID <= 0)
                {
                  reader=  PurchaseInvoicePackingListDetailDA.InsertUpdate(tc, oPurchaseInvoicePackingListDetail,EnumDBOperation.Insert , nUserID,"");
                }
                else
                {
                    reader = PurchaseInvoicePackingListDetailDA.InsertUpdate(tc, oPurchaseInvoicePackingListDetail, EnumDBOperation.Update, nUserID, "");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseInvoicePackingListDetail = new PurchaseInvoicePackingListDetail();
                    oPurchaseInvoicePackingListDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPurchaseInvoicePackingListDetail = new PurchaseInvoicePackingListDetail();
                oPurchaseInvoicePackingListDetail.ErrorMessage = e.Message;
                #endregion
            }
            
            return oPurchaseInvoicePackingListDetail;
        }
        public string Delete(PurchaseInvoicePackingListDetail oPurchaseInvoicePackingListDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                 PurchaseInvoicePackingListDetailDA.InsertUpdate(tc, oPurchaseInvoicePackingListDetail, EnumDBOperation.Delete, nUserID, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                #endregion
                return e.Message;
            }
            return Global.DeleteMessage;
        }
        public PurchaseInvoicePackingListDetail Get(int id, Int64 nUserID)
        {
            PurchaseInvoicePackingListDetail oPurchaseInvoicePackingListDetail = new PurchaseInvoicePackingListDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PurchaseInvoicePackingListDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseInvoicePackingListDetail = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oPurchaseInvoicePackingListDetail;
        }
        public List<PurchaseInvoicePackingListDetail> Gets(Int64 nUserID)
        {
            List<PurchaseInvoicePackingListDetail> lstPurchaseInvoicePackingListDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseInvoicePackingListDetailDA.Gets(tc);
                lstPurchaseInvoicePackingListDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return lstPurchaseInvoicePackingListDetail;
        }


        public List<PurchaseInvoicePackingListDetail> Gets(int nPPListID, Int64 nUserID)
        {
            List<PurchaseInvoicePackingListDetail> oPurchaseInvoicePackingListDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseInvoicePackingListDetailDA.Gets(tc, nPPListID);
                oPurchaseInvoicePackingListDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oPurchaseInvoicePackingListDetails;
        }

        #endregion
    }


}
