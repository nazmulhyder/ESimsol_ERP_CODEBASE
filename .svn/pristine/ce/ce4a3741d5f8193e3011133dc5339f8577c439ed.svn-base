using ESimSol.BusinessObjects;
using ICS.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.DataAccess
{
    public class WUSubContractYarnConsumptionDA
    {
        public WUSubContractYarnConsumptionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, WUSubContractYarnConsumption oWUSubContractYarnConsumption, EnumDBOperation eEnumDBWUSubContractYarnConsumption, Int64 nUserID, string sIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_WUSubContractYarnConsumption]" + "%n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %s",
                                    oWUSubContractYarnConsumption.WUSubContractYarnConsumptionID, oWUSubContractYarnConsumption.WUSubContractID, oWUSubContractYarnConsumption.WarpWeftTypeInt, oWUSubContractYarnConsumption.YarnID, oWUSubContractYarnConsumption.MUnitID, oWUSubContractYarnConsumption.ConsumptionPerUnit, oWUSubContractYarnConsumption.ConsumptionQty, oWUSubContractYarnConsumption.Remarks, nUserID, (int)eEnumDBWUSubContractYarnConsumption, sIDs);
        }

        public static void Delete(TransactionContext tc, WUSubContractYarnConsumption oWUSubContractYarnConsumption, EnumDBOperation eEnumDBWUSubContractYarnConsumption, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_WUSubContractYarnConsumption]" + "%n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %s",
                                    oWUSubContractYarnConsumption.WUSubContractYarnConsumptionID, oWUSubContractYarnConsumption.WUSubContractID, oWUSubContractYarnConsumption.WarpWeftTypeInt, oWUSubContractYarnConsumption.YarnID, oWUSubContractYarnConsumption.MUnitID, oWUSubContractYarnConsumption.ConsumptionPerUnit, oWUSubContractYarnConsumption.ConsumptionQty, oWUSubContractYarnConsumption.Remarks, nUserID, (int)eEnumDBWUSubContractYarnConsumption, sIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_WUSubContractYarnConsumption WHERE WUSubContractID = %n ORDER BY WUSubContractYarnConsumptionID", id);
        }

        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion


    }
}
