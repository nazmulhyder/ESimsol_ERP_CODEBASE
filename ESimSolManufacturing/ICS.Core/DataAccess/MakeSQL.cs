using System;
using System.Data;

namespace ICS.Core.DataAccess
{
    #region DataAccess: MakeSQL
    public class SQLParser
    {
        #region EnuSQL Syntax Enum
        public enum SQLSyntax
        {
            Access = 1, SQL, Oracle
        }
        #endregion
        #region Declaraion & Constructor
        static SQLSyntax _sqlSyntax = SQLSyntax.SQL;
        public SQLParser()
        {
        }

        #endregion
        #region DateTime Formatting Functions
        private static string GetDateLiteral(DateTime dt)
        {
            string s = "";
            switch (_sqlSyntax)
            {
                case SQLSyntax.Access:
                    s = "#" + dt.ToString("dd MMM yyyy") + "#";
                    break;
                case SQLSyntax.Oracle:
                    s = "TO_DATE('" + dt.ToString("dd MM yyyy") + "', '" + "DD MM YYYY" + "')";
                    break;
                case SQLSyntax.SQL:
                    s = "'" + dt.ToString("dd MMM yyyy") + "'";                    
                    break;
            }
            return s;
        }

        private static string GetDateTimeLiteral(DateTime dt)
        {
            string s = "";
            switch (_sqlSyntax)
            {
                case SQLSyntax.Access:
                    s = "#" + dt.ToString("dd MMM yyyy HH:mm:ss") + "#";
                    break;
                case SQLSyntax.Oracle:
                    s = "TO_DATE('" + dt.ToString("dd MM yyyy HH mm ss") + "', 'DD MM YYYY HH24 MI SS')";
                    break;
                case SQLSyntax.SQL:
                    s = "'" + dt.ToString("dd MMM yyyy HH:mm:ss") + "'";
                    break;
            }
            return s;
        }

        private static string GetTimeLiteral(DateTime dt)
        {
            string s = "";
            switch (_sqlSyntax)
            {
                case SQLSyntax.Access:
                    s = "#" + dt.ToString("HH:mm:ss") + "#";
                    break;
                case SQLSyntax.Oracle:
                    s = "TO_DATE('" + dt.ToString("HH mm ss") + "', 'HH24 MI SS')";
                    break;
                case SQLSyntax.SQL:
                    s = "'" + dt.ToString("HH:mm:ss") + "'";
                    break;
            }
            return s;
        }
        /// <summary>
        /// by-jakaria
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static string GetTimeSpanLiteral(TimeSpan ts)
        {
            string s = "";
            switch (_sqlSyntax)
            {
                case SQLSyntax.Access:
                    s = "#" + ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00") + "#";
                    break;
                case SQLSyntax.Oracle:
                    s = "TO_DATE('" + ts.Hours.ToString("00") + " " + ts.Minutes.ToString("00") + " " + ts.Seconds.ToString("00") + "', 'HH24 MI SS')";
                    break;
                case SQLSyntax.SQL:
                    s = "'" + ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00") + "'";
                    break;
            }
            return s;
        }
        #endregion
        #region Make SQL function
        public static string MakeSQL(string sql, params object[] args)
        {
            string retSQL = "";

            if (args.Length == 0)
            {
                return sql;
            }

            string[] argSQL = new string[args.Length];
            int argIndex = -1;

            int i = sql.IndexOf("%");

            while (i != -1)
            {
                retSQL = retSQL + sql.Substring(0, i);
                if (i == sql.Length - 1)
                {
                    throw new InvalidExpressionException("Invalid Place Holder Character");
                }

                string c = sql.Substring(i + 1, 1);
                sql = sql.Substring(i + 2);

                if (c.IndexOfAny(new char[] { '%', '{' }) != -1)
                {
                    switch (c)
                    {
                        case "%":
                            retSQL = retSQL + "%";
                            break;
                        case "{":
                            int next = sql.IndexOf("}");
                            if (next < 1)
                            {
                                throw new InvalidExpressionException("Invalid Ordinal Variable");
                            }
                            int ord = Convert.ToInt32(sql.Substring(0, next));
                            if (ord < 0 || ord > argIndex)
                            {
                                throw new IndexOutOfRangeException("Invalid Ordinal Variable");
                            }
                            retSQL = retSQL + argSQL[ord];
                            sql = sql.Substring(next + 1);
                            break;
                    }
                }
                else if (c.IndexOfAny(new char[] { 's', 'n', 'd', 't', 'D', 'b', 'q', 'u' }) != -1)
                {
                    if (++argIndex > args.Length - 1)
                    {
                        throw new ArgumentException("Too few arguments passed");
                    }

                    if (args[argIndex] == null)
                    {
                        argSQL[argIndex] = "NULL";
                    }
                    else
                    {
                        try
                        {
                            switch (c)
                            {
                                case "s":
                                    string s = Convert.ToString(args[argIndex]);
                                    argSQL[argIndex] = "'" + s.Replace("'", "''") + "'";
                                    break;
                                case "n":
                                    decimal n = Convert.ToDecimal(args[argIndex]);
                                    argSQL[argIndex] = n.ToString();
                                    break;
                                case "d":
                                    DateTime d = Convert.ToDateTime(args[argIndex]);
                                    argSQL[argIndex] = GetDateLiteral(d);
                                    break;
                                case "D":
                                    DateTime D = Convert.ToDateTime(args[argIndex]);
                                    argSQL[argIndex] = GetDateTimeLiteral(D);
                                    break;
                                case "t":
                                    //By-jakaria
                                    if (args[argIndex].GetType().FullName == "System.TimeSpan")
                                    {
                                        TimeSpan ts = (TimeSpan)args[argIndex];
                                        argSQL[argIndex] = GetTimeSpanLiteral(ts);
                                        break;
                                    }
                                    DateTime t = Convert.ToDateTime(args[argIndex]);                                    
                                    argSQL[argIndex] = GetTimeLiteral(t);
                                    break;
                                
                                case "b":
                                    bool b = Convert.ToBoolean(args[argIndex]);
                                    if (_sqlSyntax == SQLSyntax.Access)
                                    {
                                        argSQL[argIndex] = b.ToString();
                                    }
                                    else
                                    {
                                        argSQL[argIndex] = (b ? "1" : "0");
                                    }
                                    break;
                                case "q":
                                    string q = Convert.ToString(args[argIndex]);
                                    argSQL[argIndex] = q;
                                    break;
                                case "u"://for unicode conversion
                                    string u = Convert.ToString(args[argIndex]);
                                    argSQL[argIndex] = "N'" + u.Replace("'", "''") + "'";
                                    break;
                            }
                        }
                        catch
                        {
                            throw new ArgumentException("Invalid argument #" + argIndex);
                        }
                    }
                    retSQL = retSQL + argSQL[argIndex];
                }
                else
                {
                    throw new InvalidExpressionException("Invalid Place Holder Character");
                }
                i = sql.IndexOf("%");
            }
            retSQL = retSQL + sql;

            // Handle the (==)
            i = retSQL.IndexOf("==");
            while (i != -1)
            {
                string rVal = retSQL.Substring(i + 2).Trim().Substring(0, 4);
                if (rVal.ToUpper() == "NULL")
                {
                    retSQL = retSQL.Substring(0, i)
                        + "IS" + retSQL.Substring(i + 2);
                }
                else
                {
                    retSQL = retSQL.Substring(0, i)
                        + "=" + retSQL.Substring(i + 2);
                }
                i = retSQL.IndexOf("==", i + 2);
            }

            // Handle the (<>)
            i = retSQL.IndexOf("<>");
            while (i != -1)
            {
                string rVal = retSQL.Substring(i + 2).Trim().Substring(1, 4);
                if (rVal.ToUpper() == "NULL")
                {
                    retSQL = retSQL.Substring(0, i)
                        + "IS NOT" + retSQL.Substring(i + 2);
                }
                i = retSQL.IndexOf("<>", i + 2);
            }

            return retSQL;
        }
        #endregion
        #region TagSQL Function
        public static string TagSQL(string sSql)
        {
            string sRetSql = "";
            if (sSql.Trim().Length <= 0)
            { sRetSql = " WHERE "; }
            else
            { sRetSql = sSql + " AND "; }
            return sRetSql;
        }
        #endregion
    }
    #endregion
}
