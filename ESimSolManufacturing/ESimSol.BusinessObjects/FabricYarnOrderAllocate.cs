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
    public class FabricYarnOrderAllocate : BusinessObject
    {
        public FabricYarnOrderAllocate()
        {
            FYOAID = 0;
            FYOID = 0;
            WUID = 0;
            LotID = 0;
            Qty = 0;
            Balance = 0;
            ApproveBy = 0;
            ApproveDate = DateTime.MinValue;
            ApproveByName = string.Empty;
            ErrorMessage = string.Empty;
            Params = string.Empty;
            FYOAs = new List<FabricYarnOrderAllocate>();
        }
        #region Properties
        public int FYOAID { get; set; }
        public int FYOID { get; set; }
        public int WUID { get; set; }
        public int LotID { get; set; }
        public double Qty { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }

        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion

        #region dreiverd Properties
        public int FEOID { get; set; }
        public string LotNo { get; set; }
        public double Balance { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string LocationName { get; set; }
        public string OperationUnitName { get; set; }
        public string WUName { get { return this.OperationUnitName +"["+ this.LocationName +"]" ;} }
        public string ApproveByName { get; set; }
        public string MUnit { get; set; }
        public List<FabricYarnOrderAllocate> FYOAs { get; set; }
        #endregion

        #region Functions

        public static FabricYarnOrderAllocate Get(int nFYOAID, long nUserID)
        {
            return FabricYarnOrderAllocate.Service.Get(nFYOAID, nUserID);
        }
        public static List<FabricYarnOrderAllocate> Gets(string sSQL, long nUserID)
        {
            return FabricYarnOrderAllocate.Service.Gets(sSQL, nUserID);
        }
        public FabricYarnOrderAllocate IUD(int nDBOperation, long nUserID)
        {
            return FabricYarnOrderAllocate.Service.IUD(this, nDBOperation, nUserID);
        }
        public static List<FabricYarnOrderAllocate> Approve(List<FabricYarnOrderAllocate> oFYOAs, long nUserID)
        {
            return FabricYarnOrderAllocate.Service.Approve(oFYOAs, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IFabricYarnOrderAllocateService Service
        {
            get { return (IFabricYarnOrderAllocateService)Services.Factory.CreateService(typeof(IFabricYarnOrderAllocateService)); }
        }

        #endregion
    }

    #region  IFabricYarnOrderAllocate interface
    public interface IFabricYarnOrderAllocateService
    {

        FabricYarnOrderAllocate Get(int nFYOAID, Int64 nUserID);
        List<FabricYarnOrderAllocate> Gets(string sSQL, Int64 nUserID);
        FabricYarnOrderAllocate IUD(FabricYarnOrderAllocate oFabricYarnOrderAllocate, int nDBOperation, Int64 nUserID);
        List<FabricYarnOrderAllocate> Approve(List<FabricYarnOrderAllocate> oFYOAs, Int64 nUserID);
    
    }
    #endregion
}
