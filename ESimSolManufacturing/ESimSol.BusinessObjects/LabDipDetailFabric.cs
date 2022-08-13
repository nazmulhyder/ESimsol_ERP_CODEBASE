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
    #region LabDipDetailFabric

    public class LabDipDetailFabric : BusinessObject
    {
        public LabDipDetailFabric()
        {
            LDFID = 0;
            LabDipID = 0;
            FabricID = 0;
            ProductID = 0;
            FSCDetailID = 0;
            LabDipDetailID = 0;
            FabricNo = "";
            ProductName = "";
            ColorName = "";
            WarpWeftType= EnumWarpWeft.None;
            Construction = "";
            ActualConstruction = "";
            IsForAll = false;
            ErrorMessage = "";
            Params = "";
            PantonNo = "";
            ReviseNo = 0;
            RefNo = "";
            CellRowSpans = new List<CellRowSpan>();
        }

        #region Properties
        public int LDFID { get; set; }
        public int LabDipID { get; set; }
        public int LabDipDetailID { get; set; }
        public int FabricID { get; set; }
        public int ProductID { get; set; }
        public int FSCDetailID { get; set; }
        public string ProductName { get; set; }
        public string FabricNo { get; set; }
        public string Construction { get; set; }
        public string ActualConstruction { get; set; }
        public string BuyerReference { get; set; }
        public string SCNoFull { get; set; }
        public string ExeNo { get; set; }
        public int OrderType { get; set; }
        public string LDNo { get; set; }
        public string ColorName { get; set; }
        public string ColorNo { get; set; }
        public string LabDipNo { get; set; }
        public string RefNo { get; set; }
        public EnumWarpWeft WarpWeftType { get; set; }
        public string PantonNo { get; set; }
        public int TwistedGroup { get; set; }
        public int ReviseNo { get; set; }
        public string Remarks { get; set; }

        #region Derived Property
        public string WarpWeftTypeSt { get { return EnumObject.jGet(this.WarpWeftType); } }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public bool IsForAll { get; set; }

        public List<CellRowSpan> CellRowSpans { get; set; }
        #endregion

        #endregion

        #region Functions
        public LabDipDetailFabric IUD(int nDBOperation, int nUserID)
        {
            return LabDipDetailFabric.Service.IUD(this, nDBOperation, nUserID);
        }
        public static LabDipDetailFabric Get(int nLabDipDetailFabricID, int nUserID)
        {
            return LabDipDetailFabric.Service.Get(nLabDipDetailFabricID, nUserID);
        }
        public static List<LabDipDetailFabric> Gets(string sSQL, int nUserID)
        {
            return LabDipDetailFabric.Service.Gets(sSQL, nUserID);
        }
        public static List<LabDipDetailFabric> MakeTwistedGroup(string sLabDipDetailID, int nLabDipID, int nTwistedGroup, int nParentID, int nDBOperation, int nUserID)
        {
            return LabDipDetailFabric.Service.MakeTwistedGroup(sLabDipDetailID, nLabDipID, nTwistedGroup, nParentID, nDBOperation, nUserID);
        }
      
        #endregion

        #region ServiceFactory
        internal static ILabDipDetailFabricService Service
        {
            get { return (ILabDipDetailFabricService)Services.Factory.CreateService(typeof(ILabDipDetailFabricService)); }
        }
        #endregion





    }
    #endregion

    #region ILabDipDetailFabric interface

    public interface ILabDipDetailFabricService
    {
        LabDipDetailFabric IUD(LabDipDetailFabric oLabDipDetailFabric, int nDBOperation, int nUserID);
        LabDipDetailFabric Get(int nID, int nUserID);
        List<LabDipDetailFabric> Gets(string sSQL, int nUserID);
        List<LabDipDetailFabric> MakeTwistedGroup(string sLabDipDetailID, int nLabDipID, int nTwistedGroup, int nParentID, int nDBOperation, int nUserID);
    }
    #endregion
}