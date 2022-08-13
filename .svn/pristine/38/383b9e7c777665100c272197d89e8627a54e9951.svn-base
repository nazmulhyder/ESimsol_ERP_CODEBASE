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
    public class EmployeeLoanAmountService : MarshalByRefObject, IEmployeeLoanAmountService
    {
        #region Private functions and declaration
        private EmployeeLoanAmount MapObject(NullHandler oReader)
        {
            EmployeeLoanAmount oEmployeeLoanAmount = new EmployeeLoanAmount();
            oEmployeeLoanAmount.ELAID = oReader.GetInt32("ELAID");
            oEmployeeLoanAmount.EmployeeLoanID = oReader.GetInt32("EmployeeLoanID");
            oEmployeeLoanAmount.LoanRequestID = oReader.GetInt32("LoanRequestID");
            oEmployeeLoanAmount.Amount = oReader.GetDouble("Amount");
            oEmployeeLoanAmount.Note = oReader.GetString("Note");
            oEmployeeLoanAmount.ApproveBy = oReader.GetInt32("ApproveBy");
            oEmployeeLoanAmount.ApproveDate = oReader.GetDateTime("ApproveDate");
            oEmployeeLoanAmount.LoanDisburseDate = oReader.GetDateTime("LoanDisburseDate");
            return oEmployeeLoanAmount;
        }

        public static EmployeeLoanAmount CreateObject(NullHandler oReader)
        {
            EmployeeLoanAmount oEmployeeLoanAmount = new EmployeeLoanAmount();
            EmployeeLoanAmountService oService = new EmployeeLoanAmountService();
            oEmployeeLoanAmount = oService.MapObject(oReader);
            return oEmployeeLoanAmount;
        }
        private List<EmployeeLoanAmount> CreateObjects(IDataReader oReader)
        {
            List<EmployeeLoanAmount> oEmployeeLoanAmounts = new List<EmployeeLoanAmount>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeLoanAmount oItem = CreateObject(oHandler);
                oEmployeeLoanAmounts.Add(oItem);
            }
            return oEmployeeLoanAmounts;
        }

        #endregion

        #region Interface implementation
        public EmployeeLoanAmountService() { }

        public EmployeeLoanAmount Get(int nEmployeeLoanAmountID, long nUserID)
        {
            EmployeeLoanAmount oEmployeeLoanAmount = new EmployeeLoanAmount();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeLoanAmountDA.Get(tc, nEmployeeLoanAmountID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeLoanAmount = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oEmployeeLoanAmount.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeLoanAmount;
        }

        public List<EmployeeLoanAmount> Gets(string sSQL, long nUserID)
        {
            List<EmployeeLoanAmount> oEmployeeLoanAmounts = new List<EmployeeLoanAmount>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLoanAmountDA.Gets(tc, sSQL);
                oEmployeeLoanAmounts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                EmployeeLoanAmount oEmployeeLoanAmount = new EmployeeLoanAmount();
                oEmployeeLoanAmount.ErrorMessage = e.Message;
                oEmployeeLoanAmounts.Add(oEmployeeLoanAmount);
                #endregion
            }

            return oEmployeeLoanAmounts;
        }


        #endregion
    }
}