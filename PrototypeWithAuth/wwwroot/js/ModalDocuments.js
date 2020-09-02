//$(".open-document-modal").on("click", function (e) {
//	console.log("in open doc modal click");
//	e.preventDefault();
//	e.stopPropagation();
//	$(".open-document-modal").removeClass("active-document-modal");
//	$(this).addClass("active-document-modal");
//	var enumString = $(this).data("string");
//	console.log("EnumString: " + enumString);
//	var requestId = $(this).data("id");
//	var isEdittable = $(this).data("val");
//	console.log("isEdittable: " + isEdittable);
//	$.fn.OpenDocumentsModal(enumString, requestId, isEdittable);
//	return false;
//});

$.fn.OpenDocumentsModal = function (enumString, requestId, isEdittable) {
	console.log("in open doc modal");
	$("#documentsModal").replaceWith('');
	var urltogo = $("#documentSubmit").attr("url");
	//var urlToGo = "DocumentsModal?id=" + requestId + "&RequestFolderNameEnum=" + enumString + "&IsEdittable=" + isEdittable;*/
	console.log("urltogo: " + urltogo);
	urltogo = urltogo + "?id=" + requestId + "&RequestFolderNameEnum=" + enumString + "&IsEdittable=" + isEdittable
	//$(".modal-backdrop").first().removeClass();
	$.ajax({
		async: true,
		url: urltogo,
		type: 'GET',
		cache: false,
		success: function (data) {
			var modal = $(data);
			$('body').append(modal);
			$("#documentsModal").modal({
				backdrop: false,
				keyboard: true,
			});
			$(".modal").modal('show');
		}
	});
};