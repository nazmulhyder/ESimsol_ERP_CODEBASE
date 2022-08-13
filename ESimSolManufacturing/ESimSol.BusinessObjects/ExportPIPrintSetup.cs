using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region ExportPIPrintSetup
    [DataContract]
    public class ExportPIPrintSetup : BusinessObject
    {
        public ExportPIPrintSetup()
        {
            ExportPIPrintSetupID=0;
            SetupNo="";
            Date=DateTime.Today;
            Note="";
            Preface="";
            TermsOfPayment="";
            PartShipment="";
            ShipmentBy="";
            PlaceOfShipment="";
            PlaceOfDelivery="";
            Delivery="";
            RequiredPaper="";
            OtherTerms="";
            AcceptanceBy="";
            For = "";
            ErrorMessage = "";
            Activity = false;
            ValidityDays = 0;
            BUID = 0;
            BaseCurrencyID = 0;
            BINNo = "";
            PrintNo = 0;
            HeaderType = 1;
        }

        #region Properties 
        public int ExportPIPrintSetupID { get; set; }        
        public string SetupNo{ get; set; }        
        public DateTime Date{ get; set; }        
        public string Note{ get; set; }        
        public string Preface{ get; set; }        
        public string TermsOfPayment{ get; set; }        
        public string PartShipment{ get; set; }        
        public string ShipmentBy{ get; set; }        
        public string PlaceOfShipment{ get; set; }        
        public string PlaceOfDelivery{ get; set; }        
        public string Delivery{ get; set; }        
        public string RequiredPaper{ get; set; }        
        public string OtherTerms{ get; set; }        
        public string AcceptanceBy{ get; set; }        
        public string For { get; set; }        
        public string ErrorMessage { get; set; }
        public int HeaderType { get; set; }
        public string BINNo { get; set; }
        public int PrintNo { get; set; }
        public int BUID { get; set; }
        public bool Activity { get; set; }
        public int BaseCurrencyID { get; set; }
        public int ValidityDays { get; set; }      
        public List<ExportTermsAndCondition> PIPrintSetupClauses { get; set; }        
        public string DateInString
        {
            get
            {
                return Date.ToString("dd MMM yyyy");
            }
        }
        private string _sActivity = "";
        public string ActivityInString
        {
            get
            {
                if (Activity == true)
                {
                    _sActivity = "Active";
                }
                if (Activity == false)
                {
                    _sActivity = "Inactive";
                }
                return _sActivity;
            }
        }
        #endregion
                
        #region Functions
        public static List<ExportPIPrintSetup> Gets(Int64 nUserID)
        {
            return ExportPIPrintSetup.Service.Gets(nUserID);            
        }
        public static List<ExportPIPrintSetup> BUWiseGets(int nBUID, Int64 nUserID)
        {
            return ExportPIPrintSetup.Service.BUWiseGets(nBUID, nUserID);
        }
        public ExportPIPrintSetup Get(int id, Int64 nUserID)
        {
            return ExportPIPrintSetup.Service.Get(id, nUserID);            
        }
        public ExportPIPrintSetup Get(bool bActivity,int BUID, Int64 nUserID)
        {
            return ExportPIPrintSetup.Service.Get(bActivity,BUID, nUserID);            
        }
        public ExportPIPrintSetup Get(string sSetupNo, Int64 nUserID)
        {
            return ExportPIPrintSetup.Service.Get(sSetupNo, nUserID);            
        }
        public ExportPIPrintSetup Save(Int64 nUserID)
        {
            return ExportPIPrintSetup.Service.Save(this, nUserID);            
        }
        public string Delete(int id, Int64 nUserID)
        {
            return ExportPIPrintSetup.Service.Delete(id, nUserID);            
        }
        public ExportPIPrintSetup ActivatePIPrintSetup(Int64 nUserID)
        {
            return ExportPIPrintSetup.Service.ActivatePIPrintSetup(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportPIPrintSetupService Service
        {
            get { return (IExportPIPrintSetupService)Services.Factory.CreateService(typeof(IExportPIPrintSetupService)); }
        }
        #endregion
    }
    #endregion

    #region IExportPIPrintSetup interface
    public interface IExportPIPrintSetupService
    {        
        ExportPIPrintSetup Get(int id, Int64 nUserID);        
        ExportPIPrintSetup Get(bool bActivity, int BUID, Int64 nUserID);        
        ExportPIPrintSetup Get(string sSetupNo, Int64 nUserID);         
        List<ExportPIPrintSetup> Gets(Int64 nUserID);
        List<ExportPIPrintSetup> BUWiseGets(int nBUID, Int64 nUserID);   
        
        string Delete(int id, Int64 nUserID);        
        ExportPIPrintSetup Save(ExportPIPrintSetup oExportPIPrintSetup, Int64 nUserID);
        ExportPIPrintSetup ActivatePIPrintSetup(ExportPIPrintSetup oExportPIPrintSetup, Int64 nUserID);
    }
    #endregion
}
