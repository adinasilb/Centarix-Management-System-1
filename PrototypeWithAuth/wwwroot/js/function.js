$("form").off("click", ".function, .remove-function").on("click", ".function, .remove-function", function (e) {
    e.preventDefault();
    console.log("function")
    var lineID =$(this).attr("lineID");
    if($(this).attr("lineID")==undefined){
        lineID =$(".focused-line").attr("data-val") 
    }
    var url = "";
    if ($("#masterPageType").val() == "ProtocolsReports") {
        url = "/Protocols/AddReportFunctionModal?FunctionTypeID=" + $(this).val() + "&ReportID=" + $("#ReportID").val();
        	$.fn.CallPageRequest( url , "addFunction");
    }
	/*if ($("#masterPageType").val() == "ProtocolsCreate")*/
    else {
	    if(lineID != undefined)
	    {
		    url = "/Protocols/AddFunctionModal?FunctionTypeID=" + $(this).attr("typeID") + "&LineID=" + lineID+"&functionLineID="+$(this).attr("value");
        	$.fn.CallPageRequest( url , "addFunction"); 
        }
	}
});

$(".addFunctionForm").off('click', ".saveFunction, .removeFunction").on('click', ".saveFunction, .removeFunction",function (e) {
    e.preventDefault();
    if($(this).hasClass("removeFunction"))
    {
        $(".isRemove").val(true);
         var functionSelect;
        var changeToTriggerSelect;
        if ($("#masterPageType").val() == "ProtocolsReports") {
            var functionReportID = $(".function-reportID")
            functionSelect = $(".report-function[functionReportID=" + functionReportID + "]")
            console.log(functionSelect)
            changeToTriggerSelect = $(".report-text")
         }
         else {
	        functionSelect =$("div.line-input[data-val="+$(".lineID").val()+"]").find("a.function-line-node[functionline="+$(".function-lineID").val()+"]");   
            changeToTriggerSelect=$("div.line-input[data-val="+$(".lineID").val()+"]")
         }
         var prev = functionSelect.prev();
         var next = functionSelect.next();
         var html = prev.html()+next.html();
         prev.html(html);
         next.remove();
         functionSelect.remove();
         changeToTriggerSelect.trigger("change");
         //$("div.line-input").each(function(){  
         //    console.log("this is html:"+   $(this).html+"thisis the end")
         //   if($.trim($(this).html())=='')
         //   {
         //       $(this).remove();
         //   }
         //});    
    }
    //$('.materialForm').data("validator").settings.ignore = "";
    //var valid = $('.materialForm').valid();
    //console.log("valid form: " + valid)
    //if (!valid) {
    //    e.preventDefault();
    //    if (!$('.activeSubmit').hasClass('disabled-submit')) {
    //        $('.activeSubmit').addClass('disabled-submit')
    //    }

    //}
    //else {
    //$('.activeSubmit').removeClass('disabled-submit')

    var functionFormData = new FormData($(".addFunctionForm")[0]);
    var reportFormData = new FormData($(".createReportForm")[0]);
    var protocolFormData = new FormData($(".createProtocolForm")[0]);

    var functionName = "AddFunctionModal";
    if ($("#masterPageType").val() == "ProtocolsReports") {
        functionName = "AddReportFunctionModal";
        for (var pair of reportFormData.entries())
        {
            functionFormData.append(pair[0], pair[1]);
        }
    }
    else{
         for (var pair of protocolFormData.entries())
        {
            functionFormData.append(pair[0], pair[1]);
        }
        }
    $.ajax({
        url: "/Protocols/" + functionName,
        traditional: true,
        data: functionFormData,
        contentType: false,
        processData: false,
        type: "POST",
        success: function (data) {
            if ($("#masterPageType").val() == "ProtocolsReports") {
                $(".report-text-div").html(data);
            }
            else {
                $("._Lines").html(data);
            }
            $.fn.CloseModal('add-function');
        },
        error: function (jqxhr) {
            if (jqxhr.status == 500) {
                $.fn.OpenModal('modal', 'add-function', jqxhr.responseText);
            }
            return true;
        }
    });

    //}
    //$('.materialForm').data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';
});

$(".link-product-dropdown").change(function(){
    var productSelector = $("select.product");
    var subCategorySelector = $("select.subCategory");
    var parentCategoryID = $("select.parentCategory").val();
    var subCategoryID = $("select.subCategory").val();
    var vendorCategoryID = $("select.vendor").val();
    var isCategoryChange= false;
    if($(this).hasClass("parentCategory"))
    {
        isCategoryChange=true;
    }
    var url = "/Protocols/FilterLinkToProduct"
    $.getJSON(url, { ParentCategoryID: parentCategoryID,  SubCategoryID :subCategoryID,  VendorID: vendorCategoryID}, function (data) {
		   console.log(data)
            productSelector.children("option").each(function (i, option) {
				option.remove();
			});
			var productItem1 = '<option value="">Select Product</option>';
            productSelector.append(productItem1);
            if(isCategoryChange)
            {
                subCategorySelector.children("option").each(function (i, option) {
				    option.remove();
		        });
                var firstitem1 = '<option value="">Select SubCategory</option>';			
		        subCategorySelector.append(firstitem1);
            }		        
          
			$.each(data.products, function (i, product) {
                var newitem1 = '<option value="' + product.productID + '">' + product.name + '</option>'
				productSelector.append(newitem1);
            });		             			  
            if(isCategoryChange)
            {
                $.each(data.productSubCategories, function (i, subCat) {
                var newitem1 = '<option value="' + subCat.subCategoryID + '">' + subCat.subCategoryDescription + '</option>'
			    subCategorySelector.append(newitem1);
                });
                subCategorySelector.materialSelect({destroy: true});
                subCategorySelector.materialSelect();
            }	
			
            productSelector.materialSelect({destroy: true});  
            productSelector.materialSelect();   
         
			return false;
	    });

});
$(".link-protocol-dropdown").change(function(){
    var protocolSelector = $("select.protocol");
    var subCategorySelector = $("select.protocolSubCategory");
    var parentCategoryID = $("select.protocolParentCategory").val();
    var subCategoryID = $("select.protocolSubCategory").val();
    var creatorID = $("select.creator").val();
    var isCategoryChange= false;
    if($(this).hasClass("protocolParentCategory"))
    {
        isCategoryChange=true;
    }

    var url = "/Protocols/FilterLinkToProtocol"
    $.getJSON(url, { ParentCategoryID: parentCategoryID,  SubCategoryID :subCategoryID,  creatorID: creatorID}, function (data) {
		   console.log(data)
            protocolSelector.children("option").each(function (i, option) {
				option.remove();
			});
			var productItem1 = '<option value="">Select Protocol</option>';
            protocolSelector.append(productItem1);
            if(isCategoryChange)
            {
                subCategorySelector.children("option").each(function (i, option) {
				    option.remove();
		        });
                var firstitem1 = '<option value="">Select SubCategory</option>';			
		        subCategorySelector.append(firstitem1);
            }		        
          
			$.each(data.protocols, function (i, protocol) {
                var newitem1 = '<option value="' + protocol.protocolID + '">' + protocol.name + '</option>'
				protocolSelector.append(newitem1);
            });		             			  
            if(isCategoryChange)
            {
                $.each(data.protocolSubCategories, function (i, subCat) {
                var newitem1 = '<option value="' + subCat.subCategoryID + '">' + subCat.subCategoryDescription + '</option>'
			    subCategorySelector.append(newitem1);
                });
                subCategorySelector.materialSelect({destroy: true});
                subCategorySelector.materialSelect();
            }	
			
            protocolSelector.materialSelect({destroy: true});  
            protocolSelector.materialSelect();   
         
			return false;
	    });

});

$(".add-function").off("change", ".protocol, .product").on("change", ".protocol, .product", function(){
     $.ajax({
            async: true,
            url: "/Protocols/_AddFunctionModal?objectID="+$(this).val()+"&functionTypeID="+$(".function-typeID").val(),
            type: 'GET',
            cache: true,
            success: function (data) {
               $("._AddFunctionModal").html(data)
               $("._AddFunctionModal .mdb-select").materialSelect();
               return;
            }
       });
    return;
});