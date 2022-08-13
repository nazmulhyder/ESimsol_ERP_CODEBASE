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
	#region FNMachine  
	public class FNMachine : BusinessObject
	{	
		public FNMachine()
		{
			FNMachineID = 0;
            FNMachineType = EnumFNMachineType.None;
            FNMachineTypeInt = 0;
			Code = ""; 
			Name = ""; 
			Note = ""; 
			NoOfBath = 0; 
			IsAtive = true;
            CheckActiveProcess = false;
            FreeTime = "";
            FNMachineProcessList = new List<FNMachineProcess>();
			ErrorMessage = "";
            Params = string.Empty;
		}

		#region Property
		public int FNMachineID { get; set; }
        public EnumFNMachineType FNMachineType { get; set; }
        public EnumFNTreatment FNTreatment { get; set; }
        public int FNMachineTypeInt { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Note { get; set; }
		public int NoOfBath { get; set; }
		public bool IsAtive { get; set; }
        public string FreeTime { get; set; }
		public string ErrorMessage { get; set; }
        public string Params { get; set; }
		#endregion 

		#region Derived Property
        public bool CheckActiveProcess { get; set; }
       public List<FNMachineProcess> FNMachineProcessList { get; set; }
       public string NameCode
       {
           get
           {
               return "["+this.Code+"]"+this.Name;
           }
       }
        public string IsAtiveString
       {
           get
           {
               if(this.IsAtive)
               {
                   return "Active";
               }
               else
               {
                   return "In Active";
               }
           }
       }
        public string NoOfBathString
       {
           get
           {
               if (this.NoOfBath<=0)
               {
                   return "-";
               }
               else
               {
                   return this.NoOfBath.ToString();
               }
           }
       }
        
        public string FNMachineTypeSt
        {
            get
            {
                return EnumObject.jGet(this.FNMachineType);
            }
        }
		#endregion 

		#region Functions 
		public static List<FNMachine> Gets(long nUserID)
		{
			return FNMachine.Service.Gets(nUserID);
		}
		public static List<FNMachine> Gets(string sSQL, long nUserID)
		{
			return FNMachine.Service.Gets(sSQL,nUserID);
		}
		public FNMachine Get(int id, long nUserID)
		{
			return FNMachine.Service.Get(id,nUserID);
		}
		public FNMachine Save(long nUserID)
		{
			return FNMachine.Service.Save(this,nUserID);
		}

        public FNMachine ChangeActivety(long nUserID)
		{
            return FNMachine.Service.ChangeActivety(this, nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return FNMachine.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IFNMachineService Service
		{
			get { return (IFNMachineService)Services.Factory.CreateService(typeof(IFNMachineService)); }
		}
		#endregion
	}
	#endregion

	#region IFNMachine interface
	public interface IFNMachineService 
	{
		FNMachine Get(int id, Int64 nUserID); 
		List<FNMachine> Gets(Int64 nUserID);
		List<FNMachine> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		FNMachine Save(FNMachine oFNMachine, Int64 nUserID);
        FNMachine ChangeActivety(FNMachine oFNMachine, Int64 nUserID);
        
	}
	#endregion
}
