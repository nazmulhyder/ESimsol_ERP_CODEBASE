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
    public class KnitDyeingYarnBookingDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, KnitDyeingYarnBooking oKnitDyeingYarnBooking, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_KnitDyeingYarnBooking]"
                                   + "%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                   oKnitDyeingYarnBooking.KnitDyeingYarnBookingID, oKnitDyeingYarnBooking.KnitDyeingPTUID,oKnitDyeingYarnBooking.CompositionID,oKnitDyeingYarnBooking.LotID,oKnitDyeingYarnBooking.MUnitID,oKnitDyeingYarnBooking.BookingQty,oKnitDyeingYarnBooking.Remarks, nUserID, (int)eEnumDBOperation,oKnitDyeingYarnBooking.IDs);
        }

        public static void Delete(TransactionContext tc, KnitDyeingYarnBooking oKnitDyeingYarnBooking, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_KnitDyeingYarnBooking]"
                               + "%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                   oKnitDyeingYarnBooking.KnitDyeingYarnBookingID, oKnitDyeingYarnBooking.KnitDyeingPTUID, oKnitDyeingYarnBooking.CompositionID, oKnitDyeingYarnBooking.LotID, oKnitDyeingYarnBooking.MUnitID, oKnitDyeingYarnBooking.BookingQty, oKnitDyeingYarnBooking.Remarks, nUserID, (int)eEnumDBOperation,oKnitDyeingYarnBooking.IDs);
        }
        #endregion
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        } 
    }
}
