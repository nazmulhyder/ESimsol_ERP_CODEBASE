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
	#region PTUUnit2  
	public class PTUUnit2 : BusinessObject
	{	
		public PTUUnit2()
		{
			PTUUnit2ID = 0;
            PTUType = EnumPTUType.None;
            PTUTypeInt = 0;
            ExportSCID = 0;
            ExportSCDetailID = 0;
            ProductID = 0;
            ColorID = 0;
            ExportSCQty = 0;
            ProdOrderQty = 0;
            SubcontractQty = 0;
            GraceQty = 0;
            ProdPipeLineQty = 0;
            ActualFinishQty = 0;
            RejectQty = 0;
            DOQty = 0;
            ChallanQty = 0;
            ReturnQty = 0;
            ExportPINo = "";
            BuyerID = 0;
            BuyerName = "";
            ProductCode = "";
            ProductName = "";
            ColorName = "";
            PTUUnit2RefID = 0;
            UnitID  = 0;
            Measurement = "";
            UnitName = "";
            UnitSymbol = "";
            ModelReferenceID  = 0;
            ProductNature = EnumProductNature.Hanger;
            ProductNatureInInt = 0;
            ModelReferenceName = "";
            FinishGoodsWeight = 0;
            NaliWeight = 0;
            FinishGoodsUnit = 0;
            WeightFor = 0;
            FGUSymbol = "";
            YetToProductionSheeteQty = 0;
            ContractorID = 0;
            ContractorName = "";
            BUID = 0;
            ReadyStockQty = 0;
            ProductionCapacity = 0;
            PTUUnit2RefType = EnumPTUUnit2Ref.Production_Order_Detail;
            PTUUnit2RefTypeInInt = 0;
            YetToDOQty = 0;
            YetToChallanQty = 0;
            AvialableStockQty = 0;
            PIDate = DateTime.Now;
            UnitPrice = 0;
            RateUnit = 0;
            CurrencyID = 0;
            ConversionRate = 0;
            BUName = "";
            SizeName = "";
            StyleNo = "";
            SubContractReceiveQty = 0;
            PTUUnit2Distributions = new List<PTUUnit2Distribution>();
            SubContractReadStockQty = 0;
            ConvertionValue = 0;
            PolyMUnitID = 0;
			ErrorMessage = "";
		}

		#region Property
		public int PTUUnit2ID { get; set; }
        public EnumPTUType PTUType { get; set; }
        public int PTUTypeInt { get; set; }
        public int ExportSCID { get; set; }
        public int ExportSCDetailID { get; set; }
        public int ProductID { get; set; }
        public int PolyMUnitID { get; set; }
        public string ExportPINo { get; set; }
        public string ProductName { get; set; }
        public int  ColorID { get; set; }
        public double ExportSCQty { get; set; }
        public double   ProdOrderQty { get; set; }
        public double GraceQty { get; set; }
        public double ProdPipeLineQty { get; set; }
        public double SubcontractQty { get; set; }
        public double ActualFinishQty { get; set; }
        public double RejectQty { get; set; }
        public double DOQty { get; set; }
        public double ChallanQty { get; set; }
        public double ReturnQty { get; set; }
        public int  BuyerID { get; set; }
        public string  BuyerName { get; set; }
        public string  ProductCode { get; set; }
        public string ColorName { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public int PTUUnit2RefID { get; set; }
        public DateTime PIDate { get; set; }
        public EnumPTUUnit2Ref PTUUnit2RefType { get; set; }
        public int PTUUnit2RefTypeInInt { get; set; }
        public int ProductNatureInInt { get; set; }
        public int BUID { get; set; }
        public int UnitID { get; set; }
        public string     Measurement { get; set; }
        public string    UnitName { get; set; }
        public string    UnitSymbol { get; set; }
       public int    ModelReferenceID  { get; set; }
       public EnumProductNature ProductNature { get; set; }
       public string    ModelReferenceName { get; set; }
       public double    FinishGoodsWeight { get; set; }
       public double   NaliWeight { get; set; }
       public int     FinishGoodsUnit { get; set; }
       public int WeightFor { get; set; }
       public string  FGUSymbol { get; set; }
       public double   YetToProductionSheeteQty { get; set; }
       public double ReadyStockQty { get; set; }
       public double ProductionCapacity { get; set; }
       public double YetToDOQty { get; set; }
       public double YetToChallanQty { get; set; }
       public double AvialableStockQty { get; set; }
       public double UnitPrice { get; set; }
       public int RateUnit { get; set; }
       public int CurrencyID { get; set; }
       public double SubContractReadStockQty { get; set; }
       public double ConversionRate { get; set; }
       public string BUName { get; set; }
       public string SizeName { get; set; }
       public string StyleNo { get; set; }
       public double SubContractReceiveQty { get; set; }
       public double ConvertionValue { get; set; }
          
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public List<PTUUnit2Distribution> PTUUnit2Distributions {get;set; }
       
        public string PIDateInString
        {
            get
            {
                return this.PIDate.ToString("dd MMM yyyy");
            }
        }
        public string YetToProductionSheeteQtySt
        { 
            get
            {
                return Global.MillionFormat(this.YetToProductionSheeteQty)+" "+this.UnitSymbol;
            }
        }
        public string ProdOrderQtyInString
        {
            get
            {
                return this.PTUUnit2ID + "~" + (int)EnumPTUUnit2Ref.Production_Order_Detail + "~" + Global.MillionFormatActualDigit(this.ProdOrderQty);
            }
        }
        public string DOQtyInString
        {
            get
            {
                return this.PTUUnit2ID + "~" + (int)EnumPTUUnit2Ref.Delivery_Order + "~" + Global.MillionFormatActualDigit(this.DOQty);
            }
        }
        public string ChallanQtyInString
        {
            get
            {
                return this.PTUUnit2ID + "~" + (int)EnumPTUUnit2Ref.Delivery_Challan + "~" + Global.MillionFormatActualDigit(this.ChallanQty);
            }
        }
        public string GraceQtyInString
        {
            get
            {
                return this.PTUUnit2ID + "~" + (int)EnumPTUUnit2Ref.Grace + "~" + Global.MillionFormatActualDigit(this.GraceQty);
            }
        }
        public string ActualFinishQtyQtyInString
        {
            get
            {
                return this.PTUUnit2ID + "~" + (int)EnumPTUUnit2Ref.QC+ "~" + Global.MillionFormatActualDigit(this.ActualFinishQty);
            }
        }
        public string ProdPipeLineQtyInString
        {
            get
            {
                return this.PTUUnit2ID + "~" + (int)EnumPTUUnit2Ref.ProductionSheet + "~" + Global.MillionFormatActualDigit(this.ProdPipeLineQty);
            }
        }
        public string SubcontractQtyInString
        {
            get
            {
                return this.PTUUnit2ID + "~" + (int)EnumPTUUnit2Ref.Subcontract_Issue + "~" + Global.MillionFormatActualDigit(this.SubcontractQty);
            }
        }
     
        public string RejectQtyInString
        {
            get
            {
                return this.PTUUnit2ID + "~" + (int)EnumPTUUnit2Ref.Reject + "~" + Global.MillionFormatActualDigit(this.RejectQty);
            }
        }
        public string SubContractReceiveQtyInString
        {
            get
            {
                return this.PTUUnit2ID + "~" + Global.MillionFormatActualDigit(this.SubContractReceiveQty);
            }
        }
        public string ReadyStockQtyInString
        {
            get
            {
                return this.PTUUnit2ID + "~" + Global.MillionFormatActualDigit(this.ReadyStockQty);
            }
        }
		#endregion 

		#region Functions 
        public static List<PTUUnit2> WaitFoSubcontractReceivePTU(int nBUID,int nProductNature, long nUserID)
        {
            return PTUUnit2.Service.WaitFoSubcontractReceivePTU(nBUID, nProductNature, nUserID);
        }
        public static List<PTUUnit2> Gets(int nPIID, int nBUID, long nUserID) 
		{
            return PTUUnit2.Service.Gets(nPIID, nBUID, nUserID);
		}
		public static List<PTUUnit2> Gets(string sSQL, long nUserID)
		{
			return PTUUnit2.Service.Gets(sSQL,nUserID);
		}
		public PTUUnit2 Get(int id, long nUserID)
		{
			return PTUUnit2.Service.Get(id,nUserID);
		}
		public PTUUnit2 UpdateGrace(long nUserID)
		{
            return PTUUnit2.Service.UpdateGrace(this, nUserID);
		}
	
		#endregion

		#region ServiceFactory
		internal static IPTUUnit2Service Service
		{
			get { return (IPTUUnit2Service)Services.Factory.CreateService(typeof(IPTUUnit2Service)); }
		}
		#endregion
	}
	#endregion

	#region IPTUUnit2 interface
	public interface IPTUUnit2Service 
	{
		PTUUnit2 Get(int id, Int64 nUserID);
        List<PTUUnit2> Gets(int nPIID, int nBUID, Int64 nUserID);
        List<PTUUnit2> WaitFoSubcontractReceivePTU(int nBUID, int nProductNature, Int64 nUserID);
		List<PTUUnit2> Gets( string sSQL, Int64 nUserID);
        PTUUnit2 UpdateGrace(PTUUnit2 oPTUUnit2, Int64 nUserID);
	}
	#endregion
}
