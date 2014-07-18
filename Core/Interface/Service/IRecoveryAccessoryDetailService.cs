﻿using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IRecoveryAccessoryDetailService
    {
        IRecoveryAccessoryDetailValidator GetValidator();
        IRecoveryAccessoryDetailRepository GetRepository();
        IList<RecoveryAccessoryDetail> GetAll();
        IList<RecoveryAccessoryDetail> GetObjectsByRecoveryOrderDetailId(int recoveryOrderDetailId);
        IList<RecoveryAccessoryDetail> GetObjectsByItemId(int ItemId);
        RecoveryAccessoryDetail GetObjectById(int Id);
        RecoveryAccessoryDetail CreateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                             IItemService _itemService, IItemTypeService _itemTypeService);
        RecoveryAccessoryDetail CreateObject(int RecoveryOrderDetailId, int ItemId, int Quantity, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                             IItemService _itemService, IItemTypeService _itemTypeService);
        RecoveryAccessoryDetail UpdateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                             IItemService _itemService, IItemTypeService _itemTypeService);
        RecoveryAccessoryDetail SoftDeleteObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail FinishObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryOrderService _recoveryOrderService);
        RecoveryAccessoryDetail UnfinishObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryOrderService _recoveryOrderService);
        bool DeleteObject(int Id);
    }
}