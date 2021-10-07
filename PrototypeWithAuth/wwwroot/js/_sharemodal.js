$(function (e) {
	$(".share-object").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		$(this).prop('disabled', true)
		var menuEnum = $("#MenuItem").val();
		var url = "/" + menuEnum + "/ShareModal";
		var formData = new FormData($(".sharemodal")[0]);
		$.ajax({
			url: url,
			method: 'POST',
			data: formData,
			success: (e) => {
				$(this).prop('disabled', false)
				if (!e) {
					$.fn.CloseModal("share-modal");
				}
				//find error here!
			},
			processData: false,
			contentType: false
		})
	});
	$("#select-options-ApplicationUserIDs li.disabled").addClass("hidden");
});