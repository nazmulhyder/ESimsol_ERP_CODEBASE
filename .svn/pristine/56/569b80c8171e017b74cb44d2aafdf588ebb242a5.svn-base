using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class ExportBillRealizedService : MarshalByRefObject, IExportBillRealizedService
    {
        #region Private functions and declaration
        private ExportBillRealized MapObject(NullHandler oReader)
        {
            ExportBillRealized oExportBillRealized = new ExportBillRealized();
            oExportBillRealized.ExportBillRealizedID = oReader.GetInt32("ExportBillRealizedID");
            oExportBillRealized.ExportBillID = oReader.GetInt32("ExportBillID");
            oExportBillRealized.ExportBillParticularID = oReader.GetInt32("ExportBillParticularID");
            oExportBillRealized.ParticularName = oReader.GetString("ParticularName");
            oExportBillRealized.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportBillRealized.InOutType = (EnumInOutType)oReader.GetInt32("InOutType");
            oExportBillRealized.InOutTypeInt = oReader.GetInt32("InOutType");
           // oExportBillRealized.CCRate = oReader.GetDouble("CCRate");
            oExportBillRealized.Amount = oReader.GetDouble("Amount");
            //oExportBillRealized.CurrencyName = oReader.GetString("CurrencyName");
            
            return oExportBillRealized;
        }

        private ExportBillRealized CreateObject(NullHandler oReader)
        {
            ExportBillRealized oExportBillRealized = new ExportBillRealized();
            oExportBillRealized = MapObject(oReader);
            return oExportBillRealized;
        }

        private List<ExportBillRealized> CreateObjects(IDataReader oReader)
        {
            List<ExportBillRealized> oExportBillRealizeds = new List<ExportBillRealized>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportBillRealized oItem = CreateObject(oHandler);
                oExportBillRealizeds.Add(oItem);
            }
            return oExportBillRealizeds;
        }

        #endregion

        #region Interface implementation
        public ExportBillRealizedService() { }

        public ExportBillRealized Save(ExportBillRealized oExportBillRealized, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
            

                #region ExportBillRealized part
                IDataReader reader;
                if (oExportBillRealized.ExportBillRealizedID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "ExportBillRealized", EnumRoleOperationType.Add);
                    reader = ExportBillRealizedDA.InsertUpdate(tc, oExportBillRealized, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "ExportBillRealized", EnumRoleOperationType.Edit);
                    reader = ExportBillRealizedDA.InsertUpdate(tc, oExportBillRealized, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBillRealized = new ExportBillRealized();
                    oExportBillRealized = CreateObject(oReader);
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

                string Message = "";
                Message = e.Message;
                Message = Message.Split('~')[0];
                oExportBillRealized.ErrorMessage = Message;
                #endregion
            }
            return oExportBillRealized;
        }
        public string Delete(ExportBillRealized oExportBillRealized, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
              
                ExportBillRealizedDA.Delete(tc, oExportBillRealized, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ExportBillRealized. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public ExportBillRealized Get(int id, Int64 nUserId)
        {
            ExportBillRealized oAccountHead = new ExportBillRealized();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportBillRealizedDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportBillRealized", e);
                #endregion
            }

            return oAccountHead;
        }
            
        public List<ExportBillRealized> Gets(int nExportBillID, Int64 nUserID)
        {
            List<ExportBillRealized> oExportBillRealized = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportBillRealizedDA.Gets(tc, nExportBillID);
                oExportBillRealized = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBillRealized", e);
                #endregion
            }

            return oExportBillRealized;
        }

        public List<ExportBillRealized> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportBillRealized> oExportBillRealized = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportBillRealizedDA.Gets(tc, sSQL);
                oExportBillRealized = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBillRealized", e);
                #endregion
            }

            return oExportBillRealized;
        }

        #endregion
    }
}
