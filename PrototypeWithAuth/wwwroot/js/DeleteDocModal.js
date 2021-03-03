$(".delete-file-perm").on("click", function (e) {
	e.preventDefault();
	e.stopPropagation();
	var link = $('#submitDelete').attr("href");
	console.log("link: " + link)
	var formData = new FormData($(".DeleteDocumentModalForm")[0]);
	$foldername = $("#FolderName").val();
	$requestId = $("#RequestID").val();
	var $SectionType = $("#SectionType").val();
	alert($("#SectionType").val())
	var $isEdittable = $('#IsEdittable').val();
	var $documentModalType = $(".document-modal-type").val();
	console.log("mosaltype in delete: " + $documentModalType);
	console.log("$requestId: " + $requestId);
	$.ajax({
		url: link,
		method: 'POST',
		data: formData,
		success: (partialResult) => {
			$.fn.CloseModal("documents-delete");
			$.fn.OpenDocumentsModal($foldername, $requestId, true, $SectionType, $documentModalType);
			//$.fn.ChangeColorsOfDocs($foldername);
		},
		processData: false,
		contentType: false
	})
	return false;
});


$.fn.RemoveColorsOfDocs = function ($foldername) {
	$("#" + $foldername + " i").removeClass('oper-filter');
	$("#" + $foldername + " i").removeClass('order-inv-filter')
	$("#" + $foldername + " i").removeClass('lab-man-filter')
	$("#" + $foldername + " i").addClass('opac87');
	$("#" + $foldername+"Input").removeClass("contains-file");
	$("#" + $foldername+"Input").valid();
};

$.fn.OpenDocumentsModal = function (enumString, requestId, isEdittable, sectionType, modalType)  {
	//$(".modal-backdrop").first().removeClass();
	$.ajax({
		async: true,
		url: "/Requests/DocumentsModal?id=" + requestId + "&RequestFolderNameEnum=" + enumString + "&IsEdittable=" + isEdittable + "&SectionType=" + sectionType+"&ModalType="+modalType,
		type: 'GET',
		cache: true,
		success: function (data) {
			$.fn.OpenModal('documentsModal', 'documents', data)
			console.log("Here");
			var length = $('.iframe-container').length;
			if (length < 1) {
				$.fn.RemoveColorsOfDocs($foldername);
			}
		
		}
	});
};