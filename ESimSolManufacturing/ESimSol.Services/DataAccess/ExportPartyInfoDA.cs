using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.DataAccess
{
    public class ExportPartyInfoDA
    {
        public ExportPartyInfoDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportPartyInfo oEBill, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPartyInfo]"
                                    + "%n, %s, %n, %n",
                                    oEBill.ExportPartyInfoID, oEBill.Name, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc,  ExportPartyInfo oEBill, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportPartyInfo]" + "%n, %s, %n, %n",
                                 oEBill.ExportPartyInfoID, oEBill.Name, nUserID, (int)eEnumDBOperation);
        }

        #endregion


        #region Generation Function
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM [ExportPartyInfo] as ExportPartyInfo  WHERE ExportPartyInfoID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM [ExportPartyInfo] as ExportPartyInfo");
        }
        public static IDataReader Gets(TransactionContext tc, int nContractorID)
        {
            return tc.ExecuteReader("SELECT * FROM [ExportPartyInfo] where [ExportPartyInfo].ExportPartyInfoID in (select ExportPartyInfoID from [dbo].[ExportPartyInfoDetail] where ContractorID=%n)", nContractorID);
        }
     

        #endregion

    }
}
