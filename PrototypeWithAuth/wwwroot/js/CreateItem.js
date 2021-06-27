$(function () {

    $("#sublist").off("change").on("change", function (e) {
        if ($('#masterSectionType').val() != "Operations") {
            console.log("subcategory change")
            var subcategoryID = $("#sublist").val()
            var pageType = $("#masterPageType").val()
            var itemName = $("#Requests_0__Product_ProductName").val()
            var isRequestQuote = false; //always false until put back in //$(".isRequest").is(":checked")
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
                        $(".top-menu").addClass("save-item");
                        $(".side-menu").addClass("save-item");
                        $(".module-button ").addClass("save-item");
                        $(".notificationDiv").addClass("save-item");
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
     else if ($('#sublist').val() != ''){
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
        var currency = $("#currency").val();
        var subcategoryID = $("#Requests_0__Product_ProductSubcategory_ProductSubcategoryID").val();
        var url = "/Requests/_PartialItemOperationsTab?index=" + newIndex
        if (subcategoryID != null) {
            url = url + "&subcategoryID=" + subcategoryID
        }
        console.log(subcategoryID)
        $.ajax({
            async: true,
            url: url,
            type: 'GET',
            cache: false,
            success: function (data) {
                var newitem = $(data);
                $('.operations-item-div').append(newitem);
                $("#addOperationItem").attr('data-val', parseInt(newIndex) + 1);
                $(".mdb-select" + newIndex).materialSelect();
                $(".parent-select" + newIndex).materialSelect();
                if (currency == "USD") {
                    $(".dollar-cost").attr("disabled", false)
                    $(".dollar-cost").removeClass("disabled-text");
                    $(".request-cost-dollar-icon").removeClass("disabled-text");
                    $(".shekel-cost").attr("readonly", true)
                    $(".shekel-cost").addClass("disabled-text");
                    $(".request-cost-shekel-icon").addClass("disabled-text");
                }
            }
        });

    })

        $('.item-name').keyup(function (e) {   
            var val = $(this).val();
            var spaces =  val.split(" ");
            var spaceCount = spaces.length;
             console.log(spaceCount)
            var rows = Math.ceil((val.length+spaceCount) / 45);
            var lines =   val.split("\n");
            var lineCount = lines.length;
           // console.log(lineCount)
            if(lineCount>0){
                rows+=lineCount-1;
            }
            if(rows>3)
            {
                $(this).val(val.slice(0, -1))
                rows = 3;
            }
            $(this).attr('rows', rows);
        });

    $(".save-operations-item").off('click').on('click', function (e) {
        e.preventDefault();
        console.log("saveOperations")
        $("#myForm").data("validator").settings.ignore = "";
        var valid = $("#myForm").valid();
        console.log("valid form: " + valid)
        if (!valid) {

            if (!$('.activeSubmit').hasClass('disabled-submit')) {
                $('.activeSubmit').addClass('disabled-submit')
            }

        }
        else {
            $('.activeSubmit').removeClass('disabled-submit')
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
                    $.fn.OpenModal('modal', 'step-1', data)
                }
            });
        }
        $("#myForm").data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';
       
    })
    $('body').off('click', 'remove-item').on('click','.remove-item', function (e) {
        var index = $(this).attr('data-val');
        var items = $('.partial-item-tab').length
        if (items > 1) {
            console.log("index " + index)
            var itemClass = '.partial-item-tab.' + index;
            $(itemClass).children(".row").each(function (e) {
                $(this).replaceWith("");
            })
            
            var deletedid = "Requests_" + index + "__Ignore";
            console.log("deleted hidden id: " + deletedid);
            $("#" + deletedid).val("true");
        }
    })
    $('body').off('change', '.include-vat-radio').on('change', '.include-vat-radio', function (e) {
        console.log("radio click")
        var index = $(this).attr("index");
        var vatInfoClass = ".vat-info";
        var noVatRadioId = "NoVAT";
        var includeVatId = "#Requests_0__IncludeVAT";
        if (index != null) {
            vatInfoClass = vatInfoClass + index;
            noVatRadioId = noVatRadioId + index;
            includeVatId = "#Requests_" + index + "__IncludeVAT";
        }
        
        if ($(this).attr("id") == noVatRadioId) {
            $(includeVatId).val("false");
            $(vatInfoClass).addClass("d-none");
        }
        else {
            $(includeVatId).val("true");
            $(vatInfoClass).removeClass("d-none");
        }
    })
    $('body').off('click', '.received-check').on('click', '.received-check', function (e) {
        var index = $(this).attr("index");
        var checked = $(this).is(":checked");
        console.log(index)
        $("#Requests_" + index + "__IsReceived").attr("value", checked)
    })

    $('.ordersItemForm').on('click', "#addRequestComment", function () {
        console.log("clicked!");
        console.log($('#popover-content').html())
        $('[data-toggle="popover"]').popover('dispose');
        $('#addRequestComment').popover({
            sanitize: false,
            placement: 'bottom',
            html: true,
            content: function () {
                console.log('in function')
                return $('#popover-content').html();
            }
        });
        $('#addRequestComment').popover('toggle');

    });

    $('.open-history-item-modal').off('click').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();
        //alert('here')
        //highlight this
        $(this).parents('tr').addClass('gray-background');
        $(this).parents('tr').addClass('current-item');
        var $itemurl = "/Requests/HistoryItemModal/?id=" + $(this).attr("value") + "&SectionType=" + $("#masterSectionType").val();
        $.fn.CallPageRequest($itemurl, 'historyItem');
    });
})