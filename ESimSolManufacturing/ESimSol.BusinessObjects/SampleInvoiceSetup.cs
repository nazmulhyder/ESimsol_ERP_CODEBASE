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
    #region SampleInvoiceSetup

    public class SampleInvoiceSetup : BusinessObject
    {
        public SampleInvoiceSetup()
        {
            SampleInvoiceSetupID = 0;
            BUID = 0;
            IsRateChange = false;
            InvoiceType = EnumSampleInvoiceType.None;
            PrintNo = 0;
            Activity = true;
            Code = "";
            Name = "";
            ShortName = "";
            PrintName = "";
            ErrorMessage = "";
        }

        #region Properties
        public int SampleInvoiceSetupID { get; set; }
        public int BUID { get; set; }
        public EnumSampleInvoiceType InvoiceType { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string PrintName { get; set; }
        public string BusinessUnitName { get; set; }
        public int PrintNo { get; set; }
        public bool Activity { get; set; }
        public bool IsRateChange { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ActivitySt
        {
            get
            {
                if (this.Activity == true) return "Active";
                else if (this.Activity == false) return "Inactive";
                else return "-";
            }
        }
        public int InvoiceTypeInt { get { return (int)InvoiceType; } }
        public string InvoiceTypeST { get { return EnumObject.jGet(this.InvoiceType); } }
        #endregion

        #region Functions
        public static List<SampleInvoiceSetup> Gets(long nUserID)
        {
            return SampleInvoiceSetup.Service.Gets(nUserID);
        }
     
        public static List<SampleInvoiceSetup> Gets(string sSQL, long nUserID)
        {
            return SampleInvoiceSetup.Service.Gets(sSQL, nUserID);
        }
        public SampleInvoiceSetup Get(int id, long nUserID)
        {
            return SampleInvoiceSetup.Service.Get(id, nUserID);
        }
        public SampleInvoiceSetup GetByType(int nInvoiceType, int nBUID, long nUserID)
        {
            return SampleInvoiceSetup.Service.GetByType( nInvoiceType,  nBUID, nUserID);
        }
        public SampleInvoiceSetup GetByBU( int nBUID, long nUserID)
        {
            return SampleInvoiceSetup.Service.GetByBU(nBUID, nUserID);
        }

        public SampleInvoiceSetup Save(long nUserID)
        {
            return SampleInvoiceSetup.Service.Save(this, nUserID);
        }
        public SampleInvoiceSetup Activate(Int64 nUserID)
        {
            return SampleInvoiceSetup.Service.Activate(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return SampleInvoiceSetup.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ISampleInvoiceSetupService Service
        {
            get { return (ISampleInvoiceSetupService)Services.Factory.CreateService(typeof(ISampleInvoiceSetupService)); }
        }
        #endregion
    }
    #endregion

    #region ISampleInvoiceSetup interface

    public interface ISampleInvoiceSetupService
    {

        SampleInvoiceSetup Get(int id, Int64 nUserID);
        SampleInvoiceSetup GetByType(int nInvoiceType, int nBUID, Int64 nUserID);
        SampleInvoiceSetup GetByBU(int nBUID, Int64 nUserID);
        List<SampleInvoiceSetup> Gets(string sSQL, long nUserID);
        List<SampleInvoiceSetup> Gets(Int64 nUserID);
      
        string Delete(SampleInvoiceSetup oSampleInvoiceSetup, Int64 nUserID);
        SampleInvoiceSetup Save(SampleInvoiceSetup oSampleInvoiceSetup, Int64 nUserID);
        SampleInvoiceSetup Activate(SampleInvoiceSetup oSampleInvoiceSetup, Int64 nUserID);
    }
    #endregion
}