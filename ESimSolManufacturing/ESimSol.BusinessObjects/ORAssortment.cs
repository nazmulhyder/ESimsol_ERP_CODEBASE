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
    #region ORAssortment
    
    public class ORAssortment : BusinessObject
    {
        public ORAssortment()
        {
            ORAssortmentID = 0;
            OrderRecapID = 0;
            ColorID = 0;
            SizeID = 0;
            Qty = 0;
            ColorName = "";
            SizeName = "";
            OrderRecapNo = "";
            ORAssortmentLogID = 0;
            OrderRecapLogID = 0;
            ErrorMessage = "";
        }

        #region Properties
         
        public int ORAssortmentID { get; set; }
         
        public int OrderRecapID { get; set; }
         
        public int ColorID { get; set; }
         
        public int SizeID { get; set; }
         
        public int ORAssortmentLogID { get; set; }
         
        public int OrderRecapLogID { get; set; }
         
        public double Qty { get; set; }
         
        public string ColorName { get; set; }
         
        public string SizeName { get; set; }
         
        public string OrderRecapNo { get; set; }

         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        #endregion

        #region Functions

        public static List<ORAssortment> Gets(int nOrderRecapID, long nUserID)
        {
            return ORAssortment.Service.Gets(nOrderRecapID, nUserID);
        }
        public static List<ORAssortment> GetsByLog(int id, long nUserID) //OrderRecapLogId
        {
            return ORAssortment.Service.GetsByLog(id, nUserID);
        }
        public static List<ORAssortment> Gets(string sSQL, long nUserID)
        {
            return ORAssortment.Service.Gets(sSQL, nUserID);
        }
        public ORAssortment Get(int nORAssortmentID, long nUserID)
        {
            return ORAssortment.Service.Get(nORAssortmentID, nUserID);
        }
        public ORAssortment Save(long nUserID)
        {
            return ORAssortment.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory
  
        internal static IORAssortmentService Service
        {
            get { return (IORAssortmentService)Services.Factory.CreateService(typeof(IORAssortmentService)); }
        }

        #endregion
    }
    #endregion

    #region IORAssortment interface
     
    public interface IORAssortmentService
    {
         
        ORAssortment Get(int nORAssortmentID, Int64 nUserID);
         
        List<ORAssortment> Gets(int nOrderRecapID, Int64 nUserID);
         
        List<ORAssortment> Gets(string sSQL, Int64 nUserID);
         
        List<ORAssortment> GetsByLog(int id, Int64 nUserID);   //ORAssortmentLogID
         
        ORAssortment Save(ORAssortment oORAssortment, Int64 nUserID);
    }
    #endregion
    
   
}
