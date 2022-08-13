using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
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
    #region DevelopmentRecapDetail
    
    public class DevelopmentRecapDetail : BusinessObject
    {
        public DevelopmentRecapDetail()
        {
            DevelopmentRecapDetailID = 0;
            DevelopmentRecapID = 0;
            FactoryID = 0;
            FactoryPersonID = 0;
            SeekingDate = DateTime.Now;
            ReceivedBy = 0;
            UnitID = 0;
            SampleQty = 0;
            IsRawmaterialProvide  =false;
            FactoryName = "";
            FactoryPersonName = "";
            UnitName = "";
            ReceivedByName = "";
            Note = "";
            TechnicalSheetID = 0;
            ProductID = 0;
            ErrorMessage = "";
         
        }

        #region Properties
         
        public int DevelopmentRecapDetailID { get; set; }
         
        public int DevelopmentRecapID { get; set; }
         
        public int FactoryID { get; set; }
         
        public int FactoryPersonID { get; set; }
         
        public int ReceivedBy { get; set; }
         
        public int UnitID { get; set; }
         
        public double SampleQty { get; set; }
         
        public bool IsRawmaterialProvide { get; set; }
         
        public DateTime SeekingDate { get; set; }     
         
        public string FactoryName { get; set; }
         
        public string FactoryPersonName { get; set; }
         
        public string UnitName { get; set; }
         
        public string ReceivedByName { get; set; }
         
        public string Note { get; set; }
         
        public string ErrorMessage { get; set; }
        
        #endregion

        #region Derived Property      
        public int TechnicalSheetID {get;set;}
        public int ProductID { get; set; }
        public List<TechnicalSheetSize> TechnicalSheetSizes { get; set; }

        public List<SizeCategory> SizeCategorys { get; set; }
        public List<ColorCategory> ColorCategorys { get; set; }
        public List<TechnicalSheetColor> TechnicalSheetColors { get; set; }
         
        public List<MeasurementUnit> Units { get; set; }
         
        public List<DevelopmentRecapSizeColorRatio> DevelopmentRecapSizeColorRatios { get; set; }

         
        public List<ColorSizeRatio> ColorSizeRatios { get; set; }

        public string SeekingDateInString
        {
            get
            {
                return SeekingDate.ToString("dd MMM yyyy");
            }
        }

        #endregion

        #region Functions
        public static List<DevelopmentRecapDetail> Gets_Report(int id, long nUserID)
        {
            return DevelopmentRecapDetail.Service.Gets_Report(id, nUserID);
        }

        public static List<DevelopmentRecapDetail> Gets(long nUserID)
        {
            
            return DevelopmentRecapDetail.Service.Gets(nUserID);
        }

        public static List<DevelopmentRecapDetail> Gets(string sSQL, long nUserID)
        {
            return DevelopmentRecapDetail.Service.Gets(sSQL, nUserID);
        }

        public static List<DevelopmentRecapDetail> Gets(int nDRID, long nUserID)
        {
            
            return DevelopmentRecapDetail.Service.Gets(nDRID, nUserID);
        }

        public DevelopmentRecapDetail Get(int id, long nUserID)
        {
            
            return DevelopmentRecapDetail.Service.Get(id, nUserID);
        }

        public DevelopmentRecapDetail Save(long nUserID)
        {
            
            return DevelopmentRecapDetail.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID, string sDevelopmentRecapDetailIDs)
        {
            return DevelopmentRecapDetail.Service.Delete(id, nUserID,sDevelopmentRecapDetailIDs);
        }

        #endregion

        #region ServiceFactory

        internal static IDevelopmentRecapDetailService Service
        {
            get { return (IDevelopmentRecapDetailService)Services.Factory.CreateService(typeof(IDevelopmentRecapDetailService)); }
        }


        #endregion
    }
    #endregion

    #region IDevelopmentRecapDetail interface
     
    public interface IDevelopmentRecapDetailService
    {
         
        DevelopmentRecapDetail Get(int id, Int64 nUserID);
         
        List<DevelopmentRecapDetail> Gets_Report(int id, Int64 nUserID);
         
        List<DevelopmentRecapDetail> Gets(Int64 nUserID);
         
        List<DevelopmentRecapDetail> Gets(string sSQL, Int64 nUserID);
         
        List<DevelopmentRecapDetail> Gets(int nDRID, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID, string sDevelopmentRecapDetailIDs);
         
        DevelopmentRecapDetail Save(DevelopmentRecapDetail oDevelopmentRecapDetail, Int64 nUserID);
    }
    #endregion
}
