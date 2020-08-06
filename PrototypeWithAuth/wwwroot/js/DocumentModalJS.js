//$(".upload-file").on("click", function (e) {
//	e.preventDefault();
//	e.stopPropagation();
//	return false;
//});


$(".save-document-files").on("click", function (e) {
	e.preventDefault();
	e.stopPropagation();
	//var $form = $(this).parents('form');
	console.log("in save doc files");
	//console.log("form: " + $form);
	//$(this).ajaxSubmit();
	//var url = $("#documentModalForm").data('string');
	var url = $(this).attr("href");
	console.log("url : " + url);
	var formData = new FormData($("#documentModalForm")[0]);
	var data = $("#documentModalForm").serialize();
	//var formData = new FormData($(this));
	console.log("data : " + data);
	console.log("formData : " + formData);
	//console.log("form data : " + formData);

	$.ajax({
		url: url,
		method: 'POST',
		data: formData,
		success: (partialResult) => {
			//this.options.noteModalElement.modal('hide');
			$(".documentsModal .modal-body").empty();
			$("#documentsModal").replaceWith('');
			var $enumString = $(".open-document-modal").data("string");
			var $requestId = $(".open-document-modal").data("id");
			console.log("enumstring: " + $enumString + "    : requestid: " + $requestId);
			$.fn.OpenDocumentsModal($enumString, $requestId);
			return false;
		},

		processData: false,
		contentType: false
	});
	return false;

	//$("#documentModalForm").submit(function () {

	//var formData = new FormData($(this)[0]);

	//$.ajax({
	//	url: "Requests/SaveDocumentFiles",
	//	type: 'POST',
	//	data: formData,
	//	async: false,
	//	success: function (data) {
	//		alert(data)
	//	},
	//	cache: false,
	//	contentType: false,
	//	processData: false
	//});

	return false;
	//});
	//$.ajax({
	//	type: "POST",
	//	url: $form.attr('action'),
	//	data: $form.serialize(),
	//	error: function (xhr, status, error) {
	//		console.log("---error---");
	//		console.log("xhr " + xhr);
	//		console.log("status " + status);
	//		console.log("error " + error);
	//	},
	//	success: function (response) {
	//		console.log("---success---");
	//		console.log("response: " + response);
	//	}
	//});
	//$("#documentModalForm").submit();
	//var filetype = $(this).data("string");
	//console.log("filetype: " + filetype);
	//var files = $(".file-select").val();
	//console.log("files: " + files);
	//console.log("files.filetype : " + files.filetype);
	//console.log("files.files : " + files.files);
	//var $url = $(this).attr("href");
	//console.log("url: " + $url);
	//$.ajax({
	//	async: false,
	//	type: 'POST',
	//	url: $url,
	//	cache: true,
	//	success: function (data) {
	//		console.log("success!");
	//	}
	//});
	return false;
});

$.fn.OpenDocumentsModal = function (enumString, requestId) {
	console.log("in open doc modal");
	$("#documentsModal").replaceWith('');
	//$(".modal-backdrop").first().removeClass();
	$.ajax({
		async: true,
		url: "Requests/DocumentsModal?id=" + requestId + "&RequestFolderNameEnum=" + enumString,
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


//MDBootstrap Carousel
$('.carousel.carousel-multi-item.v-2 .carousel-item').each(function () {
	var next = $(this).next();
	if (!next.length) {
		next = $(this).siblings(':first');
	}
	next.children(':first-child').clone().appendTo($(this));

	for (var i = 0; i < 4; i++) {
		next = next.next();
		if (!next.length) {
			next = $(this).siblings(':first');
		}
		next.children(':first-child').clone().appendTo($(this));
	}
});