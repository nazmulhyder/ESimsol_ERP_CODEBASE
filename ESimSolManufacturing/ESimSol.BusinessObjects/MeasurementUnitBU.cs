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
    public class MeasurementUnitBU
    {
        public MeasurementUnitBU()
        {
            MeasurementUnitConID = 0;
            BUID = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int MeasurementUnitConID { get; set; }
        public int BUID { get; set; }
    
        public string ErrorMessage { get; set; }

        public string BussinessUnitName { get; set; }
        #endregion

        #region FUnction
        public static List<MeasurementUnitBU> Gets(string sSQL, long nUserID)
        {
            return MeasurementUnitBU.Service.Gets(sSQL, nUserID);
        }
        #endregion

	    #region ServiceFactory
        internal static IMeasurementUnitBUService Service
		{
            get { return (IMeasurementUnitBUService)Services.Factory.CreateService(typeof(IMeasurementUnitBUService)); }
		}
		#endregion
	}


	#region IBUWiseParty interface
    public interface IMeasurementUnitBUService 
	{
       
        List<MeasurementUnitBU> Gets(string sSQL, Int64 nUserID);
		
	}
	#endregion
}
