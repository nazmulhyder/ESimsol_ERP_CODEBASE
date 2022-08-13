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
    #region LabdipShade

    public class LabdipShade : BusinessObject
    {

        #region  Constructor
        public LabdipShade()
        {
            LabdipShadeID=0;
            LabdipDetailID=0;
            ShadeID=EnumShade.NA;
            ShadePercentage=0;
            Qty = 0;
            Note="";
            ApproveBy = 0;
            ApproveDate = DateTime.Today;
            LabdipShades = new List<LabdipShade>();
            RecipeDyes = new List<LabdipRecipe>();
            RecipeChemicals = new List<LabdipRecipe>();
            ErrorMessage = "";
        }
        #endregion

        #region Properties

        public int LabdipShadeID { get; set; }
        public int LabdipDetailID { get; set; }
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

        public List<LabdipShade> LabdipShades { get; set; }
        public List<LabdipRecipe> RecipeDyes { get; set; }

        public List<LabdipRecipe> RecipeChemicals { get; set; }

        public string ShadeStr { get{ return this.ShadeID.ToString();} }
        public string ApproveDateStr { get { return this.ApproveDate.ToString("dd MMM yyyy"); } }

        #endregion

        #region Functions

        public LabdipShade IUD(int nDBOperation, long nUserID)
        {
            return LabdipShade.Service.IUD(this, nDBOperation, nUserID);
        }
        public static LabdipShade Get(int nLabdipShadeID, long nUserID)
        {
            return LabdipShade.Service.Get(nLabdipShadeID, nUserID);
        }
        public static List<LabdipShade> Gets(string sSQL, long nUserID)
        {
            return LabdipShade.Service.Gets(sSQL, nUserID);
        }
        public LabdipShade CopyLabdipShade(long nUserID)
        {
            return LabdipShade.Service.CopyLabdipShade(this, nUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static ILabdipShadeService Service
        {
            get { return (ILabdipShadeService)Services.Factory.CreateService(typeof(ILabdipShadeService)); }
        }
        #endregion
    }
    #endregion



    #region ILabdipShade interface
    public interface ILabdipShadeService
    {
        LabdipShade IUD(LabdipShade oLabdipShade, int nDBOperation, long nUserID);

        LabdipShade Get(int nLabdipShadeID, long nUserID);

        List<LabdipShade> Gets(string sSQL, long nUserID);

        LabdipShade CopyLabdipShade(LabdipShade oLabdipShade, long nUserID);

        
    }
    #endregion
}