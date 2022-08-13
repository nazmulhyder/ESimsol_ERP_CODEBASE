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
    public class DiagramIdentificationDA
    {
        public DiagramIdentificationDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DiagramIdentification oDiagramIdentification, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {

            return tc.ExecuteReader("EXEC [SP_IUD_DiagramIdentification]"
                                 + "%n,%s,%s, %s,%n,%n",
                                 oDiagramIdentification.DiagramIdentificationID, oDiagramIdentification.MesurementPoint, oDiagramIdentification.PointName, oDiagramIdentification.Note, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, DiagramIdentification oDiagramIdentification, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DiagramIdentification]"
                                    + "%n,%s,%s, %s,%n,%n",
                                    oDiagramIdentification.DiagramIdentificationID, oDiagramIdentification.MesurementPoint, oDiagramIdentification.PointName, oDiagramIdentification.Note, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM DiagramIdentification WHERE DiagramIdentificationID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM DiagramIdentification ORDER BY PointName");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets_print(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetsByName(TransactionContext tc, string sName)
        {
            return tc.ExecuteReader("SELECT * FROM DiagramIdentification WHERE PointName LIKE ('%" + sName + "%')  Order by [PointName]");
        }

        #endregion
    }
}
