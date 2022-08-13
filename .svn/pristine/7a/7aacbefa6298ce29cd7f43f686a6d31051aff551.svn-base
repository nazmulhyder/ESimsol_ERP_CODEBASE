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
    public class SalarySchemeGradeDA
    {
        public SalarySchemeGradeDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, SalarySchemeGrade oSSG, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SalarySchemeGrade] %n, %s, %n, %s, %n, %n, %b, %b, %n, %n, %n",
                                   oSSG.SSGradeID, oSSG.Name, oSSG.ParentID, oSSG.Note, oSSG.MinAmount, oSSG.MaxAmount, oSSG.IsLastLayer, oSSG.IsActive, oSSG.SalarySchemeID, nUserID, nDBOperation);
      
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nSSGradeID)
        {
            return tc.ExecuteReader("SELECT * FROM SalarySchemeGrade WHERE SSGradeID=%n", nSSGradeID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
