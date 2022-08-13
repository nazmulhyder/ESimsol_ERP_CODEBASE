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
    #region FabricBeamFinish

    public class FabricBeamFinish : BusinessObject
    {
        public FabricBeamFinish()
        {
            FabricBeamFinishID = 0;
            FESDID = 0;
            ReadyDate = DateTime.Now;
            Qty = 0;
            LengthFinish = 0;
            Note = "";
            ErrorMessage = "";
            DyeingOrderID = 0;
            FEOSID = 0;
            BeamNo = "";
            IsFinish = false;
            FSCDID = 0;
            ExeNo = "";
            ContractorID = 0;
            CustomerName = "";
            TFlength = 0;
        }

        #region Properties

        public int FabricBeamFinishID { get; set; }
        public int FESDID { get; set; }
        public int DyeingOrderID { get; set; }
        public int FEOSID { get; set; }
        public DateTime ReadyDate { get; set; }
        public double TFlength { get; set; }
        public double Qty { get; set; }
        public double LengthFinish { get; set; }
        public string Note { get; set; }
        public string BeamNo { get; set; }
        public bool IsFinish { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public int FSCDID { get; set; }
        public string ExeNo { get; set; }
        public int ContractorID { get; set; }
        public string CustomerName { get; set; }
        public string ReadyDateInString
        {
            get
            {
                if (ReadyDate == DateTime.MinValue) return "";
                return ReadyDate.ToString("dd MMM yyyy");
            }
        }
        public string IsFinishInString
        {
            get
            {
                if (IsFinish == true) return "Yes";
                return "No";
            }
        }
        #endregion

        #region Functions

        public static List<FabricBeamFinish> Gets(long nUserID)
        {
            return FabricBeamFinish.Service.Gets(nUserID);
        }

        public FabricBeamFinish Get(int id, long nUserID)
        {
            return FabricBeamFinish.Service.Get(id, nUserID);
        }

        public FabricBeamFinish Save(long nUserID)
        {
            return FabricBeamFinish.Service.Save(this, nUserID);
        }

        public static List<FabricBeamFinish> Gets(string sSQL, long nUserID)
        {
            return FabricBeamFinish.Service.Gets(sSQL, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return FabricBeamFinish.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory


        internal static IFabricBeamFinishService Service
        {
            get { return (IFabricBeamFinishService)Services.Factory.CreateService(typeof(IFabricBeamFinishService)); }
        }

        #endregion
    }
    #endregion

    #region IFabricBeamFinish interface

    public interface IFabricBeamFinishService
    {
        FabricBeamFinish Get(int id, Int64 nUserID);

        List<FabricBeamFinish> Gets(Int64 nUserID);

        List<FabricBeamFinish> Gets(string sSQL, Int64 nUserID);

        string Delete(int id, Int64 nUserID);

        FabricBeamFinish Save(FabricBeamFinish oFabricBeamFinish, Int64 nUserID);

    }
    #endregion
}
