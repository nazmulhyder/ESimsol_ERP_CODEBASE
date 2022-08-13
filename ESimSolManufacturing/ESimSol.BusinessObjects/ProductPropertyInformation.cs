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

    #region ProductPropertyInformation
    public class ProductPropertyInformation : BusinessObject
    {
        public ProductPropertyInformation()
        {
            ProductPropertyInfoID = 0;
            ProductID = 0;
            BUID = 0;
            PropertyValueID = 0;
            ProductName = "";
            ValueOfProperty = "";
            BUName = "";
            BUShortName = "";
            PropertyType = EnumPropertyType.None;
            PropertyTypeInInt = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int ProductPropertyInfoID { get; set; }
        public int ProductID { get; set; }
        public int PropertyValueID { get; set; }
        public int BUID { get; set; }
        public string ProductName { get; set; }
        public string ValueOfProperty { get; set; }
        public EnumPropertyType PropertyType { get; set; }
        public int PropertyTypeInInt { get; set; }
        public string BUName { get; set; }
        public string BUShortName { get; set; }


        #endregion

        #region Derived Property
        public List<ProductPropertyInformation> ProductPropertyInformations { get; set; }
        public string SelecetedProduct { get; set; }
        
        public string PropertyTypeInString
        {
            get
            {
                return EnumObject.jGet(this.PropertyType);
            }
        }

        public string ErrorMessage { get; set; }
        #endregion

        #region Functions
        public ProductPropertyInformation Get(int id, int nUserID)
        {
            return ProductPropertyInformation.Service.Get(id, nUserID);
        }
        public ProductPropertyInformation Save(int nUserID)
        {
            return ProductPropertyInformation.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return ProductPropertyInformation.Service.Delete(id, nUserID);
        }
        public static List<ProductPropertyInformation> Gets(int nUserID)
        {
            return ProductPropertyInformation.Service.Gets(nUserID);
        }
        public static List<ProductPropertyInformation> Gets(int nProductID, int BUID, int nUserID)
        {
            return ProductPropertyInformation.Service.Gets(nProductID,BUID, nUserID);
        }
        public static List<ProductPropertyInformation> Gets(string sSQL, int nUserID)
        {
            return ProductPropertyInformation.Service.Gets(sSQL, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IProductPropertyInformationService Service
        {
            get { return (IProductPropertyInformationService)Services.Factory.CreateService(typeof(IProductPropertyInformationService)); }
        }
      
        #endregion
    }
    #endregion



    #region IProductPropertyInformation interface
    public interface IProductPropertyInformationService
    {
        ProductPropertyInformation Get(int id, int nUserID);
        List<ProductPropertyInformation> Gets(int nUserID);
        List<ProductPropertyInformation> Gets(int nProductID,int BUID,int nUserID);
        List<ProductPropertyInformation> Gets(string sSQL, int nUserID);
        string Delete(int id, int nUserID);
        ProductPropertyInformation Save(ProductPropertyInformation oProductPropertyInformation, int nUserID);
    }
    #endregion
}