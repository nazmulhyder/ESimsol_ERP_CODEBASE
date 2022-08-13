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
    public class CertificateDA
    {
        public CertificateDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, Certificate oCertificate, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Certificate] %n,%u,%u,%s,%n,%b,%n,%n",
                   oCertificate.CertificateID,
                   oCertificate.Description,oCertificate.CertificateNo, oCertificate.RequiredFor, oCertificate.CertificateType,
                   oCertificate.IsActive, nUserID, nDBOperation);

        }

        public static IDataReader Activity(int nCertificateID, bool IsActive, TransactionContext tc)
        {

            return tc.ExecuteReader("UPDATE Certificate SET IsActive=%b WHERE CertificateID=%n;SELECT * FROM Certificate WHERE CertificateID=%n", IsActive, nCertificateID, nCertificateID);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nCertificateID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM Certificate WHERE CertificateID=%n", nCertificateID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM Certificate");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        //public static string GetBalance(string sSQL, TransactionContext tc)
        //{
        //    object x = tc.ExecuteScalar(sSQL);
        //    return x.ToString();
        //}

        #endregion
    }
}
