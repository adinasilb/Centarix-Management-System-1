$("form").off("click", ".function").on("click", ".function", function (e) {
    e.preventDefault();
    var lineID =$(this).attr("lineID");
    if($(this).attr("lineID")==undefined){
        lineID =$(".focused-line").attr("data-val") 
    }
    var url = "";
    if ($("#masterPageType").val() == "ProtocolsReports") {
            url = "/Protocols/AddReportFunctionModal?FunctionTypeID=" + $(this).val() + "&ReportID=" + $("#ReportID").val() +"&closingTags="+$('#closingTags').val();
        	$.fn.CallPageRequest( url , "addFunction");
    }
	/*if ($("#masterPageType").val() == "ProtocolsCreate")*/
    else {
	    if(lineID != undefined)
	    {
		    url = "/Protocols/AddFunctionModal?FunctionTypeID=" + $(this).attr("typeID") + "&LineID=" + lineID+"&functionIndex="+$(this).attr("value")+"&modalType="+$(this).attr("modaltype")+"&guid="+$(this).attr("guid")
        	$.fn.CallPageRequest( url , "addFunction"); 
        }
	}
});


$(".add-function").off('click', ".saveFunction, .removeFunction").on('click',".saveFunction, .removeFunction",function (e) {
    e.preventDefault();
    var removing = $(this).hasClass("removeFunction");
    console.log("removing: "+removing)
    if(removing)
    {
        $(".isRemove").val(true);
         var functionSelect;
        var changeToTriggerSelect;
        if ($("#masterPageType").val() == "ProtocolsReports") {
            var functionReportID = $(".function-reportID").val()
            functionSelect = $(".report-function[functionReportid=" + functionReportID + "]")
            console.log(functionSelect)
            changeToTriggerSelect = $(".report-text")
         }
         else {
	        functionSelect =$("div.line-input[data-val="+$(".lineID").val()+"]").find("a.function-line-node[functionline="+$(".function-lineID").val()+"]");   
            changeToTriggerSelect=$("div.line-input[data-val="+$(".lineID").val()+"]")
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
            functionName = "AddReportFunctionModal";
        }
        for (var pair of reportFormData.entries()) {
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
                    $('.report-text').trigger("change")
                }
                
                //functionSelect.append(" <div contenteditable='true' class= 'editable-span form-control-plaintext text-transform-none'></div>")
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
        }
        $(".addFunctionForm").data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';    

});

$(".remove-function").click(function (e) {
    e.preventDefault();

    url = $(this).attr("url");
    $.fn.CallPageRequest(url, "addFunction");
});

$("form").off("click", ".open-line-product").on("click", ".open-line-product", function (e) {
	 e.preventDefault();
	 var url="/Protocols/ProtocolsProductDetails?productID="+$(this).attr("value");
	 $.fn.CallPageRequest(url, "summary");
});
$("form").off("click", ".open-line-protocol").on("click", ".open-line-protocol", function (e) {
	 e.preventDefault();
	 var url="/Protocols/ProtocolsDetailsFloatModal?protocolID="+$(this).attr("value");
	 $.fn.CallPageRequest(url, "protocolFloatModal");
});