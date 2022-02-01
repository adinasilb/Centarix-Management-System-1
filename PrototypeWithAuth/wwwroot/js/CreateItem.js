$(function () {

    $('.partial-div, body').off('change', 'select.subcategory').on('change', 'select.subcategory', function (e) {
        if ($('#masterSectionType').val() != "Operations") {
            console.log("subcategory change")
            var subcategoryID = $("#sublist").val()
            var pageType = $("#masterPageType").val()
            var sectionType = $("#masterSectionType").val()
            var modalType = $("#modalType").val();
            var requestID = $("#Requests_0__RequestID").val()
            if (modalType == "Create") {
                var itemName = $("#Requests_0__Product_ProductName").val()
                console.log("name " + itemName);
                var isRequestQuote = false; //always false until put back in //$(".isRequest").is(":checked")
                console.log(isRequestQuote)
                console.log("subcategory " + subcategoryID)
                var url = "/Requests/CreateItemTabs/?productSubCategoryId=" + subcategoryID + "&PageType=" + pageType + "&itemName=" + itemName + "&isRequestQuote=" + isRequestQuote;
                console.log(url);
                if (subcategoryID != "") {
                    $.ajax({
                        //processData: true,
                        //contentType: true,
                        async: true,
                        url: url,
                        type: 'GET',
                        cache: false,
                        //data: formData,
                        success: function (data) {
                            $(".outer-partial").html(data);
                            $(".mdb-select").materialSelect();
                            $.fn.AddSaveItemClass();
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
            else if (modalType == "Edit") {
                var url = "/Requests/ItemData/?id=" + requestID + "&tab=1"+"&productSubCategoryId=" + subcategoryID + "&SectionType=" + sectionType;
                if (subcategoryID != "") {
                    $.ajax({
                        //processData: true,
                        //contentType: true,
                        async: true,
                        url: url,
                        type: 'GET',
                        cache: false,
                        //data: formData,
                        success: function (data) {
                            $(".ordersItemForm .partial-div").html(data);
                            $(".ordersItemForm .mdb-select").materialSelect();
                            $("#loading").hide();
                            var category = $("#categoryDescription").val();
                            console.log("category " + category)
                            $('.turn-edit-on-off').prop("checked", true);
                            $(".ordersItemForm .edit-mode-switch-description").text("Edit Mode On");
                            $(".ordersItemForm .turn-edit-on-off").attr('name', 'edit');
                            $("." + category).removeClass("d-none");
                            $("." + category).prop("disabled", false);
                            enableMarkReadonly($('.turn-edit-on-off'));
                            $(".proprietryHidenCategory").attr("disabled", false);
                            $(".proprietryHidenCategory").attr("disabled", false);               
                            $.ajax({
                                //processData: true,
                                //contentType: true,
                                async: true,
                                url: "/Requests/GetCategoryImageSrc?productSubCategoryID="+subcategoryID,
                                type: 'GET',
                                cache: false,
                                //data: formData,
                                success: function (data) {
                                    $(".sub-category-image").attr("src", data);
                                }
                            })
                        }
                    })
                }
            }
        }            
    })
   

    


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
        var subcategoryID = $("#Requests_0__Product_ProductSubcategoryID").val();
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
                //alert("newindex: " + newIndex);
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
                    $(".temprequesthiddenfors").html(''); //remove hidden fors so don't conflict further down the line
                    $.fn.OpenModal('modal', 'step-1', data)
                }
            });
        }
        $("#myForm").data("validator").settings.ignore = ':not(select:hidden, .location-error:hidden,input:visible, textarea:visible)';
       
    })
    $('.ordersItemForm').off('click', '.remove-item').on('click','.remove-item', function (e) {
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
    $('.ordersItemForm').off('change', '.include-vat-radio').on('change', '.include-vat-radio', function (e) {
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
    $('.ordersItemForm').off('click', '.received-check').on('click', '.received-check', function (e) {
        var index = $(this).attr("index");
        var checked = $(this).is(":checked");
        console.log(index)
        console.log('checked!!!!')
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

        $(".add-comment").off("click").click(function () {
            var type = $(this).attr("data-val")
            var index = $('#index').val();
            $.ajax({
                async: false,
                url: '/Requests/_CommentInfoPartialView?typeID=' + type + '&index=' + index,
                type: 'GET',
                cache: false,
                success: function (data) {
                    $(".comment-info-div").append(data);
                    $('#index').val(++index);
                }
            });
        });

        $('#addRequestComment').popover('toggle');

    });

    $('.open-history-item-modal').off('click').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();
        //alert('here')
        console.log(this)
        $(".tooltip").remove();
        //highlight this
        $(this).parents('tr').addClass('gray-background');
        $(this).parents('tr').addClass('current-item');
        var $itemurl = "/Requests/HistoryItemModal/?id=" + $(this).attr("value") + "&SectionType=" + $("#masterSectionType").val();
        $.fn.CallPageRequest($itemurl, 'historyItem');
    });

    $(".url-shown").on("click", function (e) {
        $("#url-click").click();
    });
})