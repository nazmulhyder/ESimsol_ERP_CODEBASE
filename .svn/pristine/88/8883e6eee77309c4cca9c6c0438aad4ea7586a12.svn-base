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
    public class GratuitySchemeDetailService : MarshalByRefObject, IGratuitySchemeDetailService
    {
        #region Private functions and declaration
        private GratuitySchemeDetail MapObject(NullHandler oReader)
        {
            GratuitySchemeDetail oGratuitySchemeDetail = new GratuitySchemeDetail();

            oGratuitySchemeDetail.GSID = oReader.GetInt32("GSID");
            oGratuitySchemeDetail.GSDID = oReader.GetInt32("GSDID");
            oGratuitySchemeDetail.MaturityYearStart = oReader.GetInt32("MaturityYearStart");
            oGratuitySchemeDetail.MaturityYearEnd = oReader.GetInt32("MaturityYearEnd");
            oGratuitySchemeDetail.ActivationAfter = (EnumRecruitmentEvent)oReader.GetInt16("ActivationAfter");
            oGratuitySchemeDetail.ValueInPercent = oReader.GetInt32("ValueInPercent");
            oGratuitySchemeDetail.GratuityApplyOn = (EnumPayrollApplyOn)oReader.GetInt16("GratuityApplyOn");
            oGratuitySchemeDetail.NoOfMonthCountOneYear = oReader.GetInt32("NoOfMonthCountOneYear");
            oGratuitySchemeDetail.ActiveDate = oReader.GetDateTime("ActiveDate");
            oGratuitySchemeDetail.InactiveDate = oReader.GetDateTime("InactiveDate");

            return oGratuitySchemeDetail;

        }

        private GratuitySchemeDetail CreateObject(NullHandler oReader)
        {
            GratuitySchemeDetail oGratuitySchemeDetail = MapObject(oReader);
            return oGratuitySchemeDetail;
        }

        private List<GratuitySchemeDetail> CreateObjects(IDataReader oReader)
        {
            List<GratuitySchemeDetail> oGratuitySchemeDetails = new List<GratuitySchemeDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GratuitySchemeDetail oItem = CreateObject(oHandler);
                oGratuitySchemeDetails.Add(oItem);
            }
            return oGratuitySchemeDetails;
        }

        #endregion

        #region Interface implementation
        public GratuitySchemeDetailService() { }

        public GratuitySchemeDetail IUD(GratuitySchemeDetail oGratuitySchemeDetail, int nDBOperation, Int64 nUserID)
        {
            GratuitySchemeDetail oGSD = new GratuitySchemeDetail();
            TransactionContext tc = null;
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = GratuitySchemeDetailDA.IUD(tc, oGratuitySchemeDetail, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oGSD = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if(nDBOperation==3)
                {
                    oGSD.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oGSD = new GratuitySchemeDetail();
                oGSD.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                //oGratuitySchemeDetail.GSDID = 0;
                #endregion
            }
            return oGSD;
        }


        public GratuitySchemeDetail Get(int nGSID, Int64 nUserId)
        {
            GratuitySchemeDetail oGratuitySchemeDetail = new GratuitySchemeDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GratuitySchemeDetailDA.Get(nGSID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGratuitySchemeDetail = CreateObject(oReader);
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

                oGratuitySchemeDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oGratuitySchemeDetail;
        }

        public GratuitySchemeDetail Get(string sSQL, Int64 nUserId)
        {
            GratuitySchemeDetail oGratuitySchemeDetail = new GratuitySchemeDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GratuitySchemeDetailDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGratuitySchemeDetail = CreateObject(oReader);
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

                oGratuitySchemeDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oGratuitySchemeDetail;
        }

        public List<GratuitySchemeDetail> Gets(Int64 nUserID)
        {
            List<GratuitySchemeDetail> oGratuitySchemeDetail = new List<GratuitySchemeDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GratuitySchemeDetailDA.Gets(tc);
                oGratuitySchemeDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GratuitySchemeDetail", e);
                #endregion
            }
            return oGratuitySchemeDetail;
        }

        public List<GratuitySchemeDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<GratuitySchemeDetail> oGratuitySchemeDetail = new List<GratuitySchemeDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GratuitySchemeDetailDA.Gets(sSQL, tc);
                oGratuitySchemeDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GratuitySchemeDetail", e);
                #endregion
            }
            return oGratuitySchemeDetail;
        }

        #endregion


    }
}
