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
    #region SalaryHead

    public class SalaryHead : BusinessObject
    {
        public SalaryHead()
        {
            SalaryHeadID = 0;
            Name = "";
            NameInBangla = "";
            Description = "";
            SalaryHeadType = EnumSalaryHeadType.None;
            IsActive = true;
            IsProcessDependent = false;
            ErrorMessage = "";
            Sequence = 0;
        }

        #region Properties

        public int SalaryHeadID { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public bool IsUp { get; set; }
        public string NameInBangla { get; set; }
        public string Description { get; set; }
        public EnumSalaryHeadType SalaryHeadType { get; set; }
        public bool IsActive { get; set; }
        public bool IsProcessDependent { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string Activity { get { if (IsActive)return "Active"; else return "Inactive"; } }

        public string DeriveSalaryHeadName { get { if (this.SalaryHeadType == EnumSalaryHeadType.Addition)return this.Name + "(+)"; else if (this.SalaryHeadType == EnumSalaryHeadType.Deduction)return this.Name + "(-)"; else return this.Name; } }
        public int SalaryHeadTypeInt { get; set; }

        public string SalaryHeadTypeInString
        {
            get
            {
                return SalaryHeadType.ToString();
            }
        }
        public List<SalaryHead> oSalaryHeads = new List<SalaryHead>();

        #endregion

        #region Functions
        public static List<SalaryHead> Gets(long nUserID)
        {
            return SalaryHead.Service.Gets(nUserID);
        }

        public static List<SalaryHead> Gets(string sSQL, long nUserID)
        {
            return SalaryHead.Service.Gets(sSQL, nUserID);
        }

        public SalaryHead Get(int id, long nUserID)
        {
            return SalaryHead.Service.Get(id, nUserID);
        }

        public SalaryHead Save(long nUserID)
        {
            return SalaryHead.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return SalaryHead.Service.Delete(id, nUserID);
        }

        public string ChangeActiveStatus(SalaryHead oSalaryHead, long nUserID)
        {
            return SalaryHead.Service.ChangeActiveStatus(oSalaryHead, nUserID);
        }
        public SalaryHead UpDown(SalaryHead oSalaryHead, long nUserID)
        {
            return SalaryHead.Service.UpDown(oSalaryHead, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ISalaryHeadService Service
        {
            get { return (ISalaryHeadService)Services.Factory.CreateService(typeof(ISalaryHeadService)); }
        }

        #endregion
    }
    #endregion

    #region ISalaryHead interface

    public interface ISalaryHeadService
    {
        SalaryHead Get(int id, Int64 nUserID);
        List<SalaryHead> Gets(Int64 nUserID);
        List<SalaryHead> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        SalaryHead Save(SalaryHead oSalaryHead, Int64 nUserID);
        string ChangeActiveStatus(SalaryHead oSalaryHead, Int64 nUserID);
        SalaryHead UpDown(SalaryHead oSalaryHead, Int64 nUserID);
    }
    #endregion
}
