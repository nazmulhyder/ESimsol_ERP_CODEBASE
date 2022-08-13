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
    #region RouteSheetDCOut

    public class RouteSheetDCOut : BusinessObject
    {
        public RouteSheetDCOut()
        {
            ProductID = 0;
            ProductCode = string.Empty;
            ProductName = string.Empty;
            RouteSheetNo = string.Empty;
            ProductCode  = string.Empty;
            LotNo = string.Empty;
            Qty = 0;
            QtyAdd = 0;
            QtyRet = 0;
            Balance = 0;
            MUName = string.Empty;
            LocationName = string.Empty;
            OperationUnitName = string.Empty;
            UserName= string.Empty;
            DateTime = string.Empty;
            Shift = "";

            IssueDate = System.DateTime.MinValue;
            ApprovedDate= System.DateTime.MinValue;
            InOutType = EnumInOutType.None;
            SequenceNo = 0;

            MUName = "";
            IssuedByName = "";
            ApprovedByName = "";
            RouteSheetNo = "";
            Shift = "";
            ProductCategoryName = "";
        }

        #region Properties
        public int ProductID { get; set; }
        public string RouteSheetNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string LotNo { get; set; }
        public string MUName { get; set; }
        public string Shift { get; set; }
        public double Qty { get; set; }
        public double QtyAdd { get; set; }
        public double QtyRet { get; set; }
        public double Balance { get; set; }
        public string OperationUnitName { get; set; }
        public string LocationName { get; set; }
        public string UserName { get; set; }
        public string DateTime { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string IssuedByName { get; set; }
        public string ApprovedByName { get; set; }
        public EnumInOutType InOutType { get; set; }
        public int SequenceNo { get; set; }

        public string ProductCategoryName { get; set; }
        public string Note { get; set; }
        public EnumProductNature ProductType { get; set; }
        public string ErrorMessage { get; set; }
        public string QtySt
        {
            get
            {
               return Global.MillionFormat(this.Qty);
            }
        }
        public string ProductTypeSt
        {
            get
            {
                return EnumObject.jGet(ProductType);
            }
        }
        public string InOutTypeSt
        {
            get
            {
                if (this.InOutType == EnumInOutType.Disburse && this.SequenceNo > 0) 
                    return "Addition";
                else if (this.InOutType == EnumInOutType.Receive) 
                    return "Return"; 
                else if (this.InOutType == EnumInOutType.Disburse && this.SequenceNo <= 0) 
                    return "Fresh";
                else 
                    return "-";
            }
        }
        #endregion

        #region Derived Property
        public string IssueDateSt
        {
            get
            {
                if (this.IssueDate == System.DateTime.MinValue)
                    return "-";
                else
                    return this.IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string ApprovedDateSt
        {
            get
            {
                if (this.ApprovedDate == System.DateTime.MinValue)
                    return "-";
                else
                    return this.ApprovedDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions


        public static List<RouteSheetDCOut> Gets(string sSQL, Int64 nUserID)
        {
            return RouteSheetDCOut.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IRouteSheetDCOutService Service
        {
            get { return (IRouteSheetDCOutService)Services.Factory.CreateService(typeof(IRouteSheetDCOutService)); }
        }
        #endregion

    }
    #endregion

    #region IPIReport interface
    [ServiceContract]
    public interface IRouteSheetDCOutService
    {
     
        [OperationContract]
        List<RouteSheetDCOut> Gets(string sSQL, Int64 nUserID);
        
    }
    #endregion
}
