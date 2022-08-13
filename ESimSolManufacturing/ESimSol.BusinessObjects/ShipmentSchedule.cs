using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ShipmentSchedule

    public class ShipmentSchedule : BusinessObject
    {
        public ShipmentSchedule()
        {
            ShipmentScheduleID = 0;
            OrderRecapID=0;
            CountryID=0;
            ShipmentDate=DateTime.MinValue;
            CutOffType=EnumCutOffType.None;
            CutOffDate=DateTime.MinValue;
            CutOffWeek=0;
            TotalQty=0;
            ShipmentMode=EnumShipmentBy.None;
            Remarks="";
            OrderRecapNo="";
            BUID=0;
            BuyerName="";
            ProductName="";
            CountryCode="";
            CountryName = "";
            CountryShortName = "";
            OrderQty = 0;
            MeasurementUnitID = 0;
            YetToScheduleQty = 0;
            TechnicalSheetID = 0;
            ShipmentScheduleDetails = new List<ShipmentScheduleDetail>();
            TechnicalSheetSizes = new List<TechnicalSheetSize>();
            ColorSizeRatios = new List<ColorSizeRatio>();
            ErrorMessage = "";
        }

        #region Properties
        public int ShipmentScheduleID { get; set; }
        public int OrderRecapID { get; set; }
        public int CountryID { get; set; }
        public DateTime ShipmentDate { get; set; }
        public EnumCutOffType CutOffType{ get; set; }
        public DateTime CutOffDate { get; set; }
        public int CutOffWeek { get; set; }
        public double TotalQty { get; set; }
        public double OrderQty { get; set; }
        public double YetToScheduleQty { get; set; }
        public EnumShipmentBy ShipmentMode { get; set; }
        public string Remarks { get; set; }
        public string OrderRecapNo { get; set; }
        public int TechnicalSheetID { get; set; }
        public int BUID { get; set; }
        public int MeasurementUnitID { get; set; }
        public string BuyerName { get; set; }
        public string ProductName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CountryShortName { get; set; }
        public string Name { get; set; }

        public string UnitName { get; set; }
     

        #endregion
        #region Derived Property
        public List<ShipmentScheduleDetail> ShipmentScheduleDetails { get; set; }
        public List<TechnicalSheetSize> TechnicalSheetSizes { get; set; }
        public List<ColorSizeRatio> ColorSizeRatios { get; set; }
        public string CountryWithCodeShortName
        {
            get
            {
                return '[' + this.CountryCode + ']' + this.CountryName + '(' + this.CountryShortName + ')';
            }
        }
        public string TotalQtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.TotalQty) + " " + this.UnitName;
            }
        }
        public string ErrorMessage { get; set; }
        public int CutOffTypeInt
        {
            get
            {
                return (int)this.CutOffType;
            }
        }
        public string CutOffTypeST
        {
            get
            {
                return EnumObject.jGet(this.CutOffType);
            }
        }
        public int ShipmentModeInt
        {
            get
            {
                return (int)this.ShipmentMode;
            }
        }
        public string ShipmentModeST
        {
            get
            {
                return EnumObject.jGet(this.ShipmentMode);
            }
        }

        public string CutOffDateSt
        {
            get
            {
                if (this.CutOffDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.CutOffDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ShipmentDateSt
        {
            get
            {
                if (this.ShipmentDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ShipmentDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region Functions
        public static List<ShipmentSchedule> Gets(long nUserID)
        {
            return ShipmentSchedule.Service.Gets(nUserID);
        }
        public static List<ShipmentSchedule> Gets(int nORID, long nUserID)
        {
            return ShipmentSchedule.Service.Gets(nORID, nUserID);
        }
        public static List<ShipmentSchedule> Gets(string sSQL, long nUserID)
        {
            return ShipmentSchedule.Service.Gets(sSQL, nUserID);
        }
        public ShipmentSchedule Get(int id, long nUserID)
        {
            return ShipmentSchedule.Service.Get(id, nUserID);
        }
        public ShipmentSchedule Save(long nUserID)
        {
            return ShipmentSchedule.Service.Save(this, nUserID);
        }
        public ShipmentSchedule GetByType(int nCutOffType, long nUserID)
        {
            return ShipmentSchedule.Service.GetByType(nCutOffType, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return ShipmentSchedule.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IShipmentScheduleService Service
        {
            get { return (IShipmentScheduleService)Services.Factory.CreateService(typeof(IShipmentScheduleService)); }
        }
        #endregion
    }
    #endregion


    #region IShipmentSchedule interface

    public interface IShipmentScheduleService
    {
        ShipmentSchedule Get(int id, Int64 nUserID);
        ShipmentSchedule Save(ShipmentSchedule oShipmentSchedule, Int64 nUserID);
        ShipmentSchedule GetByType(int nCutOffType, Int64 nUserID);
        List<ShipmentSchedule> Gets(string sSQL, long nUserID);
        List<ShipmentSchedule> Gets(Int64 nUserID);
        List<ShipmentSchedule> Gets(int nORID, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
    }
    #endregion
}
