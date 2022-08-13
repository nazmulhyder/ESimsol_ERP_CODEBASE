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
    #region RouteSheetDetail

    public class RouteSheetDetail : BusinessObject
    {
        public RouteSheetDetail()
        {
            RouteSheetDetailID = 0;
            RouteSheetID = 0;
            ProcessID = 0;
            ParentID = 0;
            IsDyesChemical = false;
            TempTime = string.Empty;
            GL = 0;
            Percentage = 0;
            DAdjustment = 0;
            Note = string.Empty;
            BatchManID = 0;
            Equation = string.Empty;
            Sequence = 0;
            ForCotton = false;
            SupportMaterial = false;
            TotalQty = 0;
            AddOneQty = 0;
            AddTwoQty = 0;
            AddThreeQty = 0;
            ReturnQty = 0;
            TotalQtyLotID = 0;
            TotalQtyLotNo = string.Empty;
            AddOneLotID = 0;
            AddOneLotNo = string.Empty;
            AddTwoLotID = 0;
            AddTwoLotNo = string.Empty;
            AddThreeLotID = 0;
            AddThreeLotNo = string.Empty;
            ReturnLotID = 0;
            ReturnLotNo = string.Empty;
            ErrorMessage = string.Empty;
            Params = string.Empty;
            RSDetailAdditonals = new List<RSDetailAdditonal>();
            children = new List<RouteSheetDetail>();
            SuggestLotID = 0;
            SuggestLotNo = "";
            Balance = 0;
            OutState = 0;
            PCategoryID = 0;
            WUID = 0;
            RecipeCalType = EnumDyeingRecipeType.General;
            ProductType = EnumProductNature.Chemical;
        }

        #region Properties
        public int RouteSheetDetailID { get; set; }
        public int RouteSheetID { get; set; }
        public int ProcessID { get; set; }
        public int ParentID { get; set; }
        public bool IsDyesChemical { get; set; }
        public string TempTime { get; set; }
        public double GL { get; set; }
        public double Percentage { get; set; }
        public double DAdjustment { get; set; }
        public string Note { get; set; }
        public int BatchManID { get; set; }
        public string Equation { get; set; }
        public short Sequence { get; set; }
        public bool ForCotton { get; set; }
        public bool SupportMaterial { get; set; }
        public double TotalQty { get; set; }
        public double AddOneQty { get; set; }
        public double AddTwoQty { get; set; }
        public double AddThreeQty { get; set; }
        public double ReturnQty { get; set; }
        public int TotalQtyLotID { get; set; }
        public string TotalQtyLotNo { get; set; }
        public int AddOneLotID { get; set; }
        public string AddOneLotNo { get; set; }
        public int AddTwoLotID { get; set; }
        public string AddTwoLotNo { get; set; }
        public int ReturnLotID { get; set; }
        public string ReturnLotNo { get; set; }
        public int AddThreeLotID { get; set; }
        public string AddThreeLotNo { get; set; }
        public int SuggestLotID { get; set; }
        public string SuggestLotNo { get; set; }
        public double Balance { get; set; }
        public int OutState { get; set; }
        #endregion

        #region Derived Property
        public int PCategoryID { get; set; }
        public int WUID { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public List<RSDetailAdditonal> RSDetailAdditonals { get; set; }
        public List<RouteSheetDetail> children { get; set; }
        public string ProcessName { get; set; }
        public string BatchEmpName { get; set; }
        public double AddOnePercentage { get; set; }
        public double AddTwoPercentage { get; set; }
        public double AddThreePercentage { get; set; }
        public string ProductCode { get; set; }
        public string ProductCategoryName { get; set; }

        public EnumProductNature ProductType { get; set; }/// Dyes Chemical
        public EnumDyeingRecipeType RecipeCalType { get; set; }/// Calculation Type

        public double DeriveGL
        {
            get
            {
                if (this.Percentage > 0)
                {
                    return this.DAdjustment;
                }
                else
                {
                    return this.GL;
                }
            }
        }
        public string TotalQtyStr
        {
            get
            {
                return SIUnitGenerator.Mass(this.TotalQty);
            }
        }
        public string ConvertToStr(double n)
        {
            return SIUnitGenerator.Mass(n);
        }
        public string AddOneQtyStr
        {
            get
            {
                return SIUnitGenerator.Mass(this.AddOneQty);
            }
        }
        public string AddTwoQtyStr
        {
            get
            {
                return SIUnitGenerator.Mass(this.AddTwoQty);
            }
        }
        public string AddThreeQtyStr
        {
            get
            {
                return SIUnitGenerator.Mass(this.AddThreeQty);
            }
        }
        public string ReturnQtyStr
        {
            get
            {
                return SIUnitGenerator.Mass(this.ReturnQty);
            }
        }
        public string GTotalInString
        {
            get
            {
                double nGTotal = 0; //this.TotalQty + this.AddOne + this.AddTwo - this.Return;
                if (this.TotalQty != 0) { nGTotal = nGTotal + this.TotalQty; }
                if (this.AddOneQty != 0) { nGTotal = nGTotal + this.AddOneQty; }
                if (this.AddTwoQty != 0) { nGTotal = nGTotal + this.AddTwoQty; }
                if (this.AddThreeQty != 0) { nGTotal = nGTotal + this.AddThreeQty; }
                if (this.ReturnQty != 0) { nGTotal = nGTotal - this.ReturnQty; }
                return SIUnitGenerator.Mass(nGTotal);
            }
        }

        #endregion

        #region Functions
        public RouteSheetDetail IUD(int nDBOperation, long nUserID)
        {
            return RouteSheetDetail.Service.IUD(this, nDBOperation, nUserID);
        }
        public static RouteSheetDetail Get(int nRouteSheetDetailID, long nUserID)
        {
            return RouteSheetDetail.Service.Get(nRouteSheetDetailID, nUserID);
        }
        public static List<RouteSheetDetail> Gets(string sSQL, long nUserID)
        {
            return RouteSheetDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<RouteSheetDetail> Gets(int nRSID, long nUserID)
        {
            return RouteSheetDetail.Service.Gets(nRSID, nUserID);
        }
        public static List<RouteSheetDetail> IUDTemplate(int nDSID, int nRSID, long nUserID)
        {
            return RouteSheetDetail.Service.IUDTemplate(nDSID, nRSID, nUserID);
        }
        public static List<RouteSheetDetail> IUDTemplateCopyFromRS(int nRSID_CopyFrom, int nRSID, long nUserID)
        {
            return RouteSheetDetail.Service.IUDTemplateCopyFromRS(nRSID_CopyFrom, nRSID, nUserID);
        }
        public RouteSheetDetail Update_RSDetail(long nUserID)
        {
            return RouteSheetDetail.Service.Update_RSDetail(this, nUserID);
        }
        public RouteSheetDetail DyeChemicalOut(long nUserID)
        {
            return RouteSheetDetail.Service.DyeChemicalOut(this, nUserID);
        }
        public RouteSheetDetail RefreshSequence(long nUserID)
        {
            return RouteSheetDetail.Service.RefreshSequence(this, nUserID);
        }
        public static List<RouteSheetDetail> Update(List<RouteSheetDetail> oItems, long nUserID)
        {
            return RouteSheetDetail.Service.Update(oItems, nUserID);
        }
        public static List<RouteSheetDetail> DyeChemicalOut_All(List<RouteSheetDetail> oRouteSheetDetails, long nUserID)
        {

            return RouteSheetDetail.Service.DyeChemicalOut_All(oRouteSheetDetails, nUserID);
        }
        public static List<RouteSheetDetail> DyeChemicalOut_All_V2(List<RSDetailAdditonal> oRSDetailAdditonals, long nUserID)
        {
            return RouteSheetDetail.Service.DyeChemicalOut_All_V2(oRSDetailAdditonals, nUserID);

        }
        public static List<RouteSheetDetail> DyeChemicalOut_All_Return(List<RouteSheetDetail> oRouteSheetDetails, long nUserID)
        {
            return RouteSheetDetail.Service.DyeChemicalOut_All_Return(oRouteSheetDetails, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IRouteSheetDetailService Service
        {
            get { return (IRouteSheetDetailService)Services.Factory.CreateService(typeof(IRouteSheetDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IRouteSheetDetail interface

    public interface IRouteSheetDetailService
    {
        RouteSheetDetail IUD(RouteSheetDetail oRouteSheetDetail, int nDBOperation, long nUserID);
        RouteSheetDetail Get(int nID, long nUserID);
        List<RouteSheetDetail> Gets(string sSQL, long nUserID);
        List<RouteSheetDetail> Gets(int nRSID, long nUserID);
        List<RouteSheetDetail> IUDTemplate(int nDSID, int nRSID, long nUserID);
        List<RouteSheetDetail> IUDTemplateCopyFromRS(int nRSID_CopyFrom, int nRSID, long nUserID);
        RouteSheetDetail DyeChemicalOut(RouteSheetDetail oRouteSheetDetail, long nUserID);
        RouteSheetDetail Update_RSDetail(RouteSheetDetail oRouteSheetDetail, long nUserID);
        RouteSheetDetail RefreshSequence(RouteSheetDetail oRouteSheetDetail, long nUserID);
        List<RouteSheetDetail> Update(List<RouteSheetDetail> oRouteSheetDetails, long nUserID);
        List<RouteSheetDetail> DyeChemicalOut_All(List<RouteSheetDetail> oRouteSheetDetails, long nUserID);
        List<RouteSheetDetail> DyeChemicalOut_All_V2(List<RSDetailAdditonal> oRSDetailAdditonals, long nUserID);
        List<RouteSheetDetail> DyeChemicalOut_All_Return(List<RouteSheetDetail> oRouteSheetDetails, long nUserID);
    }
    #endregion

    static class SIUnitGenerator
    {
        public static string Mass(double nQty)
        {
            if (nQty != 0)
            {
                double nTotalQty = nQty * 1000;
                double nKG = 0;
                double nGM = 0;
                double nGM1 = 0;
                double nGM2 = 0;

                if (nTotalQty >= 1000) { nKG = nTotalQty / 1000; } else nKG = 0;
                //if (nKG >= 1) { nGM1 = nKG; } else nGM1 = nTotalQty;
                nGM1 = nKG - (int)nKG;
                //nGM2 = nGM1 - (int)nKG;
                nGM2 = Math.Round(nGM1, 4);
                //if (nGM2 < 1) { nGM = nGM2 * 1000; } else nGM = nGM2;
                if ((int)nKG >= 1) { nGM = nGM2 * 1000; } else { nGM = nTotalQty; }
                return ( (int)nKG >0 ?  ((int)nKG).ToString() + " Kg " : "") + Global.MillionFormatActualDigit(Math.Round(nGM,4)) + " Gm." ;
            }
            else
            {
                return "";
            }
           
        }
    }

    #region RSDTree
    //this is a derive class that use only for display user menu(j tree tree architecture reference : http://www.jeasyui.com/documentation/index.php# )
    public class RSDTree
    {
        public RSDTree()
        {
            id = 0;
            text = "";
            state = "";
            attributes = "";
            parentid = 0;
            code = "";
            Description = "";

            RouteSheetID = 0;
            ProcessID = 0;
            IsDyesChemical = false;
            TempTime = "";
            GL = 0.0;
            Percentage = 0.0;
            TotalQty = 0.0;
            AddOne = 0.0;
            AddTwo = 0.0;
            AddThree = 0.0;
            Return = 0.0;
            ForCotton = false;
            ErrorMessage = "";
            DeriveGL = 0.0;
            TotalQtyInString = "";
            ProductNameCode = "";
            AddOneInString = "";
            AddTwoInString = "";
            AddThreeInString = "";
            ReturnInString = "";
            GTotalInString = "";
            TotalQtyLotNo = "";
            AddOneLotNo = "";
            AddThreeLotNo = "";
            AddTwoLotNo = "";
            ReturnLotNo = "";
        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public string state { get; set; }           //: node state, 'open' or 'closed', default is 'open'. When set to 'closed', the node have children nodes and will load them from remote site        
        public string attributes { get; set; }      //: custom attributes can be added to a node // attributes set a sting that hold action name & controller namr seperated by '~'
        public int parentid { get; set; }
        public string code { get; set; }
        public string Description { get; set; }

        public int RouteSheetID { get; set; }
        public int ProcessID { get; set; }
        public bool IsDyesChemical { get; set; }
        public string TempTime { get; set; }
        public double GL { get; set; }
        public double Percentage { get; set; }
        public double TotalQty { get; set; }
        public double AddOne { get; set; }
        public double AddTwo { get; set; }
        public double AddThree { get; set; }
        public double Return { get; set; }
        public bool ForCotton { get; set; }
        public string ErrorMessage { get; set; }
        public double DeriveGL { get; set; }
        public string TotalQtyInString { get; set; }
        public string ProductNameCode { get; set; }
        public string AddOneInString { get; set; }
        public string AddTwoInString { get; set; }
        public string AddThreeInString { get; set; }
        public string ReturnInString { get; set; }
        public string GTotalInString { get; set; }


        public string TotalQtyLotNo { get; set; }
        public string AddOneLotNo { get; set; }
        public string AddTwoLotNo { get; set; }
        public string ReturnLotNo { get; set; }
        public string AddThreeLotNo { get; set; }

        public double AddOnePercentage { get; set; }
        public double AddTwoPercentage { get; set; }
        public double AddThreePercentage { get; set; }


        public List<RSDTree> children { get; set; }//: an array nodes defines some children nodes
    }
    #endregion
}