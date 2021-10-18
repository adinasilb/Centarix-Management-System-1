$(function () {
	$('.chartForm').validate({
		rules: {
			"SelectedYears": { selectRequired: true },
			"SelectedMonths": {
				selectRequired: true,
			},
			"Currency": {
				selectRequired: true,
			},
		}
	});

	$("#createPieChart").click(function (e) {
		//alert("validate form");
		e.preventDefault();
		$.fn.getChart("/Expenses/_PieChart");
		
	});
	$(".chart-dropdownlists").on("click", "#createGraphChart", function (e) {
		e.preventDefault();
		$.fn.getChart("/Expenses/_GraphChart");
	
	});
	
	$.fn.getChart = function (url) {
		$(".chartForm").data("validator").settings.ignore = "";
		var valid = $(".chartForm").valid();

		if (!valid) {
			$(".chartForm").data("validator").settings.ignore = ':not(select:hidden, .location-error:hidden,input:visible, textarea:visible)';
			if (!$('.activeSubmit').hasClass('disabled-submit')) {
				$('.activeSubmit').addClass('disabled-submit')
			}
			return false;
		}
		else {
			$('.activeSubmit').removeClass('disabled-submit')
			var formData = new FormData($(".chartForm")[0]);
			$.ajax({
				url: url,
				method: 'POST',
				data: formData,

				success: function (data) {

					$('.chartDiv').html(data);
				},
				processData: false,
				contentType: false
			});
			return true;
		}
	}
	$.fn.filterByCategoryType = function () {

		var selectedCategoryTypes = $("#SelectedCategoryTypes").map(function (i, el) {
			if ($(el).val() != '') {
				return $(el).val();
			}
		}).get();
		if (selectedCategoryTypes == "") {
			$.fn.reloadDDLS();
		}
		else {
			$.fn.disableNotCategoryDDLS();
		}
		var url = "/ProductSubcategories/FilterByCategoryType";
		$.ajax({
			async: true,
			url: url,
			data: { SelectedCategoryTypes: selectedCategoryTypes },
			traditional: true,
			type: 'GET',
			cache: false,
			success: function (data) {
				$("#parentlistMulitple").destroyMaterialSelect()				
				$("#parentlistMulitple").empty();
				$.each(data.parentCategories, function (i, category) {
					$("#parentlistMulitple").append('<option value="' + category.parentCategoryID + '">' + category.parentCategoryDescription + '</option>');
				});				
				$("#parentlistMulitple").after('<button type="button" onclick="$.fn.filterByParentCategoryType();" class="btn-save  btn text-white expenses-background-color rounded-pill no-box-shadow  ">Save</button>');			
				$("#parentlistMulitple").materialSelect();
				$("input[data-activates='select-options-parentlistMulitple']").attr('placeholder', "Select Category")
				$("input[data-activates='select-options-parentlistMulitple']").val('')				
				var dropdownParentlistMulitple = $("#select-options-parentlistMulitple input.form-check-input");
				dropdownParentlistMulitple.addClass("filled-in");
				$.fn.SetupSubCategoriesDDL(data);
				$.fn.SetupVendorsDDL(data);
				$.fn.SetupWorkersDDL(data);		
			
				//dropdown.addClass("fci-exp");
				$(".multiple-select-dropdown li span").replaceWith(function () {
					return "<div class='form-check pl-0 py-2'>" + this.innerHTML + "</div>";
				});
				return false;
			}
		});

	}
	$.fn.filterByParentCategoryType = function () {
		var parentCategoryIds = $("#parentlistMulitple").map(function (i, el) {
			if ($(el).val() != '') {
				return $(el).val();
			}
		}).get();
		if (parentCategoryIds == "") {
			$.fn.reloadDDLS();
		}
		else {
			$.fn.disableNotCategoryDDLS();
		}
		var url = "/ProductSubcategories/FilterByParentCategories";
		$.ajax({
			async: true,
			url: url,
			data: { ParentCategoryIds: parentCategoryIds },
			traditional: true,
			type: 'GET',
			cache: false,
			success: function (data) {
				$.fn.SetupSubCategoriesDDL(data);
				$.fn.SetupVendorsDDL(data);
				$.fn.SetupWorkersDDL(data);		
				$(".multiple-select-dropdown li span").replaceWith(function () {
					return "<div class='form-check pl-0 py-2'>" + this.innerHTML + "</div>";
				});
				return false;
			}
		});
	}
	$.fn.filterBySubCategoryType = function () {
		var subCategoryIds = $("#sublist").map(function (i, el) {
			if ($(el).val() != '') {
				return $(el).val();
			}
		}).get();
		if (subCategoryIds == "") {
			$.fn.reloadDDLS();
		} else {
			$.fn.disableNotCategoryDDLS();
		}
		var url = "/ProductSubcategories/FilterBySubCategories";
		$.ajax({
			async: true,
			url: url,
			data: { SubCategoryIds: subCategoryIds },
			traditional: true,
			type: 'GET',
			cache: false,
			success: function (data) {
				$.fn.SetupVendorsDDL(data);
				$.fn.SetupWorkersDDL(data);
				$(".multiple-select-dropdown li span").replaceWith(function () {
					return "<div class='form-check pl-0 py-2'>" + this.innerHTML + "</div>";
				});
				return false;
			}
		});
	}
	$.fn.filterByProject = function () {
	
		var projectIds = $("#SelectedProjects").map(function (i, el) {
			if ($(el).val() != '') {
				return $(el).val();
			}
		}).get();
		if (projectIds == "") {
			$.fn.reloadDDLS();
		} else {
			$.fn.disableNotProjectDDLS();
		}
		var url = "/Requests/FilterByProjects";
		$.ajax({
			async: true,
			url: url,
			data: { ProjectIDs: projectIds },
			traditional: true,
			type: 'GET',
			cache: false,
			success: function (data) {
				$("#SelectedSubProjects").destroyMaterialSelect()
				$("#SelectedSubProjects").empty();
				$.each(data.subProjects, function (i, subproject) {
					item = '<option value="' + subproject.subProjectID + '">' + subproject.subProjectDescription + '</option>'
					$("#SelectedSubProjects").append(item);
				});
				$("#SelectedSubProjects").after('<button type="button" onclick="$.fn.filterBySubProject();" class="btn-save  btn text-white expenses-background-color rounded-pill no-box-shadow  ">Save</button>');		
				$("#SelectedSubProjects").materialSelect();
				$("input[data-activates='select-options-SelectedSubProjects']").attr('placeholder', "Select Sub Project")
				$("input[data-activates='select-options-SelectedSubProjects']").val('')
				var dropdown = $("#select-options-SelectedSubProjects input.form-check-input");
				dropdown.addClass("filled-in");

				$.fn.SetupVendorsDDL(data);

				$(".multiple-select-dropdown li span").replaceWith(function () {
					return "<div class='form-check pl-0 py-2'>" + this.innerHTML + "</div>";
				});
				return false;
			}
		});
	}
	$.fn.filterBySubProject = function () {

		var subProjectIDs = $("#SelectedSubProjects").map(function (i, el) {
			if ($(el).val() != '') {
				return $(el).val();
			}
		}).get();
		if (subProjectIDs == "") {
			$.fn.reloadDDLS();
		}
		else {
			$.fn.disableNotProjectDDLS();
		}
		var url = "/Requests/FilterBySubProjects";
		$.ajax({
			async: true,
			url: url,
			data: { SubProjectIDs: subProjectIDs },
			traditional: true,
			type: 'GET',
			cache: false,
			success: function (data) {
				
				$.fn.SetupWorkersDDL(data);

				$(".multiple-select-dropdown li span").replaceWith(function () {
					return "<div class='form-check pl-0 py-2'>" + this.innerHTML + "</div>";
				});
				return false;
			}
		});
	}

	$.fn.SetupWorkersDDL = function (data) {
		$("#SelectedEmployees").destroyMaterialSelect()
		$("#SelectedEmployees").empty();
		$.each(data.employees, function (i, worker) {
			$("#SelectedEmployees").append('<option value="' + worker.workerID + '">' + worker.workerName + '</option>');
		});
		$("#SelectedEmployees").materialSelect();
		$("input[data-activates='select-options-SelectedEmployees']").attr('placeholder', "Select Worker")
		$("input[data-activates='select-options-SelectedEmployees']").val('')
		var dropdownSelectedEmployees = $("#select-options-SelectedEmployees input.form-check-input");
		dropdownSelectedEmployees.addClass("filled-in");
	}
	$.fn.SetupVendorsDDL = function (data) {
		$("#SelectedVendors").destroyMaterialSelect()
		$("#SelectedVendors").empty();
		$.each(data.vendors, function (i, vendor) {
			$("#SelectedVendors").append('<option value="' + vendor.vendorID + '">' + vendor.vendorName + '</option>');
		});
		$("#SelectedVendors").materialSelect();
		$("input[data-activates='select-options-SelectedVendors']").attr('placeholder', "Select Vendor")
		$("input[data-activates='select-options-SelectedVendors']").val('')
		var dropdownSelectedVendors = $("#select-options-SelectedVendors input.form-check-input");
		dropdownSelectedVendors.addClass("filled-in");
	}
	$.fn.SetupSubCategoriesDDL = function (data) {
		$("#sublist").destroyMaterialSelect()
		$("#sublist").empty();
		$.each(data.productSubcategories, function (i, subCategory) {
			$("#sublist").append('<option value="' + subCategory.subCategoryID + '">' + subCategory.subCategoryDescription + '</option>');
		});
		$("#sublist").after('<button type="button" onclick="$.fn.filterBySubCategoryType();" class="btn-save  btn text-white expenses-background-color rounded-pill no-box-shadow  ">Save</button>');
		$("#sublist").materialSelect();
		$("input[data-activates='select-options-sublist']").attr('placeholder', "Select Sub Category")
		$("input[data-activates='select-options-sublist']").val('')
		var dropdown = $("#select-options-sublist input.form-check-input");
		dropdown.addClass("filled-in");
	}
	$.fn.reloadDDLS = function () {
		$.ajax({
			async: true,
			url: "/Expenses/_DDLForCharts",
			type: 'GET',
			cache: false,
			success: function (data) {
				$('.chart-dropdownlists').html(data)
				$('.chartDiv').html('')
			}
		});
	}
	$.fn.disableNotProjectDDLS = function () {
		$("input[data-activates='select-options-SelectedCategoryTypes']").attr("disabled", true)
		$("input[data-activates='select-options-parentlistMulitple']").attr("disabled", true)
		$("input[data-activates='select-options-sublist']").attr("disabled", true)
		$("input[data-activates='select-options-SelectedVendors']").attr("disabled", true)
		$("#SelectedCategoryTypes").attr("disabled", true)
		$("#parentlistMulitple").attr("disabled", true)
		$("#sublist").attr("disabled", true)
		$("#SelectedVendors").attr("disabled", true)
	}
	$.fn.disableNotCategoryDDLS = function () {
		$("input[data-activates='select-options-SelectedSubProjects']").attr("disabled", true)
		$("input[data-activates='select-options-SelectedProjects']").attr("disabled", true)
		$("#SelectedSubProjects").attr("disabled", true)
		$("#SelectedProjects").attr("disabled", true)

	}
})