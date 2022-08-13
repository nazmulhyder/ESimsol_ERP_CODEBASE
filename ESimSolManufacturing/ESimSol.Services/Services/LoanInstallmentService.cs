using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class LoanInstallmentService : MarshalByRefObject, ILoanInstallmentService
    {
        #region Private functions and declaration

        private LoanInstallment MapObject(NullHandler oReader)
        {
            LoanInstallment oLoanInstallment = new LoanInstallment();
            oLoanInstallment.LoanInstallmentID = oReader.GetInt32("LoanInstallmentID");
            oLoanInstallment.LoanID = oReader.GetInt32("LoanID");
            oLoanInstallment.InstallmentNo = oReader.GetString("InstallmentNo");
            oLoanInstallment.InstallmentStartDate = oReader.GetDateTime("InstallmentStartDate");
            oLoanInstallment.InstallmentDate = oReader.GetDateTime("InstallmentDate");
            oLoanInstallment.PrincipalAmount = oReader.GetDouble("PrincipalAmount");
            oLoanInstallment.LoanTransferType = (EnumLoanTransfer)oReader.GetInt32("LoanTransferType");
            oLoanInstallment.LoanTransferTypeInt = oReader.GetInt32("LoanTransferType");
            oLoanInstallment.TransferDate = oReader.GetDateTime("TransferDate");
            oLoanInstallment.TransferDays = oReader.GetInt32("TransferDays");
            oLoanInstallment.TransferInterestRate = oReader.GetDouble("TransferInterestRate");
            oLoanInstallment.TransferInterestAmount = oReader.GetDouble("TransferInterestAmount");
            oLoanInstallment.SettlementDate = oReader.GetDateTime("SettlementDate");
            oLoanInstallment.InterestDays = oReader.GetInt32("InterestDays");
            oLoanInstallment.InterestRate = oReader.GetDouble("InterestRate");
            oLoanInstallment.InterestAmount = oReader.GetDouble("InterestAmount");
            oLoanInstallment.LiborRate = oReader.GetDouble("LiborRate");
            oLoanInstallment.LiborInterestAmount = oReader.GetDouble("LiborInterestAmount");
            oLoanInstallment.TotalInterestAmount = oReader.GetDouble("TotalInterestAmount");
            oLoanInstallment.ChargeAmount = oReader.GetDouble("ChargeAmount");
            oLoanInstallment.DiscountPaidAmount = oReader.GetDouble("DiscountPaidAmount");
            oLoanInstallment.DiscountRcvAmount = oReader.GetDouble("DiscountRcvAmount");
            oLoanInstallment.TotalPayableAmount = oReader.GetDouble("TotalPayableAmount");
            oLoanInstallment.PaidAmount = oReader.GetDouble("PaidAmount");
            oLoanInstallment.PaidAmountBC = oReader.GetDouble("PaidAmountBC");
            oLoanInstallment.PrincipalDeduct = oReader.GetDouble("PrincipalDeduct");
            oLoanInstallment.PrincipalBalance = oReader.GetDouble("PrincipalBalance");
            oLoanInstallment.Remarks = oReader.GetString("Remarks");
            oLoanInstallment.SettlementBy = oReader.GetInt32("SettlementBy");
            oLoanInstallment.SettlementByName = oReader.GetString("SettlementByName");
            oLoanInstallment.FileNo = oReader.GetString("FileNo");
            oLoanInstallment.LoanNo = oReader.GetString("LoanNo");
            oLoanInstallment.LoanRefType = (EnumLoanRefType)oReader.GetInt32("LoanRefType");
            oLoanInstallment.LoanRefID = oReader.GetInt32("LoanRefID");
            oLoanInstallment.LoanRefNo = oReader.GetString("LoanRefNo");
            oLoanInstallment.LoanType = (EnumFinanceLoanType)oReader.GetInt32("LoanType");
            oLoanInstallment.LoanTypeInt = oReader.GetInt32("LoanType");
            oLoanInstallment.LoanCRate = oReader.GetDouble("LoanCRate");
            oLoanInstallment.LoanPrincipalAmount = oReader.GetDouble("LoanPrincipalAmount");
            oLoanInstallment.IssueDate = oReader.GetDateTime("IssueDate");
            oLoanInstallment.ApproxSettlement = oReader.GetDateTime("ApproxSettlement");
            oLoanInstallment.BankAccNo = oReader.GetString("BankAccNo");
            oLoanInstallment.LoanCurencyID = oReader.GetInt32("LoanCurencyID");
            oLoanInstallment.LoanCurency = oReader.GetString("LoanCurency");
            oLoanInstallment.BaseCSymbol = oReader.GetString("BaseCSymbol");
            return oLoanInstallment;
        }

        public LoanInstallment CreateObject(NullHandler oReader)
        {
            LoanInstallment oLoanInstallment = new LoanInstallment();
            oLoanInstallment = MapObject(oReader);
            return oLoanInstallment;
        }

        private List<LoanInstallment> CreateObjects(IDataReader oReader)
        {
            List<LoanInstallment> oLoanInstallment = new List<LoanInstallment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LoanInstallment oItem = CreateObject(oHandler);
                oLoanInstallment.Add(oItem);
            }
            return oLoanInstallment;
        }

        #endregion

        #region Interface implementation
        public LoanInstallment Save(LoanInstallment oLoanInstallment, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<LoanSettlement> oLoanSettlements = new List<LoanSettlement>();
            oLoanSettlements = oLoanInstallment.LoanchargeList;
            oLoanSettlements.AddRange(oLoanInstallment.PaymentList);
            string sLoanSettlementIDs = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLoanInstallment.LoanInstallmentID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Loan, EnumRoleOperationType.Add);
                    reader = LoanInstallmentDA.InsertUpdate(tc, oLoanInstallment, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Loan, EnumRoleOperationType.Edit);
                    reader = LoanInstallmentDA.InsertUpdate(tc, oLoanInstallment, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLoanInstallment = new LoanInstallment();
                    oLoanInstallment = CreateObject(oReader);
                }
                reader.Close();

                #region Settlment Part
                if (oLoanSettlements != null)
                {
                    foreach (LoanSettlement oItem in oLoanSettlements)
                    {
                        IDataReader readerdetail;
                        oItem.LoanInstallmentID = oLoanInstallment.LoanInstallmentID;
                        if (oItem.LoanSettlementID <= 0)
                        {
                            readerdetail = LoanSettlementDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = LoanSettlementDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sLoanSettlementIDs = sLoanSettlementIDs + oReaderDetail.GetString("LoanSettlementID") + ",";
                        }
                        readerdetail.Close();
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
                
                #region Get Loan Installment
                reader = LoanInstallmentDA.Get(tc, oLoanInstallment.LoanInstallmentID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLoanInstallment = new LoanInstallment();
                    oLoanInstallment = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oLoanInstallment = new LoanInstallment();
                    oLoanInstallment.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oLoanInstallment;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;            
            try
            {
                tc = TransactionContext.Begin(true);
                LoanInstallment oLoanInstallment = new LoanInstallment();
                oLoanInstallment.LoanInstallmentID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.LoanInstallment, EnumRoleOperationType.Delete);
                LoanInstallmentDA.Delete(tc, oLoanInstallment, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public LoanInstallment Approved(LoanInstallment oLoanInstallment, Int64 nUserID)
        {
            TransactionContext tc = null;            
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.LoanInstallment, EnumRoleOperationType.Add);
                reader = LoanInstallmentDA.Approved(tc, oLoanInstallment, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLoanInstallment = new LoanInstallment();
                    oLoanInstallment = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oLoanInstallment = new LoanInstallment();
                    oLoanInstallment.ErrorMessage = e.Message.Split('~')[0];
                }
                #endregion
            }
            return oLoanInstallment;
        }

        public LoanInstallment Get(int id, Int64 nUserId)
        {
            LoanInstallment oLoanInstallment = new LoanInstallment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = LoanInstallmentDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLoanInstallment = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LoanInstallment", e);
                #endregion
            }
            return oLoanInstallment;
        }

        public List<LoanInstallment> Gets(int LoanID, Int64 nUserID)
        {
            List<LoanInstallment> oLoanInstallments = new List<LoanInstallment>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LoanInstallmentDA.Gets(LoanID, tc);
                oLoanInstallments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                LoanInstallment oLoanInstallment = new LoanInstallment();
                oLoanInstallment.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oLoanInstallments;
        }

        public List<LoanInstallment> Gets(string sSQL, Int64 nUserID)
        {
            List<LoanInstallment> oLoanInstallments = new List<LoanInstallment>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LoanInstallmentDA.Gets(tc, sSQL);
                oLoanInstallments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LoanInstallment", e);
                #endregion
            }
            return oLoanInstallments;
        }

        #endregion
    }

}