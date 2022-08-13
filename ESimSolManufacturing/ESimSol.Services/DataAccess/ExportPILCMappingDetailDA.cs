using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class ExportPILCMappingDetailDA
    {
        public ExportPILCMappingDetailDA() { }

        #region Insert Update Delete Function
      
        #endregion

        #region Get & Exist Function


        public static IDataReader GetsBy(TransactionContext tc, int nExportLCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPILCMappingDetail as PILCD WHERE PILCD.PIStatus=" + (int)EnumPIStatus.BindWithLC + " and PILCD.ExportPIDetailID in (SELECT ExportPIDetail.ExportPIDetailID FROM ExportPIDetail WHERE ExportPIID in (Select ExportPIID from ExportPILCMapping where ExportLCID=%n and Activity=1)) and ExportPILCMappingID in (Select ExportPILCMapping.ExportPILCMappingID from ExportPILCMapping where Activity=1 and ExportLCID=%n)", nExportLCID, nExportLCID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
      
        #endregion
    }
}
