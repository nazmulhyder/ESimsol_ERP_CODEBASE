using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region FNOrderFabricTransfer
    public class FNOrderFabricTransfer
    {
        public FNOrderFabricTransfer()
        {
            FNOrderFabricTransferID = 0;          
            FSCDID_From = 0;
            FSCDID_To = 0;
            FNOrderFabricReceiveID_From = 0;
            FNOrderFabricReceiveID_To = 0;
            Qty = 0;
            LastUpdateDateTime = DateTime.Now;
            ErrorMessage = "";
            LastUpdateByName = "";
            Dispo_From = "";
            Dispo_To = "";
            Lot_From = "";
            Lot_To = "";
            Fabric_From = "";

        }
        #region Property
        public int FNOrderFabricTransferID { get; set; }
        public int FSCDID_From { get; set; }
        public int FSCDID_To{ get; set; }
        public int FNOrderFabricReceiveID_From { get; set; }
        public int FNOrderFabricReceiveID_To { get; set; }
        public int LastUpdateBy { get; set; }
        public EnumFBQCGrade QCGradeType { get; set; }
        public int SLNo { get; set; }
        public double Qty { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string Dispo_From { get; set; }
        public string Dispo_To { get; set; }
        public string Lot_From { get; set; }
        public string Lot_To { get; set; }
        public string Fabric_From { get; set; }
        public string ErrorMessage { get; set; }
        public string LastUpdateByName { get; set; }

        #endregion
        #region Derived Property      
        public string LastUpdateDateTimeInString
        {
            get
            {
                return LastUpdateDateTime.ToString("dd MMM yyyy");
            }
        }

        #endregion
        #region Functions
        public static List<FNOrderFabricTransfer> Gets(long nUserID)
        {
            return FNOrderFabricTransfer.Service.Gets(nUserID);
        }
        public static List<FNOrderFabricTransfer> Gets(string sSQL, long nUserID)
        {
            return FNOrderFabricTransfer.Service.Gets(sSQL, nUserID);
        }
        public FNOrderFabricTransfer Get(int id, long nUserID)
        {
            return FNOrderFabricTransfer.Service.Get(id, nUserID);
        }
        public FNOrderFabricTransfer Save(long nUserID)
        {
            return FNOrderFabricTransfer.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FNOrderFabricTransfer.Service.Delete(id, nUserID);
        }
        public List<FNOrderFabricTransfer> SaveList(List<FNOrderFabricTransfer> oFNOrderFabricTransfers, long nUserID)
        {
            return FNOrderFabricTransfer.Service.SaveList(oFNOrderFabricTransfers, nUserID);
        }
        public List<FNOrderFabricTransfer> ReturnFabrics(List<FNOrderFabricTransfer> oFNOrderFabricTransfers, long nUserID)
        {
            return FNOrderFabricTransfer.Service.ReturnFabrics(oFNOrderFabricTransfers, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IFNOrderFabricTransferService Service
        {
            get { return (IFNOrderFabricTransferService)Services.Factory.CreateService(typeof(IFNOrderFabricTransferService)); }
        }
        #endregion
    }
    #endregion
    #region IFNOrderFabricTransfer interface
    public interface IFNOrderFabricTransferService
    {
        FNOrderFabricTransfer Get(int id, Int64 nUserID);
        List<FNOrderFabricTransfer> Gets(Int64 nUserID);
        List<FNOrderFabricTransfer> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FNOrderFabricTransfer Save(FNOrderFabricTransfer oFNOrderFabricTransfer, Int64 nUserID);
        List<FNOrderFabricTransfer> SaveList(List<FNOrderFabricTransfer> oFNOrderFabricTransfers, Int64 nUserID);
        List<FNOrderFabricTransfer> ReturnFabrics(List<FNOrderFabricTransfer> oFNOrderFabricTransfers, Int64 nUserID);
    }
    #endregion
}
