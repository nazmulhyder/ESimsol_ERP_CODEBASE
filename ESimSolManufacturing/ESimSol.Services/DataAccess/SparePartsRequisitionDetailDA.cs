using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class SparePartsRequisitionDetailDA
    {
        public SparePartsRequisitionDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SparePartsRequisitionDetail oSparePartsRequisitionDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sSparePartsRequisitionDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SparePartsRequisitionDetail]" +
                "%n, %n, %n, %n, %n, %s, %n, %n, %s",
                oSparePartsRequisitionDetail.SparePartsRequisitionDetailID, oSparePartsRequisitionDetail.SparePartsRequisitionID, oSparePartsRequisitionDetail.ProductID,
                oSparePartsRequisitionDetail.UnitID, oSparePartsRequisitionDetail.Quantity, oSparePartsRequisitionDetail.Remarks,
                nUserID, (int)eEnumDBOperation, sSparePartsRequisitionDetailIDs);
        }

        public static void Delete(TransactionContext tc, SparePartsRequisitionDetail oSparePartsRequisitionDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sSparePartsRequisitionDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SparePartsRequisitionDetail]" +
                "%n, %n, %n, %n, %n, %s, %n, %n, %s",
                oSparePartsRequisitionDetail.SparePartsRequisitionDetailID, oSparePartsRequisitionDetail.SparePartsRequisitionID, oSparePartsRequisitionDetail.ProductID,
                oSparePartsRequisitionDetail.UnitID, oSparePartsRequisitionDetail.Quantity, oSparePartsRequisitionDetail.Remarks,
                nUserID, (int)eEnumDBOperation, sSparePartsRequisitionDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SparePartsRequisitionDetail WHERE SparePartsRequisitionDetailID=%n", nID);
        }

        public static IDataReader Gets(int nCourierServiceID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SparePartsRequisitionDetail where SparePartsRequisitionID =%n ", nCourierServiceID);
        }

        public static IDataReader GetsLog(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM SparePartsRequisitionDetailLog AS HH WHERE HH.SparePartsRequisitionLogID=%n ORDER BY SparePartsRequisitionDetailLogID ", id);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }


}

