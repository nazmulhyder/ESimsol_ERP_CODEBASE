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

    public class PurchaseQuotationDetailService : MarshalByRefObject, IPurchaseQuotationDetailService
    {
        #region Private functions and declaration
        private PurchaseQuotationDetail MapObject(NullHandler oReader)
        {
            PurchaseQuotationDetail oPurchaseQuotationDetail = new PurchaseQuotationDetail();
            oPurchaseQuotationDetail.PurchaseQuotationDetailLogID = oReader.GetInt32("PurchaseQuotationDetailLogID");
            oPurchaseQuotationDetail.PurchaseQuotationDetailID = oReader.GetInt32("PurchaseQuotationDetailID");
            oPurchaseQuotationDetail.PurchaseQuotationID = oReader.GetInt32("PurchaseQuotationID");
            oPurchaseQuotationDetail.ProductID = oReader.GetInt32("ProductID");
            oPurchaseQuotationDetail.ProductCode = oReader.GetString("ProductCode");
            oPurchaseQuotationDetail.ProductName = oReader.GetString("ProductName");
            oPurchaseQuotationDetail.ProductSpec = oReader.GetString("ProductSpec");
            oPurchaseQuotationDetail.ItemDescription = oReader.GetString("ItemDescription");
            oPurchaseQuotationDetail.Quantity = oReader.GetDouble("Quantity");
            oPurchaseQuotationDetail.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oPurchaseQuotationDetail.MUnitID = oReader.GetInt32("MUnitID");
            oPurchaseQuotationDetail.UnitName = oReader.GetString("UnitName");
            oPurchaseQuotationDetail.CollectByName = oReader.GetString("CollectByName");
            oPurchaseQuotationDetail.PurchaseQuotationNo = oReader.GetString("PurchaseQuotationNo");
            oPurchaseQuotationDetail.SupplierName = oReader.GetString("SupplierName");
            oPurchaseQuotationDetail.UnitSymbol = oReader.GetString("UnitSymbol");
            oPurchaseQuotationDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oPurchaseQuotationDetail.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oPurchaseQuotationDetail.ApprovedByName = oReader.GetString("ApprovedByName");
            oPurchaseQuotationDetail.ApprovedDate = oReader.GetDateTime("ApprovedDate");
            oPurchaseQuotationDetail.SupplierID = oReader.GetInt32("SupplierID");
            oPurchaseQuotationDetail.ActualPrice = oReader.GetDouble("ActualPrice");
            oPurchaseQuotationDetail.Discount = oReader.GetDouble("Discount");
            oPurchaseQuotationDetail.PaymentTerm = oReader.GetString("PaymentTerm");
            oPurchaseQuotationDetail.Vat = oReader.GetDouble("Vat");
            oPurchaseQuotationDetail.TransportCost = oReader.GetDouble("TransportCost");
            oPurchaseQuotationDetail.PRDetailID = oReader.GetInt32("PRDetailID");

            return oPurchaseQuotationDetail;
        }

        private PurchaseQuotationDetail CreateObject(NullHandler oReader)
        {
            PurchaseQuotationDetail oPurchaseQuotationDetail = new PurchaseQuotationDetail();
            oPurchaseQuotationDetail = MapObject(oReader);
            return oPurchaseQuotationDetail;
        }

        public List<PurchaseQuotationDetail> CreateObjects(IDataReader oReader)
        {
            List<PurchaseQuotationDetail> oPurchaseQuotationDetail = new List<PurchaseQuotationDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PurchaseQuotationDetail oItem = CreateObject(oHandler);
                oPurchaseQuotationDetail.Add(oItem);
            }
            return oPurchaseQuotationDetail;
        }

        #endregion

        #region Interface implementation
        public List<PurchaseQuotationDetail> Approve(PurchaseQuotation oPurchaseQuotation , Int64 nUserId)
        {

            TransactionContext tc = null;
            List<PurchaseQuotationDetail> oTempPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();
            List<PurchaseQuotationDetail> oPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();
            oPurchaseQuotationDetails = oPurchaseQuotation.PurchaseQuotationDetails;
            PurchaseQuotationDetail oPurchaseQuotationDetail = new PurchaseQuotationDetail();
            string sPurchaseQuotationDetailIDs = "";
            try
            {
                tc = TransactionContext.Begin();
                foreach (PurchaseQuotationDetail oItem in oPurchaseQuotationDetails)
                {
                    IDataReader readerdetail = PurchaseQuotationDetailDA.Approve(tc, oItem,nUserId);
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        oPurchaseQuotationDetail = new PurchaseQuotationDetail();
                        oPurchaseQuotationDetail = CreateObject(oReaderDetail);
                        sPurchaseQuotationDetailIDs = sPurchaseQuotationDetailIDs + oPurchaseQuotationDetail.PurchaseQuotationDetailID.ToString() + ",";
                    }
                    readerdetail.Close();
                }
                if (sPurchaseQuotationDetailIDs.Length > 0)
                {
                    sPurchaseQuotationDetailIDs = sPurchaseQuotationDetailIDs.Remove(sPurchaseQuotationDetailIDs.Length - 1, 1);
                }
                string sSQL = "SELECT * FROM View_PurchaseQuotationDetail WHERE PurchaseQuotationDetailID IN (" + sPurchaseQuotationDetailIDs + ") Order By SupplierID, ProductID";
                IDataReader reader = null;
                reader = PurchaseQuotationDetailDA.Gets(tc, sSQL);
                oTempPurchaseQuotationDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseQuotation", e);
                #endregion
            }

            return oTempPurchaseQuotationDetails;
        }
       
        public PurchaseQuotationDetail Get(int PurchaseQuotationDetailID, Int64 nUserId)
        {
            PurchaseQuotationDetail oAccountHead = new PurchaseQuotationDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader readerDetail = PurchaseQuotationDetailDA.Get(tc, PurchaseQuotationDetailID);
                NullHandler oReaderDetail = new NullHandler(readerDetail);
                if (readerDetail.Read())
                {
                    oAccountHead = CreateObject(oReaderDetail);
                }
                readerDetail.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseQuotationDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<PurchaseQuotationDetail> Gets(int PQID, Int64 nUserID)
        {
            List<PurchaseQuotationDetail> oPurchaseQuotationDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseQuotationDetailDA.Gets(PQID, tc);
                oPurchaseQuotationDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseQuotationDetail", e);
                #endregion
            }

            return oPurchaseQuotationDetail;
        }
        public List<PurchaseQuotationDetail> GetsByLog(int PQID, Int64 nUserID)
        {
            List<PurchaseQuotationDetail> oPurchaseQuotationDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseQuotationDetailDA.GetsByLog(PQID, tc);
                oPurchaseQuotationDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseQuotationDetail", e);
                #endregion
            }

            return oPurchaseQuotationDetail;
        }

        public List<PurchaseQuotationDetail> GetsForNOA(int nProductID, int nMunitID, Int64 nUserID)
        {
            List<PurchaseQuotationDetail> oPurchaseQuotationDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseQuotationDetailDA.GetsForNOA(tc, nProductID, nMunitID);
                oPurchaseQuotationDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseQuotationDetail", e);
                #endregion
            }

            return oPurchaseQuotationDetail;
        }
        public List<PurchaseQuotationDetail> GetsBy(int nProductID, int nMunitID, Int64 nUserID)
        {
            List<PurchaseQuotationDetail> oPurchaseQuotationDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseQuotationDetailDA.GetsBy(tc, nProductID, nMunitID);
                oPurchaseQuotationDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseQuotationDetail", e);
                #endregion
            }

            return oPurchaseQuotationDetail;
        }
        public List<PurchaseQuotationDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<PurchaseQuotationDetail> oPurchaseQuotationDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseQuotationDetailDA.Gets(tc, sSQL);
                oPurchaseQuotationDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseQuotationDetail", e);
                #endregion
            }

            return oPurchaseQuotationDetail;
        }
        #endregion
    }
    
 
}
