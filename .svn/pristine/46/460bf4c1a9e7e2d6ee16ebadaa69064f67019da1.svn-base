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
    public class GratuitySchemeDetailDA
    {
        public GratuitySchemeDetailDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, GratuitySchemeDetail oGratuitySchemeDetail, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_GratuitySchemeDetail] %n,%n,%n,%n,%n,%n,%n,%n,%n,%n",
                   oGratuitySchemeDetail.GSDID, oGratuitySchemeDetail.GSID, oGratuitySchemeDetail.MaturityYearStart,
                   oGratuitySchemeDetail.MaturityYearEnd, oGratuitySchemeDetail.ActivationAfter, 
                   oGratuitySchemeDetail.ValueInPercent, oGratuitySchemeDetail.GratuityApplyOn,
                   oGratuitySchemeDetail.NoOfMonthCountOneYear, nUserID, nDBOperation);


        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nGSDID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM GratuitySchemeDetail WHERE GSDID=%n", nGSDID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM GratuitySchemeDetail");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
