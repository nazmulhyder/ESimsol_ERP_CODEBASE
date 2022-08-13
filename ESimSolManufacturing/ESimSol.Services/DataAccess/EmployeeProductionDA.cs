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
    public class EmployeeProductionDA
    {
        public EmployeeProductionDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeProduction oEmployeeProduction, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeProduction] %n,%n,%n,%n,%n,%s,%n,%n,%d,%n,%s,%b,%n,%s,%n,%n,%n",
                   oEmployeeProduction.EPSID, oEmployeeProduction.EmployeeID,
                   oEmployeeProduction.OrderRecapDetailID, (int)oEmployeeProduction.ProductionProcess,
                   (int)oEmployeeProduction.GPID, oEmployeeProduction.MachineNo,
                   oEmployeeProduction.TSPID, oEmployeeProduction.IssueQty,
                   oEmployeeProduction.IssueDate, oEmployeeProduction.YarnRcvQty,
                   oEmployeeProduction.EPSLotNo, oEmployeeProduction.IsActive,
                   oEmployeeProduction.ReferenceEPSID, oEmployeeProduction.SLNO, oEmployeeProduction.DepartmentID,
                   nUserID, nDBOperation);

        }

        public static IDataReader TransferEmployeeProduction(TransactionContext tc, EmployeeProduction oEmployeeProduction)
        {
            return tc.ExecuteReader("EXEC [SP_Process_TransferEmployeeProduction] %n,%n,%s",
                   oEmployeeProduction.EPSID, oEmployeeProduction.EmployeeID, oEmployeeProduction.MachineNo

                   );

        }

        public static IDataReader Activity(int nEmployeeProductionID, bool IsActive, TransactionContext tc)
        {

            return tc.ExecuteReader("UPDATE EmployeeProduction SET IsActive=%b WHERE EPSID=%n;SELECT * FROM View_EmployeeProduction WHERE EPSID=%n", IsActive, nEmployeeProductionID, nEmployeeProductionID);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nEPSID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeProduction WHERE EPSID=%n", nEPSID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeProduction");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static string GetBalance(string sSQL, TransactionContext tc)
        {
            object x = tc.ExecuteScalar(sSQL);
            return x.ToString();
        }

        #endregion
    }
}
