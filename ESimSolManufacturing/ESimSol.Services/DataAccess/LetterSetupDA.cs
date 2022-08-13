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
    public class LetterSetupDA
    {
        public LetterSetupDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, LetterSetup oLetterSetup, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LetterSetup]" + "%n, %u, %u, %n,   %n, %u, %u, %b, %n, %n, %b, %s, %n, %n, %n, %n"
                , oLetterSetup.LSID
                , oLetterSetup.Name
                , oLetterSetup.HeaderName
                , oLetterSetup.HeaderFontSize
                , oLetterSetup.HeaderTextAlign
                , oLetterSetup.Body
                , oLetterSetup.Remark
                , oLetterSetup.IsEnglish
                , nUserID
                , nDBOperation
                , oLetterSetup.IsPadFormat
                , oLetterSetup.PageSize
                , oLetterSetup.MarginTop
                , oLetterSetup.MarginBottom
                , oLetterSetup.MarginLeft
                , oLetterSetup.MarginRight
            );
        }
        public static IDataReader Delete(TransactionContext tc, LetterSetup oLetterSetup, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LetterSetup]" + "%n, %u, %u, %n,%n, %u, %u, %b, %n, %n, %b, %s, %n, %n, %n, %n"
                , oLetterSetup.LSID
                , oLetterSetup.Name
                , oLetterSetup.HeaderName
                , oLetterSetup.HeaderFontSize
                , oLetterSetup.HeaderTextAlign
                , oLetterSetup.Body
                , oLetterSetup.Remark
                , oLetterSetup.IsEnglish
                , nUserID
                , nDBOperation
                , oLetterSetup.IsPadFormat
                , oLetterSetup.PageSize
                , oLetterSetup.MarginTop
                , oLetterSetup.MarginBottom
                , oLetterSetup.MarginLeft
                , oLetterSetup.MarginRight
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

        public static IDataReader Get(TransactionContext tc, int nLSID)
        {
            return tc.ExecuteReader("SELECT * FROM LetterSetup WHERE LSID=%n", nLSID);
        }

        #endregion
    }
}


