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

    public class PurchaseRequisitionDetailService : MarshalByRefObject, IPurchaseRequisitionDetailService
    {
        #region Private functions and declaration
        private PurchaseRequisitionDetail MapObject(NullHandler oReader)
        {
            PurchaseRequisitionDetail oPurchaseRequisitionDetail = new PurchaseRequisitionDetail();
            oPurchaseRequisitionDetail.PRDetailID = oReader.GetInt32("PRDetailID");
            oPurchaseRequisitionDetail.PRID = oReader.GetInt32("PRID");
            oPurchaseRequisitionDetail.VehicleModelID = oReader.GetInt32("VehicleModelID");

            oPurchaseRequisitionDetail.PresentStock = oReader.GetDouble("PresentStock");
            oPurchaseRequisitionDetail.PresentStockUnitName = oReader.GetString("PresentStockUnitName");
            oPurchaseRequisitionDetail.LastSupplyUnitName = oReader.GetString("LastSupplyUnitName");
            oPurchaseRequisitionDetail.StockInQty = oReader.GetDouble("StockInQty");
            oPurchaseRequisitionDetail.LastSupplyQty = oReader.GetDouble("LastSupplyQty");
            oPurchaseRequisitionDetail.LastSupplyDate = oReader.GetDateTime("LastSupplyDate");
            oPurchaseRequisitionDetail.ModelNo = oReader.GetString("ModelNo");
            oPurchaseRequisitionDetail.ModelShortName = oReader.GetString("ModelShortName");


            oPurchaseRequisitionDetail.ProductID = oReader.GetInt32("ProductID");
            oPurchaseRequisitionDetail.ProductCode = oReader.GetString("ProductCode");
            oPurchaseRequisitionDetail.Remarks = oReader.GetString("Remarks");
            oPurchaseRequisitionDetail.ProductName = oReader.GetString("ProductName");
            oPurchaseRequisitionDetail.ProductSpec = oReader.GetString("ProductSpec");
            oPurchaseRequisitionDetail.Qty = oReader.GetDouble("Qty") < 0 ? 0 : oReader.GetDouble("Qty");
            oPurchaseRequisitionDetail.MUnitID = oReader.GetInt32("MUnitID");
            oPurchaseRequisitionDetail.UnitName = oReader.GetString("UnitName");
            oPurchaseRequisitionDetail.UnitSymbol = oReader.GetString("UnitSymbol");
            oPurchaseRequisitionDetail.Note = oReader.GetString("Note");
            oPurchaseRequisitionDetail.PRNo = oReader.GetString("PRNo");
            oPurchaseRequisitionDetail.RequirementDate = oReader.GetDateTime("RequirementDate");
            oPurchaseRequisitionDetail.PrepareByName = oReader.GetString("PrepareByName");
            oPurchaseRequisitionDetail.BuyerName = oReader.GetString("BuyerName");
            oPurchaseRequisitionDetail.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oPurchaseRequisitionDetail.StyleNo = oReader.GetString("StyleNo");
            oPurchaseRequisitionDetail.RequiredFor = oReader.GetString("RequiredFor");
            oPurchaseRequisitionDetail.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oPurchaseRequisitionDetail.IsSpecExist = oReader.GetBoolean("IsSpecExist");
            oPurchaseRequisitionDetail.YetToPOQty = oReader.GetDouble("YetToPOQty");
            return oPurchaseRequisitionDetail;
        }

        private PurchaseRequisitionDetail CreateObject(NullHandler oReader)
        {
            PurchaseRequisitionDetail oPurchaseRequisitionDetail = new PurchaseRequisitionDetail();
            oPurchaseRequisitionDetail = MapObject(oReader);
            return oPurchaseRequisitionDetail;
        }

        public List<PurchaseRequisitionDetail> CreateObjects(IDataReader oReader)
        {
            List<PurchaseRequisitionDetail> oPurchaseRequisitionDetail = new List<PurchaseRequisitionDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PurchaseRequisitionDetail oItem = CreateObject(oHandler);
                oPurchaseRequisitionDetail.Add(oItem);
            }
            return oPurchaseRequisitionDetail;
        }

        #endregion

        #region Interface implementation
        public PurchaseRequisitionDetail Save(PurchaseRequisitionDetail oPurchaseRequisitionDetail, Int64 nUserID)
        {
           
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPurchaseRequisitionDetail.PRDetailID <= 0)
                {

                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseRequisition", EnumRoleOperationType.Add);
                    reader = PurchaseRequisitionDetailDA.InsertUpdate(tc, oPurchaseRequisitionDetail, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    // AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseRequisition", EnumRoleOperationType.Edit);
                    reader = PurchaseRequisitionDetailDA.InsertUpdate(tc, oPurchaseRequisitionDetail, EnumDBOperation.Update, nUserID,"");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseRequisitionDetail = new PurchaseRequisitionDetail();
                    oPurchaseRequisitionDetail = CreateObject(oReader);
                }
                reader.Close();
               

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPurchaseRequisitionDetail = new PurchaseRequisitionDetail();
                oPurchaseRequisitionDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oPurchaseRequisitionDetail;
        }
   
        public string Delete(PurchaseRequisitionDetail oPurchaseRequisitionDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                PurchaseRequisitionDetailDA.Delete(tc, oPurchaseRequisitionDetail, EnumDBOperation.Delete, nUserId,"");
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
        public PurchaseRequisitionDetail Get(int PurchaseRequisitionDetailID, Int64 nUserId)
        {
            PurchaseRequisitionDetail oAccountHead = new PurchaseRequisitionDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader readerDetail = PurchaseRequisitionDetailDA.Get(tc, PurchaseRequisitionDetailID);
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
                throw new ServiceException("Failed to Get PurchaseRequisitionDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<PurchaseRequisitionDetail> Gets(int nPRID, Int64 nUserID)
        {
            List<PurchaseRequisitionDetail> oPurchaseRequisitionDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseRequisitionDetailDA.Gets(nPRID, tc);
                oPurchaseRequisitionDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseRequisitionDetail", e);
                #endregion
            }

            return oPurchaseRequisitionDetail;
        }
        public List<PurchaseRequisitionDetail> GetsBy(int nPRID,int nContractorID, Int64 nUserID)
        {
            List<PurchaseRequisitionDetail> oPurchaseRequisitionDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseRequisitionDetailDA.GetsBy(nPRID,nContractorID, tc);
                oPurchaseRequisitionDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseRequisitionDetail", e);
                #endregion
            }

            return oPurchaseRequisitionDetail;
        }

        public List<PurchaseRequisitionDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<PurchaseRequisitionDetail> oPurchaseRequisitionDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseRequisitionDetailDA.Gets(tc, sSQL);
                oPurchaseRequisitionDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseRequisitionDetail", e);
                #endregion
            }

            return oPurchaseRequisitionDetail;
        }
        #endregion
    }
    
 
}
