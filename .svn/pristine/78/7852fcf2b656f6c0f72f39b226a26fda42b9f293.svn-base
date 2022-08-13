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
    #region Department
    public class Department : BusinessObject
    {
        public Department()
        {
            DepartmentID = 0;
            Code = "";
            Name = "";
            Description = "";
            ParentID = 0;
            Sequence = 0;
            RequiredPerson = 0;
            ErrorMessage = "";
            IsActive = true;
            PCode = "";

            BusinessUnitID = 0;
            LocationID = 0;
            BusinessUnitIDs = "";
            LocationIDs = "";
            NameInBangla = "";
        }

        #region Properties
        public int DepartmentID { get; set; }
        public string Code { get; set; }
        public string NameInBangla { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentID { get; set; }
        public int Sequence { get; set; }
        public int RequiredPerson { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        public string ParentNodeName { get; set; }
        #endregion

        #region Derived Property
        public string PCode { get; set; }
        public List<Department> ChildNodes { get; set; }
        public List<Department> Departments { get; set; }
        public string SelectedParentDepartment { get; set; }
        public TDepartment TDepartment { get; set; }
        public bool IsChild { get; set; }
        public bool IsSibling { get; set; }
        private int[] _sKeys;

        public int[] Permissions
        {
            get { return _sKeys; }
            set { _sKeys = value; }
        }
        public string DepartmentNameCode
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
        public int BusinessUnitID { get; set; }
        public int LocationID { get; set; }
        public string BusinessUnitIDs { get; set; }
        public string LocationIDs { get; set; }
        #endregion

        #region Functions
        public static List<Department> Gets(long nUserID)
        {
            return Department.Service.Gets(nUserID);
        }
        public static List<Department> Gets(string sSQL, long nUserID)
        {
            return Department.Service.Gets(sSQL, nUserID);
        }
        public static List<Department> GetsDeptWithParent(string DeptName, long nUserID)
        {
            return Department.Service.GetsDeptWithParent(DeptName, nUserID);
        }
        public Department Get(int id, long nUserID)
        {
            return Department.Service.Get(id, nUserID);
        }
        public Department Save(long nUserID)
        {
            return Department.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return Department.Service.Delete(id, nUserID);
        }
        public static List<Department> GetDepartmentHierarchy(string sDepartmentIDs, long nUserID)
        {
            return Department.Service.GetDepartmentHierarchy(sDepartmentIDs, nUserID);
        }

        public static List<Department> GetsXL(string sSQL, long nUserID)
        {
            return Department.Service.GetsXL(sSQL, nUserID);
        }

        #endregion

        #region Non DB Function
        public static string IDInString(List<Department> oDepartment)
        {
            string sReturn = "";
            if (oDepartment != null)
            {
                foreach (Department oItem in oDepartment)
                {
                    sReturn = sReturn + oItem.DepartmentID.ToString() + ",";
                }
                if (sReturn == "") return "";
                sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            }
            return sReturn;
        }
        public static int GetIndex(List<Department> oDepartments, int nDepartmentID)
        {
            int index = -1, i = 0;

            foreach (Department oItem in oDepartments)
            {
                if (oItem.DepartmentID == nDepartmentID)
                {
                    index = i; break;
                }
                i++;
            }
            return index;
        }
        #endregion

        #region ServiceFactory
        internal static IDepartmentService Service
        {
            get { return (IDepartmentService)Services.Factory.CreateService(typeof(IDepartmentService)); }
        }

        #endregion
    }
    #endregion

    #region Departments
    public class Departments : IndexedBusinessObjects
    {
        #region Collection Class Methods
        public void Add(Department item)
        {
            base.AddItem(item);
        }
        public void Remove(Department item)
        {
            base.RemoveItem(item);
        }
        public Department this[int index]
        {
            get { return (Department)GetItem(index); }
        }
        public int GetIndex(int id)
        {
            return base.GetIndex(new ID(id));
        }
        #endregion
    }
    #endregion

    #region IDepartment interface

    public interface IDepartmentService
    {
        Department Get(int id, Int64 nUserID);
        List<Department> Gets(Int64 nUserID);
        List<Department> Gets(string sSQl, Int64 nUserID);
        List<Department> GetsDeptWithParent(string DepatName, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        Department Save(Department oDepartment, Int64 nUserID);
        List<Department> GetDepartmentHierarchy(string sDepartmentIDs, Int64 nUserID);

        List<Department> GetsXL(string sSQl, Int64 nUserID);
    }
    #endregion

    #region TDepartment
    //this is a derive class that use only for display user menu(j tree tree architecture reference : http://www.jeasyui.com/documentation/index.php# )
    public class TDepartment
    {
        public TDepartment()
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
            IsActiveInString = "";
        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public string state { get; set; }           //: node state, 'open' or 'closed', default is 'open'. When set to 'closed', the node have children nodes and will load them from remote site        
        public string attributes { get; set; }      //: custom attributes can be added to a node // attributes set a sting that hold action name & controller namr seperated by '~'
        public int parentid { get; set; }
        public string code { get; set; }
        public int sequence { get; set; }
        public int requiredPerson { get; set; }
        public bool isActive { get; set; }
        public string Description { get; set; }
        public string IsActiveInString { get; set; }
        public IEnumerable<TDepartment> children { get; set; }//: an array nodes defines some children nodes
    }
    #endregion


}
