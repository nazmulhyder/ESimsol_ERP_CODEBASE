using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class BusinessUnitDA
    {
        public BusinessUnitDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BusinessUnit oBusinessUnit, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BusinessUnit]"
                                    + "%n, %s, %s, %s, %s, %s, %s, %n, %n, %s, %s, %s, %s, %s, %b, %b, %b,%n,%n, %n, %u, %u, %s",
                                    oBusinessUnit.BusinessUnitID, oBusinessUnit.Code, oBusinessUnit.Name, oBusinessUnit.ShortName, oBusinessUnit.RegistrationNo, oBusinessUnit.TINNo, oBusinessUnit.VatNo, (int)oBusinessUnit.BusinessNature, (int)oBusinessUnit.LegalFormat, oBusinessUnit.Address, oBusinessUnit.Phone, oBusinessUnit.Email, oBusinessUnit.WebAddress, oBusinessUnit.Note, oBusinessUnit.IsAreaEffect, oBusinessUnit.IsZoneEffect, oBusinessUnit.IsSiteEffect, (int)oBusinessUnit.BusinessUnitType,  (int)eEnumDBOperation, nUserID, oBusinessUnit.NameInBangla, oBusinessUnit.AddressInBangla, oBusinessUnit.FaxNo);
        }

        public static void Delete(TransactionContext tc, BusinessUnit oBusinessUnit, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BusinessUnit]"
                                    + "%n, %s, %s, %s, %s, %s, %s, %n, %n, %s, %s, %s, %s, %s, %b, %b, %b,%n,%n, %n, %u, %u, %s",
                                    oBusinessUnit.BusinessUnitID, oBusinessUnit.Code, oBusinessUnit.Name, oBusinessUnit.ShortName, oBusinessUnit.RegistrationNo, oBusinessUnit.TINNo, oBusinessUnit.VatNo, (int)oBusinessUnit.BusinessNature, (int)oBusinessUnit.LegalFormat, oBusinessUnit.Address, oBusinessUnit.Phone, oBusinessUnit.Email, oBusinessUnit.WebAddress, oBusinessUnit.Note, oBusinessUnit.IsAreaEffect, oBusinessUnit.IsZoneEffect, oBusinessUnit.IsSiteEffect, (int)oBusinessUnit.BusinessUnitType, (int)eEnumDBOperation, nUserID, oBusinessUnit.NameInBangla, oBusinessUnit.AddressInBangla, oBusinessUnit.FaxNo);
        }

        public static void UpdateImage(TransactionContext tc, byte[] attachfile, int nBusinessUnitID, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = attachfile;
            string sSQL = SQLParser.MakeSQL("UPDATE BusinessUnit SET BUImage=%q"
                 + " WHERE BusinessUnitID=%n", "@file", nBusinessUnitID);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID=%n", nID);
        }
        public static IDataReader GetWithImage(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BusinessUnitWithImage WHERE BusinessUnitID=%n", nID);
        }
        public static IDataReader GetByType(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT Top(1)* FROM View_BusinessUnit WHERE BusinessUnitType=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BusinessUnit ORDER BY BusinessUnitID ASC");
        }
        public static IDataReader GetsPermittedBU(TransactionContext tc, int nUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BusinessUnit AS HH WHERE HH.BusinessUnitID IN (SELECT MM.BUID FROM BUPermission AS MM WHERE MM.UserID=%n) ORDER BY BusinessUnitID ASC", nUserID);
        }
        public static IDataReader IsPermittedBU(TransactionContext tc, int nBUID, int nUserID)
        {
            return tc.ExecuteReader("SELECT [dbo].[FN_IsPermittedBU] (%n, %n)  AS IsPermitted", nBUID, nUserID);
        }
        public static IDataReader GetsBUByCodeOrName(TransactionContext tc, string sCodeOrName)
        {
            if (sCodeOrName == "00")
            {
                return tc.ExecuteReader("SELECT * FROM View_BusinessUnit ORDER BY Code");
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_BusinessUnit WHERE NameCode LIKE '%" + sCodeOrName + "%' ORDER BY Code");
            }
        }
        public static IDataReader GetsBUByCodeOrNameAndAccountHead(TransactionContext tc, string sCodeOrName, int nAccountHeadID)
        {
            if (sCodeOrName == "00")
            {
                return tc.ExecuteReader("SELECT * FROM View_BusinessUnit AS TT WHERE TT.BusinessUnitID IN(SELECT BUWA.BusinessUnitID FROM BusinessUnitWiseAccountHead AS BUWA WHERE BUWA.AccountHeadID=%n) ORDER BY Code ", nAccountHeadID);
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_BusinessUnit AS TT WHERE TT.BusinessUnitID IN(SELECT BUWA.BusinessUnitID FROM BusinessUnitWiseAccountHead AS BUWA WHERE BUWA.AccountHeadID="+nAccountHeadID.ToString()+") AND TT.NameCode LIKE '%" + sCodeOrName + "%' ORDER BY Code");
            }
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
      

       

       
        #endregion
    }  
}
