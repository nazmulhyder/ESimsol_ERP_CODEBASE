﻿using System;
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


    public class ConsumptionRequisitionDetailDA
    {
        public ConsumptionRequisitionDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ConsumptionRequisitionDetail oConsumptionRequisitionDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sConsumptionRequisitionDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ConsumptionRequisitionDetail]" + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %s", 
                                    oConsumptionRequisitionDetail.ConsumptionRequisitionDetailID, oConsumptionRequisitionDetail.ConsumptionRequisitionID, oConsumptionRequisitionDetail.ProductID,  oConsumptionRequisitionDetail.LotID,  oConsumptionRequisitionDetail.UnitID, oConsumptionRequisitionDetail.Quantity, oConsumptionRequisitionDetail.UnitPrice, nUserID, (int)eEnumDBOperation, sConsumptionRequisitionDetailIDs);
        }

        public static void Delete(TransactionContext tc, ConsumptionRequisitionDetail oConsumptionRequisitionDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sConsumptionRequisitionDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ConsumptionRequisitionDetail]" + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %s",
                                    oConsumptionRequisitionDetail.ConsumptionRequisitionDetailID, oConsumptionRequisitionDetail.ConsumptionRequisitionID, oConsumptionRequisitionDetail.ProductID, oConsumptionRequisitionDetail.LotID, oConsumptionRequisitionDetail.UnitID, oConsumptionRequisitionDetail.Quantity, oConsumptionRequisitionDetail.UnitPrice, nUserID, (int)eEnumDBOperation, sConsumptionRequisitionDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ConsumptionRequisitionDetail WHERE ConsumptionRequisitionDetailID=%n", nID);
        }

        public static IDataReader Gets(int nCourierServiceID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ConsumptionRequisitionDetail where ConsumptionRequisitionID =%n ", nCourierServiceID);
        }

        public static IDataReader GetsLog(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ConsumptionRequisitionDetailLog AS HH WHERE HH.ConsumptionRequisitionLogID=%n ORDER BY ConsumptionRequisitionDetailLogID ", id);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
 
   
}
