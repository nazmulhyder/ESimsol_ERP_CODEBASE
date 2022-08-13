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
	#region FARegister   
	public class FARegister  : BusinessObject
	{	
		public FARegister ()
		{
			FARegisterID=0;        
	        BUID=0;        
	        LocationID=0;        
	        ProductID=0;        

	        FACodeFull ="";     
	        FAMethod =EnumFAMethod.None;
            DEPCalculateOn = EnumDEPCalculateOn.None;      
	        ActualSalvage =0.0;        
	        UseFullLife=0;        
	        ActualDepRate =0.0;        

	        CurrencyID=0;        
	        Qty =0.0;        
	        MUnitID=0;        
	        ActualCostPrice =0.0;        

	        PurchaseDate=DateTime.Today;
            DepStartDate = DateTime.Today;        
	        Note="";        
	        Model="";
            VersionNo = 0;

	        BrandName="";        
	        Manufacturer="";        
	        ManufacturerYear="";        
	        ProductSLNo="";        
	        CountryOfOrigin="";        

	        BasicFunction="";        
	        WarrantyPeriod=0;        
	        WarrantyDate=DateTime.MinValue;        
	        WarrantyDate_Exp=DateTime.MinValue;        
	        PowerConumption="";        

	        Capacity="";        
	        TechnicalSpec="";        
	        PerformanceSpec="";        

	        PortOfShipment="";        
	        LCNoWithDate="";        
	        HSCode="";        
	        AssestNote="";        

	        SupplierName="";        
	        SupplierAddress="";        
	        SupplierEmail="";        
	        SupplierPhone="";        
	        SupplierFax="";        
	        SupplierCPName="";        
	        SupplierCPPhone="";        
	        SupplierCPEmail="";        
	        SupplierNote="";
            CurrencyName = "";

	        AgentName="";        
	        AgentAddress="";        
	        AgentEmail="";        
	        AgentPhone="";        

	        AgentFax="";        
	        AgentCPName="";        
	        AgentCPPhone="";        
	        AgentCPEmail="";        
	        AgentWorkStation="";        
	        AgentNote="";
            BUName = "";
            FAStatus = EnumFAStatus.None;
            ScheduleStartDate = DateTime.MinValue;
            ScheduleEndDate = DateTime.MinValue;
			ErrorMessage = "";
		}

        #region Property
        public int FARegisterID { get; set; }
        public int BUID { get; set; }
        public int LocationID { get; set; }
        public int ProductID { get; set; }
        public string FACodeFull { get; set; }
        public EnumFAMethod FAMethod { get; set; }
        public EnumDEPCalculateOn DEPCalculateOn { get; set; }
        public double ActualSalvage { get; set; }
        public int UseFullLife { get; set; }
        public double ActualDepRate { get; set; }
        public int CurrencyID { get; set; }
        public double Qty { get; set; }
        public int MUnitID { get; set; }
        public double ActualCostPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime DepStartDate { get; set; }
        public int FARegisterLogID { get; set; }
        public string Note { get; set; }
        public string Model { get; set; }
        public string BrandName { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerYear { get; set; }
        public string ProductSLNo { get; set; }
        public string CountryOfOrigin { get; set; }
        public string BasicFunction { get; set; }
        public int WarrantyPeriod { get; set; }
        public DateTime WarrantyDate { get; set; }
        public DateTime WarrantyDate_Exp { get; set; }
        public string PowerConumption { get; set; }
        public string Capacity { get; set; }
        public string TechnicalSpec { get; set; }
        public string PerformanceSpec { get; set; }
        public string PortOfShipment { get; set; }
        public string LCNoWithDate { get; set; }
        public string HSCode { get; set; }
        public string AssestNote { get; set; }
        public EnumFAStatus FAStatus {get;set;}
        public int FAStatusInt { get; set; }
        public string SupplierName { get; set; }

        public string SupplierAddress { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierPhone { get; set; }
        public string SupplierFax { get; set; }
        public string SupplierCPName { get; set; }
        public string SupplierCPPhone { get; set; }
        public string SupplierCPEmail { get; set; }
        public string SupplierNote { get; set; }
        public string BUName { get; set; }
        public string LocationName { get; set; }
        public string AgentName { get; set; }
        public string AgentAddress { get; set; }
        public string AgentEmail { get; set; }
        public string AgentPhone { get; set; }
        public string AgentFax { get; set; }
        public string AgentCPName { get; set; }
        public string AgentCPPhone { get; set; }
        public string AgentCPEmail { get; set; }
        public string AgentWorkStation { get; set; }
        public string AgentNote { get; set; }
        public string ErrorMessage { get; set; }
        public int FAMethodInt { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryName { get; set; }
        public string Params { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }
        public DateTime ScheduleStartDate { get; set; }
        public DateTime ScheduleEndDate { get; set; }

        #endregion 

		#region Derived Property


        public string ScheduleStartDateInString 
		{
			get
			{
                
                return this.ScheduleStartDate==DateTime.MinValue?"":this.ScheduleStartDate.ToString("dd MMM yyyy"); 
			}
		}
        public string ScheduleEndDateInString
        {
            get
            {

                return this.ScheduleEndDate == DateTime.MinValue ? "" : this.ScheduleEndDate.ToString("dd MMM yyyy");
            }
        }
		public string PurchaseDateInString 
		{
			get
			{
				return PurchaseDate.ToString("dd MMM yyyy") ; 
			}
		}
        public string DepStartDateInString 
		{
			get
			{
				return this.DepStartDate.ToString("dd MMM yyyy") ; 
			}
		}
        public string WarrantyDateInString
        {
            get
            {
                return WarrantyDate.ToString("dd MMM yyyy");
            }
        }
        public string WarrantyDate_ExpInString
        {
            get
            {
                return WarrantyDate_Exp.ToString("dd MMM yyyy");
            }
        }
        public string ActualSalvageSt
        {
            get
            {
                return Global.MillionFormatRound(ActualSalvage, 2);
            }
        }
        public string ActualDepRateSt
        {
            get
            {
                return Global.MillionFormatRound(ActualDepRate, 2);
            }
        }
        public string QtySt
        {
            get
            {
                return Global.MillionFormatRound(Qty, 2);
            }
        }
        public string ActualCostPriceSt
        {
            get
            {
                return Global.MillionFormatRound(ActualCostPrice, 2);
            }
        }
        public string FAMethodSt { get { return EnumObject.jGet(this.FAMethod); } }
        public string DEPCalculateOnSt { get { return EnumObject.jGet(this.DEPCalculateOn); } }
        public string FAStatusSt { get { return EnumObject.jGet(this.FAStatus); } }
        public int ProductCategoryID { get; set; }
        public int DEPCalculateOnInt { get; set; }
        public int VersionNo { get; set; }
		#endregion 

		#region Functions 
		public static List<FARegister > Gets(long nUserID)
		{
			return FARegister.Service.Gets(nUserID);
		}
		public static List<FARegister > Gets(string sSQL, long nUserID)
		{
			return FARegister.Service.Gets(sSQL,nUserID);
		}
		public FARegister  Get(int id, long nUserID)
		{
			return FARegister.Service.Get(id,nUserID);
		}
        public FARegister GetLogByLogID(int id, long nUserID)
        {
            return FARegister.Service.GetLogByLogID(id, nUserID);
        }
		public FARegister  Save(long nUserID)
		{
			return FARegister.Service.Save(this,nUserID);
		}
        public FARegister RequestForRevise(long nUserID)
        {
            return FARegister.Service.RequestForRevise(this, nUserID);
        }
        public FARegister Revise(long nUserID)
        {
            return FARegister.Service.Revise(this, nUserID);
        }
        public FARegister RequestForApprove(long nUserID)
        {
            return FARegister.Service.RequestForApprove(this, nUserID);
        }
        public FARegister SaveLog(long nUserID)
        {
            return FARegister.Service.SaveLog(this, nUserID);
        }
        public static List<FARegister> FA_Process(FARegister oFARegister, long nUserID)
        {
            return FARegister.Service.FA_Process(oFARegister, nUserID);
        }
        public FARegister FARProcess(long nUserID)
        {
            return FARegister.Service.FARProcess(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return FARegister.Service.Delete(id,nUserID);
		}
        public string GetFACode(FARegister oFARegister, long nUserID)
        {
            return FARegister.Service.GetFACode(oFARegister, nUserID);
        }
        public static List<FARegister> FAGRNProcess(List<GRNDetail> oGRNDetails, long nUserID)
        {
            return FARegister.Service.FAGRNProcess(oGRNDetails, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static IFARegisterService Service
		{
			get { return (IFARegisterService)Services.Factory.CreateService(typeof(IFARegisterService)); }
		}
		#endregion

    }
	#endregion

	#region IFARegisterinterface
	public interface IFARegisterService 
	{
		FARegister  Get(int id, Int64 nUserID);
        FARegister GetLogByLogID(int id, Int64 nUserID); 
		List<FARegister > Gets(Int64 nUserID);
		List<FARegister > Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
        FARegister Save(FARegister oFARegister, Int64 nUserID);
        string GetFACode(FARegister oFARegister, Int64 nUserID);
        FARegister RequestForRevise(FARegister oFARegister, Int64 nUserID);
        FARegister RequestForApprove(FARegister oFARegister, Int64 nUserID);
        FARegister Revise(FARegister oFARegister, Int64 nUserID);
        FARegister SaveLog(FARegister oFARegister, Int64 nUserID);
        FARegister FARProcess(FARegister oFARegister, Int64 nUserID);
        List<FARegister> FA_Process(FARegister oFARegister, Int64 nUserID);
        List<FARegister> FAGRNProcess(List<GRNDetail> oGRNDetails, Int64 nUserID);

	}
	#endregion
}
