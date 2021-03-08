$(function () {
    $("#submitCategory").on("click", function (e) {
        console.log("submit category")
        e.preventDefault();
        $("#myForm").data("validator").settings.ignore = "";
        var valid = $("#myForm").valid();
		console.log("valid form: " + valid)
        if (!valid) {

            e.stopPropagation();
            if (!$('input[type="submit"], button[type="submit"] ').hasClass('disabled-submit')) {
                $('input[type="submit"], button[type="submit"] ').addClass('disabled-submit')
            }

        }
		else {
            //var categoryId = $("#categorylist").val();
            $('input[type="submit"], button[type="submit"] ').removeClass('disabled-submit')
            $("#loading").show();
            var formData = new FormData($("#myForm")[0]);
            console.log(...formData)
			$.ajax({processData: false,
				contentType: false,
				
                async: true,
                url: "/Requests/CreateItemTabs",
                type: 'GET',
                data: formData,
				cache: false,
				success: function (data) {
                    $(".outer-partial").html(data);
                    $("#loading").hide();
                    var category = $("#categoryDescription").val();
                    $("." + category).removeClass("d-none");
				}
             
			})
        }
        $("#myForm").data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';

        return false;
    });
    $("#sublist").off("change").on("change", function (e) {
        if ($('#masterSectionType').val() != "Operations") {
            console.log("subcategory change")
            var subcategoryID = $("#sublist").val()
            var pageType = $("#masterPageType").val()
            var itemName = $("#Requests_0__Product_ProductName").val()
            var isRequestQuote = $(".isRequest").is(":checked")
            console.log(isRequestQuote)
            console.log("subcategory " + subcategoryID)
            if (subcategoryID != "") {
                $.ajax({
                    //processData: true,
                    //contentType: true,
                    async: true,
                    url: "/Requests/CreateItemTabs/?productSubCategoryId=" + subcategoryID + "&PageType=" + pageType + "&itemName=" + itemName + "&isRequestQuote=" + isRequestQuote,
                    type: 'GET',
                    cache: false,
                    //data: formData,
                    success: function (data) {
                        $(".outer-partial").html(data);
                        $(".mdb-select").materialSelect();

                        $("#loading").hide();
                        var category = $("#categoryDescription").val();
                        console.log("category " + category)
                        $("." + category).removeClass("d-none");
                        $("." + category).prop("disabled", false);
                        $.fn.DisableMaterialSelect("#parentlist", 'select-options-parentlist');
                        $.fn.DisableMaterialSelect("#sublist", 'select-options-sublist');
                        $(".proprietryHidenCategory").attr("disabled", false);
                        console.log($("#requestQuoteValue").attr("value"))
                        if ($("#requestQuoteValue").val() == "true") {
                            console.log("request")
                            $(".requestPriceQuote").addClass("d-none");
                            $(".requestPriceQuote").attr("disabled", true)
                        }
                        else {
                            $('.requestQuoteHide').addClass("d-none");
                        }
                    }
                })
            }
        }
    })
   
	$(".categoryForm").validate({
		rules: {
			"SelectedCategory": "required"
        }
    })
     $(".order-tab").on("click", function (e) {
        if ($(".all-mail").length == 0) {
            let data = [
                $("#vendor-primary-email").val()
            ]
            $("#allEmails").email_multiple({
                data: data
            })
            var newEmailLine = $(".enter-mail-id");
            newEmailLine.removeAttr('placeholder');
            newEmailLine.addClass('form-control-plaintext');
            newEmailLine.addClass('border-bottom');

            $(".email-ids:first span").attr("disabled", true);
            $(".email-ids:first span").css("display", "none");
            $(".email-ids:first").addClass("supplier-email");
        }
    });
    

    $.fn.UpdatePrimaryOrderEmail = function () {
        if ($(".supplier-email").length > 0) {
            $(".supplier-email").html($("#vendor-primary-email").val());
            $(".isSupplier").val($("#vendor-primary-email").val());
        }
        else {
            $('.all-mail').prepend('<span class="email-ids supplier-email">' + $("#vendor-primary-email").val() + '</span>');
            $(".isSupplier").val($("#vendor-primary-email").val());
        }
    };

    $.fn.CheckListLength = function () {
        //Disable if 5 emails
        var listlength = $(".email-ids").length;
        if (listlength >= 5 || (listlength == 4 && $(".supplier-email").length == 0)) {
            $(".enter-mail-id").attr("disabled", true);
        }
        else {
            $(".enter-mail-id").attr("disabled", false);
        }
    }

    $.fn.RemoveFromHiddenIds = function (emailValue) {
        $(".emailaddresses[value='" + emailValue + "']:first").val('');
    }

    $.fn.AddToHiddenIds = function (emailValue) {
        $(".emailaddresses[value='']:first").val(emailValue);
    }


    $(".isRequest").click(function(){
     if ($(this).is(":checked")) {
            $(".requestPriceQuote").addClass("d-none");
         $(".requestPriceQuote ").attr("disabled", true)
         $(".requestQuoteHide").removeClass("d-none");
        }
        else {
            $(".requestPriceQuote").removeClass("d-none");
         $(".requestPriceQuote ").attr("disabled", false)
         $(".requestQuoteHide").addClass("d-none");
        }
    
    })
    $('.complete-order').click(function (e) {
        if (!$(this).hasClass('disabled-submit')) {
            $(".save-item").removeClass("save-item")
        }
    })

    $('.add-item').off('click').on('click', function (e) {
        var newIndex = $(this).attr('data-val');
        console.log(newIndex)
        $.ajax({
            async: true,
            url: "/Requests/_PartialItemOperationsTab?index=" + newIndex,
            type: 'GET',
            cache: false,
            success: function (data) {
                var newitem = $(data);
                $('.operations-item-div').append(newitem);
                $("#addOperationItem").attr('data-val', parseInt(newIndex) + 1);
                $(".mdb-select" + newIndex).materialSelect();
                $(".parent-select" + newIndex).materialSelect();
                $(".remove-item-btn").removeClass("d-none")
            }
        });

    })
    $(".save-operations-item").off('click').on('click', function (e) {
        e.preventDefault();
        console.log("saveOperations")
        var formData = new FormData($("#myForm")[0]);
        $.ajax({
            processData: false,
            contentType: false,
            async: true,
            url: "/Requests/AddItemView?OrderType=SaveOperations",
            type: 'POST',
            data: formData,
            cache: false,
            success: function (data) {
                $.fn.OpenModal('termsModal', 'terms', data)
            }
        });
    })
})