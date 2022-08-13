using ESimSol.BusinessObjects;
using ICS.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.DataAccess
{
    public class WUSubContractTermsConditionDA
    {
        public WUSubContractTermsConditionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, WUSubContractTermsCondition oWUSubContractTermsCondition, EnumDBOperation eEnumDBWUSubContractTermsCondition, Int64 nUserID, string sIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_WUSubContractTermsCondition]" + "%n, %n, %s, %n, %n, %s",
                                    oWUSubContractTermsCondition.WUSubContractTermsConditionID, oWUSubContractTermsCondition.WUSubContractID, oWUSubContractTermsCondition.TermsAndCondition, nUserID, (int)eEnumDBWUSubContractTermsCondition, sIDs);
        }

        public static void Delete(TransactionContext tc, WUSubContractTermsCondition oWUSubContractTermsCondition, EnumDBOperation eEnumDBWUSubContractTermsCondition, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_WUSubContractTermsCondition]" + "%n, %n, %s, %n, %n, %s",
                                    oWUSubContractTermsCondition.WUSubContractTermsConditionID, oWUSubContractTermsCondition.WUSubContractID, oWUSubContractTermsCondition.TermsAndCondition, nUserID, (int)eEnumDBWUSubContractTermsCondition, sIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM WUSubContractTermsCondition WHERE WUSubContractID = %n ORDER BY WUSubContractTermsConditionID", id);
        }
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion


    }
}
