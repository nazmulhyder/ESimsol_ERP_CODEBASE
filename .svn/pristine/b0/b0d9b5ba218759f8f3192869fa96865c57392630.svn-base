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
    #region FabricQCGrade
    public class FabricQCGrade
    {
        public FabricQCGrade()
        {
            FabricQCGradeID = 0;
            Name = "";
            LastUpdateBy = 0;
            SLNo = 0;
            MinValue = 0;
            MaxValue = 0;
            LastUpdateDateTime = DateTime.Now;
            QCGradeType = EnumFBQCGrade.None;
            ErrorMessage = "";
            LastUpdateByName = "";
        }
        #region Property
        public int FabricQCGradeID { get; set; }
        public string Name { get; set; }
        public int LastUpdateBy { get; set; }
        public EnumFBQCGrade QCGradeType { get; set; }
        public int SLNo { get; set; }
        public EnumExcellColumn GradeSL { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string LastUpdateByName { get; set; }

        #endregion
        #region Derived Property
        public string QCGradeTypeST
        {
            get
            {
                return EnumObject.jGet(this.QCGradeType);
            }
        }
        public string GradeSLInSt
        {
            get
            {
                return EnumObject.jGet(this.GradeSL);
            }
        }
        public string LastUpdateDateTimeInString
        {
            get
            {
                return LastUpdateDateTime.ToString("dd MMM yyyy");
            }
        }
      
        #endregion
        #region Functions
        public static List<FabricQCGrade> Gets(long nUserID)
        {
            return FabricQCGrade.Service.Gets(nUserID);
        }
        public static List<FabricQCGrade> Gets(string sSQL, long nUserID)
        {
            return FabricQCGrade.Service.Gets(sSQL, nUserID);
        }
        public FabricQCGrade Get(int id, long nUserID)
        {
            return FabricQCGrade.Service.Get(id, nUserID);
        }
        public FabricQCGrade Save(long nUserID)
        {
            return FabricQCGrade.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FabricQCGrade.Service.Delete(id, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IFabricQCGradeService Service
        {
            get { return (IFabricQCGradeService)Services.Factory.CreateService(typeof(IFabricQCGradeService)); }
        }
        #endregion
    }
    #endregion
    #region IFabricQCGrade interface
    public interface IFabricQCGradeService
    {
        FabricQCGrade Get(int id, Int64 nUserID);
        List<FabricQCGrade> Gets(Int64 nUserID);
        List<FabricQCGrade> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FabricQCGrade Save(FabricQCGrade oFabricQCGrade, Int64 nUserID);
    }
    #endregion
}
