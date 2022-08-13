using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class BenefitOnAttendanceEmployeeAssignService : MarshalByRefObject, IBenefitOnAttendanceEmployeeAssignService
    {
        #region Private functions and declaration
        private static BenefitOnAttendanceEmployeeAssign MapObject(NullHandler oReader)
        {
            BenefitOnAttendanceEmployeeAssign oBOAEA = new BenefitOnAttendanceEmployeeAssign();
            oBOAEA.BOAEAID = oReader.GetInt32("BOAEAID");
            oBOAEA.BOAEmployeeID = oReader.GetInt32("BOAEmployeeID");
            oBOAEA.StartDate = oReader.GetDateTime("StartDate");
            oBOAEA.EndDate = oReader.GetDateTime("EndDate");
            return oBOAEA;
        }

        public static BenefitOnAttendanceEmployeeAssign CreateObject(NullHandler oReader)
        {
            BenefitOnAttendanceEmployeeAssign oBOAEA = new BenefitOnAttendanceEmployeeAssign();
            oBOAEA = MapObject(oReader);
            return oBOAEA;
        }

        private List<BenefitOnAttendanceEmployeeAssign> CreateObjects(IDataReader oReader)
        {
            List<BenefitOnAttendanceEmployeeAssign> oBOAEA = new List<BenefitOnAttendanceEmployeeAssign>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BenefitOnAttendanceEmployeeAssign oItem = CreateObject(oHandler);
                oBOAEA.Add(oItem);
            }
            return oBOAEA;
        }

        #endregion

        #region Interface implementation
        public BenefitOnAttendanceEmployeeAssignService() { }

        public BenefitOnAttendanceEmployeeAssign IUD(BenefitOnAttendanceEmployeeAssign oBOAEA, short nDBOperation, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                {
                    IDataReader reader;
                    reader = BenefitOnAttendanceEmployeeAssignDA.InsertUpdate(tc, oBOAEA, nUserId, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oBOAEA = new BenefitOnAttendanceEmployeeAssign();
                        oBOAEA = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else
                {
                    BenefitOnAttendanceEmployeeAssignDA.Delete(tc, oBOAEA, nUserId, nDBOperation);
                    oBOAEA = new BenefitOnAttendanceEmployeeAssign();
                    oBOAEA.ErrorMessage = Global.DeleteMessage;
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oBOAEA.ErrorMessage = e.Message;
                #endregion
            }
            return oBOAEA;
        }

        public BenefitOnAttendanceEmployeeAssign Get(int id, Int64 nUserId)
        {
            BenefitOnAttendanceEmployeeAssign oBOAEA = new BenefitOnAttendanceEmployeeAssign();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BenefitOnAttendanceEmployeeAssignDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBOAEA = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }

            return oBOAEA;
        }

        public List<BenefitOnAttendanceEmployeeAssign> Gets(string sSQL, Int64 nUserId)
        {
            List<BenefitOnAttendanceEmployeeAssign> oBOAEAs = new List<BenefitOnAttendanceEmployeeAssign>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BenefitOnAttendanceEmployeeAssignDA.Gets(tc, sSQL);
                oBOAEAs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }

            return oBOAEAs;
        }
        #endregion
    }
}
