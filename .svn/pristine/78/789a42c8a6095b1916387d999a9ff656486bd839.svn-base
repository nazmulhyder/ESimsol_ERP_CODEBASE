using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.Services
{
    public class DBOperationArchiveService : MarshalByRefObject, IDBOperationArchiveService
    {
        #region Private functions and declaration

        private DBOperationArchive MapObject(NullHandler oReader)
        {
            DBOperationArchive oDBOperationArchive = new DBOperationArchive();
            oDBOperationArchive.DBOperationArchiveID = oReader.GetInt32("DBOperationArchiveID");
            oDBOperationArchive.BUID = oReader.GetInt32("BUID");
            oDBOperationArchive.DBOperationType = (EnumDBOperation)oReader.GetInt32("DBOperationType");
            oDBOperationArchive.ModuleName = (EnumModuleName)oReader.GetInt32("ModuleName");
            oDBOperationArchive.DBRefObjID = oReader.GetInt32("DBRefObjID");
            oDBOperationArchive.RefText = oReader.GetString("RefText");
            oDBOperationArchive.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oDBOperationArchive.UserName = oReader.GetString("UserName");
            oDBOperationArchive.BUName = oReader.GetString("BUName");
            return oDBOperationArchive;
        }

        private DBOperationArchive CreateObject(NullHandler oReader)
        {
            DBOperationArchive oDBOperationArchive = new DBOperationArchive();
            oDBOperationArchive = MapObject(oReader);
            return oDBOperationArchive;
        }

        private List<DBOperationArchive> CreateObjects(IDataReader oReader)
        {
            List<DBOperationArchive> oDBOperationArchive = new List<DBOperationArchive>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DBOperationArchive oItem = CreateObject(oHandler);
                oDBOperationArchive.Add(oItem);
            }
            return oDBOperationArchive;
        }

        #endregion

        #region Interface implementation

        public DBOperationArchiveService() { }

        public List<DBOperationArchive> Gets(string sSQL, int nCurrentUserID)
        {
            List<DBOperationArchive> oDBOperationArchive = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DBOperationArchiveDA.Gets(tc, sSQL);
                oDBOperationArchive = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DBOperationArchive", e);
                #endregion
            }

            return oDBOperationArchive;
        }

        #endregion
    }
}
