using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class KnitDyeingYarnConsumptionDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, KnitDyeingYarnConsumption oKnitDyeingYarnConsumption, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_KnitDyeingYarnConsumption]"
                                   + "%n, %n, %n, %n,%n,%n,%s,%n,%n,%s",
                                   oKnitDyeingYarnConsumption.KnitDyeingYarnConsumptionID, oKnitDyeingYarnConsumption.KnitDyeingProgramDetailID, oKnitDyeingYarnConsumption.YarnID, oKnitDyeingYarnConsumption.UsagesParcent, oKnitDyeingYarnConsumption.FinishReqQty, oKnitDyeingYarnConsumption.MUnitID,oKnitDyeingYarnConsumption.Remarks,nUserID, (int)eEnumDBOperation, sIDs);
        }

        public static void Delete(TransactionContext tc, KnitDyeingYarnConsumption oKnitDyeingYarnConsumption, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_KnitDyeingYarnConsumption]"
                                    + "%n, %n, %n, %n,%n,%n,%s,%n,%n,%s",
                                    oKnitDyeingYarnConsumption.KnitDyeingYarnConsumptionID, oKnitDyeingYarnConsumption.KnitDyeingProgramDetailID, oKnitDyeingYarnConsumption.YarnID, oKnitDyeingYarnConsumption.UsagesParcent, oKnitDyeingYarnConsumption.FinishReqQty, oKnitDyeingYarnConsumption.MUnitID, oKnitDyeingYarnConsumption.Remarks, nUserID, (int)eEnumDBOperation, sIDs);
        }
        public static void CommitGrace(TransactionContext tc, KnitDyeingYarnConsumption oKnitDyeingYarnConsumption)
        {
            tc.ExecuteNonQuery("UPDATE KnitDyeingYarnConsumption SET GracePercent = %n, ReqQty = %n WHERE KnitDyeingYarnConsumptionID = %n", oKnitDyeingYarnConsumption.GracePercent, oKnitDyeingYarnConsumption.ReqQty, oKnitDyeingYarnConsumption.KnitDyeingYarnConsumptionID);
        }
        #endregion

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnitDyeingYarnConsumption WHERE KnitDyeingProgramDetailID = %n Order By KnitDyeingYarnConsumptionID", id);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
    }
}
