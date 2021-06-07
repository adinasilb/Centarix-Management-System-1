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
