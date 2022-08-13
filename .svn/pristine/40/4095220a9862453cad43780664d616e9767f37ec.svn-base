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
    public class PurchaseOrderDetailService : MarshalByRefObject, IPurchaseOrderDetailService
    {
        #region Private functions and declaration
        private PurchaseOrderDetail MapObject(NullHandler oReader)
        {
            PurchaseOrderDetail oPurchaseOrderDetail = new PurchaseOrderDetail();            
            oPurchaseOrderDetail.PODetailID = oReader.GetInt32("PODetailID");
            oPurchaseOrderDetail.POID = oReader.GetInt32("POID");
            oPurchaseOrderDetail.ProductID = oReader.GetInt32("ProductID");
            oPurchaseOrderDetail.Qty = oReader.GetDouble("Qty");
            oPurchaseOrderDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oPurchaseOrderDetail.MUnitID = oReader.GetInt32("MUnitID");
            oPurchaseOrderDetail.Note = oReader.GetString("Note");
            oPurchaseOrderDetail.ProductCode = oReader.GetString("ProductCode");
            oPurchaseOrderDetail.ProductName = oReader.GetString("ProductName");
            oPurchaseOrderDetail.ProductSpec = oReader.GetString("ProductSpec");
            oPurchaseOrderDetail.UnitSymbol = oReader.GetString("UnitSymbol");
            oPurchaseOrderDetail.UnitName = oReader.GetString("UnitName");
            oPurchaseOrderDetail.Qty_Invoice = oReader.GetDouble("Qty_Invoice");
            oPurchaseOrderDetail.YetToGRNQty = oReader.GetDouble("YetToGRNQty");
            oPurchaseOrderDetail.GRNValue = oReader.GetDouble("GRNValue");
            oPurchaseOrderDetail.YetToInvoiceQty = oReader.GetDouble("YetToInvoiceQty");
            oPurchaseOrderDetail.AdvInvoice = oReader.GetDouble("AdvInvoice");
            oPurchaseOrderDetail.AdvanceSettle = oReader.GetDouble("AdvanceSettle");            
            oPurchaseOrderDetail.InvoiceValue = oReader.GetDouble("InvoiceValue");
            oPurchaseOrderDetail.BuyerName = oReader.GetString("BuyerName");
            oPurchaseOrderDetail.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oPurchaseOrderDetail.StyleNo = oReader.GetString("StyleNo");
            oPurchaseOrderDetail.RefNo = oReader.GetString("RefNo");
            oPurchaseOrderDetail.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oPurchaseOrderDetail.LotBalance = oReader.GetDouble("LotBalance");
            oPurchaseOrderDetail.YetToPurchaseReturnQty = oReader.GetDouble("YetToPurchaseReturnQty");
            oPurchaseOrderDetail.LotNo = oReader.GetString("LotNo");
            oPurchaseOrderDetail.LotID = oReader.GetInt32("LotID");
            oPurchaseOrderDetail.VehicleModelID = oReader.GetInt32("VehicleModelID");
            oPurchaseOrderDetail.RefDetailID = oReader.GetInt32("RefDetailID");
            oPurchaseOrderDetail.ModelShortName = oReader.GetString("ModelShortName");
            
            return oPurchaseOrderDetail;
        }

        private PurchaseOrderDetail CreateObject(NullHandler oReader)
        {
            PurchaseOrderDetail oPurchaseOrderDetail = new PurchaseOrderDetail();
            oPurchaseOrderDetail = MapObject(oReader);
            return oPurchaseOrderDetail;
        }

        public List<PurchaseOrderDetail> CreateObjects(IDataReader oReader)
        {
            List<PurchaseOrderDetail> oPurchaseOrderDetail = new List<PurchaseOrderDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PurchaseOrderDetail oItem = CreateObject(oHandler);
                oPurchaseOrderDetail.Add(oItem);
            }
            return oPurchaseOrderDetail;
        }

        #endregion

        #region Interface implementation
        public string Delete(PurchaseOrderDetail oPurchaseOrderDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                PurchaseOrderDetailDA.Delete(tc, oPurchaseOrderDetail, EnumDBOperation.Delete, nUserId,"");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public PurchaseOrderDetail Save(PurchaseOrderDetail oPurchaseOrderDetail, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPurchaseOrderDetail.PODetailID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseRequisition, EnumRoleOperationType.Add);
                    reader = PurchaseOrderDetailDA.InsertUpdate(tc, oPurchaseOrderDetail, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseRequisition, EnumRoleOperationType.Edit);
                    reader = PurchaseOrderDetailDA.InsertUpdate(tc, oPurchaseOrderDetail, EnumDBOperation.Update, nUserID,"");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseOrderDetail = new PurchaseOrderDetail();
                    oPurchaseOrderDetail = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPurchaseOrderDetail = new PurchaseOrderDetail();
                oPurchaseOrderDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oPurchaseOrderDetail;
        }
        public PurchaseOrderDetail Get(int PurchaseOrderDetailID, Int64 nUserId)
        {
            PurchaseOrderDetail oAccountHead = new PurchaseOrderDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader readerDetail = PurchaseOrderDetailDA.Get(tc, PurchaseOrderDetailID);
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
                throw new ServiceException("Failed to Get PurchaseOrderDetail", e);
                #endregion
            }

            return oAccountHead;
        }


        public List<PurchaseOrderDetail> Gets(int nPRID, Int64 nUserID)
        {
            List<PurchaseOrderDetail> oPurchaseOrderDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseOrderDetailDA.Gets(nPRID, tc);
                oPurchaseOrderDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseOrderDetail", e);
                #endregion
            }

            return oPurchaseOrderDetail;
        }
        public List<PurchaseOrderDetail> GetsForInvoice(PurchaseInvoice oPurchaseInvoice, Int64 nUserID)
        {
            List<PurchaseOrderDetail> oPurchaseOrderDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseOrderDetailDA.GetsForInvoice(oPurchaseInvoice, tc);
                oPurchaseOrderDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseOrderDetail", e);
                #endregion
            }

            return oPurchaseOrderDetail;
        }

        public List<PurchaseOrderDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<PurchaseOrderDetail> oPurchaseOrderDetail = new List<PurchaseOrderDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseOrderDetailDA.Gets(tc, sSQL);
                oPurchaseOrderDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseOrderDetail", e);
                #endregion
            }

            return oPurchaseOrderDetail;
        }
        #endregion
    }
    
 
}
