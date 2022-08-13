using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class MarketingAccountDA
    {
        public MarketingAccountDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, MarketingAccount oMarketingAccount, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MarketingAccount]"
                                    + "%n,  %s, %n,%n, %s, %s,%s, %s, %s,%b, %b,%b,%n,%n, %n",
                                    oMarketingAccount.MarketingAccountID, oMarketingAccount.Name, oMarketingAccount.EmployeeID, oMarketingAccount.GroupID, oMarketingAccount.Phone, oMarketingAccount.Phone2, oMarketingAccount.Email, oMarketingAccount.ShortName, oMarketingAccount.Note, oMarketingAccount.IsGroup, oMarketingAccount.Activity, oMarketingAccount.IsGroupHead, oMarketingAccount.UserID, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, MarketingAccount oMarketingAccount, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MarketingAccount]"
                                    + "%n,  %s, %n,%n, %s, %s,%s, %s, %s,%b, %b,%b,%n,%n, %n",
                                     oMarketingAccount.MarketingAccountID, oMarketingAccount.Name, oMarketingAccount.EmployeeID, oMarketingAccount.GroupID, oMarketingAccount.Phone, oMarketingAccount.Phone2, oMarketingAccount.Email, oMarketingAccount.ShortName, oMarketingAccount.Note, oMarketingAccount.IsGroup, oMarketingAccount.Activity,oMarketingAccount.IsGroupHead, oMarketingAccount.UserID,nUserId, (int)eEnumDBOperation);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MarketingAccount WHERE MarketingAccountID=%n", nID);
        }
        public static IDataReader GetByUser(TransactionContext tc, int nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_MarketingAccount WHERE  Activity =1 and UserID=%n", nUserId);
        }
       
        public static void CommitActivity(TransactionContext tc, int  id, bool Activity, Int64 nUserId)
        {
            tc.ExecuteNonQuery("Update MarketingAccount SET Activity =%b  WHERE MarketingAccountID = %n", Activity, id);
        }
        public static void GroupActivity(TransactionContext tc, string sMarketingIDs, bool Activity, Int64 nUserId)
        {
            tc.ExecuteNonQuery("Update MarketingAccount SET Activity =%b  WHERE MarketingAccountID IN ("+sMarketingIDs+")",Activity);
        }
        
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_MarketingAccount where Isnull(IsGroup,0)=0 ORDER BY GroupID, Name ASC");
        }
        public static IDataReader GetsByBU(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MarketingAccount AS TT WHERE  Activity=1 and Isnull(IsGroup,0)=0 and TT.MarketingAccountID IN (SELECT HH.MarketingAccountID FROM MarketingAccount_BU AS HH WHERE HH.BUID=%n) ORDER BY TT.GroupID, TT.Name ASC", nBUID);
        }

        public static IDataReader GetsByBUAndGroup(TransactionContext tc, int nBUID, int nDBUserID)
        {
            return tc.ExecuteReader(" SELECT * FROM View_MarketingAccount AS TT WHERE TT.Activity=1 and Isnull(IsGroup,0)=0 and TT.MarketingAccountID IN (SELECT HH.MarketingAccountID FROM MarketingAccount_BU AS HH WHERE HH.BUID=%n) and TT.MarketingAccountID  in ( Select MarketingAccount.MarketingAccountID from MarketingAccount where Activity=1 and GroupID>0 and GroupID in (Select GroupID from MarketingAccount where MarketingAccount.Activity=1 and UserID =%n)) or  ( UserID=%n and Activity=1)  ORDER BY TT.GroupID, TT.Name ASC", nBUID, nDBUserID, nDBUserID);
        }
  
        public static IDataReader GetsByName(TransactionContext tc, string sName, int nBUID)
        {
            if (string.IsNullOrEmpty(sName))
            {
                return tc.ExecuteReader("SELECT * FROM View_MarketingAccount WHERE  MarketingAccountID in (Select MarketingAccountID from MarketingAccount_BU where BUID  in (" + nBUID.ToString() + ")) AND Activity =1 and Isnull(IsGroup,0)=0 Order by GroupID,[Name]");
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_MarketingAccount WHERE Name LIKE ('%" + sName + "%')  AND MarketingAccountID in (Select MarketingAccountID from MarketingAccount_BU where BUID  in (" + nBUID.ToString() + ")) AND Activity =1 and Isnull(IsGroup,0)=0 Order by GroupID,[Name]");
            }
            
            
        }
        public static IDataReader GetsGroupHead(TransactionContext tc, string sName, int nBUID)
        {
            if (string.IsNullOrEmpty(sName))
            {
                return tc.ExecuteReader("SELECT * FROM View_MarketingAccount WHERE  Isnull(IsGroupHead,0)=1 and  MarketingAccountID in (Select MarketingAccountID from MarketingAccount_BU where BUID  in (" + nBUID.ToString() + ")) AND Activity =1 and Isnull(IsGroup,0)=0 Order by GroupID,[Name]");
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_MarketingAccount WHERE  Isnull(IsGroupHead,0)=1 and Name LIKE ('%" + sName + "%')  AND MarketingAccountID in (Select MarketingAccountID from MarketingAccount_BU where BUID  in (" + nBUID.ToString() + ")) AND Activity =1 and Isnull(IsGroup,0)=0 Order by GroupID,[Name]");
            }


        }

        public static IDataReader GetsByNameGroup(TransactionContext tc, string sName, int nBUID, int nUserId)
        {
            if (string.IsNullOrEmpty(sName))
            {
                return tc.ExecuteReader("SELECT * FROM View_MarketingAccount WHERE  MarketingAccountID in (Select MarketingAccountID from MarketingAccount_BU where BUID  in (" + nBUID.ToString() + ")) AND Activity =1 and Isnull(IsGroup,0)=0 and MarketingAccountID  in ( Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =%n)) or UserID=%n Order by GroupID, [Name]", nUserId, nUserId);
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_MarketingAccount WHERE Name LIKE ('%" + sName + "%')  AND MarketingAccountID in (Select MarketingAccountID from MarketingAccount_BU where BUID  in (" + nBUID.ToString() + ")) AND Activity =1 and Isnull(IsGroup,0)=0 and MarketingAccountID in ( Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID=" + nUserId + ")) or UserID=" + nUserId + " Order by GroupID, [Name]");
            }
            
            
        }

        public static IDataReader GetsOnlyGroupByUser(TransactionContext tc, string sName, int nBUID, int nUserId)
        {
            if (string.IsNullOrEmpty(sName))
            {
                return tc.ExecuteReader("SELECT * FROM View_MarketingAccount  where Activity=1 and IsGroup=1 and isnull(GroupID,0)<=0 and MarketingAccountID in (Select GroupID from MarketingAccount where Activity=1 AND MarketingAccountID in (Select MarketingAccountID from MarketingAccount_BU where BUID  in (" + nBUID.ToString() + ")) and UserID =%n) Order by  [Name]", nUserId, nUserId);
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_MarketingAccount where  Name LIKE ('%" + sName + "%')and IsGroup=1 and isnull(GroupID,0)<=0 and MarketingAccountID in (Select GroupID from MarketingAccount where Activity=1 AND MarketingAccountID in (Select MarketingAccountID from MarketingAccount_BU where BUID  in (" + nBUID.ToString() + ")) and UserID =%n) Order by  [Name]", nUserId, nUserId);
            }
        }
        public static IDataReader GetsOnlyGroup(TransactionContext tc, string sName, int nBUID, int nUserId)
        {
            if (string.IsNullOrEmpty(sName))
            {
                return tc.ExecuteReader("SELECT * FROM View_MarketingAccount AS TT  WHERE Activity=1 and Isnull(IsGroup,0)=1 and isnull(GroupID,0)<=0 and TT.MarketingAccountID IN (SELECT GG.GroupID FROM MarketingAccount AS GG WHERE GG.GroupID>0 and GG.MarketingAccountID in (SELECT HH.MarketingAccountID FROM MarketingAccount_BU AS HH WHERE  HH.BUID in (" + nBUID.ToString() + "))) ORDER BY  TT.Name ASC", nUserId, nUserId);
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_MarketingAccount AS TT  WHERE Isnull(IsGroup,0)=1 and isnull(GroupID,0)<=0 "  + (!string.IsNullOrEmpty(sName) ? " and Name LIKE '%" +sName + "%'" : "") +"and Activity=1 and  TT.MarketingAccountID IN (SELECT GG.GroupID FROM MarketingAccount AS GG WHERE GG.GroupID>0 and GG.MarketingAccountID in (SELECT HH.MarketingAccountID FROM MarketingAccount_BU AS HH WHERE HH.BUID in (" + nBUID.ToString() + "))) ORDER BY  TT.Name ASC", nUserId, nUserId);
            }
        }
      

        public static int GetIsMKTUser(TransactionContext tc, Int64 nUserId)
        {
            object obj = tc.ExecuteScalar("SELECT Count(*) FROM View_MarketingAccount where UserID=%n", nUserId);
            if (obj == null) return 0;
            return Convert.ToInt32(obj);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
