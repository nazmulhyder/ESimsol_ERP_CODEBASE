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
	#region FARule  
	public class FARule : BusinessObject
	{	
		public FARule()
		{
			FARuleID = 0;
            ProductID = 0;
            FAMethod = EnumFAMethod.None;
            DefaultDepRate = 0;
            UseFullLife = 0;
            DefaultSalvage = 0; 
            FACodeFull = "";
            ProductCode = "";
            ProductName = "";
            ProductCategoryName = "";
            CurrencyID = 0;
            FAMethodInt = -1;
            DEPCalculateOn = EnumDEPCalculateOn.None;
            DEPCalculateOnInt = -1;
	        DefaultCostPrice=0;
	        CostPriceEffectOn=EnumFAEffectOn.GRN;
            DepEffectFormOn = EnumFADeptEffectFrom.None;
	        Remarks="";
            RegisterApplyOn= EnumFARegisterOn.None;
            CurrencyName = "";
            CurrencySymbol = "";
			MUnitID = 0;
            MUName = "";
            BUName = "";
            MUSymbol = "";
            IsApplyForProductBase = false;
            IsApplyForFACode = false;
			ErrorMessage = "";
		}

		#region Property
        public int FARuleID { get; set; }
        public EnumFAMethod FAMethod { get; set; }
        public int FAMethodInt { get; set; }
        public EnumDEPCalculateOn DEPCalculateOn { get; set; }
		public int DEPCalculateOnInt { get; set; }
        public double DefaultSalvage { get; set; }
		public int UseFullLife { get; set; }
        public double DefaultDepRate { get; set; }
	
        public int ProductCategoryID { get; set; }
        public int BUID { get; set; }
        public int CurrencyID { get; set; }
		public int MUnitID { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string MUName { get; set; }
        public string BUName { get; set; }
        public string MUSymbol { get; set; }
        public double DefaultCostPrice { get; set; }
        public EnumFAEffectOn CostPriceEffectOn { get; set; }
        //public EnumFAEffectOn SalvageEffectOn { get; set; }
        public EnumFADeptEffectFrom DepEffectFormOn { get; set; }
        public string Remarks { get; set; }
        public EnumFARegisterOn RegisterApplyOn { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

        #region Derived Property
        public bool IsApplyForProductBase { get; set; }
        public bool IsApplyForFACode { get; set; }
        public string FACodeFull { get; set; }
        public int IsApplyForProductBaseInt
        {
            get
            {
                if (this.IsApplyForProductBase == true)
                    return 1;
                else
                    return 0;
            }
        }
        public string FAMethodSt { get 
            {
                if (this.FAMethod == EnumFAMethod.None)
                    return "-";
                else
                    return EnumObject.jGet(this.FAMethod); 
            }
        }
        public string RegisterApplyOnSt
        {
            get
            {
                if (this.RegisterApplyOn == EnumFARegisterOn.None)
                    return "-";
                else
                    return EnumObject.jGet(this.RegisterApplyOn);
            }
        }
        public string CostPriceEffectOnSt
        {
            get
            {
                if (this.CostPriceEffectOn == EnumFAEffectOn.None)
                    return "-";
                else
                    return EnumObject.jGet(this.CostPriceEffectOn);
            }
        }
        public string DepEffectFormOnSt
        {
            get
            {
                if (this.DepEffectFormOn == EnumFADeptEffectFrom.None)
                    return "-";
                else
                    return EnumObject.jGet(this.DepEffectFormOn);
            }
        }
        public string DEPCalculateOnSt
        {
            get
            {
                if (this.DEPCalculateOn == EnumDEPCalculateOn.None)
                    return "-";
                else
                    return EnumObject.jGet(this.DEPCalculateOn);
            }
        }
		#endregion 

		#region Functions 
		public static List<FARule> Gets(long nUserID)
		{
			return FARule.Service.Gets(nUserID);
		}
		public static List<FARule> Gets(string sSQL, long nUserID)
		{
			return FARule.Service.Gets(sSQL,nUserID);
		}
		public FARule Get(int id, long nUserID)
		{
			return FARule.Service.Get(id,nUserID);
		}
        public FARule GetByProduct(int pid, long nUserID)
        {
            return FARule.Service.GetByProduct(pid, nUserID);
        }
        public FARule Save(long nUserID)
        {
            return FARule.Service.Save(this, nUserID);
        }
        public FARule Remove_FACode(long nUserID)
        {
            return FARule.Service.Remove_FACode(this, nUserID);
        }
        public FARule Remove_FARule(long nUserID)
        {
            return FARule.Service.Remove_FARule(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return FARule.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IFARuleService Service
		{
			get { return (IFARuleService)Services.Factory.CreateService(typeof(IFARuleService)); }
		}
		#endregion
    }
	#endregion

	#region IFARule interface
	public interface IFARuleService 
	{
        FARule Get(int id, Int64 nUserID);
        FARule GetByProduct(int pid, Int64 nUserID); 
		List<FARule> Gets(Int64 nUserID);
		List<FARule> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
        FARule Save(FARule oFARule, Int64 nUserID);
        FARule Remove_FACode(FARule oFARule, Int64 nUserID);
        FARule Remove_FARule(FARule oFARule, Int64 nUserID);
	}
	#endregion
}
