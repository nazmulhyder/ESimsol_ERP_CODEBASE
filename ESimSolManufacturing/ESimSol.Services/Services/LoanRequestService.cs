using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class LoanRequestService : MarshalByRefObject, ILoanRequestService
    {
        #region Private functions and declaration
        private LoanRequest MapObject(NullHandler oReader)
        {
            LoanRequest oLoanRequest = new LoanRequest();
            oLoanRequest.LoanRequestID = oReader.GetInt32("LoanRequestID");
            oLoanRequest.RequestNo = oReader.GetString("RequestNo");
            oLoanRequest.EmployeeID = oReader.GetInt32("EmployeeID");
            oLoanRequest.LoanType = (EnumLoanType)oReader.GetInt16("LoanType");
            oLoanRequest.RequestDate = oReader.GetDateTime("RequestDate");
            oLoanRequest.ExpectDate = oReader.GetDateTime("ExpectDate");
            oLoanRequest.Purpose = oReader.GetString("Purpose");
            oLoanRequest.RequestStatus = (EnumRequestStatus)oReader.GetInt32("RequestStatus");
            oLoanRequest.LoanAmount = oReader.GetDouble("LoanAmount");
            oLoanRequest.NoOfInstallment = oReader.GetInt16("NoOfInstallment");
            oLoanRequest.InstallmentAmount = oReader.GetDouble("InstallmentAmount");
            oLoanRequest.InterestRate = oReader.GetDouble("InterestRate");
            oLoanRequest.Remarks = oReader.GetString("Remarks");
            oLoanRequest.ProceedBy = oReader.GetInt32("ProceedBy");
            oLoanRequest.EmployeeLoanID = oReader.GetInt32("EmployeeLoanID");
            oLoanRequest.IsPFLoan = oReader.GetBoolean("IsPFLoan");
            oLoanRequest.EmployeeName = oReader.GetString("EmployeeName");
            oLoanRequest.EmployeeCode = oReader.GetString("EmployeeCode");
            oLoanRequest.ProceedByName = oReader.GetString("ProceedByName");
            
            return oLoanRequest;
        }

        public static LoanRequest CreateObject(NullHandler oReader)
        {
            LoanRequest oLoanRequest = new LoanRequest();
            LoanRequestService oService = new LoanRequestService();
            oLoanRequest = oService.MapObject(oReader);
            return oLoanRequest;
        }
        private List<LoanRequest> CreateObjects(IDataReader oReader)
        {
            List<LoanRequest> oLoanRequests = new List<LoanRequest>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LoanRequest oItem = CreateObject(oHandler);
                oLoanRequests.Add(oItem);
            }
            return oLoanRequests;
        }

        #endregion

        #region Interface implementation
        public LoanRequestService() { }

        public LoanRequest IUD(LoanRequest oLoanRequest, int nDBOperation, long nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = LoanRequestDA.IUD(tc, oLoanRequest, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLoanRequest = new LoanRequest();
                    oLoanRequest = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete) { oLoanRequest = new LoanRequest(); oLoanRequest.ErrorMessage = Global.DeleteMessage; }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oLoanRequest.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oLoanRequest;
        }


        public LoanRequest Approval(LoanRequest oLoanRequest, List<EmployeeLoanInstallment> oELIs, long nUserID)
        {
            TransactionContext tc = null;
            try
            {
                string sNote = oLoanRequest.Params;
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;

                /*--- Update Loan Request ----*/
                reader = LoanRequestDA.IUD(tc, oLoanRequest, (int)EnumDBOperation.Update, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLoanRequest = new LoanRequest();
                    oLoanRequest = CreateObject(oReader);
                }
                reader.Close();

                /*--- Approval ----*/
                reader = LoanRequestDA.Approval(tc, oLoanRequest.LoanRequestID, (short)EnumRequestStatus.Approve, sNote, oLoanRequest.EmployeeLoanID, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLoanRequest = new LoanRequest();
                    oLoanRequest = CreateObject(oReader);
                }
                reader.Close();

                /*--- Get Employee Loan ID ----*/
                int nEmployeeLoanID = EmployeeLoanAmountDA.GetEmployeeLoanID(tc, oLoanRequest.LoanRequestID);

                /*--- Employee Loan Installment ----*/
                EmployeeLoanInstallment oELI = new EmployeeLoanInstallment();
                foreach(EmployeeLoanInstallment oItem in oELIs)
                {
                    oItem.EmployeeLoanID = nEmployeeLoanID;
                    reader = EmployeeLoanInstallmentDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oELI = new EmployeeLoanInstallment();
                        oELI = EmployeeLoanInstallmentService.CreateObject(oReader);
                    }
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oLoanRequest.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oLoanRequest;
        }

        public LoanRequest Get(int nLoanRequestID, long nUserID)
        {
            LoanRequest oLoanRequest = new LoanRequest();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LoanRequestDA.Get(tc, nLoanRequestID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLoanRequest = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oLoanRequest.ErrorMessage = e.Message;
                #endregion
            }

            return oLoanRequest;
        }

        public List<LoanRequest> Gets(string sSQL, long nUserID)
        {
            List<LoanRequest> oLoanRequests = new List<LoanRequest>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LoanRequestDA.Gets(tc, sSQL);
                oLoanRequests = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                LoanRequest oLoanRequest = new LoanRequest();
                oLoanRequest.ErrorMessage = e.Message;
                oLoanRequests.Add(oLoanRequest);
                #endregion
            }

            return oLoanRequests;
        }


        #endregion
    }
}