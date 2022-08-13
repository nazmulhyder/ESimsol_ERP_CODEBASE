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
    public class CircularDA
    {
        public CircularDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, Circular oCircular, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Circular] %n,%n,%n,%n,%s,%d,%d,%n,%n",
                   oCircular.CircularID, oCircular.DepartmentID,oCircular.DesignationID,
                   oCircular.NoOfPosition, oCircular.Description, oCircular.StartDate,oCircular.EndDate,
                   nUserID, nDBOperation);

        }

        public static IDataReader Activity(int nCircularID,  TransactionContext tc)
        {

            return tc.ExecuteReader("UPDATE Circular SET IsActive=~IsActive WHERE CircularID=%n;SELECT * FROM View_Circular WHERE CircularID=%n", nCircularID, nCircularID);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nCircularID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Circular WHERE CircularID=%n", nCircularID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Circular");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetNewCirculars(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
