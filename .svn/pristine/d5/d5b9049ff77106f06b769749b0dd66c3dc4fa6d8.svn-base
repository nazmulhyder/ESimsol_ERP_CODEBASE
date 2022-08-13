using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Linq;

namespace ESimSol.Services.Services
{
    public class BenefitOnAttendanceEmployeeStoppedService : MarshalByRefObject, IBenefitOnAttendanceEmployeeStoppedService
    {
        #region Private functions and declaration
        private BenefitOnAttendanceEmployeeStopped MapObject(NullHandler oReader)
        {
            BenefitOnAttendanceEmployeeStopped oBenefitOnAttendanceEmployeeStopped = new BenefitOnAttendanceEmployeeStopped();

            oBenefitOnAttendanceEmployeeStopped.BOAEmployeeID = oReader.GetInt32("BOAEmployeeID");
            oBenefitOnAttendanceEmployeeStopped.BOAESID = oReader.GetInt32("BOAESID");
            oBenefitOnAttendanceEmployeeStopped.StartDate = oReader.GetDateTime("StartDate");
            oBenefitOnAttendanceEmployeeStopped.EndDate = oReader.GetDateTime("EndDate");
            oBenefitOnAttendanceEmployeeStopped.InactiveDate = oReader.GetDateTime("InactiveDate");
            
        
            return oBenefitOnAttendanceEmployeeStopped;
        }

        private BenefitOnAttendanceEmployeeStopped CreateObject(NullHandler oReader)
        {
            BenefitOnAttendanceEmployeeStopped oBenefitOnAttendanceEmployeeStopped = MapObject(oReader);
            return oBenefitOnAttendanceEmployeeStopped;
        }

        private List<BenefitOnAttendanceEmployeeStopped> CreateObjects(IDataReader oReader)
        {
            List<BenefitOnAttendanceEmployeeStopped> oBenefitOnAttendanceEmployeeStoppeds = new List<BenefitOnAttendanceEmployeeStopped>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BenefitOnAttendanceEmployeeStopped oItem = CreateObject(oHandler);
                oBenefitOnAttendanceEmployeeStoppeds.Add(oItem);
            }
            return oBenefitOnAttendanceEmployeeStoppeds;
        }

        #endregion

        #region Interface implementation
        public BenefitOnAttendanceEmployeeStoppedService() { }

    
        public BenefitOnAttendanceEmployeeStopped IUD(BenefitOnAttendanceEmployeeStopped oBOEStopped, short nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                if (nDBOperation == (short)EnumDBOperation.Insert || nDBOperation == (short)EnumDBOperation.Update)
                {
                    reader = BenefitOnAttendanceEmployeeStoppedDA.InsertUpdate(tc, oBOEStopped, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oBOEStopped = new BenefitOnAttendanceEmployeeStopped();
                        oBOEStopped = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else
                {
                    BenefitOnAttendanceEmployeeStoppedDA.Delete(tc, oBOEStopped, nUserID, nDBOperation);
                    oBOEStopped = new BenefitOnAttendanceEmployeeStopped();
                    oBOEStopped.ErrorMessage = Global.DeleteMessage;
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oBOEStopped.ErrorMessage = e.Message;

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save InvoiceProduct", e);
                #endregion
            }
            return oBOEStopped;
        }

        public List<BenefitOnAttendanceEmployeeStopped> MultiStopped(List<BenefitOnAttendanceEmployeeStopped> oBOAESs, Int64 nUserID)
        {
            List<BenefitOnAttendanceEmployeeStopped> oBOAEStoppeds = new List<BenefitOnAttendanceEmployeeStopped>();
            BenefitOnAttendanceEmployeeStopped oBOAES = new BenefitOnAttendanceEmployeeStopped();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                foreach (BenefitOnAttendanceEmployeeStopped oItem in oBOAESs)
                {
                    reader = BenefitOnAttendanceEmployeeStoppedDA.InsertUpdate(tc, oItem, nUserID, (int)EnumDBOperation.Insert);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oBOAES = new BenefitOnAttendanceEmployeeStopped();
                        oBOAES = CreateObject(oReader);
                        oBOAEStoppeds.Add(oBOAES);
                    }
                    reader.Close();
                }
              
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oBOAES.ErrorMessage = e.Message;

                oBOAEStoppeds = new List<BenefitOnAttendanceEmployeeStopped>();
                oBOAEStoppeds.Add(oBOAES);
                #endregion
            }
            return oBOAEStoppeds;
        }
      
        
        
        public BenefitOnAttendanceEmployeeStopped Get(int nBOAEmployeeID, Int64 nUserId)
        {
            BenefitOnAttendanceEmployeeStopped oBenefitOnAttendanceEmployeeStopped = new BenefitOnAttendanceEmployeeStopped();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BenefitOnAttendanceEmployeeStoppedDA.Get(nBOAEmployeeID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBenefitOnAttendanceEmployeeStopped = CreateObject(oReader);
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

                oBenefitOnAttendanceEmployeeStopped.ErrorMessage = e.Message;
                #endregion
            }

            return oBenefitOnAttendanceEmployeeStopped;
        }

        public BenefitOnAttendanceEmployeeStopped GetBy(int nEmployeeID, int nBOAID, Int64 nUserId)
        {
            BenefitOnAttendanceEmployeeStopped oBenefitOnAttendanceEmployeeStopped = new BenefitOnAttendanceEmployeeStopped();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BenefitOnAttendanceEmployeeStoppedDA.GetBy(nEmployeeID, nBOAID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBenefitOnAttendanceEmployeeStopped = CreateObject(oReader);
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

                oBenefitOnAttendanceEmployeeStopped.ErrorMessage = e.Message;
                #endregion
            }

            return oBenefitOnAttendanceEmployeeStopped;
        }

        public List<BenefitOnAttendanceEmployeeStopped> Gets(Int64 nUserID)
        {
            List<BenefitOnAttendanceEmployeeStopped> oBenefitOnAttendanceEmployeeStopped = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BenefitOnAttendanceEmployeeStoppedDA.Gets(tc);
                oBenefitOnAttendanceEmployeeStopped = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BenefitOnAttendanceEmployeeStopped", e);
                #endregion
            }
            return oBenefitOnAttendanceEmployeeStopped;
        }

        public List<BenefitOnAttendanceEmployeeStopped> Gets(string sSQL, Int64 nUserID)
        {
            List<BenefitOnAttendanceEmployeeStopped> oBenefitOnAttendanceEmployeeStopped = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BenefitOnAttendanceEmployeeStoppedDA.Gets(sSQL, tc);
                oBenefitOnAttendanceEmployeeStopped = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BenefitOnAttendanceEmployeeStopped", e);
                #endregion
            }
            return oBenefitOnAttendanceEmployeeStopped;
        }

        #endregion

    }
}
