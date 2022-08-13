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

    public class QCTemplateDA
    {
        public QCTemplateDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, QCTemplate oQCTemplate, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_QCTemplate]"
                                    + "%n,  %s, %s, %s,%n, %d, %n, %n",
                                    oQCTemplate.QCTemplateID, oQCTemplate.TemplateNo, oQCTemplate.TemplateName, oQCTemplate.Note, oQCTemplate.CreateBy, oQCTemplate.CreateDate, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, QCTemplate oQCTemplate, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_QCTemplate]"
                                    + "%n,  %s, %s, %s,%n, %d, %n, %n",
                                    oQCTemplate.QCTemplateID, oQCTemplate.TemplateNo, oQCTemplate.TemplateName, oQCTemplate.Note, oQCTemplate.CreateBy, oQCTemplate.CreateDate, (int)eEnumDBOperation, nUserID);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_QCTemplate WHERE QCTemplateID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_QCTemplate");
        }

       
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }  
    
 
}
