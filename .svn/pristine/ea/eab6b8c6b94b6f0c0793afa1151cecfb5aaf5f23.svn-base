using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region RSInQCDetail
    public class RSInQCDetail : BusinessObject
    {
        public RSInQCDetail()
        {
            RSInQCDetailID = 0;
            RouteSheetID = 0;
            RSInQCSetupID = 0;
            ManageDate=DateTime.MinValue;
            ProductID = 0;
            MUnitID = 0;
            ManagedLotID = 0;
            DyeingOrderDetailID = 0;
            Qty = 0;
            Note = string.Empty;
            ErrorMessage = "";
        }

        #region Properties
        public int RSInQCDetailID { get; set; }
        public int RouteSheetID { get; set; }
        public int RSInQCSetupID { get; set; }
        public DateTime ManageDate { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public int ManagedLotID { get; set; }
        public double Qty { get; set; }
        public string Note { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derive Properties
        public string QCSetupName { get; set; }
        public EnumYarnType YarnType { get; set; }
        public string ManageDateStr
        {
            get
            {
                return (this.ManageDate == DateTime.MinValue) ? "" : ManageDate.ToString("dd MMM yyyy hh:mm");
            }

        }

        #endregion

        #region Functions
        public static RSInQCDetail Get(int nId, long nUserID)
        {
            return RSInQCDetail.Service.Get(nId, nUserID);
        }

        public static List<RSInQCDetail> Gets(string sSQL, long nUserID)
        {
            return RSInQCDetail.Service.Gets(sSQL, nUserID);
        }

        public RSInQCDetail IUD(int nDBOperation, long nUserID)
        {
            return RSInQCDetail.Service.IUD(this, nDBOperation, nUserID);
        }
        public static List<RSInQCDetail> SaveAll(List<RSInQCDetail> oRSInQCDetails, long nUserID)
        {
            return RSInQCDetail.Service.SaveAll(oRSInQCDetails, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IRSInQCDetailService Service
        {
            get { return (IRSInQCDetailService)Services.Factory.CreateService(typeof(IRSInQCDetailService)); }
        }
        #endregion

    }
    #endregion

    #region IRSInQCDetail interface

    public interface IRSInQCDetailService
    {
        RSInQCDetail Get(int id, long nUserID);
        List<RSInQCDetail> Gets(string sSQL, long nUserID);
        RSInQCDetail IUD(RSInQCDetail oRSInQCDetail, int nDBOperation, long nUserID);
        List<RSInQCDetail> SaveAll(List<RSInQCDetail> oRSInQCDetails, Int64 nUserID);
    }
    #endregion
}
