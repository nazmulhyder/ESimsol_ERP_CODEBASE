using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class ChartsOfAccountDA
    {
        public ChartsOfAccountDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ChartsOfAccount oChartsOfAccount, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ChartOfAccount]"
                                    + "%n, %n, %s, %s, %n, %n, %n, %s, %b, %b, %n, %n, %n, %n, %n",
                                    oChartsOfAccount.AccountHeadID, oChartsOfAccount.DAHCID, oChartsOfAccount.AccountCode, oChartsOfAccount.AccountHeadName, (int)oChartsOfAccount.AccountType, oChartsOfAccount.CurrencyID, oChartsOfAccount.ReferenceObjectID, oChartsOfAccount.Description, oChartsOfAccount.IsJVNode, oChartsOfAccount.IsDynamic, oChartsOfAccount.ParentHeadID, oChartsOfAccount.ReferenceTypeInt, (int)oChartsOfAccount.AccountOperationType, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ChartsOfAccount oChartsOfAccount, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ChartOfAccount]"
                                    + "%n, %n, %s, %s, %n, %n, %n, %s, %b, %b, %n, %n, %n, %n, %n",
                                    oChartsOfAccount.AccountHeadID, oChartsOfAccount.DAHCID, oChartsOfAccount.AccountCode, oChartsOfAccount.AccountHeadName, (int)oChartsOfAccount.AccountType, oChartsOfAccount.CurrencyID, oChartsOfAccount.ReferenceObjectID, oChartsOfAccount.Description, oChartsOfAccount.IsJVNode, oChartsOfAccount.IsDynamic, oChartsOfAccount.ParentHeadID, oChartsOfAccount.ReferenceTypeInt, (int)oChartsOfAccount.AccountOperationType, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader Insert(TransactionContext tc, ChartsOfAccount oChartsOfAccount, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CompanyWiseAccountHeadForCopy]"
                                    + "%s, %n, %n", oChartsOfAccount.IDs, nUserID);
        }
        public static void RestChartsOfAccount(TransactionContext tc)
        {
            tc.ExecuteNonQuery("DELETE FROM ChartsOfAccount WHERE AccountHeadID>106");
        }
        public static void RestCompanyWiseAccountHead(TransactionContext tc)
        {
            tc.ExecuteNonQuery("DELETE FROM CompanyWiseAccountHead");
        }
        public static void CopyCOA(TransactionContext tc, string sAccountHeadIDs, int nCompanyID, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC[SP_Copy_COA]"
                                    + " %s,%n,%n",sAccountHeadIDs, nCompanyID,nUserID);
        }       
       
        public static void Update_DynamicHead(TransactionContext tc, ChartsOfAccount oCOA)
        {
            tc.ExecuteNonQuery("UPDATE ChartsOfAccount SET ReferenceObjectID=%n, ReferenceType=%n, IsDynamic=%b"
                            + " WHERE AccountHeadID=%n", oCOA.ReferenceObjectID, oCOA.ReferenceTypeInt, true, oCOA.AccountHeadID);
        }


        public static void Update_InventoryHead(TransactionContext tc, ChartsOfAccount oCOA)
        {
            tc.ExecuteNonQuery("UPDATE ChartsOfAccount SET InventoryHeadID=%n "+ " WHERE AccountHeadID=%n", oCOA.InventoryHeadID, oCOA.AccountHeadID);
        }  
        public static IDataReader MoveChartOfAccount(TransactionContext tc, ChartsOfAccount oChartsOfAccount, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_MoveChartOfAccounts]" + "%n, %n, %n", oChartsOfAccount.AccountHeadID, oChartsOfAccount.ParentHeadID, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChartsOfAccount WHERE AccountHeadID=%n", nID);
        }
        public static IDataReader GetByCode(TransactionContext tc, string sAccountCode)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChartsOfAccount WHERE AccountCode=%s", sAccountCode);
        }
        public static IDataReader Get(TransactionContext tc, string sAccountCode)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChartsOfAccount WHERE AccountCode=%s", sAccountCode);
        }
        public static IDataReader GetDependencies(TransactionContext tc, int nAccountHeadID)
        {
            return tc.ExecuteReader("SELECT	COA.IsCostCenterApply, COA.IsBillRefApply, COA.IsInventoryApply, COA.IsOrderReferenceApply, COA.AccountOperationType FROM View_ChartsOfAccount AS COA WHERE COA.AccountHeadID=%n", nAccountHeadID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChartsOfAccount ORDER BY AccountHeadName ASC");
        }
        public static IDataReader GetsByCodeOrName(TransactionContext tc, ChartsOfAccount oChartsOfAccount)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChartsOfAccount WHERE AccountType=5 AND AccountHeadCodeName LIKE '%" + oChartsOfAccount.AccountHeadCodeName + "%' ORDER BY AccountHeadName ASC");
        }
        public static IDataReader GetsByCodeOrNameWithBU(TransactionContext tc, ChartsOfAccount oChartsOfAccount)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChartsOfAccount WHERE AccountHeadID IN (SELECT HH.AccountHeadID FROM BusinessUnitWiseAccountHead AS HH WHERE HH.BusinessUnitID="+oChartsOfAccount.BusinessUnitID.ToString()+") AND AccountType=5 AND AccountHeadCodeName LIKE '%" + oChartsOfAccount.AccountHeadCodeName + "%' ORDER BY AccountHeadName ASC");
        }
        public static IDataReader GetsByCodeOrNameWithBUForVoucher(TransactionContext tc, ChartsOfAccount oChartsOfAccount, bool bCheckHeadDisplayConfigure, int nUserID)
        {
            if (bCheckHeadDisplayConfigure)
            {
                return tc.ExecuteReader("SELECT *, dbo.GetLedgerBalance(MM.AccountHeadID, " + oChartsOfAccount.BusinessUnitID.ToString() + "," + nUserID.ToString() + ") AS LedgerBalance FROM View_ChartsOfAccount AS MM WHERE MM.AccountHeadID IN (SELECT HH.AccountHeadID FROM BusinessUnitWiseAccountHead AS HH WHERE HH.BusinessUnitID=" + oChartsOfAccount.BusinessUnitID.ToString() + ") AND MM.AccountType=5 AND MM.AccountHeadCodeName LIKE '%" + oChartsOfAccount.AccountHeadCodeName + "%' AND MM.ParentHeadID IN(SELECT BB.SubGroupID FROM View_HeadDisplayConfigure AS BB WHERE BB.VoucherTypeID=" + oChartsOfAccount.VoucherTypeID.ToString() + " AND BB.IsDebit=" + (Convert.ToInt16(oChartsOfAccount.IsDebit)).ToString() + ") ORDER BY MM.AccountHeadName ASC");
            }
            else
            {
                return tc.ExecuteReader("SELECT *, dbo.GetLedgerBalance(MM.AccountHeadID, " + oChartsOfAccount.BusinessUnitID.ToString() + "," + nUserID.ToString() + ") AS LedgerBalance FROM View_ChartsOfAccount AS MM WHERE MM.AccountHeadID IN (SELECT HH.AccountHeadID FROM BusinessUnitWiseAccountHead AS HH WHERE HH.BusinessUnitID=" + oChartsOfAccount.BusinessUnitID.ToString() + ") AND MM.AccountType=5 AND MM.AccountHeadCodeName LIKE '%" + oChartsOfAccount.AccountHeadCodeName + "%' ORDER BY MM.AccountHeadName ASC");
            }
        }

        /// <summary>
        /// multi purpose function
        /// parameters ComponentID,AccountType,AccountHeadCodeName. none compulsory
        /// WILL RETURN DATA ACCORDING TO PROVIDED PARAMETERS.
        /// </summary>
        /// <param name="tc"></param>
        /// <param name="oChartsOfAccount"></param>
        /// <returns></returns>
        public static IDataReader GetsByComponentAndCodeName(TransactionContext tc, ChartsOfAccount oChartsOfAccount)
        {
            if (oChartsOfAccount.AccountType != EnumAccountType.None)
            {
                if (oChartsOfAccount.ComponentID > 0)
                {
                    return tc.ExecuteReader("SELECT * FROM View_ChartsOfAccount AS COA WHERE COA.AccountType=" + (int)oChartsOfAccount.AccountType + " AND COA.ComponentID=" + oChartsOfAccount.ComponentID + " AND COA.AccountHeadCodeName LIKE '%" + oChartsOfAccount.AccountHeadCodeName + "%' AND COA.ParentHeadID!=0 ORDER BY COA.AccountHeadName");
                }
                else
                {
                    return tc.ExecuteReader("SELECT * FROM View_ChartsOfAccount AS COA WHERE COA.AccountType=" + (int)oChartsOfAccount.AccountType + " AND COA.AccountHeadCodeName LIKE '%" + oChartsOfAccount.AccountHeadCodeName + "%' AND COA.ParentHeadID!=0 ORDER BY COA.AccountHeadName");
                }
            }
            else
            {
                if (oChartsOfAccount.ComponentID > 0)
                {
                    return tc.ExecuteReader("SELECT * FROM View_ChartsOfAccount AS COA WHERE COA.ComponentID=" + oChartsOfAccount.ComponentID + " AND COA.AccountHeadCodeName LIKE '%" + oChartsOfAccount.AccountHeadCodeName + "%' AND COA.ParentHeadID!=0 ORDER BY COA.AccountHeadName");
                }
                else
                {
                    return tc.ExecuteReader("SELECT * FROM View_ChartsOfAccount AS COA WHERE COA.AccountHeadCodeName LIKE '%" + oChartsOfAccount.AccountHeadCodeName + "%' AND COA.ParentHeadID!=0 ORDER BY COA.AccountHeadName");
                }
            }
        }
        public static IDataReader GetsByParent(TransactionContext tc, int nParentID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChartsOfAccount WHERE ParentHeadID=%n ORDER BY AccountHeadID", nParentID);
        }
        public static IDataReader GetsForCOATemplate(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChartsOfAccount WHERE AccountHeadID<=106 ORDER BY AccountHeadName");
        }
        public static IDataReader GetAccountHeads(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChartsOfAccount");
        }
        public static IDataReader GetsbyAccountsName(TransactionContext tc, string sAccountsName)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChartsOfAccount WHERE AccountType=5 AND AccountHeadName LIKE ('%" + sAccountsName + "%')  ORDER BY AccountHeadName");
        }
        public static IDataReader GetsbyAccountTypeOrName(TransactionContext tc, int nAccountType, string sAccountsName)
        {

            string sSQl = "";
            if (sAccountsName.Length>0)
            {
                sSQl = "SELECT * FROM View_ChartsOfAccount WHERE AccountType=" + nAccountType.ToString() + " AND AccountHeadName LIKE ('%" + sAccountsName + "%')  ORDER BY AccountHeadName";

            }
            else
            {
                sSQl = "SELECT * FROM View_ChartsOfAccount WHERE AccountType=" + nAccountType.ToString() + "  ORDER BY AccountHeadName";
            }


            return tc.ExecuteReader(sSQl);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader AccountHeadGets(TransactionContext tc, int nVoucherTypeID, bool bIsDebit)
        {
            return tc.ExecuteReader("EXEC [SP_GetAccountHead]" + "%n, %b, %n", nVoucherTypeID, bIsDebit);
        }

        public static IDataReader GetRefresh(TransactionContext tc, int nParentHeadID, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_U_DynamicHeadGetRefresh]" + "%n,%n", nParentHeadID, nUserID);
        }
        public static int GetExistsAccount(TransactionContext tc, int nReferenceObjectID, int nReferenceObjectType)
        {
            object obj = tc.ExecuteScalar("SELECT COA.AccountHeadID FROM ChartsOfAccount AS COA WHERE COA.ReferenceType=%n AND COA.ReferenceObjectID=%n", nReferenceObjectType, nReferenceObjectID);
            if (obj == null) return 0;
            return Convert.ToInt32(obj);
        }

        public static string GetAccountCode(TransactionContext tc, int nParentID)
        {
            object obj = tc.ExecuteScalar("SELECT dbo.FN_ChartofAccountCode(%n) AS AccountCode", nParentID);
            if (obj == null) return "";
            return Convert.ToString(obj);
        }
        #endregion
    }
}
