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
	console.log("foldername: " + $foldername);
	console.log("$requestId: " + $requestId);
	$.ajax({
		url: link,
		method: 'POST',
		data: formData,
		success: (partialResult) => {
			$("#DeleteDocumentsModal").replaceWith('');
			$.fn.OpenDocumentsModal($foldername, $requestId, true, $SectionType);
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
};

$.fn.OpenDocumentsModal = function (enumString, requestId, isEdittable, sectionType)  {
	$(".documentsModal").replaceWith('');
	//$(".modal-backdrop").first().removeClass();
	$.ajax({
		async: true,
		url: "/Requests/DocumentsModal?id=" + requestId + "&RequestFolderNameEnum=" + enumString + "&IsEdittable=" + isEdittable + "&SectionType=" + sectionType,
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