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

$(".line").keydown(function (event) {

    if (window.getSelection && event.which == 8) { // backspace
        var selection = window.getSelection();
        console.log(selection);
        if (!selection.isCollapsed || !selection.rangeCount) {
            return;
        }
        console.log("contains node"+selection.containsNode('<a'))
        var curRange = selection.getRangeAt(selection.rangeCount - 1);
        console.log(curRange)
        if (curRange.commonAncestorContainer.nodeType == 3 && curRange.startOffset > 0) {
            // we are in child selection. The characters of the text node is being deleted
            return;
        }

        var range = document.createRange();
        console.log(selection.anchorNode)
        if (selection.anchorNode != this) {
            // selection is in character mode. expand it to the whole editable field
            range.selectNodeContents(this);
            range.setEndBefore(selection.anchorNode);
        } else if (selection.anchorOffset > 0) {
            range.setEnd(this, selection.anchorOffset);
        } else {
            // reached the beginning of editable field
            return;
        }
        range.setStart(this, range.endOffset - 1);


        var previousNode = range.cloneContents().lastChild;
        console.log(previousNode);
        if (previousNode && previousNode.contentEditable == 'false') {
            // this is some rich content, e.g. smile. We should help the user to delete it
          //  range.deleteContents();
            event.preventDefault();
        }
        }
    else if (window.getSelection && event.which == 46)
    {

    }
    });