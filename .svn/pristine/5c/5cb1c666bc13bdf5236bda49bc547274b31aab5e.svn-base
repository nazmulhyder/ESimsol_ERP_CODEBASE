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
    public class VehicleReturnChallanDA
    {
        public VehicleReturnChallanDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VehicleReturnChallan oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VehicleReturnChallan]"
                                    + "%n,%s, %D,    %n,%n,%n, %n, %s,  %n,%n",
                                    oDO.VehicleReturnChallanID, oDO.ReturnChallanNo, oDO.ReturnChallanDate,
                                    oDO.SaleInvoiceID, oDO.WorkingUnitID, oDO.LotID, oDO.Refund_Amount, oDO.Note, 
                                    nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, VehicleReturnChallan oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VehicleReturnChallan]"
                                        + "%n,%s, %D,    %n,%n,%n, %n, %s,  %n,%n",
                                    oDO.VehicleReturnChallanID, oDO.ReturnChallanNo, oDO.ReturnChallanDate,
                                    oDO.SaleInvoiceID, oDO.WorkingUnitID, oDO.LotID, oDO.Refund_Amount, oDO.Note,
                                    nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(int nID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VehicleReturnChallan WHERE VehicleReturnChallanID=%n", nID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
