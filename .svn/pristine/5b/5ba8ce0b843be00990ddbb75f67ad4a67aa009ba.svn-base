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
    #region PNWiseAccountHead

    public class PNWiseAccountHead : BusinessObject
    {
        public PNWiseAccountHead()
        {
            PNWiseAccountHeadID=0;
            AccountHeadNature=0;
            ProductNature=0;
            AccountHeadID=0;
            AccountHeadName = "";
            AccountCode = "";
            ErrorMessage = "";
            PNWiseAccountHeads = new List<PNWiseAccountHead>();
        }

        #region Properties


        public int PNWiseAccountHeadID { get; set; }
        public int AccountHeadNature { get; set; }
        public int ProductNature { get; set; }
        public int AccountHeadID { get; set; }
        public string AccountHeadName { get; set; }
        public string AccountCode { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string AccountHeadNatureInString
        {
            get
            {
                return EnumObject.jGet((EnumAccountHeadNature)AccountHeadNature);
            }
        }
        public string ProductNatureInString
        {
            get
            {
                return EnumObject.jGet((EnumProductNature)ProductNature);
            }
        }
        public List<PNWiseAccountHead> PNWiseAccountHeads { get; set; }
        public Company Company { get; set; }

        #endregion

        #region Functions

        public static List<PNWiseAccountHead> Gets(long nUserID)
        {
            return PNWiseAccountHead.Service.Gets(nUserID);
        }
        public static List<PNWiseAccountHead> Gets(string sSQL, Int64 nUserID)
        {
            return PNWiseAccountHead.Service.Gets(sSQL, nUserID);
        }

        public PNWiseAccountHead Get(int nId, long nUserID)
        {
            return PNWiseAccountHead.Service.Get(nId, nUserID);
        }

        public PNWiseAccountHead Save(long nUserID)
        {
            return PNWiseAccountHead.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return PNWiseAccountHead.Service.Delete(nId, nUserID);
        }
        public static List<PNWiseAccountHead> GetByCategory(bool bCategory, long nUserID)
        {
            return PNWiseAccountHead.Service.GetByCategory(bCategory, nUserID);
        }
        public static List<PNWiseAccountHead> GetByNegoPNWiseAccountHead(int nPNWiseAccountHeadID, long nUserID)
        {
            return PNWiseAccountHead.Service.GetByNegoPNWiseAccountHead(nPNWiseAccountHeadID, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IPNWiseAccountHeadService Service
        {
            get { return (IPNWiseAccountHeadService)Services.Factory.CreateService(typeof(IPNWiseAccountHeadService)); }
        }
        #endregion
    }
    #endregion

    #region IPNWiseAccountHead interface

    public interface IPNWiseAccountHeadService
    {

        PNWiseAccountHead Get(int id, long nUserID);

        List<PNWiseAccountHead> Gets(long nUserID);
        List<PNWiseAccountHead> Gets(string sSQL, Int64 nUserID);

        string Delete(int id, long nUserID);

        PNWiseAccountHead Save(PNWiseAccountHead oPNWiseAccountHead, long nUserID);

        List<PNWiseAccountHead> GetByCategory(bool bCategory, long nUserID);

        List<PNWiseAccountHead> GetByNegoPNWiseAccountHead(int nPNWiseAccountHeadID, long nUserID);
    }
    #endregion
}