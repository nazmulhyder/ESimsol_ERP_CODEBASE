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
    public class LoanSummaryService : MarshalByRefObject, ILoanSummaryService
    {
        #region Private functions and declaration
        private LoanSummary MapObject(NullHandler oReader)
        {
            LoanSummary oLoanSummary = new LoanSummary();
            oLoanSummary.DepartmentID = oReader.GetInt32("DepartmentID");
            oLoanSummary.DepartmentName = oReader.GetString("DepartmentName");
            oLoanSummary.LoanAmount = oReader.GetDouble("LoanAmount");
            oLoanSummary.InstallmentDeduction = oReader.GetDouble("InstallmentDeduction");
            oLoanSummary.InterestDeduction = oReader.GetDouble("InterestDeduction");
            oLoanSummary.RefundAmount = oReader.GetDouble("RefundAmount");
            oLoanSummary.RefundCharge = oReader.GetDouble("RefundCharge");
            return oLoanSummary;
        }

        public static LoanSummary CreateObject(NullHandler oReader)
        {
            LoanSummary oLoanSummary = new LoanSummary();
            LoanSummaryService oService = new LoanSummaryService();
            oLoanSummary = oService.MapObject(oReader);
            return oLoanSummary;
        }
        private List<LoanSummary> CreateObjects(IDataReader oReader)
        {
            List<LoanSummary> oLoanSummarys = new List<LoanSummary>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LoanSummary oItem = CreateObject(oHandler);
                oLoanSummarys.Add(oItem);
            }
            return oLoanSummarys;
        }

        #endregion

        #region Interface implementation
        public LoanSummaryService() { }


        public List<LoanSummary> Gets(DateTime dtFrom, DateTime dtTo, string sDeptID, int nSalaryMonth, long nUserID)
        {
            List<LoanSummary> oLoanSummarys = new List<LoanSummary>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LoanSummaryDA.Gets(tc, dtFrom, dtTo, sDeptID, nSalaryMonth, nUserID);
                oLoanSummarys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                LoanSummary oLoanSummary = new LoanSummary();
                oLoanSummary.ErrorMessage = e.Message;
                oLoanSummarys.Add(oLoanSummary);
                #endregion
            }

            return oLoanSummarys;
        }


        #endregion
    }
}