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
    #region MgtDBACSetup
    public class MgtDBACSetup : BusinessObject
    {
        public MgtDBACSetup()
        {
            MgtDBACSetupID = 0;
            MgtDBACType = EnumMgtDBACType.None;
            AccountHeadID = 0;
            Remarks = "";
            ErrorMessage = "";
            MgtDBACSetups = new List<MgtDBACSetup>();
        }

        #region Property
        public int MgtDBACSetupID { get; set; }
        public EnumMgtDBACType MgtDBACType { get; set; }
        public int AccountHeadID { get; set; }
        public string Remarks { get; set; }        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property 
        public List<MgtDBACSetup> MgtDBACSetups { get; set; }
        public string AccountHeadName { get; set; }
        public string AccountHeadIDs { get; set; }
        //public string RequestDateInString
        //{
        //    get
        //    {
        //        return RequestDate.ToString("dd MMM yyyy");
        //    }
        //}
        public string MgtDBACTypeInString
        {
            get
            {
                return EnumObject.jGet(this.MgtDBACType);
            }
        }
        public int MgtDBACTypeInt
        {
            get
            {
                return (int)MgtDBACType;
            }
        }

        #endregion

        #region Functions
        public static List<MgtDBACSetup> Gets(long nUserID)
        {
            return MgtDBACSetup.Service.Gets(nUserID);
        }
        public static List<MgtDBACSetup> Gets(string sSQL, long nUserID)
        {
            return MgtDBACSetup.Service.Gets(sSQL, nUserID);
        }
        public MgtDBACSetup Get(int id, long nUserID)
        {
            return MgtDBACSetup.Service.Get(id, nUserID);
        }
        public MgtDBACSetup Save(long nUserID)
        {
            return MgtDBACSetup.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return MgtDBACSetup.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IMgtDBACSetupService Service
        {
            get { return (IMgtDBACSetupService)Services.Factory.CreateService(typeof(IMgtDBACSetupService)); }
        }
        #endregion
    }
    #endregion

    #region IMgtDBACSetup interface
    public interface IMgtDBACSetupService
    {
        MgtDBACSetup Get(int id, Int64 nUserID);
        List<MgtDBACSetup> Gets(Int64 nUserID);
        List<MgtDBACSetup> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        MgtDBACSetup Save(MgtDBACSetup oMgtDBACSetup, Int64 nUserID);


    }
    #endregion
}
