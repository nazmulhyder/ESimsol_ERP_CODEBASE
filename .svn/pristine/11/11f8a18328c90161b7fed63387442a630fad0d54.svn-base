using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class LoanRegisterService : MarshalByRefObject, ILoanRegisterService
    {
        #region Private functions and declaration

        private LoanRegister MapObject(NullHandler oReader)
        {
            LoanRegister oLoanRegister = new LoanRegister();
            oLoanRegister.LoanInstallmentID = oReader.GetInt32("LoanInstallmentID");
            oLoanRegister.LoanID = oReader.GetInt32("LoanID");
            oLoanRegister.InstallmentNo = oReader.GetString("InstallmentNo");
            oLoanRegister.InstallmentStartDate = oReader.GetDateTime("InstallmentStartDate");
            oLoanRegister.InstallmentDate = oReader.GetDateTime("InstallmentDate");
            oLoanRegister.InstallmentPrincipal = oReader.GetDouble("InstallmentPrincipal");
            oLoanRegister.LoanTransferType = (EnumLoanTransfer)oReader.GetInt32("LoanTransferType");
            oLoanRegister.TransferDate = oReader.GetDateTime("TransferDate");
            oLoanRegister.TransferDays = oReader.GetInt32("TransferDays");
            oLoanRegister.TransferInterestRate = oReader.GetDouble("TransferInterestRate");
            oLoanRegister.TransferInterestAmount = oReader.GetDouble("TransferInterestAmount");
            oLoanRegister.SettlementDate = oReader.GetDateTime("SettlementDate");
            oLoanRegister.InterestDays = oReader.GetInt32("InterestDays");
            oLoanRegister.InstallmentInterestRate = oReader.GetDouble("InstallmentInterestRate");
            oLoanRegister.InstallmentInterestAmount = oReader.GetDouble("InstallmentInterestAmount");
            oLoanRegister.InstallmentLiborRate = oReader.GetDouble("InstallmentLiborRate");
            oLoanRegister.InstallmentLiborInterestAmount = oReader.GetDouble("InstallmentLiborInterestAmount");
            oLoanRegister.ChargeAmount = oReader.GetDouble("ChargeAmount");
            oLoanRegister.TotalPayableAmount = oReader.GetDouble("TotalPayableAmount");
            oLoanRegister.PaidAmount = oReader.GetDouble("PaidAmount");
            oLoanRegister.PrincipalDeduct = oReader.GetDouble("PrincipalDeduct");
            oLoanRegister.PrincipalBalance = oReader.GetDouble("PrincipalBalance");
            oLoanRegister.SettleByName = oReader.GetString("SettleByName");
            oLoanRegister.Remarks = oReader.GetString("Remarks");
            oLoanRegister.LoanNo = oReader.GetString("LoanNo");
            oLoanRegister.LoanRefType = (EnumLoanRefType)oReader.GetInt32("LoanRefType");
            oLoanRegister.LoanType = (EnumFinanceLoanType)oReader.GetInt32("LoanType");
            oLoanRegister.LoanRefID = oReader.GetInt32("LoanRefID");
            oLoanRegister.LoanRefNo = oReader.GetString("LoanRefNo");
            oLoanRegister.IssueDate = oReader.GetDateTime("IssueDate");
            oLoanRegister.BUID = oReader.GetInt32("BUID");
            oLoanRegister.BUName = oReader.GetString("BUName");
            oLoanRegister.BUShortName = oReader.GetString("BUShortName");
            oLoanRegister.LoanStartDate = oReader.GetDateTime("LoanStartDate");
            oLoanRegister.PrincipalAmount = oReader.GetDouble("PrincipalAmount");
            oLoanRegister.LoanCurencyID = oReader.GetInt32("LoanCurencyID");
            oLoanRegister.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oLoanRegister.CRate = oReader.GetDouble("CRate");
            oLoanRegister.PrincipalAmountBC = oReader.GetDouble("PrincipalAmountBC");
            oLoanRegister.LoanAmount = oReader.GetDouble("LoanAmount");
            oLoanRegister.InterestRate = oReader.GetDouble("InterestRate");
            oLoanRegister.LiborRate = oReader.GetDouble("LiborRate");
            oLoanRegister.StlmtStartDate = oReader.GetDateTime("StlmtStartDate");
            oLoanRegister.RcvBankAccountID = oReader.GetInt32("RcvBankAccountID");
            oLoanRegister.RcvBankAccountNo = oReader.GetString("RcvBankAccountNo");
            oLoanRegister.BankShortName = oReader.GetString("BankShortName");
            oLoanRegister.RcvDate = oReader.GetDateTime("RcvDate");
            oLoanRegister.ApprovedByName = oReader.GetString("ApprovedByName");
            oLoanRegister.ApprovedDate = oReader.GetDateTime("ApprovedDate");
            oLoanRegister.ReceivedByName = oReader.GetString("ReceivedByName");
            return oLoanRegister;
        }

        private LoanRegister CreateObject(NullHandler oReader)
        {
            LoanRegister oLoanRegister = new LoanRegister();
            oLoanRegister = MapObject(oReader);
            return oLoanRegister;
        }

        private List<LoanRegister> CreateObjects(IDataReader oReader)
        {
            List<LoanRegister> oLoanRegister = new List<LoanRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LoanRegister oItem = CreateObject(oHandler);
                oLoanRegister.Add(oItem);
            }
            return oLoanRegister;
        }

        #endregion

        #region Interface implementation
      
      
        public List<LoanRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<LoanRegister> oLoanRegisters = new List<LoanRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LoanRegisterDA.Gets(tc, sSQL);
                oLoanRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LoanRegister", e);
                #endregion
            }
            return oLoanRegisters;
        }

        #endregion
    }

}
