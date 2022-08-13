using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region TechnicalSheetColor
    
    public class TechnicalSheetColor : BusinessObject
    {
        public TechnicalSheetColor()
        {
            TechnicalSheetColorID = 0;	
            TechnicalSheetID=0;	
            ColorCategoryID=0;	
            IsSelected=false;            
            Note = "";
            PantonNo = "";
            Sequence = 0;
            ColorName = "";
            Quantity = 0;
            ErrorMessage = "";
        }

        #region Properties
         
        public int TechnicalSheetColorID{ get; set; }
         
        public int TechnicalSheetID{ get; set; }
         
        public int ColorCategoryID{ get; set; }
         
        public bool IsSelected{ get; set; }
         
        public string Note { get; set; }
        public string PantonNo { get; set; }
        public int Sequence { get; set; }
         
        public string ColorName { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public double Quantity { get; set; }
        public string QuantitySt
        {
            get
            {
                return Global.MillionFormat(this.Quantity,0);
            }
        }
        #endregion

        #region Functions

        public static List<TechnicalSheetColor> Gets(long nUserID)
        {
            return TechnicalSheetColor.Service.Gets( nUserID);
        }

        public static List<TechnicalSheetColor> Gets(int id,long nUserID) //TechnicalSheetID
        {           
            return TechnicalSheetColor.Service.Gets(id, nUserID);
        }

        public TechnicalSheetColor Get(int id, long nUserID)
        {
            return TechnicalSheetColor.Service.Get(id, nUserID);
        }

        public TechnicalSheetColor Save(long nUserID)
        {
            return TechnicalSheetColor.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return TechnicalSheetColor.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static ITechnicalSheetColorService Service
        {
            get { return (ITechnicalSheetColorService)Services.Factory.CreateService(typeof(ITechnicalSheetColorService)); }
        }
        #endregion
    }
    #endregion

    #region ITechnicalSheetColor interface
     
    public interface ITechnicalSheetColorService
    {
         
        TechnicalSheetColor Get(int id, Int64 nUserID);
         
        List<TechnicalSheetColor> Gets(Int64 nUserID);
         
        List<TechnicalSheetColor> Gets(int id,Int64 nUserID);  //TechnicalSheetID
         
        string Delete(int id, Int64 nUserID);
         
        TechnicalSheetColor Save(TechnicalSheetColor oTechnicalSheetColor, Int64 nUserID);
    }
    #endregion
}
