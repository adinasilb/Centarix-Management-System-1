$(".function").click(function (e) {
	e.preventDefault();

	var lineID =$(".containsLineID").attr("data-val");
		alert(lineID)
	if(lineID != undefined)
		{
		var url = "";
	 if ($("#masterPageType").val() == "ProtocolsReports") {
		url = "/Protocols/AddReportFunctionModal?FunctionTypeID=" + $(this).val() + "&ReportID=" + $("#ReportID").val();
    }
	/*if ($("#masterPageType").val() == "ProtocolsCreate")*/ else {
		url = "/Protocols/AddFunctionModal?FunctionTypeID=" + $(this).attr("typeID") + "&LineID=" + lineID+"&functionLineID="+$(this).attr("value");
	}
	$.fn.CallPageRequest( url , "addFunction");
		}
	
});
