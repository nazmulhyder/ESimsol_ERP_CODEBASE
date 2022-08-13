using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.BusinessObjects
{
    #region LotBaseTest
    public class LotBaseTest :BusinessObject
    {
        public LotBaseTest()
        {
        LotBaseTestID =0;
        BUID = 0;
        Name =string.Empty;
        Activity = true;
        ErrorMessage = string.Empty;
        Params = string.Empty;
        IsRaw = true;
        }
        #region Properties
        public int LotBaseTestID {get; set;}
        public int  BUID {get; set;}
        public string  Name {get; set;}
        public bool Activity { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public bool IsRaw { get; set; }
        #endregion
        #region Derived properties
        #endregion
        #region Functions

        public static LotBaseTest Get(int nLotBaseTestID, long nUserID)
        {
            return LotBaseTest.Service.Get(nLotBaseTestID, nUserID);
        }
        public static List<LotBaseTest> Gets(string sSQL, long nUserID)
        {
            return LotBaseTest.Service.Gets(sSQL, nUserID);
        }
        public LotBaseTest IUD(int nDBOperation, long nUserID)
        {
            return LotBaseTest.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ILotBaseTestService Service
        {
            get { return (ILotBaseTestService)Services.Factory.CreateService(typeof(ILotBaseTestService)); }
        }

        #endregion
    }
    #endregion
    #region  LotBaseTest interface
    public interface ILotBaseTestService
    {

        LotBaseTest Get(int nLotBaseTestID, Int64 nUserID);
        List<LotBaseTest> Gets(string sSQL, Int64 nUserID);
        LotBaseTest IUD(LotBaseTest oDUDyeingType, int nDBOperation, Int64 nUserID);

    }
    #endregion
  
}
