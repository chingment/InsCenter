﻿using LocalS.BLL;
using Lumos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.Api.InsApp
{
    public class InsCarService : BaseDbContext
    {
        public CustomJsonResult GetIndexPageData()
        {
            var result = new CustomJsonResult();

            var ret = new RetGetIndexPageData();

            var carPlateNoSearchHiss = CurrentDb.InsCarPlateNoSearchHis.OrderByDescending(m => m.CreateTime).ToList();

            foreach (var item in carPlateNoSearchHiss)
            {
                ret.SearchPlateNoRecords.Add(new InsCarSearchPlateNoRecordModel { PlateNo = item.CarPlateNo });
            }

            var carCompanyRules = CurrentDb.InsCarCompanyRule.OrderByDescending(m => m.Priority).ToList();

            foreach (var item in carCompanyRules)
            {
                ret.CompanyRules.Add(new InsCarCompanyRuleModel { CompanyId = item.CompanyId, CompanyName = item.CompanyName, CompanyImgUrl = item.CompanyImgUrl, CommissionRate = item.CommissionRate });
            }


            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "", ret);

            return result;
        }

        public CustomJsonResult SearchCarInfo(RupSearchCarInfo rup)
        {
            var result = new CustomJsonResult();

            var ret = new RetSearchCarInfo();


            var carPlateNoInfo = CurrentDb.InsCarPlateNoInfo.Where(m => rup.PlateNo != null && m.PlateNo == rup.PlateNo).FirstOrDefault();

            if (carPlateNoInfo == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到车牌号信息");
            }

            ret.CarInfo.PlateNo = carPlateNoInfo.PlateNo;
            ret.CarInfo.Vin = carPlateNoInfo.Vin;
            ret.CarInfo.EngineNo = carPlateNoInfo.EngineNo;
            ret.CarInfo.RegisterDate = carPlateNoInfo.RegisterDate.ToString("yyyy-MM-dd");
            ret.CarInfo.ModelCode = carPlateNoInfo.ModelCode;
            ret.CarInfo.ModelName = carPlateNoInfo.ModelName;
            ret.CarInfo.Displacement = carPlateNoInfo.Displacement;
            ret.CarInfo.MarketYear = carPlateNoInfo.MarketYear;
            ret.CarInfo.PassengerNumber = carPlateNoInfo.PassengerNumber;
            ret.CarInfo.PurchasePrice = carPlateNoInfo.PurchasePrice.ToString("F2");
            ret.CarInfo.Tonnage = carPlateNoInfo.Tonnage;
            ret.CarInfo.WholeWeight = carPlateNoInfo.WholeWeight;
            ret.CarInfo.IsTransfer = carPlateNoInfo.IsTransfer;
            ret.CarInfo.TransferDate = carPlateNoInfo.TransferDate.ToString("yyyy-MM-dd");
            ret.CarInfo.IsCompanyCar = carPlateNoInfo.IsCompanyCar;
            ret.CarInfo.IsTransfer = carPlateNoInfo.IsTransfer;


            ret.CarOwner.Name = carPlateNoInfo.OwnerName;
            ret.CarOwner.CertNo = carPlateNoInfo.OwnerCertNo;
            ret.CarOwner.Mobile = carPlateNoInfo.OwnerMobile;
            ret.CarOwner.Address = carPlateNoInfo.OwnerAddress;

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "", ret);

            return result;
        }
    }
}
