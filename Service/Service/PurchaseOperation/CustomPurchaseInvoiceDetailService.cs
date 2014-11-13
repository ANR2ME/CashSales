using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Service.Service
{
    public class CustomPurchaseInvoiceDetailService : ICustomPurchaseInvoiceDetailService
    {
        private ICustomPurchaseInvoiceDetailRepository _repository;
        private ICustomPurchaseInvoiceDetailValidator _validator;
        public CustomPurchaseInvoiceDetailService(ICustomPurchaseInvoiceDetailRepository _customPurchaseInvoiceDetailRepository, ICustomPurchaseInvoiceDetailValidator _customPurchaseInvoiceDetailValidator)
        {
            _repository = _customPurchaseInvoiceDetailRepository;
            _validator = _customPurchaseInvoiceDetailValidator;
        }

        public ICustomPurchaseInvoiceDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<CustomPurchaseInvoiceDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<CustomPurchaseInvoiceDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IQueryable<CustomPurchaseInvoiceDetail> GetQueryableObjectsByCustomPurchaseInvoiceId(int CustomPurchaseInvoiceId)
        {
            return _repository.GetQueryableObjectsByCustomPurchaseInvoiceId(CustomPurchaseInvoiceId);
        }

        public IList<CustomPurchaseInvoiceDetail> GetObjectsByCustomPurchaseInvoiceId(int CustomPurchaseInvoiceId)
        {
            return _repository.GetObjectsByCustomPurchaseInvoiceId(CustomPurchaseInvoiceId);
        }

        public CustomPurchaseInvoiceDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CustomPurchaseInvoiceDetail CreateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService, 
                                                     IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            customPurchaseInvoiceDetail.Errors = new Dictionary<String, String>();
            if(_validator.ValidCreateObject(customPurchaseInvoiceDetail, _customPurchaseInvoiceService, this, _itemService, _warehouseItemService))
            {
                Item item = _itemService.GetObjectById(customPurchaseInvoiceDetail.ItemId);
                CustomPurchaseInvoice customPurchaseInvoice = _customPurchaseInvoiceService.GetObjectById(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
                customPurchaseInvoiceDetail.Price = customPurchaseInvoiceDetail.ListedUnitPrice * (100 - customPurchaseInvoiceDetail.Discount) / 100;
                customPurchaseInvoiceDetail.Amount = customPurchaseInvoiceDetail.Price * customPurchaseInvoiceDetail.Quantity;
                customPurchaseInvoiceDetail.CoGS = customPurchaseInvoiceDetail.Quantity * _itemService.CalculateAvgPrice(item, customPurchaseInvoiceDetail.Quantity, customPurchaseInvoiceDetail.Price); 
                customPurchaseInvoiceDetail = _repository.CreateObject(customPurchaseInvoiceDetail);
                customPurchaseInvoice.Total = CalculateTotal(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
                customPurchaseInvoice.CoGS = CalculateCoGS(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
                _customPurchaseInvoiceService.GetRepository().Update(customPurchaseInvoice);
            }
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail UpdateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService,
                                                     IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUpdateObject(customPurchaseInvoiceDetail, _customPurchaseInvoiceService, this, _itemService, _warehouseItemService))
            {
                Item item = _itemService.GetObjectById(customPurchaseInvoiceDetail.ItemId);
                CustomPurchaseInvoice customPurchaseInvoice = _customPurchaseInvoiceService.GetObjectById(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
                customPurchaseInvoiceDetail.Price = customPurchaseInvoiceDetail.ListedUnitPrice * (100 - customPurchaseInvoiceDetail.Discount) / 100;
                customPurchaseInvoiceDetail.Amount = customPurchaseInvoiceDetail.Price * customPurchaseInvoiceDetail.Quantity;
                customPurchaseInvoiceDetail.CoGS = customPurchaseInvoiceDetail.Quantity * _itemService.CalculateAvgPrice(item, customPurchaseInvoiceDetail.Quantity, customPurchaseInvoiceDetail.Price); 
                customPurchaseInvoiceDetail = _repository.UpdateObject(customPurchaseInvoiceDetail);
                customPurchaseInvoice.Total = CalculateTotal(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
                customPurchaseInvoice.CoGS = CalculateCoGS(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
                _customPurchaseInvoiceService.GetRepository().Update(customPurchaseInvoice);
            }
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail ConfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService, 
                                                      IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService, IPriceMutationService _priceMutationService)
        {
            if(_validator.ValidConfirmObject(customPurchaseInvoiceDetail, _customPurchaseInvoiceService, _warehouseItemService))
            {
                CustomPurchaseInvoice customPurchaseInvoice = _customPurchaseInvoiceService.GetObjectById(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(customPurchaseInvoice.WarehouseId, customPurchaseInvoiceDetail.ItemId);
                Item item = _itemService.GetObjectById(customPurchaseInvoiceDetail.ItemId);
                StockMutation stockMutation = new StockMutation()
                {
                    ItemId = customPurchaseInvoiceDetail.ItemId,
                    ItemCase = Core.Constants.Constant.ItemCase.Ready,
                    Status = Core.Constants.Constant.MutationStatus.Addition,
                    Quantity = customPurchaseInvoiceDetail.Quantity,
                    SourceDocumentCode = customPurchaseInvoice.Code,
                    SourceDocumentId = customPurchaseInvoice.Id,
                    SourceDocumentType = Core.Constants.Constant.SourceDocumentType.CustomPurchaseInvoice,
                    SourceDocumentDetailCode = customPurchaseInvoiceDetail.Code,
                    SourceDocumentDetailId = customPurchaseInvoiceDetail.Id,
                    SourceDocumentDetailType = Core.Constants.Constant.SourceDocumentDetailType.CustomPurchaseInvoiceDetail,
                    WarehouseId = customPurchaseInvoice.WarehouseId,
                    WarehouseItemId = warehouseItem.Id
                };

                decimal hiPrice = GetHighestPrice(/*customPurchaseInvoice.Id,*/ customPurchaseInvoiceDetail.ItemId);
                decimal newPrice = hiPrice * (100 + item.Margin) / 100;
                if (newPrice > item.SellingPrice)
                {
                    item.SellingPrice = newPrice;
                    PriceMutation priceMutation = _priceMutationService.CreateObject(item.Id, item.SellingPrice, DateTime.Now);
                    item.PriceMutationId = priceMutation.Id;
                }
                decimal itemPrice = customPurchaseInvoiceDetail.Amount / customPurchaseInvoiceDetail.Quantity;
                item.AvgPrice = _itemService.CalculateAndUpdateAvgPrice(item, customPurchaseInvoiceDetail.Quantity, itemPrice);

                stockMutation = _stockMutationService.CreateObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                stockMutation.CreatedAt = (DateTime)customPurchaseInvoice.ConfirmationDate.GetValueOrDefault();
                _stockMutationService.UpdateObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                _stockMutationService.StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                customPurchaseInvoiceDetail.CoGS = customPurchaseInvoiceDetail.Quantity * item.AvgPrice;
                customPurchaseInvoiceDetail = _repository.ConfirmObject(customPurchaseInvoiceDetail);
            }
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail UnconfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, IWarehouseItemService _warehouseItemService, ICustomPurchaseInvoiceService _customPurchaseInvoiceService,
                                                      IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService, IPriceMutationService _priceMutationService)
        {
            if (_validator.ValidUnconfirmObject(customPurchaseInvoiceDetail, _customPurchaseInvoiceService, _warehouseItemService))
            {
                Item item = _itemService.GetObjectById(customPurchaseInvoiceDetail.ItemId);
                //CustomPurchaseInvoiceDetail hiCustomPurchaseInvoiceDetail = GetAll().Where(x => x.ItemId == item.Id).OrderByDescending(x => x.Price).FirstOrDefault();
                decimal hiPrice = GetHighestPrice(/*customPurchaseInvoiceDetail.CustomPurchaseInvoiceId,*/ customPurchaseInvoiceDetail.ItemId);
                decimal newPrice = hiPrice * (100 + item.Margin) / 100;
                if (newPrice > item.SellingPrice)
                {
                    item.SellingPrice = newPrice;
                    PriceMutation priceMutation = _priceMutationService.CreateObject(item.Id, item.SellingPrice, DateTime.Now);
                    item.PriceMutationId = priceMutation.Id;
                }
                decimal itemPrice = customPurchaseInvoiceDetail.Amount / customPurchaseInvoiceDetail.Quantity;
                item.AvgPrice = _itemService.CalculateAndUpdateAvgPrice(item, customPurchaseInvoiceDetail.Quantity * (-1), itemPrice);

                IList<StockMutation> stockMutations = _stockMutationService.GetObjectsBySourceDocumentDetailForItem(customPurchaseInvoiceDetail.ItemId, Core.Constants.Constant.SourceDocumentDetailType.CustomPurchaseInvoiceDetail, customPurchaseInvoiceDetail.Id);
                foreach (var stockMutation in stockMutations)
                {
                    stockMutation.Errors = new Dictionary<string, string>();
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                    _stockMutationService.SoftDeleteObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                }
                customPurchaseInvoiceDetail.CoGS = 0;
                customPurchaseInvoiceDetail = _repository.UnconfirmObject(customPurchaseInvoiceDetail);
            }
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail SoftDeleteObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService)
        {
            if(_validator.ValidDeleteObject(customPurchaseInvoiceDetail, _customPurchaseInvoiceService))
            {
                CustomPurchaseInvoice customPurchaseInvoice = _customPurchaseInvoiceService.GetObjectById(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
                _repository.SoftDeleteObject(customPurchaseInvoiceDetail);
                customPurchaseInvoice.Total = CalculateTotal(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
                customPurchaseInvoice.CoGS = CalculateCoGS(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
                _customPurchaseInvoiceService.GetRepository().Update(customPurchaseInvoice);
            }
            return customPurchaseInvoiceDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public decimal CalculateTotal(int CustomPurchaseInvoiceId)
        {
            IList<CustomPurchaseInvoiceDetail> customPurchaseInvoiceDetails = GetObjectsByCustomPurchaseInvoiceId(CustomPurchaseInvoiceId);
            decimal Total = 0;
            foreach (var customPurchaseInvoiceDetail in customPurchaseInvoiceDetails)
            {
                Total += customPurchaseInvoiceDetail.Amount;
            }
            return Total;
        }

        public decimal CalculateCoGS(int CustomPurchaseInvoiceId)
        {
            IList<CustomPurchaseInvoiceDetail> customPurchaseInvoiceDetails = GetObjectsByCustomPurchaseInvoiceId(CustomPurchaseInvoiceId);
            decimal Total = 0;
            foreach (var customPurchaseInvoiceDetail in customPurchaseInvoiceDetails)
            {
                Total += customPurchaseInvoiceDetail.CoGS;
            }
            return Total;
        }

        public decimal GetHighestPrice(/*int CustomPurchaseInvoiceId,*/ int ItemId)
        {
            //IList<CustomPurchaseInvoiceDetail> customPurchaseInvoiceDetails = GetObjectsByCustomPurchaseInvoiceId(CustomPurchaseInvoiceId);
            
            decimal Price = 0;
            /*foreach (var customPurchaseInvoiceDetail in customPurchaseInvoiceDetails)
            {
                if (customPurchaseInvoiceDetail.ItemId == ItemId && Price < customPurchaseInvoiceDetail.Price)
                {
                    Price = customPurchaseInvoiceDetail.Price;
                };
            }*/
            CustomPurchaseInvoiceDetail hiCustomPurchaseInvoiceDetail = GetQueryable().Where(x => x.ItemId == ItemId).OrderByDescending(x => x.Price).FirstOrDefault();
            if (hiCustomPurchaseInvoiceDetail != null)
            {
                Price = hiCustomPurchaseInvoiceDetail.Price;
            }
            return Price;
        }

    }
}
