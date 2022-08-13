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
    public class AddressConfigDA
    {
        public AddressConfigDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, AddressConfig oAddressConfig, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AddressConfig]" + "%n, %s, %s, %u, %n,%n, %s, %n,%n"
                , oAddressConfig.AddressConfigID
                , oAddressConfig.Code
                , oAddressConfig.NameInEnglish
                , oAddressConfig.NameInBangla
                , oAddressConfig.AddressTypeInInt
                , oAddressConfig.ParentAddressID
                , oAddressConfig.Remarks
                , nUserID
                , nDBOperation
            );
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}

