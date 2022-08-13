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
    public class EmployeeTrainingDA
    {
        public EmployeeTrainingDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EmployeeTraining oET, int nEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeTraining]"
                                    + "%n, %n,%n, %u, %u,%u, %d,%n, %d,%n, %u,%s, %d,%n, %u, %u, %u, %u, %n, %n",
                                    oET.EmployeeTrainingID, oET.EmployeeID,oET.Sequence, oET.CourseName, oET.Specification,oET.CertificateRegNo, oET.StartDate,oET.StartDateTrainingFormatType, oET.EndDate,oET.EndDateTrainingFormatType, oET.Duration,oET.TrainingDuration, oET.PassingDate,oET.PassingDateTrainingFormatType, oET.Result, oET.Institution, oET.CertifyBodyVendor, oET.Country, nUserID, nEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)//EmployeeTrainingID
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeTraining WHERE EmployeeTrainingID=%n ORDER BY Sequence ASC", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int nEmployeeID)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeTraining WHERE EmployeeID=%n ORDER BY Sequence ASC", nEmployeeID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
