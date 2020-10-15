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
			console.log("has class operations");
			url = "/Operations/EditModalView";
		} else if ($('.turn-edit-on-off').hasClass('suppliers') || $('.turn-edit-on-off').hasClass('accounting')) {
			console.log("has class suppliers or accounting");
			url = "/Vendors/Edit";
		} else if ($('.turn-edit-on-off').hasClass('users')) {
			console.log("has class users");
			url = "/Admin/EditUser";

		} else if ($('.turn-edit-on-off').hasClass('orders')) {
			console.log("has class orders");
			url = "/Requests/EditModalView";

		}
		else {
			alert("didn't go into any edits");
		}
		console.log("url: " + url);

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

	$(".cancel-request-edits").off("click").on("click", function (e) {
		$(".confirm-edit-modal").remove();
		console.log("cancel request edits");
		$('.mark-readonly').attr("disabled", false);
		$('.mark-edditable').data("val", true);
		$('.edit-mode-switch-description').text("Edit Mode On");
		$('.turn-edit-on-off').attr('name', 'edit')
		if ($('.turn-edit-on-off').hasClass('operations') || $('.turn-edit-on-off').hasClass('orders')) {
			console.log("orders operations")
			$.fn.EnableMaterialSelect('#parentlist', 'select-options-parentlist')
			$.fn.EnableMaterialSelect('#sublist', 'select-options-sublist')
			$.fn.EnableMaterialSelect('#vendorList', 'select-options-vendorList')
			$.fn.EnableMaterialSelect('#currency', 'select-options-currency')
		}
		if ($('.turn-edit-on-off').hasClass('orders')) {
			console.log("orders")
			$.fn.EnableMaterialSelect('#Request_SubProject_ProjectID', 'select-options-Request_SubProject_ProjectID');
			$.fn.EnableMaterialSelect('#SubProject', 'select-options-SubProject');
			$.fn.EnableMaterialSelect('#Request_UnitTypeID', 'select-options-Request_UnitTypeID');
			$.fn.CheckUnitsFilled();
			$.fn.CheckSubUnitsFilled();
		}
		if ($('.turn-edit-on-off').hasClass('suppliers') || $('.turn-edit-on-off').hasClass('accounting')) {
			$.fn.EnableMaterialSelect('#VendorCategoryTypes', 'select-options-VendorCategoryTypes');
		}
		if ($(this).hasClass('users')) {
			$.fn.EnableMaterialSelect('#NewEmployee_JobCategoryTypeID', 'select-options-NewEmployee_JobCategoryTypeID');
			$.fn.EnableMaterialSelect('#NewEmployee_DegreeID', 'select-options-NewEmployee_DegreeID');
			$.fn.EnableMaterialSelect('#NewEmployee_MaritalStatusID', 'select-options-NewEmployee_MaritalStatusID');
			$.fn.EnableMaterialSelect('#NewEmployee_CitizenshipID', 'select-options-NewEmployee_CitizenshipID');
		}
		$(".turn-edit-on-off").click();
	});
});