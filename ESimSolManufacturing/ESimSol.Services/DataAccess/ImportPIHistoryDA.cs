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
    public class ImportPIHistoryDA
    {
        public ImportPIHistoryDA() { }

        #region Insert Function
      
       
        #endregion
            
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPIHistory LH WHERE LH.ImportPIHistoryID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int nImportPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPIHistory LH WHERE LH.ImportPIID=%n", nImportPIID);
        }
        
     
        #endregion
    }
}
