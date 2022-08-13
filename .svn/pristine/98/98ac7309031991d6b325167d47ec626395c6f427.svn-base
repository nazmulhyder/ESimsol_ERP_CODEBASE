using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{

    #region Designation

    public class Designation : BusinessObject
    {
        public Designation()
        {
            DesignationID = 0;
            Code = "";
            Name = "";
            HRResponsibilityID = 0;
            Responsibility = "";
            ResponsibilityInBangla = "";
            Description = "";
            ParentID = 0;
            Sequence = 0;
            RequiredPerson = 0;
            ErrorMessage = "";
            IsActive = true;
            Params = "";
            PCode = "";
            NameInBangla = "";
            EmployeeTypeID = 0;
        }

        #region Properties
        public int DesignationID { get; set; }
        public int EmployeeTypeID { get; set; }
        public string Code { get; set; }
        public string NameInBangla { get; set; }
        public string Name { get; set; }
        public int HRResponsibilityID { get; set; }
        public string Responsibility { get; set; }
        public string ResponsibilityInBangla { get; set; }
        public string Description { get; set; }
        public int ParentID { get; set; }
        public int Sequence { get; set; }
        public int RequiredPerson { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        public string ParentNodeName { get; set; }
        public string Params { get; set; }
        #endregion

        #region Derived Property
        public string PCode { get; set; }
        public List<Designation> ChildNodes { get; set; }
        public List<Designation> Designations { get; set; }
        public string SelectedParentDesignation { get; set; }
        public TDesignation TDesignation { get; set; }
        public bool IsChild { get; set; }
        public bool IsSibling { get; set; }
        public string DesignationNameCode
        {
            get
            {
                return Name + "[" + Code + "]";
            }
        }

        public string Activity
        {
            get
            {
                if (IsActive)
                {
                    return "Active";
                }
                else
                {
                    return "Inactive";
                }

            }

        }
        #endregion

        #region Functions

        public static List<Designation> Gets(long nUserID)
        {
            return Designation.Service.Gets(nUserID);
        }
        public static List<Designation> Gets(string sSQL, long nUserID)
        {
            return Designation.Service.Gets(sSQL, nUserID);
        }
        public Designation Get(int id, long nUserID)
        {
            return Designation.Service.Get(id, nUserID);
        }
        public Designation Save(long nUserID)
        {
            return Designation.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return Designation.Service.Delete(id, nUserID);
        }
        public static List<Designation> GetsXL(string sSQL, long nUserID)
        {
            return Designation.Service.GetsXL(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDesignationService Service
        {
            get { return (IDesignationService)Services.Factory.CreateService(typeof(IDesignationService)); }
        }

        #endregion
    }
    #endregion

    #region Designations
    public class Designations : IndexedBusinessObjects
    {
        #region Collection Class Methods
        public void Add(Designation item)
        {
            base.AddItem(item);
        }
        public void Remove(Designation item)
        {
            base.RemoveItem(item);
        }
        public Designation this[int index]
        {
            get { return (Designation)GetItem(index); }
        }
        public int GetIndex(int id)
        {
            return base.GetIndex(new ID(id));
        }
        #endregion
    }
    #endregion

    #region IDesignation interface

    public interface IDesignationService
    {
        Designation Get(int id, Int64 nUserID);
        List<Designation> Gets(Int64 nUserID);
        List<Designation> Gets(string sSQl, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        Designation Save(Designation oDesignation, Int64 nUserID);
        List<Designation> GetsXL(string sSQl, Int64 nUserID);
    }
    #endregion

    #region TDesignation
    //this is a derive class that use only for display user menu(j tree tree architecture reference : http://www.jeasyui.com/documentation/index.php# )
    public class TDesignation
    {
        public TDesignation()
        {
            id = 0;
            text = "";
            state = "";
            attributes = "";
            parentid = 0;
            code = "";
            sequence = 0;
            requiredPerson = 0;
            isActive = true;
            Description = "";
            Responsibility = "";
            HRResponsibilityID = 0;
            ResponsibilityInBangla = "";
        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public string state { get; set; }           //: node state, 'open' or 'closed', default is 'open'. When set to 'closed', the node have children nodes and will load them from remote site        
        public string attributes { get; set; }      //: custom attributes can be added to a node // attributes set a sting that hold action name & controller namr seperated by '~'
        public int parentid { get; set; }
        public string code { get; set; }
        public int HRResponsibilityID { get; set; }
        public string Responsibility { get; set; }
        public string ResponsibilityInBangla { get; set; }
        public int sequence { get; set; }
        public int requiredPerson { get; set; }
        public bool isActive { get; set; }
        public string Description { get; set; }
        public IEnumerable<TDesignation> children { get; set; }//: an array nodes defines some children nodes
    }
    #endregion
}
