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
    #region DyeingSolution
    
    public class DyeingSolution : BusinessObject
    {
        public DyeingSolution()
        {
            DyeingSolutionID = 0;
            Code = "";
            DyeingSolutionType = EnumDyeingSolutionType.None;
            Name = "";
            Description = "";
            AdviseBy = "";
            ErrorMessage = "";
        }

        #region Properties
        public int DyeingSolutionID { get; set; }
        public string Code { get; set; }
        public EnumDyeingSolutionType DyeingSolutionType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AdviseBy { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        

        #endregion

        #region Functions
        public static DyeingSolution Get(int nId, long nUserID)
        {
            return DyeingSolution.Service.Get(nId, nUserID);
        }

        public static List<DyeingSolution> Gets(long nUserID)
        {
            return DyeingSolution.Service.Gets(nUserID);
        }

        public static List<DyeingSolution> Gets(string sSQL, long nUserID)
        {
            return DyeingSolution.Service.Gets(sSQL, nUserID);
        }

        public DyeingSolution IUD(int nDBOperation, long nUserID)
        {
            return DyeingSolution.Service.IUD(this, nDBOperation, nUserID);
        }
        public DyeingSolution Copy(int nDyeingSolutionID, long nUserID)
        {
            return DyeingSolution.Service.Copy(nDyeingSolutionID, nUserID);
        }
        


        #endregion

        #region ServiceFactory

        internal static IDyeingSolutionService Service
        {
            get { return (IDyeingSolutionService)Services.Factory.CreateService(typeof(IDyeingSolutionService)); }
        }
        #endregion
    }
    #endregion

    #region IDyeingSolution interface
    
    public interface IDyeingSolutionService
    {
        
        DyeingSolution Get(int id, long nUserID);

        
        List<DyeingSolution> Gets(long nUserID);

        
        List<DyeingSolution> Gets(string sSQL, long nUserID);

        
        DyeingSolution IUD(DyeingSolution oDyeingSolution, int nDBOperation, long nUserID);
        
        DyeingSolution Copy(int nDyeingSolutionID, long nUserID);

       
    }
    #endregion
}
