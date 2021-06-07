$(".function").click(function (e) {
	e.preventDefault();
	var url = "";
	 if ($("#masterPageType").val() == "ProtocolsReports") {
		url = "/Protocols/AddReportFunctionModal?FunctionTypeID=" + $(this).val() + "&ReportID=" + $("#ReportID").val();
    }
	/*if ($("#masterPageType").val() == "ProtocolsCreate")*/ else {
		url = "/Protocols/AddFunctionModal?FunctionTypeID=" + $(this).val() + "&LineID=" + $(".focused-line").attr("data-val")+"&functionLineID"+$(this).attr("data-val");
	}
	$.fn.CallPageRequest( url , "addFunction");
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
    $.ajax({
        url: "/Protocols/AddFunctionModal",
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
