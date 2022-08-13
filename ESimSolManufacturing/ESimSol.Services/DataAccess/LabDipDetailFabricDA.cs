using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class LabDipDetailFabricDA
    {
        public LabDipDetailFabricDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, LabDipDetailFabric oLabDipDetailFabric, int nBDOperation , int nUserID, string nIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabDipDetailFabric] %n, %n, %n, %n, %s, %n, %b, %s, %n, %n,  %n,  %n, %n, %s",
                                    oLabDipDetailFabric.LDFID, oLabDipDetailFabric.LabDipID, oLabDipDetailFabric.FabricID, oLabDipDetailFabric.FSCDetailID, oLabDipDetailFabric.Remarks, oLabDipDetailFabric.LabDipDetailID, oLabDipDetailFabric.IsForAll,
                                    oLabDipDetailFabric.ColorName,  oLabDipDetailFabric.WarpWeftType, oLabDipDetailFabric.ProductID,  oLabDipDetailFabric.ReviseNo,  nUserID, nBDOperation, nIDs);
        }
        #endregion
    
        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM  View_LabDipDetailFabric Where LabDipDetailFabricID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nLabDipID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LabDipDetailFabric Where LabDipID=%n", nLabDipID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader MakeTwistedGroup(TransactionContext tc, string sLabDipDetailFabricID, int nLabDipID, int nTwistedGroup, int nParentID, int nDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabDipDetailFabric_Twisted] %s, %n, %n, %n, %n, %n", sLabDipDetailFabricID, nLabDipID, nTwistedGroup, nParentID, nUserID, nDBOperation);
        }
        #endregion
        
    }
}