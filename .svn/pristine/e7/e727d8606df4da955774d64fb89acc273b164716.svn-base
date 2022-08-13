using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.DataAccess
{
    public class LabdipHistoryDA
    {
        public LabdipHistoryDA() { }

        #region Insert Function
    
        #endregion

       


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LabdipHistory LH WHERE LH.LabdipHistoryID=%n", nID);
        }
        public static IDataReader Getby(TransactionContext tc, int nLabdipID, int nOrderStatus)
        {
            return tc.ExecuteReader("SELECT * FROM View_LabdipHistory WHERE LabdipID=%n AND CurrentStatus=%n", nLabdipID, nOrderStatus);
        }
        public static IDataReader Gets(TransactionContext tc, int nLabdipID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LabdipHistory LH WHERE LH.LabdipID=%n order by LH.DBServerDateTime", nLabdipID);
        }
    

     
        #endregion
    }
}
