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
    #region KnittingYarn
    public class KnittingYarn : BusinessObject
    {
        public KnittingYarn()
        {
            KnittingYarnID = 0;
            KnittingOrderID = 0;
            YarnID = 0;
            Remarks = "";
            ErrorMessage = "";
        }

        #region Property
        public int KnittingYarnID { get; set; }
        public int KnittingOrderID { get; set; }
        public int YarnID { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string YarnName { get; set; }
        public string YarnCode { get; set; }
        #endregion

        #region Functions
        public static List<KnittingYarn> Gets(long nUserID)
        {
            return KnittingYarn.Service.Gets(nUserID);
        }
        public static List<KnittingYarn> Gets(int id,long nUserID)
        {
            return KnittingYarn.Service.Gets(id,nUserID);
        }
        public static List<KnittingYarn> Gets(string sSQL, long nUserID)
        {
            return KnittingYarn.Service.Gets(sSQL, nUserID);
        }
        public KnittingYarn Get(int id, long nUserID)
        {
            return KnittingYarn.Service.Get(id, nUserID);
        }
        public KnittingYarn Save(long nUserID)
        {
            return KnittingYarn.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return KnittingYarn.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IKnittingYarnService Service
        {
            get { return (IKnittingYarnService)Services.Factory.CreateService(typeof(IKnittingYarnService)); }
        }
        #endregion
    }
    #endregion

    #region IKnittingYarn interface
    public interface IKnittingYarnService
    {
        KnittingYarn Get(int id, Int64 nUserID);
        List<KnittingYarn> Gets(Int64 nUserID);
        List<KnittingYarn> Gets(int id,Int64 nUserID);
        List<KnittingYarn> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        KnittingYarn Save(KnittingYarn oKnittingYarn, Int64 nUserID);
    }
    #endregion
}
