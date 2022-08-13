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
    public class TrainingDevelopmentDA
    {
        public TrainingDevelopmentDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, TrainingDevelopment oTrainingDevelopment, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TrainingDevelopment] %n,%n,%u,%u,%u,%u,%u,%u,%u,%u,%d,%d,%d,%d,%u,%n,%n",
                   oTrainingDevelopment.TDID, oTrainingDevelopment.EmployeeID,
                   oTrainingDevelopment.CourseName, oTrainingDevelopment.Specification,
                   oTrainingDevelopment.Institute, oTrainingDevelopment.Vendor,
                   oTrainingDevelopment.Country, oTrainingDevelopment.State,
                   oTrainingDevelopment.Address, oTrainingDevelopment.Duration,
                   oTrainingDevelopment.StartDate, oTrainingDevelopment.EndDate,
                   oTrainingDevelopment.EffectFromDate, oTrainingDevelopment.EffectToDate,
                   oTrainingDevelopment.Note, 
                   nUserID, nDBOperation);

        }

        public static IDataReader Activity(int nEmpID, int nTDID, bool IsActive, TransactionContext tc)
        {
            string sSql = "";
            if (IsActive == false)
            {
                sSql = "BEGIN TRAN IF EXISTS(SELECT EmployeeID FROM View_TrainingDevelopment WHERE IsActive=1 AND EmployeeID =" + nEmpID + " AND TDID !=" + nTDID + ")BEGIN ROLLBACK RAISERROR (N'One Active Item is Exist!',16,1)RETURN END;UPDATE TrainingDevelopment SET IsActive=1 WHERE TDID=" + nTDID + ";SELECT * FROM View_TrainingDevelopment WHERE TDID=" + nTDID + " ";
            }
            else
            {
                sSql = "UPDATE View_TrainingDevelopment SET IsActive=0 WHERE TDID=" + nTDID + ";SELECT * FROM View_TrainingDevelopment WHERE TDID=" + nTDID + "";
            }
            return tc.ExecuteReader(sSql);

        }

      
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nTDID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TrainingDevelopment WHERE TDID=%n", nTDID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TrainingDevelopment");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

     
        #endregion
    }
}
