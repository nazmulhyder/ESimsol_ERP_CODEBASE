using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class VehicleChallanDA
    {
        public VehicleChallanDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VehicleChallan oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VehicleChallan]"
                                    + "%n,%s, %D,    %n,%n,%n, %s, %s,  %n,%n",
                                    oDO.VehicleChallanID, oDO.ChallanNo, oDO.ChallanDate,
                                    oDO.SaleInvoiceID, oDO.WorkingUnitID, oDO.LotID, oDO.GatePassNo, oDO.Note, 
                                    nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, VehicleChallan oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VehicleChallan]"
                                        + "%n,%s, %D,    %n,%n,%n, %s, %s,  %n,%n",
                                    oDO.VehicleChallanID, oDO.ChallanNo, oDO.ChallanDate,
                                    oDO.SaleInvoiceID, oDO.WorkingUnitID, oDO.LotID, oDO.GatePassNo, oDO.Note,
                                    nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(int nID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VehicleChallan WHERE VehicleChallanID=%n", nID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
