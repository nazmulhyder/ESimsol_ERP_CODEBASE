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
    #region YarnRequisitionProduct
    public class YarnRequisitionProduct : BusinessObject
    {
        public YarnRequisitionProduct()
        {
            YarnRequisitionProductID = 0;
            YarnRequisitionID = 0;
            YarnID = 0;
            YarnCount = "";
            RequisitionQty = 0;
            MUnitID = 0;
            YarnCode = "";
            YarnName = "";
            MUSymbol = "";
            ErrorMessage = "";
        }

        #region Property
        public int YarnRequisitionProductID { get; set; }
        public int YarnRequisitionID { get; set; }
        public int YarnID { get; set; }
        public string YarnCount { get; set; }
        public double RequisitionQty { get; set; }
        public int MUnitID { get; set; }
        public string YarnCode { get; set; }
        public string YarnName { get; set; }
        public string MUSymbol { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        #endregion

        #region Functions
        public static List<YarnRequisitionProduct> Gets(long nUserID)
        {
            return YarnRequisitionProduct.Service.Gets(nUserID);
        }
        public static List<YarnRequisitionProduct> Gets(string sSQL, long nUserID)
        {
            return YarnRequisitionProduct.Service.Gets(sSQL, nUserID);
        }
        public static List<YarnRequisitionProduct> Gets(int nYarnRequisitionID, long nUserID)
        {
            return YarnRequisitionProduct.Service.Gets(nYarnRequisitionID, nUserID);
        }
        public YarnRequisitionProduct Get(int id, long nUserID)
        {
            return YarnRequisitionProduct.Service.Get(id, nUserID);
        }
        public YarnRequisitionProduct Save(long nUserID)
        {
            return YarnRequisitionProduct.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return YarnRequisitionProduct.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IYarnRequisitionProductService Service
        {
            get { return (IYarnRequisitionProductService)Services.Factory.CreateService(typeof(IYarnRequisitionProductService)); }
        }
        #endregion
    }
    #endregion

    #region IYarnRequisitionProduct interface
    public interface IYarnRequisitionProductService
    {
        YarnRequisitionProduct Get(int id, Int64 nUserID);
        List<YarnRequisitionProduct> Gets(Int64 nUserID);
        List<YarnRequisitionProduct> Gets(int nYarnRequisitionID, Int64 nUserID);
        List<YarnRequisitionProduct> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        YarnRequisitionProduct Save(YarnRequisitionProduct oYarnRequisitionProduct, Int64 nUserID);
    }
    #endregion
}