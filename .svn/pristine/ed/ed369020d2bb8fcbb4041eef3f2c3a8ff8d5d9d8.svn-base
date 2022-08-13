using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region FabricMachineType
    public class FabricMachineType : BusinessObject
    {
        public FabricMachineType()
        {
            FabricMachineTypeID = 0;
            Name = "";
            Brand = "";
            ParentID = 0;
            Note = "";
            ErrorMessage = "";
            FMTTree = new FMTTree();
        }

        #region Property
        public int FabricMachineTypeID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public int ParentID { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public FMTTree FMTTree { get; set; }
        public string ParentName { get; set; }
        public string ChildWithParent
        {
            get
            {
                return this.Name + " - " + this.ParentName;
            }
        }
        #endregion

        #region Functions
        public static List<FabricMachineType> Gets(long nUserID)
        {
            return FabricMachineType.Service.Gets(nUserID);
        }
        public static List<FabricMachineType> Gets(string sSQL, long nUserID)
        {
            return FabricMachineType.Service.Gets(sSQL, nUserID);
        }
        public FabricMachineType Get(int id, long nUserID)
        {
            return FabricMachineType.Service.Get(id, nUserID);
        }
        public FabricMachineType Save(long nUserID)
        {
            return FabricMachineType.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FabricMachineType.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricMachineTypeService Service
        {
            get { return (IFabricMachineTypeService)Services.Factory.CreateService(typeof(IFabricMachineTypeService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricMachineType interface
    public interface IFabricMachineTypeService
    {
        FabricMachineType Get(int id, Int64 nUserID);
        List<FabricMachineType> Gets(Int64 nUserID);
        List<FabricMachineType> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FabricMachineType Save(FabricMachineType oFabricMachineType, Int64 nUserID);
    }
    #endregion

    #region FMTTree
    public class FMTTree
    {
        public FMTTree()
        {
            id = 0;
            text = "";
            state = "";
            attributes = "";
            parentid = 0;
            code = "";
            FabricMachineTypeID = 0;
            Name = "";
            Brand = "";
            ParentID = 0;
            Note = "";
            
        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public string state { get; set; }           //: node state, 'open' or 'closed', default is 'open'. When set to 'closed', the node have children nodes and will load them from remote site        
        public string attributes { get; set; }      //: custom attributes can be added to a node // attributes set a sting that hold action name & controller namr seperated by '~'
        public int parentid { get; set; }
        public string code { get; set; }
        public int FabricMachineTypeID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public int ParentID { get; set; }
        public string Note { get; set; }
        public List<FMTTree> children { get; set; }//: an array nodes defines some children nodes
    }
    #endregion
}
