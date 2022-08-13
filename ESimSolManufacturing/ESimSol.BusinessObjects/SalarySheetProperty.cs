using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region SalarySheetProperty

    public class SalarySheetProperty
    {
        public SalarySheetProperty()
        {
            SalarySheetPropertyID = 0;
            SalarySheetFormatProperty = EnumSalarySheetFormatProperty.None;
            PropertyFor = 0;
            IsActive = true;
            ErrorMessage = "";
        }

        #region Properties
        public int SalarySheetPropertyID { get; set; }
        public EnumSalarySheetFormatProperty SalarySheetFormatProperty { get; set; }
        public int SalarySheetFormatPropertyInt { get; set; }
        public Int16 PropertyFor { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string SalarySheetFormatPropertyStr
        {
            get
            {
                return Global.CapitalSpilitor(this.SalarySheetFormatProperty.ToString());
            }
        }
        public string ActivityStr
        {
            get
            {
                return (this.IsActive) ? "Active" : "Inactive";
            }
        }
        #endregion

        #region Functions
        public SalarySheetProperty Get(int nSalarySheetPropertyID, long nUserID)
        {
            return SalarySheetProperty.Service.Get(nSalarySheetPropertyID, nUserID);
        }
        public static List<SalarySheetProperty> Gets(string sSQL, long nUserID)
        {
            return SalarySheetProperty.Service.Gets(sSQL, nUserID);
        }
        public SalarySheetProperty IUD(int nDBOperation, long nUserID)
        {
            return SalarySheetProperty.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ISalarySheetPropertyService Service
        {
            get { return (ISalarySheetPropertyService)Services.Factory.CreateService(typeof(ISalarySheetPropertyService)); }
        }

        #endregion
    }
    #endregion


    #region ISalarySheetProperty interface
    public interface ISalarySheetPropertyService
    {
        SalarySheetProperty Get(int nSalarySheetPropertyID, Int64 nUserID);
        List<SalarySheetProperty> Gets(string sSQL,Int64 nUserID);
        SalarySheetProperty IUD(SalarySheetProperty oSalarySheetProperty, int nDBOperation, Int64 nUserID);
    }
    #endregion
}