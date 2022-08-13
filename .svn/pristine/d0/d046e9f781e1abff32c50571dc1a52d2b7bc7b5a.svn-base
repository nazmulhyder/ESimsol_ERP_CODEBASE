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
    #region MgtDBObj
    public class MgtDBObj : BusinessObject
    {
        public MgtDBObj()
        {
            TempID = 0;
            BUID = 0;
            ReportDate = DateTime.Today;
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            RefType = EnumMgtDBRefType.None;
            RefTypeInt = 0;
            RefValueType = EnumMgtDBRefValueType.None;
            RefValueTypeInt = 0;
            RefValueID = 0;
            RefCaption = "";
            RefQty = 0;
            RefAmount = 0;
            ExportPIAmount = 0;
            LCRcvAmount = 0;
            ExportRecevableAmount = 0;
            ImportPayableAmount = 0;
            MUnitID = 0;
            MUnit = "";
            CurrencyID = 0;
            Currency = "";
            ErrorMessage = "";
        }

        #region Property
        public int TempID { get; set; }
        public int BUID { get; set; }
        public DateTime ReportDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EnumMgtDBRefType RefType { get; set; }
        public int RefTypeInt { get; set; }
        public EnumMgtDBRefValueType RefValueType { get; set; }
        public int RefValueTypeInt { get; set; }
        public int RefValueID { get; set; }
        public string RefCaption { get; set; }
        public double RefQty { get; set; }
        public double RefAmount { get; set; }
        public double ExportPIAmount { get; set; }
        public double LCRcvAmount { get; set; }
        public double ExportRecevableAmount { get; set; }
        public double ImportPayableAmount { get; set; }
        public int MUnitID { get; set; }
        public string MUnit { get; set; }
        public int CurrencyID { get; set; }
        public string Currency { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ReportDateSt
        {
            get
            {
                return this.ReportDate.ToString("dd MMM yyyy");
            }
        }
        public string StartDateSt
        {
            get
            {
                return StartDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateSt
        {
            get
            {
                return EndDate.ToString("dd MMM yyyy");
            }
        }
        public string RefTypeSt
        {
            get
            {
                return EnumObject.jGet(this.RefType);
            }
        }
        public string RefQtySt
        {
            get
            {
                return this.RefQty.ToString("##,##,##,###");
            }
        }
        public string RefAmountSt
        {
            get
            {
                return this.Currency + " " + this.RefAmount.ToString("##,##,##,###");
            }
        }
        #endregion

        #region Functions
        public static List<MgtDBObj> Gets(MgtDBObj oMgtDBObj, long nUserID)
        {
            return MgtDBObj.Service.Gets(oMgtDBObj, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IMgtDBObjService Service
        {
            get { return (IMgtDBObjService)Services.Factory.CreateService(typeof(IMgtDBObjService)); }
        }
        #endregion
    }
    #endregion

    #region IMgtDBObj interface
    public interface IMgtDBObjService
    {
        List<MgtDBObj> Gets(MgtDBObj oMgtDBObj, Int64 nUserID);

    }
    #endregion
}