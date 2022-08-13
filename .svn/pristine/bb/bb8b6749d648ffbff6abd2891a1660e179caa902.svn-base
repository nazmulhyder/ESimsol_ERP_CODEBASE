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
    #region RSInQCManage
    public class RSInQCManage : BusinessObject
    {
        public RSInQCManage()
        {
            RSInQCDetailID = 0;
            RouteSheetID = 0;
            RSInQCSetupID = 0;
            ManageDate=DateTime.MinValue;
            ProductID = 0;
            MUnitID = 0;
            ManagedLotID = 0;
            Qty = 0;
            WorkingUnitID = 0;
            Note = string.Empty;
            WUName = string.Empty;
            DyeingType= string.Empty;
            OrderTypeSt = string.Empty;
            LotNo = string.Empty;
            QtyRS = 0;
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
        public int WorkingUnitID { get; set; }
        public double Qty { get; set; }
        public double QtyRS { get; set; }
        public string RouteSheetNo { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string LotNo { get; set; }
        public string OrderNo { get; set; }
        public string NoCode { get; set; }
        public string OrderTypeSt { get; set; }
        public string DyeingType { get; set; }
        public string Note { get; set; }
        public string WUName { get; set; }
        public int OrderType { get; set; }
        public string ErrorMessage { get; set; }
        private string _sOrderNoFull = "";
        public string OrderNoFull
        {
            get
            {
                _sOrderNoFull = this.NoCode + this.OrderNo;
                return _sOrderNoFull;
            }
        }

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
        public static RSInQCManage Get(int nId, long nUserID)
        {
            return RSInQCManage.Service.Get(nId, nUserID);
        }

        public static List<RSInQCManage> Gets(string sSQL, long nUserID)
        {
            return RSInQCManage.Service.Gets(sSQL, nUserID);
        }

        public static List<RSInQCManage> IUD(List<RSInQCManage> oRSInQCManages, long nUserID)
        {
            return RSInQCManage.Service.IUD(oRSInQCManages, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IRSInQCManageService Service
        {
            get { return (IRSInQCManageService)Services.Factory.CreateService(typeof(IRSInQCManageService)); }
        }
        #endregion
    }
    #endregion

    #region IRSInQCManage interface

    public interface IRSInQCManageService
    {
        RSInQCManage Get(int id, long nUserID);
        List<RSInQCManage> Gets(string sSQL, long nUserID);
        List<RSInQCManage> IUD(List<RSInQCManage> oRSInQCManages, long nUserID);
    }
    #endregion
}
