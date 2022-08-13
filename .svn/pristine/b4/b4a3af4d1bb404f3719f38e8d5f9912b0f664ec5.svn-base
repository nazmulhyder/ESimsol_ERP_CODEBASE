using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region ApprovalHead
    public class EmployeeBonusProcessObject
    {
        public EmployeeBonusProcessObject()
        {
            EBPObjectID=0;
            EBPID=0;
            PPMObject = 0;
            ObjectID = 0;
        }

        #region Properties
        public int EBPObjectID { get; set; }
        public int EBPID { get; set; }
        public int PPMObject { get; set; }
        public int ObjectID { get; set; }
        public string ErrorMessage { get; set; }
        
        #endregion

        #region Functions


        public static List<EmployeeBonusProcessObject> Gets(string sSQL, long nUserID)
        {
            return EmployeeBonusProcessObject.Service.Gets(sSQL, nUserID);
        }
        public static EmployeeBonusProcessObject Get(string sSQL, long nUserID)
        {
            return EmployeeBonusProcessObject.Service.Get(sSQL, nUserID);
        }
        public EmployeeBonusProcessObject IUD(int nDBOperation, long nUserID)
        {
            return EmployeeBonusProcessObject.Service.IUD(this, nDBOperation, nUserID);
        }
 
        #endregion

        #region ServiceFactory

        internal static IEmployeeBonousProcessObjectService Service
        {
            get { return (IEmployeeBonousProcessObjectService)Services.Factory.CreateService(typeof(IEmployeeBonousProcessObjectService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceDaily interface

    public interface IEmployeeBonousProcessObjectService
    {
        List<EmployeeBonusProcessObject> Gets(string sSQL, Int64 nUserID);
        EmployeeBonusProcessObject Get(string sSQL, Int64 nUserID);
        EmployeeBonusProcessObject IUD(EmployeeBonusProcessObject oEmployeeBonousProcessObject, int nDBOperation, Int64 nUserID);
      
    }
    #endregion
}



