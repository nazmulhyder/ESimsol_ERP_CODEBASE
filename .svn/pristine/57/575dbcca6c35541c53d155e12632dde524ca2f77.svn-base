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
    #region KnittingComposition
    public class KnittingComposition : BusinessObject
    {
        public KnittingComposition()
        {
            KnittingCompositionID = 0;
            KnittingOrderDetailID = 0;
            FabricID = 0;
            YarnID = 0;
            Qty = 0;
            RatioInPercent = 0;
            ErrorMessage = "";
            KnittingCompositions = new List<KnittingComposition>();
            KnittingOrderID = 0;
            YetToChallanQty = 0;
            LotNo = "";
            ColorName = "";
            BrandName = "";
        }

        #region Property
        public int KnittingCompositionID { get; set; }
        public int KnittingOrderDetailID { get; set; }
        public int FabricID { get; set; }
        public int YarnID { get; set; }
        public double Qty { get; set; }
        public double RatioInPercent { get; set; }
        public string LotNo { get; set; }
        public string ColorName { get; set; }
        public string BrandName { get; set; }
        public string ErrorMessage { get; set; }
        public List<KnittingComposition> KnittingCompositions { get; set; }
        #endregion

        #region Derived Property
        public int KnittingOrderID { get; set; }
        public string YarnCode{ get; set; }
        public string YarnName{ get; set; }
        public string FabricCode{ get; set; }
        public string FabricName { get; set; }
        public string Color { get; set; }
        public string Style { get; set; }
        public string MUnitName { get; set; }
        public double YetToChallanQty { get; set; }
        public string YarnNameWithQty
        {
            get
            {
                return this.YarnName +" ("+ Global.MillionFormat(this.Qty)+")";
            }
        }
             
        #endregion

        #region Functions
        public static List<KnittingComposition> Gets(long nUserID)
        {
            return KnittingComposition.Service.Gets(nUserID);
        }
        public static List<KnittingComposition> Gets(string sSQL, long nUserID)
        {
            return KnittingComposition.Service.Gets(sSQL, nUserID);
        }
        public KnittingComposition Get(int id, long nUserID)
        {
            return KnittingComposition.Service.Get(id, nUserID);
        }
        public KnittingComposition Save(long nUserID)
        {
            return KnittingComposition.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return KnittingComposition.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IKnittingCompositionService Service
        {
            get { return (IKnittingCompositionService)Services.Factory.CreateService(typeof(IKnittingCompositionService)); }
        }
        #endregion
    }
    #endregion

    #region IKnittingComposition interface
    public interface IKnittingCompositionService
    {
        KnittingComposition Get(int id, Int64 nUserID);
        List<KnittingComposition> Gets(Int64 nUserID);
        List<KnittingComposition> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        KnittingComposition Save(KnittingComposition oKnittingComposition, Int64 nUserID);
    }
    #endregion
}
