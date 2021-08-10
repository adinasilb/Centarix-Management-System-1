﻿using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class AddResultsFunctionViewModel : AddFunctionViewModel<FunctionResult>
    {
        public string ClosingTags { get; set; }
        public AddFunctionViewModel<FunctionBase> ConvertToBaseClass()
        {
            AddFunctionViewModel<FunctionBase> baseObject = new AddFunctionViewModel<FunctionBase>
            {
                Function = Function,
                IsRemove = IsRemove,
                Products = Products,
                ProductSubcategories = ProductSubcategories,
                ParentCategories = ParentCategories,
                ProtocolCategories = ProtocolCategories,
                ProtocolSubCategories = ProtocolSubCategories,
                Protocols = Protocols,
                Creators = Creators,
                DocumentsInfo = DocumentsInfo,
                DocumentsModalViewModel = DocumentsModalViewModel,
                ModalType = ModalType,
                ErrorMessage = ErrorMessage,
                Vendors = Vendors
            };
            return baseObject;
        }
    }
}
