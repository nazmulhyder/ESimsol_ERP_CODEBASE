using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{

    public class HRResponsibilityService : MarshalByRefObject, IHRResponsibilityService
    {
        #region Private functions and declaration
        private HRResponsibility MapObject(NullHandler oReader)
        {
            HRResponsibility oHRResponsibility = new HRResponsibility();
            oHRResponsibility.HRRID = oReader.GetInt32("HRRID");
            oHRResponsibility.Code = oReader.GetString("Code");
            oHRResponsibility.Description = oReader.GetString("Description");
            oHRResponsibility.DescriptionInBangla = oReader.GetString("DescriptionInBangla");

            return oHRResponsibility;
        }

        private HRResponsibility CreateObject(NullHandler oReader)
        {
            HRResponsibility oHRResponsibility = new HRResponsibility();
            oHRResponsibility = MapObject(oReader);
            return oHRResponsibility;
        }

        private List<HRResponsibility> CreateObjects(IDataReader oReader)
        {
            List<HRResponsibility> oHRResponsibilitys = new List<HRResponsibility>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                HRResponsibility oItem = CreateObject(oHandler);
                oHRResponsibilitys.Add(oItem);
            }
            return oHRResponsibilitys;
        }

        #endregion

        #region Interface implementation
        public HRResponsibilityService() { }

        public HRResponsibility Save(HRResponsibility oHRResponsibility, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region Raw Material Requisition Part
                IDataReader reader;
                if (oHRResponsibility.HRRID <= 0)
                {
                    reader = HRResponsibilityDA.InsertUpdate(tc, oHRResponsibility, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = HRResponsibilityDA.InsertUpdate(tc, oHRResponsibility, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHRResponsibility = new HRResponsibility();
                    oHRResponsibility = CreateObject(oReader);
                }
                reader.Close();
                #endregion


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oHRResponsibility = new HRResponsibility();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oHRResponsibility.ErrorMessage = Message;
                #endregion
            }
            return oHRResponsibility;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                HRResponsibility oHRResponsibility = new HRResponsibility();
                oHRResponsibility.HRRID = id;
                HRResponsibilityDA.Delete(tc, oHRResponsibility, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                #endregion
                return e.Message;
            }
            return Global.DeleteMessage;
        }

        public HRResponsibility Get(int id, Int64 nUserId)
        {
            HRResponsibility oHRResponsibility = new HRResponsibility();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = HRResponsibilityDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHRResponsibility = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oHRResponsibility = new HRResponsibility();
                oHRResponsibility.ErrorMessage = e.Message;
                #endregion
            }

            return oHRResponsibility;
        }

        public List<HRResponsibility> Gets(Int64 nUserID)
        {
            List<HRResponsibility> oHRResponsibilitys = new List<HRResponsibility>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = HRResponsibilityDA.Gets(tc);
                oHRResponsibilitys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                HRResponsibility oHRResponsibility = new HRResponsibility();
                oHRResponsibility.ErrorMessage = e.Message;
                oHRResponsibilitys.Add(oHRResponsibility);
                #endregion
            }

            return oHRResponsibilitys;
        }

        public List<HRResponsibility> Gets(string sSQL, Int64 nUserId)
        {
            List<HRResponsibility> oHRResponsibility = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = HRResponsibilityDA.Gets(tc, sSQL);
                oHRResponsibility = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get HRResponsibility", e);
                #endregion
            }

            return oHRResponsibility;
        }

        #endregion
    }
   
}
