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
    public class EmployeeLoanService : MarshalByRefObject, IEmployeeLoanService
    {
        #region Private functions and declaration
        private EmployeeLoan MapObject(NullHandler oReader)
        {
            EmployeeLoan oEmployeeLoan = new EmployeeLoan();
            oEmployeeLoan.EmployeeLoanID = oReader.GetInt32("EmployeeLoanID");
            oEmployeeLoan.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeLoan.Code = oReader.GetString("Code");
            oEmployeeLoan.IsActive = oReader.GetBoolean("IsActive");
            oEmployeeLoan.LoanType = (EnumLoanType)oReader.GetInt16("LoanType");
            oEmployeeLoan.Purpose = oReader.GetString("Purpose");
            oEmployeeLoan.InterestRate = oReader.GetDouble("InterestRate");
            oEmployeeLoan.NoOfTotalInstallment = oReader.GetInt16("NoOfTotalInstallment");
            oEmployeeLoan.InstallmentAmount = oReader.GetInt16("InstallmentAmount");
            oEmployeeLoan.RecommendBy = oReader.GetInt32("RecommendBy");
            oEmployeeLoan.RecommendNote = oReader.GetString("RecommendNote");
            oEmployeeLoan.RecommendDate = oReader.GetDateTime("RecommendDate");
            oEmployeeLoan.ApproveBy = oReader.GetInt32("ApproveBy");
            oEmployeeLoan.ApproveNote = oReader.GetString("ApproveNote");
            oEmployeeLoan.ApproveDate = oReader.GetDateTime("ApproveDate");
            oEmployeeLoan.IsFinish = oReader.GetBoolean("IsFinish");
            oEmployeeLoan.FinishNote = oReader.GetString("FinishNote");
            oEmployeeLoan.IsPFLoan = oReader.GetBoolean("IsPFLoan");
            oEmployeeLoan.LoanAmount = oReader.GetDouble("LoanAmount");
            oEmployeeLoan.EmployeeName = oReader.GetString("EmployeeName");
            oEmployeeLoan.EmployeeCode = oReader.GetString("EmployeeCode");
            oEmployeeLoan.RecommendByName = oReader.GetString("RecommendByName");
            oEmployeeLoan.ApproveByName = oReader.GetString("ApproveByName");
            return oEmployeeLoan;
        }

        public static EmployeeLoan CreateObject(NullHandler oReader)
        {
            EmployeeLoan oEmployeeLoan = new EmployeeLoan();
            EmployeeLoanService oService = new EmployeeLoanService();
            oEmployeeLoan = oService.MapObject(oReader);
            return oEmployeeLoan;
        }
        private List<EmployeeLoan> CreateObjects(IDataReader oReader)
        {
            List<EmployeeLoan> oEmployeeLoans = new List<EmployeeLoan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeLoan oItem = CreateObject(oHandler);
                oEmployeeLoans.Add(oItem);
            }
            return oEmployeeLoans;
        }

        #endregion

        #region Interface implementation
        public EmployeeLoanService() { }

        public EmployeeLoan Get(int nEmployeeLoanID, long nUserID)
        {
            EmployeeLoan oEmployeeLoan = new EmployeeLoan();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeLoanDA.Get(tc, nEmployeeLoanID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeLoan = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oEmployeeLoan.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeLoan;
        }

        public List<EmployeeLoan> Gets(string sSQL, long nUserID)
        {
            List<EmployeeLoan> oEmployeeLoans = new List<EmployeeLoan>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLoanDA.Gets(tc, sSQL);
                oEmployeeLoans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                EmployeeLoan oEmployeeLoan = new EmployeeLoan();
                oEmployeeLoan.ErrorMessage = e.Message;
                oEmployeeLoans.Add(oEmployeeLoan);
                #endregion
            }

            return oEmployeeLoans;
        }


        #endregion
    }
}