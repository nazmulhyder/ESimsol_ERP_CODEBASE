using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ExportBillParticular
    [DataContract]
    public class ExportBillParticular: BusinessObject
    {
        public ExportBillParticular()
        {
            ExportBillParticularID = 0;
            Name = "";
            InOutType = EnumInOutType.None;
            InOutTypeInInt = 0;
            Activity = true;
            ErrorMessage = "";
        }

        #region Properties
        public int ExportBillParticularID { get; set; }
        public string Name { get; set; }
        public EnumInOutType InOutType { get; set; }
        public int InOutTypeInInt { get; set; }
        public bool Activity { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string InOutTypeSt
        {
            get
            {
                if (this.InOutType == EnumInOutType.Receive) return "Add";
                else if (this.InOutType == EnumInOutType.Disburse) return "Deduct";
                else return "-";
            }
        }
        public string ActivityInString
        {
            get
            {
                if (this.Activity == true) return "Active";
                else if (this.Activity == false) return "Inactive";
                else return "-";
            }
        }
        #endregion
        #region Functions

        public static List<ExportBillParticular> Gets(Int64 nUserID)
        {
            return ExportBillParticular.Service.Gets(nUserID);
        }
        public static List<ExportBillParticular> Gets(string sSQL, Int64 nUserID)
        {
            return ExportBillParticular.Service.Gets(sSQL, nUserID);
        }
        public static List<ExportBillParticular> Gets(bool bActivity, Int64 nUserID)
        {
            return ExportBillParticular.Service.Gets(bActivity, nUserID);
        }
        public ExportBillParticular Get(int nId, Int64 nUserID)
        {
            return ExportBillParticular.Service.Get(nId, nUserID);
        }
        public ExportBillParticular Save(Int64 nUserID)
        {
            return ExportBillParticular.Service.Save(this, nUserID);
        }
        public string Delete(int nId, Int64 nUserID)
        {
            return ExportBillParticular.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
     
        internal static IExportBillParticularService Service
        {
            get { return (IExportBillParticularService)Services.Factory.CreateService(typeof(IExportBillParticularService)); }
        }
        #endregion

    }
    #endregion

    #region IExportBillParticular interface
    [ServiceContract]
    public interface IExportBillParticularService
    {
        ExportBillParticular Get(int id, Int64 nUserID);
        List<ExportBillParticular> Gets(Int64 nUserID);
        List<ExportBillParticular> Gets(bool bActivity,Int64 nUserID);
        List<ExportBillParticular> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        ExportBillParticular Save(ExportBillParticular oExportBillParticular, Int64 nUserID);
    }
    #endregion
}
