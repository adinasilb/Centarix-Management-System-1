$(function () {

	$(".upload-file").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		return false;
	});

	$(".file-select").on("change", function (e) {
		e.preventDefault();
		e.stopPropagation();
		console.log("upload file submitted");

		var inputButton = $('#save-documents');

		//var $form = $(this).parents('form');
		console.log("in save doc files");
		//console.log("form: " + $form);
		//$(this).ajaxSubmit();
		//var url = $("#documentModalForm").data('string');
		console.log("input button: " + inputButton);
		var url = inputButton.attr("href");
		var $isEdittable = $('#IsEdittable').val();
		console.log("url : " + url);
		var formData = new FormData($("#documentModalForm")[0]);
		var data = $("#documentModalForm").serialize();
		//var formData = new FormData($(this));
		//console.log("data : " + data);
		console.log("formData : " + formData);
		//console.log("data : " + model);


		$.ajax({
			url: url,
			method: 'POST',
			data: formData,
			success: (partialResult) => {
				//this.options.noteModalElement.modal('hide');
				$(".carousel-item").remove();
				$("#documentsModal").replaceWith('');

				var $enumString = $(".open-document-modal.active-document-modal").data("string");
				var $requestId = $(".open-document-modal.active-document-modal").data("id");

				console.log("enumstring: " + $enumString + "    : requestid: " + $requestId + "isedditable" + $isEdittable);
				var isOperations = $(".open-document-modal.active-document-modal").hasClass('operations')
				$.fn.ChangeColorsOfModal($enumString, isOperations);
				$.fn.OpenDocumentsModal($enumString, $requestId, $isEdittable, isOperations);
				return false;
			},
			processData: false,
			contentType: false
		});
		return false;

	});



	$.fn.OpenDocumentsModal = function (enumString, requestId, isEdittable, isOperations) {
		$("#documentsModal").replaceWith('');
		//$(".modal-backdrop").first().removeClass();
		$.ajax({
			async: true,
			url: "/Requests/DocumentsModal?id=" + requestId + "&RequestFolderNameEnum=" + enumString + "&IsEdittable=" + isEdittable + "&IsOperations=" + isOperations,
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

	$.fn.ChangeColorsOfModal = function ($foldername, isOperations) {
		console.log("foldername: " + $foldername);
		var numCards = $(".card.document-border").length;
		console.log("numcards: " + numCards);

		var div = $("#" + $foldername + " img");
		console.log("div: " + div);
		if (div.hasClass("order-inv-filter") || div.hasClass("oper-filter")) {
			console.log("has class already");
		} else {
			console.log("does not class already");
			if (isOperations) {
				div.addClass("oper-filter");
			} else {
				div.addClass("order-inv-filter");
			}

		}
	};

	$("#delete-file-document").on("click", function (e) {
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
				var modal = data;
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

	$(".modal").on("click", ".turn-edit-doc-on-off", function () {
		alert("djs modal turned on or off!");
	});
	$(".turn-edit-doc-on-off").off("click").on("click", function () {
		alert("djs turned on or off!");
	});
	$(".modal").on("change", ".turn-edit-doc-on-off", function () {
		alert("djs modal turned on or off!");
	});
	$(".turn-edit-doc-on-off").off("change").on("change", function () {
		alert("djs turned on or off!");
	});

	function ChangeEdits() {
		alert("djs change edits");
	};

});