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

    public class PurchaseOrderService : MarshalByRefObject, IPurchaseOrderService
    {
        #region Private functions and declaration
        private PurchaseOrder MapObject(NullHandler oReader)
        {
            PurchaseOrder oPurchaseOrder = new PurchaseOrder();
            oPurchaseOrder.POID = oReader.GetInt32("POID");
            oPurchaseOrder.BUID = oReader.GetInt32("BUID");
            oPurchaseOrder.PONo = oReader.GetString("PONO");
            oPurchaseOrder.PODate = oReader.GetDateTime("PODate");            
            oPurchaseOrder.RefType = (EnumPOReferenceType)oReader.GetInt32("RefType");
            oPurchaseOrder.RefTypeInt = oReader.GetInt32("RefType");
            oPurchaseOrder.RefID = oReader.GetInt32("RefID");
            oPurchaseOrder.Status = (EnumPOStatus)oReader.GetInt32("Status");
            oPurchaseOrder.StatusInt = oReader.GetInt32("Status");
            oPurchaseOrder.ContractorID = oReader.GetInt32("ContractorID");
            oPurchaseOrder.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oPurchaseOrder.Note = oReader.GetString("Note");
            oPurchaseOrder.ConcernPersonID = oReader.GetInt32("ConcernPersonID");
            oPurchaseOrder.ApproveBy = oReader.GetInt32("ApproveBy");
            oPurchaseOrder.ApproveDate = oReader.GetDateTime("ApproveDate");
            oPurchaseOrder.CurrencyID = oReader.GetInt32("CurrencyID");
            oPurchaseOrder.ContractorName = oReader.GetString("ContractorName");
            oPurchaseOrder.ContractorShortName = oReader.GetString("ContractorShortName");
            oPurchaseOrder.ApprovedByName = oReader.GetString("ApprovedByName");
            oPurchaseOrder.PrepareByName = oReader.GetString("PrepareByName");
            oPurchaseOrder.PrepareBy = oReader.GetInt32("DBUserID");
            oPurchaseOrder.ConcernPersonName = oReader.GetString("ConcernPersonName");
            oPurchaseOrder.ContactPersonName = oReader.GetString("ContactPersonName");
            oPurchaseOrder.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oPurchaseOrder.CurrencyBFDP = oReader.GetString("CurrencyBFDP");
            oPurchaseOrder.CurrencyBADP = oReader.GetString("CurrencyBADP");
            oPurchaseOrder.BUCode = oReader.GetString("BUCode");
            oPurchaseOrder.BUName = oReader.GetString("BUName");
            oPurchaseOrder.RefNo = oReader.GetString("RefNo");
            oPurchaseOrder.RefDate = oReader.GetDateTime("RefDate");
            oPurchaseOrder.RefBy = oReader.GetString("RefBy");
            oPurchaseOrder.Amount = oReader.GetDouble("Amount");
            oPurchaseOrder.YetToGRNQty = oReader.GetDouble("YetToGRNQty");
            oPurchaseOrder.YetToInvocieQty = oReader.GetDouble("YetToInvocieQty");
            oPurchaseOrder.PaymentTermID = oReader.GetInt32("PaymentTermID");
            oPurchaseOrder.ShipBy = oReader.GetString("ShipBy");
            oPurchaseOrder.TradeTerm = oReader.GetString("TradeTerm");
            oPurchaseOrder.DeliveryTo = oReader.GetInt32("DeliveryTo");
            oPurchaseOrder.DeliveryToName = oReader.GetString("DeliveryToName");
            oPurchaseOrder.DeliveryToContactPerson = oReader.GetInt32("DeliveryToContactPerson");
            oPurchaseOrder.DeliveryToContactPersonName = oReader.GetString("DeliveryToContactPersonName");
            oPurchaseOrder.BillTo = oReader.GetInt32("BillTo");
            oPurchaseOrder.BillToName = oReader.GetString("BillToName");
            oPurchaseOrder.BIllToContactPerson = oReader.GetInt32("BIllToContactPerson");
            oPurchaseOrder.BIllToContactPersonName = oReader.GetString("BIllToContactPersonName");
            oPurchaseOrder.PaymentTermText = oReader.GetString("PaymentTermText");
            oPurchaseOrder.CRate = oReader.GetDouble("CRate");
            oPurchaseOrder.LotBalance = oReader.GetDouble("LotBalance");
            oPurchaseOrder.YetToPurchaseReturnQty = oReader.GetDouble("YetToPurchaseReturnQty");
            oPurchaseOrder.DiscountInAmount = oReader.GetDouble("DiscountInAmount");
            oPurchaseOrder.DiscountInPercent = oReader.GetDouble("DiscountInPercent");
            oPurchaseOrder.ApprovalStatus = oReader.GetString("ApprovalStatus");
            oPurchaseOrder.ApprovalSequence = oReader.GetInt32("ApprovalSequence");
            oPurchaseOrder.LastApprovalSequence = oReader.GetInt32("LastApprovalSequence");
            oPurchaseOrder.PaymentMode = (EnumInvoicePaymentMode)oReader.GetInt32("PaymentMode");
            oPurchaseOrder.PaymentModeInt = oReader.GetInt32("PaymentMode");
            oPurchaseOrder.YetToPI_Amount = oReader.GetDouble("YetToPI_Amount");
            oPurchaseOrder.SubjectName = oReader.GetString("SubjectName");
            oPurchaseOrder.DiscountAmountOfPO = oReader.GetDouble("DiscountAmountOfPO");
            return oPurchaseOrder;
        }

        private PurchaseOrder CreateObject(NullHandler oReader)
        {
            PurchaseOrder oPurchaseOrder = new PurchaseOrder();
            oPurchaseOrder = MapObject(oReader);
            return oPurchaseOrder;
        }

        private List<PurchaseOrder> CreateObjects(IDataReader oReader)
        {
            List<PurchaseOrder> oPurchaseOrder = new List<PurchaseOrder>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PurchaseOrder oItem = CreateObject(oHandler);
                oPurchaseOrder.Add(oItem);
            }
            return oPurchaseOrder;
        }

        #endregion

        #region Interface implementation
        public PurchaseOrderService() { }

        public PurchaseOrder Save(PurchaseOrder oPurchaseOrder, Int64 nUserID)
        {
            List<PurchaseOrderDetail> oPurchaseOrderDetails = new List<PurchaseOrderDetail>();
            List<POTandCClause> oPOTandCClauses = new List<POTandCClause>();
            List<PurchaseCost> oPurchaseCosts = new List<PurchaseCost>();
            oPurchaseOrderDetails = oPurchaseOrder.PurchaseOrderDetails;
            oPOTandCClauses = oPurchaseOrder.POTandCClauses;
            oPurchaseCosts = oPurchaseOrder.PurchaseCosts;
            string sPurchaseOrderDetailIDs = "", sPOTandCIDs = "",sPurchaseCostIDs="";

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPurchaseOrder.POID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseOrder, EnumRoleOperationType.Add);
                    reader = PurchaseOrderDA.InsertUpdate(tc, oPurchaseOrder, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseOrder, EnumRoleOperationType.Edit);
                    reader = PurchaseOrderDA.InsertUpdate(tc, oPurchaseOrder, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseOrder = new PurchaseOrder();
                    oPurchaseOrder = CreateObject(oReader);
                }
                reader.Close();
                #region Purchase Order Detail Part
                if (oPurchaseOrderDetails != null)
                {
                    foreach (PurchaseOrderDetail oItem in oPurchaseOrderDetails)
                    {
                        IDataReader readerdetail;
                        oItem.POID = oPurchaseOrder.POID;
                        if (oItem.PODetailID <= 0)
                        {
                            readerdetail = PurchaseOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                        }
                        else
                        {
                            readerdetail = PurchaseOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID,"");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sPurchaseOrderDetailIDs = sPurchaseOrderDetailIDs + oReaderDetail.GetString("PODetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sPurchaseOrderDetailIDs.Length > 0)
                    {
                        sPurchaseOrderDetailIDs = sPurchaseOrderDetailIDs.Remove(sPurchaseOrderDetailIDs.Length - 1, 1);
                    }
                    PurchaseOrderDetail oPurchaseOrderDetail = new PurchaseOrderDetail();
                    oPurchaseOrderDetail.POID = oPurchaseOrder.POID;
                   PurchaseOrderDetailDA.Delete(tc, oPurchaseOrderDetail, EnumDBOperation.Delete, nUserID, sPurchaseOrderDetailIDs);
                }

                #endregion

                #region P T And C
                if (oPOTandCClauses != null)
                {
                    foreach (POTandCClause oItem in oPOTandCClauses)
                    {
                        IDataReader readerdetail;
                        oItem.POID = oPurchaseOrder.POID;
                        if (oItem.POTandCClauseID <= 0)
                        {
                            readerdetail = POTandCClauseDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = POTandCClauseDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sPOTandCIDs = sPOTandCIDs + oReaderDetail.GetString("POTandCClauseID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sPOTandCIDs.Length > 0)
                    {
                        sPOTandCIDs = sPOTandCIDs.Remove(sPOTandCIDs.Length - 1, 1);
                    }
                    POTandCClause oPOTandCClause = new POTandCClause();
                    oPOTandCClause.POID = oPurchaseOrder.POID;
                    POTandCClauseDA.Delete(tc, oPOTandCClause, EnumDBOperation.Delete, nUserID, sPOTandCIDs);
                }

                #endregion

                #region P Costing
                if (oPurchaseCosts != null)
                {
                    foreach (PurchaseCost oItem in oPurchaseCosts)
                    {
                        IDataReader readerdetail;
                        oItem.RefID = oPurchaseOrder.POID;
                        if (oItem.PurchaseCostID <= 0)
                        {
                            readerdetail = PurchaseCostDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = PurchaseCostDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sPurchaseCostIDs = sPurchaseCostIDs + oReaderDetail.GetString("PurchaseCostID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sPurchaseCostIDs.Length > 0)
                    {
                        sPurchaseCostIDs = sPurchaseCostIDs.Remove(sPurchaseCostIDs.Length - 1, 1);
                    }
                    PurchaseCost oPurchaseCost = new PurchaseCost();
                    oPurchaseCost.RefID = oPurchaseOrder.POID;
                    PurchaseCostDA.Delete(tc, oPurchaseCost, EnumDBOperation.Delete, nUserID, sPurchaseCostIDs);
                }

                #endregion

                reader = PurchaseOrderDA.Get(tc, oPurchaseOrder.POID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseOrder = new PurchaseOrder();
                    oPurchaseOrder = CreateObject(oReader);
                }
                reader.Close();
                IDataReader readerdetails = null;
                readerdetails = PurchaseOrderDetailDA.Gets(oPurchaseOrder.POID, tc);
                PurchaseOrderDetailService obj = new PurchaseOrderDetailService();
                oPurchaseOrder.PurchaseOrderDetails = obj.CreateObjects(readerdetails);
                readerdetails.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPurchaseOrder = new PurchaseOrder();
                oPurchaseOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oPurchaseOrder;
        }
        
        public PurchaseOrder Approved(PurchaseOrder oPurchaseOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region Approved
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseOrder, EnumRoleOperationType.Approved);
                //PurchaseOrderDA.Approved(tc, oPurchaseOrder, nUserID);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseOrder, EnumRoleOperationType.Approved);
                oPurchaseOrder.ApproveDate = DateTime.Today;
                oPurchaseOrder.Status = EnumPOStatus.Approved;
                oPurchaseOrder.StatusInt = (int)EnumPOStatus.Approved;
                reader = PurchaseOrderDA.InsertUpdate(tc, oPurchaseOrder, EnumDBOperation.Approval, nUserID);
                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseOrder = new PurchaseOrder();
                    oPurchaseOrder = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPurchaseOrder = new PurchaseOrder();
                oPurchaseOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oPurchaseOrder;
        }

        public PurchaseOrder UndoApproved(PurchaseOrder oPurchaseOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region UndoApproved
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseOrder, EnumRoleOperationType.UnApproved);                
                oPurchaseOrder.Status = EnumPOStatus.Initialize;
                oPurchaseOrder.StatusInt = (int)EnumPOStatus.Initialize;
                reader = PurchaseOrderDA.InsertUpdate(tc, oPurchaseOrder, EnumDBOperation.UnApproval, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseOrder = new PurchaseOrder();
                    oPurchaseOrder = CreateObject(oReader);
                }
                reader.Close();
                #endregion


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPurchaseOrder = new PurchaseOrder();
                oPurchaseOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oPurchaseOrder;
        }

        public PurchaseOrder UpdateReportSubject(PurchaseOrder oPurchaseOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region UpdateReportSubject

                PurchaseOrderDA.UpdateReportSubject(tc, oPurchaseOrder, nUserID);
                #endregion

                #region Get PO
                IDataReader reader;
                reader = PurchaseOrderDA.Get(tc, oPurchaseOrder.POID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseOrder = new PurchaseOrder();
                    oPurchaseOrder = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPurchaseOrder = new PurchaseOrder();
                oPurchaseOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oPurchaseOrder;
        }
  
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PurchaseOrder oPurchaseOrder = new PurchaseOrder();
                oPurchaseOrder.POID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.PurchaseOrder, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "PurchaseOrder", id);
                PurchaseOrderDA.Delete(tc, oPurchaseOrder, EnumDBOperation.Delete, nUserId);
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

        public PurchaseOrder Get(int id, Int64 nUserId)
        {
            PurchaseOrder oPurchaseOrder = new PurchaseOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PurchaseOrderDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseOrder = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchaseOrder", e);
                #endregion
            }

            return oPurchaseOrder;
        }

 


      
        public List<PurchaseOrder> Gets(Int64 nUserID)
        {
            List<PurchaseOrder> oPurchaseOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseOrderDA.Gets(tc);
                oPurchaseOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseOrder", e);
                #endregion
            }

            return oPurchaseOrder;
        }

        public List<PurchaseOrder> Gets(string sSQL, Int64 nUserID)
        {
            List<PurchaseOrder> oPurchaseOrders = new List<PurchaseOrder>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseOrderDA.Gets(tc, sSQL);
                oPurchaseOrders = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseOrder", e);
                #endregion
            }

            return oPurchaseOrders;
        }

        #endregion
    }   
    
    
}
