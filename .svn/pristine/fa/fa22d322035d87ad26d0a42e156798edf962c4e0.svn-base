using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region ContainingProduct
    public class ContainingProduct : BusinessObject
    {
        public ContainingProduct()
        {
            ContainingProductID = 0;
            WorkingUnitID = 0;
            ProductCategoryID = 0;
            Remarks = "";
            LocationName = "";
            OperationUnitName = "";
            WorkingUnitCode = "";
            ProductCategoryName = "";
            ErrorMessage = "";
            ContainingProducts = new List<ContainingProduct>();
            WorkingUnits = new List<WorkingUnit>();
        }
        #region Properties
        public int ContainingProductID { get; set; }
        public int WorkingUnitID { get; set; }
        public int ProductCategoryID { get; set; }
        public string Remarks { get; set; }
        public string LocationName { get; set; }
        public string OperationUnitName { get; set; }
        public string WorkingUnitCode { get; set; }
        public string ProductCategoryName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties        
        public List<WorkingUnit> WorkingUnits { get; set; }
        public List<ContainingProduct> ContainingProducts { get; set; }
        public string WorkingUnitName
        {
            get
            {
                return this.LocationName + "[" + this.OperationUnitName + "]";
            }
        }
        #endregion

        #region Functions
        public ContainingProduct Get(int id, int nUserID)
        {
            return ContainingProduct.Service.Get(id, nUserID);
        }
        public ContainingProduct Save(int nUserID)
        {
            return ContainingProduct.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return ContainingProduct.Service.Delete(id, nUserID);
        }
        public static List<ContainingProduct> Gets(int nUserID)
        {
            return ContainingProduct.Service.Gets(nUserID);
        }
        public static List<ContainingProduct> Gets(string sSQL, int nUserID)
        {
            return ContainingProduct.Service.Gets(sSQL, nUserID);
        }
        public static List<ContainingProduct> GetsByWU(int nWUID, int nUserID)
        {
            return ContainingProduct.Service.GetsByWU(nWUID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IContainingProductService Service
        {
            get { return (IContainingProductService)Services.Factory.CreateService(typeof(IContainingProductService)); }
        }
        #endregion
    }
    #endregion

    #region IContainingProduct interface
    public interface IContainingProductService
    {
        ContainingProduct Get(int id, int nUserID);
        List<ContainingProduct> Gets(int nUserID);
        string Delete(int id, int nUserID);
        ContainingProduct Save(ContainingProduct oContainingProduct, int nUserID);
        List<ContainingProduct> Gets(string sSQL, int nUserID);
        List<ContainingProduct> GetsByWU(int nWUID, int nUserID);
    }
    #endregion
}
