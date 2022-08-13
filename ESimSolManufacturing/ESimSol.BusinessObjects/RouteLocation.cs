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
    #region RouteLocation
    public class RouteLocation : BusinessObject
    {
        public RouteLocation()
        {
            RouteLocationID = 0;
            LocCode = "";
            Name = "";
            Description = "";
            BUID = 0;
            LocationType = EnumRouteLocation.None;
            LocationTypeInt = 0;
            ErrorMessage = "";
            LocationTypes = new List<EnumObject>();
        }

        #region Properties
        public int RouteLocationID { get; set; }
        public string LocCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BUID { get; set; } 
        public string ErrorMessage { get; set; }
        public EnumRouteLocation LocationType { get; set; }
        public int LocationTypeInt { get; set; }
        public List<EnumObject> LocationTypes { get; set; }
        public string LocationTypeSt
        {
            get
            {
                return  EnumObject.jGet(LocationType);

            }
        }

        
        #endregion

    
        #region Functions
        public static List<RouteLocation> Gets(int nUserID)
        {
            return RouteLocation.Service.Gets( nUserID);
        }
        public static List<RouteLocation> BUWiseGets(int BUID, int nUserID)
        {
            return RouteLocation.Service.BUWiseGets(BUID, nUserID);
        }
        public static List<RouteLocation> Gets(EnumRouteLocation eEnumRouteLocation, int nUserID)
        {
            return RouteLocation.Service.Gets((int)eEnumRouteLocation, nUserID);
        }
        public RouteLocation Get(int id, int nUserID)
        {
            return RouteLocation.Service.Get(id, nUserID);
        }
        public RouteLocation Save(int nUserID)
        {
            return RouteLocation.Service.Save(this, nUserID);
        }

        public string Delete(int nUserID)
        {
            return RouteLocation.Service.Delete(this, nUserID);
        }

        public static List<RouteLocation> Gets(string sSQL, int nUserID)
        {
            return RouteLocation.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IRouteLocationService Service
        {
            get { return (IRouteLocationService)Services.Factory.CreateService(typeof(IRouteLocationService)); }
        }
        #endregion
    }
    #endregion

    
    #region IRouteLocation interface
    public interface IRouteLocationService
    {
        List<RouteLocation> Gets(string sSQL, int nUserID);
        RouteLocation Get(int id, Int64 nUserID);
        List<RouteLocation> Gets(Int64 nUserID);
        List<RouteLocation> BUWiseGets(int BUID, Int64 nUserID);
        List<RouteLocation> Gets(int neEnumRouteLocation, Int64 nUserID);
        string Delete(RouteLocation oRouteLocation, Int64 nUserID);
        RouteLocation Save(RouteLocation oRouteLocation, Int64 nUserID);
    }
    #endregion
}  