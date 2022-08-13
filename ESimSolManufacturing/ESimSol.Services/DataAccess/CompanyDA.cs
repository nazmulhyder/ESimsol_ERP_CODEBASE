using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class CompanyDA
    {
        public CompanyDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Company oCompany, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Company]"
                                    + "%n, %s, %s, %s, %s, %s, %s, %s, %q, %n,%b, %n, %n, %q, %n,%b, %s, %s,%u,%q,%b,%u,%u,%n, %n, %s",
                                    oCompany.CompanyID, oCompany.Name, oCompany.Address, oCompany.FactoryAddress, oCompany.Phone, oCompany.Email, oCompany.Note, oCompany.WebAddress, oCompany.OrganizationLogo, oCompany.ImageSize, oCompany.IsRemoveLogo, oCompany.BaseCurrencyID, oCompany.ParentID, oCompany.OrganizationTitle, oCompany.TitleImageSize, oCompany.IsRemoveTitle,
                                    oCompany.VatRegNo,
                                    oCompany.PostalCode,
                                    oCompany.Country,
                                    oCompany.AuthorizedSignature,
                                    oCompany.IsRemoveSignature,
                                    oCompany.NameInBangla,
                                    oCompany.AddressInBangla,
                                    nUserId, (int)eEnumDBOperation, oCompany.FaxNo);
        }

        public static void Delete(TransactionContext tc, Company oCompany, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Company]"
                                    + "%n, %s, %s, %s, %s, %s, %s, %s, %q, %n,%b, %n, %n, %q, %n,%b, %s, %s,%u,%q,%b,%s,%s,%n, %n, %s",
                                    oCompany.CompanyID, oCompany.Name, oCompany.Address, oCompany.FactoryAddress, oCompany.Phone, oCompany.Email, oCompany.Note, oCompany.WebAddress, oCompany.OrganizationLogo, oCompany.ImageSize, oCompany.IsRemoveLogo, oCompany.BaseCurrencyID, oCompany.ParentID, oCompany.OrganizationTitle, oCompany.TitleImageSize, oCompany.IsRemoveTitle,
                                    oCompany.VatRegNo,
                                    oCompany.PostalCode,
                                    oCompany.Country,
                                    oCompany.AuthorizedSignature,
                                    oCompany.IsRemoveSignature,
                                    oCompany.NameInBangla,
                                    oCompany.AddressInBangla,
                                    nUserId, (int)eEnumDBOperation, oCompany.FaxNo);
        }

        public static void UpdatePhoto(TransactionContext tc, Company oCompany, Int64 nUserID)
        {

            SqlParameter Photo = new SqlParameter();
            Photo.SqlDbType = SqlDbType.Image;
            Photo.ParameterName = "Photopic";
            Photo.Value = oCompany.OrganizationLogo;

            string sSQL = SQLParser.MakeSQL("UPDATE Company SET OrganizationLogo=%q,ImageSize=%n"
                + " WHERE CompanyID=%n", "@Photopic", oCompany.OrganizationLogo.Length, oCompany.CompanyID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, Photo);
        }

        public static void UpdatePhotoCompanyTitle(TransactionContext tc, Company oCompany, Int64 nUserID)
        {

            SqlParameter Photo = new SqlParameter();
            Photo.SqlDbType = SqlDbType.Image;
            Photo.ParameterName = "Photopic";
            Photo.Value = oCompany.OrganizationTitle;

            string sSQL = SQLParser.MakeSQL("UPDATE Company SET OrganizationTitle=%q,TitleImageSize=%n"
                + " WHERE CompanyID=%n", "@Photopic", oCompany.OrganizationTitle.Length, oCompany.CompanyID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, Photo);
        }

        public static void UpdateAuthorizedSignature(TransactionContext tc, Company oCompany, Int64 nUserID)
        {
            SqlParameter Photo = new SqlParameter();
            Photo.SqlDbType = SqlDbType.Image;
            Photo.ParameterName = "Photopic";
            Photo.Value = oCompany.AuthorizedSignature;

            string sSQL = SQLParser.MakeSQL("UPDATE Company SET AuthorizedSignature=%q"
                + " WHERE CompanyID=%n", "@Photopic", oCompany.CompanyID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, Photo);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CompanyWithLogo WHERE CompanyID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Company ORDER BY Name ASC");
        }        
        public static IDataReader GetCompanyLogo(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CompanyWithLogo WHERE CompanyID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
