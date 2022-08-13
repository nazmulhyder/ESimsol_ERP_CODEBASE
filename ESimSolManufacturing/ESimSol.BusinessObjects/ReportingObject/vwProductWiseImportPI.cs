using System;
using System.IO;
using ICS.Base.Client.BOFoundation;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Base.Client.ServiceVessel;
using ICS.Base.Client.Utility;


namespace ESimSol.BusinessObjects.ReportingObject
{
    #region vwProductWiseImportPI
    [DataContract]
    public class vwProductWiseImportPI : BusinessObject
    {
        public vwProductWiseImportPI()
        {
            Qty = 0;
            UnitPrice = 0;
            ProductType = EnumProductType.None;
            ProductCategory = EnumYarnCategory.None;
            ImportPINo = "";
            PIIssueDate = DateTime.Now;
            ContractorID = 0;
            TotalValue = 0;
            TotalQty = 0;
            ProductName = "";
            Count = "";
            SupplierName = "";
            Origin = "";
            ImportPIID = 0;
            ProductID = 0;
            GrossOrNetWeight = EnumGrossOrNetWeight.None;
            Status = EnumPurchaseOrderState.None;
            Expr1 = EnumProductType.None;
            LCID = 0;
            LCNo = "";
            LCCurrentStatus = EnumLCCurrentStatus.None;

        }

        #region Properties
        [DataMember]
        public double Qty { get; set; }
        [DataMember]
        public double UnitPrice { get; set; }
        [DataMember]
        public EnumProductType ProductType { get; set; }
        [DataMember]
        public EnumYarnCategory ProductCategory { get; set; }
        [DataMember]
        public string ImportPINo { get; set; }
        [DataMember]
        public DateTime PIIssueDate { get; set; }
        [DataMember]
        public int ContractorID { get; set; }
        [DataMember]
        public double TotalValue { get; set; }
        [DataMember]
        public double TotalQty { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string Count { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public string Origin { get; set; }
        [DataMember]
        public int ImportPIID { get; set; }
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public EnumGrossOrNetWeight GrossOrNetWeight { get; set; }
        [DataMember]
        public EnumPurchaseOrderState Status { get; set; }
        [DataMember]
        public EnumProductType Expr1 { get; set; }
        [DataMember]
        public int LCID { get; set; }
        [DataMember]
        public string LCNo { get; set; }
         [DataMember]
        public EnumLCCurrentStatus LCCurrentStatus { get; set; }


       
        #endregion


        #region Functions

        public static List<vwProductWiseImportPI> Gets(Guid wcfSessionid)
        {
            return (List<vwProductWiseImportPI>)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "Gets")[0];
        }


        public static List<vwProductWiseImportPI> Gets(string sSQL, Guid wcfSessionid)
        {
            return (List<vwProductWiseImportPI>)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "Gets", sSQL)[0];
        }
        #endregion

        #region ServiceFactory

        internal static Type ServiceType
        {
            get
            {
                return typeof(IvwProductWiseImportPIService);
            }
        }
        #endregion
    }
    #endregion

    #region IvwProductWiseImportPI interface
    [ServiceContract]
    public interface IvwProductWiseImportPIService
    {
        [OperationContract]
        List<vwProductWiseImportPI> Gets(Int64 nUserID);
        [OperationContract]
        List<vwProductWiseImportPI> Gets(string sSQL,Int64 nUserID);
    }
    #endregion
  
}
