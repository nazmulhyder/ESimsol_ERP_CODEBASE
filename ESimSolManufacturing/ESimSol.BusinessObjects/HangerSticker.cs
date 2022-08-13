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
    public class HangerSticker
    {
        public HangerSticker()
        {
            HangerStickerID = 0;
            ART = "";
            Supplier = "";
            Composition = "";
            Construction = "";
            Finishing = "";
            MOQ = "";
            Remarks = "";
            Price = 0;
            Date = "";
            Width = "";
            ErrorMessage = "";
            Params = "";
            PrintCount = 6;
            PrintCopy = 1;
        }
        #region Derived Properties
        public int HangerStickerID{get; set;}
        public string  ART{get; set;}
        public string Supplier{get; set;}
        public string Composition{get; set;}
        public string Construction{get; set;}
        public string Finishing{get; set;}
        public string MOQ{get; set;}
        public string Remarks{get; set;}
        public double Price{get; set;}
        public string Date { get; set; }
        public string Width { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public int FSCDID { get; set; }
        public int PrintCount { get; set; }
        public int PrintCopy { get; set; }
        

        #endregion
        #region Functions
        public static List<HangerSticker> Gets(long nUserID)
        {
            return HangerSticker.Service.Gets(nUserID);
        }
        public static List<HangerSticker> Gets(string sSQL, long nUserID)
        {
            return HangerSticker.Service.Gets(sSQL, nUserID);
        }
        public HangerSticker Get(int nId, long nUserID)
        {
            return HangerSticker.Service.Get(nId, nUserID);
        }
        public HangerSticker Save(long nUserID)
        {
            return HangerSticker.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return HangerSticker.Service.Delete(nId, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IHangerStickerService Service
        {
            get { return (IHangerStickerService)Services.Factory.CreateService(typeof(IHangerStickerService)); }
        }
        #endregion

    }
    #region IFabric interface
    public interface IHangerStickerService
    {
        HangerSticker Get(int id, long nUserID);
        List<HangerSticker> Gets(long nUserID);
        List<HangerSticker> Gets(string sSQL, long nUserID);
        string Delete(int id, long nUserID);
        HangerSticker Save(HangerSticker oFabricSticker, long nUserID);
    }
    #endregion
}
