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
	#region ReportConfigure  
	public class ReportConfigure : BusinessObject
	{	
		public ReportConfigure()
		{
			ReportConfigureID = 0; 
			UserID = 0; 
			FieldNames = ""; 
			CaptionNames = "";
            ReportType = EnumReportType.Order_Tracking_ColorView;
            ReportConfigures = new List<ReportConfigure>();
            Selected = false;
            IsExist = false;
			ErrorMessage = "";
            ColumnWidth = 0;
		}

		#region Property
		public int ReportConfigureID { get; set; }
		public int UserID { get; set; }
		public string FieldNames { get; set; }
		public string CaptionNames { get; set; }
        public EnumReportType ReportType { get; set; }
        public int ReportTypeInInt { get; set; }
        public bool Selected { get; set; }
        public bool IsExist { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public List<ReportConfigure> ReportConfigures { get; set; }
        public float ColumnWidth { get; set; }
		#endregion 

		#region Functions 
		public static List<ReportConfigure> Gets(long nUserID)
		{
			return ReportConfigure.Service.Gets(nUserID);
		}
		public static List<ReportConfigure> Gets(string sSQL, long nUserID)
		{
			return ReportConfigure.Service.Gets(sSQL,nUserID);
		}
		public ReportConfigure Get(int nReportType, long nUserID)
		{
			return ReportConfigure.Service.Get(nReportType,nUserID);
		}
		public ReportConfigure Save(long nUserID)
		{
			return ReportConfigure.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return ReportConfigure.Service.Delete(id,nUserID);
		}

        #region Report Column Map
        public static List<ReportConfigure> Gets(EnumReportType eReportConfigureType)
        {
            ReportConfigure oReportConfigure = new ReportConfigure();
            List<ReportConfigure> oReportConfigures = new List<ReportConfigure>();
            if (eReportConfigureType == EnumReportType.FNMachine)
            {
                #region Order Recap
                oReportConfigure = new ReportConfigure(); oReportConfigure.FieldNames = "FNMachineTypeSt"; oReportConfigure.CaptionNames = "Machine Type"; oReportConfigures.Add(oReportConfigure);
                oReportConfigure = new ReportConfigure(); oReportConfigure.FieldNames = "Name"; oReportConfigure.CaptionNames = "Name"; oReportConfigures.Add(oReportConfigure);
                oReportConfigure = new ReportConfigure(); oReportConfigure.FieldNames = "Code"; oReportConfigure.CaptionNames = "Code"; oReportConfigures.Add(oReportConfigure);
                oReportConfigure = new ReportConfigure(); oReportConfigure.FieldNames = "Note"; oReportConfigure.CaptionNames = "Note"; oReportConfigures.Add(oReportConfigure);
                oReportConfigure = new ReportConfigure(); oReportConfigure.FieldNames = "NoOfBathString"; oReportConfigure.CaptionNames = "No Of Bath"; oReportConfigures.Add(oReportConfigure);
                oReportConfigure = new ReportConfigure(); oReportConfigure.FieldNames = "FreeTime"; oReportConfigure.CaptionNames = "Free Time"; oReportConfigures.Add(oReportConfigure);
                oReportConfigure = new ReportConfigure(); oReportConfigure.FieldNames = "IsAtiveString"; oReportConfigure.CaptionNames = "Is Active"; oReportConfigures.Add(oReportConfigure);
                
                #endregion
            }

            return oReportConfigures;
        }
        public static bool HasConfigure(string[] aReportColumns, string sColumnName)
        {
            for (int i = 0; i < aReportColumns.Length; i++)
            {
                if (aReportColumns[i] == sColumnName)
                {
                    return true;
                }
            }
            return false;
        }
        public static float HasConfigureAndGetWidth(List<ReportConfigure> oReportConfigures, string sColumnName)
        {
            for (int i = 0; i < oReportConfigures.Count; i++)
            {
                if (oReportConfigures[i].FieldNames == sColumnName)
                {
                    return oReportConfigures[i].ColumnWidth;
                }
            }
            return 0;
        }
        #endregion


		#endregion

		#region ServiceFactory
		internal static IReportConfigureService Service
		{
			get { return (IReportConfigureService)Services.Factory.CreateService(typeof(IReportConfigureService)); }
		}
		#endregion
	}
	#endregion

	#region IReportConfigure interface
	public interface IReportConfigureService 
	{
        ReportConfigure Get(int nReportType, Int64 nUserID); 
		List<ReportConfigure> Gets(Int64 nUserID);
		List<ReportConfigure> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		ReportConfigure Save(ReportConfigure oReportConfigure, Int64 nUserID);
	}
	#endregion
}
