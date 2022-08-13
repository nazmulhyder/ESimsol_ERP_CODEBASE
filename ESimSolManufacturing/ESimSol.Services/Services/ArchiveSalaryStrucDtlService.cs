using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
namespace ESimSol.Services.Services
{
    public class ArchiveSalaryStrucDtlService : MarshalByRefObject, IArchiveSalaryStrucDtlService
    {
        #region Private functions and declaration

        private ArchiveSalaryStrucDtl MapObject(NullHandler oReader)
        {
            ArchiveSalaryStrucDtl oArchiveSalaryStrucDtl = new ArchiveSalaryStrucDtl();
            oArchiveSalaryStrucDtl.ArchiveSalaryStrucDtlID = oReader.GetInt32("ArchiveSalaryStrucDtlID");
            oArchiveSalaryStrucDtl.ArchiveSalaryStrucID = oReader.GetInt32("ArchiveSalaryStrucID");
            oArchiveSalaryStrucDtl.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oArchiveSalaryStrucDtl.Amount = oReader.GetDouble("Amount");
            oArchiveSalaryStrucDtl.CompAmount = oReader.GetDouble("CompAmount");
            oArchiveSalaryStrucDtl.DBUSerID = oReader.GetInt32("DBUSerID");
            oArchiveSalaryStrucDtl.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oArchiveSalaryStrucDtl.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oArchiveSalaryStrucDtl.EntryUserName = oReader.GetString("EntryUserName");
            oArchiveSalaryStrucDtl.SalaryHeadType = (EnumSalaryHeadType)oReader.GetInt16("SalaryHeadType");
            oArchiveSalaryStrucDtl.SalaryHeadNameInBangla = oReader.GetString("SalaryHeadNameInBangla");
            oArchiveSalaryStrucDtl.IsProcessDependent = oReader.GetBoolean("IsProcessDependent");
            oArchiveSalaryStrucDtl.Condition = (EnumAllowanceCondition)oReader.GetInt16("Condition");
            oArchiveSalaryStrucDtl.Period = (EnumPeriod)oReader.GetInt16("Period");
            oArchiveSalaryStrucDtl.Times = oReader.GetInt32("Times");
            oArchiveSalaryStrucDtl.DeferredDay = oReader.GetInt32("DeferredDay");
            oArchiveSalaryStrucDtl.ActivationAfter = (EnumRecruitmentEvent)oReader.GetInt16("ActivationAfter");


            return oArchiveSalaryStrucDtl;
        }

        private ArchiveSalaryStrucDtl CreateObject(NullHandler oReader)
        {
            ArchiveSalaryStrucDtl oArchiveSalaryStrucDtl = new ArchiveSalaryStrucDtl();
            oArchiveSalaryStrucDtl = MapObject(oReader);
            return oArchiveSalaryStrucDtl;
        }

        private List<ArchiveSalaryStrucDtl> CreateObjects(IDataReader oReader)
        {
            List<ArchiveSalaryStrucDtl> oArchiveSalaryStrucDtl = new List<ArchiveSalaryStrucDtl>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ArchiveSalaryStrucDtl oItem = CreateObject(oHandler);
                oArchiveSalaryStrucDtl.Add(oItem);
            }
            return oArchiveSalaryStrucDtl;
        }

        public List<ArchiveSalaryStrucDtl> Gets(int id, Int64 nUserID)
        {
            List<ArchiveSalaryStrucDtl> oArchiveSalaryStrucDtls = new List<ArchiveSalaryStrucDtl>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ArchiveSalaryStrucDtlDA.Gets(tc, id);
                oArchiveSalaryStrucDtls = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ArchiveSalaryStrucDtl oArchiveSalaryStrucDtl = new ArchiveSalaryStrucDtl();
                oArchiveSalaryStrucDtl.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oArchiveSalaryStrucDtls;
        }

        #endregion
    }
}
