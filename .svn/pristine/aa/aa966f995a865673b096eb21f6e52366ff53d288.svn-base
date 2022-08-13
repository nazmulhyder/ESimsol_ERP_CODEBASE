using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{

    public class PlanAnalysisDA
    {
        public PlanAnalysisDA() { }

        #region Insert Update Delete Function

        public static IDataReader Gets(int stepID, int ProductionOrderID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC[SP_PlanAnalysis]"+"%n,%n",stepID,ProductionOrderID);
        }

        #endregion

    }  

}
