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
    #region SourcingConfigHead

    public class SourcingConfigHead : BusinessObject
    {
        public SourcingConfigHead()
        {
            SourcingConfigHeadID = 0;
            SourcingConfigHeadType = EnumSourcingConfigHeadType.None;
            SourcingConfigHeadName = "";
            Remarks = "";
        }

        #region Properties

        public int SourcingConfigHeadID { get; set; }

        public EnumSourcingConfigHeadType SourcingConfigHeadType { get; set; }
        public string SourcingConfigHeadName { get; set; }
        public string Remarks { get; set; }

        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string SourcingConfigHeadTypeInString
        {
            get
            {
                return EnumObject.jGet(this.SourcingConfigHeadType);
            }
        }
        #endregion

        #region Functions

        public static List<SourcingConfigHead> Gets(long nUserID)
        {
            return SourcingConfigHead.Service.Gets(nUserID);
        }

        public SourcingConfigHead Get(int id, long nUserID)
        {
            return SourcingConfigHead.Service.Get(id, nUserID);
        }

        public SourcingConfigHead Save(long nUserID)
        {
            return SourcingConfigHead.Service.Save(this, nUserID);
        }

        public static List<SourcingConfigHead> Gets(string sSQL, long nUserID)
        {
            return SourcingConfigHead.Service.Gets(sSQL, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return SourcingConfigHead.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory


        internal static ISourcingConfigHeadService Service
        {
            get { return (ISourcingConfigHeadService)Services.Factory.CreateService(typeof(ISourcingConfigHeadService)); }
        }

        #endregion
    }
    #endregion

    #region ISourcingConfigHead interface

    public interface ISourcingConfigHeadService
    {

        SourcingConfigHead Get(int id, Int64 nUserID);

        List<SourcingConfigHead> Gets(Int64 nUserID);

        List<SourcingConfigHead> Gets(string sSQL, Int64 nUserID);

        string Delete(int id, Int64 nUserID);

        SourcingConfigHead Save(SourcingConfigHead oSourcingConfigHead, Int64 nUserID);

    }
    #endregion
}
