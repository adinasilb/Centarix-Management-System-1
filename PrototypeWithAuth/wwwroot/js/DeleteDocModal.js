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
			$("#DeleteDocumentsModal").replaceWith('');
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
	$(".documentsModal").replaceWith('');
	//$(".modal-backdrop").first().removeClass();
	$.ajax({
		async: true,
		url: "/Requests/DocumentsModal?id=" + requestId + "&RequestFolderNameEnum=" + enumString + "&IsEdittable=" + isEdittable + "&SectionType=" + sectionType+"&ModalType="+modalType,
		type: 'GET',
		cache: true,
		success: function (data) {
			var modal = $(data);
			$('body').append(modal);
			$(".documentsModal").modal({
				backdrop: false,
				keyboard: true,
			});
			$(".documentsModal").modal('show');
			console.log("Here");
			var length = $('.iframe-container').length;
			if (length < 1) {
				$.fn.RemoveColorsOfDocs($foldername);
			}
		
		}
	});
};