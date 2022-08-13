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
    #region FNRecipe
    
    public class FNRecipe : BusinessObject
    {
        public FNRecipe()
        {
            FNRecipeID = 0;
            FSCDID = 0;
            FNTPID = 0;
            ProductType = EnumProductType.None;
            ProductTypeInInt = 0;
            ProductID = 0;
            GL = 0.0;
            ShadeID = EnumShade.A;
            QtyColor = 0.0;
            Qty = 0.0;
            PrepareByName = "";
            BathSize = 0.0;
            Note = "";
            ProductName = "";
            ProductCode = "";
            PadderPressure = "";
            Temp = "";
            Speed = "";
            PH = "";
            Flem = "";
            CausticStrength = "";
            IsProcess = false;
            FNTreatment = 0;
            Code = "";
            FNProcess = "";
            ErrorMessage = "";
            OrderType = true; // true means Copy From lab Dip
        }

        #region Properties
        public int FNRecipeID { get; set; }
        public int FSCDID { get; set; }
        public int FNTPID { get; set; }
        public EnumProductType ProductType { get; set; }
        public int ProductID { get; set; }
        public string PrepareByName { get; set; }
        public double GL { get; set; }
        public double QtyColor { get; set; }
        public double Qty { get; set; }
        public double BathSize { get; set; }
        public string Note { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int ProductTypeInInt { get; set; }
        public string PadderPressure { get; set; }
        public string Temp { get; set; }
        public string Speed { get; set; }
        public string PH { get; set; }
        public string Flem { get; set; }
        public string CausticStrength { get; set; }
        public bool IsProcess { get; set; }
        public int FNTreatment { get; set; }
        public int FNPBatchID { get; set; }
        public string Code { get; set; }
        public bool OrderType { get; set; }
        public string FNProcess { get; set; }
        public EnumShade ShadeID { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string ProductTypeSt
        {
            get
            {
                return this.ProductType.ToString();
            }
        }
        public string ShadeStr
        {
            get
            {
                return this.ShadeID.ToString();
            }
        }
        public string FNTreatmentSt
        {
            get
            {
                return ((EnumFNTreatment)FNTreatment).ToString();
            }
        }
        public string IsHaveParameter
        {
            get
            {
                if ((this.PadderPressure + this.Temp + this.Speed + this.PH + this.Flem + this.CausticStrength) != "")
                {
                    return "✅";
                }
                else
                {
                    return "⛔";
                }
            }
        }
        #endregion

        #region Functions

        public static List<FNRecipe> Gets(long nUserID)
        {
            return FNRecipe.Service.Gets(nUserID);
        }
        public static List<FNRecipe> Gets(string sSQL, Int64 nUserID)
        {
            return FNRecipe.Service.Gets(sSQL, nUserID);
        }
     
        public FNRecipe Get(int nId, long nUserID)
        {
            return FNRecipe.Service.Get(nId,nUserID);
        }
               
        public FNRecipe Save(long nUserID)
        {
            return FNRecipe.Service.Save(this, nUserID);
        }
        public List<FNRecipe> CopyOrder(int nFNLabDipDetailID, bool IsFromLabDip, int nFSCDID, int nFromFSCDID, int nShadeID, long nUserID)
        {
            return FNRecipe.Service.CopyOrder(nFNLabDipDetailID, IsFromLabDip, nFSCDID, nFromFSCDID, nShadeID, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FNRecipe.Service.Delete(nId, nUserID);
        }
        public string DeleteProcess(int nId, long nUserID)
        {
            return FNRecipe.Service.DeleteProcess(nId, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFNRecipeService Service
        {
            get { return (IFNRecipeService)Services.Factory.CreateService(typeof(IFNRecipeService)); }
        }
        #endregion
    }
    #endregion

    #region IFNRecipe interface
    
    public interface IFNRecipeService
    {
        FNRecipe Get(int id, long nUserID);
        List<FNRecipe> Gets(long nUserID);
        List<FNRecipe> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        FNRecipe Save(FNRecipe oFNRecipe, long nUserID);
        List<FNRecipe> CopyOrder(int nFNLabDipDetailID, bool IsFromLabDip, int nFSCDID, int nFromFSCDID, int nShadeID, long nUserID);
        string DeleteProcess(int id, long nUserID);
    }
    #endregion
}
