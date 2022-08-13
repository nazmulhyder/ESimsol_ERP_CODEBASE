using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region PI Sizer BreakDown
    public class POSizerBreakDown : BusinessObject
    {
        public POSizerBreakDown()
        {
            POSizerBreakDownID = 0;
            ProductionOrderID = 0;
            POSizerBreakDownLogID = 0;
            ProductionOrderLogID = 0;
            ProductID = 0;
            ColorID = 0;
            Model = "";
            SizeID = 0;
            StyleNo = "";
            PantonNo = "";
            Quantity = 0;
            ColorName = "";
            SizeName = "";
            ProductCode = "";
            ProductName = "";
            Remarks = "";
            ErrorMessage = "";
        }
        #region Properties
        public int POSizerBreakDownID { get; set; }
        public int ProductionOrderID { get; set; }
        public int POSizerBreakDownLogID { get; set; }
        public int ProductionOrderLogID { get; set; }
        public int ProductID { get; set; }
        public int ColorID { get; set; }
        public int SizeID { get; set; }
        public double Quantity { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Model { get; set; }
        public string StyleNo { get; set; }
        public string PantonNo { get; set; }
        public string Remarks { get;set;}
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
     
        #endregion

        #region Functions

        public POSizerBreakDown Get(int id, int nUserID)
        {
            return POSizerBreakDown.Service.Get(id, nUserID);
        }
        public POSizerBreakDown Save(int nUserID)
        {
            return POSizerBreakDown.Service.Save(this, nUserID);
        }
        public static List<POSizerBreakDown> Gets(int nUserID)
        {
            return POSizerBreakDown.Service.Gets(nUserID);
        }
        public static List<POSizerBreakDown> Gets(int nPIID, int nUserID)
        {
            return POSizerBreakDown.Service.Gets(nPIID, nUserID);
        }

        public static List<POSizerBreakDown> GetsByLog(int nPILogID, int nUserID)
        {
            return POSizerBreakDown.Service.GetsByLog(nPILogID, nUserID);
        }
        public static List<POSizerBreakDown> Gets(string sSQL, int nUserID)
        {
            return POSizerBreakDown.Service.Gets(sSQL, nUserID);
        }
     
        #endregion


        #region ServiceFactory
        internal static IPOSizerBreakDownService Service
        {
            get { return (IPOSizerBreakDownService)Services.Factory.CreateService(typeof(IPOSizerBreakDownService)); }
        }
        #endregion
    }
    #endregion


    #region IPOSizerBreakDown interface
    public interface IPOSizerBreakDownService
    {
        POSizerBreakDown Get(int id, int nUserID);
        List<POSizerBreakDown> Gets(int nUserID);
        List<POSizerBreakDown> Gets(int nGRNID, int nUserID);
        List<POSizerBreakDown> GetsByLog(int nPILogID, int nUserID);
        string Delete(int id, int nUserID);
        POSizerBreakDown Save(POSizerBreakDown oPOSizerBreakDown, int nUserID);

        List<POSizerBreakDown> Gets(string sSQL, int nUserID);

       
    }
    #endregion
    
   
}
