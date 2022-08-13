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
    public class SalarySheetSignatureService : MarshalByRefObject, ISalarySheetSignatureService
    {
        #region Private functions and declaration
        private SalarySheetSignature MapObject(NullHandler oReader)
        {
            SalarySheetSignature oSalarySheetSignature = new SalarySheetSignature();
            oSalarySheetSignature.SignatureID = oReader.GetInt32("SignatureID");
            oSalarySheetSignature.SignatureName = oReader.GetString("SignatureName");
            return oSalarySheetSignature;
        }

        private SalarySheetSignature CreateObject(NullHandler oReader)
        {
            SalarySheetSignature oSalarySheetSignature = MapObject(oReader);
            return oSalarySheetSignature;
        }

        private List<SalarySheetSignature> CreateObjects(IDataReader oReader)
        {
            List<SalarySheetSignature> oSalarySheetSignature = new List<SalarySheetSignature>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalarySheetSignature oItem = CreateObject(oHandler);
                oSalarySheetSignature.Add(oItem);
            }
            return oSalarySheetSignature;
        }

        #endregion

        #region Interface implementation
        public SalarySheetSignatureService() { }

        public SalarySheetSignature IUD(SalarySheetSignature oSalarySheetSignature, short nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = SalarySheetSignatureDA.IUD(tc, oSalarySheetSignature, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oSalarySheetSignature = new SalarySheetSignature();
                        oSalarySheetSignature = CreateObject(oReader);
                    }
                    reader.Close();
                }

                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oSalarySheetSignature = new SalarySheetSignature();
                    oSalarySheetSignature.ErrorMessage = Global.DeleteMessage;
                }
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSalarySheetSignature = new SalarySheetSignature();
                oSalarySheetSignature.ErrorMessage = (e.Message.Contains("~"))?e.Message.Split('~')[0]:e.Message ;
                #endregion
            }
            return oSalarySheetSignature;
        }
        public SalarySheetSignature Get(int nId, Int64 nUserId)
        {
            SalarySheetSignature oSalarySheetSignature = new SalarySheetSignature();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalarySheetSignatureDA.Get(tc, nId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalarySheetSignature = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SalarySheetSignature", e);
                #endregion
            }

            return oSalarySheetSignature;
        }
        public List<SalarySheetSignature> Gets(string sSQL, Int64 nUserID)
        {
            List<SalarySheetSignature> oSalarySheetSignatures = new List<SalarySheetSignature>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalarySheetSignatureDA.Gets(tc, sSQL);
                oSalarySheetSignatures = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalarySheetSignature", e);
                #endregion
            }
            return oSalarySheetSignatures;
        }

        #endregion
    }
}
