using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{

    #region ConsumptionUnit

    public class ConsumptionUnit : BusinessObject
    {
        public ConsumptionUnit()
        {
            ConsumptionUnitID = 0;
            Name = "";
            Note = "";
            ErrorMessage = "";
            IsLastLayer = false;
            ParentConsumptionUnitID = 0;
            CUSequence = 0;
            BUWiseConsumptionUnits = new List<BUWiseConsumptionUnit>();
            BusinessUnits = new List<BusinessUnit>();
        }

        #region Properties

        public int ConsumptionUnitID { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        public string ErrorMessage { get; set; }

        public int ParentConsumptionUnitID { get; set; }

        public int CUSequence { get; set; }
        #endregion

        #region Derived Property
        public List<ConsumptionUnit> ConsumptionUnits { get; set; }
        public List<BUWiseConsumptionUnit> BUWiseConsumptionUnits { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }
        public int BUID { get; set; }
        public Company Company { get; set; }
        public bool IsLastLayer { get; set; }
        #endregion

        #region Functions

        public static List<ConsumptionUnit> Gets(long nUserID)
        {
            return ConsumptionUnit.Service.Gets(nUserID);
        }

        public static List<ConsumptionUnit> Gets(string sSQL, long nUserID)
        {
            return ConsumptionUnit.Service.Gets(sSQL, nUserID);
        }

        public ConsumptionUnit Get(int id, long nUserID)
        {
            return ConsumptionUnit.Service.Get(id, nUserID);
        }

        public ConsumptionUnit Save(long nUserID)
        {
            return ConsumptionUnit.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ConsumptionUnit.Service.Delete(id, nUserID);
        }

        public string ChangeGroup(string sSql, long nUserID)
        {
            return ConsumptionUnit.Service.ChangeGroup(sSql, nUserID);
        }

        public string RefreshSequence(long nUserID)
        {
            return ConsumptionUnit.Service.RefreshSequence(this, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IConsumptionUnitService Service
        {
            get { return (IConsumptionUnitService)Services.Factory.CreateService(typeof(IConsumptionUnitService)); }
        }

        #endregion
    }
    #endregion

    #region Report Study
    public class ConsumptionUnitList : List<ConsumptionUnit>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IConsumptionUnit interface

    public interface IConsumptionUnitService
    {

        ConsumptionUnit Get(int id, Int64 nUserID);

        List<ConsumptionUnit> Gets(Int64 nUserID);

        List<ConsumptionUnit> Gets(string sSQL, Int64 nUserID);

        string Delete(int id, Int64 nUserID);

        ConsumptionUnit Save(ConsumptionUnit oConsumptionUnit, Int64 nUserID);

        string ChangeGroup(string sSql, Int64 nUserID);

        string RefreshSequence(ConsumptionUnit oConsumptionUnit, Int64 nUserID);

    }
    #endregion

    public class TConsumptionUnit
    {
        public TConsumptionUnit()
        {
            id = 0;
            text = "";
            state = "";
            attributes = "";
            parentid = 0;
            CUSequence = 0;
            Description = "";
            IsLastLayer = false;
            AssetTypeInString = "";
            IsActive = true;
            productId = 0;
            DrAccountHeadID = 0;
            DrAccountHeadName = "";
            CrAccountHeadID = 0;
            CrAccountHeadName = "";

        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public string state { get; set; }           //: node state, 'open' or 'closed', default is 'open'. When set to 'closed', the node have children nodes and will load them from remote site        
        public string attributes { get; set; }      //: custom attributes can be added to a node // attributes set a sting that hold action name & controller namr seperated by '~'
        public int parentid { get; set; }
        public int CUSequence { get; set; }
        public string Description { get; set; }
        public bool IsLastLayer { get; set; }
        public string AssetTypeInString { get; set; }
        public bool IsActive { get; set; }
        public int productId { get; set; }
        public int DrAccountHeadID { get; set; }
        public string DrAccountHeadName { get; set; }
        public int CrAccountHeadID { get; set; }
        public bool IsApplyGroup { get; set; }
        public bool IsApplyCategory { get; set; }
        public bool ApplyGroup_IsShow { get; set; }
        public bool ApplyProductType_IsShow { get; set; }/// for Account Effece
        public bool ApplyProperty_IsShow { get; set; }
        public bool ApplyPlantNo_IsShow { get; set; }
        public string CrAccountHeadName { get; set; }
        public IEnumerable<TConsumptionUnit> children { get; set; }//: an array nodes defines some children nodes
    }
   
}
