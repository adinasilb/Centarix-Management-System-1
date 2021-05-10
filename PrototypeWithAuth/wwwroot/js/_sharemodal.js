$(function (e) {
	$(".share-object").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var modelsEnum = $("#ModelsEnum").val();
		var url = "/" + modelsEnum + "/ShareModal";
		var formData = new FormData($(".sharemodal")[0]);
		$.ajax({
			url: link,
			method: 'POST',
			data: formData,
			success: (partialResult) => {
				$.fn.CloseModal("shared-modal");
				//find error here!
			},
			processData: false,
			contentType: false
		})
	});
});