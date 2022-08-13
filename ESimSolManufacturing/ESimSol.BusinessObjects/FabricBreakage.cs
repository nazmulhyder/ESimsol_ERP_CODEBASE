using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
   
    
    #region FabricBreakage
    
    public class FabricBreakage:BusinessObject
    {
        public FabricBreakage()
        {
            FBreakageID = 0;             
            Name = "";
            WeavingProcess = EnumWeavingProcess.Warping;
            ErrorMessage = "";

        }

        #region Properties

        public int FBreakageID { get; set; }

        public EnumWeavingProcess WeavingProcess { get; set; }
       
         
        public string Name { get; set; }
       
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<FabricBreakage> FabricBreakages { get; set; }
        public Company Company { get; set; }
        public string WeavingProcessInString
        {
            get
            {
                return this.WeavingProcess.ToString();
            }
        }
        #endregion

        #region Functions

        public static List<FabricBreakage> Gets(long nUserID)
        {
            return FabricBreakage.Service.Gets( nUserID);
        }

        public static List<FabricBreakage> Gets( EnumWeavingProcess eProcess, long nUserID)
        {
            return FabricBreakage.Service.Gets((int)eProcess, nUserID);
        }

        public static List<FabricBreakage> Gets(string sSQL, long nUserID)
        {
            return FabricBreakage.Service.Gets(sSQL, nUserID);
        }

        public FabricBreakage Get(int id, long nUserID)
        {
            return FabricBreakage.Service.Get(id, nUserID);
        }

        public FabricBreakage Save(long nUserID)
        {
            return FabricBreakage.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return FabricBreakage.Service.Delete(id, nUserID);
        }
        

        #endregion

        #region ServiceFactory
        internal static IFabricBreakageService Service
        {
            get { return (IFabricBreakageService)Services.Factory.CreateService(typeof(IFabricBreakageService)); }
        }


        #endregion
    }
    #endregion

    #region Report Study
    public class FabricBreakageList : List<FabricBreakage>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IFabricBreakage interface
     
    public interface IFabricBreakageService
    {
         
        FabricBreakage Get(int id, Int64 nUserID);
         
        List<FabricBreakage> Gets(int eProcess, Int64 nUserID);
        List<FabricBreakage> Gets(Int64 nUserID);
         
        List<FabricBreakage> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        FabricBreakage Save(FabricBreakage oFabricBreakage, Int64 nUserID);
       
    }
    #endregion
    
  
}
