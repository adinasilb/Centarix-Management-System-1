$(".function").click(function (e) {
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

$(".saveFunction").click(function (e) {
    e.preventDefault();

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
    var formData = new FormData($(".addFunctionForm")[0]);
    var functionName = "AddFunctionModal";
    if ($("#masterPageType").val() == "ProtocolsReports") {
        functionName = "AddReportFunctionModal";
    }
    $.ajax({
        url: "/Protocols/" + functionName,
        traditional: true,
        data: formData,
        contentType: false,
        processData: false,
        type: "POST",
        success: function (data) {
            if ($("#masterPageType").val() == "ProtocolsReports") {
                $(".report-text").html(data);
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
