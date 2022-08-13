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
    public class ITaxHeadConfigurationDA
    {
        public ITaxHeadConfigurationDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ITaxHeadConfiguration oITHC, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ITaxHeadConfiguration] %n, %n, %b, %n, %n, %n, %n, %n, %n",
                                    oITHC.ITaxHeadConfigurationID, oITHC.SalaryHeadID, oITHC.IsExempted, oITHC.MaxExemptAmount, 
                                    oITHC.CalculationOn, oITHC.CalculationSalaryHeadID, oITHC.PercentOfCalculation, nUserID, nDBOperation);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nITaxHeadConfigurationID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ITaxHeadConfiguration WHERE ITaxHeadConfigurationID=%n", nITaxHeadConfigurationID);
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
