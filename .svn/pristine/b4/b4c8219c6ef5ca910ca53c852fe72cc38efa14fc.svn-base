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
    #region Location
    public class Location : BusinessObject
    {
        public Location()
        {
            LocationID = 0;
            LocCode = "";
            Name = "";
            NameInBangla = "";
            ShortName = "";
            Description = "";
            ParentID = 0;
            IsActive = true;
            LocationType = EnumLocationType.None;
            
            LocationNameCode = "";
            BusinessUnitID = 0;
            BusinessUnitIDs = "";
            ParentName = "";
            AreaID = 0;
            AreaName = "";
            LocationName = "";
            ErrorMessage = "";
        }

        #region Properties
        public int LocationID { get; set; }
        public string LocCode { get; set; }
        public string NameInBangla { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public int ParentID { get; set; }
        public bool IsActive { get; set; }
        public EnumLocationType LocationType { get; set; }
        public int LocationTypeInInt {get;set;}
        public string ErrorMessage { get; set; }
        public string ParentNodeName { get; set; }
        public int BusinessUnitID { get; set; }
        public string ParentName { get; set; }
        public int AreaID { get; set; }
        public string AreaName { get; set; }
        public string LocationName { get; set; }
        #endregion

        #region Derived Property
        public List<Location> ChildNodes { get; set; }
        public List<Location> Locations { get; set; }
        public string SelectedParentLocation { get; set; }
        public bool IsChild { get; set; }
        public bool IsSibling { get; set; }
        public string LocationNameCode { get; set; }
        public string Activity { get { return this.IsActive ? "Active" : "Inactive"; } }
        public string LocationTypeName { get { return this.LocationType==EnumLocationType.None?this.Name: this.LocationType.ToString(); } }
        public TLocation TLocation { get; set; }
        public string BusinessUnitIDs { get; set; }
        #endregion

        #region Functions
        public static List<Location> GetsAll(int nUserID)
        {
            return Location.Service.GetsAll(nUserID);
        }
        public static List<Location> GetsIncludingStore(int nUserID)
        {
            return Location.Service.GetsIncludingStore(nUserID);
        }
        public Location Get(int id, int nUserID)
        {
            return Location.Service.Get(id, nUserID);
        }
        public Location Save(int nUserID)
        {
            return Location.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return Location.Service.Delete(id, nUserID);
        }
        public static List<Location> Gets(int nUserID)
        {
            return Location.Service.Gets(nUserID);
        }
        public static List<Location> GetsByType(EnumLocationType eLocationType, int nUserId)
        {
            return Location.Service.GetsByType(eLocationType, nUserId);
        }
        public static List<Location> Gets(string sSQL, int nUserID)
        {
            return Location.Service.Gets(sSQL, nUserID);
        }
        public static List<Location> GetsByCodeOrName(Location oLocation, int nUserID)
        {
            return Location.Service.GetsByCodeOrName(oLocation, nUserID);
        }
        public static List<Location> GetsByCodeOrNamePick(Location oLocation, int nUserID)
        {
            return Location.Service.GetsByCodeOrNamePick(oLocation, nUserID);
        }
        public static List<Location> GetsByCode(Location oLocation, int nUserID)
        {
            return Location.Service.GetsByCode(oLocation, nUserID);
        }
        
        #endregion

        #region Non DB Function
        public static string IDInString(List<Location> oLocation)
        {
            string sReturn = "";
            if (oLocation != null)
            {
                foreach (Location oItem in oLocation)
                {
                    sReturn = sReturn + oItem.LocationID.ToString() + ",";
                }
                if (sReturn == "") return "";
                sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            }
            return sReturn;
        }
        public static int GetIndex(List<Location> oLocations, int nLocationID)
        {
            int index = -1, i = 0;

            foreach (Location oItem in oLocations)
            {
                if (oItem.LocationID == nLocationID)
                {
                    index = i; break;
                }
                i++;
            }
            return index;
        }
        #endregion

        #region ServiceFactory
        internal static ILocationService Service
        {
            get { return (ILocationService)Services.Factory.CreateService(typeof(ILocationService)); }
        }
        #endregion
    }
    #endregion


    #region ILocation interface
    public interface ILocationService
    {
        Location Get(int id, int nUserID);
        List<Location> Gets(int nUserID);
        List<Location> GetsByType(EnumLocationType eLocationType, int nUserId);
        List<Location> Gets(string sSQl, int nUserID);
        List<Location> GetsByCodeOrName(Location oLocation, int nUserID);
        List<Location> GetsByCodeOrNamePick(Location oLocation, int nUserID);
        List<Location> GetsByCode(Location oLocation, int nUserID);
        string Delete(int id, int nUserID);
        Location Save(Location oLocation, int nUserID);
        List<Location> GetsAll(int nUserID);
        List<Location> GetsIncludingStore(int nUserID);
    }
    #endregion

    #region TLocation
    //this is a derive class that use only for display user menu(j tree tree architecture reference : http://www.jeasyui.com/documentation/index.php# )
    public class TLocation
    {
        public TLocation()
        {
            id = 0;
            text = "";
            state = "";
            attributes = "";
            parentid = 0;
            code = "";
            Description = "";
            Activity = "";
            IsActive = true;
            LocationType = EnumLocationType.None;
            BLID = 0;
            LocationTypeName = "";
        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public string state { get; set; }           //: node state, 'open' or 'closed', default is 'open'. When set to 'closed', the node have children nodes and will load them from remote site        
        public string attributes { get; set; }      //: custom attributes can be added to a node // attributes set a sting that hold action name & controller namr seperated by '~'
        public int parentid { get; set; }
        public string code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsChecked { get; set; }
        public string Activity { get; set; }
        public EnumLocationType LocationType { get; set; }
        public int BLID { get; set; }
        public string LocationTypeName { get; set; }
        public IEnumerable<TLocation> children { get; set; }//: an array nodes defines some children nodes
    }


    #endregion

}