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
    public class SalarySchemeDetailDA
    {
        public SalarySchemeDetailDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, SalarySchemeDetail oSalarySchemeDetail, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SalarySchemeDetail] %n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n",
                   oSalarySchemeDetail.SalarySchemeDetailID, oSalarySchemeDetail.SalarySchemeID,
                   oSalarySchemeDetail.SalaryHeadID, oSalarySchemeDetail.ConditionInt, 
                   oSalarySchemeDetail.PeriodInt, oSalarySchemeDetail.Times,
                   oSalarySchemeDetail.DeferredDay, oSalarySchemeDetail.ActivationAfterInt,
                    oSalarySchemeDetail.SalaryTypeInt, nUserID, nDBOperation);
            
        }

       
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nSalarySchemeDetailID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalarySchemeDetail WHERE SalarySchemeDetailID=%n", nSalarySchemeDetailID);
        }
        public static IDataReader Gets(TransactionContext tc, int nSID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalarySchemeDetail WHERE SalarySchemeID=%n", nSID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
