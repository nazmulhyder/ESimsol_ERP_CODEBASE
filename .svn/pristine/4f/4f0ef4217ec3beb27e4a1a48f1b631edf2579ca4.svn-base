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
    #region MeasurementUnitCon
    
    public class MeasurementUnitCon : BusinessObject
    {
        public MeasurementUnitCon()
        {
            MeasurementUnitConID = 0;
            BUID = 0;
            ErrorMessage = "";
            ToMUnitID = 0;
            FromMUnitID = 0;
            Value = 0;
            FromMUnit = "";
            ToMUnit = "";
            MeasurementUnitBUs = new List<MeasurementUnitBU>();
            BusinessUnits = new List<BusinessUnit>();
        }

        #region Properties
        public int MeasurementUnitConID { get; set; }
        public int BUID { get; set; }
        public int FromMUnitID { get; set; }
        public int ToMUnitID { get; set; }
        public double Value { get; set; }
        public string ErrorMessage { get; set; }
        
        #region Derived Property
        public string FromMUnit { get; set; }
        public string ToMUnit { get; set; }
        public List<MeasurementUnitBU> MeasurementUnitBUs { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }
        #endregion

        #endregion

        #region Functions
        public static List<MeasurementUnitCon> Gets(long nUserID)
        {
            return MeasurementUnitCon.Service.Gets(nUserID);
        }
        public static List<MeasurementUnitCon> Gets(int nBUID,long nUserID)
        {
            return MeasurementUnitCon.Service.Gets(nBUID, nUserID);
        }
        public MeasurementUnitCon Get(int id, long nUserID)
        {
            return MeasurementUnitCon.Service.Get(id, nUserID);
        }
        public MeasurementUnitCon GetByBU(int nBUID, long nUserID)
        {
            return MeasurementUnitCon.Service.GetByBU(nBUID, nUserID);
        }
        public MeasurementUnitCon GetBy(int nFromMUnitID,int ToMUnitID, long nUserID)
        {
            return MeasurementUnitCon.Service.GetBy(nFromMUnitID, ToMUnitID, nUserID);
        }

        public MeasurementUnitCon Save(long nUserID)
        {
            return MeasurementUnitCon.Service.Save(this, nUserID);
        }
        public MeasurementUnitCon Activate(Int64 nUserID)
        {
            return MeasurementUnitCon.Service.Activate(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return MeasurementUnitCon.Service.Delete(this, nUserID);
        }
        public MeasurementUnitCon GetByMUnit(int nFromMUnitID, long nUserID)
        {
            return MeasurementUnitCon.Service.GetByMUnit(nFromMUnitID,  nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IMeasurementUnitConService Service
        {
            get { return (IMeasurementUnitConService)Services.Factory.CreateService(typeof(IMeasurementUnitConService)); }
        }
        #endregion
    }
    #endregion


    #region IMeasurementUnitCon interface
    
    public interface IMeasurementUnitConService
    {
        
        MeasurementUnitCon Get(int id, Int64 nUserID);
        MeasurementUnitCon GetBy(int nFromMUnitID, int ToMUnitID, Int64 nUserID);
        MeasurementUnitCon GetByBU(int nBUID, Int64 nUserID);
        List<MeasurementUnitCon> Gets(Int64 nUserID);
        List<MeasurementUnitCon> Gets(int nBUID,Int64 nUserID);
        string Delete(MeasurementUnitCon oMeasurementUnitCon, Int64 nUserID);
        MeasurementUnitCon Save(MeasurementUnitCon oMeasurementUnitCon, Int64 nUserID);
        MeasurementUnitCon Activate(MeasurementUnitCon oMeasurementUnitCon, Int64 nUserID);
        MeasurementUnitCon GetByMUnit(int nFromMUnitID, Int64 nUserID);
    }
    #endregion
}