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

    #region EmployeeLeaveTransfer

    public class EmployeeLeaveTransfer : BusinessObject
    {
        public EmployeeLeaveTransfer()
        {
            LeaveTransferID = 0;
            EmployeeID = 0;
            ELLIDFrom = 0;
            ELLIDTo = 0;
            TransferDays = 0;
            Note = "";
            ErrorMessage = "";
            Params = "";
        }

        #region Properties

        public int LeaveTransferID { get; set; }
        public int EmployeeID { get; set; }
        public int ELLIDFrom { get; set; }
        public int ELLIDTo { get; set; }
        public int TransferDays { get; set; }
        public string Note { get; set; }

        public string ErrorMessage { get; set; }
        public string Params { get; set; }


        #endregion

        #region Derived Property

        public string TransferFrom { get; set; }
        public string TransferTo { get; set; }

        public string TransferStatus { get; set; }

        public string TransferLeaveName { get; set; }
        #endregion

        #region Functions

        public EmployeeLeaveTransfer Get(int nLeaveTransferID, long nUserID)
        {
            return EmployeeLeaveTransfer.Service.Get(nLeaveTransferID, nUserID);
        }
        public static List<EmployeeLeaveTransfer> Gets(int nEmpLeaveLedgerID, long nUserID)
        {
            return EmployeeLeaveTransfer.Service.Gets(nEmpLeaveLedgerID, nUserID);
        }
        public static List<EmployeeLeaveTransfer> Gets(string sSQL, long nUserID)
        {
            return EmployeeLeaveTransfer.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IEmployeeLeaveTransferService Service
        {
            get { return (IEmployeeLeaveTransferService)Services.Factory.CreateService(typeof(IEmployeeLeaveTransferService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeLeaveTransfer interface

    public interface IEmployeeLeaveTransferService
    {
        EmployeeLeaveTransfer Get(int nLeaveTransferID, Int64 nUserID);
        List<EmployeeLeaveTransfer> Gets(int nEmpLeaveLedgerID, Int64 nUserID);
        List<EmployeeLeaveTransfer> Gets(string sSQL, Int64 nUserID);
    }
    #endregion

}
