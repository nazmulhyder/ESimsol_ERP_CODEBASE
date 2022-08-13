using System;
using System.Data;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using System.IO;

namespace ESimSol.Services.Services
{
    /// <summary>
    /// Summary description for RunSQLService.
    /// </summary>
    public class RunSQLService : MarshalByRefObject, IRunSQLService
    {
        public RunSQLService()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public void RunSQL(string FilePath)
        {
            RunSQLDA oDA = new RunSQLDA();
            if (!File.Exists(FilePath + "/Scripts.txt"))
            {
                return;
            }

            string line = null;
            TransactionContext tc = null;
            tc = TransactionContext.Begin(true);
            StreamReader strReader = null;
            try
            {

                strReader = new StreamReader(FilePath + "/Scripts.txt");

                while (((line = strReader.ReadLine()) != null))
                {
                    RunSQLDA.RunSQL(tc, line.ToString());
                }
                strReader.Close();
                tc.End();
                File.Delete(FilePath + "/Scripts.txt");
            }
            catch (Exception e)
            {
                strReader.Close();
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                File.Delete(FilePath + "/Scripts.txt");
                throw new ServiceException(e.Message, e);
                #endregion
            }
        }

        public bool UpdateField(string sDBTable, string sDBField, object Value, TypeCode oType, string sPKField, int nPKID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                RunSQLDA.UpdateField(tc, sDBTable, sDBField, Value, oType, sPKField, nPKID);
                tc.End();
                return true;
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError(); 

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to update field ", e);
                #endregion
            }

            return false;
        }

        public int GetIntValue(string sValueReturnField, string sTableName, string sWhereField, int nWhereVal)
        {
            int nReturn = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nReturn = RunSQLDA.GetIntValue(tc, sValueReturnField, sTableName, sWhereField, nWhereVal);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteSheet", e);
                #endregion
            }
            return nReturn;
        }
    }
}