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

    #region DevelopmentYarnOption
    
    public class DevelopmentYarnOption : BusinessObject
    {
        public DevelopmentYarnOption()
        {

            DevelopmentYarnOptionID = 0;
            DevelopmentRecapID = 0;
            YarnCategoryID = 0;
            YarnPly = "";
            Note ="";
            ProductCode = "";
            ProductName = "";
            ErrorMessage = "";
            FabricsOptions = "";
        }

        #region Properties

         
        public int DevelopmentYarnOptionID { get; set; }
         
        public int DevelopmentRecapID { get; set; }
         
        public int YarnCategoryID { get; set; }
         
        public string YarnPly { get; set; }
         
        public string Note { get; set; }
         
        public string ProductCode { get; set; }
         
        public string ProductName { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string FabricsOptions { get; set; }
        public string ProductNameCode {

            get {
                return "[" + ProductCode + "]" + ProductName;
            }

        }
        #endregion

        #region Functions


        public static List<DevelopmentYarnOption> Gets_Report(int id, long nUserID)
        {
            return DevelopmentYarnOption.Service.Gets_Report(id, nUserID);
        }


        public static List<DevelopmentYarnOption> Gets(int id, long nUserID)
        {
            return DevelopmentYarnOption.Service.Gets(id, nUserID);
        }
        public static List<DevelopmentYarnOption> Gets(string sSQL, long nUserID)
        {
            
            return DevelopmentYarnOption.Service.Gets(sSQL, nUserID);
        }
        public DevelopmentYarnOption Get(int id, long nUserID)
        {
            return DevelopmentYarnOption.Service.Get(id, nUserID);
        }

        public DevelopmentYarnOption Save(long nUserID)
        {
            return DevelopmentYarnOption.Service.Save(this, nUserID);

        }
        #endregion

        #region ServiceFactory

        internal static IDevelopmentYarnOptionService Service
        {
            get { return (IDevelopmentYarnOptionService)Services.Factory.CreateService(typeof(IDevelopmentYarnOptionService)); }
        }

        #endregion
    }
    #endregion

    #region IDevelopmentYarnOption interface
     
    public interface IDevelopmentYarnOptionService
    {

         
        List<DevelopmentYarnOption> Gets_Report(int id, Int64 nUserID);
         
        DevelopmentYarnOption Get(int id, Int64 nUserID);
         
        List<DevelopmentYarnOption> Gets(int id, Int64 nUserID);
         
        List<DevelopmentYarnOption> Gets(string sSQL, Int64 nUserID);
         
        DevelopmentYarnOption Save(DevelopmentYarnOption oDevelopmentYarnOption, Int64 nUserID);


    }
    #endregion
    
}
