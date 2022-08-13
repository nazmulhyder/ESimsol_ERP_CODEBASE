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
    [Serializable]
    public class PurchaseInvoiceDetailService : MarshalByRefObject, IPurchaseInvoiceDetailService
    {
        #region Private functions and declaration
        private PurchaseInvoiceDetail MapObject(NullHandler oReader)
        {
            PurchaseInvoiceDetail oPurchaseInvoiceDetail = new PurchaseInvoiceDetail();
            oPurchaseInvoiceDetail.PurchaseInvoiceDetailID = oReader.GetInt32("PurchaseInvoiceDetailID");
            oPurchaseInvoiceDetail.VehicleModelID = oReader.GetInt32("VehicleModelID");
            oPurchaseInvoiceDetail.PurchaseInvoiceID = oReader.GetInt32("PurchaseInvoiceID");
            oPurchaseInvoiceDetail.ProductID = oReader.GetInt32("ProductID");
            oPurchaseInvoiceDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oPurchaseInvoiceDetail.Qty = oReader.GetDouble("Qty");
            oPurchaseInvoiceDetail.ReceiveQty = oReader.GetDouble("ReceiveQty");
            oPurchaseInvoiceDetail.MUnitID = oReader.GetInt32("MUnitID");
            oPurchaseInvoiceDetail.RefDetailID = oReader.GetInt32("RefDetailID");
            oPurchaseInvoiceDetail.GRNID = oReader.GetInt32("GRNID");
            oPurchaseInvoiceDetail.Amount = oReader.GetDouble("Amount");
            oPurchaseInvoiceDetail.AdvanceSettle = oReader.GetDouble("AdvanceSettle");
            oPurchaseInvoiceDetail.LCID = oReader.GetInt32("LCID");
            oPurchaseInvoiceDetail.InvoiceID = oReader.GetInt32("InvoiceID");
            oPurchaseInvoiceDetail.CostHeadID = oReader.GetInt32("CostHeadID");
            oPurchaseInvoiceDetail.ModelNo = oReader.GetString("ModelNo");
            oPurchaseInvoiceDetail.ModelShortName = oReader.GetString("ModelShortName");
            oPurchaseInvoiceDetail.Remarks = oReader.GetString("Remarks");
            oPurchaseInvoiceDetail.ProductCode = oReader.GetString("ProductCode");
            oPurchaseInvoiceDetail.ProductName = oReader.GetString("ProductName");
            oPurchaseInvoiceDetail.ProductSpec = oReader.GetString("ProductSpec");
            oPurchaseInvoiceDetail.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oPurchaseInvoiceDetail.MUName = oReader.GetString("MUName");
            oPurchaseInvoiceDetail.MUSymbol = oReader.GetString("MUSymbol");
            oPurchaseInvoiceDetail.PODQty = oReader.GetDouble("PODQty");
            oPurchaseInvoiceDetail.PODRate = oReader.GetDouble("PODRate");
            oPurchaseInvoiceDetail.PODAmount = oReader.GetDouble("PODAmount");
            oPurchaseInvoiceDetail.LCNo = oReader.GetString("LCNo");
            oPurchaseInvoiceDetail.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oPurchaseInvoiceDetail.InvoiceNo = oReader.GetString("InvoiceNo");
            oPurchaseInvoiceDetail.InvoiceDate = oReader.GetDateTime("InvoiceDate");
            oPurchaseInvoiceDetail.CostHeadCode = oReader.GetString("CostHeadCode");
            oPurchaseInvoiceDetail.CostHeadName = oReader.GetString("CostHeadName");
            oPurchaseInvoiceDetail.ConvertionRate = oReader.GetDouble("ConvertionRate");
            oPurchaseInvoiceDetail.CurrencyID = oReader.GetInt32("CurrencyID");
            oPurchaseInvoiceDetail.CurrencyName = oReader.GetString("CurrencyName");
            oPurchaseInvoiceDetail.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oPurchaseInvoiceDetail.YetToInvoiceQty = oReader.GetDouble("YetToInvoiceQty");
            oPurchaseInvoiceDetail.YetToGRNQty = oReader.GetDouble("YetToGRNQty");
            oPurchaseInvoiceDetail.PrevoiusInvoice = oReader.GetDouble("PrevoiusInvoice");
            oPurchaseInvoiceDetail.AdvInvoice = oReader.GetDouble("AdvInvoice");
            oPurchaseInvoiceDetail.BuyerName = oReader.GetString("BuyerName");
            oPurchaseInvoiceDetail.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oPurchaseInvoiceDetail.StyleNo = oReader.GetString("StyleNo");
            oPurchaseInvoiceDetail.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oPurchaseInvoiceDetail.ColorName = oReader.GetString("ColorName");
            oPurchaseInvoiceDetail.SizeName = oReader.GetString("SizeName");
            oPurchaseInvoiceDetail.WorkOrderNo = oReader.GetString("WorkOrderNo");
            oPurchaseInvoiceDetail.WOGRNQty = oReader.GetDouble("WOGRNQty");
            oPurchaseInvoiceDetail.LotBalance = oReader.GetDouble("LotBalance");
            oPurchaseInvoiceDetail.YetToPurchaseReturnQty = oReader.GetDouble("YetToPurchaseReturnQty");
            oPurchaseInvoiceDetail.LotNo = oReader.GetString("LotNo");
            oPurchaseInvoiceDetail.LotID = oReader.GetInt32("LotID");
            oPurchaseInvoiceDetail.InvoiceDetailID = oReader.GetInt32("InvoiceDetailID");
            oPurchaseInvoiceDetail.LandingCostType = (EnumLandingCostType) oReader.GetInt32("LandingCostType");
            oPurchaseInvoiceDetail.LandingCostTypeInt = oReader.GetInt32("LandingCostType");
            oPurchaseInvoiceDetail.ProductWithGroupName = oReader.GetString("ProductWithGroupName");
            oPurchaseInvoiceDetail.RefID = oReader.GetInt32("RefID");
            oPurchaseInvoiceDetail.RefNo = oReader.GetString("RefNo");
            oPurchaseInvoiceDetail.RefDate = oReader.GetDateTime("RefDate");
            oPurchaseInvoiceDetail.RefAmount = oReader.GetDouble("RefAmount");
            
            return oPurchaseInvoiceDetail;
        }

        private PurchaseInvoiceDetail CreateObject(NullHandler oReader)
        {
            PurchaseInvoiceDetail oPurchaseInvoiceDetail = new PurchaseInvoiceDetail();
            oPurchaseInvoiceDetail = MapObject(oReader);
            return oPurchaseInvoiceDetail;
        }

        public List<PurchaseInvoiceDetail> CreateObjects(IDataReader oReader)
        {
            List<PurchaseInvoiceDetail> oPurchaseInvoiceDetails = new List<PurchaseInvoiceDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PurchaseInvoiceDetail oItem = CreateObject(oHandler);
                oPurchaseInvoiceDetails.Add(oItem);
            }
            return oPurchaseInvoiceDetails;
        }
        #endregion

        #region Interface implementation
        public PurchaseInvoiceDetailService() { }

        public string Delete(PurchaseInvoiceDetail oPurchaseInvoiceDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PurchaseInvoiceDetailDA.Delete(tc, oPurchaseInvoiceDetail, EnumDBOperation.Delete, nUserID, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;                
                #endregion
            }
            return Global.DeleteMessage;
        }
        public PurchaseInvoiceDetail Save(PurchaseInvoiceDetail oPurchaseInvoiceDetail, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPurchaseInvoiceDetail.PurchaseInvoiceDetailID <= 0)
                {
                    reader = PurchaseInvoiceDetailDA.InsertUpdate(tc, oPurchaseInvoiceDetail, EnumDBOperation.Insert, nUserID, "");
                }
                else
                {
                    reader = PurchaseInvoiceDetailDA.InsertUpdate(tc, oPurchaseInvoiceDetail, EnumDBOperation.Update, nUserID, "");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseInvoiceDetail = new PurchaseInvoiceDetail();
                    oPurchaseInvoiceDetail = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPurchaseInvoiceDetail = new PurchaseInvoiceDetail();
                oPurchaseInvoiceDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oPurchaseInvoiceDetail;
        }

        public PurchaseInvoiceDetail Get(int nPurchaseInvoiceDetailID, Int64 nUserId)
        {
            PurchaseInvoiceDetail oPurchaseInvoiceDetail = new PurchaseInvoiceDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PurchaseInvoiceDetailDA.Get(tc, nPurchaseInvoiceDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseInvoiceDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Purchase Invoice Product", e);
                #endregion
            }

            return oPurchaseInvoiceDetail;
        }

        public List<PurchaseInvoiceDetail> Gets(int nPurchaseInvoiceID, Int64 nUserId)
        {
            List<PurchaseInvoiceDetail> oPurchaseInvoiceDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseInvoiceDetailDA.Gets(nPurchaseInvoiceID, tc);
                oPurchaseInvoiceDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Purchase Invoice Products", e);
                #endregion
            }
            return oPurchaseInvoiceDetails;
        }

        public List<PurchaseInvoiceDetail> Gets(string sSQL, Int64 nUserId)
        {
            List<PurchaseInvoiceDetail> oPurchaseInvoiceDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseInvoiceDetailDA.Gets(tc, sSQL);
                oPurchaseInvoiceDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseInvoiceDetail", e);
                #endregion
            }

            return oPurchaseInvoiceDetail;
        }

        public List<PurchaseInvoiceDetail> GetsByPurchaseInvoiceID(int nPurchaseInvoiceId, Int64 nUserId)
        {
            List<PurchaseInvoiceDetail> oPurchaseInvoiceDetails = new List<PurchaseInvoiceDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseInvoiceDetailDA.GetsByPurchaseInvoiceID(tc, nPurchaseInvoiceId);
                oPurchaseInvoiceDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseContractDetail", e);
                #endregion
            }

            return oPurchaseInvoiceDetails;
        }

        #endregion
    }
}
