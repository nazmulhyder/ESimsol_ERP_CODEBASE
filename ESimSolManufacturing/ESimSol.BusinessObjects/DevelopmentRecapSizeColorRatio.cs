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


    #region DevelopmentRecapSizeColorRatio
    
    public class DevelopmentRecapSizeColorRatio : BusinessObject
    {
        public DevelopmentRecapSizeColorRatio()
        {
            DevelopmentRecapSizeColorRatioID = 0;
            DevelopmentRecapDetailID = 0;
            ColorID = 0;
            SizeID = 0;
            Qty = 0;
            ColorName = "";
            SizeName = "";
            ErrorMessage = "";
        }

        #region Properties

         
        public int DevelopmentRecapSizeColorRatioID { get; set; }
         
        public int DevelopmentRecapDetailID { get; set; }
         
        public int ColorID { get; set; }
         
        public int SizeID { get; set; }
         
        public double Qty { get; set; }
         
        public string SizeName { get; set; }
         
        public string ColorName { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
      



        #endregion

        #region Functions

        public static List<DevelopmentRecapSizeColorRatio> Gets(int id, long nUserID)
        {
            return DevelopmentRecapSizeColorRatio.Service.Gets(id, nUserID);
        }
        public static List<DevelopmentRecapSizeColorRatio> Gets(string sSQL, long nUserID)
        {
            return DevelopmentRecapSizeColorRatio.Service.Gets(sSQL, nUserID);
        }
        public DevelopmentRecapSizeColorRatio Get(int id, long nUserID)
        {
            return DevelopmentRecapSizeColorRatio.Service.Get(id, nUserID);
        }

        public DevelopmentRecapSizeColorRatio Save(long nUserID)
        {
            return DevelopmentRecapSizeColorRatio.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory


        internal static IDevelopmentRecapSizeColorRatioService Service
        {
            get { return (IDevelopmentRecapSizeColorRatioService)Services.Factory.CreateService(typeof(IDevelopmentRecapSizeColorRatioService)); }
        }
        #endregion
    }
    #endregion

    #region IDevelopmentRecapSizeColorRatio interface
     
    public interface IDevelopmentRecapSizeColorRatioService
    {
         
        DevelopmentRecapSizeColorRatio Get(int id, Int64 nUserID);
         
        List<DevelopmentRecapSizeColorRatio> Gets(int id, Int64 nUserID);
         
        List<DevelopmentRecapSizeColorRatio> Gets(string sSQL, Int64 nUserID);
         
        DevelopmentRecapSizeColorRatio Save(DevelopmentRecapSizeColorRatio oDevelopmentRecapSizeColorRatio, Int64 nUserID);
    }
    #endregion
    
    

}
