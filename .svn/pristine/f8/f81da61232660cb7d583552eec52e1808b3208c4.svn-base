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
    public class LCTransferService : MarshalByRefObject, ILCTransferService
    {
        #region Private functions and declaration
        private LCTransfer MapObject(NullHandler oReader)
        {
            LCTransfer oLCTransfer = new LCTransfer();
            oLCTransfer.LCTransferID = oReader.GetInt32("LCTransferID");
            oLCTransfer.LCTransferLogID = oReader.GetInt32("LCTransferLogID");
            oLCTransfer.MasterLCID = oReader.GetInt32("MasterLCID");
            oLCTransfer.RefNo = oReader.GetString("RefNo");
            oLCTransfer.LCTransferStatus = (EnumLCTransferStatus)oReader.GetInt32("LCTransferStatus");
            oLCTransfer.LCTransferStatusInInt = oReader.GetInt32("LCTransferStatus");
            oLCTransfer.TransferIssueDate = oReader.GetDateTime("TransferIssueDate");
            oLCTransfer.ProductionFactoryID = oReader.GetInt32("ProductionFactoryID");
            oLCTransfer.BuyerID = oReader.GetInt32("BuyerID");
            oLCTransfer.CommissionFavorOf = oReader.GetInt32("CommissionFavorOf");
            oLCTransfer.CommissionAccountID = oReader.GetInt32("CommissionAccountID");
            oLCTransfer.TransferNo = oReader.GetString("TransferNo");
            oLCTransfer.TransferDate = oReader.GetDateTime("TransferDate");
            oLCTransfer.TransferAmount = oReader.GetDouble("TransferAmount");
            oLCTransfer.CommissionAmount = oReader.GetDouble("CommissionAmount");
            oLCTransfer.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oLCTransfer.FactoryBranchID = oReader.GetInt32("FactoryBranchID");
            oLCTransfer.Note = oReader.GetString("Note");
            oLCTransfer.IsCommissionCollect = oReader.GetBoolean("IsCommissionCollect");
            oLCTransfer.ProductionFactoryName = oReader.GetString("ProductionFactoryName");
            oLCTransfer.BuyerName = oReader.GetString("BuyerName");
            oLCTransfer.LCFavorOfName = oReader.GetString("LCFavorOfName");
            oLCTransfer.AccountNo = oReader.GetString("AccountNo");
            oLCTransfer.BranchName = oReader.GetString("BranchName");
            oLCTransfer.BankName = oReader.GetString("BankName");
            oLCTransfer.MasterLCNo = oReader.GetString("MasterLCNo");
            oLCTransfer.ApprovedByName = oReader.GetString("ApprovedByName");
            oLCTransfer.LCStatus = (EnumLCStatus)oReader.GetInt32("LCStatus");
            oLCTransfer.LCType = (EnumLCType)oReader.GetInt32("LCType");
            oLCTransfer.LCValue = oReader.GetDouble("LCValue");
            oLCTransfer.LCTransferQty = oReader.GetDouble("LCTransferQty");
            oLCTransfer.YetToTransferValue = oReader.GetDouble("YetToTransferValue");
            oLCTransfer.YetToInvoiceAmount = oReader.GetDouble("YetToInvoiceAmount");
            oLCTransfer.BankNameAccountNo = oReader.GetString("BankNameAccountNo");
            oLCTransfer.FactoryAddress = oReader.GetString("FactoryAddress");
            oLCTransfer.FactoryOrigin = oReader.GetString("FactoryOrigin");
            oLCTransfer.FactoryPhone = oReader.GetString("FactoryPhone");
            oLCTransfer.FactoryEmail = oReader.GetString("FactoryEmail");
            oLCTransfer.BankAddress = oReader.GetString("BankAddress");
            oLCTransfer.FactoryBankBranchName = oReader.GetString("FactoryBankBranchName");
            oLCTransfer.FactoryBankName = oReader.GetString("FactoryBankName");
            oLCTransfer.FactorBankAddress = oReader.GetString("FactorBankAddress");
            oLCTransfer.LCDate = oReader.GetDateTime("LCDate");
            oLCTransfer.ActualTransferAmount = oReader.GetDouble("ActualTransferAmount");
            oLCTransfer.BUID = oReader.GetInt32("BUID");
            return oLCTransfer;
        }

        private LCTransfer CreateObject(NullHandler oReader)
        {
            LCTransfer oLCTransfer = new LCTransfer();
            oLCTransfer = MapObject(oReader);
            return oLCTransfer;
        }

        private List<LCTransfer> CreateObjects(IDataReader oReader)
        {
            List<LCTransfer> oLCTransfer = new List<LCTransfer>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LCTransfer oItem = CreateObject(oHandler);
                oLCTransfer.Add(oItem);
            }
            return oLCTransfer;
        }

        #endregion

        #region Interface implementation
        public LCTransfer Save(LCTransfer oLCTransfer, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<LCTransferDetail> oLCTransferDetails = new List<LCTransferDetail>();
                LCTransferDetail oLCTransferDetail = new LCTransferDetail();                
                
                oLCTransferDetails = oLCTransfer.LCTransferDetails;
                string sLCTransferDetailIDs = "";

                #region LCTransfer part
                IDataReader reader;
                if (oLCTransfer.LCTransferID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.LCTransfer, EnumRoleOperationType.Add);
                    reader = LCTransferDA.InsertUpdate(tc, oLCTransfer, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.LCTransfer, EnumRoleOperationType.Edit);
                    reader = LCTransferDA.InsertUpdate(tc, oLCTransfer, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLCTransfer = new LCTransfer();
                    oLCTransfer = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region LCTransfer Detail Part
                if (oLCTransferDetails != null)
                {
                    foreach (LCTransferDetail oItem in oLCTransferDetails)
                    {
                        IDataReader readerdetail;
                        oItem.LCTransferID = oLCTransfer.LCTransferID;
                        if (oItem.LCTransferDetailID <= 0)
                        {
                            readerdetail = LCTransferDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = LCTransferDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sLCTransferDetailIDs = sLCTransferDetailIDs + oReaderDetail.GetString("LCTransferDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sLCTransferDetailIDs.Length > 0)
                    {
                        sLCTransferDetailIDs = sLCTransferDetailIDs.Remove(sLCTransferDetailIDs.Length - 1, 1);
                    }
                    oLCTransferDetail = new LCTransferDetail();
                    oLCTransferDetail.LCTransferID = oLCTransfer.LCTransferID;
                    LCTransferDetailDA.Delete(tc, oLCTransferDetail, EnumDBOperation.Delete, nUserID, sLCTransferDetailIDs);

                }

                #endregion

                #region LCTransfer Get

                reader = LCTransferDA.InsertUpdate(tc, oLCTransfer, EnumDBOperation.Update, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLCTransfer = CreateObject(oReader);
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

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oLCTransfer.ErrorMessage = Message;

                #endregion
            }
            return oLCTransfer;
        }

        public LCTransfer UpdateTransferNoDate(LCTransfer oLCTransfer, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region Uppdate part
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "LCTransfer", EnumRoleOperationType.Edit);
                LCTransferDA.UpdateTransferNoDate(tc, oLCTransfer, nUserID);
                #endregion

                #region LCTransfer Get
                IDataReader reader;
                reader = LCTransferDA.Get(tc, oLCTransfer.LCTransferID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLCTransfer = CreateObject(oReader);
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

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oLCTransfer.ErrorMessage = Message;

                #endregion
            }
            return oLCTransfer;
        }

        public string Delete(int nLCTransferID, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                LCTransfer oLCTransfer = new LCTransfer();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.LCTransfer, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "LCTransfer", nLCTransferID);
                oLCTransfer.LCTransferID = nLCTransferID;
                LCTransferDA.Delete(tc, oLCTransfer, EnumDBOperation.Delete, nUserId);
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
            return "Delete sucessfully";
        }

        public LCTransfer AcceptLCTransferRevise(LCTransfer oLCTransfer, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<LCTransferDetail> oLCTransferDetails = new List<LCTransferDetail>();
                LCTransferDetail oLCTransferDetail = new LCTransferDetail();
                oLCTransferDetails = oLCTransfer.LCTransferDetails;
                string sLCTransferDetailIDs = "";
                double nTotalDetailQty = 0;
                #region Calculate Detail qty
                foreach(LCTransferDetail  oItem in oLCTransferDetails)
                {
                    nTotalDetailQty += oItem.TransferQty;
                }
                #endregion

                #region LCTransfer Acept part
                IDataReader reader;
                reader = LCTransferDA.AcceptRevise(tc, oLCTransfer, nTotalDetailQty,  nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLCTransfer = new LCTransfer();
                    oLCTransfer = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region LCTransfer Detail Part
                if (oLCTransferDetails != null)
                {
                    foreach (LCTransferDetail oItem in oLCTransferDetails)
                    {
                        IDataReader readerdetail;
                        oItem.LCTransferID = oLCTransfer.LCTransferID;
                        if (oItem.LCTransferDetailID <= 0)
                        {
                            readerdetail = LCTransferDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = LCTransferDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sLCTransferDetailIDs = sLCTransferDetailIDs + oReaderDetail.GetString("LCTransferDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sLCTransferDetailIDs.Length > 0)
                    {
                        sLCTransferDetailIDs = sLCTransferDetailIDs.Remove(sLCTransferDetailIDs.Length - 1, 1);
                    }
                    oLCTransferDetail = new LCTransferDetail();
                    oLCTransferDetail.LCTransferID = oLCTransfer.LCTransferID;
                    LCTransferDetailDA.Delete(tc, oLCTransferDetail, EnumDBOperation.Delete, nUserID, sLCTransferDetailIDs);

                }

                #endregion

                #region LCTransfer Get

                reader = LCTransferDA.InsertUpdate(tc, oLCTransfer, EnumDBOperation.Update, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLCTransfer = CreateObject(oReader);
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

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oLCTransfer.ErrorMessage = Message;
                #endregion
            }
            return oLCTransfer;
        }
        public LCTransfer ChangeStatus(LCTransfer oLCTransfer, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLCTransfer.LCTranferActionType == EnumLCTranferActionType.Approve)
                {
                   // AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "LCTransfer", EnumRoleOperationType.Approved);
                }
                reader = LCTransferDA.ChangeStatus(tc, oLCTransfer, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLCTransfer = new LCTransfer();
                    oLCTransfer = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oLCTransfer.ErrorMessage = Message;
                #endregion
            }
            return oLCTransfer;
        }

        public LCTransfer Get(int id, Int64 nUserId)
        {
            LCTransfer oAccountHead = new LCTransfer();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LCTransferDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get LCTransfer", e);
                #endregion
            }

            return oAccountHead;
        }
        public LCTransfer GetLog(int id, Int64 nUserId)
        {
            LCTransfer oAccountHead = new LCTransfer();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LCTransferDA.GetLog(tc, id);
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
                throw new ServiceException("Failed to Get LCTransfer", e);
                #endregion
            }

            return oAccountHead;
        }
        
        public List<LCTransfer> Gets(Int64 nUserID)
        {
            List<LCTransfer> oLCTransfer = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LCTransferDA.Gets(tc);
                oLCTransfer = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LCTransfer", e);
                #endregion
            }

            return oLCTransfer;
        }

        public List<LCTransfer> Gets(string sSQL, Int64 nUserID)
        {
            List<LCTransfer> oLCTransfer = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LCTransferDA.Gets(tc, sSQL);
                oLCTransfer = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LCTransfer", e);
                #endregion
            }

            return oLCTransfer;
        }
        public List<LCTransfer> Gets(int id, Int64 nUserID)
        {
            List<LCTransfer> oLCTransfer = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LCTransferDA.Gets(tc, id);
                oLCTransfer = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LCTransfer", e);
                #endregion
            }

            return oLCTransfer;
        }
        #endregion
    }
}
