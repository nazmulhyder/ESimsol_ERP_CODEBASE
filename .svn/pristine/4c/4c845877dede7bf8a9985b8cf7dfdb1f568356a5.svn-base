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
    #region BodyMeasure
    public class BodyMeasure : BusinessObject
    {
        public BodyMeasure()
        {
            BodyMeasureID = 0;
            CostSheetID = 0;
            BodyPartID = 0;
            MeasureInCM = 0;
            GSM = 0;
            Remarks = "";
            BodyPartCode = "";
            BodyPartName = "";
            ErrorMessage = "";
        }

        #region Properties
        public int BodyMeasureID { get; set; }
        public int CostSheetID { get; set; }
        public int BodyPartID { get; set; }
        public double MeasureInCM { get; set; }
        public double GSM { get; set; }
        public string Remarks { get; set; }
        public string BodyPartCode { get; set; }
        public string BodyPartName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<BodyMeasure> BodyMeasures { get; set; }
        #endregion

        #region Functions
        public static List<BodyMeasure> Gets(long nUserID)
        {
            return BodyMeasure.Service.Gets(nUserID);
        }
        public static List<BodyMeasure> Gets(int nCostSheetID, long nUserID)
        {
            return BodyMeasure.Service.Gets(nCostSheetID, nUserID);
        }
        public BodyMeasure Get(int id, long nUserID)
        {
            return BodyMeasure.Service.Get(id, nUserID);
        }
        public BodyMeasure Save(long nUserID)
        {
            return BodyMeasure.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return BodyMeasure.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IBodyMeasureService Service
        {
            get { return (IBodyMeasureService)Services.Factory.CreateService(typeof(IBodyMeasureService)); }
        }

        #endregion
    }
    #endregion

    #region IBodyMeasure interface

    public interface IBodyMeasureService
    {
        BodyMeasure Get(int id, Int64 nUserID);
        List<BodyMeasure> Gets(Int64 nUserID);
        List<BodyMeasure> Gets(int nCostSheetID, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        BodyMeasure Save(BodyMeasure oBodyMeasure, Int64 nUserID);
    }
    #endregion
}