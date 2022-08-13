using System;
using System.IO;
using ICS.Base.Client.BOFoundation;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Base.Client.ServiceVessel;
using ICS.Base.Client.Utility;

namespace ESimSol.BusinessObjects
{

    /// <summary>
    /// Summary description for RunSQL.
    /// </summary>
    public class RunSQL
    {
        public RunSQL()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public void RunSQLScript(string sFilePath)
        {

            try
            {
                RunSQL.Service.RunSQL(sFilePath);
            }
            catch (Exception Exp)
            {
                throw new ServiceException(Exp.Message);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sDBTable">table that field will be updated in DataBase</param>
        /// <param name="sDBField">field that updated in table sDBTable of DataBase</param>
        /// <param name="oValue">value passed as object </param>
        /// <param name="oType">DBField Type(e.g. int, string, datetime, boolean)</param>
        /// <param name="sPKField">PrimaryKey FieldName of that table</param>
        /// <param name="nPKID">PrimaryKey value of that table; that is ObjectID</param>
        /// <returns></returns>
        public bool UpdateField(string sDBTable, string sDBField, object oValue, TypeCode oType, string sPKField, int nObjectID)
        {
            return RunSQL.Service.UpdateField(sDBTable, sDBField, oValue, oType,sPKField, nObjectID);
        }
        /// <summary>
        /// use in various class for getting only one value of a selected field
        /// </summary>
        /// <param name="sValueReturnField"></param>
        /// <param name="sTableName"></param>
        /// <param name="sWhereField">get top of mulitple items</param>
        /// <param name="nWhereVal">search criteria</param>
        /// <returns></returns>
        public int GetIntValue(string sValueReturnField, string sTableName, string sWhereField, int nWhereVal)
        {
            return RunSQL.Service.GetIntValue(sValueReturnField, sTableName, sWhereField, nWhereVal );
        }

        #region ServiceFactory

        internal static Type ServiceType
        {
            get
            {
                return typeof(IRunSQLService);
            }
        }
        #endregion

    }
    #region IRunSQL Service
    public interface IRunSQLService
    {
        void RunSQL(string FilePath);
        bool UpdateField(string sDBTable, string sDBField, object Value, TypeCode oType, string sPKField, int nPKID);
        int GetIntValue(string sValueReturnField, string sTableName, string sWhereField, int nWhereVal);
    }
    #endregion
}
