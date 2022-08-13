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
    #region UnitConversion
    
    public class UnitConversion : BusinessObject
    {
        public UnitConversion()
        {
            UnitConversionID = 0;
            MeasurementUnitID = 0;
            MeasureUnitValue = 0;
            ConvertedUnitID = 0;
            ConvertedUnitValue = 0;
            ProductID = 0;
            ProductCode ="";
			ProductName = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int UnitConversionID { get; set; }
         
        public int MeasurementUnitID { get; set; }
        public int ConvertedUnitID { get; set; }
        public double ConvertedUnitValue { get; set; }
        public double MeasureUnitValue { get; set; }
        public string MeasurementUnitName { get; set; }
        public string MeasurementUnitSymbol { get; set; }
        public string ConvertedUnitName { get; set; }
        public string ConvertedUnitSymbol { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<MeasurementUnit> OppositeUnits { get; set; }
        public int MUID { get; set; }
        public string SelectedMUName { get; set; }
        public List<UnitConversion> ConvertionList { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MeasurementUnitwithconvertedValue
        {
            get
            {
                return "1 " + this.MeasurementUnitName + " = " + this.ConvertedUnitValue.ToString("0.000") + " " + this.ConvertedUnitName;
            }
        }
        #endregion

        #region Functions
        public static List<UnitConversion> Gets(long nUserID)
        {
            return UnitConversion.Service.Gets(nUserID);
        }

        public static List<UnitConversion> Gets(int nProductID, long nUserID)
        {
            return UnitConversion.Service.Gets(nProductID, nUserID);
        }
        public static List<UnitConversion> Gets(string sSQL, long nUserID)
        {
            return UnitConversion.Service.Gets(sSQL, nUserID);
        }

        public UnitConversion Get(int id, long nUserID)
        {
            return UnitConversion.Service.Get(id, nUserID);
        }

        public UnitConversion Save(long nUserID)
        {
            return UnitConversion.Service.Save(this, nUserID);
        }
        public string CommitUnitConversion(int nProductId, string sCopyProductIDs, long nUserID)//for commit product
        {
            return UnitConversion.Service.CommitUnitConversion(nProductId, sCopyProductIDs, nUserID);
        }

        

        public string Delete( long nUserID)
        {
            return UnitConversion.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory

    
        internal static IUnitConversionService Service
        {
            get { return (IUnitConversionService)Services.Factory.CreateService(typeof(IUnitConversionService)); }
        }
        #endregion
    }
    #endregion

    #region IUnitConversion interface
     
    public interface IUnitConversionService
    {         
        UnitConversion Get(int id, Int64 nUserID);         
        List<UnitConversion> Gets(Int64 nUserID);
        List<UnitConversion> Gets(int nProductID, Int64 nUserID);
        List<UnitConversion> Gets(string sSQL, Int64 nUserID); 
        string Delete(UnitConversion oUnitConversion, Int64 nUserID);         
        UnitConversion Save(UnitConversion oUnitConversion, Int64 nUserID);
        string CommitUnitConversion(int nProductId, string sCopyProductIDs, long nUserID);
    }
    #endregion
}