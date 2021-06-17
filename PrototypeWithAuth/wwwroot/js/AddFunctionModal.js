 $(function(){
     $(".protocol, .product, input.unique-number").change(function () {
         if($(this).val()!='')
         {
             var uniqueNumber = $(".unique-number").val();
             var objectID =$("select.object").val();
             if(!$(".unique-number").valid())
             {
                uniqueNumber="";
                 if(objectID ==0 || objectID==''
                     || $(this).hasClass("unique-number"))
                 {
                    return;
                 }
             }
            $.ajax({
                    async: true,
                    url: "/Protocols/_AddFunctionModal?objectID=" + objectID+ "&functionTypeID=" + $(".function-typeID").val()+"&uniqueNumber="+uniqueNumber,
                    type: 'GET',
                    cache: true,
                    success: function (data) {
                        $("._AddFunctionModal").html(data)
                        $("._AddFunctionModal .mdb-select").materialSelect();
                        return;
                    }
                 });
         }
         
        return;
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
     });