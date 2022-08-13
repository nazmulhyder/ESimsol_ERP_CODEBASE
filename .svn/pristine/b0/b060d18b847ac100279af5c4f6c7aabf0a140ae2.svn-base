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

    #region ProductionRecipe
    public class ProductionRecipe : BusinessObject
    {
        public ProductionRecipe()
        {
            ProductionRecipeID = 0;
            ProductionSheetID = 0;
            ProductID = 0;
            QtyInPercent = 0;
            MUnitID = 0;
            RequiredQty = 0;
            Remarks = "";
            ProductCode = "";
            ProductName = "";
            ProductUnitID = 0;
            ProductMUName = "";
            MUName = "";
            ErrorMessage = "";
        }

        #region Property
        public int ProductionRecipeID { get; set; }
        public int ProductionSheetID { get; set; }
        public int ProductID { get; set; }
        public int QtyInPercent { get; set; }
        public double RequiredQty { get; set; }

        public string Remarks { get; set; }
        public int ProductUnitID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductMUName { get; set; }
        public string MUName { get; set; }
        public int MUnitID { get; set; }

        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property


        public string QtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.RequiredQty);
            }
        }

        #endregion

        #region Functions
        public static List<ProductionRecipe> Gets(int nProductionSheetID, long nUserID)
        {
            return ProductionRecipe.Service.Gets(nProductionSheetID, nUserID);
        }

        public static List<ProductionRecipe> Gets(string sSQL, long nUserID)
        {
            return ProductionRecipe.Service.Gets(sSQL, nUserID);
        }
        public ProductionRecipe Get(int id, long nUserID)
        {
            return ProductionRecipe.Service.Get(id, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IProductionRecipeService Service
        {
            get { return (IProductionRecipeService)Services.Factory.CreateService(typeof(IProductionRecipeService)); }
        }
        #endregion
    }
    #endregion

    #region IProductionRecipe interface
    public interface IProductionRecipeService
    {
        ProductionRecipe Get(int id, Int64 nUserID);
        List<ProductionRecipe> Gets(int nORSID, Int64 nUserID);

        List<ProductionRecipe> Gets(string sSQL, Int64 nUserID);

    }
    #endregion


}
