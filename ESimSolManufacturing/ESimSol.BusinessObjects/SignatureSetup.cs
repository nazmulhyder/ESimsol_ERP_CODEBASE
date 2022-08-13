using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region SignatureSetup
    public class SignatureSetup
    {
        public SignatureSetup()
        {
            SignatureSetupID = 0;
            ReportModule = EnumReportModule.None;
            ReportModuleInt = 0;
            SignatureCaption = "";
            SignatureSequence = 0;
            DisplayDataColumn = "";
            DisplayFixedName = "";
            ErrorMessage = "";
        }
        #region Properties
        public int SignatureSetupID { get; set; }
        public EnumReportModule ReportModule { get; set; }
        public int ReportModuleInt { get; set; }
        public string SignatureCaption { get; set; }
        public int SignatureSequence { get; set; }
        public string DisplayDataColumn { get; set; }
        public string DisplayFixedName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties        
        public string ReportModuleSt
        {
            get
            {
                return EnumObject.jGet(this.ReportModule);
            }
        }
        #endregion

        #region Functions
        public SignatureSetup Get(int id, int nUserID)
        {
            return SignatureSetup.Service.Get(id, nUserID);
        }
        public SignatureSetup Save(int nUserID)
        {
            return SignatureSetup.Service.Save(this, nUserID);
        }
        public string Delete(int nUserID)
        {
            return SignatureSetup.Service.Delete(this, nUserID);
        }
        public static List<SignatureSetup> Gets(int nUserID)
        {
            return SignatureSetup.Service.Gets(nUserID);
        }
        public static List<SignatureSetup> Gets(string sSQL, int nUserID)
        {
            return SignatureSetup.Service.Gets(sSQL, nUserID);
        }
        public static List<SignatureSetup> GetsByReportModule(EnumReportModule eReportModule, int nUserID)
        {
            return SignatureSetup.Service.GetsByReportModule(eReportModule, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ISignatureSetupService Service
        {
            get { return (ISignatureSetupService)Services.Factory.CreateService(typeof(ISignatureSetupService)); }
        }
        #endregion
    }
    #endregion

    #region ISignatureSetup interface
    public interface ISignatureSetupService
    {
        SignatureSetup Get(int id, int nUserID);
        List<SignatureSetup> Gets(int nUserID);
        string Delete(SignatureSetup oSignatureSetup, int nUserID);
        SignatureSetup Save(SignatureSetup oSignatureSetup, int nUserID);
        List<SignatureSetup> Gets(string sSQL, int nUserID);
        List<SignatureSetup> GetsByReportModule(EnumReportModule eReportModule, int nUserID);
    }
    #endregion
}
