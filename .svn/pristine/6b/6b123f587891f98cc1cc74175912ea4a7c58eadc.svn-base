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
    public class ITaxHeadEquationDA
    {
        public ITaxHeadEquationDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ITaxHeadEquation oITaxHeadEquation, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ITaxHeadEquation] %n,%n,%n,%n,%n,%n,%n",
                   oITaxHeadEquation.ITaxHeadEquationID, oITaxHeadEquation.ITaxHeadConfigurationID,
                   oITaxHeadEquation.CalculateOn, oITaxHeadEquation.Value,
                   oITaxHeadEquation.SalaryHeadID, nUserID, nDBOperation);


        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nITaxHeadEquationID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ITaxHeadEquation WHERE ITaxHeadEquationID=%n", nITaxHeadEquationID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ITaxHeadEquation");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
