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
    public class HRResponsibilityDA
    {
        public HRResponsibilityDA()
        { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, HRResponsibility oHRResponsibility, EnumDBOperation eEnumDBHRResponsibility, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_HRResponsibility]"
                                    + "%n, %s, %s, %n, %n, %u",
                                    oHRResponsibility.HRRID, oHRResponsibility.Code, oHRResponsibility.Description, nUserId, (int)eEnumDBHRResponsibility, oHRResponsibility.DescriptionInBangla);
        }

        public static void Delete(TransactionContext tc, HRResponsibility oHRResponsibility, EnumDBOperation eEnumDBHRResponsibility, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_HRResponsibility]"
                                    + "%n, %s, %s, %n, %n, %u",
                                    oHRResponsibility.HRRID, oHRResponsibility.Code, oHRResponsibility.Description, nUserId, (int)eEnumDBHRResponsibility, oHRResponsibility.DescriptionInBangla);        
        }



        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM HRResponsibility WHERE HRRID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM HRResponsibility");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
 
}
