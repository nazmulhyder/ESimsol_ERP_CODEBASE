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
    #region BusinessLocation
    public class BusinessLocation : BusinessObject
    {
        public BusinessLocation()
        {
            BusinessLocationID = 0;
            BusinessUnitID = 0;
            LocationID = 0;
            LocationCode = "";
            LocationName = "";
            LocationDescription = "";
            Locationparentid = 0;
            LocationIsActive = false;
            LocationType = EnumLocationType.None;
            BusinessUnitCode = "";
            BusinessUnitName = "";
            BusinessUnitShortName = "";

            ErrorMessage = "";
        }
        #region Properties
        public int BusinessLocationID { get; set; }
        public int BusinessUnitID { get; set; }
     
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string LocationDescription { get; set; }
     
        public bool LocationIsActive { get; set; }
        public EnumLocationType LocationType { get; set; }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public string BusinessUnitShortName { get; set; }
        public int LocationID { get; set; }       //LocationID          //: node id, which is important to load remote data
     
        public int Locationparentid { get; set; }       //LocationParentID 


       
        public string ErrorMessage { get; set; }
        #endregion
        
        #region Derived Property
        public List<BusinessLocation> BusinessLocations { get; set; }
        public string LocationNameCode { get { return this.LocationName + "[" + this.LocationCode + "]"; } }
        public string BusinessUnitNameCode { get { return this.BusinessUnitName + "[" + this.BusinessUnitCode + "]"; } }
        
        #endregion

        #region Functions
        
        public BusinessLocation Get(int id, int nUserID)
        {
            return BusinessLocation.Service.Get(id, nUserID);
        }
        public BusinessLocation Save(int nUserID)
        {
            return BusinessLocation.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return BusinessLocation.Service.Delete(id, nUserID);
        }
        public static List<BusinessLocation> Gets(int nUserID)
        {
            return BusinessLocation.Service.Gets(nUserID);
        }
        public static List<BusinessLocation> Gets(int nBUID, int nUserID)
        {
            return BusinessLocation.Service.Gets(nBUID, nUserID);
        }
        public static List<BusinessLocation> Gets(string sSQL, int nUserID)
        {
            return BusinessLocation.Service.Gets(sSQL, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IBusinessLocationService Service
        {
            get { return (IBusinessLocationService)Services.Factory.CreateService(typeof(IBusinessLocationService)); }
        }
        #endregion
    }
    #endregion

    

    #region IBusinessLocation interface
    public interface IBusinessLocationService
    {
        BusinessLocation Get(int id, int nUserID);
        List<BusinessLocation> Gets(int nUserID);
        List<BusinessLocation> Gets(int nBUID, int nUserID);
        string Delete(int id, int nUserID);
        BusinessLocation Save(BusinessLocation oBusinessLocation, int nUserID);
        
        List<BusinessLocation> Gets(string sSQL, int nUserID);
        
        
    }
    #endregion

    #region TBusinessLocation
    //this is a derive class that use only for display user menu(j tree tree architecture reference : http://www.jeasyui.com/documentation/index.php# )
    public class TBusinessLocation
    {
        public TBusinessLocation()
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
        
        public IEnumerable<TBusinessLocation> children { get; set; }//: an array nodes defines some children nodes
    }


    #endregion
}