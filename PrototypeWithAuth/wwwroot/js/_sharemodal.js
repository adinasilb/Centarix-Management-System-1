$(function (e) {
	$(".share-object").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var menuEnum = $("#MenuItem").val();
		var url = "/" + menuEnum + "/ShareModal";
		var formData = new FormData($(".sharemodal")[0]);
		$.ajax({
			url: url,
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