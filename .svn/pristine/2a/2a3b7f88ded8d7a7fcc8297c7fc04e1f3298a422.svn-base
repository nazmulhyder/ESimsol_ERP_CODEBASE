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
    public class PurchaseInvoicePackingListService : MarshalByRefObject, IPurchaseInvoicePackingListService
    {
        #region Private functions and declaration
        private PurchaseInvoicePackingList MapObject(NullHandler oReader)
        {
            PurchaseInvoicePackingList oPurchaseInvoicePackingList = new PurchaseInvoicePackingList();
            oPurchaseInvoicePackingList.PurchaseInvoicePackingListID = oReader.GetInt32("PurchaseInvoicePackingListID");
            oPurchaseInvoicePackingList.PurchaseInvoiceLCDetailID = oReader.GetInt32("PurchaseInvoiceLCDetailID");
            oPurchaseInvoicePackingList.PurchaseInvoiceDetailID = oReader.GetInt32("PurchaseInvoiceDetailID");
            oPurchaseInvoicePackingList.MUnitID = oReader.GetInt32("MUnitID");
            oPurchaseInvoicePackingList.LotNo = oReader.GetString("LotNo");
            oPurchaseInvoicePackingList.ProductName = oReader.GetString("ProductName");
            oPurchaseInvoicePackingList.Qty = oReader.GetDouble("Qty");
            
            return oPurchaseInvoicePackingList;
        }

        private PurchaseInvoicePackingList CreateObject(NullHandler oReader)
        {
            PurchaseInvoicePackingList oPurchaseInvoicePackingList = new PurchaseInvoicePackingList();
            oPurchaseInvoicePackingList = MapObject(oReader);
            return oPurchaseInvoicePackingList;
        }

        private List<PurchaseInvoicePackingList> CreateObjects(IDataReader oReader)
        {
            List<PurchaseInvoicePackingList> lstPurchaseInvoicePackingLists = new List<PurchaseInvoicePackingList>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PurchaseInvoicePackingList oItem = CreateObject(oHandler);
                lstPurchaseInvoicePackingLists.Add(oItem);
            }
            return lstPurchaseInvoicePackingLists;
        }
        #endregion

        #region Interface implementation
        public PurchaseInvoicePackingListService() { }

   
        public PurchaseInvoicePackingList Save(PurchaseInvoicePackingList oPurchaseInvoicePackingList, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<PurchaseInvoicePackingListDetail> oPurchaseInvoicePackingListDetails = new List<PurchaseInvoicePackingListDetail>();
            String sPurchaseInvoicePackingListDetaillIDs = "";
            try
            {
                oPurchaseInvoicePackingListDetails = oPurchaseInvoicePackingList.PIPackingListDetails;

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPurchaseInvoicePackingList.PurchaseInvoicePackingListID <= 0)
                {
                    reader = PurchaseInvoicePackingListDA.InsertUpdate(tc, oPurchaseInvoicePackingList, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = PurchaseInvoicePackingListDA.InsertUpdate(tc, oPurchaseInvoicePackingList, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseInvoicePackingList = new PurchaseInvoicePackingList();
                    oPurchaseInvoicePackingList = CreateObject(oReader);
                }
                reader.Close();

                #region Terms & Condition Part
                if (oPurchaseInvoicePackingListDetails != null)
                {
                    oPurchaseInvoicePackingList.Qty = 0;
                    foreach (PurchaseInvoicePackingListDetail oItem in oPurchaseInvoicePackingListDetails)
                    {
                       
                            IDataReader readertnc;
                            oItem.PurchaseInvoicePackingListID = oPurchaseInvoicePackingList.PurchaseInvoicePackingListID;
                            oPurchaseInvoicePackingList.Qty = oPurchaseInvoicePackingList.Qty + oItem.Qty;
                             if (oItem.PurchaseInvoicePackingListDetailID <= 0)
                            {
                                readertnc = PurchaseInvoicePackingListDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readertnc = PurchaseInvoicePackingListDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderTNC = new NullHandler(readertnc);

                            if (readertnc.Read())
                            {
                                sPurchaseInvoicePackingListDetaillIDs = sPurchaseInvoicePackingListDetaillIDs + oReaderTNC.GetString("PurchaseInvoicePackingListDetailID") + ",";
                            }
                            readertnc.Close();
                       
                    }

                    if (sPurchaseInvoicePackingListDetaillIDs.Length > 0)
                    {
                        sPurchaseInvoicePackingListDetaillIDs = sPurchaseInvoicePackingListDetaillIDs.Remove(sPurchaseInvoicePackingListDetaillIDs.Length - 1, 1);
                    }
                    PurchaseInvoicePackingListDetail oPurchaseInvoicePackingListDetail = new PurchaseInvoicePackingListDetail();
                    oPurchaseInvoicePackingListDetail.PurchaseInvoicePackingListID = oPurchaseInvoicePackingList.PurchaseInvoicePackingListID;
                    PurchaseInvoicePackingListDetailDA.Delete(tc, oPurchaseInvoicePackingListDetail, EnumDBOperation.Delete, nUserID, sPurchaseInvoicePackingListDetaillIDs);
                    sPurchaseInvoicePackingListDetaillIDs = "";
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oPurchaseInvoicePackingList = new PurchaseInvoicePackingList();
                oPurchaseInvoicePackingList.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oPurchaseInvoicePackingList;
        }               
        public string Delete(PurchaseInvoicePackingList oPurchaseInvoicePackingList, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
              
                PurchaseInvoicePackingListDA.Delete(tc, oPurchaseInvoicePackingList, nUserID);
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
        public PurchaseInvoicePackingList Get(int id, Int64 nUserID)
        {
            PurchaseInvoicePackingList oPurchaseInvoicePackingList = new PurchaseInvoicePackingList();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PurchaseInvoicePackingListDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseInvoicePackingList = CreateObject(oReader);
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

            return oPurchaseInvoicePackingList;
        }


        public List<PurchaseInvoicePackingList> Gets(int nPurchaseInvoiceLCDetailID, Int64 nUserID)
        {
            List<PurchaseInvoicePackingList> PurchaseInvoicePackingLists = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseInvoicePackingListDA.Gets(tc, nPurchaseInvoiceLCDetailID);
                PurchaseInvoicePackingLists = CreateObjects(reader);
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

            return PurchaseInvoicePackingLists;
        }
        public List<PurchaseInvoicePackingList> GetsBy(int nPurchaseInvoiceLCID, Int64 nUserID)
        {
            List<PurchaseInvoicePackingList> PurchaseInvoicePackingLists = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseInvoicePackingListDA.GetsBy(tc, nPurchaseInvoiceLCID);
                PurchaseInvoicePackingLists = CreateObjects(reader);
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

            return PurchaseInvoicePackingLists;
        }
        #endregion
    }


}
