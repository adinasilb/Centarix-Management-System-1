$(".upload-file").on("click", function (e) {
	e.preventDefault();
	e.stopPropagation();
	return false;
});


$(".file-select").on("change", function (e) {
	e.preventDefault();
	e.stopPropagation();
	console.log("upload file clicked");

	var inputButton = $('input[type="submit"]');

	//var $form = $(this).parents('form');
	console.log("in save doc files");
	//console.log("form: " + $form);
	//$(this).ajaxSubmit();
	//var url = $("#documentModalForm").data('string');
	var url = inputButton.attr("href");
	console.log("url : " + url);
	var formData = new FormData($("#documentModalForm")[0]);
	var data = $("#documentModalForm").serialize();
	//var formData = new FormData($(this));
	//console.log("data : " + data);
	console.log("formData : " + formData);
	//console.log("form data : " + formData);

	$.ajax({
		url: url,
		method: 'POST',
		data: formData,
		success: (partialResult) => {
			//this.options.noteModalElement.modal('hide');
			$(".carousel-item").remove();
			$("#documentsModal").replaceWith('');
			var $enumString = $(".open-document-modal").data("string");
			var $requestId = $(".open-document-modal").data("id");
			console.log("enumstring: " + $enumString + "    : requestid: " + $requestId);
			$.fn.OpenDocumentsModal($enumString, $requestId);
			$.fn.ChangeColorsOfModal();
			return false;
		},
		processData: false,
		contentType: false
	});
	return false;

});



$.fn.OpenDocumentsModal = function (enumString, requestId) {
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

$.fn.ChangeColorsOfModal = function () {

};

$(".delete-document").on("click", function (e) {
	e.preventDefault();
	e.stopPropagation();
	console.log("delete doc clicked");
	var link = $("#deleteUrl").attr("href");
	console.log("link: " + link);
	$.ajax({
		async: true,
		url: link,
		type: 'GET',
		cache: false,
		success: function (data) {
			var modal = $(data);
			$('body').append(modal);
			$("#DeleteDocumentsModal").modal({
				backdrop: false,
				keyboard: true,
			});
			$(".modal").modal('show');
		}
	});
	return false;
});


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