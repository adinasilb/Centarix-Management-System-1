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
		var filePath = $(".file-select")[0].value;
		var extn = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
		console.log("extn: " + extn);
		if (extn != "pdf" && extn != "png" && extn != "jpg" && extn != "jpeg" && extn != "docx" && extn != "doc" && extn !="") {
			alert("invalid file extension");
			return;
		}

		//var $form = $(this).parents('form');
		console.log("in save doc files");
		//console.log("form: " + $form);
		//$(this).ajaxSubmit();
		//var url = $("#documentModalForm").data('string');
		console.log("input button: " + inputButton);
		var url = inputButton.attr("href");
		var $isEdittable = $('.active-document-modal.withinDocModal').data("val");
		//alert($isEdittable)
		var $showSwitch =  $('.active-document-modal.withinDocModal').attr("showSwitch");
		console.log("url : " + url);
		var formData = new FormData($(".documentModalForm")[0]);
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
				var section = $("#masterSectionType").val();
				
				if ($(".open-document-modal.active-document-modal").hasClass('operations') || $(".open-document-modal").hasClass('Operations')) {
					section = "Operations"
				} else if ($(".open-document-modal.active-document-modal").hasClass('labManagement')|| $(".open-document-modal.active-document-modal").hasClass('LabManagement')) {
					section = "LabManagement"
				}
				$.fn.ChangeColorsOfModal($enumString, section);
				$.fn.OpenDocumentsModal($enumString, $requestId, $isEdittable, section, $showSwitch);
				return true;
			},
			processData: false,
			contentType: false
		});
		return true;

	});

	
	//seems to not be used. only diff is that other one has $(".open-document-modal").removeClass("active-document-modal");
	$(".open-document-modal").off('click').on("click", function (e) {

		e.preventDefault();
		e.stopPropagation();
		console.log("clicked open doc modal 1");
		var section = $("#masterSectionType").val();
		console.log("section"+section)
		$(this).addClass("active-document-modal");
		var enumString = $(this).data("string");
		console.log("enumString: " + enumString);
		var requestId = $(this).data("id");
		console.log("requestId: " + requestId);
		var isEdittable = $(".active-document-modal").attr("data-val");
		console.log($("#masterSidebarType").val())
		var showSwitch = $(".active-document-modal").attr("showSwitch");
		$.fn.OpenDocumentsModal(enumString, requestId, isEdittable, section, showSwitch);
		return true;
	});


	$(".file-select").on("change", function (e) {
		console.log("file was changed");
		$cardDiv = $(this).closest("div.card");
		console.log("cardDiv: " + JSON.stringify($cardDiv));
		$cardDiv.addClass("document-border");
		return true;
	});


	$.fn.ChangeColorsOfModal = function ($foldername, section) {
		//alert("section: " + section)
		console.log("foldername: " + $foldername);
		var numCards = $(".card.document-border").length;
		console.log("numcards: " + numCards);
		var folder = "#" + $foldername + ".active-document-modal";
		var div = $(folder + " i");
		
		if (div.hasClass("order-inv-filter") || div.hasClass("oper-filter") || div.hasClass("lab-man-filter") || div.hasClass("contains-file")) {
			console.log("has class already");
		} else {
			console.log("does not class already");
			$(folder +".active-document-modal" + " div.card.document-border").addClass("hasFile");
			if (section=="Operations") {
				div.addClass("oper-filter");
			} else if ((section == "LabManagement")) {
				div.addClass("lab-man-filter");
			}
			else {
				div.addClass("order-inv-filter");
			}
			var folderInput = "#" + $foldername + "Input";
			$(folderInput).addClass("contains-file");
			if ($(folderInput).rules()) {
					$(folderInput).valid();
			}
		}
	};

	$(".modal").on("click",".delete-document", function (e) {
		e.preventDefault();
		var hasClass = $(this).hasClass("delete-file-document");
		if (hasClass ==true) {
			console.log("delete doc clicked");
			var link = $(this).attr("url");
			console.log("link: " + link);
			$.ajax({
				async: true,
				url: link,
				type: 'GET',
				cache: false,
				success: function (data) {
					$.fn.OpenModal('modal-document-delete', "documents-delete", data)
					
				}
			});
		}
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

	//$(".modal").on("click", ".turn-edit-doc-on-off", function () {
	//	alert("djs modal turned on or off!");
	//});
	$(".modal .turn-edit-doc-on-off").off("click").on("click", function () {
		//alert('.turneditdoc on and off')
		var detailsBool = $(".isEdittable").hasClass("details");
		var editBool = $(".isEdittable").hasClass("edit");


		var $bcColor = $("#bcColor").attr("class");
		var $color = $("#color").attr("class");


		//alert("classes: " + detailsBool + " ; " + editBool);
		if (detailsBool) {
			$(".isEdittable").removeClass("details");
			$(".isEdittable").addClass("edit");

			$(".document-modal-buttons").removeClass("disabled-color");
			$(".document-modal-buttons").addClass($bcColor);
			$(".file-select").attr("disabled", false);
			$(".isEdittable").val(true);
			$(".document-modal-cancel").addClass("d-none");
			$(".document-modal-save").removeClass("d-none");
			$(".documents-delete-icon.icon-delete-24px").removeClass("disabled-filter");
			$(".documents-delete-icon.icon-delete-24px").addClass($color);
		    $(".view-img i").removeClass("disabled-filter");
			$(".view-img i").addClass($color);
			$(".active-document-modal.withinDocModal").attr("data-val", true);
			$(".delete-document").addClass("delete-file-document");
			$(this).prev('.edit-mode-switch-description').text("Edit Mode On");
		}
		else if (editBool) {

			$(".isEdittable").addClass("details");
			$(".isEdittable").removeClass("edit");
			$(".isEdittable").val(false);
			$(".document-modal-buttons").addClass("disabled-color");
			$(".document-modal-buttons").removeClass($bcColor);
			$(".file-select").attr("disabled", true);
			$(".document-modal-cancel").removeClass("d-none");
			$(".document-modal-save").addClass("d-none");
			$(".active-document-modal.withinDocModal").attr("data-val", false);
			$(".documents-delete-icon.icon-delete-24px").addClass("disabled-filter");
			$(".documents-delete-icon.icon-delete-24px").removeClass($color);
			$("i.view-img").addClass("disabled-filter");
			$("i.view-img").removeClass($color);
			$(".delete-document").removeClass("delete-file-document");
			$(this).prev('.edit-mode-switch-description').text("Edit Mode Off");
		}
	});
	//$(".modal").on("change", ".turn-edit-doc-on-off", function () {
	//	alert("djs modal turned on or off!");
	//});
	//$(".turn-edit-doc-on-off").off("change").on("change", function () {
	//	alert("djs turned on or off!");
	//});

	//function ChangeEdits() {
	//	alert("djs change edits");
	//};

});