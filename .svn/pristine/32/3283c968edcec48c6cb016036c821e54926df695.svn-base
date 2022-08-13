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
	#region FACode  
	public class FACode : BusinessObject
	{	
		public FACode()
		{
			FACodeID = 0; 
			ProductID = 0;
            CodingPartType = EnumFACodingPartType.None;
			CodingPartValue = ""; 
			//ValueLength = 0; 
			Sequence = 0; 
			Remarks = "";
            FACodes = new List<FACode>();
			ErrorMessage = "";
		}

		#region Property
		public int FACodeID { get; set; }
        public int ProductID { get; set; }
        public EnumFACodingPartType CodingPartType { get; set; }
		public int CodingPartTypeInt { get; set; }
        public string CodingPartValue { get; set; }
        public int ValueLength { get { return this.CodingPartValue.Length;  } }
		public int Sequence { get; set; }
		public string Remarks { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

        #region Derived Property
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string CodingPartTypeSt
        {
            get
            {
                return EnumObject.jGet(this.CodingPartType);
            }
        }
        public List<FACode> FACodes { get; set; }
		#endregion 

		#region Functions 
		public static List<FACode> Gets(long nUserID)
		{
			return FACode.Service.Gets(nUserID);
		}
		public static List<FACode> Gets(string sSQL, long nUserID)
		{
			return FACode.Service.Gets(sSQL,nUserID);
		}
		public FACode Get(int id, long nUserID)
		{
			return FACode.Service.Get(id,nUserID);
		}
        public FACode Save(long nUserID)
        {
            return FACode.Service.Save(this, nUserID);
        }
        public FACode Update(long nUserID)
        {
            return FACode.Service.Update(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return FACode.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IFACodeService Service
		{
			get { return (IFACodeService)Services.Factory.CreateService(typeof(IFACodeService)); }
		}
		#endregion

        public static List<FACode> GetsByProduct(int id, int nUserID)
        {
            return FACode.Service.GetsByProduct(id, nUserID);
        }
    }
	#endregion

	#region IFACode interface
	public interface IFACodeService
    {
        FACode Get(int id, Int64 nUserID);
        List<FACode> GetsByProduct(int id, Int64 nUserID); 
		List<FACode> Gets(Int64 nUserID);
		List<FACode> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
        FACode Save(FACode oFACode, Int64 nUserID);
        FACode Update(FACode oFACode, Int64 nUserID);
	}
	#endregion
}
