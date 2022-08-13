using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class ITaxAdvancePaymentDA
    {
        public ITaxAdvancePaymentDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ITaxAdvancePayment oITaxAdvancePayment, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ITaxAdvancePayment] %n,%n,%n,%n,%s,%n,%n",
                   oITaxAdvancePayment.ITaxAdvancePaymentID, oITaxAdvancePayment.ITaxAssessmentYearID,
                   oITaxAdvancePayment.EmployeeID, oITaxAdvancePayment.Amount,
                   oITaxAdvancePayment.Note, nUserID, nDBOperation);


        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nITaxAdvancePaymentID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ITaxAdvancePayment WHERE ITaxAdvancePaymentID=%n", nITaxAdvancePaymentID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ITaxAdvancePayment");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
