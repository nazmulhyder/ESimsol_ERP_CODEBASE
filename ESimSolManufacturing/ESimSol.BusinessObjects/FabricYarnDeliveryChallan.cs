using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.BusinessObjects
{
    public class FabricYarnDeliveryChallan :BusinessObject
    {
        #region FabricYarnDeliveryChallan
        public FabricYarnDeliveryChallan()
        {
            FYDChallanID=0;
            FYDOID =0;
            FYDChallanNo =string.Empty;
            WUID =0;
            DisburseBy =0;
            DisburseDate =DateTime.Today;
            DBServerDateTime = DateTime.Now;
            ErrorMessage = string.Empty;
            Params = string.Empty;
            FYDCDetails = new List<FabricYarnDeliveryChallanDetail>();
            FEONo = string.Empty;
            FEOID = 0;
        }
        #endregion

        #region Properties
        public int  FYDChallanID {get; set;}
        public int  FYDOID {get; set;}
        public string  FYDChallanNo {get; set;}
        public int  WUID {get; set;}
        public int  DisburseBy {get; set;}
        public DateTime DisburseDate { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        #endregion

        #region Deriverd Properties
        public int FEOID { get; set; }
        public string FEONo { get; set; }
        public string FYDNo { get; set; }
        public string WUName { get; set; }
        public string DisburseByName { get; set; }   
        public string DisburseDateStr { get { return (this.DisburseDate == DateTime.MinValue) ? "" : this.DisburseDate.ToString("dd MMM yyyy"); } }
        public string DBServerDateTimeStr { get { return (this.DBServerDateTime == DateTime.MinValue) ? "" : this.DBServerDateTime.ToString("dd MMM yyyy"); } }

        public List<FabricYarnDeliveryChallanDetail> FYDCDetails { get; set; }
        #endregion

        #region Functions

        public static FabricYarnDeliveryChallan Get(int nFYDChallanID, long nUserID)
        {
            return FabricYarnDeliveryChallan.Service.Get(nFYDChallanID, nUserID);
        }
        public static List<FabricYarnDeliveryChallan> Gets(string sSQL, long nUserID)
        {
            return FabricYarnDeliveryChallan.Service.Gets(sSQL, nUserID);
        }
        public FabricYarnDeliveryChallan IUD(int nDBOperation, long nUserID)
        {
            return FabricYarnDeliveryChallan.Service.IUD(this, nDBOperation, nUserID);
        }
        public FabricYarnDeliveryChallan Disburse(long nUserID)
        {
            return FabricYarnDeliveryChallan.Service.Disburse(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFabricYarnDeliveryChallanService Service
        {
            get { return (IFabricYarnDeliveryChallanService)Services.Factory.CreateService(typeof(IFabricYarnDeliveryChallanService)); }
        }

        #endregion

    }
    #region IFabricYarnDeliveryChallan

    public interface IFabricYarnDeliveryChallanService
    {

        FabricYarnDeliveryChallan Get(int nFYDChallanID, Int64 nUserID);
        List<FabricYarnDeliveryChallan> Gets(string sSQL, Int64 nUserID);
        FabricYarnDeliveryChallan IUD(FabricYarnDeliveryChallan oFabricYarnDeliveryChallan, int nDBOperation, Int64 nUserID);
        FabricYarnDeliveryChallan Disburse(FabricYarnDeliveryChallan oFabricYarnDeliveryChallan, Int64 nUserID);

        
    }
    #endregion
}
