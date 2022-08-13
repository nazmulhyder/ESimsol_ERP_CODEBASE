using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class TempMeasurementSpecDetail
    {
        public TempMeasurementSpecDetail()
        {
            MeasurementSpecDetailID = 0;
            POM = "";
            DescriptionNote = "";
            Addition = 0;
            Deduction = 0;
            SizeValue1 = 0;
            SizeValue2 = 0;
            SizeValue3 = 0;
            SizeValue4 = 0;
            SizeValue5 = 0;
            SizeValue6 = 0;
            SizeValue7 = 0;
            SizeValue8 = 0;
            SizeValue9 = 0;
            SizeValue10 = 0;
            SizeValue11 = 0;
            SizeValue12 = 0;
            SizeValue13 = 0;
            SizeValue14 = 0;
            SizeValue15 = 0;
            Sequence = 0;
        }

        public int MeasurementSpecDetailID { get; set; }
        public string POM { get; set; }
        public string DescriptionNote { get; set; }
        public double Addition { get; set; }
        public double Deduction { get; set; }
        public double SizeValue1 { get; set; }
        public double SizeValue2 { get; set; }
        public double SizeValue3 { get; set; }
        public double SizeValue4 { get; set; }
        public double SizeValue5 { get; set; }
        public double SizeValue6 { get; set; }
        public double SizeValue7 { get; set; }
        public double SizeValue8 { get; set; }
        public double SizeValue9 { get; set; }
        public double SizeValue10 { get; set; }
        public double SizeValue11 { get; set; }
        public double SizeValue12 { get; set; }
        public double SizeValue13 { get; set; }
        public double SizeValue14 { get; set; }
        public double SizeValue15 { get; set; }
        public int Sequence { get; set; }
    }
}
