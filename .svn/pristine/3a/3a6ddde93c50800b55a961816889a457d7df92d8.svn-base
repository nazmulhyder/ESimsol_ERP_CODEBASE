using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ACCostCenterDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ACCostCenter oACCostCenter, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ACCostCenter]" + "%n, %s, %s, %s, %n, %n, %n, %d, %d, %b, %n, %n",
                                    oACCostCenter.ACCostCenterID, oACCostCenter.Code, oACCostCenter.Name, oACCostCenter.Description, oACCostCenter.ParentID, oACCostCenter.ReferenceTypeInt, oACCostCenter.ReferenceObjectID, oACCostCenter.ActivationDate, oACCostCenter.ExpireDate, oACCostCenter.IsActive, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ACCostCenter oACCostCenter, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ACCostCenter]" + "%n, %s, %s, %s, %n, %n, %n, %d, %d, %b, %n, %n",
                                    oACCostCenter.ACCostCenterID, oACCostCenter.Code, oACCostCenter.Name, oACCostCenter.Description, oACCostCenter.ParentID, oACCostCenter.ReferenceTypeInt, oACCostCenter.ReferenceObjectID, oACCostCenter.ActivationDate, oACCostCenter.ExpireDate, oACCostCenter.IsActive, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ACCostCenter WHERE ACCostCenterID=%n", nID);
        }
        public static IDataReader GetByRef(TransactionContext tc, EnumReferenceType eEnumReferenceType, int nReferenceObjectID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ACCostCenter AS ACC WHERE ACC.ReferenceType=%n AND ACC.ReferenceObjectID=%n", (int)eEnumReferenceType, nReferenceObjectID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ACCostCenter");
        }
        public static IDataReader Gets(TransactionContext tc, int nParentID)
        {
            if (nParentID == 0)
            {
                return tc.ExecuteReader("SELECT * FROM View_ACCostCenter WHERE ParentID=%n ORDER BY Name", nParentID);
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_ACCostCenter WHERE ParentID=%n ORDER BY Name", nParentID);
            }
        }
        public static IDataReader GetsByCodeOrName(TransactionContext tc, ACCostCenter oACCostCenter, int nBUID)
        {
            string sSQL = "";
            if (nBUID > 0)
            {
                sSQL = "ACCostCenterID IN(SELECT SubLedgerID FROM BUWiseSubLedger AS MM WHERE MM.BusinessUnitID=" + nBUID.ToString() + ") AND";
            }
            if (oACCostCenter.AccountHeadID > 0)
            {
                return tc.ExecuteReader("SELECT * FROM View_ACCostCenter WHERE " + sSQL + " ACCostCenterID!=1 AND ParentID IN (SELECT TT.ReferenceObjectID FROM AccountHeadConfigure  AS TT WHERE AccountHeadID=" + oACCostCenter.AccountHeadID.ToString() + " AND TT.ReferenceObjectType=1) AND NameCode LIKE '%" + oACCostCenter.NameCode + "%' ORDER BY Name");
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_ACCostCenter WHERE " + sSQL + " ACCostCenterID!=1 AND ParentID!=1 AND NameCode LIKE '%" + oACCostCenter.NameCode + "%' ORDER BY Name");
            }
        }
        public static IDataReader GetsByCode(TransactionContext tc, ACCostCenter oACCostCenter)
        {
            return tc.ExecuteReader("SELECT * FROM View_ACCostCenter WHERE ACCostCenterID!=1 AND NameCode LIKE '%" + oACCostCenter.NameCode + "%' ORDER BY Name");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsByConfigure(TransactionContext tc, int nAccountHeadID, string sCCName, int nBUID, Int64 nUserId)
        {
            if (nAccountHeadID > 0)
            {
                if (nBUID > 0)
                {
                    return tc.ExecuteReader("SELECT *, (SELECT SLC.IsBillRefApply FROM SubledgerRefConfig AS SLC WHERE SLC.AccountHeadID=" + nAccountHeadID.ToString() + " AND SLC.SubledgerID=HH.ACCostCenterID) AS IsBillRefApply, (SELECT SLC2.IsOrderRefApply FROM SubledgerRefConfig AS SLC2 WHERE SLC2.AccountHeadID=" + nAccountHeadID.ToString() + " AND SLC2.SubledgerID=HH.ACCostCenterID) AS IsOrderRefApply, [dbo].[GetSubLedgerBalance](HH.ACCostCenterID, " + nAccountHeadID.ToString() + ", " + nBUID.ToString() + ", " + nUserId.ToString() + " ) AS CurrentBalance FROM View_ACCostCenter AS HH WHERE HH.NameCode LIKE ('%" + sCCName + "%') AND HH.ParentID IN (SELECT AHC.ReferenceObjectID FROM AccountHeadConfigure AS AHC WHERE AHC.AccountHeadID=" + nAccountHeadID.ToString() + " AND AHC.ReferenceObjectType=1) AND ACCostCenterID IN(SELECT SubLedgerID FROM BUWiseSubLedger AS MM WHERE MM.BusinessUnitID=" + nBUID.ToString() + ") ORDER BY HH.Name ASC");
                }
                else
                {
                    return tc.ExecuteReader("SELECT *, (SELECT SLC.IsBillRefApply FROM SubledgerRefConfig AS SLC WHERE SLC.AccountHeadID=" + nAccountHeadID.ToString() + " AND SLC.SubledgerID=HH.ACCostCenterID) AS IsBillRefApply, (SELECT SLC2.IsOrderRefApply FROM SubledgerRefConfig AS SLC2 WHERE SLC2.AccountHeadID=" + nAccountHeadID.ToString() + " AND SLC2.SubledgerID=HH.ACCostCenterID) AS IsOrderRefApply, [dbo].[GetSubLedgerBalance](HH.ACCostCenterID, " + nAccountHeadID.ToString() + ", " + nBUID.ToString() + ", " + nUserId.ToString() + " ) AS CurrentBalance FROM View_ACCostCenter AS HH WHERE HH.NameCode LIKE ('%" + sCCName + "%') AND HH.ParentID IN (SELECT AHC.ReferenceObjectID FROM AccountHeadConfigure AS AHC WHERE AHC.AccountHeadID=" + nAccountHeadID.ToString() + " AND AHC.ReferenceObjectType=1) ORDER BY HH.Name  ASC");
                }
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_ACCostCenter WHERE NameCode LIKE ('%" + sCCName + "%') ORDER BY Name  ASC");
            }
        }
        #endregion
    }
}
