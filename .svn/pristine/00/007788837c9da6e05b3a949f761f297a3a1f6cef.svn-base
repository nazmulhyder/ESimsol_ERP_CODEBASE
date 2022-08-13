using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    #region MktSaleTarget
    public class MktSaleTarget
    {
        public MktSaleTarget()
        {
            MktSaleTargetID = 0;
            MarketingAccountID = 0;
            OrderType = EnumFabricRequestType.None;
            Date = DateTime.Now;
            Value = 0;
            LastUpdateBy = 0;
            LastUpdateDateTime = DateTime.Now;
            ErrorMessage = "";
            LastUpdateByName = "";
            ContractorID = 0;
            ContractorName = "";
            BuyerPosition = "";
            OrderQty = 0;
            ContractorName = "";
            BuyerName = "";
            GroupHeadName = "";
            Day = 0;
            Month = 0;
            Year = 0;
            RefName = "";
            ReceiveQty = 0;
            BUID = 0;
            BPercent = 0;
            BuyerPercentID = 0;
            ApproveBy = 0;
            ApproveDate = DateTime.Now;
            ApprovedByName = "";
            Name = "";
            WeaveType = 0;
            FinishType = 0;
            WeaveTypeName = "";
            FinishTypeName = "";
            Construction = "";
            ProductName = "";
            ProductID = 0;
            Amount = 0;
            IsMonth = false;
            oMktSaleTargets = new List<MktSaleTarget>();
        }
        #region Property
        public int MktSaleTargetID { get; set; }
        public int MarketingAccountID { get; set; }
        public EnumFabricRequestType OrderType { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public int LastUpdateBy { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string LastUpdateByName { get; set; }
        public int ContractorID { get; set; }
        public string BuyerPosition { get; set; }
        public double OrderQty { get; set; }
        public string ContractorName { get; set; }
        public string BuyerName { get; set; }
        public string GroupHeadName { get; set; }
        public double BPercent { get; set; }
        public int BuyerPercentID { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ApprovedByName { get; set; }
        public string Name { get; set; }
        public int WeaveType { get; set; }
        public int FinishType { get; set; }
        public string WeaveTypeName { get; set; }
        public string FinishTypeName{ get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Construction { get; set; }


        //for search
        public bool IsMonth { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string RefName { get; set; }
        public double ReceiveQty { get; set; }
        public int BUID { get; set; }
        public int ViewType { get; set; }
        public int MKTViewType { get; set; }
        public double Amount { get; set; }


        #endregion
        #region Derived Property
        public string EstOrderQtyST
        {
            get
            {
                return this.OrderQty.ToString("#,##0.00;(#,##0.00)");
            }
        }

        public string ReceiveQtyST
        {
            get
            {
                return this.ReceiveQty.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string SaleTargetST
        {
            get
            {
                return this.Value.ToString("#,##0.00;(#,##0.00)");
            }
        }

        public string DateInString
        {
            get
            {
                return Date.ToString("dd MMM yyyy");
            }
        }
        public string ApproveDateInString
        {
            get
            {
                return (ApproveDate==DateTime.MinValue)? "" : ApproveDate.ToString("dd MMM yyyy");
            }
        }
        public string OrderTypeST
        {
            get
            {
                return EnumObject.jGet(this.OrderType);
            }
        }	
        public string LastUpdateDateTimeInString
        {
            get
            {
                return LastUpdateDateTime.ToString("dd MMM yyyy");
            }
        }
        public string BuyerPercentInST
        {
            get
            {
                return this.BPercent + "%";
            }
        }
        public List<MktSaleTarget> oMktSaleTargets { get; set; }
        #endregion
        #region Functions
        public static List<MktSaleTarget> Gets(long nUserID)
        {
            return MktSaleTarget.Service.Gets(nUserID);
        }
        public static List<MktSaleTarget> Gets(string sSQL, long nUserID)
        {
            return MktSaleTarget.Service.Gets(sSQL, nUserID);
        }
        public MktSaleTarget Get(int id, long nUserID)
        {
            return MktSaleTarget.Service.Get(id, nUserID);
        }
        public MktSaleTarget Save(long nUserID)
        {
            return MktSaleTarget.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return MktSaleTarget.Service.Delete(id, nUserID);
        }
        public List<MktSaleTarget> Approve(List<MktSaleTarget> oMktSaleTargets, long nUserID)
        {
            return MktSaleTarget.Service.Approve(oMktSaleTargets, nUserID);
        }
        public List<MktSaleTarget> UndoApprove(List<MktSaleTarget> oMktSaleTargets, long nUserID)
        {
            return MktSaleTarget.Service.UndoApprove(oMktSaleTargets, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IMktSaleTargetService Service
        {
            get { return (IMktSaleTargetService)Services.Factory.CreateService(typeof(IMktSaleTargetService)); }
        }
        #endregion
    }
    #endregion
    #region IMktSaleTarget interface
    public interface IMktSaleTargetService
    {
        MktSaleTarget Get(int id, Int64 nUserID);
        List<MktSaleTarget> Gets(Int64 nUserID);
        List<MktSaleTarget> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        MktSaleTarget Save(MktSaleTarget oMktSaleTarget, Int64 nUserID);
        List<MktSaleTarget> Approve(List<MktSaleTarget> oMktSaleTargets, long nUserID);
        List<MktSaleTarget> UndoApprove(List<MktSaleTarget> oMktSaleTargets, long nUserID);

    }
    #endregion
}
