using ICS.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    public class EmployeeActivityCategory : BusinessObject
    {
        public EmployeeActivityCategory()
        {
            EACID = 0;
            Description = "";
            ErrorMessage = "";
        }
        #region Properties
        public int EACID { get; set; }
        public string Description { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions
        public static List<EmployeeActivityCategory> Gets(int nUserID)
        {
            return EmployeeActivityCategory.Service.Gets(nUserID);
        }
        public EmployeeActivityCategory Save(Int64 nUserID)
        {
            return EmployeeActivityCategory.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return EmployeeActivityCategory.Service.Delete(nId, nUserID);
        }
        public static List<EmployeeActivityCategory> Gets(string sSQL, long nUserID)
        {
            return EmployeeActivityCategory.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeActivityCategoryService Service
        {
            get { return (IEmployeeActivityCategoryService)Services.Factory.CreateService(typeof(IEmployeeActivityCategoryService)); }
        }
        #endregion

        #region IEmployeeActivityCategoryService interface
        public interface IEmployeeActivityCategoryService
        {
            List<EmployeeActivityCategory> Gets(Int64 nUserID);

            EmployeeActivityCategory Save(EmployeeActivityCategory oEmployeeActivityCategory, Int64 nUserID);

            string Delete(int id, long nUserID);


            List<EmployeeActivityCategory> Gets(string sSQL, long nUserID);
        }
        #endregion
    }
}
