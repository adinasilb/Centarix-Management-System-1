using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class FunctionTypeData
    {
        public static List<FunctionType> Get()
        {
            List<FunctionType> list = new List<FunctionType>();
            list.Add(new FunctionType
            {
                FunctionTypeID = 1,
                FunctionDescription = "Add Image",
                Icon = "icon-account_box-24px1",
                IconActionClass = "add-image-to-line",
                DescriptionEnum = "AddImage"
            });
            list.Add(new FunctionType
            {
                FunctionTypeID = 2,
                FunctionDescription = "Add Timer",
                Icon = "icon-centarix-icons-19",
                IconActionClass = "add-timer-to-line",
                DescriptionEnum = "AddTimer"
            });
            list.Add(new FunctionType
            {
                FunctionTypeID = 3,
                FunctionDescription = "Add Comment",
                Icon = "icon-comment-24px",
                IconActionClass = "add-comment-to-line",
                DescriptionEnum = "AddComment"
            });
            list.Add(new FunctionType
            {
                FunctionTypeID = 4,
                FunctionDescription = "Add Warning",
                Icon = "icon-report_problem-24px",
                IconActionClass = "add-warning-to-line",
                DescriptionEnum = "AddWarning"
            });
            list.Add(new FunctionType
            {
                FunctionTypeID = 5,
                FunctionDescription = "Add Tip",
                Icon = "icon-tip-24px",
                IconActionClass = "add-tip-to-line",
                DescriptionEnum = "AddTip"
            });
            list.Add(new FunctionType
            {
                FunctionTypeID = 6,
                FunctionDescription = "Add Table",
                Icon = "icon-table_chart-24px1",
                IconActionClass = "add-table-to-line",
                DescriptionEnum = "AddTable"
            });
            list.Add(new FunctionType
            {
                FunctionTypeID = 7,
                FunctionDescription = "Add Template",
                Icon = "",
                IconActionClass = "add-template-to-line",
                DescriptionEnum = "AddTemplate"
            });
            list.Add(new FunctionType
            {
                FunctionTypeID = 8,
                FunctionDescription = "Add Stop",
                Icon = "icon-stop-24px",
                IconActionClass = "add-stop-to-line",
                DescriptionEnum = "AddStop"
            });
            list.Add(new FunctionType
            {
                FunctionTypeID = 9,
                FunctionDescription = "Add Link To Product",
                Icon = "icon-attach-item-24px",
                IconActionClass = "add-product-to-line",
                DescriptionEnum = "AddLinkToProduct"
            });
            list.Add(new FunctionType
            {
                FunctionTypeID = 10,
                FunctionDescription = "Add Link To Protocol",
                Icon = "icon-attach-protocol-24px",
                IconActionClass = "add-protocol-to-line",
                DescriptionEnum = "AddLinkToProtocol"
            });
            list.Add(new FunctionType
            {
                FunctionTypeID = 11,
                FunctionDescription = "Add File",
                Icon = "icon-description-24px2",
                IconActionClass = "add-file-to-line",
                DescriptionEnum = "AddFile"
            });
            return list;
        }

    }
}
