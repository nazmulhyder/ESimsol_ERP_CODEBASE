using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region POTandCSetup
    [DataContract]
    public class POTandCSetup : BusinessObject
    {
        public POTandCSetup()
        {
            POTandCSetupID = 0;
            Clause = "";
            ClauseType = 0;
            Activity = true;
            BUID = 0;
            ErrorMessage = "";
        }

        #region Properties
       
        
        public int POTandCSetupID { get; set; }
        
        public string Clause { get; set; }
        
        public string Note { get; set; }
        
        public int ClauseType { get; set; }
        
        public bool Activity { get; set; }
        public int BUID { get; set; }
        public string ErrorMessage { get; set; }
        private string _sClauseType = "";
        public string ClauseTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumPOTerms)this.ClauseType).ToString();
            }
        }

        private string _sActivity = "";
        public string ActivityInString
        {
            get
            {

                if (this.Activity == true)
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
        private string _sClausenType = "";
        public string ClauseAndType
        {
            get
            {

                _sClausenType = this.ClauseTypeSt + ": " + this.Clause;

                return _sClausenType;
            }
        }
     
        #endregion


        #region Functions
        public static List<POTandCSetup> Gets( int nUserID)
        {
            return POTandCSetup.Service.Gets(nUserID);
        }
        public static List<POTandCSetup> Gets(string sClauseType, int nUserID)
        {
            return POTandCSetup.Service.Gets(sClauseType,nUserID);
        }
        
        public static List<POTandCSetup> Gets(bool bActivity, int nUserID)
        {
            return POTandCSetup.Service.Gets(bActivity,nUserID);
        }

        public static List<POTandCSetup> GetsByBU(int nBUID, int nUserID)
        {
            return POTandCSetup.Service.GetsByBU(nBUID, nUserID);
        }
        public POTandCSetup Get(int id, Int64 nUserID)
        {
            return POTandCSetup.Service.Get(id, nUserID);
        }

        public POTandCSetup Save(int nUserID)
        {
            return POTandCSetup.Service.Save(this, nUserID);
        }


        public string Delete(POTandCSetup oPOTandCSetup, int nUserID)
        {
            return POTandCSetup.Service.Delete(oPOTandCSetup, nUserID);
        }


        #endregion

        #region ServiceFactory

      
        internal static IPOTandCSetupService Service
        {
            get { return (IPOTandCSetupService)Services.Factory.CreateService(typeof(IPOTandCSetupService)); }
        }
        #endregion
    }
    #endregion



    #region IPOTandCSetup interface
    public interface IPOTandCSetupService
    {
        POTandCSetup Get(int id, Int64 nUserID);
        List<POTandCSetup> Gets(Int64 nUserID);
        List<POTandCSetup> Gets(string sClauseType,Int64 nUserID);
        List<POTandCSetup> Gets(bool bActivity,Int64 nUserID);
        List<POTandCSetup> GetsByBU(int nBUID, Int64 nUserID);
        POTandCSetup Save(POTandCSetup oPOTandCSetup, Int64 nUserID);
         string ActivatePOTandCSetup(POTandCSetup oPOTandCSetup, Int64 nUserID);
         string Delete(POTandCSetup oPOTandCSetup, Int64 nUserID);
    }
    #endregion
}
