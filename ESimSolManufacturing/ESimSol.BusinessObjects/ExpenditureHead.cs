using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ExpenditureHead
    public class ExpenditureHead : BusinessObject
    {
        #region  Constructor
        public ExpenditureHead()
        {
            ExpenditureHeadID = 0;
            AccountHeadID = 0;
            Name = "";
            AccountCode = "";
            AccountHeadName = "";
            ErrorMessage = "";
            Activity = false;
            ExpenditureHeadType = EnumExpenditureHeadType.None;
            ExpenditureHeadTypeInt = (int)EnumExpenditureHeadType.None;
            ExpenditureHeadMappings = new List<ExpenditureHeadMapping>();
        }

        #endregion

        #region Properties
        public int ExpenditureHeadID { get; set; }
        public int AccountHeadID { get; set; }
        public string Name { get; set; }
        public bool Activity { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public EnumExpenditureHeadType ExpenditureHeadType { get; set; }
        public int ExpenditureHeadTypeInt { get; set; }
        public string ErrorMessage { get; set; }
        
      
        #region Derive property
        public List<ExpenditureHeadMapping> ExpenditureHeadMappings { get; set; }

        #endregion
        public string ActivitySt
        {
            get
            {
                if (this.Activity)
                {
                    return "Active";
                }
                else
                {
                    return "In Active";
                }
            }
        }
        #endregion

        #region Function New Version
        public ExpenditureHead Save(int nUserID)
        {
            return ExpenditureHead.Service.Save(this, nUserID);
        }

        public string Delete(int nUserID)
        {
            return ExpenditureHead.Service.Delete(this, nUserID);
        }

        public ExpenditureHead Get(int nExpenditureHeadID, int nUserID)
        {
            return ExpenditureHead.Service.Get(nExpenditureHeadID, nUserID);
        }
        public static List<ExpenditureHead> Gets( int nUserID)
        {
            return ExpenditureHead.Service.Gets( nUserID);
        }
        public static List<ExpenditureHead> Gets(int nOperationType, int nUserID)
        {
            return ExpenditureHead.Service.Gets(nOperationType, nUserID);
        }
        public static List<ExpenditureHead> Gets(string sSQL, int nUserID)
        {
            return ExpenditureHead.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IExpenditureHeadService Service
        {
            get { return (IExpenditureHeadService)Services.Factory.CreateService(typeof(IExpenditureHeadService)); }
        }
        #endregion

    }
    #endregion

    #region IExpenditureHead interface
    public interface IExpenditureHeadService
    {
        List<ExpenditureHead> Gets(Int64 nUserID);
        string Delete(ExpenditureHead oImportLC, Int64 nUserID);
        ExpenditureHead Save(ExpenditureHead oExpenditureHead, Int64 nUserID);
        ExpenditureHead Get(int nExpenditureHeadID, Int64 nUserID);
        List<ExpenditureHead> Gets(string sSQL, Int64 nUserID);
        List<ExpenditureHead> Gets(int nOperationType, Int64 nUserID);

    }
    #endregion
}
