using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;



namespace ESimSol.Services.Services
{
    
    public class ExportBillService : MarshalByRefObject, IExportBillService
    {
        #region Private functions and declaration
        private static ExportBill MapObject(NullHandler oReader)
        {
            ExportBill oExportBill = new ExportBill();
            oExportBill.ExportBillID = oReader.GetInt32("ExportBillID");
            oExportBill.ExportBillNo = oReader.GetString("ExportBillNo");
            oExportBill.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportBill.Amount = oReader.GetDouble("Amount");
            oExportBill.State = (EnumLCBillEvent)oReader.GetInt16("State");
            oExportBill.StateInt = oReader.GetInt16("State");
            oExportBill.StartDate = oReader.GetDateTime("StartDate");
            oExportBill.SendToParty = oReader.GetDateTime("SendToParty");
            oExportBill.RecdFromParty = oReader.GetDateTime("RecdFromParty");
            oExportBill.SendToBankDate = oReader.GetDateTime("SendToBank");
            oExportBill.RecedFromBankDate = oReader.GetDateTime("RecdFromBank");
            oExportBill.IsActive = oReader.GetBoolean("IsActive");
            oExportBill.Sequence = oReader.GetInt32("Sequence");
            oExportBill.NoOfPackages = oReader.GetString("NoOfPackages");
            oExportBill.NetWeight = oReader.GetString("NetWeight");
            oExportBill.GrossWeight = oReader.GetString("GrossWeight");
            oExportBill.Bill = oReader.GetString("Bill");
            oExportBill.Qty = oReader.GetDouble("Qty");
            oExportBill.OverDueAmount = oReader.GetDouble("OverDueAmount");
            oExportBill.ExpBankAccountID = oReader.GetInt32("ExpBankAccountID");
            oExportBill.ExpCurrencyID = oReader.GetInt32("ExpCurrencyID");
            oExportBill.ExpAmount = oReader.GetDouble("ExpAmount");
            oExportBill.ExpCRate = oReader.GetDouble("ExpCRate");
            oExportBill.ExpAmountBC = oReader.GetDouble("ExpAmountBC");
            oExportBill.ProductNature = (EnumProductNature)oReader.GetInt32("ProductNature");
            oExportBill.DiscountAdjAmount = oReader.GetDouble("DiscountAdjAmount");
            oExportBill.DiscountAdjCRate = oReader.GetDouble("DiscountAdjCRate");
            oExportBill.DiscountAdjAmountBC = oReader.GetDouble("DiscountAdjAmountBC");
            oExportBill.DiscountAdjIsGain = oReader.GetBoolean("DiscountAdjIsGain");
            oExportBill.DiscountAdjGainLossBC = oReader.GetDouble("DiscountAdjGainLossBC");

            // Drive property 
            ///Export LC
            oExportBill.FileNo = oReader.GetString("FileNo");
            oExportBill.ExportLCNo = oReader.GetString("ExportLCNo");
            oExportBill.Amount_LC = oReader.GetDouble("Amount_LC");
            oExportBill.LCOpeningDate = oReader.GetDateTime("LCOpeningDate");
            oExportBill.LCRecivedDate = oReader.GetDateTime("LCRecivedDate");
            oExportBill.DocPrepareDate = oReader.GetDateTime("DocPrepareDate");
            oExportBill.DocDate = oReader.GetDateTime("DocDate");
            oExportBill.BUID = oReader.GetInt32("BUID");
            //oExportBill.AtSightDiffered = oReader.GetBoolean("AtSightDiffered");

            oExportBill.ApplicantName = oReader.GetString("ApplicantName");
            oExportBill.ApplicantID = oReader.GetInt32("ApplicantID");
            oExportBill.BankBranchID_Nego = oReader.GetInt32("BankBranchID_Negotiation");
            oExportBill.BankBranchID_Bill = oReader.GetInt32("BankBranchID_Bill");
            oExportBill.BankBranchID_Advice = oReader.GetInt32("BankBranchID_Advice");
            oExportBill.BankBranchID_Issue = oReader.GetInt32("BankBranchID_Issue");
            oExportBill.BankBranchID_Ford = oReader.GetInt32("BankBranchID_Ford");
            oExportBill.BankBranchID_Endorse = oReader.GetInt32("BankBranchID_Endorse");
            oExportBill.OverDueRate = oReader.GetDouble("OverdueRate");
            oExportBill.ApplicantAddress = oReader.GetString("ApplicantAddress");
            oExportBill.Currency = oReader.GetString("Currency");
            ///LDBC
            oExportBill.LDBCID = oReader.GetInt32("ExportLDBCID");
            oExportBill.MaturityDate = oReader.GetDateTime("MaturityDate");
            oExportBill.MaturityReceivedDate = oReader.GetDateTime("MaturityReceivedDate");
            oExportBill.LDBCNo = oReader.GetString("LDBCNo");
            oExportBill.LDBPNo = oReader.GetString("LDBPNo");
            oExportBill.LDBPAmount = oReader.GetDouble("LDBPAmount");
            oExportBill.LDBCDate = oReader.GetDateTime("LDBCDate");
            oExportBill.AcceptanceDate = oReader.GetDateTime("AcceptanceDate");
            oExportBill.AcceptanceRate = oReader.GetDouble("AcceptanceRate");
            oExportBill.DiscountedDate = oReader.GetDateTime("DiscountedDate");
            oExportBill.BankFDDRecDate = oReader.GetDateTime("BankFDDRecDate");
            oExportBill.RelizationDate = oReader.GetDateTime("RelizationDate");
            oExportBill.EncashmentDate = oReader.GetDateTime("EncashmentDate");
            oExportBill.EncashCurrencyID = oReader.GetInt32("EncashCurrencyID");
            oExportBill.EncashCRate = oReader.GetDouble("EncashCRate");
            oExportBill.EncashAmountBC = oReader.GetDouble("EncashAmountBC");
            oExportBill.EncashRemarks = oReader.GetString("EncashRemarks");
            oExportBill.UPNo = oReader.GetString("UPNo");
            oExportBill.ForExAmount = oReader.GetDouble("ForExAmount");
            oExportBill.LCTramsID = oReader.GetInt32("LCTramsID");
            oExportBill.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportBill.BankName_Nego = oReader.GetString("BankName_Nego");
            oExportBill.BBranchName_Nego = oReader.GetString("BBranchName_Nego");
            oExportBill.BankAddress_Nego = oReader.GetString("BankAddress_Nego");
            oExportBill.BankName_Advice = oReader.GetString("BankName_Advice");
            oExportBill.BBranchName_Advice = oReader.GetString("BBranchName_Advice");
            oExportBill.BankAddress_Advice = oReader.GetString("BankAddress_Advice");
            oExportBill.BankName_Issue = oReader.GetString("BankName_Issue");
            oExportBill.BBranchName_Issue = oReader.GetString("BBranchName_Issue");
            oExportBill.BankAddress_Issue = oReader.GetString("BankAddress_Issue");
            oExportBill.LDBPCCRate = oReader.GetInt32("LDBPCCRate");
            oExportBill.LDBPCurrcncyID = oReader.GetInt32("LDBPCurrcncyID");
            oExportBill.LDBPCSymbol = oReader.GetString("LDBPCSymbol");
            oExportBill.BankName_Ford = oReader.GetString("BankName_Ford");
            oExportBill.BBranchName_Ford = oReader.GetString("BBranchName_Ford");
            oExportBill.BankName_Endorse = oReader.GetString("BankName_Endorse");
            oExportBill.BBranchName_Endorse = oReader.GetString("BBranchName_Endorse");

            oExportBill.ExportLCType = (EnumExportLCType)oReader.GetInt32("ExportLCType");
            if (oExportBill.ExportLCType == EnumExportLCType.FDD || oExportBill.ExportLCType == EnumExportLCType.TT) { oExportBill.ExportLCNo = oExportBill.ExportLCType.ToString() + "" + oExportBill.ExportLCNo; }
                  
            return oExportBill;            
        }

          public static  ExportBill CreateObject(NullHandler oReader)
        {
            ExportBill oExportBill = new ExportBill();
            oExportBill=MapObject(oReader);
            return oExportBill;
        }

        private List<ExportBill> CreateObjects(IDataReader oReader)
        {
            List<ExportBill> oExportBills = new List<ExportBill>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportBill oItem = CreateObject(oHandler);
                oExportBills.Add(oItem);
            }
            return oExportBills;
        }
        #endregion

        #region Interface implementation
        public ExportBillService() { }


        public ExportBill Save(ExportBill oExportBill, Int64 nUserID)
        {
            int nCount = 0;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
                ExportBillDetail oExportBillDetail = new ExportBillDetail();
                oExportBillDetails = oExportBill.ExportBillDetails;                
                IDataReader reader;
                if (oExportBill.ExportBillID <= 0)
                {                    
                    reader = ExportBillDA.InsertUpdate(tc, oExportBill, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    VoucherDA.CheckVoucherReference(tc, "ExportBill", "ExportBillID", oExportBill.ExportBillID);
                    DBOperationArchiveDA.Insert(tc, EnumDBOperation.Update, EnumModuleName.ExportBill, (int)oExportBill.ExportBillID, "View_ExportBill", "ExportBillID", "BUID", "ExportBillNo", nUserID);
                    reader = ExportBillDA.InsertUpdate(tc, oExportBill, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBill = new ExportBill();
                    oExportBill = CreateObject(oReader);
                }
                reader.Close();

                #region ExportBillDetails
                oExportBill.Amount = 0;
                oExportBill.Qty = 0;
                foreach (ExportBillDetail oItem in oExportBillDetails)
                {
                    IDataReader readerPPC;
                   
                    oItem.ExportBillID = oExportBill.ExportBillID;
                    oExportBill.Qty = oExportBill.Qty + oItem.Qty;
                    if (oItem.IsDeduct)
                    {
                        oExportBill.Amount = oExportBill.Amount - (oItem.Qty * oItem.UnitPrice);
                    }
                    else
                    {
                        oExportBill.Amount = oExportBill.Amount + (oItem.Qty * oItem.UnitPrice);
                    }
                    if (oItem.ExportBillDetailID <= 0)
                    {
                        readerPPC = ExportBillDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readerPPC = ExportBillDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReaderDetailPPC = new NullHandler(readerPPC);
                    //if (readerPPC.Read())
                    //{
                    //    sPPCIDs = sPPCIDs + oReaderDetailPPC.GetString("PILCMappingID") + ",";
                    //}
                    readerPPC.Close();

                }
                //if (sPPCIDs.Length > 0)
                //{
                //    sPPCIDs = sPPCIDs.Remove(sPPCIDs.Length - 1, 1);
                //}
                //oPurchasePaymentContract = new PurchasePaymentContract();
                //oPurchasePaymentContract.ExportLCID = oExportLC.ExportLCID;
                //PurchasePaymentContractDA.Delete(tc, oPurchasePaymentContract, EnumDBOperation.Delete, nUserID, sPPCIDs);
                #endregion


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportBill.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportBill;
        }
      
        public ExportBill Save_SendToBuyer(ExportBill oExportBill, Int64 nUserID)
        {
            ExportBill oExportBillReturn = new ExportBill();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;
                if (oExportBill.ExportBillID <= 0)
                {
                    reader = ExportBillHistoryDA.InsertUpdateHistory_SendToBuyer(tc, oExportBill, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportBillHistoryDA.InsertUpdateHistory_SendToBuyer(tc, oExportBill, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBillReturn = new ExportBill();
                    oExportBillReturn = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportBillReturn = new ExportBill();
                oExportBillReturn.ErrorMessage = e.Message.Split('~')[0];

                #endregion
            }

            return oExportBillReturn;

        }
        public ExportBill SaveHistory(ExportBill oExportBill, Int64 nUserID)
        {
            ExportBill oExportBillReturn = new ExportBill();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;
                if (oExportBill.ExportBillID <= 0)
                {
                    reader = ExportBillHistoryDA.InsertUpdateHistory(tc, oExportBill, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportBillHistoryDA.InsertUpdateHistory(tc, oExportBill, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBillReturn = new ExportBill();
                    oExportBillReturn = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportBillReturn = new ExportBill();
                oExportBillReturn.ErrorMessage = e.Message.Split('~')[0];

                #endregion
            }

            return oExportBillReturn;

        }
        public ExportBill SaveMaturityReceived(ExportBill oExportBill, Int64 nUserID)
        {
            ExportBill oExportBillReturn = new ExportBill();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;
                if (oExportBill.ExportBillID <= 0)
                {
                    reader = ExportBillHistoryDA.InsertUpdateMaturityReceived(tc, oExportBill, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportBillHistoryDA.InsertUpdateMaturityReceived(tc, oExportBill, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBillReturn = new ExportBill();
                    oExportBillReturn = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportBillReturn = new ExportBill();
                oExportBillReturn.ErrorMessage = e.Message.Split('~')[0];

                #endregion
            }

            return oExportBillReturn;

        }
        public ExportBill SaveSAN(ExportBill oExportBill, Int64 nUserID)
        {
            ExportBill oExportBillReturn = new ExportBill();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;
                if (oExportBill.ExportBillID <= 0)
                {
                    reader = ExportBillHistoryDA.InsertUpdateSAN(tc, oExportBill, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportBillHistoryDA.InsertUpdateSAN(tc, oExportBill, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBillReturn = new ExportBill();
                    oExportBillReturn = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportBillReturn = new ExportBill();
                oExportBillReturn.ErrorMessage = e.Message.Split('~')[0];

                #endregion
            }

            return oExportBillReturn;

        }

        public ExportBill Save_Encashment(ExportBill oExportBill, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                List<ExportBillEncashment> oExportBillEncashments=new List<ExportBillEncashment>();
                oExportBillEncashments = oExportBill.ExportBillEncashments;

                #region ExportBillEncashment
                reader = ExportBillHistoryDA.ExportBill_Encashment(tc, oExportBill, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBill = new ExportBill();
                    oExportBill = CreateObject(oReader);
                }
                reader.Close();
                #endregion 

                #region ExportBillEncashment Distribution
                if (oExportBill.ExportBillID > 0)
                {
                    if (oExportBillEncashments!=null && oExportBillEncashments.Count > 0)
                    {
                        string sExportBillEncashmentIDs = "";
                        string sLoanSettlementIDs = "";
                        foreach (ExportBillEncashment oEBE in oExportBillEncashments)
                        {

                            if (oEBE.LoanInstallment != null && oEBE.LoanInstallment.LoanID > 0)
                            {
                                #region Loan Installment
                                LoanInstallment oLoanInstallment = new LoanInstallment();
                                List<LoanSettlement> oLoanSettlements = new List<LoanSettlement>();
                                oLoanInstallment = oEBE.LoanInstallment;
                                oLoanSettlements = oEBE.LoanInstallment.LoanchargeList;
                                oLoanSettlements.AddRange(oEBE.LoanInstallment.PaymentList);

                                IDataReader readerInstallment;
                                if (oLoanInstallment.LoanInstallmentID <= 0)
                                {
                                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Loan, EnumRoleOperationType.Add);
                                    readerInstallment = LoanInstallmentDA.InsertUpdate(tc, oLoanInstallment, EnumDBOperation.Insert, nUserID);
                                }
                                else
                                {
                                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Loan, EnumRoleOperationType.Edit);
                                    readerInstallment = LoanInstallmentDA.InsertUpdate(tc, oLoanInstallment, EnumDBOperation.Update, nUserID);
                                }
                                NullHandler oReaderInstallment = new NullHandler(readerInstallment);
                                if (readerInstallment.Read())
                                {
                                    oLoanInstallment = new LoanInstallment();
                                    oLoanInstallment = (new LoanInstallmentService()).CreateObject(oReaderInstallment);
                                    
                                }
                                readerInstallment.Close();

                                #region Settlment Part
                                if (oLoanSettlements != null)
                                {
                                    sLoanSettlementIDs = "";
                                    foreach (LoanSettlement oItem in oLoanSettlements)
                                    {
                                        IDataReader readerSettlement;
                                        oItem.LoanInstallmentID = oLoanInstallment.LoanInstallmentID;
                                        if (oItem.LoanSettlementID <= 0)
                                        {
                                            readerSettlement = LoanSettlementDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                                        }
                                        else
                                        {
                                            readerSettlement = LoanSettlementDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                                        }
                                        NullHandler oReaderSettlement = new NullHandler(readerSettlement);
                                        if (readerSettlement.Read())
                                        {
                                            sLoanSettlementIDs = sLoanSettlementIDs + oReaderSettlement.GetString("LoanSettlementID") + ",";
                                        }
                                        readerSettlement.Close();
                                    }
                                    if (sLoanSettlementIDs.Length > 0)
                                    {
                                        sLoanSettlementIDs = sLoanSettlementIDs.Remove(sLoanSettlementIDs.Length - 1, 1);
                                    }
                                    LoanSettlement oLoanSettlement = new LoanSettlement();
                                    oLoanSettlement.LoanInstallmentID = oLoanInstallment.LoanInstallmentID;
                                    LoanSettlementDA.Delete(tc, oLoanSettlement, EnumDBOperation.Delete, nUserID, sLoanSettlementIDs);
                                }
                                #endregion


                                #region Installment Approved
                                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.LoanInstallment, EnumRoleOperationType.Add);
                                readerInstallment = LoanInstallmentDA.Approved(tc, oLoanInstallment, EnumDBOperation.Approval, nUserID);
                                oReaderInstallment = new NullHandler(readerInstallment);
                                if (readerInstallment.Read())
                                {
                                    oLoanInstallment = new LoanInstallment();
                                    oLoanInstallment = (new LoanInstallmentService()).CreateObject(oReaderInstallment);
                                    oEBE.LoanInstallmentID = oLoanInstallment.LoanInstallmentID;
                                }
                                readerInstallment.Close();
                                #endregion

                                #endregion
                            }
                            
                            IDataReader readerdetail;
                            oEBE.ExportBillID = oExportBill.ExportBillID;
                            if (oEBE.ExportBillEncashmentID <= 0)
                            {
                                readerdetail = ExportBillEncashmentDA.InsertUpdate(tc, oEBE, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = ExportBillEncashmentDA.InsertUpdate(tc, oEBE, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderExportBillEncashment = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sExportBillEncashmentIDs = sExportBillEncashmentIDs + oReaderExportBillEncashment.GetString("ExportBillEncashmentID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sExportBillEncashmentIDs.Length > 0)
                        {
                            sExportBillEncashmentIDs = sExportBillEncashmentIDs.Remove(sExportBillEncashmentIDs.Length - 1, 1);
                        }

                        ExportBillEncashment oExportBillEncashment = new ExportBillEncashment();
                        oExportBillEncashment.ExportBillID = oExportBill.ExportBillID;
                        ExportBillEncashmentDA.Delete(tc, oExportBillEncashment, EnumDBOperation.Delete, nUserID, sExportBillEncashmentIDs);
                    }

                }
                #endregion                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportBill.ErrorMessage = e.Message;
                oExportBill.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportBill;
        }
        public ExportBill Save_BillRealized(ExportBill oExportBill, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                List<ExportBillRealized> oExportBillRealizeds = new List<ExportBillRealized>();
                oExportBillRealizeds = oExportBill.ExportBillRealizeds;

                #region
                if (oExportBill.ExportBillID <= 0)
                {
                    reader = ExportBillHistoryDA.InsertUpdate_Realized(tc, oExportBill, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportBillHistoryDA.InsertUpdate_Realized(tc, oExportBill, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBill = new ExportBill();
                    oExportBill = CreateObject(oReader);
                }
                reader.Close();
                #endregion 
                #region ExportBillRealizeds
                if (oExportBill.ExportBillID > 0)
                {
                    if (oExportBillRealizeds.Count > 0)
                    {
                        foreach (ExportBillRealized oEBE in oExportBillRealizeds)
                        {
                            oEBE.ExportBillID = oExportBill.ExportBillID;
                            if (oEBE.ExportBillRealizedID <= 0)
                            {
                                reader = ExportBillRealizedDA.InsertUpdate(tc, oEBE, EnumDBOperation.Insert, nUserID);
                            }
                            else
                            {
                                reader = ExportBillRealizedDA.InsertUpdate(tc, oEBE, EnumDBOperation.Update, nUserID);
                            }
                            NullHandler oSampleDetail = new NullHandler(reader);

                            reader.Close();
                        }
                    }
                }
                #endregion ExportBillEncashment

                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportBill.ErrorMessage = e.Message;
                oExportBill.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportBill;
        }
      
        public string Delete(ExportBill oExportBill, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VoucherDA.CheckVoucherReference(tc, "ExportBill", "ExportBillID", oExportBill.ExportBillID);
                DBOperationArchiveDA.Insert(tc, EnumDBOperation.Delete, EnumModuleName.ExportBill, (int)oExportBill.ExportBillID, "View_ExportBill", "ExportBillID", "BUID", "ExportBillNo", nUserId);
                ExportBillDA.Delete(tc, oExportBill, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public ExportBill Get(int id, Int64 nUserID)
        {
            ExportBill oExportBill = new ExportBill();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportBillDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBill = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportBill", e);
                #endregion
            }

            return oExportBill;
        }
        public ExportBill GetByLDBCNo(string sLDBCNo, Int64 nUserID)
        {
            ExportBill oExportBill = new ExportBill();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportBillDA.GetByLDBCNo(tc, sLDBCNo);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBill = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportBill", e);
                #endregion
            }

            return oExportBill;
        }
        public List<ExportBill> Gets( Int64 nUserID)
        {
            List<ExportBill> oExportBills = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportBillDA.Gets(tc);
                oExportBills = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBills", e);
                #endregion
            }

            return oExportBills;
        }
        public List<ExportBill> Gets(int nExportLCID, Int64 nUserID)
        {
            List<ExportBill> oExportBills = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportBillDA.Gets(tc, nExportLCID);
                oExportBills = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBills", e);
                #endregion
            }

            return oExportBills;
        }
        public List<ExportBill> GetsByPI(string nExportPIID, Int64 nUserID)
        {
            List<ExportBill> oExportBills = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportBillDA.GetsByPI(tc, nExportPIID);
                oExportBills = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBills", e);
                #endregion
            }

            return oExportBills;
        }
        public ExportBill Save_UpdateStatus(ExportBill oExportBill, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ExportBillHistoryDA.InsertUpdateHistory_Manualy(tc, oExportBill, EnumDBOperation.Insert, nUserID);
               // reader = ExportBillDA.UpdateStatus(tc, oExportBill);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBill = new ExportBill();
                    oExportBill = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportBill.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oExportBill;

        }
      
        public List<ExportBill> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportBill> oExportBills = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportBillDA.Gets(tc, sSQL);
                oExportBills = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBills", e);
                #endregion
            }

            return oExportBills;
        }
      
        public List<ExportBill> GetBills(int eBillEvent, Int64 nUserID)
        {
            List<ExportBill> oExportBills = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportBillDA.GetBills(tc, eBillEvent);
                oExportBills = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBills", e);
                #endregion
            }

            return oExportBills;
        }

        public ExportBill SaveDocDate(ExportBill oExportBill, Int64 nUserId)
        {
           
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportBillDA.SaveDocDate(tc, oExportBill, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBill = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportBill = new ExportBill();
                oExportBill.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportBill;
        }




        #endregion
    }
}
