using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region GarmentsClass
    
    public class GarmentsClass : BusinessObject
    {
        public GarmentsClass()
        {
            GarmentsClassID=0;
            ClassName = "";
            ParentClassID = 0;
            Note = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int GarmentsClassID{ get; set; }
         
        public string ClassName{ get; set; }
         
        public int ParentClassID{ get; set; }
         
        public string Note { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        #endregion

        #region Functions

        public static List<GarmentsClass> Gets(long nUserID)
        {
            return GarmentsClass.Service.Gets(nUserID);
        }

        public static List<GarmentsClass> GetsGarmentsClass(long nUserID)
        {
            return GarmentsClass.Service.GetsGarmentsClass(nUserID);
        }


        public static List<GarmentsClass> GetsGarmentsSubClass(long nUserID)
        {
            return GarmentsClass.Service.GetsGarmentsSubClass(nUserID);
        }

        public static List<GarmentsClass> Gets(string sSQL, long nUserID)
        {
            return GarmentsClass.Service.Gets(sSQL, nUserID);
        }

        public GarmentsClass Get(int id, long nUserID)
        {
            
            return GarmentsClass.Service.Get(id, nUserID);
        }

        public GarmentsClass Save(long nUserID)
        {
            return GarmentsClass.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return GarmentsClass.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IGarmentsClassService Service
        {
            get { return (IGarmentsClassService)Services.Factory.CreateService(typeof(IGarmentsClassService)); }
        }

        #endregion
    }
    #endregion

    #region IGarmentsClass interface
     
    public interface IGarmentsClassService
    {
         
        GarmentsClass Get(int id, Int64 nUserID);
         
        List<GarmentsClass> Gets(Int64 nUserID);
         
        List<GarmentsClass> GetsGarmentsClass(Int64 nUserID);
         
        List<GarmentsClass> GetsGarmentsSubClass(Int64 nUserID);
         
        List<GarmentsClass> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        GarmentsClass Save(GarmentsClass oGarmentsClass, Int64 nUserID);
    }
    #endregion

    #region TGarmentsClass
    //this is a derive class that use only for display user menu(j tree tree architecture reference : http://www.jeasyui.com/documentation/index.php# )
    public class TGarmentsClass
    {
        public TGarmentsClass()
        {
            id = 0;
            text = "";
            state = "";
            attributes = "";
            parentid = 0;
            Description = "";
        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public string state { get; set; }           //: node state, 'open' or 'closed', default is 'open'. When set to 'closed', the node have children nodes and will load them from remote site        
        public string attributes { get; set; }      //: custom attributes can be added to a node // attributes set a sting that hold action name & controller namr seperated by '~'
        public int parentid { get; set; }
        public string Description { get; set; }        
        public IEnumerable<TGarmentsClass> children { get; set; }//: an array nodes defines some children nodes
    }
    #endregion
}
