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
    #region RouteSheetSetup
    
    public class RouteSheetSetup : BusinessObject
    {
        public RouteSheetSetup()
        {
            RouteSheetSetupID = 0;
            RSName = "";
            RSShortName = "";
            MUnitID = 0;
            MUnitID_Two =0;
            Activity = true;
            IsLotMandatory = false;
            Note = "";
            SmallUnit_Cal = 1;
            SMUnitValue = 0;
            NumberOfAddition = 0;
            RSName_Print = "";
            ErrorMessage = "";
            GracePercentage=0;
            LossPercentage=0;
            GainPercentage=0;
            PrintNo = EnumExcellColumn.A;
            RestartBy = EnumRestartPeriod.None;
            DyesChemicalViewType = EnumDyesChemicalViewType.None;
            MUnit = "";
            MUnitTwo = "";
            IsShowBuyer = false;
            IsApplyHW = false;
            IsGraceApplicable = false;
            BatchTime = DateTime.MinValue;
            WorkingUnitIDWIP = 0;
            //DCEntryType = 0;
            DCEntryValType = EnumDCEntryType.None;
            DCOutValType = EnumDCEntryType.None;
            RSStateForCost = EnumRSState.None;
            IsRateOnUSD = false;
        }

        #region Properties
        public int RouteSheetSetupID { get; set; }
        public string RSName { get; set; }
        public string RSName_Print { get; set; }
        public string RSShortName { get; set; }
        public int MUnitID { get; set; }
        public int MUnitID_Two { get; set; }
        public double SmallUnit_Cal { get; set; }
        public double SMUnitValue { get; set; }
        public bool Activity { get; set; }
        public double GracePercentage { get; set; }
        public double LossPercentage { get; set; }
        public double GainPercentage { get; set; }
        public EnumExcellColumn PrintNo { get; set; }
        public EnumRestartPeriod RestartBy { get; set; }
        public bool IsLotMandatory { get; set; }
        public bool IsShowBuyer { get; set; }
        public bool IsApplyHW { get; set; }
        public bool IsRateOnUSD { get; set; }
        public string Note { get; set; }
        public string BatchCode { get; set; }
        public int MachinePerDoc { get; set; }
        public int WorkingUnitIDWIP { get; set; }
        //public int DCEntryType { get; set; } /// Dyes Chemical entry Validation        
        public string ErrorMessage { get; set; }
        public DateTime BatchTime { get; set; }
        public double FontSize { get; set; }
        public int NumberOfAddition { get; set; }
        public bool IsGraceApplicable { get; set; }
        public EnumDyesChemicalViewType DyesChemicalViewType { get; set; }
        public EnumDCEntryType DCEntryValType { get; set; }
        public EnumDCEntryType DCOutValType { get; set; }
        public EnumRSState RSStateForCost { get; set; }


        public List<RouteSheetSetup> RouteSheetSetups { get; set; }

        #region Derived Property
        public string MUnit { get; set; }
        public string MUnitTwo { get; set; }
        public int PrintNoInt { get { return (int)PrintNo; } }
        public int RestartByInt { get { return (int)RestartBy; } }
        public int DyesChemicalViewTypeInt { get { return (int)DyesChemicalViewType; } }
        public string RestartBySt { get { return EnumObject.jGet(this.RestartBy); } }
        public string DyesChemicalViewTypeSt { get { return EnumObject.jGet(this.DyesChemicalViewType); } }
        public string BatchTimeSt { get { return BatchTime.ToString("HH : mm"); } }
        public string BatchTimeTT
        {
            get
            {
                return this.BatchTime.ToString("hh:mm tt");
            }
        }
        public string DCEntryValTypeST
        {
            get
            {
                return EnumObject.jGet(this.DCEntryValType);
            }
        }
        public string DCOutValTypeST
        {
            get
            {
                return EnumObject.jGet(this.DCOutValType);
            }
        }
        public string RSStateForCostST
        {
            get
            {
                return EnumObject.jGet(this.RSStateForCost);
            }
        }
        
        #endregion

        #endregion

        #region Functions
        public static List<RouteSheetSetup> Gets(long nUserID)
        {
            return RouteSheetSetup.Service.Gets(nUserID);
        }

        public RouteSheetSetup Get(int id, long nUserID)
        {
            return RouteSheetSetup.Service.Get(id, nUserID);
        }
        public RouteSheetSetup GetBy( long nUserID)
        {
            return RouteSheetSetup.Service.GetBy(nUserID);
        }

        public RouteSheetSetup Save(long nUserID)
        {
            return RouteSheetSetup.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return RouteSheetSetup.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IRouteSheetSetupService Service
        {
            get { return (IRouteSheetSetupService)Services.Factory.CreateService(typeof(IRouteSheetSetupService)); }
        }
        #endregion

    }
    #endregion

    #region IRouteSheetSetup interface
    
    public interface IRouteSheetSetupService
    {
        RouteSheetSetup Get(int id, Int64 nUserID);
        RouteSheetSetup GetBy( Int64 nUserID);
        List<RouteSheetSetup> Gets(Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        RouteSheetSetup Save(RouteSheetSetup oRouteSheetSetup, Int64 nUserID);
    }
    #endregion
}