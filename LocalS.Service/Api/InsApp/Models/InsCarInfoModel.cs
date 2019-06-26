using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.Api.InsApp
{
    public class InsCarInfoModel
    {
        public string PlateNo { get; set; }
        public string Vin { get; set; }
        public string EngineNo { get; set; }
        public string RegisterDate { get; set; }
        public string ModelCode { get; set; }
        public string ModelName { get; set; }
        public string Displacement { get; set; }
        public string MarketYear { get; set; }
        public int PassengerNumber { get; set; }
        public string PurchasePrice { get; set; }
        public string Tonnage { get; set; }
        public string WholeWeight { get; set; }
        public bool IsTransfer { get; set; }
        public string TransferDate { get; set; }
        public bool IsCompanyCar { get; set; }
    }
}
