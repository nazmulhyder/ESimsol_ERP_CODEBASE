using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region DUDeliveryChallanImage
    public class DUDeliveryChallanImage : BusinessObject
    {
        public DUDeliveryChallanImage()
        {
            DUDeliveryChallanImageID = 0;
            DUDeliveryChallanID = 0;
            Name = "";
            ContractNo = "";
            Note = "";
            Picture = null;
            PictureName = "";
            IsImg = false;
            ByteInString = "";
            ErrorMessage = "";
        }

        #region Properties
        public int DUDeliveryChallanImageID { get; set; }
        public int DUDeliveryChallanID { get; set; }
        public string Name { get; set; }
        public string ContractNo { get; set; }
        public string Note { get; set; }
        public string PictureName { get; set; }
        public byte[] Picture { get; set; }
        public string ByteInString { get; set; }
        public bool IsImg { get; set; }
        public System.Drawing.Image Image { get; set; }
        
        public string ErrorMessage { get; set; }

        #endregion
        

        #region Functions
        public static List<DUDeliveryChallanImage> Gets(Int64 nUserID)
        {
            return DUDeliveryChallanImage.Service.Gets(nUserID);
        }
        public static List<DUDeliveryChallanImage> Gets(string sSQL, Int64 nUserID)
        {
            return DUDeliveryChallanImage.Service.Gets(sSQL, nUserID);
        }
        public DUDeliveryChallanImage Get(int id, Int64 nUserID)
        {
            return DUDeliveryChallanImage.Service.Get(id, nUserID);
        }
        public static DUDeliveryChallanImage GetByDeliveryChallan(int id, Int64 nUserID)
        {
            return DUDeliveryChallanImage.Service.GetByDeliveryChallan(id, nUserID);
        }
        public DUDeliveryChallanImage GetByBU(int buid, Int64 nUserID)
        {
            return DUDeliveryChallanImage.Service.GetByBU(buid, nUserID);
        }

        public DUDeliveryChallanImage Save(DUDeliveryChallanImage oDUDeliveryChallanImage, Int64 nUserID)
        {
            return DUDeliveryChallanImage.Service.Save(oDUDeliveryChallanImage, nUserID);
        }
        public string Delete(int id, Int64 nUserID)
        {
            return DUDeliveryChallanImage.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDUDeliveryChallanImageService Service
        {
            get { return (IDUDeliveryChallanImageService)Services.Factory.CreateService(typeof(IDUDeliveryChallanImageService)); }
        }
        #endregion
    }
    #endregion

    #region IDUDeliveryChallanImage interface
    public interface IDUDeliveryChallanImageService
    {
        List<DUDeliveryChallanImage> Gets(Int64 nUserID);
        DUDeliveryChallanImage Get(int id, Int64 nUserID);
        DUDeliveryChallanImage GetByDeliveryChallan(int id, Int64 nUserID);
        DUDeliveryChallanImage GetByBU(int buid, Int64 nUserID);
        List<DUDeliveryChallanImage> Gets(string sSQL, Int64 nUserID);
        DUDeliveryChallanImage Save(DUDeliveryChallanImage oDUDeliveryChallanImage, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
    }
    #endregion
}

