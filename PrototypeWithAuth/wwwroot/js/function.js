$(".function").click(function (e) {
	e.preventDefault();
	var url = ""
	if ($("#masterPageType").val() == "ProtocolsCreate") {
		url = "/Protocols/AddFunctionModal?FunctionTypeID=" + $(this).val() + "&LineID=" + $(".focused-line").attr("data-val");
	}
	else if ($("#masterPageType").val() == "ProtocolsReports") {
		url = "/Protocols/AddReportFunctionModal?FunctionTypeID=" + $(this).val() + "&ReportID=" + $("#ReportID").val();
    }
	$.fn.CallPageRequest( url , "addFunction");
});
