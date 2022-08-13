using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class SubcontractDA
    {
        public SubcontractDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Subcontract oSubcontract, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Subcontract]" + "%n, %s, %n, %n, %n, %n, %d, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n",
                                    oSubcontract.SubcontractID, oSubcontract.SubcontractNo, oSubcontract.ContractStatusInt, oSubcontract.IssueBUID, oSubcontract.ContractBUID, oSubcontract.PTU2ID, oSubcontract.IssueDate, oSubcontract.ExportSCID, oSubcontract.ExportSCDetailID, oSubcontract.ProductID, oSubcontract.ColorID, oSubcontract.MoldRefID, oSubcontract.UintID, oSubcontract.Qty, oSubcontract.RateUnit, oSubcontract.UnitPrice, oSubcontract.CurrencyID, oSubcontract.CRate, oSubcontract.ApprovedBy,  oSubcontract.Remarks, (int)eEnumDBOperation, nUserID);
        }
        public static void Delete(TransactionContext tc, Subcontract oSubcontract, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Subcontract]" + "%n, %s, %n, %n, %n, %n, %d, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n",
                                    oSubcontract.SubcontractID, oSubcontract.SubcontractNo, oSubcontract.ContractStatusInt, oSubcontract.IssueBUID, oSubcontract.ContractBUID, oSubcontract.PTU2ID, oSubcontract.IssueDate, oSubcontract.ExportSCID, oSubcontract.ExportSCDetailID, oSubcontract.ProductID, oSubcontract.ColorID, oSubcontract.MoldRefID, oSubcontract.UintID, oSubcontract.Qty, oSubcontract.RateUnit, oSubcontract.UnitPrice, oSubcontract.CurrencyID, oSubcontract.CRate, oSubcontract.ApprovedBy, oSubcontract.Remarks, (int)eEnumDBOperation, nUserID);
        }
        public static void Approved(TransactionContext tc, Subcontract oSubcontract, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_SubContractApprove]" + "%n, %n", oSubcontract.SubcontractID, nUserID); 
        }

        public static IDataReader SendToProduction(TransactionContext tc, Subcontract oSubcontract, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Subcontract_SendToProduction]" + "%n, %n", oSubcontract.SubcontractID, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Subcontract WHERE SubcontractID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Subcontract");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}
