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
    #region EmployeeTraining

    public class EmployeeTraining : BusinessObject
    {
        public EmployeeTraining()
        {
            EmployeeTrainingID = 0;
            EmployeeID = 0;
            Sequence = 0;
            CertificateID = 0;
            CertificateNo = "";
            CourseName = "";
            Specification = "";
            CertificateRegNo = "";
            StartDate = DateTime.Now;
            StartDateTrainingFormatType = EnumCustomDateFormat.None;
            EndDateTrainingFormatType = EnumCustomDateFormat.None;
            PassingDateTrainingFormatType = EnumCustomDateFormat.dd_MMM_yyyy;

            EndDate = DateTime.Now;
            Duration = "";
            TrainingDuration = "";
            PassingDate = DateTime.Now;
            Result = "";
            Institution = "";
            CertifyBodyVendor = "";
            Country = "";
            ErrorMessage = "";
            CertificateType = EnumCertificateType.None;

        }

        #region Properties
        public int EmployeeTrainingID { get; set; }
        public int EmployeeID { get; set; }
        public int Sequence { get; set; }
        public int CertificateID { get; set; }
        public string TrainingDuration { get; set; }
        public string CertificateNo { get; set; }

        public string CourseName { get; set; }

        public string Specification { get; set; }
        public string CertificateRegNo { get; set; }
        public DateTime StartDate { get; set; }
        public EnumCustomDateFormat StartDateTrainingFormatType { get; set; }
        public DateTime EndDate { get; set; }
        public EnumCustomDateFormat EndDateTrainingFormatType { get; set; }
        public string Duration { get; set; }

        public DateTime PassingDate { get; set; }
        public EnumCustomDateFormat PassingDateTrainingFormatType { get; set; }
        public string Result { get; set; }

        public string Institution { get; set; }

        public string CertifyBodyVendor { get; set; }

        public string Country { get; set; }

        public string ErrorMessage { get; set; }
        #endregion

        #region Derive
        public string DurationString
        {
            get
            {
                if(TrainingDuration!="")
                {
                    return TrainingDuration;
                }
                else
                {
                    if (this.StartDateTrainingFormatType != EnumCustomDateFormat.None && this.EndDateTrainingFormatType != EnumCustomDateFormat.None)
                    {
                        return this.StartDateInString + " to " + this.EndDateInString;
                    }
                }
                return "";
            }
        }
        public string PassingDateInString
        {
            get
            {
                if (PassingDateTrainingFormatType == EnumCustomDateFormat.dd_MMM_yyyy)
                {
                    return this.PassingDate.ToString("dd MMM yyyy");
                }
                else if (PassingDateTrainingFormatType == EnumCustomDateFormat.MMM_yyyy)
                {
                    return this.PassingDate.ToString("MMM yyyy");
                }
                else
                {
                    return this.PassingDate.ToString("yyyy");
                }
            }
        }

        public string StartDateInString
        {
            get
            {
                if (StartDateTrainingFormatType == EnumCustomDateFormat.dd_MMM_yyyy)
                {
                    return this.StartDate.ToString("dd MMM yyyy");
                }
                else if (StartDateTrainingFormatType == EnumCustomDateFormat.MMM_yyyy)
                {
                    return this.StartDate.ToString("MMM yyyy");
                }
                else
                {
                    return this.StartDate.ToString("yyyy");
                }
            }
        }

        public string EndDateInString
        {
            get
            {
                if (EndDateTrainingFormatType == EnumCustomDateFormat.dd_MMM_yyyy)
                {
                    return this.EndDate.ToString("dd MMM yyyy");
                }
                else if (EndDateTrainingFormatType == EnumCustomDateFormat.MMM_yyyy)
                {
                    return this.EndDate.ToString("MMM yyyy");
                }
                else
                {
                    return this.EndDate.ToString("yyyy");
                }
            }
        }


        public string RequiredFor { get; set; }


        public EnumCertificateType CertificateType { get; set; }
        public string CertificateTypeString
        {
            get
            {
                return CertificateType.ToString();
            }
        }

        #endregion

        #region Functions

        public static List<EmployeeTraining> Gets(int nEmployeeID, long nUserID)
        {
            return EmployeeTraining.Service.Gets(nEmployeeID, nUserID);
        }

        public EmployeeTraining Get(int id, long nUserID) //EmployeeTrainingID
        {
            return EmployeeTraining.Service.Get(id, nUserID);
        }

        public EmployeeTraining IUD(int nDBOperation, long nUserID)
        {
            return EmployeeTraining.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeTrainingService Service
        {
            get { return (IEmployeeTrainingService)Services.Factory.CreateService(typeof(IEmployeeTrainingService)); }
        }

        #endregion
    }
    #endregion

    #region Report Study
    public class EmployeeTrainingList : List<EmployeeTraining>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IEmployeeTraining interface

    public interface IEmployeeTrainingService
    {
        EmployeeTraining Get(int id, Int64 nUserID);//EmployeeTrainingID
        List<EmployeeTraining> Gets(int nEmployeeID, Int64 nUserID);
        EmployeeTraining IUD(EmployeeTraining oEmployeeTraining, int nDBOperation, Int64 nUserID);
    }
    #endregion
}