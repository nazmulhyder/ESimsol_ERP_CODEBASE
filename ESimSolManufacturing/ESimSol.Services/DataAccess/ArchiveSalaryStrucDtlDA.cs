using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;
namespace ESimSol.Services.DataAccess
{
   public  class ArchiveSalaryStrucDtlDA
    {
       public static IDataReader Gets(TransactionContext tc, int id)
       {
           return tc.ExecuteReader("SELECT * FROM View_ArchiveSalaryStrucDtl WHERE ArchiveSalaryStrucID = %n Order By ArchiveSalaryStrucDtlID", id);
       } 
    }
}
