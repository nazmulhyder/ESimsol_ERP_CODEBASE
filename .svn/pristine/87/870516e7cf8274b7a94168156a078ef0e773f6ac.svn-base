using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{

    #region MerchandiserDashboard

    public class MerchandiserDashboard : BusinessObject
    {
        public MerchandiserDashboard()
        {
            BuyerID = 0;
            TechnicalSheetID = 0 ;
			MerchandiserName = "" ;
			StyleNo = "" ;
			BuyerName = "" ;
			SessionName = "" ;
            NumberOfCostSheet = 0;
            NumberOfOrderRecap = 0;
            NumberOfProductionOrder = 0;
            NumberOfPEPlan = 0;
			NumberOfPendingTask = 0 ;
			NumberOfCompleteTask = 0 ;
            ErrorMessage = "";
        }

        #region Properties
        public int BuyerID { get; set; }
        public int TechnicalSheetID { get; set; }
        public string MerchandiserName { get; set; }
        public string SessionName { get; set; }
        public int NumberOfCostSheet { get; set; }
        public int NumberOfOrderRecap { get; set; }
        public string StyleNo { get; set; }
        public string BuyerName { get; set; }
        public int NumberOfProductionOrder { get; set; }
        public int NumberOfPEPlan { get; set; }
        public int NumberOfPendingTask { get; set; }
        public int NumberOfCompleteTask { get; set; }
        public string sParam { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
       
        public List<MerchandiserDashboard> MerchandiserDashboards { get; set; }

        public string NumberOfCostSheetInString
        {
            get
            {
                return this.TechnicalSheetID + "~" + this.NumberOfCostSheet.ToString();
            }
        }
        public string NumberOfOrderRecapInString
        {
            get
            {
                return this.TechnicalSheetID + "~" + this.NumberOfOrderRecap.ToString();
            }
        }
        public string NumberOfProductionOrderInString
        {
            get
            {
                return this.TechnicalSheetID + "~" + this.NumberOfProductionOrder.ToString();
            }
        }
        public string NumberOfPEPlanInString
        {
            get
            {
                return this.TechnicalSheetID + "~" + this.NumberOfPEPlan.ToString();
            }
        }
        public string NumberOfPendingTaskInString
        {
            get
            {
                return this.TechnicalSheetID + "~" + this.NumberOfPendingTask.ToString();
            }
        }
        public string NumberOfCompleteTaskInString
        {
            get
            {
                return this.TechnicalSheetID + "~" + this.NumberOfCompleteTask.ToString();
            }
        }
        public Company Company { get; set; }
        #endregion

        #region Functions

        public static List<MerchandiserDashboard> Gets(string sMainSQL, string sPOSQL, string sORSQL, string sCSSQL, string sPESQL, string sPendingSQL, string sCompleteSQL)
        {
            return MerchandiserDashboard.Service.Gets(sMainSQL, sPOSQL, sORSQL, sCSSQL, sPESQL, sPendingSQL, sCompleteSQL);
        }
        #endregion

        #region ServiceFactory
        internal static IMerchandiserDashboardService Service
        {
            get { return (IMerchandiserDashboardService)Services.Factory.CreateService(typeof(IMerchandiserDashboardService)); }
        }
        #endregion
    }

    #endregion

    #region IMerchandiserDashboard interface

    public interface IMerchandiserDashboardService
    {


        List<MerchandiserDashboard> Gets(string sMainSQL, string sPOSQL, string sORSQL, string sCSSQL, string sPESQL, string sPendingSQL, string sCompleteSQL);

    }
    #endregion
    
 
}
