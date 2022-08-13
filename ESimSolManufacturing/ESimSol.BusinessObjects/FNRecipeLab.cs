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
    #region FNRecipeLab

    public class FNRecipeLab : BusinessObject
    {
        public FNRecipeLab()
        {
            FNRecipeLabID = 0;
            FNLDDID = 0;
            FNTPID = 0;
            ProductType = EnumProductType.None;
            ProductTypeInInt = 0;
            ProductID = 0;
            GL = 0.0;
            QtyColor = 0.0;
            Qty = 0.0;
            BathSize = 0.0;
            Note = "";
            PadderPressure = "";
            Temp = "";
            Speed = "";
            PH = "";
            Flem = "";
            CausticStrength = "";
            ProductName = "";
            ProductCode = "";
            IsProcess = false;
            ErrorMessage = "";
            FNTreatment = 0;
            FNProcess = "";
            Code = "";
            PrepareByName = "";
            ShadeID = EnumShade.A;
        }

        #region Properties
        public int FNRecipeLabID { get; set; }
        public int FNLDDID { get; set; }
        public int FNTPID { get; set; }
        public EnumProductType ProductType { get; set; }
        public int ProductID { get; set; }
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
        public bool IsProcess { get; set; }
        public string CausticStrength { get; set; }
        public EnumShade ShadeID { get; set; }
        public int FNTreatment { get; set; }
        public string Code { get; set; }
        public string FNProcess { get; set; }
        public string PrepareByName { get; set; }
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
        public string ShadeStr
        {
            get
            {
                return this.ShadeID.ToString();
            }
        }

        #endregion

        #region Functions

        public static List<FNRecipeLab> Gets(long nUserID)
        {
            return FNRecipeLab.Service.Gets(nUserID);
        }
        public static List<FNRecipeLab> Gets(string sSQL, Int64 nUserID)
        {
            return FNRecipeLab.Service.Gets(sSQL, nUserID);
        }

        public FNRecipeLab Get(int nId, long nUserID)
        {
            return FNRecipeLab.Service.Get(nId, nUserID);
        }

        public FNRecipeLab Save(long nUserID)
        {
            return FNRecipeLab.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return FNRecipeLab.Service.Delete(nId, nUserID);
        }
        public string DeleteProcess(int nId, long nUserID)
        {
            return FNRecipeLab.Service.DeleteProcess(nId, nUserID);
        }
        public List<FNRecipeLab> CopyShadeSave(int nFNLDDID, int nShadeID, int nShadeIDCopy, int nFNLabDipDetailID, long nUserID)
        {
            return FNRecipeLab.Service.CopyShadeSave(nFNLDDID, nShadeID, nShadeIDCopy, nFNLabDipDetailID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFNRecipeLabService Service
        {
            get { return (IFNRecipeLabService)Services.Factory.CreateService(typeof(IFNRecipeLabService)); }
        }
        #endregion
    }
    #endregion

    #region IFNRecipeLab interface

    public interface IFNRecipeLabService
    {
        FNRecipeLab Get(int id, long nUserID);
        List<FNRecipeLab> Gets(long nUserID);
        List<FNRecipeLab> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID); 
        FNRecipeLab Save(FNRecipeLab oFNRecipeLab, long nUserID);
        string DeleteProcess(int id, long nUserID);
        List<FNRecipeLab> CopyShadeSave(int nFNLDDID, int nShadeID, int nShadeIDCopy, int nFNLabDipDetailID, long nUserID);

    }
    #endregion
}
