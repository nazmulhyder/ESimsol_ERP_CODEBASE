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
    #region FabricSticker
    public class FabricSticker
    {
        public FabricSticker()
        {
            FabricStickerID = 0;
            Title = "";
            FabricMillName = "";
            FabricArticleNo = "";
            Composition = 0;
            Construction = "";
            Width = "";
            Weight = "";
            FinishType = 0;
            Email = "";
            Phone = "";
            
            StickerDate = DateTime.Today;
            Price = 0;
            PrintCount = 0;
            ProductName = "";
            ErrorMessage = "";
            FabricStickers = new List<FabricSticker>();
            FabricDesignID = 0;
            FabricDesignName = "";
            FabricWeave = 0;
            FabricWeaveName = "";
        }

        #region Properties
        public int FabricStickerID { get; set; }
        public string Title { get; set; }
        public string FabricMillName { get; set; }
        public string FabricArticleNo { get; set; }
        public int Composition { get; set; }
        public string Construction { get; set; }
        public string Width { get; set; }
        public string Weight { get; set; }
        public int FinishType { get; set; }
        public string FinishTypeName { get; set; }
        public int FinishTypeInInt { get; set; }
        public DateTime StickerDate { get; set; }
        public double Price { get; set; }
        public int PrintCount { get; set; }
        public string ProductName { get; set; }
        public string ErrorMessage { get; set; }
        public int FabricDesignID { get; set; }
        public string FabricDesignName { get; set; }
        public int FabricWeave { get; set; }
        public string FabricWeaveName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        #endregion

        #region Derived Property
        public List<FabricSticker> FabricStickers { get; set; }
        public Company Company { get; set; }
        #endregion


        #region Functions
        public static List<FabricSticker> Gets(long nUserID)
        {
            return FabricSticker.Service.Gets(nUserID);
        }
        public static List<FabricSticker> Gets(string sSQL, long nUserID)
        {
            return FabricSticker.Service.Gets(sSQL, nUserID);
        }
        public FabricSticker Get(int nId, long nUserID)
        {
            return FabricSticker.Service.Get(nId, nUserID);
        }
        public FabricSticker Save(long nUserID)
        {
            return FabricSticker.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricSticker.Service.Delete(nId, nUserID);
        }
        #endregion

        #region Derived Property

        public string StickerDateSt
        {
            get { return this.StickerDate.ToString("dd MMM yyyy"); }
        }
        //public string FinishTypeSt
        //{
        //    get
        //    {
        //        return EnumFinishTypeObj.GetEnumFinishTypeObjs(this.FinishType);
        //    }
        //}
        public string PriceSt
        {
            get
            {
                return Global.MillionFormat(this.Price);
            }
        }
        #endregion

        #region ServiceFactory
        internal static IFabricStickerService Service
        {
            get { return (IFabricStickerService)Services.Factory.CreateService(typeof(IFabricStickerService)); }
        }
        #endregion
    }
    #endregion

    #region IFabric interface
    public interface IFabricStickerService
    {
        FabricSticker Get(int id, long nUserID);
        List<FabricSticker> Gets(long nUserID);
        List<FabricSticker> Gets(string sSQL, long nUserID);
        string Delete(int id, long nUserID);
        FabricSticker Save(FabricSticker oFabricSticker, long nUserID);
    }
    #endregion
}
