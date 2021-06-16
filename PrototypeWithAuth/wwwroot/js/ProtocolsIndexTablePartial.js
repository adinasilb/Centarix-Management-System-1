
function ajaxPartialIndexTable(url, viewClass, type, formdata, modalClass = "") {

	var contentType = true;
	var processType = true;
	if (formdata == undefined) {
		console.log("formdata is undefined");
		formdata = {
			PageNumber: $('.page-number').val(),
			PageType: $('#masterPageType').val(),
			SectionType: $('#masterSectionType').val(),
			SidebarType: $('#masterSidebarType').val(),
			SidebarFilterID: $('.sideBarFilterID').val(),
		};
		console.log(formdata);
	}
	else {
		$.fn.CloseModal(modalClass);
		contentType = false;
		processType = false;
	}

	$.ajax({
		contentType: contentType,
		processData: processType,
		async: true,
		url: url,
		data: formdata,
		traditional: true,
		type: type,
		cache: false,
		success: function (data) {
			$(viewClass).html(data);
			$("#loading").hide();
			return true;
		}
	});

	return false;
}

$(".load-protocol").click(function(e){
	var val = $(this).val();
	$.ajax({
		url: "/Protocols/_IndexTableWithEditProtocol?protocolID="+val,
		async: true,
		type: "GET",
		success: function (data) {
			$("._IndexTable").html(data);
			$(".mdb-select").materialSelect();
			return true;
		}
	});


});