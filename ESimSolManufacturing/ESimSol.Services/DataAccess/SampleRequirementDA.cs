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

    public class SampleRequirementDA
    {
        public SampleRequirementDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SampleRequirement oSampleRequirement, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SampleRequirement]"
                                    + "%n, %n, %n, %s, %n, %n",
                                    oSampleRequirement.SampleRequirementID, oSampleRequirement.OrderRecapID, oSampleRequirement.SampleTypeID, oSampleRequirement.Remark,  nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, SampleRequirement oSampleRequirement, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SampleRequirement]"
                                    + "%n, %n, %n, %s, %n, %n",
                                    oSampleRequirement.SampleRequirementID, oSampleRequirement.OrderRecapID, oSampleRequirement.SampleTypeID, oSampleRequirement.Remark, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleRequirement WHERE SampleRequirementID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleRequirement ");
        }

        public static IDataReader Gets(int id , TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleRequirement  WHERE OrderRecapID = %n", id);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
    
 
}
