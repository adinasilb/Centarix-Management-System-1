using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class AddLineFunctionViewModel : AddFunctionViewModel<FunctionLine>
    {
        public Guid UniqueGuid { get; set; }
        public int FunctionIndex { get; set; }
        public string ClosingTags { get; set; }

        public AddFunctionViewModel<FunctionBase> ConvertToBaseClass( )
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
                ProtocolVersions = ProtocolVersions,
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
