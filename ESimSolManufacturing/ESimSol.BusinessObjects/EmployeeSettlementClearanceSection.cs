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
    #region EmployeeSettlementClearanceSection

    public class EmployeeSettlementClearanceSection : BusinessObject
    {
        public EmployeeSettlementClearanceSection()
        {
            ESCSID = 0;
            Name = "";
            ErrorMessage = "";
        }

        #region Properties
        public int ESCSID { get; set; }
        public string Name { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        #endregion

        #region Functions
        public static EmployeeSettlementClearanceSection Get(int Id, long nUserID)
        {
            return EmployeeSettlementClearanceSection.Service.Get(Id, nUserID);
        }
        public static EmployeeSettlementClearanceSection Get(string sSQL, long nUserID)
        {
            return EmployeeSettlementClearanceSection.Service.Get(sSQL, nUserID);
        }
        public static List<EmployeeSettlementClearanceSection> Gets(long nUserID)
        {
            return EmployeeSettlementClearanceSection.Service.Gets(nUserID);
        }
        public static List<EmployeeSettlementClearanceSection> Gets(string sSQL, long nUserID)
        {
            return EmployeeSettlementClearanceSection.Service.Gets(sSQL, nUserID);
        }
        public EmployeeSettlementClearanceSection IUD(int nDBOperation, long nUserID)
        {
            return EmployeeSettlementClearanceSection.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeSettlementClearanceSectionService Service
        {
            get { return (IEmployeeSettlementClearanceSectionService)Services.Factory.CreateService(typeof(IEmployeeSettlementClearanceSectionService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeSettlementClearanceSection interface

    public interface IEmployeeSettlementClearanceSectionService
    {
        EmployeeSettlementClearanceSection Get(int id, Int64 nUserID);
        EmployeeSettlementClearanceSection Get(string sSQL, Int64 nUserID);
        List<EmployeeSettlementClearanceSection> Gets(Int64 nUserID);
        List<EmployeeSettlementClearanceSection> Gets(string sSQL, Int64 nUserID);
        EmployeeSettlementClearanceSection IUD(EmployeeSettlementClearanceSection oEmployeeSettlementClearanceSection, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
