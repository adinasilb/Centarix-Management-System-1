$(".delete-file-perm").on("click", function (e) {
	e.preventDefault();
	e.stopPropagation();
	if ($("#masterSidebarType").val() == "NoInvoice") {
		$(".invoice-image-name").text("")
	}
	var link = $('#submitDelete').attr("href");
	console.log("link: " + link)
	var formData = new FormData($(".DeleteDocumentModalForm")[0]);
	$foldername = $("#FolderName").val();
	$parentfoldername = $("#ParentFolderName").val();
	$objectId = $("#ObjectID").val();
	var $SectionType = $("#masterSectionType").val();
	
	var $isEdittable = $('.active-document-modal').attr("data-val");
	var $showSwitch = $('.active-document-modal').attr("showSwitch");
	var allowMultipleFiles = $("input.active-document-modal").attr("multiple-files");
	console.log("allowmultiple " + allowMultipleFiles);
	console.log("$requestId: " + $objectId);
	$.ajax({
		url: link,
		method: 'POST',
		data: formData,
		success: (partialResult) => {
			$.fn.CloseModal("documents-delete");
			var deletedReportFile = $(".report-file-card.delete-card");
			if (deletedReportFile.length > 0) {
				deletedReportFile.prev().remove();
				deletedReportFile.next().remove();
				deletedReportFile.remove();
				$(".report-text").trigger("change");
			}
			else {
				$.fn.OpenDocumentsModal($foldername, $objectId, $isEdittable, $SectionType, $showSwitch, $parentfoldername, allowMultipleFiles);
			}
			//$.fn.ChangeColorsOfDocs($foldername);
			$(".document-name").text('')
			$(".document-name#FileName").val('')
		},
		processData: false,
		contentType: false
	})
	return false;
});


$.fn.RemoveColorsOfDocs = function ($foldername) {
	console.log("in remove colors")
	$("#" + $foldername + " i").removeClass('oper-filter');
	$("#" + $foldername + " i").removeClass('order-inv-filter')
	$("#" + $foldername + " i").removeClass('lab-man-filter')
	$(".active-document-modal .material-image-icon").removeClass("protocols-filter");
	$(".active-document-modal .material-image-icon").addClass("disabled-text");
	$("#" + $foldername + " i").addClass('opac87');
	$("#" + $foldername+"Input").removeClass("contains-file");
	if ($("#" + $foldername+"Input").rules()) {
				$("#" + $foldername+"Input").valid();
		}

	$("#" + $foldername +" .document-border").removeClass("hasFile")
};
