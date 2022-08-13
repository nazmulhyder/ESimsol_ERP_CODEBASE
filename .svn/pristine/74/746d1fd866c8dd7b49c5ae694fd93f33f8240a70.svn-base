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
    #region PolyMeasurement
    
    public class PolyMeasurement : BusinessObject
    {
        public PolyMeasurement()
        {
            PolyMeasurementID = 0;
            Measurement="";
            Note = "";
            Param = "";
            ObjectID = 0;
            ObjectName = "";
            PolyMeasurementType = EnumPolyMeasurementType.None;
            PolyMeasurementTypeInt = 0;//combobox
            Length = 0;
            LengthUnit = "";
            Width = 0;
            WidthUnit = "";
            Thickness = 0;
            ThicknessUnit = "";
            Flap = 0;
            FlapUnit = "";
            Lip = 0;
            LipUnit = "";
            Gusset = 0;
            GussetUnit = "";
            Gusset1 = 0;
            GussetUnit1 = "";

            ErrorMessage = "";
        }

        #region Properties
         
        public int PolyMeasurementID { get; set; }
        public string Measurement { get; set; }
        public string Note { get; set; }

        public EnumPolyMeasurementType PolyMeasurementType { get; set; }
        public int PolyMeasurementTypeInt { get; set; }//combobox
        public double Length { get; set; }
        public string LengthUnit { get; set; }
        public double Width { get; set; }
        public string WidthUnit { get; set; }
        public double Thickness { get; set; }
        public string ThicknessUnit { get; set; }
        public double Flap { get; set; }
        public string FlapUnit { get; set; }
        public double Lip { get; set; }
        public string LipUnit { get; set; }
        public double Gusset { get; set; }
        public string GussetUnit { get; set; }
        public double Gusset1 { get; set; }
        public string GussetUnit1 { get; set; }

        public string Param { get; set; } 
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string PolyMeasurementTypeSt
        {
            get
            {
                return EnumObject.jGet(this.PolyMeasurementType);//combobox
            }
        }
        public bool Selected { get; set; }
        public List<PolyMeasurement> ColorCategories { get; set; }
        public int ObjectID { get; set; }
        public string ObjectName { get; set; }
        #endregion

        #region Functions
        
        public static List<PolyMeasurement> Gets(long nUserID)
        {
            return PolyMeasurement.Service.Gets( nUserID);
        }

        public static List<PolyMeasurement> GetsbyMeasurement(string sMeasurement, long nUserID)
        {
            return PolyMeasurement.Service.GetsbyMeasurement(sMeasurement, nUserID);
        }
        public PolyMeasurement Get(int id, long nUserID)
        {
            return PolyMeasurement.Service.Get(id, nUserID);
        }
        public static List<PolyMeasurement> Gets(string sSQL, long nUserID)
        {           
            return PolyMeasurement.Service.Gets(sSQL, nUserID);
        }
        public PolyMeasurement Save(long nUserID)
        {
            
            return PolyMeasurement.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return PolyMeasurement.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IPolyMeasurementService Service
        {
            get { return (IPolyMeasurementService)Services.Factory.CreateService(typeof(IPolyMeasurementService)); }
        }


        #endregion
    }
    #endregion

    #region IPolyMeasurement interface
     
    public interface IPolyMeasurementService
    {         
        PolyMeasurement Get(int id, Int64 nUserID);
        List<PolyMeasurement> Gets(Int64 nUserID);         
        List<PolyMeasurement> Gets(string sSQL, Int64 nUserID);         
        List<PolyMeasurement> GetsbyMeasurement(string sMeasurement, Int64 nUserID);         
        string Delete(int id, Int64 nUserID);         
        PolyMeasurement Save(PolyMeasurement oPolyMeasurement, Int64 nUserID);
    }
    #endregion
}
