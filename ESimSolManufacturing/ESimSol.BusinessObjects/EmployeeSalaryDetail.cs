using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;


namespace ESimSol.BusinessObjects
{
    #region EmployeeSalaryDetail

    public class EmployeeSalaryDetail : BusinessObject
    {
        public EmployeeSalaryDetail()
        {
            ESDID = 0;
            EmployeeSalaryID = 0;
            SalaryHeadID = 0;
            Amount = 0;
            SalaryHeadName = "";
            Equation = "";
            SalaryHeadType = 0;
            ErrorMessage = "";
            CompAmount = 0;
            EmployeeID = 0;
        }

        #region Properties
        public int ESDID { get; set; }
        public int EmployeeSalaryID { get; set; }
        public int SalaryHeadID { get; set; }
        public int EmployeeID { get; set; }
        public double Amount { get; set; }
        public double CompAmount { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string SalaryHeadName { get; set; }
        public string Equation { get; set; }
        public int SalaryHeadType { get; set; }

        public string SalaryHeadNameWithEquation
        {
            get { if (Equation != "") return (SalaryHeadName + "(" + Equation + ")"); else return SalaryHeadName; }
        }

        #endregion

        private static EmployeeSalaryDetail MappingObject(DataRow oDataRow)
        {
            EmployeeSalaryDetail oEmployeeSalaryDetail = new EmployeeSalaryDetail();
            oEmployeeSalaryDetail.EmployeeSalaryID = (oDataRow["EmployeeSalaryID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["EmployeeSalaryID"]);
            oEmployeeSalaryDetail.SalaryHeadType = (oDataRow["SalaryHeadType"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["SalaryHeadType"]);
            oEmployeeSalaryDetail.SalaryHeadID = (oDataRow["SalaryHeadID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["SalaryHeadID"]);
            oEmployeeSalaryDetail.SalaryHeadName = (oDataRow["SalaryHeadName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["SalaryHeadName"]);
            oEmployeeSalaryDetail.Amount = (oDataRow["Amount"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["Amount"]);
            return oEmployeeSalaryDetail;
        }
        #region CreateObject
        public static EmployeeSalaryDetail CreateObject(DataRow oDataRow)
        {
            EmployeeSalaryDetail oEmployeeSalaryDetail = new EmployeeSalaryDetail();
            oEmployeeSalaryDetail = MappingObject(oDataRow);
            return oEmployeeSalaryDetail;
        }
        #endregion

        #region CreateObjects
        public static List<EmployeeSalaryDetail> CreateObjects(DataTable oDataTable)
        {
            List<EmployeeSalaryDetail> oEmployeeSalaryDetails = new List<EmployeeSalaryDetail>();
            foreach (DataRow oDataRow in oDataTable.Rows)
            {
                EmployeeSalaryDetail oItem = CreateObject(oDataRow);
                oEmployeeSalaryDetails.Add(oItem);
            }
            return oEmployeeSalaryDetails;
        }
        public static List<EmployeeSalaryDetail> CreateObjects(DataRow[] oDataRows)
        {
            List<EmployeeSalaryDetail> oEmployeeSalaryDetails = new List<EmployeeSalaryDetail>();
            foreach (DataRow oDataRow in oDataRows)
            {
                EmployeeSalaryDetail oItem = CreateObject(oDataRow);
                oEmployeeSalaryDetails.Add(oItem);
            }
            return oEmployeeSalaryDetails;
        }
        #endregion

        #region Functions
        public static EmployeeSalaryDetail Get(int id, long nUserID)
        {
            return EmployeeSalaryDetail.Service.Get(id, nUserID);
        }

        public static EmployeeSalaryDetail Get(string sSQL, long nUserID)
        {
            return EmployeeSalaryDetail.Service.Get(sSQL, nUserID);
        }

        public static List<EmployeeSalaryDetail> Gets(long nUserID)
        {
            return EmployeeSalaryDetail.Service.Gets(nUserID);
        }

        public static List<EmployeeSalaryDetail> Gets(string sSQL, long nUserID)
        {
            return EmployeeSalaryDetail.Service.Gets(sSQL, nUserID);
        }

        public static List<EmployeeSalaryDetail> GetsForPaySlip(string sEmployeeSalaryIDs, long nUserID)
        {
            return EmployeeSalaryDetail.Service.GetsForPaySlip(sEmployeeSalaryIDs, nUserID);
        }

        public EmployeeSalaryDetail IUD(int nDBOperation, long nUserID)
        {
            return EmployeeSalaryDetail.Service.IUD(this, nDBOperation, nUserID);
        }



        #endregion

        #region ServiceFactory
        internal static IEmployeeSalaryDetailService Service
        {
            get { return (IEmployeeSalaryDetailService)Services.Factory.CreateService(typeof(IEmployeeSalaryDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeSalaryDetail interface

    public interface IEmployeeSalaryDetailService
    {
        EmployeeSalaryDetail Get(int id, Int64 nUserID);
        EmployeeSalaryDetail Get(string sSQL, Int64 nUserID);
        List<EmployeeSalaryDetail> Gets(Int64 nUserID);
        List<EmployeeSalaryDetail> Gets(string sSQL, Int64 nUserID);
        List<EmployeeSalaryDetail> GetsForPaySlip(string sEmployeeSalaryIDs, Int64 nUserID);
        EmployeeSalaryDetail IUD(EmployeeSalaryDetail oEmployeeSalaryDetailSheet, int nDBOperation, Int64 nUserID);


    }
    #endregion
}
