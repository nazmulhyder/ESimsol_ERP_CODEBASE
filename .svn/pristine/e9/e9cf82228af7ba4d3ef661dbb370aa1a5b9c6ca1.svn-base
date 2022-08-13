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
    public class ProductionBonusService : MarshalByRefObject, IProductionBonusService
    {
        #region Private functions and declaration
        private ProductionBonus MapObject(NullHandler oReader)
        {
            ProductionBonus oProductionBonus = new ProductionBonus();

            oProductionBonus.ProductionBonusID = oReader.GetInt32("ProductionBonusID");
            oProductionBonus.SalarySchemeID = oReader.GetInt32("SalarySchemeID");
            oProductionBonus.MinAmount = oReader.GetDouble("MinAmount");
            oProductionBonus.MaxAmount = oReader.GetDouble("MaxAmount");
            oProductionBonus.Value = oReader.GetDouble("Value");
            oProductionBonus.IsPercent = oReader.GetBoolean("IsPercent");
            oProductionBonus.IsActive = oReader.GetBoolean("IsActive");

            return oProductionBonus;

        }

        private ProductionBonus CreateObject(NullHandler oReader)
        {
            ProductionBonus oProductionBonus = MapObject(oReader);
            return oProductionBonus;
        }

        private List<ProductionBonus> CreateObjects(IDataReader oReader)
        {
            List<ProductionBonus> oProductionBonuss = new List<ProductionBonus>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionBonus oItem = CreateObject(oHandler);
                oProductionBonuss.Add(oItem);
            }
            return oProductionBonuss;
        }

        #endregion

        #region Interface implementation
        public ProductionBonusService() { }

        public ProductionBonus IUD(ProductionBonus oProductionBonus, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;

            try
            {
                if (nDBOperation == 2 || nDBOperation == 3)
                {
                    string sSql = "SELECT TOP(1)EmployeeSalaryID FROM EmployeeSalary WHERE EmployeeID IN(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID=" + oProductionBonus.SalarySchemeID+")";
                    tc = TransactionContext.Begin(true);
                    bool IsAssigned = SalarySchemeDA.IsAssigned(sSql, tc);
                    tc.End();

                    if (IsAssigned == true)
                    {
                        throw new Exception("This Salary Scheme is already assigned to employee. So Inactive is not possible !");
                    }
                }

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ProductionBonusDA.IUD(tc, oProductionBonus, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oProductionBonus = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oProductionBonus.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oProductionBonus.ProductionBonusID = 0;
                #endregion
            }
            return oProductionBonus;
        }

        public List<ProductionBonus> Gets(string sSQL, Int64 nUserID)
        {
            List<ProductionBonus> oProductionBonus = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionBonusDA.Gets(sSQL, tc);
                oProductionBonus = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionBonus", e);
                #endregion
            }
            return oProductionBonus;
        }

        #region Activity
        public ProductionBonus ActivityStatus(int nProductionBonusID, bool Active, Int64 nUserId)
        {
            ProductionBonus oProductionBonus = new ProductionBonus();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductionBonusDA.ActivityStatus(nProductionBonusID, Active, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionBonus = CreateObject(oReader);
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
                oProductionBonus.ErrorMessage = e.Message;
                #endregion
            }

            return oProductionBonus;
        }


        #endregion

        #endregion

    }
}
