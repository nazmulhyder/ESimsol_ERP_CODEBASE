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
    public class LetterSetupEmployeeService : MarshalByRefObject, ILetterSetupEmployeeService
    {
        #region Private functions and declaration
        private LetterSetupEmployee MapObject(NullHandler oReader)
        {
            LetterSetupEmployee oLetterSetupEmployee = new LetterSetupEmployee();
            oLetterSetupEmployee.LSEID = oReader.GetInt32("LSEID");
            oLetterSetupEmployee.LSID = oReader.GetInt32("LSID");
            oLetterSetupEmployee.EmployeeID = oReader.GetInt32("EmployeeID");
            oLetterSetupEmployee.ApproveBy = oReader.GetInt32("ApproveBy");
            oLetterSetupEmployee.Code = oReader.GetString("Code");
            oLetterSetupEmployee.ApproveByName = oReader.GetString("ApproveByName");
            oLetterSetupEmployee.LetterName = oReader.GetString("LetterName");
            oLetterSetupEmployee.EmployeeName = oReader.GetString("EmployeeName");
            oLetterSetupEmployee.EmployeeCode = oReader.GetString("EmployeeCode");
            oLetterSetupEmployee.ApproveDate = oReader.GetDateTime("ApproveDate");
            oLetterSetupEmployee.Body = oReader.GetString("Body");
            oLetterSetupEmployee.Remark = oReader.GetString("Remark");
            oLetterSetupEmployee.BUName = oReader.GetString("BUName");
            oLetterSetupEmployee.BUAddress = oReader.GetString("BUAddress");
            oLetterSetupEmployee.IsEnglish = oReader.GetBoolean("IsEnglish");
            return oLetterSetupEmployee;

        }

        private LetterSetupEmployee CreateObject(NullHandler oReader)
        {
            LetterSetupEmployee oLetterSetupEmployee = MapObject(oReader);
            return oLetterSetupEmployee;
        }

        private List<LetterSetupEmployee> CreateObjects(IDataReader oReader)
        {
            List<LetterSetupEmployee> oLetterSetupEmployee = new List<LetterSetupEmployee>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LetterSetupEmployee oItem = CreateObject(oHandler);
                oLetterSetupEmployee.Add(oItem);
            }
            return oLetterSetupEmployee;
        }


        #endregion

        #region Interface implementation
        public LetterSetupEmployeeService() { }
        public LetterSetupEmployee IUD(LetterSetupEmployee oLetterSetupEmployee, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval)
                {
                    reader = LetterSetupEmployeeDA.IUD(tc, oLetterSetupEmployee, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oLetterSetupEmployee = new LetterSetupEmployee();
                        oLetterSetupEmployee = CreateObject(oReader);
                    }
                    reader.Close();
                }
                //else if (nDBOperation == (int)EnumDBOperation.Delete)
                //{
                //    reader = LetterSetupEmployeeDA.IUD(tc, oLetterSetupEmployee, nUserID, nDBOperation);
                //    NullHandler oReader = new NullHandler(reader);
                //    reader.Close();
                //    oLetterSetupEmployee.ErrorMessage = Global.DeleteMessage;
                //    reader.Close();
                //}

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oLetterSetupEmployee = new LetterSetupEmployee();
                oLetterSetupEmployee.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oLetterSetupEmployee;
        }


        public LetterSetupEmployee Get(string sSQL, Int64 nUserId)
        {
            LetterSetupEmployee oLetterSetupEmployee = new LetterSetupEmployee();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LetterSetupEmployeeDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLetterSetupEmployee = CreateObject(oReader);
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
                throw new ServiceException(e.Message);;
                #endregion
            }

            return oLetterSetupEmployee;
        }

        public String Delete(LetterSetupEmployee oLetterSetupEmployee, Int64 nUserID)
        {
            TransactionContext tc1 = null;
            try
            {
                tc1 = TransactionContext.Begin(true);
                //LetterSetupEmployeeDA.Delete(tc1, oLetterSetupEmployee, nUserID, (int)EnumDBOperation.Delete);

                IDataReader reader = LetterSetupEmployeeDA.Delete(tc1, oLetterSetupEmployee, nUserID, (int)EnumDBOperation.Delete);
                NullHandler oReader = new NullHandler(reader);
                reader.Close();
                oLetterSetupEmployee.ErrorMessage = Global.DeleteMessage;
                tc1.End();


            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc1 != null)
                    tc1.HandleError();

                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public String GetEmpLetter(LetterSetupEmployee oLetterSetupEmployee, Int64 nUserID)
        {
            TransactionContext tc1 = null;
            try
            {
                tc1 = TransactionContext.Begin(true);

                IDataReader reader = LetterSetupEmployeeDA.GetEmpLetter(tc1, oLetterSetupEmployee, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLetterSetupEmployee = CreateObject(oReader);
                }
                //oLetterSetupEmployee.Body = oReader.GetString("Body"); 
                reader.Close();
                tc1.End();


            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc1 != null)
                    tc1.HandleError();

                return e.Message.Split('~')[0];
                #endregion
            }
            return oLetterSetupEmployee.Body;
        }

        public LetterSetupEmployee Get(int id, Int64 nUserId)
        {
            LetterSetupEmployee oLetterSetupEmployee = new LetterSetupEmployee();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = LetterSetupEmployeeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLetterSetupEmployee = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LetterSetupEmployee", e);
                #endregion
            }
            return oLetterSetupEmployee;
        }
        public List<LetterSetupEmployee> Gets(string sSQL, Int64 nUserID)
        {
            List<LetterSetupEmployee> oLetterSetupEmployee = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LetterSetupEmployeeDA.Gets(sSQL, tc);
                oLetterSetupEmployee = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oLetterSetupEmployee;
        }

        public LetterSetupEmployee Approve(LetterSetupEmployee oLetterSetupEmployee, Int64 nUserID)
        {
            LetterSetupEmployee _oLetterSetupEmployee = new LetterSetupEmployee();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = LetterSetupEmployeeDA.IUD(tc, oLetterSetupEmployee, nUserID, (int)EnumDBOperation.Approval);
                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLetterSetupEmployee = new LetterSetupEmployee();
                    oLetterSetupEmployee = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oLetterSetupEmployee = new LetterSetupEmployee();
                oLetterSetupEmployee.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oLetterSetupEmployee;
        }
        #endregion
    }
}



