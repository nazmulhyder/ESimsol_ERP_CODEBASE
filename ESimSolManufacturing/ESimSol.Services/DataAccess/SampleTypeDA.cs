﻿using System;
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


    public class SampleTypeDA
    {
        public SampleTypeDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SampleType oSampleType, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SampleType]"
                                    + "%n, %s, %s, %s, %n, %n",
                                    oSampleType.SampleTypeID, oSampleType.Code, oSampleType.SampleName, oSampleType.Note, nUserID,  (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, SampleType oSampleType, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SampleType]"
                                    + "%n, %s, %s, %s, %n, %n",
                                    oSampleType.SampleTypeID, oSampleType.Code, oSampleType.SampleName, oSampleType.Note, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM SampleType WHERE SampleTypeID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM SampleType");
        }

      
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
    

}
