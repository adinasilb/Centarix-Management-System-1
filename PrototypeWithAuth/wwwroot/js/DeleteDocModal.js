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

function ChangeColorsOfDocs($foldername) {
	console.log("foldername: " + $foldername);
	var numCards = $(".carousel-inner").length;
	console.log("numcards: " + numCards);
}

$.fn.ChangeColorsOfDocs = function ($foldername) {
	console.log("foldername: " + $foldername);
	var numCards = $(".card.document-border").length;
	console.log("numcards: " + numCards);

	var div = $("#" + $foldername + " img");
	console.log("div: " + div);
	//if (div.hasClass("order-inv-filter")) {
	//	console.log("has class already");
	//} else {
	//	console.log("does not class already");
	//	div.addClass("order-inv-filter");
	//}
};

$.fn.OpenDocumentsModal = function (enumString, requestId, isEdittable, sectionType)  {
	$("#documentsModal").replaceWith('');
	//$(".modal-backdrop").first().removeClass();
	$.ajax({
		async: true,
		url: "/Requests/DocumentsModal?id=" + requestId + "&RequestFolderNameEnum=" + enumString + "&IsEdittable=" + isEdittable + "&SectionType=" + sectionType,
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
			console.log("Here");
			ChangeColorsOfDocs($foldername);
		}
	});
};