using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region CostSheetCM
    public class CostSheetCM : BusinessObject
    {
        public CostSheetCM()
        {
            CostSheetCMID = 0;
            CostSheetID = 0;
            CMType = EnumCMType.None;
            CMTypeInt = 0;
            NumberOfMachine = 0;
            MachineCost = 0;
            ProductionPerDay = 0;
            BufferDays = 2; //detault
            TotalRequiredDays = 0;
            CMAdditionalPerent = 15; //default
            CSQty = 0;
            CMPart = "";
            ErrorMessage = "";
        }

        #region Property
        public int CostSheetCMID { get; set; }
        public int CostSheetID { get; set; }
        public EnumCMType CMType { get; set; }
        public int CMTypeInt { get; set; }
        public int NumberOfMachine { get; set; }
        public double MachineCost { get; set; }
        public double ProductionPerDay { get; set; }
        public int BufferDays { get; set; }
        public int TotalRequiredDays { get; set; }
        public double CMAdditionalPerent { get; set; }
        public string CMPart { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string CMTypeSt
        {
            get 
            {
                return EnumObject.jGet(this.CMType);
            }
        }
        public double CSQty { get; set; }
        public double CMPerPc
        {
            get
            {
                return (this.NumberOfMachine > 0 && this.MachineCost > 0 && this.TotalRequiredDays > 0) ? Math.Round(((this.NumberOfMachine * this.MachineCost * this.TotalRequiredDays) / this.CSQty), 2) : 0;
            }
        }
        public double CMWithAddition
        {
            get
            {
                return Math.Round((this.CMPerPc + (this.CMPerPc * (this.CMAdditionalPerent / 100))), 2);//consider%
            }
        }
        public double CMPerDozen
        {
            get
            {
                return this.CMWithAddition * 12;
            }
        }
        public string CSQtySt
        {
            get
            {
                return Global.MillionFormat(this.CSQty, 0);
            }
        }
        #endregion

        #region Functions
        public static List<CostSheetCM> Gets(int CSID, long nUserID)
        {
            return CostSheetCM.Service.Gets(CSID, nUserID);
        }

        public static List<CostSheetCM> GetsByLog(int CSLogID, long nUserID)
        {
            return CostSheetCM.Service.GetsByLog(CSLogID, nUserID);
        }
        public static List<CostSheetCM> Gets(string sSQL, long nUserID)
        {
            return CostSheetCM.Service.Gets(sSQL, nUserID);
        }
        public CostSheetCM Get(int id, long nUserID)
        {
            return CostSheetCM.Service.Get(id, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static ICostSheetCMService Service
        {
            get { return (ICostSheetCMService)Services.Factory.CreateService(typeof(ICostSheetCMService)); }
        }
        #endregion
    }
    #endregion

    #region ICostSheetCM interface
    public interface ICostSheetCMService
    {
        CostSheetCM Get(int id, Int64 nUserID);
        List<CostSheetCM> Gets(int CSID, Int64 nUserID);
        List<CostSheetCM> GetsByLog(int CSLogID, Int64 nUserID);

        List<CostSheetCM> Gets(string sSQL, Int64 nUserID);

    }
    #endregion
}
