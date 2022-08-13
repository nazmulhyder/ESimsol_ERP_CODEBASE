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
    #region FNLabdipShade

    public class FNLabdipShade : BusinessObject
    {

        #region  Constructor
        public FNLabdipShade()
        {
            FNLabdipShadeID=0;
            FNLabDipDetailID=0;
            ShadeID=EnumShade.NA;
            ShadePercentage=0;
            Qty = 0;
            Note="";
            ApproveBy = 0;
            ApproveDate = DateTime.Today;
            FNLabdipShades = new List<FNLabdipShade>();
            RecipeDyes = new List<FNLabdipRecipe>();
            RecipeChemicals = new List<FNLabdipRecipe>();
            ErrorMessage = "";
        }
        #endregion

        #region Properties
        public int FNLabdipShadeID { get; set; }
        public int FNLabDipDetailID { get; set; }
        public EnumShade ShadeID { get; set; }
        public double ShadePercentage { get; set; }
        public double Qty { get; set; }
        public string Note { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived properties

        public string ApproveByName { get; set; }

        public List<FNLabdipShade> FNLabdipShades { get; set; }
        public List<FNLabdipRecipe> RecipeDyes { get; set; }

        public List<FNLabdipRecipe> RecipeChemicals { get; set; }

        public string ShadeStr { get{ return this.ShadeID.ToString();} }
        public string ApproveDateStr { get { return this.ApproveDate.ToString("dd MMM yyyy"); } }

        #endregion

        #region Functions

        public FNLabdipShade IUD(int nDBOperation, long nUserID)
        {
            return FNLabdipShade.Service.IUD(this, nDBOperation, nUserID);
        }
        public static FNLabdipShade Get(int nFNLabdipShadeID, long nUserID)
        {
            return FNLabdipShade.Service.Get(nFNLabdipShadeID, nUserID);
        }
        public static List<FNLabdipShade> Gets(string sSQL, long nUserID)
        {
            return FNLabdipShade.Service.Gets(sSQL, nUserID);
        }
        public FNLabdipShade CopyFNLabdipShade(long nUserID)
        {
            return FNLabdipShade.Service.CopyFNLabdipShade(this, nUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static IFNLabdipShadeService Service
        {
            get { return (IFNLabdipShadeService)Services.Factory.CreateService(typeof(IFNLabdipShadeService)); }
        }
        #endregion
    }
    #endregion



    #region IFNLabdipShade interface
    public interface IFNLabdipShadeService
    {
        FNLabdipShade IUD(FNLabdipShade oFNLabdipShade, int nDBOperation, long nUserID);
        FNLabdipShade Get(int nFNLabdipShadeID, long nUserID);
        List<FNLabdipShade> Gets(string sSQL, long nUserID);
        FNLabdipShade CopyFNLabdipShade(FNLabdipShade oFNLabdipShade, long nUserID);

        
    }
    #endregion
}