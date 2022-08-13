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
    public class PISizerBreakDown : BusinessObject
    {
        public PISizerBreakDown()
        {
            PISizerBreakDownID = 0;
            PISizerBreakDownLogID = 0;
            ExportPIID = 0;
            ExportPILogID = 0;
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
        public int PISizerBreakDownID { get; set; }
        public int PISizerBreakDownLogID { get; set; }
        public int ExportPIID { get; set; }
        public int ExportPILogID { get; set; }
        public int ProductID { get; set; }
        public int ColorID { get; set; }
        public string Model { get; set; }
        public int SizeID { get; set; }
        public string StyleNo { get; set; }
        public string PantonNo { get; set; }
        public string Remarks { get; set; }
        public double Quantity { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
     
        #endregion

        #region Functions

        public PISizerBreakDown Get(int id, int nUserID)
        {
            return PISizerBreakDown.Service.Get(id, nUserID);
        }
        public PISizerBreakDown Save(int nUserID)
        {
            return PISizerBreakDown.Service.Save(this, nUserID);
        }
        public static List<PISizerBreakDown> Gets(int nUserID)
        {
            return PISizerBreakDown.Service.Gets(nUserID);
        }
        public static List<PISizerBreakDown> Gets(int nPIID, int nUserID)
        {
            return PISizerBreakDown.Service.Gets(nPIID, nUserID);
        }

        public static List<PISizerBreakDown> GetsByLog(int nPILogID, int nUserID)
        {
            return PISizerBreakDown.Service.GetsByLog(nPILogID, nUserID);
        }
        public static List<PISizerBreakDown> Gets(string sSQL, int nUserID)
        {
            return PISizerBreakDown.Service.Gets(sSQL, nUserID);
        }
     
        #endregion


        #region ServiceFactory
        internal static IPISizerBreakDownService Service
        {
            get { return (IPISizerBreakDownService)Services.Factory.CreateService(typeof(IPISizerBreakDownService)); }
        }
        #endregion
    }
    #endregion


    #region IPISizerBreakDown interface
    public interface IPISizerBreakDownService
    {
        PISizerBreakDown Get(int id, int nUserID);
        List<PISizerBreakDown> Gets(int nUserID);
        List<PISizerBreakDown> Gets(int nGRNID, int nUserID);
        List<PISizerBreakDown> GetsByLog(int nPILogID, int nUserID);
        
        string Delete(int id, int nUserID);
        PISizerBreakDown Save(PISizerBreakDown oPISizerBreakDown, int nUserID);

        List<PISizerBreakDown> Gets(string sSQL, int nUserID);

       
    }
    #endregion
    
   
}
