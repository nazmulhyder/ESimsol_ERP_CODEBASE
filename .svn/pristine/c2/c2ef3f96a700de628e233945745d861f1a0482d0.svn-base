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
    public class TradingPaymentService : MarshalByRefObject, ITradingPaymentService
    {
        #region Private functions and declaration
        private TradingPayment MapObject(NullHandler oReader)
        {
            TradingPayment oTradingPayment = new TradingPayment();
            oTradingPayment.TradingPaymentID = oReader.GetInt32("TradingPaymentID");
            oTradingPayment.BUID = oReader.GetInt32("BUID");
            oTradingPayment.ContractorID = oReader.GetInt32("ContractorID");
            oTradingPayment.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oTradingPayment.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oTradingPayment.RefNo = oReader.GetString("RefNo");
            oTradingPayment.PaymentDate = oReader.GetDateTime("PaymentDate");
            oTradingPayment.EncashmentDate = oReader.GetDateTime("EncashmentDate");
            oTradingPayment.PaymentMethod = (EnumPaymentMethod)oReader.GetInt32("PaymentMethod");
            oTradingPayment.PaymentMethodInt = oReader.GetInt32("PaymentMethod");
            oTradingPayment.CurrencyID = oReader.GetInt32("CurrencyID");
            oTradingPayment.Amount = oReader.GetDouble("Amount");
            oTradingPayment.ReferenceType = (EnumPaymentRefType)oReader.GetInt32("ReferenceType");
            oTradingPayment.ReferenceTypeInt = oReader.GetInt32("ReferenceType");
            oTradingPayment.TradingPaymentStatus = (EnumPaymentStatus)oReader.GetInt32("TradingPaymentStatus");
            oTradingPayment.TradingPaymentStatusInt = oReader.GetInt32("TradingPaymentStatus");
            oTradingPayment.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oTradingPayment.ApprovedDate = oReader.GetDateTime("ApprovedDate");
            oTradingPayment.Note = oReader.GetString("Note");
            oTradingPayment.ChequeNo = oReader.GetString("ChequeNo");
            oTradingPayment.AccountID = oReader.GetInt32("AccountID");
            oTradingPayment.BankName = oReader.GetString("BankName");
            oTradingPayment.AccountNo = oReader.GetString("AccountNo");
            oTradingPayment.BranchName = oReader.GetString("BranchName");
            oTradingPayment.BUName = oReader.GetString("BUName");
            oTradingPayment.BUCode = oReader.GetString("BUCode");
            oTradingPayment.ContractorName = oReader.GetString("ContractorName");
            oTradingPayment.ContactPersonName = oReader.GetString("ContactPersonName");            
            oTradingPayment.CurrencyName = oReader.GetString("CurrencyName");
            oTradingPayment.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oTradingPayment.AccountCode = oReader.GetString("AccountCode");
            oTradingPayment.AccountHeadName = oReader.GetString("AccountHeadName");
            oTradingPayment.ApprovedByName = oReader.GetString("ApprovedByName");
            oTradingPayment.SalesManID = oReader.GetInt32("SalesManID");
            oTradingPayment.SalesManName = oReader.GetString("SalesManName");
            return oTradingPayment;
        }

        private TradingPayment CreateObject(NullHandler oReader)
        {
            TradingPayment oTradingPayment = new TradingPayment();
            oTradingPayment = MapObject(oReader);
            return oTradingPayment;
        }

        private List<TradingPayment> CreateObjects(IDataReader oReader)
        {
            List<TradingPayment> oTradingPayments = new List<TradingPayment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TradingPayment oItem = CreateObject(oHandler);
                oTradingPayments.Add(oItem);
            }
            return oTradingPayments;
        }
        #endregion

        #region Interface implementation
        public TradingPaymentService() { }

        public TradingPayment Save(TradingPayment oTradingPayment, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<TradingPaymentDetail> oTradingPaymentDetails = new List<TradingPaymentDetail>();
                TradingPaymentDetail oTradingPaymentDetail = new TradingPaymentDetail();

                oTradingPaymentDetails = oTradingPayment.TradingPaymentDetails;
                string sTradingPaymentDetailIDs = "";

                #region TradingPayment part
                IDataReader reader;
                if (oTradingPayment.TradingPaymentID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingPayment, EnumRoleOperationType.Add);
                    reader = TradingPaymentDA.InsertUpdate(tc, oTradingPayment, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingPayment, EnumRoleOperationType.Edit);
                    reader = TradingPaymentDA.InsertUpdate(tc, oTradingPayment, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingPayment = new TradingPayment();
                    oTradingPayment = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Valid TradingPayment
                if (oTradingPayment.TradingPaymentID <= 0)
                {
                    throw new Exception("Invalid TradingPayment!");
                }
                #endregion

                #region TradingPayment Detail Part
                if (oTradingPaymentDetails != null)
                {
                    foreach (TradingPaymentDetail oItem in oTradingPaymentDetails)
                    {
                        IDataReader readerdetail;
                        oItem.TradingPaymentID = oTradingPayment.TradingPaymentID;
                        if (oItem.TradingPaymentDetailID <= 0)
                        {
                            readerdetail = TradingPaymentDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerdetail = TradingPaymentDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sTradingPaymentDetailIDs = sTradingPaymentDetailIDs + oReaderDetail.GetString("TradingPaymentDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sTradingPaymentDetailIDs.Length > 0)
                    {
                        sTradingPaymentDetailIDs = sTradingPaymentDetailIDs.Remove(sTradingPaymentDetailIDs.Length - 1, 1);
                    }
                    oTradingPaymentDetail = new TradingPaymentDetail();
                    oTradingPaymentDetail.TradingPaymentID = oTradingPayment.TradingPaymentID;
                    TradingPaymentDetailDA.Delete(tc, oTradingPaymentDetail, EnumDBOperation.Delete, nUserID, sTradingPaymentDetailIDs);
                }

                #endregion

                #region Get TradingPayment
                reader = TradingPaymentDA.Get(tc, oTradingPayment.TradingPaymentID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingPayment = CreateObject(oReader);
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

                oTradingPayment = new TradingPayment();
                oTradingPayment.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oTradingPayment;
        }

        public TradingPayment Approved(TradingPayment oTradingPayment, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);                               
                
                #region Valid TradingPayment
                if (oTradingPayment.TradingPaymentID <= 0)
                {
                    throw new Exception("Invalid TradingPayment!");
                }
                if (oTradingPayment.ApprovedBy != 0)
                {
                    throw new Exception("Your Selected TradingPayment Already Approved!");
                }
                #endregion

                #region Approved PurchaseInvocie
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingPayment, EnumRoleOperationType.Approved);
                TradingPaymentDA.Approved(tc, oTradingPayment, nUserID);
                #endregion

                #region Get TradingPayment
                IDataReader reader;
                NullHandler oReader;
                reader = TradingPaymentDA.Get(tc, oTradingPayment.TradingPaymentID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingPayment = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Salesman Log
                if (oTradingPayment.TradingPaymentID > 0 && oTradingPayment.SalesManID > 0 && oTradingPayment.ApprovedBy!=0)
                {
                    reader = TradingPaymentDA.SalesmanTradingPayment(tc, oTradingPayment.TradingPaymentID, nUserID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTradingPayment = CreateObject(oReader);
                    }
                    reader.Close();
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTradingPayment = new TradingPayment();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oTradingPayment.ErrorMessage = Message;

                #endregion
            }
            return oTradingPayment;
        }

        public string Delete(TradingPayment oTradingPayment, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingPayment, EnumRoleOperationType.Delete);
                TradingPaymentDA.Delete(tc, oTradingPayment, EnumDBOperation.Delete, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                //throw new Exception(e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public TradingPayment Get(int nTradingPaymentID, int nUserID)
        {
            TradingPayment oTradingPayment = new TradingPayment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TradingPaymentDA.Get(tc, nTradingPaymentID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingPayment = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new Exception("Failed to Get TradingPayment \n" + e.Message, e);
                #endregion
            }
            return oTradingPayment;
        }

        public List<TradingPayment> Gets(int nUserID)
        {
            List<TradingPayment> oTradingPayments = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingPaymentDA.Gets(tc);
                oTradingPayments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingPayment \n" + e.Message, e);
                #endregion
            }

            return oTradingPayments;
        }

        public List<TradingPayment> Gets(string sSQL, int nUserID)
        {
            List<TradingPayment> oTradingPayments = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingPaymentDA.Gets(tc, sSQL);
                oTradingPayments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingPayments \n" + e.Message, e);
                #endregion
            }
            return oTradingPayments;
        }
        public List<TradingPayment> GetsInitialTradingPayments(int nBUID, EnumPaymentRefType eTradingPaymentRefType, int TradingPaymentBy, int nUserID)
        {
            List<TradingPayment> oTradingPayments = new List<TradingPayment>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingPaymentDA.GetsInitialTradingPayments(tc, nBUID, eTradingPaymentRefType, TradingPaymentBy, nUserID);
                oTradingPayments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingPayments \n" + e.Message, e);
                #endregion
            }
            return oTradingPayments;
        }
        
        #endregion
    }
}
