using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
   public class LabdipColorDA
    {
       public LabdipColorDA() { }

        #region Insert Update Delete Function
       public static IDataReader IUD(TransactionContext tc, LabdipColor oLapdipColor, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabdipColor] %n , %s,%s,%s,%s ,%n ,%n",
                                               oLapdipColor.LabdipColorID, oLapdipColor.Code, oLapdipColor.CodeNo, oLapdipColor.Name, oLapdipColor.Note,nUserID, nDBOperation);

        }
        #endregion

        #region Get & Exist Function
       public static IDataReader Get(TransactionContext tc, int nLapdipColorID)
        {
            return tc.ExecuteReader("Select * from LabdipColor where LabdipColorID=%n",nLapdipColorID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
