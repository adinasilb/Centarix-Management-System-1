$("form").off("click", ".function").on("click", ".function", function (e) {
    e.preventDefault();
    var lineID = $(this).attr("lineID");
    if ($(this).attr("lineID") == undefined) {
        lineID = $(".focused-line").attr("data-val")
    }
    var url = "";
    if ($("#masterPageType").val() == "ProtocolsReports") {
        url = "/Protocols/AddReportFunctionModal?FunctionTypeID=" + $(this).val() + "&ReportID=" + $("#ReportID").val() + "&closingTags=" + $('#closingTags').val();
        $.fn.CallPageRequest(url, "addFunction");
    }
    /*if ($("#masterPageType").val() == "ProtocolsCreate")*/
    else {
        if (lineID != undefined) {
            url = "/Protocols/AddFunctionModal?FunctionTypeID=" + $(this).attr("typeID") + "&LineID=" + lineID + "&functionIndex=" + $(this).attr("value") + "&modalType=" + $(this).attr("modaltype") + "&guid=" + $(this).attr("guid")
            $.fn.CallPageRequest(url, "addFunction");
        }
        else if ($(".results-tab").hasClass("active")) {
            url = "/Protocols/AddResultsFunctionModal?FunctionTypeID=" + $(this).attr("typeID") + "&ProtocolInstanceID=" + $(".protocolInstanceID").val() + "&functionResultID=" + $(this).val() + "&modalType=" + $(this).attr("modaltype") + "&closingTags=" + $('#closingTags').val();
            $.fn.CallPageRequest(url, "addFunction");
        }
    }
});


$(".add-function").off('click', ".saveFunction, .removeFunction").on('click', ".saveFunction, .removeFunction", function (e) {
    e.preventDefault();
    var removing = $(this).hasClass("removeFunction");
    console.log("removing: " + removing)
    if (removing) {
        $(".isRemove").val(true);
        var functionSelect;
        var changeToTriggerSelect;
        if ($("#masterPageType").val() == "ProtocolsReports") {
            var functionReportID = $(".function-reportID").val()
            functionSelect = $(".report-function[functionReportid=" + functionReportID + "]")
            console.log(functionSelect)
            changeToTriggerSelect = $(".text-editor")
        }
        else if ($(".results-tab").hasClass("active")) {
            var functionResultID = $(".function-resultID").val()
            functionSelect = $(".result-function[functionResultID=" + functionResultID + "]")
            console.log(functionSelect)
            changeToTriggerSelect = $(".text-editor")
        }
        else {
            functionSelect = $("div.line-input[data-val=" + $(".lineID").val() + "]").find("a.function-line-node[functionline=" + $(".function-lineID").val() + "]");
            changeToTriggerSelect = $("div.line-input[data-val=" + $(".lineID").val() + "]")
        }
        var prev = functionSelect.prev();
        var next = functionSelect.next();
        var html = prev.html() + next.html();
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
    $(".addFunctionForm").data("validator").settings.ignore = "";
    var valid = $(".addFunctionForm").valid();
    console.log("valid form: " + valid)
    if (!valid && !removing) {

        if (!$('.activeSubmit').hasClass('disabled-submit')) {
            $('.activeSubmit').addClass('disabled-submit')
        }
        return;
    }
    else {
        var functionFormData = new FormData($(".addFunctionForm")[0]);
        var reportFormData = new FormData($(".createReportForm")[0]);
        var protocolFormData = new FormData($(".createProtocolForm")[0]);

        var functionName = "AddFunctionModal";
        if ($("#masterPageType").val() == "ProtocolsReports") {
            if ($(this).hasClass("removeFunction")) {
                functionName = "DeleteReportDocumentModal"
                functionFormData = new FormData($(".deleteFunctionForm")[0]);
            }
            else {
                functionName = "AddReportFunctionModal?guid=" + $(".guid").val();
            }
            for (var pair of reportFormData.entries()) {
                functionFormData.append(pair[0], pair[1]);
            }
        }
        else if ($(".results-tab").hasClass("active")) {
            if ($(this).hasClass("deleteDoc")) {
                functionName = "DeleteResultsDocumentModal"
                functionFormData = new FormData($(".deleteFunctionForm")[0]);
            }
            else{

                functionName = "AddResultsFunctionModal?guid=" + $(".guid").val();
            }
            for (var pair of protocolFormData.entries()) {
                functionFormData.append(pair[0], pair[1]);
            }
        }
        else {
            for (var pair of protocolFormData.entries()) {
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
                if ($("#masterPageType").val() == "ProtocolsReports" || $(".results-tab").hasClass("active")) {
                    $(".text-editor-div").html(data);
                    if (!removing) {
                        var newDiv = $(".added-div");

                    var newDivText = newDiv.next()?.html()
                    console.log(newDivText)
                    if (newDivText == null) {
                        if (newDiv.get(0).nextSibling != null) {
                            console.log("no div")
                            newDivText = newDiv[0].nextSibling.textContent;
                        }
                        else {
                            newDivText = "";
                        }
                    }
                    newDiv.html(newDiv.html()+newDivText)
                    newDiv[0].nextSibling?.remove();
                    newDiv.removeClass("added-div");
                    $('.text-editor').trigger("change")
                    $(".createReportForm .back-arrow").addClass("load-save-report")
                }
                
                //functionSelect.append(" <div contenteditable='true' class= 'editable-span form-control-plaintext text-transform-none'></div>")
            }
            else {
                $("._Lines").html(data);
                }
             console.log("save modal")
                $(".protocols-edit-arrow.back-arrow").addClass("save-item");
            $.fn.CloseModal('add-function');
        },
        error: function (jqxhr) {
            if (jqxhr.status == 500) {
                $.fn.OpenModal('modal', 'add-function', jqxhr.responseText);
            }
            return true;
        }
    });
        }
        $(".addFunctionForm").data("validator").settings.ignore = ':not(select:hidden, .location-error:hidden,input:visible, textarea:visible)';    

});

$(".remove-function").click(function (e) {
    e.preventDefault();

    url = $(this).attr("url");
    $.fn.CallPageRequest(url, "addFunction");
});

$("form").off("click", ".open-line-product").on("click", ".open-line-product", function (e) {
    e.preventDefault();
    if($(".modal").length>0)
    {
         var url = "/Protocols/_ProtocolsProductDetails?productID=" + $(this).attr("value");  
        var type = "summaryProtocolsParital";
    }
    else
    {
        var url = "/Protocols/ProtocolsProductDetails?productID=" + $(this).attr("value");
        var type ="summary";
    }
      $.fn.LoadProtocolDetailsModal(url, type);
});
$(".modal").off("click", ".back-btn-container").on("click", ".back-btn-container", function (e) {
    e.preventDefault();
    var index =$(".lastIndexOfUrls").val();
    var url = $(".LastUrl"+index).val()+"&backButtonClicked=true";   
    var type=""
    if(url.indexOf("_ProtocolsDetailsFloatModal") !=-1)
    {
        type="protocolFloatModalPartial";
    }
    else if(url.indexOf("_ProtocolsProductDetails") !=-1)
    {
        type="summaryProtocolsParital";
    }
    else
    {
        type="protocolFloatModal";
    }
    $.fn.LoadProtocolDetailsModal(url, type);
});

$("form").off("click", ".open-line-protocol").on("click", ".open-line-protocol", function (e) {
    e.preventDefault();
     if($(".modal").length>0)
    {
         var url = "/Protocols/_ProtocolsDetailsFloatModal?protocolID=" + $(this).attr("value");  
         var type = "protocolFloatModalPartial";
    }
    else
    {
         var url = "/Protocols/ProtocolsDetailsFloatModal?protocolID=" + $(this).attr("value");    
         var type ="protocolFloatModal";
    }
    $.fn.LoadProtocolDetailsModal(url, type);
});

    $.fn.LoadProtocolDetailsModal = function(url, $type)
    {
        $("#loading").show();
    	var formdata = new FormData($(".inner-lines-link")[0]);
        $.ajax({
            contentType: false,
            processData: false,
            async: true,
            url: url,
            data: formdata,
            traditional: true,
            type: "POST",
            cache: false,
            success: function (data) {
                $("#loading").hide();
                switch ($type) {                       
                        case "summary":
                            //$.fn.OnOpenModalView();
                            $.fn.OpenModal('modal', 'edits', data)
                            $.fn.LoadSummaryModal('edits');
                            $('.modal-content a:first').tab('show');
                            break;
                        case "summaryProtocolsParital":    
                            $("._ProtocolsDetailsFloatModal").html(data);
                            $.fn.LoadSummaryModal('_ProtocolsDetailsFloatModal');
                            $('.modal-content a:first').tab('show');                         
                            break;                                        
                        case "protocolFloatModal":
                            $.fn.OpenModal('modal', 'protocol-details', data)
                            $.fn.ProtocolsMarkReadonly("protocol-details")
                            break
                        case "protocolFloatModalPartial":
                            $("._ProtocolsDetailsFloatModal").html(data);
                            $.fn.ProtocolsMarkReadonly("protocol-details")
                           $(".back-btn-container").removeClass("d-none")
                            break 
                    }   
                return true;
            }
        });
    }