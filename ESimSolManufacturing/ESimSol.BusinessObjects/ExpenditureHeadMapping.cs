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
    #region ExpenditureHeadMapping
    public class ExpenditureHeadMapping : BusinessObject
    {
        #region  Constructor
        public ExpenditureHeadMapping()
        {
            ExpenditureHeadMappingID = 0;
            ExpenditureHeadID = 0;
            DrCrType = 0;
        }
        #endregion

        #region Properties
        public int ExpenditureHeadMappingID { get; set; }
        public int ExpenditureHeadID { get; set; }
        public EnumExpenditureType OperationType { get; set; }
        public int OperationTypeInt { get; set; }
        public int DrCrType { get; set; }
        public string ErrorMessage { get; set; }                      
        #region DerivedProperties
        public string OperationTypest
        {
            get { return this.OperationType.ToString(); }

        }
        #endregion

        #endregion

        #region Functions
        public ExpenditureHeadMapping Get(int nExpenditureHeadMappingID, long nUserID)
        {
            return ExpenditureHeadMapping.Service.Get(nExpenditureHeadMappingID, nUserID);
        }
     
        public string Delete(long nUserID)
        {
            return ExpenditureHeadMapping.Service.Delete(this, nUserID);
        }

     
        public static List<ExpenditureHeadMapping> Gets(int nImportInvChallanID, long nUserID)
        {
            return ExpenditureHeadMapping.Service.Gets(nImportInvChallanID, nUserID);
        }

        public static List<ExpenditureHeadMapping> Gets(string sSQL, long nUserID)
        {
            return ExpenditureHeadMapping.Service.Gets(sSQL, nUserID);
        }
     
       
        #endregion

        #region ServiceFactory

     
        internal static IExpenditureHeadMappingService Service
        {
            get { return (IExpenditureHeadMappingService)Services.Factory.CreateService(typeof(IExpenditureHeadMappingService)); }
        }
        #endregion
    }
    #endregion

    #region IExpenditureHeadMapping interface
    public interface IExpenditureHeadMappingService
    {
        ExpenditureHeadMapping Get(int nID, Int64 nUserId);
        List<ExpenditureHeadMapping> Gets(int nImportInvChallanID, Int64 nUserId);
        List<ExpenditureHeadMapping> Gets(string sSQL, Int64 nUserID);
        string Delete(ExpenditureHeadMapping oExpenditureHeadMapping, Int64 nUserId);
        
    }
    #endregion
}
