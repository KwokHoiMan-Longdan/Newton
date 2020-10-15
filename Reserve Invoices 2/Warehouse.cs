using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve_Invoices_2
{
    public class Warehouse
    {
        public string InitialStockAccount { get; }
        public string InitialCostOfGoodsSoldAccount { get; }
        public string InitialAllocationAccount { get; }
        private Warehouses oWarehouse;

        public bool StockAccountChanged = false;
        public bool CostOfGoodsSoldAccountChanged = false;
        public bool AllocationAccountChanged = false;

        //
        // Methods
        //

        public Warehouse(SAP sap, string whscode)
        {
            oWarehouse = (Warehouses)sap.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oWarehouses);
            oWarehouse.GetByKey(whscode);

            InitialStockAccount = oWarehouse.StockAccount;
            InitialCostOfGoodsSoldAccount = oWarehouse.CostOfGoodsSold;
            InitialAllocationAccount = oWarehouse.TransfersAcc;
        }

        public int SetStockAccount(string account)
        {
            if (InitialStockAccount != account)
            {
                oWarehouse.StockAccount = account;
                int result = oWarehouse.Update();
                StockAccountChanged = true;
                return result;
            }
            else
                return 0;
        }

        public int SetCostOfGoodsSoldAccount(string account)
        {
            if (InitialCostOfGoodsSoldAccount != account)
            {
                oWarehouse.CostOfGoodsSold = account;
                int result = oWarehouse.Update();
                CostOfGoodsSoldAccountChanged = true;
                return result;
            }
            else
                return 0;
        }

        public int SetAllocationAccount(string account)
        {
            if (InitialAllocationAccount != account)
            {
                oWarehouse.TransfersAcc = account;
                int result = oWarehouse.Update();
                CostOfGoodsSoldAccountChanged = true;
                return result;
            }
            else
                return 0;
        }

        public int RevertInitialAccounts()
        {
            if (CostOfGoodsSoldAccountChanged)
                oWarehouse.CostOfGoodsSold = InitialCostOfGoodsSoldAccount;

            if (StockAccountChanged)
                oWarehouse.StockAccount = InitialCostOfGoodsSoldAccount;

            if (AllocationAccountChanged)
                oWarehouse.TransfersAcc = InitialAllocationAccount;

            if (CostOfGoodsSoldAccountChanged || StockAccountChanged || AllocationAccountChanged)
                return oWarehouse.Update();
            else
                return 0;
        }
    }
}
