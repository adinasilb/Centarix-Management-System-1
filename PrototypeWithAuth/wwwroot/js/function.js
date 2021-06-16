$("form").off("click", ".function").on("click", ".function", function (e) {
    e.preventDefault();
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

$(".remove-function").click(function (e) {
    e.preventDefault();
    console.log("remove file")
    url = $(this).attr("url");
    $.fn.CallPageRequest(url, "addFunction"); 
})

$(".add-function").off('click', ".saveFunction, .removeFunction").on('click',".saveFunction, .removeFunction",function (e) {
    e.preventDefault();
    var removing = $(this).hasClass("removeFunction");
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

                    var newDivText = newDiv.next().html();
                    if (newDivText == null && newDiv[0].nextSibling) {
                        console.log("no div")
                        newDivText = newDiv[0].nextSibling.textContent;
                    }
                    newDiv.html(newDivText)
                    newDiv[0].nextSibling?.remove();
                    newDiv.removeClass("added-div");
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

    //}
    //$('.materialForm').data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';
});
//var globalLine;
////$('.line').on('focusin', function(){
////    console.log("Saving value " + $(this).val());
//// $(this).attr('old-val', $(this).html());
////    return;
////});

//$(".line").keyup(function (event) {
  
//        var prev =$.trim($(this).attr('old-val'));
//        var current =$.trim(event.target.innerHTML);   
//        console.log("Prev value " + prev);
//        console.log("New value " +  current);
//    if(prev > current)
//    {
//        var firstIndex =prev.indexOf(current)
//        var lastIndex = firstIndex + current.length;
  
//        var diff = prev.substr(0, firstIndex);
//        diff+= prev.substr(lastIndex, prev.length-1)
//        if(diff.includes("</a>"))
//        {
//            event.target.innerHTML = $(this).attr('old-val');
//        }
//        $(this).attr('old-val', $(this).html());
//        console.log(diff);
//        return;
//    }
//    else{
//        var firstIndex =current.indexOf(current)
//        var lastIndex = firstIndex + prev.length;
  
//        var diff = current.substr(0, firstIndex);
//        diff+= current.substr(lastIndex, current.length-1)
//        if(diff.includes("</a>"))
//        {
//            event.target.innerHTML = $(this).attr('old-val');
//        }
//            $(this).attr('old-val', $(this).html());
//        console.log(diff);
//        return;

//     }
       
  
    
//});
