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
    #region ManagementDashboard

    public class ManagementDashboard : BusinessObject
    {
        public ManagementDashboard()
        {
            MerchandiserID = 0;
            MerchandiserName = "";
            LogInID = "";
            NumberOfStyle = 0;
            NumberOfDevelopment = 0;
            NumberOfOrder = 0;
            TotalOrderQty = 0;
            NumberOfBuyer = 0;
            NumberOfFactory = 0;
            TaskPending = 0;
            RegularTask = 0;
            BusinessSessionID = 0;
            NumberOfTAP = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int MerchandiserID { get; set; }
        public string MerchandiserName { get; set; }
        public string LogInID { get; set; }
        public int NumberOfStyle { get; set; }
        public int NumberOfDevelopment { get; set; }
        public int NumberOfOrder { get; set; }
        public int NumberOfTAP { get; set; }
        public int TotalOrderQty { get; set; }
        public int NumberOfBuyer { get; set; }
        public int NumberOfFactory { get; set; }
        public int TaskPending { get; set; }
        public int RegularTask { get; set; }
        public int BusinessSessionID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string NumberOfStyleWithMerchandiserID
        {
            get
            {
                return this.MerchandiserID.ToString() + "~" + this.NumberOfStyle.ToString();
            }
        }
        public string NumberOfDevelopmentWithMerchandiserID
        {
            get
            {
                return this.MerchandiserID.ToString() + "~" + this.NumberOfDevelopment.ToString();
            }
        }
        public string NumberOfOrderWithMerchandiserID
        {
            get
            {
                return this.MerchandiserID.ToString() + "~" + this.NumberOfOrder.ToString();
            }
        }
        public string TotalOrderQtyWithMerchandiserID
        {
            get
            {
                return this.MerchandiserID.ToString() + "~" + this.TotalOrderQty.ToString();
            }
        }
        public string NumberOfBuyerWithMerchandiserID
        {
            get
            {
                return this.MerchandiserID.ToString() + "~" + this.NumberOfBuyer.ToString();
            }
        }
        public string NumberOfFactoryWithMerchandiserID
        {
            get
            {
                return this.MerchandiserID.ToString() + "~" + this.NumberOfFactory.ToString();
            }
        }
        public string TaskPendingWithMerchandiserID
        {
            get
            {
                return this.MerchandiserID.ToString() + "~" + this.TaskPending.ToString();
            }
        }
        public string RegularTaskWithMerchandiserID
        {
            get
            {
                return this.MerchandiserID.ToString() + "~" + this.RegularTask.ToString();
            }
        }
        public string NumberOfTAPWithMerchandiserID
        {
            get
            {
                return this.MerchandiserID.ToString() + "~" + this.NumberOfTAP.ToString() + "~" + this.NumberOfTAP + " Of " + this.NumberOfOrder;
            }
        }
      
        public List<ManagementDashboard> ManagementDashboards { get; set; }
        #endregion

        #region Functions

        public static List<ManagementDashboard> Gets(string sSQL, int BusinessSessionID, long nUserID)
        {
            return ManagementDashboard.Service.Gets(sSQL,BusinessSessionID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IManagementDashboardService Service
        {
            get { return (IManagementDashboardService)Services.Factory.CreateService(typeof(IManagementDashboardService)); }
        }
        #endregion
    }

    #endregion

    #region IManagementDashboard interface

    public interface IManagementDashboardService
    {
        List<ManagementDashboard> Gets(string sSQL, int BusinessSessionID, Int64 nUserID);

    }
    #endregion
}
