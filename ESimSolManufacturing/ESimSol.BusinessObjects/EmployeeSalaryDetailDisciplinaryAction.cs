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
    #region EmployeeSalaryDetailDisciplinaryAction

    public class EmployeeSalaryDetailDisciplinaryAction : BusinessObject
    {
        public EmployeeSalaryDetailDisciplinaryAction()
        {
            ESDDAID = 0;
            EmployeeSalaryID = 0;
            DisciplinaryActionID = 0;
            ActionName = "";
            Amount = 0;
            Note = "";
            ErrorMessage = "";

        }

        #region Properties
        public int ESDDAID { get; set; }
        public int EmployeeSalaryID { get; set; }
        public int DisciplinaryActionID { get; set; }
        public string ActionName { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }

        #endregion
        #region Derived Property

        #endregion

        #region Functions
        public static EmployeeSalaryDetailDisciplinaryAction Get(int id, long nUserID)
        {
            return EmployeeSalaryDetailDisciplinaryAction.Service.Get(id, nUserID);
        }

        public static EmployeeSalaryDetailDisciplinaryAction Get(string sSQL, long nUserID)
        {
            return EmployeeSalaryDetailDisciplinaryAction.Service.Get(sSQL, nUserID);
        }

        public static List<EmployeeSalaryDetailDisciplinaryAction> Gets(long nUserID)
        {
            return EmployeeSalaryDetailDisciplinaryAction.Service.Gets(nUserID);
        }

        public static List<EmployeeSalaryDetailDisciplinaryAction> Gets(string sSQL, long nUserID)
        {
            return EmployeeSalaryDetailDisciplinaryAction.Service.Gets(sSQL, nUserID);
        }

        public EmployeeSalaryDetailDisciplinaryAction IUD(int nDBOperation, long nUserID)
        {
            return EmployeeSalaryDetailDisciplinaryAction.Service.IUD(this, nDBOperation, nUserID);
        }



        #endregion

        #region ServiceFactory
        internal static IEmployeeSalaryDetailDisciplinaryActionService Service
        {
            get { return (IEmployeeSalaryDetailDisciplinaryActionService)Services.Factory.CreateService(typeof(IEmployeeSalaryDetailDisciplinaryActionService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeSalaryDetailDisciplinaryAction interface

    public interface IEmployeeSalaryDetailDisciplinaryActionService
    {
        EmployeeSalaryDetailDisciplinaryAction Get(int id, Int64 nUserID);
        EmployeeSalaryDetailDisciplinaryAction Get(string sSQL, Int64 nUserID);
        List<EmployeeSalaryDetailDisciplinaryAction> Gets(Int64 nUserID);
        List<EmployeeSalaryDetailDisciplinaryAction> Gets(string sSQL, Int64 nUserID);
        EmployeeSalaryDetailDisciplinaryAction IUD(EmployeeSalaryDetailDisciplinaryAction oEmployeeSalaryDetailDisciplinaryActionSheet, int nDBOperation, Int64 nUserID);


    }
    #endregion
}
