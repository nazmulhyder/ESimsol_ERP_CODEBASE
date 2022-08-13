using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ContactPersonnelDA
    {
        public ContactPersonnelDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ContactPersonnel oCP, EnumDBOperation eEnumDBOperation, Int64 nUserId, string BUIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ContactPersonnel]"
                                    + "%n,%n,%n,%s,%s,%s,%s,%n,%s,%b,%n,%n,%n,%n,%n,%n,%s,%b, %s",
                                    oCP.ContactPersonnelID, oCP.ContractorID, oCP.WorkingUnitID, oCP.Name, oCP.Address, oCP.Phone, oCP.Email, oCP.CPGroupID,oCP.Note,oCP.RefUpdated ,
                                 oCP.CommissionAmount, oCP.CommissionApproveAmount, oCP.PayableAmount, oCP.PaidAmount, nUserId, (int)eEnumDBOperation, oCP.Designation, oCP.IsAuthenticate, BUIDs);
        }

        public static void Delete(TransactionContext tc, ContactPersonnel oCP, EnumDBOperation eEnumDBOperation, Int64 nUserId, string BUIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ContactPersonnel]"
                                    + "%n,%n,%n,%s,%s,%s,%s,%n,%s,%b,%n,%n,%n,%n,%n,%n,%s,%b, %s",
                                    oCP.ContactPersonnelID, oCP.ContractorID, oCP.WorkingUnitID, oCP.Name, oCP.Address, oCP.Phone, oCP.Email, oCP.CPGroupID, oCP.Note, oCP.RefUpdated,
                                     oCP.CommissionAmount, oCP.CommissionApproveAmount, oCP.PayableAmount, oCP.PaidAmount, nUserId, (int)eEnumDBOperation, oCP.Designation, oCP.IsAuthenticate, BUIDs);
        }

        public static void InsertContractor(TransactionContext tc, int nContactPersonnelID, Contractor oCP, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ContactPersonMapping]"
                                    + "%n, %n,%n, %n,%n",
                                       0, nContactPersonnelID, oCP.ContractorID, nUserId, (int)eEnumDBOperation);
        }
        public static void UpdatePhoto(TransactionContext tc, ContactPersonnel oContactPersonnel, Int64 nUserID)
        {
         
            SqlParameter Photo = new SqlParameter();
            Photo.SqlDbType = SqlDbType.Image;
            Photo.ParameterName = "Photopic";
            Photo.Value = oContactPersonnel.Photo;
            
            string sSQL = SQLParser.MakeSQL("UPDATE ContactPersonnel SET Photo=%q"
                + " WHERE ContactPersonnelID=%n", "@Photopic", oContactPersonnel.ContactPersonnelID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, Photo);
        }

        public static void UpdateSignature(TransactionContext tc, ContactPersonnel oContactPersonnel, Int64 nUserID)
        {                        
            SqlParameter Signature = new SqlParameter();
            Signature.SqlDbType = SqlDbType.Image;
            Signature.ParameterName = "Signaturepic";
            Signature.Value = oContactPersonnel.Signature;

            string sSQL = SQLParser.MakeSQL("UPDATE ContactPersonnel SET [Signature]=%q"
                + " WHERE ContactPersonnelID=%n", "@Signaturepic", oContactPersonnel.ContactPersonnelID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, Signature);
        }
        #endregion

        #region Get & Exist Function
        //public static bool IsAuthenticate(TransactionContext tc, int nContactPersonnelID)
        //{
        //    object oPermission = tc.ExecuteScalar("select IsAuthenticate from ContactPersonnel where ContactPersonnelID=%n", nContactPersonnelID);
        //    if (oPermission == null || oPermission == DBNull.Value)
        //    {
        //        return false;
        //    }
        //    bool nTemp = Convert.ToBoolean(oPermission);
        //    return nTemp;
        //}
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ContactPersonnel WHERE ContactPersonnelID=%n", nID);
        }
        public static IDataReader GetWithImage(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ContactPersonnelWithImage WHERE ContactPersonnelID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ContactPersonnel");
        }
        public static IDataReader GetsByContractor(TransactionContext tc, int nContractorID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ContactPersonnel WHERE  ContactPersonnelID in (Select ContactPersonnelID from ContactPersonMapping where ContractorID=%n)", nContractorID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsOnlyCommission(TransactionContext tc, string sContractorIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_ContactPersonnel WHERE ContactPersonnelID in(select SalesCommission.ContactPersonnelID from SalesCommission where  ContractorID IN (%q)) Order By NameD", sContractorIDs);
        }

        public static IDataReader Gets(TransactionContext tc, int nContractorID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ContactPersonnel WHERE Activity=1 and ContactPersonnelID in (Select ContactPersonnelID from ContactPersonMapping where ContractorID=%n)", nContractorID);
        }
        #endregion

        public static void MergeCP(TransactionContext tc, ContactPersonnel oContractorPersonal, int nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ContactPersonnelMerge]"
                                   + "%n, %s",
                                      oContractorPersonal.ContactPersonnelID, oContractorPersonal.ContactPersonnelIDs);
        }
    }
}
