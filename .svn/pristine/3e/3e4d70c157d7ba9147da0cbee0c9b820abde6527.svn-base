using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region VProduct
    public class VProduct : BusinessObject
    {
        public VProduct()
        {
            VProductID = 0;
            ProductCode = "";
            ProductName = "";
            ShortName = "";
            BrandName = "";
            Remarks = "";
            NameCode = "";
            ErrorMessage = "";
        }

        #region Properties
        public int VProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ShortName { get; set; }
        public string BrandName { get; set; }
        public string Remarks { get; set; }
        public string NameCode { get; set; }
        public string ErrorMessage { get; set; }


        #region Derived Property

        #endregion

        #endregion

        #region Functions
        public static List<VProduct> Gets(Int64 nUserID)
        {
            return VProduct.Service.Gets(nUserID);
        }
        public static List<VProduct> Gets(string sSQL,Int64 nUserID)
        {
            return VProduct.Service.Gets(sSQL, nUserID);
        }
        public VProduct Get(int id, Int64 nUserID)
        {
            return VProduct.Service.Get(id, nUserID);
        }
        public VProduct Save(Int64 nUserID)
        {
            return VProduct.Service.Save(this, nUserID);
        }
        public string Delete(int id, Int64 nUserID)
        {
            return VProduct.Service.Delete(id, nUserID);
        }
        public static List<VProduct> GetsByCodeOrName(VProduct oVProduct, int nUserID)
        {
            return VProduct.Service.GetsByCodeOrName(oVProduct, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IVProductService Service
        {
            get { return (IVProductService)Services.Factory.CreateService(typeof(IVProductService)); }
        }
        #endregion
    }
    #endregion

    #region IVProducts interface
    public interface IVProductService
    {
        VProduct Get(int id, Int64 nUserID);
        List<VProduct> Gets(string sSQL, Int64 nUserID);
        List<VProduct> Gets(Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        VProduct Save(VProduct oVProduct, Int64 nUserID);
        List<VProduct> GetsByCodeOrName(VProduct oVProduct, int nUserID);
    }
    #endregion
}