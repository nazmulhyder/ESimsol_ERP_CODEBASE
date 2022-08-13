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


    public class PartsRequisitionDetailDA
    {
        public PartsRequisitionDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PartsRequisitionDetail oPartsRequisitionDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sPartsRequisitionDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PartsRequisitionDetail]" + 
                "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s",
                oPartsRequisitionDetail.PartsRequisitionDetailID, oPartsRequisitionDetail.PartsRequisitionID, oPartsRequisitionDetail.ProductID, oPartsRequisitionDetail.LotID, 
                oPartsRequisitionDetail.UnitID, oPartsRequisitionDetail.Quantity, oPartsRequisitionDetail.UnitPrice, oPartsRequisitionDetail.WorkingUnitID, 
                oPartsRequisitionDetail.ChargeType, nUserID, (int)eEnumDBOperation, sPartsRequisitionDetailIDs);
        }


        public static void Delete(TransactionContext tc, PartsRequisitionDetail oPartsRequisitionDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sPartsRequisitionDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PartsRequisitionDetail]" + 
                "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s",
                oPartsRequisitionDetail.PartsRequisitionDetailID, oPartsRequisitionDetail.PartsRequisitionID, oPartsRequisitionDetail.ProductID, oPartsRequisitionDetail.LotID, 
                oPartsRequisitionDetail.UnitID, oPartsRequisitionDetail.Quantity, oPartsRequisitionDetail.UnitPrice,oPartsRequisitionDetail.WorkingUnitID, 
                oPartsRequisitionDetail.ChargeType, nUserID, (int)eEnumDBOperation, sPartsRequisitionDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PartsRequisitionDetail WHERE PartsRequisitionDetailID=%n", nID);
        }

        public static IDataReader Gets(int nCourierServiceID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PartsRequisitionDetail where PartsRequisitionID =%n ", nCourierServiceID);
        }

        public static IDataReader GetsLog(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM PartsRequisitionDetailLog AS HH WHERE HH.PartsRequisitionLogID=%n ORDER BY PartsRequisitionDetailLogID ", id);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
 
   
}
