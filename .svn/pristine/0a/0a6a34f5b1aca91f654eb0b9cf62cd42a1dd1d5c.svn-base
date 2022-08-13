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
    #region DyeingSolutionTemplet
    
    public class DyeingSolutionTemplet : BusinessObject
    {
        public DyeingSolutionTemplet()
        {
            DSTID=0;
            DyeingSolutionID=0;
            ProcessID = 0;
            ParentID=0;
            IsDyesChemical=false;
            TempTime="";
            GL=0;
            Percentage=0;
            Note = "";
            ProcessName = "";
            Sequence = 0;
            DyeingSolutionType = EnumDyeingSolutionType.None;
            ErrorMessage="";
            children = new List<DyeingSolutionTemplet>();
            RecipeCalType = EnumDyeingRecipeType.General;
            ProductType = EnumProductNature.Chemical;
        }

        #region Properties
        public int DSTID { get; set; }
        public int DyeingSolutionID { get; set; }
        public int ProcessID { get; set; }        
        public int ParentID { get; set; }
        public bool IsDyesChemical { get; set; }
        public string TempTime { get; set; }
        public double GL { get; set; }
        public EnumProductNature ProductType { get; set; }
        public double Percentage { get; set; }
        public EnumDyeingRecipeType RecipeCalType { get; set; }/// Calculation Type
        public string Note { get; set; }
        public int Sequence { get; set; }
        public string ErrorMessage { get; set; }
        public string GLst
        {
            get
            {
                if (this.GL>0)
                {
                    return this.GL.ToString("0.000");
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        #region Derived Property
        public string DyeingSolutionName { get; set; }
        public string ProcessName { get; set; }
        public EnumDyeingSolutionType DyeingSolutionType { get; set; }
        public List<DyeingSolutionTemplet> children { get; set; }
        #endregion

        #region Functions
        public static DyeingSolutionTemplet Get(int nId, long nUserID)
        {
            return DyeingSolutionTemplet.Service.Get(nId, nUserID);
        }

        public static List<DyeingSolutionTemplet> Gets(long nUserID)
        {
            return DyeingSolutionTemplet.Service.Gets(nUserID);
        }

        public static List<DyeingSolutionTemplet> Gets(string sSQL, long nUserID)
        {
            return DyeingSolutionTemplet.Service.Gets(sSQL, nUserID);
        }

        public DyeingSolutionTemplet IUD(int nDBOperation, long nUserID)
        {
            return DyeingSolutionTemplet.Service.IUD(this, nDBOperation, nUserID);
        }

        public DyeingSolutionTemplet RefreshSequence(long nUserID)
        {
            return DyeingSolutionTemplet.Service.RefreshSequence(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDyeingSolutionTempletService Service
        {
            get { return (IDyeingSolutionTempletService)Services.Factory.CreateService(typeof(IDyeingSolutionTempletService)); }
        }
        #endregion
    }
    #endregion

    #region IDyeingSolutionTemplet interface
    
    public interface IDyeingSolutionTempletService
    {
        DyeingSolutionTemplet Get(int id, long nUserID);
        List<DyeingSolutionTemplet> Gets(long nUserID);
        List<DyeingSolutionTemplet> Gets(string sSQL, long nUserID);
        DyeingSolutionTemplet IUD(DyeingSolutionTemplet oDyeingSolutionTemplet, int nDBOperation, long nUserID);
        DyeingSolutionTemplet RefreshSequence(DyeingSolutionTemplet oDyeingSolutionTemplet, long nUserID);
    }
    #endregion

  
}
