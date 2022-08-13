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
    #region MeasurementUnit
    public class MeasurementUnit : BusinessObject
    {
        public MeasurementUnit()
        {
            MeasurementUnitID = 0;            
            UnitName = "";
            UnitType = EnumUniteType.None;
            Symbol="";
            Note="";
            IsRound = false;
            ErrorMessage = "";
            ProductID = 0;
        }

        #region Properties
        public int MeasurementUnitID { get; set; }       
        public string UnitName { get; set; }
        public EnumUniteType UnitType { get; set; }
        public string Symbol { get; set; }
        public string Note { get; set; }
        public bool IsRound { get; set; }
        public bool IsSmallUnit { get; set; }
        public string ErrorMessage { get; set; }
        public int ProductID { get; set; }
        #endregion

        #region Derived Property
        public List<MeasurementUnit> OppositeUnits { get; set; }
        public double SmallUnitValue { get; set; }
        public int UnitTypeInt { get; set; }
        public int UnitTypeInInt { get; set; }
        
        public string UnitTypeInString
        {
            get
            {
                return UnitType.ToString();
            }
        }
        public string UnitNameSymbol
        {
            get
            {
                return this.UnitName + "[" + this.Symbol + "]";
            }
        }
        public string UnitNameType
        {
            get
            {
                return this.UnitName + "[" + this.Symbol + "]" + this.UnitType.ToString(); 
            }
        }
        #endregion

        #region Functions
        public static List<MeasurementUnit> GetsbyProductID(int productId, int nUserID)
        {
            return MeasurementUnit.Service.GetsbyProductID(productId, nUserID);
        }
        public MeasurementUnit Get(int id, int nUserID)
        {
            return MeasurementUnit.Service.Get(id, nUserID);
        }
        public MeasurementUnit Save(int nUserID)
        {
            return MeasurementUnit.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return MeasurementUnit.Service.Delete(id, nUserID);
        }
        public static List<MeasurementUnit> Gets(int nUserID)
        {
            return MeasurementUnit.Service.Gets(nUserID);
        }
        public static List<MeasurementUnit> Gets(EnumUniteType sEnumUniteType, int nUserID)
        {
            return MeasurementUnit.Service.Gets((int)sEnumUniteType, nUserID);
        }
        public static List<MeasurementUnit> Gets(string sSQL, int nUserID)
        {
            return MeasurementUnit.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IMeasurementUnitService Service
        {
            get { return (IMeasurementUnitService)Services.Factory.CreateService(typeof(IMeasurementUnitService)); }
        }
        #endregion
    }
    #endregion
        
    #region IMeasurementUnit interface
    public interface IMeasurementUnitService
    {
        List<MeasurementUnit> GetsbyProductID(int productId, int nUserID);
        MeasurementUnit Get(int id, int nUserID);
        List<MeasurementUnit> Gets(int nUserID);
        List<MeasurementUnit> Gets(int nUnitType, int nUserID);
        List<MeasurementUnit> Gets(string sSQL, int nUserID);
        string Delete(int id, int nUserID);
        MeasurementUnit Save(MeasurementUnit oMeasurementUnit, int nUserID);
    }
    #endregion
}