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
    #region TechnicalSheetSize
    
    public class TechnicalSheetSize : BusinessObject
    {
        public TechnicalSheetSize()
        {
            TechnicalSheetSizeID = 0;
            TechnicalSheetID = 0;
            SizeCategoryID=0;
            Sequence = 0;
            QtyInPercent = 0;
            Note = "";
            ErrorMessage = "";
            SizeCategoryName = "";
        }

        #region Properties
         
        public int TechnicalSheetSizeID{ get; set; }
         
        public int TechnicalSheetID{ get; set; }
         
        public int SizeCategoryID{ get; set; }
         
        public int Sequence { get; set; }
         
        public string Note { get; set; }
        public double QtyInPercent { get; set; }        
        public string ErrorMessage { get; set; }
         
        public string SizeCategoryName { get; set; }
        #endregion

        #region Derived Property

        #endregion

        #region Functions

        public static List<TechnicalSheetSize> Gets(long nUserID)
        {
            return TechnicalSheetSize.Service.Gets(nUserID);
        }

        public static List<TechnicalSheetSize> Gets(int nTSID, long nUserID)
        {           
            return TechnicalSheetSize.Service.Gets(nTSID, nUserID);
        }

        public TechnicalSheetSize Get(int id, long nUserID)
        {
            return TechnicalSheetSize.Service.Get(id, nUserID);
        }

        public TechnicalSheetSize Save(long nUserID)
        {
            return TechnicalSheetSize.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return TechnicalSheetSize.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory

     
        internal static ITechnicalSheetSizeService Service
        {
            get { return (ITechnicalSheetSizeService)Services.Factory.CreateService(typeof(ITechnicalSheetSizeService)); }
        }

        #endregion
    }
    #endregion

    #region ITechnicalSheetSize interface
     
    public interface ITechnicalSheetSizeService
    {
         
        TechnicalSheetSize Get(int id, Int64 nUserID);
         
        List<TechnicalSheetSize> Gets(Int64 nUserID);
         
        List<TechnicalSheetSize> Gets(int nTSID, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        TechnicalSheetSize Save(TechnicalSheetSize oTechnicalSheetSize, Int64 nUserID);
    }
    #endregion
}
