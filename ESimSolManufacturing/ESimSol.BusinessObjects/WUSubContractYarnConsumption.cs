using ICS.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    #region WUSubContractYarnConsumption
    public class WUSubContractYarnConsumption : BusinessObject
    {
        public WUSubContractYarnConsumption()
        {

            WUSubContractYarnConsumptionID = 0;
            WUSubContractID = 0;
            WarpWeftType = EnumWarpWeft.None;
            WarpWeftTypeInt = 0;
            YarnID = 0;
            MUnitID = 0;
            ConsumptionPerUnit = 0;
            ConsumptionQty = 0;
            Remarks = "";
            DBUserID = 0;
            DBServerDateTime = DateTime.Today;
            YarnCode = "";
            YarnName = "";
            MUSymbol = "";
            UnitType = "";
            YetToChallanQty = 0;
        }

        #region Properties
        public int WUSubContractYarnConsumptionID { get; set; }
        public int WUSubContractID { get; set; }
        public EnumWarpWeft WarpWeftType { get; set; }
        public int WarpWeftTypeInt { get; set; }
        public int YarnID { get; set; }
        public int MUnitID { get; set; }
        public double ConsumptionPerUnit { get; set; }
        public double ConsumptionQty { get; set; }
        public string Remarks { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string YarnCode { get; set; }
        public string YarnName { get; set; }
        public string MUSymbol { get; set; }
        public string ErrorMessage { get; set; }
        public string UnitType { get; set; }
        public double YetToChallanQty { get; set; }
        #endregion

        #region Derived Property
        public string WarpWeftTypeSt
        {
            get
            {
                return EnumObject.jGet(this.WarpWeftType);
            }
        }
        public string YarnNameWithReqQty
        {
            get
            {
                return EnumObject.jGet(this.WarpWeftType) + " : " + this.YarnName + "[" + this.YarnCode + "]   Req Qty : " + this.ConsumptionQty.ToString("#,##0.00") + "   Yet to Challan : " + this.YetToChallanQty.ToString("#,##0.00");
            }
        }
        #endregion

        #region Functions
        public static List<WUSubContractYarnConsumption> Gets(int id, long nUserID)
        {
            return WUSubContractYarnConsumption.Service.Gets(id, nUserID);
        }
        public static List<WUSubContractYarnConsumption> Get(string sSQL, int nCurrentUserID)
        {
            return WUSubContractYarnConsumption.Service.Get(sSQL, nCurrentUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IWUSubContractYarnConsumptionService Service
        {
            get { return (IWUSubContractYarnConsumptionService)Services.Factory.CreateService(typeof(IWUSubContractYarnConsumptionService)); }
        }
        #endregion
    }
    #endregion

    #region IWUSubContractYarnConsumption interface
    public interface IWUSubContractYarnConsumptionService
    {
        List<WUSubContractYarnConsumption> Gets(int id, Int64 nUserID);
        List<WUSubContractYarnConsumption> Get(string sSQL, int nCurrentUserID);
    }
    #endregion
}