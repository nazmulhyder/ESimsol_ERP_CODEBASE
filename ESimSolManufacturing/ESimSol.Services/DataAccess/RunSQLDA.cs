using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    /// <summary>
    /// Summary description for RunSQLDA.
    /// </summary>
    public class RunSQLDA
    {
        public RunSQLDA()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static void RunSQL(TransactionContext tc, string sSQL)
        {
            tc.ExecuteNonQuery(sSQL);
        }

        public static void UpdateField(TransactionContext tc, string sDBTable, string sDBField, object Value, TypeCode oType, string sPKField, int nPKID)
        {
            switch (oType)
            {
                case TypeCode.Boolean:
                    tc.ExecuteNonQuery("UPDATE %q SET %q=%b WHERE %q=%n", sDBTable, sDBField, Convert.ToBoolean(Value), sPKField, nPKID);
                    break;

                case TypeCode.DateTime:
                    tc.ExecuteNonQuery("UPDATE %q SET  %q=%D WHERE %q=%n", sDBTable, sDBField, Convert.ToDateTime(Value), sPKField, nPKID);
                    break;

                case TypeCode.Decimal:
                    tc.ExecuteNonQuery("UPDATE %q SET  %q=%n WHERE %q=%n", sDBTable, sDBField, Convert.ToDecimal(Value), sPKField, nPKID);
                    break;

                case TypeCode.Double:
                    tc.ExecuteNonQuery("UPDATE %q SET  %q=%n WHERE %q=%n", sDBTable, sDBField, Convert.ToDouble(Value), sPKField, nPKID);
                    break;

                case TypeCode.Int16:
                case TypeCode.Int32:
                //case TypeCode.Int64:
                    tc.ExecuteNonQuery("UPDATE %q SET  %q=%n WHERE %q=%n", sDBTable, sDBField, Convert.ToInt32(Value), sPKField, nPKID);
                    break;

                case TypeCode.String:
                    tc.ExecuteNonQuery("UPDATE %q SET  %q=%s WHERE %q=%n", sDBTable, sDBField, Convert.ToString(Value), sPKField, nPKID);
                    break;

                default:
                    break;
            }
        }

        public static int ChildCount(TransactionContext tc, string sTableName, string sFKField, int nParentID)
        {
            return ChildCount(tc, sTableName, sFKField, nParentID, "");
        }
        public static int ChildCount(TransactionContext tc, string sTableName, string sFKField, int nParentID, string sMoreCondition)
        {
            object obj = tc.ExecuteScalar("SELECT Count(*) From %q WHERE %q=%n %q", sTableName, sFKField, nParentID, sMoreCondition);
            int Temp = Convert.ToInt32(obj);
            return Temp;
        }

        public static int GetIntValue(TransactionContext tc, string sValueReturnField, string sTableName, string sWhereField, int nWhereVal)
        {
            object obj = tc.ExecuteScalar("SELECT TOP 1 %q From %q WHERE %q=%n ORDER BY %q", sValueReturnField, sTableName, sWhereField, nWhereVal, sValueReturnField);
            int Temp = Convert.ToInt32(obj);
            return Temp;
        }

        public static double GetIntValueDouble(TransactionContext tc, string sValueReturnField, string sTableName, string sWhereField, int nWhereVal)
        {
            object obj = tc.ExecuteScalar("SELECT TOP 1 %q From %q WHERE %q=%n ORDER BY %q", sValueReturnField, sTableName, sWhereField, nWhereVal, sValueReturnField);
            double Temp = Convert.ToDouble(obj);
            return Temp;
        }
    }
}
