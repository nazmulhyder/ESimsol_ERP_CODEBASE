using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DURequisitionSetup

    public class DURequisitionSetup : BusinessObject
    {
        public DURequisitionSetup()
        {
            DURequisitionSetupID = 0;
            InOutType = EnumInOutType.None;
            Name = "";
            ShortName = "";
            Activity = true;
            WorkingUnitID_Issue = 0;
            WorkingUnitID_Receive = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int DURequisitionSetupID { get; set; }
        public string Name { get; set; }
        public EnumInOutType InOutType { get; set; }
        public string ShortName { get; set; }
        public bool Activity { get; set; }
        public int WorkingUnitID_Issue { get; set; }
        public int WorkingUnitID_Receive { get; set; }
        public string ErrorMessage { get; set; }

        #region Derived Property
        public string ActivitySt
        {
            get
            {
                if (this.Activity == true) return "Active";
                else if (this.Activity == false) return "Inactive";
                else return "-";
            }
        }
        public int InOutTypeInt
        {
            get
            {
                return (int)this.InOutType;
            }
        }
        public string InOutTypeST
        {
            get
            {
                return EnumObject.jGet(this.InOutType);
            }
        }

        #endregion

        #endregion

        #region Functions
        public static List<DURequisitionSetup> Gets(long nUserID)
        {
            return DURequisitionSetup.Service.Gets(nUserID);
        }
        public static List<DURequisitionSetup> Gets(int nBUID, long nUserID)
        {
            return DURequisitionSetup.Service.Gets(nBUID, nUserID);
        }
        public static List<DURequisitionSetup> Gets(string sSQL, long nUserID)
        {
            return DURequisitionSetup.Service.Gets(sSQL, nUserID);
        }
        public DURequisitionSetup Get(int id, long nUserID)
        {
            return DURequisitionSetup.Service.Get(id, nUserID);
        }
        public DURequisitionSetup GetByType(int nInOutType, long nUserID)
        {
            return DURequisitionSetup.Service.GetByType(nInOutType, nUserID);
        }

        public DURequisitionSetup Save(long nUserID)
        {
            return DURequisitionSetup.Service.Save(this, nUserID);
        }
        public DURequisitionSetup Activate(Int64 nUserID)
        {
            return DURequisitionSetup.Service.Activate(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return DURequisitionSetup.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDURequisitionSetupService Service
        {
            get { return (IDURequisitionSetupService)Services.Factory.CreateService(typeof(IDURequisitionSetupService)); }
        }
        #endregion

    }
    #endregion

    #region IDURequisitionSetup interface

    public interface IDURequisitionSetupService
    {
        DURequisitionSetup Get(int id, Int64 nUserID);
        DURequisitionSetup GetByType(int nInOutType, Int64 nUserID);
        List<DURequisitionSetup> Gets(string sSQL, long nUserID);
        List<DURequisitionSetup> Gets(Int64 nUserID);
        List<DURequisitionSetup> Gets(int nBUID, Int64 nUserID);
        string Delete(DURequisitionSetup oDURequisitionSetup, Int64 nUserID);
        DURequisitionSetup Save(DURequisitionSetup oDURequisitionSetup, Int64 nUserID);
        DURequisitionSetup Activate(DURequisitionSetup oDURequisitionSetup, Int64 nUserID);
    }
    #endregion
}
