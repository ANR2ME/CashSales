﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Validation.Validation
{
    public class CashSalesReturnDetailValidator : ICashSalesReturnDetailValidator
    {
        public CashSalesReturnDetail VIsNotConfirmed(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService)
        {
            CashSalesReturn cashSalesReturn = _cashSalesReturnService.GetObjectById(cashSalesReturnDetail.CashSalesReturnId);
            if (cashSalesReturn != null)
            {
                if (cashSalesReturn.IsConfirmed)
                {
                    cashSalesReturnDetail.Errors.Add("Generic", "CashSalesReturn tidak boleh terkonfirmasi");
                }
            }
            else
            {
                cashSalesReturnDetail.Errors.Add("Generic", "CashSalesReturn tidak ada");
            }
            return cashSalesReturnDetail;
        }

        public CashSalesReturnDetail VHasCashSalesReturn(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService)
        {
            CashSalesReturn cashSalesReturn = _cashSalesReturnService.GetObjectById(cashSalesReturnDetail.CashSalesReturnId);
            if (cashSalesReturn == null)
            {
                cashSalesReturnDetail.Errors.Add("CashSalesReturnId", "Tidak valid");
            }
            return cashSalesReturnDetail;
        }

        public CashSalesReturnDetail VIsValidCashSalesInvoiceDetail(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, ICashSalesReturnService _cashSalesReturnService)
        {            
            CashSalesInvoiceDetail cashSalesInvoiceDetail = _cashSalesInvoiceDetailService.GetObjectById(cashSalesReturnDetail.CashSalesInvoiceDetailId);
            CashSalesReturn cashSalesReturn = _cashSalesReturnService.GetObjectById(cashSalesReturnDetail.CashSalesReturnId);
            if (cashSalesInvoiceDetail == null)
            {
                cashSalesReturnDetail.Errors.Add("CashSalesInvoiceDetailId", "Tidak valid");
            }
            else if (cashSalesReturn == null)
            {
                cashSalesReturnDetail.Errors.Add("CashSalesReturnId", "Tidak valid");
            }
            else if (cashSalesInvoiceDetail.CashSalesInvoiceId != cashSalesReturn.CashSalesInvoiceId)
            {
                cashSalesReturnDetail.Errors.Add("CashSalesInvoiceId", "Tidak sama");
            }
            return cashSalesReturnDetail;
        }

        public CashSalesReturnDetail VIsValidQuantity(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, ICashSalesReturnDetailService _cashSalesReturnDetailService)
        {
            CashSalesInvoiceDetail cashSalesInvoiceDetail = _cashSalesInvoiceDetailService.GetObjectById(cashSalesReturnDetail.CashSalesInvoiceDetailId);

            if (cashSalesReturnDetail.Quantity <= 0 || _cashSalesReturnDetailService.GetTotalQuantityByCashSalesInvoiceDetailId(cashSalesReturnDetail.CashSalesInvoiceDetailId) > cashSalesInvoiceDetail.Quantity)
            {
                cashSalesReturnDetail.Errors.Add("Quantity", "Quantity harus lebih besar dari 0 dan lebih kecil atau sama dengan CashSalesInvoiceDetail Quantity");
                return cashSalesReturnDetail;
            }
            return cashSalesReturnDetail;
        }

        /*public CashSalesReturnDetail VIsValidTotalPrice(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService)
        {
            CashSalesInvoiceDetail cashSalesInvoiceDetail = _cashSalesInvoiceDetailService.GetObjectById(cashSalesReturnDetail.CashSalesInvoiceDetailId);

            if (cashSalesReturnDetail.TotalPrice < 0 || cashSalesReturnDetail.TotalPrice > (cashSalesInvoiceDetail.Amount / cashSalesInvoiceDetail.Quantity) * cashSalesReturnDetail.Quantity)
            {
                cashSalesReturnDetail.Errors.Add("Generic", "TotalPrice Harus lebih besar atau sama dengan 0 dan lebih kecil atau sama dengan CashSalesInvoiceDetail Amount ( " + cashSalesInvoiceDetail.Amount + " )");
                return cashSalesReturnDetail;
            }
            return cashSalesReturnDetail;
        }*/

        public CashSalesReturnDetail VConfirmObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, ICashSalesReturnDetailService _cashSalesReturnDetailService)
        {
            VIsValidQuantity(cashSalesReturnDetail, _cashSalesInvoiceDetailService, _cashSalesReturnDetailService);
            return cashSalesReturnDetail;
        }

        public CashSalesReturnDetail VUnconfirmObject(CashSalesReturnDetail cashSalesReturnDetail)
        {
            
            return cashSalesReturnDetail;
        }

        public CashSalesReturnDetail VCreateObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService,
                                                      ICashSalesReturnDetailService _cashSalesReturnDetailService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService)
        {
            VHasCashSalesReturn(cashSalesReturnDetail, _cashSalesReturnService);
            if (!isValid(cashSalesReturnDetail)) { return cashSalesReturnDetail; }
            VIsValidCashSalesInvoiceDetail(cashSalesReturnDetail, _cashSalesInvoiceDetailService, _cashSalesReturnService);
            if (!isValid(cashSalesReturnDetail)) { return cashSalesReturnDetail; }
            VIsValidQuantity(cashSalesReturnDetail, _cashSalesInvoiceDetailService, _cashSalesReturnDetailService);
            if (!isValid(cashSalesReturnDetail)) { return cashSalesReturnDetail; }
            VIsNotConfirmed(cashSalesReturnDetail, _cashSalesReturnService);
            if (!isValid(cashSalesReturnDetail)) { return cashSalesReturnDetail; }
            //VIsValidTotalPrice(cashSalesReturnDetail, _cashSalesInvoiceDetailService);
            return cashSalesReturnDetail;
        }

        public CashSalesReturnDetail VUpdateObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService,
                                                      ICashSalesReturnDetailService _cashSalesReturnDetailService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService)
        {
            //VIsNotConfirmed(cashSalesReturnDetail, _cashSalesReturnService);
            //if (!isValid(cashSalesReturnDetail)) { return cashSalesReturnDetail; }
            return VCreateObject(cashSalesReturnDetail, _cashSalesReturnService, _cashSalesReturnDetailService, _cashSalesInvoiceDetailService);
        }

        public CashSalesReturnDetail VDeleteObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService)
        {
            VIsNotConfirmed(cashSalesReturnDetail, _cashSalesReturnService);
            if (!isValid(cashSalesReturnDetail)) { return cashSalesReturnDetail; }
            return cashSalesReturnDetail;
        }

        public bool ValidCreateObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService,
                                      ICashSalesReturnDetailService _cashSalesReturnDetailService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService)
        {
            VCreateObject(cashSalesReturnDetail, _cashSalesReturnService, _cashSalesReturnDetailService, _cashSalesInvoiceDetailService);
            return isValid(cashSalesReturnDetail);
        }

        public bool ValidConfirmObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, ICashSalesReturnDetailService _cashSalesReturnDetailService)
        {
            cashSalesReturnDetail.Errors.Clear();
            VConfirmObject(cashSalesReturnDetail, _cashSalesInvoiceDetailService, _cashSalesReturnDetailService);
            return isValid(cashSalesReturnDetail);
        }

        public bool ValidUnconfirmObject(CashSalesReturnDetail cashSalesReturnDetail)
        {
            cashSalesReturnDetail.Errors.Clear();
            VUnconfirmObject(cashSalesReturnDetail);
            return isValid(cashSalesReturnDetail);
        }

        public bool ValidUpdateObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService,
                                      ICashSalesReturnDetailService _cashSalesReturnDetailService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService)
        {
            cashSalesReturnDetail.Errors.Clear();
            VUpdateObject(cashSalesReturnDetail, _cashSalesReturnService, _cashSalesReturnDetailService, _cashSalesInvoiceDetailService);
            return isValid(cashSalesReturnDetail);
        }

        public bool ValidDeleteObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService)
        {
            cashSalesReturnDetail.Errors.Clear();
            VDeleteObject(cashSalesReturnDetail, _cashSalesReturnService);
            return isValid(cashSalesReturnDetail);
        }

        public bool isValid(CashSalesReturnDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CashSalesReturnDetail obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
