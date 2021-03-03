$(function () {
	$(".save-request-edits").on("click", function (e) {
		$.fn.CloseModal("confirm-edit");
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
		console.log(...formData)
		$.ajax({
			processData: false,
			contentType: false,
			data: formData,
			async: true,
			url: url,
			type: 'POST',
			cache: false,
			success: function (data) {
				$.fn.getMenuItems();
				//reload index pages
				if ($('.turn-edit-on-off').hasClass('operations')) {
					    ajaxPartialIndexTable($(".request-status-id").val(), "/Requests/_IndexTableData/", "._IndexTableData", "GET");
				}
				else if ($('.turn-edit-on-off').hasClass('suppliers') || $('.turn-edit-on-off').hasClass('accounting')) {

					$.ajax({
						async: true,
						url: '/Vendors/_IndexForPayment?SectionType=' + $('#SectionType').val(),
						type: 'GET',
						cache: true,
						success: function (data) {
							$('.indexTable').html(data);

						}
					});

				}
				else if ($('.turn-edit-on-off').hasClass('users')) {
					var url = "";
					var pageType = $('#PageType').val();
					if (pageType == "Workers") {
						url = "/ApplicationUsers/_Details"
					}
					else {
						url = "/Admin/_Index"
					}
					$.ajax({
						async: true,
						url: url,
						type: 'GET',
						cache: true,
						success: function (data) {
							$('#usersTable').html(data);
							alert("Updated CentarixID: " + $("#CentarixID").val());
							$("#OriginalStatusID").attr("CentarixID", $("#CentarixID").val());
						}
					});

				} else if ($('.turn-edit-on-off').hasClass('orders')) {
					    ajaxPartialIndexTable($(".request-status-id").val(), "/Requests/_IndexTableData/", "._IndexTableData", "GET");
				}
				//sets up error message if it has the setup in the view
				//if ($(".hasErrorMessage").length > 0) {
				//	alert("error message: " + $(".hasErrorMessage").val());
				//	$(".error-message").html($(".hasErrorMessage").val());
				//}
			},
			error: function (xhr) {
				$.fn.AppendModalToBody(xhr.responseText);
			}
		});
		$.fn.TurnToDetails();
	});


	$(".dont-save-request-edits").on("click", function (e) {
		$.fn.CloseModal("confirm-edit");
		console.log("don't save request edits");
		var selectedTab = $('.nav-tabs .active').parent().index() + 1;
		var url = '';
		var section = "";
		var id = $('.turn-edit-on-off').val();
		if ($('.turn-edit-on-off').hasClass('operations')) {
			console.log("has class operations");
			url = "/Operations/EditModalViewPartial?id=" + id + "&Tab=" + selectedTab;
		} else if ($('.turn-edit-on-off').hasClass('suppliers')) {
			section = "LabManagement";
			url = "/Vendors/EditPartial?id=" + id + "&SectionType=" + section + "&Tab=" + selectedTab;

		} else if ($('.turn-edit-on-off').hasClass('accounting')) {
			section = "Accounting";
			url = "/Vendors/EditPartial?id=" + id + "&SectionType=" + section + "&Tab=" + selectedTab;

		}
		else if ($('.turn-edit-on-off').hasClass('users')) {
			alert("in users");
			url = "/Admin/EditUserPartial?id=" + id + "&Tab=" + selectedTab;

		} else if ($('.turn-edit-on-off').hasClass('orders')) {
			selectedTab = $('.tab-content').children('.active').attr("value");
			console.log(selectedTab)
			section = $("#masterSectionType").val();
			url = "/Requests/ItemData?id=" + id + "&Tab=" + selectedTab + "&SectionType=" + section;

		}
		else {
			alert("didn't go into any edits");
		}
		console.log("url: " + url);

		$.ajax({
			url: url,
			type: 'GET',
			cache: true,
			success: function (data) {
				console.log("cancel edit successful!")
				//open the confirm edit modal
				$('.partial-div').html(data);
				$('.name').val($('.old-name').val())
				if ($('.turn-edit-on-off').hasClass('orders')) {
					$.fn.LoadEditModalDetails();
				}
			}
		});
	});

	
	$(".cancel-request-edits").off("click").on("click", function (e) {
		$.fn.CloseModal("confirm-edit");
		console.log("cancel request edits");
		$('.mark-readonly').attr("disabled", false);
		$('.mark-edditable').data("val", true);
		$('.edit-mode-switch-description').text("Edit Mode On");
		$('.turn-edit-on-off').attr('name', 'edit')
		if ($('.turn-edit-on-off').hasClass('operations') || $('.turn-edit-on-off').hasClass('orders')) {
			console.log("orders operations")
			//$.fn.EnableMaterialSelect('#parentlist', 'select-options-parentlist')
			//$.fn.EnableMaterialSelect('#sublist', 'select-options-sublist')
			$.fn.EnableMaterialSelect('#vendorList', 'select-options-vendorList')
			$.fn.EnableMaterialSelect('#currency', 'select-options-currency')
		}
		if ($('.turn-edit-on-off').hasClass('orders')) {
			console.log("orders")
			$.fn.EnableMaterialSelect('#Request_SubProject_ProjectID', 'select-options-Request_SubProject_ProjectID');
			$.fn.EnableMaterialSelect('#SubProject', 'select-options-SubProject');
			$.fn.EnableMaterialSelect('#unitTypeID', 'select-options-unitTypeID');
			$.fn.CheckUnitsFilled();
			$.fn.CheckSubUnitsFilled();
		}
		if ($('.turn-edit-on-off').hasClass('suppliers') || $('.turn-edit-on-off').hasClass('accounting')) {
			$.fn.EnableMaterialSelect('#VendorCategoryTypes', 'select-options-VendorCategoryTypes');
		}
		if ($(this).hasClass('users')) {
			$.fn.EnableMaterialSelect('#NewEmployee_JobSubcategoryType_JobCategoryTypeID', 'select-options-NewEmployee_JobSubcategoryType_JobCategoryTypeID');
			$.fn.EnableMaterialSelect('#NewEmployee_DegreeID', 'select-options-NewEmployee_DegreeID');
			$.fn.EnableMaterialSelect('#NewEmployee_MaritalStatusID', 'select-options-NewEmployee_MaritalStatusID');
			$.fn.EnableMaterialSelect('#NewEmployee_CitizenshipID', 'select-options-NewEmployee_CitizenshipID');
			$.fn.EnableMaterialSelect('#NewEmployee_JobSubcategoryTypeID', 'select-options-NewEmployee_JobSubcategoryTypeID');
		}
		$('.turn-edit-on-off').prop('checked', true);
		$('.open-document-modal').attr("data-val", true);
	});

	$(".exit-edit-modal").off("click").on("click", function (e) {
		var url = $(this).attr("value");
		var itemurl = "/Requests/ConfirmExit/";
		console.log(itemurl);
		var formData = {
			SectionType : $('#masterSectionType').val(),
			PageType : $('#masterPageType').val(),
			URL : url
		}
		console.log(formData);
		$.ajax({
			contentType: false,
			processData: true,
			async: true,
			url: $itemurl,
			type: 'POST',
			data: formData,
			cache: true,
			success: function (data) {
				$("#loading").hide();
				if(url != "")
				{
					location.href = url;
				}

				$.fn.CloseModal("confirm-exit");
				$.fn.CloseModal("edit-item");
				//$('.confirm-exit-modal').remove();
				//$(".modal").modal('hide');
				//$(".modal").replaceWith('');
				$(".save-item").removeClass("save-item");
			}
		});
	})

	$(".return-edit-modal").off("click").on("click", function (e) {
		$.fn.CloseModal("confirm-exit");
		//$(".modal").attr("data-backdrop", true);
	})
});