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

    public class TAPTemplateDA
    {
        public TAPTemplateDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TAPTemplate oTAPTemplate, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TAPTemplate]"
                                    + "%n, %s, %s, %n,%d,%s,%n,%n, %n,%n",
                                    oTAPTemplate.TAPTemplateID, oTAPTemplate.TemplateNo, oTAPTemplate.TemplateName, oTAPTemplate.CreateBy, oTAPTemplate.CreateDate, oTAPTemplate.Remarks, (int)oTAPTemplate.TampleteType,(int)oTAPTemplate.CalculationType, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, TAPTemplate oTAPTemplate, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TAPTemplate]"
                                    + "%n, %s, %s, %n,%d,%s,%n,%n, %n,%n",
                                    oTAPTemplate.TAPTemplateID, oTAPTemplate.TemplateNo, oTAPTemplate.TemplateName, oTAPTemplate.CreateBy, oTAPTemplate.CreateDate, oTAPTemplate.Remarks, (int)oTAPTemplate.TampleteType, (int)oTAPTemplate.CalculationType, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TAPTemplate WHERE TAPTemplateID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TAPTemplate");
        }

        public static IDataReader GetsByTemplateType(int nTamplateType, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TAPTemplate WHERE ISNULL(TampleteType,0) = "+nTamplateType);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }  
    
 
}
