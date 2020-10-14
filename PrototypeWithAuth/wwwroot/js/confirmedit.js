$(function () {
	$(".save-request-edits").on("click", function (e) {
		$(".confirm-edit-modal").remove();
		console.log("save request edits");
		var formData = new FormData($("#myForm")[0]);
		$("#myForm").data("validator").settings.ignore = "";
		var valid = $("#myForm").valid();
		console.log("valid form: " + valid)
		if (!valid) {
			$("#myForm").data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';
			$('.turn-edit-on-off').prop('checked', true);
			return false;
		}

		var url = '';
		if ($('.turn-edit-on-off').hasClass('operations')) {
			url = "/Operations/EditModalView";
		} else if ($('.turn-edit-on-off').hasClass('suppliers') || $(this).hasClass('accounting')) {
			url = "/Vendors/Edit";
		} else {
			url = "/Requests/EditModalView";

		}

		$.ajax({
			processData: false,
			contentType: false,
			data: formData,
			async: true,
			url: url,
			type: 'POST',
			cache: true,
			success: function (data) {
				alert("save edit successful!")
				//open the confirm edit modal

			}
		});
		$.fn.TurnToDetails();
	});

	$(".dont-save-request-edits").on("click", function (e) {
		$(".confirm-edit-modal").remove();
		console.log("don't save request edits");
		$("body").remove(".confirm-edit-modal");
		$.fn.TurnToDetails();
	});

	$.fn.TurnToDetails = function () {
		console.log("after ajax call");
		$('.mark-readonly').prop("disabled", true);
		$('.mark-readonly input').prop("disabled", true);
		$('.mark-edditable').data("val", false)
		$('.edit-mode-switch-description').text("Edit Mode Off");
		$('.turn-edit-on-off').attr('name', 'details')
	};
});