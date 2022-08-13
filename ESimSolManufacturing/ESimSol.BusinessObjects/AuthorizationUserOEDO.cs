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

    #region AuthorizationUserOEDO
    public class AuthorizationUserOEDO : BusinessObject
    {
        public AuthorizationUserOEDO()
        {
            AUOEDOID = 0;
            UserID = 0;
            AWUOEDBID = 0;
            IsActive = true;
            TriggerParentType = EnumTriggerParentsType.None;

        }

        #region Properties
        public int AUOEDOID { get; set; }
        public int UserID { get; set; }
        public int AWUOEDBID { get; set; }
        public bool IsMTRApply { get; set; }
        public bool IsActive { get; set; }
        public string sString { get; set; }

        #endregion

        #region Derive Property
        public string ErrorMessage { get; set; }
        public string DBObjectName { get; set; }
        public string OEName { get; set; }
        public EnumOperationFunctionality OEValue { get; set; }
        public EnumTriggerParentsType TriggerParentType { get; set; }
        public int WorkingUnitID { get; set; }
        public string WorkingUnitName { get; set; }
        public string UserName { get; set; }
        public string UserLocation { get; set; }
        public List<AuthorizationUserOEDO> AuthorizationUserOEDODBOforSearch { get; set; }
        public List<AuthorizationUserOEDO> AuthorizationUserOEDOTPTforSearch { get; set; }
        public List<AuthorizationUserOEDO> AuthorizationUserOEDOOEVforSearch { get; set; }
        public List<AuthorizationUserOEDO> AuthorizationUserOEDOWUforSearch { get; set; }

        public string SearchedString { get; set; }
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

        public string MTRApply
        {
            get
            {
                if (IsMTRApply)
                {
                    return "Applicable";
                }
                else
                {
                    return "Not Applicable";
                }

            }

        }

        public string TriggerParentTypeInString
        {
            get
            {
                return TriggerParentType.ToString();
            }
        }
        public string OEValueInString
        {
            get
            {
                return OEValue.ToString();
            }
        }

        public int OEValueInInt { get; set; }
        #endregion
        #region Functions
        public static List<AuthorizationUserOEDO> Gets(long nUserID)
        {
            return AuthorizationUserOEDO.Service.Gets(nUserID);
        }
        public static List<AuthorizationUserOEDO> Gets(string sSQL, long nUserID)
        {
            return AuthorizationUserOEDO.Service.Gets(sSQL, nUserID);
        }
        public static List<AuthorizationUserOEDO> GetsByUser(int ID, long nUserID)
        {
            return AuthorizationUserOEDO.Service.GetsByUser(ID, nUserID);
        }
        public static AuthorizationUserOEDO Get(int id, long nUserID)
        {
            return AuthorizationUserOEDO.Service.Get(id, nUserID);
        }

        public static List<AuthorizationUserOEDO> Save(AuthorizationUserOEDO oAuthorizationUserOEDO, long nUserID)
        {
            return AuthorizationUserOEDO.Service.Save(oAuthorizationUserOEDO, nUserID);

        }

        public string Delete(long nUserID)
        {
            return AuthorizationUserOEDO.Service.Delete(this, nUserID);

        }

        #endregion

        #region ServiceFactory
        internal static IAuthorizationUserOEDOService Service
        {
            get { return (IAuthorizationUserOEDOService)Services.Factory.CreateService(typeof(IAuthorizationUserOEDOService)); }
        }

        #endregion
    }
    #endregion

    #region AuthorizationUserOEDOs
    public class AuthorizationUserOEDOs : IndexedBusinessObjects
    {
        #region Collection Class Methods
        public void Add(AuthorizationUserOEDO item)
        {
            base.AddItem(item);
        }
        public void Remove(AuthorizationUserOEDO item)
        {
            base.RemoveItem(item);
        }
        public AuthorizationUserOEDO this[int index]
        {
            get { return (AuthorizationUserOEDO)GetItem(index); }
        }
        public int GetIndex(int id)
        {
            return base.GetIndex(new ID(id));
        }
        #endregion
    }
    #endregion

    #region IAuthorizationUserOEDO interface

    public interface IAuthorizationUserOEDOService
    {
        AuthorizationUserOEDO Get(int id, Int64 nUserID);
        List<AuthorizationUserOEDO> Gets(Int64 nUserID);
        string Delete(AuthorizationUserOEDO oAuthorizationUserOEDO, Int64 nUserID);
        List<AuthorizationUserOEDO> Save(AuthorizationUserOEDO oAuthorizationUserOEDO, Int64 nUserID);
        List<AuthorizationUserOEDO> Gets(string sSQL, Int64 nUserID);
        List<AuthorizationUserOEDO> GetsByUser(int ID, Int64 nUserID);

    }
    #endregion

}
