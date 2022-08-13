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
    #region LabdipAssignedPersonnel

    public class LabdipAssignedPersonnel : BusinessObject
    {

        #region  Constructor
        public LabdipAssignedPersonnel()
        {
            LabdipAssignedPersonnelID = 0;
            LabdipDetailID = 0;
            EmployeeID = 0;
            ErrorMessage = "";
            LabdipAssignedPersonnels=new List<LabdipAssignedPersonnel>();
        }
        #endregion

        #region Properties

        public int LabdipAssignedPersonnelID { get; set; }
        public int LabdipDetailID { get; set; }
        public int EmployeeID { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Properties
        public string EmployeeName { get; set; }

        public List<LabdipAssignedPersonnel> LabdipAssignedPersonnels { get; set; }
        #endregion


        #region Functions

        public LabdipAssignedPersonnel IUD(int nDBOperation, long nUserID)
        {
            return LabdipAssignedPersonnel.Service.IUD(this, nDBOperation, nUserID);
        }
        public LabdipAssignedPersonnel Get(int nLabdipAssignedPersonnelID, long nUserID)
        {
            return LabdipAssignedPersonnel.Service.Get(nLabdipAssignedPersonnelID, nUserID);
        }
        public static List<LabdipAssignedPersonnel> Gets(string sSQL, long nUserID)
        {
            return LabdipAssignedPersonnel.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ILabdipAssignedPersonnelService Service
        {
            get { return (ILabdipAssignedPersonnelService)Services.Factory.CreateService(typeof(ILabdipAssignedPersonnelService)); }
        }
        #endregion
    }
    #endregion



    #region ILabdipAssignedPersonnel interface
    public interface ILabdipAssignedPersonnelService
    {
        LabdipAssignedPersonnel IUD(LabdipAssignedPersonnel oLabdipAssignedPersonnel, int nDBOperation, long nUserID);

        LabdipAssignedPersonnel Get(int nLabdipAssignedPersonnelID, long nUserID);

        List<LabdipAssignedPersonnel> Gets(string sSQL, long nUserID);
    }
    #endregion
}