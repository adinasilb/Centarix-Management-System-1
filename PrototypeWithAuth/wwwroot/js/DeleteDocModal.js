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
	console.log($SectionType);
		var $isEdittable = $('.active-document-modal').attr("data-val");
		var $showSwitch =  $('.active-document-modal').attr("showSwitch");
	console.log("$requestId: " + $objectId);
	$.ajax({
		url: link,
		method: 'POST',
		data: formData,
		success: (partialResult) => {
			$.fn.CloseModal("documents-delete");
			$.fn.OpenDocumentsModal($foldername, $objectId, $isEdittable, $SectionType, $showSwitch, $parentfoldername);
			//$.fn.ChangeColorsOfDocs($foldername);	
			$(".document-name").text(fileName)
			$(".document-name#FileName").val(fileName)
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
