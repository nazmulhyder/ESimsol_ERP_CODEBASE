using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;

namespace ESimSol.BusinessObjects
{
    #region EmployeeInfo
    public class EmployeeInfo
    {
        public EmployeeInfo()
        {

            EmployeeID = 0;
        }

        #region Properties
        public int EmployeeID { get; set; }
        public List<ApprovalHeadPerson> ApprovalHeadPersons { get; set; }
        #endregion

        #region Functions


        public static List<EmployeeInfo> Gets(string sSQL, long nUserID)
        {
            return EmployeeInfo.Service.Gets(sSQL, nUserID);
        }
        public static EmployeeInfo Get(string sSQL, long nUserID)
        {
            return EmployeeInfo.Service.Get(sSQL, nUserID);
        }
        public static DataSet SearchProfile(int nEmployeeID)
        {
            return EmployeeInfo.Service.SearchProfile(nEmployeeID);
        }
        #endregion


        private static EmployeeInfo MappingObject(DataRow oDataRow)
        {
            EmployeeInfo oEmployeeInfo = new EmployeeInfo();
            oEmployeeInfo.EmployeeID = (oDataRow["EmployeeID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["EmployeeID"]);


            return oEmployeeInfo;
        }
        public static EmployeeInfo CreateObject(DataRow oDataRow)
        {
            EmployeeInfo oEmployeeInfo = new EmployeeInfo();
            oEmployeeInfo = MappingObject(oDataRow);
            return oEmployeeInfo;
        }
        #endregion

        #region CreateObjects
        public static List<EmployeeInfo> CreateObjects(DataTable oDataTable)
        {
            List<EmployeeInfo> oEmployeeInfos = new List<EmployeeInfo>();
            foreach (DataRow oDataRow in oDataTable.Rows)
            {
                EmployeeInfo oItem = CreateObject(oDataRow);
                oEmployeeInfos.Add(oItem);
            }
            return oEmployeeInfos;
        }
        public static List<EmployeeInfo> CreateObjects(DataRow[] oDataRows)
        {
            List<EmployeeInfo> oEmployeeInfos = new List<EmployeeInfo>();
            foreach (DataRow oDataRow in oDataRows)
            {
                EmployeeInfo oItem = CreateObject(oDataRow);
                oEmployeeInfos.Add(oItem);
            }
            return oEmployeeInfos;
        }
        #region ServiceFactory

        internal static IEmployeeInfoService Service
        {
            get { return (IEmployeeInfoService)Services.Factory.CreateService(typeof(IEmployeeInfoService)); }
        }
        #endregion
    }
    #endregion

    #region IEmployeeInfo interface

    public interface IEmployeeInfoService
    {
        DataSet SearchProfile(int nEmployeeID);
        List<EmployeeInfo> Gets(string sSQL, Int64 nUserID);
        EmployeeInfo Get(string sSQL, Int64 nUserID);
       
      
    }
    #endregion
}

