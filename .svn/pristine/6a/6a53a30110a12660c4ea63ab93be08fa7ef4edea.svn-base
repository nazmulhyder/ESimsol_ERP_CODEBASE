using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region COA_ChartOfAccountCostCenter
    public class COA_ChartOfAccountCostCenter : BusinessObject
    {
        public COA_ChartOfAccountCostCenter()
        {
            CACCID = 0;
            AccountHeadID = 0;
            CCID = 0;
            CCCode="";
            CCName="";
            Amount = 0.00;
        }

        #region Properties
        public int CACCID { get; set; }
        public int ParentID { get; set; }
        public string DisplayMessage { get; set; }
        public int AccountHeadID { get; set; }
        public int CCID { get; set; }
        public string SelectedCostCenterIDs { get; set; }
        public string CCCode { get; set; }
        public string CCName { get; set; }
        public string SelectedCOAIDs { get; set; }
        public string COACode { get; set; }
        public string COAName { get; set; }
        public double Amount { get; set; }        
        #endregion

        #region Derive Properties
        public IEnumerable<COA_ChartOfAccountCostCenter> LstChartOfAccountcostCenter { get; set; }
        public string CCNameCode
        {
            get
            {
                return CCName + " [" + CCCode + "]";
            }
        }
        public IEnumerable<COA_ChartOfAccountCostCenter> LstCostCenterChartOfAccount { get; set; }
        public string COANameCode
        {
            get
            {
                return COAName + " [" + COACode + "]";
            }
        }     
        #endregion

        #region Functions
        public static List<COA_ChartOfAccountCostCenter> GetsByCostCente(int nCCID, int nUserID)
        {
            return COA_ChartOfAccountCostCenter.Service.GetsByCostCente(nCCID, nUserID);
        }
        public COA_ChartOfAccountCostCenter Get(int id, int nUserID)
        {
            return COA_ChartOfAccountCostCenter.Service.Get(id, nUserID);
        }
        public COA_ChartOfAccountCostCenter Save(int nUserID)
        {
            return COA_ChartOfAccountCostCenter.Service.Save(this, nUserID);
        }
        public bool Delete(int id, int nUserID)
        {
            return COA_ChartOfAccountCostCenter.Service.Delete(id, nUserID);
        }
        public static List<COA_ChartOfAccountCostCenter> Gets(int nUserID)
        {
            return COA_ChartOfAccountCostCenter.Service.Gets(nUserID);
        }
        public static List<COA_ChartOfAccountCostCenter> Gets(string sSQL, int nUserID)
        {
            return COA_ChartOfAccountCostCenter.Service.Gets(sSQL, nUserID);
        }

        #endregion

        
        #region ServiceFactory
        internal static ICOA_ChartOfAccountCostCenterService Service
        {
            get { return (ICOA_ChartOfAccountCostCenterService)Services.Factory.CreateService(typeof(ICOA_ChartOfAccountCostCenterService)); }
        }
        #endregion
    }
    #endregion

    #region ICOA_ChartOfAccountCostCenter interface
    public interface ICOA_ChartOfAccountCostCenterService
    {
        COA_ChartOfAccountCostCenter Get(int id, int nUserID);
        List<COA_ChartOfAccountCostCenter> Gets(int nUserID);
        List<COA_ChartOfAccountCostCenter> GetsByCostCente(int nCCID, int nUserID);
        bool Delete(int id, int nUserID);
        COA_ChartOfAccountCostCenter Save(COA_ChartOfAccountCostCenter oCOA_ChartOfAccountCostCenter, int nUserID);
        List<COA_ChartOfAccountCostCenter> Gets(string sSQL, int nUserID);
    }
    #endregion

}
