
using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region CRWiseSpareParts
    public class CRWiseSpareParts : BusinessObject
    {
        public CRWiseSpareParts()
        {
            CRWiseSparePartsID = 0;
            CRID = 0;
            SparePartsID = 0;
            BUID = 0;
            ReqPartsQty = 0;
            Remarks = "";
            ErrorMessage = "";
            ProductName = "";
            ProductCode = "";
            LastUsage = DateTime.MinValue;
            TotalLotBalance = 0;
        }

        #region Properties
        public int CRWiseSparePartsID { get; set; }
        public int CRID { get; set; }
        public int SparePartsID { get; set; }
        public int BUID { get; set; }
        public double ReqPartsQty { get; set; }
        public string Remarks { get; set; }
        public DateTime LastUsage { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public double TotalLotBalance { get; set; }
        public string LastUsageStr
        {
            get
            {
                if(this.LastUsage == DateTime.MinValue)
                {
                    return " ";
                }
                else
                {
                    return this.LastUsage.ToString("dd MMM yyyy");                
                }
            }
        }
        public string TotalLotBalanceSt
        {
            get
            {
                return Global.MillionFormat(this.TotalLotBalance);
            }
        }
        #endregion

        #region Functions
        public static List<CRWiseSpareParts> Gets(int nUserID)
        {
            return CRWiseSpareParts.Service.Gets(nUserID);
        }
        public static List<CRWiseSpareParts> Gets(int nCRID, int nBUID, int nUserID)
        {
            return CRWiseSpareParts.Service.Gets(nCRID, nBUID, nUserID);
        }
        public static List<CRWiseSpareParts> GetsByNameCRAndBUID(string sName, int nCRID, int nBUID, int nUserID)
        {
            return CRWiseSpareParts.Service.GetsByNameCRAndBUID(sName, nCRID, nBUID, nUserID);
        }
        public static List<CRWiseSpareParts> GetsByNameCRAndBUIDWithLot(string sName, int nCRID, int nBUID, int nUserID)
        {
            return CRWiseSpareParts.Service.GetsByNameCRAndBUIDWithLot(sName, nCRID, nBUID, nUserID);
        }
        public static List<CRWiseSpareParts> Gets(string sSQL, int nUserID)
        {
            return CRWiseSpareParts.Service.Gets(sSQL, nUserID);
        }
        public CRWiseSpareParts Get(int id, int nUserID)
        {
            return CRWiseSpareParts.Service.Get(id, nUserID);
        }
        public CRWiseSpareParts Save(int nUserID)
        {
            return CRWiseSpareParts.Service.Save(this, nUserID);
        }
        public static List<CRWiseSpareParts> SaveFromCopy(List<CRWiseSpareParts> oCRWiseSparePartsss, int nUserID)
        {
            return CRWiseSpareParts.Service.SaveFromCopy(oCRWiseSparePartsss, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return CRWiseSpareParts.Service.Delete(id, nUserID);
        }
        public static List<CRWiseSpareParts> GetsbyName(string sCRWiseSpareParts, int nUserID)
        {
            return CRWiseSpareParts.Service.GetsbyName(sCRWiseSpareParts, nUserID);
        }
        public static List<CRWiseSpareParts> BUWiseGets(int nBUID, int nUserID)
        {
            return CRWiseSpareParts.Service.BUWiseGets(nBUID, nUserID);
        }
        #endregion
                
        #region ServiceFactory
        internal static ICRWiseSparePartsService Service
        {
            get { return (ICRWiseSparePartsService)Services.Factory.CreateService(typeof(ICRWiseSparePartsService)); }
        }
        #endregion
    }
    #endregion

    #region ICRWiseSpareParts interface
    public interface ICRWiseSparePartsService
    {
        List<CRWiseSpareParts> GetsbyName(string sCRWiseSpareParts, int nUserID);
        CRWiseSpareParts Get(int id, int nUserID);
        List<CRWiseSpareParts> Gets(int nUserID);
        List<CRWiseSpareParts> Gets(int nCRID, int nBUID, int nUserID);
        List<CRWiseSpareParts> GetsByNameCRAndBUID(string sName, int nCRID, int nBUID, int nUserID);
        List<CRWiseSpareParts> GetsByNameCRAndBUIDWithLot(string sName, int nCRID, int nBUID, int nUserID);                           
        List<CRWiseSpareParts> Gets(string sSQL, int nUserID);
        string Delete(int id, int nUserID);
        CRWiseSpareParts Save(CRWiseSpareParts oCRWiseSpareParts, int nUserID);
        List<CRWiseSpareParts> SaveFromCopy(List<CRWiseSpareParts> oCRWiseSparePartss, int nUserID);   
        List<CRWiseSpareParts> BUWiseGets(int nBUID, int nUserID);
    }
    #endregion
}