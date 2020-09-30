// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//global Exchange Rate variable (usd --> nis)

$(function () {

	var VatPercentage = .17;


	function showmodal() {
		$("#modal").modal('show');
	};

	jQuery.fn.extend({
		destroyMaterialSelect: function () {
			return this.each(function () {
				let wrapper = $(this).parent();
				let core = wrapper.find('select');
				wrapper.after(core.removeClass('initialized').prop('outerHTML'));
				wrapper.remove();
			});
		}
	});

	//modal adjust scrollability/height
	//$("#myModal").modal('handleUpdate');

	$(".stop-event").on("click", function (e) {
		console.log("in stop event");
		e.preventDefault();
		e.stopPropagation();
		return false;
	});

	//change product subcategory dropdown according to the parent categroy selection when a parent category is selected
	$("#parentlist").change(function () {
		$.fn.parentListChange();
	});
	$('.modal').on('change', '#parentlist', function () {
		$.fn.parentListChange();
	});

	$.fn.parentListChange = function () {
		console.log("in parent list");
		var parentCategoryId = $("#parentlist").val();
		console.log("parentcategoryid: " + parentCategoryId);
		var url = "/Requests/GetSubCategoryList";
		console.log("url: " + url);

		$.getJSON(url, { ParentCategoryId: parentCategoryId }, function (data) {
			console.log("in getjson");
			var firstitem1 = '<option value=""> Select Subcategory</option>';
			$("#sublist").empty();
			$("#sublist").append(firstitem1);
			console.log("ONE");

			$.each(data, function (i, subCategory) {
				var newitem1 = '<option value="' + subCategory.productSubcategoryID + '">' + subCategory.productSubcategoryDescription + '</option>';
				$("#sublist").append(newitem1);
			});
			$("#sublist").materialSelect();
			return false;
		});
	};
	//change product subcategory dropdown according to the parent categroy selection when a parent category is selected
	$(".Project").change(function () {
		$.fn.changeProject($(this).val());
	});

	$('.modal').on('change', ".Project", function () {
		$.fn.changeProject($(this).val());
	});

	$.fn.changeProject = function (val) {
		console.log("project was changed");
		var projectId = val;
		var url = "/Requests/GetSubProjectList";

		$.getJSON(url, { ProjectID: projectId }, function (data) {
			var item = "<option value=''>Select Sub Project</option>";
			$("#SubProject").empty();
			$("#SubProject").append(item);
			$.each(data, function (i, subproject) {
				item = '<option value="' + subproject.subProjectID + '">' + subproject.subProjectDescription + '</option>'
				$("#SubProject").append(item);
			});
			$("#SubProject").materialSelect();
			return false
		});
	};

	//search forms- Redo js in newer versions
	$("#search-form #Project").change(function () {
	});

	$("#search-form #SubProject").change(function () {
		if ($(this).val() != 'Select a sub project') {
			$("#search-form #Project").attr("disabled", true);
		}
		else {
			$("#search-form #Project").attr("disabled", false);
		}
	});

	$("#search-form #sublist").change(function () {
		if ($(this).val() == 'Please select a subcategory') {
			$("#search-form #parentlist").attr("disabled", false);
		}
		else if ($(this).val() == '') {
			$("#search-form #parentlist").attr("disabled", false);
		}
		else {
			$("#search-form #parentlist").attr("disabled", true);
		}
	});

	$("#search-form #vendorList").change(function () {
		$("#search-form #vendorBusinessIDList").val($(this).val());
	});

	$("#search-form #vendorBusinessIDList").change(function () {
		$("#search-form #vendorList").val($(this).val());
	});


	var today = new Date();
	var dd = today.getDate();
	var mm = today.getMonth(); //January is 0 but do not add because everytime you create a new line it adds 1
	var yyyy = today.getFullYear();

	if (dd < 10) { dd = '0' + dd }

	var prevmm = mm;
	var prevyyyy = yyyy;
	//insert the payment lines
	$("#Request_ParentRequest_Installments").change(function () {
		$.fn.ChangePaymentsTable($(this).val());
	});

	$("#Installments").on('change', function () {
		console.log("installments changed...");
		$.fn.ChangePaymentsTable($(this).val());
	});

	//$.fn.ChangePaymentsTable = function (installments) {
	//	//var installments = $(this).val();
	//	var countPrevInstallments = $(".payment-line").length;
	//	var difference = installments - countPrevInstallments;


	//	if (difference > 0) {
	//		var newIncrementNumber = countPrevInstallments;
	//		for (x = difference; x > 0; x--) {

	//			var newmm = 0;
	//			var newyyyy = 0;
	//			if (prevmm < 12) {
	//				newmm = parseInt(prevmm);
	//				newmm = newmm + 1;
	//				newyyyy = prevyyyy;
	//			}
	//			else {
	//				newyyyy = parseInt(prevyyyy) + 1;
	//				newmm = 1;
	//			}

	//			if (newmm < 10) { newmm = '0' + newmm }

	//			var paymentDate = newyyyy + '-' + newmm + '-' + dd;

	//			prevyyyy = newyyyy;
	//			prevmm = newmm;
	//			$.fn.AddNewPaymentLine(newIncrementNumber, paymentDate);
	//			newIncrementNumber++;
	//		};

	//		$.fn.AdjustPaymentDates();
	//	}
	//	else if (difference < 0) { //installments were removed
	//		for (x = difference; x < 0; x++) {
	//			$(".payments-table tr:last").remove();
	//		}
	//	}
	//};



	$.fn.AddNewPaymentLine = function (increment, date) {
		var htmlTR = "";
		htmlTR += "<tr class='payment-line'>";
		htmlTR += "<td>";
		htmlTR += '<input class="form-control-plaintext border-bottom payment-date" type="date" data-val="true" data-val-required="The PaymentDate field is required." id="NewPayments_' + increment + '__PaymentDate" name="NewPayments[' + increment + '].PaymentDate" value="' + date + '" />';
		htmlTR += '<span class="text-danger-centarix field-validation-valid" data-valmsg-for="NewPayments[' + increment + '].PaymentDate" data-valmsg-replace="true"></span>';
		htmlTR += '</td>';
		htmlTR += '<td>';
		htmlTR += '<select class="form-control-plaintext border-bottom paymentType" id="NewPayments_' + increment + '__CompanyAccount_PaymentType" name="NewPayments[' + increment + '].CompanyAccount.PaymentType"><option value="">Select A Payment Type </option>';
		htmlTR += '<option value="1">Credit Card</option>';
		htmlTR += '<option value="2">Bank Account</option>';
		htmlTR += '</select>';
		htmlTR += '<span class="text-danger-centarix field-validation-valid" data-valmsg-for="NewPayments[' + increment + '].CompanyAccount.PaymentType" data-valmsg-replace="true"></span>';
		htmlTR += '</td>';
		htmlTR += '<td>';
		var newPaymentsId = "NewPayments_" + increment + "__CompanyAccountID";
		var newPaymentsName = "NewPayments[" + increment + "].CompanyAccountID";
		htmlTR += '<select class="form-control-plaintext border-bottom companyAccountNum" id="' + newPaymentsId + '" name="' + newPaymentsName + '"></select>';
		htmlTR += '<span class="text-danger-centarix field-validation-valid" data-valmsg-for="NewPayments[' + increment + '].CompanyAccount.CompanyAccountID" data-valmsg-replace="true"></span>';
		htmlTR += '</td>';
		htmlTR += '<td>';
		htmlTR += '<input class="form-control-plaintext border-bottom" type="number" data-val="true" data-val-required="The PaymentID field is required." id="NewPayments_' + increment + '__PaymentID" name="NewPayments[' + increment + '].PaymentID" value="" />';
		htmlTR += '<span class="text-danger-centarix field-validation-valid" data-valmsg-for="NewPayments[' + increment + '].PaymentID" data-valmsg-replace="true"></span>';
		htmlTR += '</td>';
		htmlTR += '</tr >';
		//$("body").append(htmlTR);
		$(".payments-table tr:last").after(htmlTR);


	};

	//since the paymentType field is dynamically created, the function needs to be bound the payments-table b/c js binds server-side
	$(".payments-table").on("change", ".paymentType", function (e) {
		var paymentTypeID = $(this).val();
		var url = "/CompanyAccounts/GetAccountsByPaymentType";
		var id = "" + $(this).attr('id');
		var number = id.substr(12, 1);
		var newid = "NewPayments_" + number + "__CompanyAccountID";
		$.getJSON(url, { paymentTypeID: paymentTypeID }, function (data) {
			var item = "";
			$("#" + newid).empty();
			$.each(data, function (i, companyAccount) {
				item += '<option value="' + companyAccount.companyAccountID + '">' + companyAccount.companyAccountNum + '</option>'
			});
			$("#" + newid).html(item);
		});
	});


	$.fn.AdjustPaymentDates = function () {

	};
	//$('input[type=text]').on('blur', function () {
	//		$(this).prev().prev().css('border-bottom', '1px solid black');
	//		$(this).prev().prev().css('box-shadow', '0 1px 0 0 black');
	//		$(this).prev().prev().css('-webkit-box-shadow:', '0 1px 0 0 black');
	//}).on('focus', function () {
	//	if ($(this).val()) {
	//		$(this).prev().prev().css('border-bottom', '1px solid var(--order-inv-color)');
	//		$(this).prev().prev().css('box-shadow', '0 1px 0 0 var(--order-inv-color)');
	//		$(this).prev().prev().css('-webkit-box-shadow:', '0 1px 0 0 var(--order-inv-color)');
	//	}
	//});


	////Location Add View - Change dropdownlist
	//$("LocationInstance_LocationTypeID").change(function () {
	//	console.log("entered js got json function");
	//	var locationTypeID = $(this).val();
	//	var url = "/Locations/GetChildrenTypes";
	//	$.getJSON(url, { LocationTypeID: locationTypeId }, function (data) {
	//		console.log("in location instance json");
	//		$.each(data, function (i, locationType) {
	//			htmlChildType = '<div class='
	//		});
	//	});
	//});

	//payments on the price modal -cascading dropdown choice with json

	$(".paymentType").change(function () {
		var paymentTypeId = $(this).val();
		var url = "/Requests/GetCompanyAccountList";
		var paymentTypeId = $(this).attr("id");
		var firstNum = paymentTypeId.charAt(12);
		var secondNum = paymentTypeId.charAt(13);
		var numId = firstNum;
		if (secondNum != "_") {
			numId = firstNum + secondNum;
		}
		var companyAccountId = "NewPayments_" + numId + "__CompanyAccount";

		$.getJSON(url, { paymentTypeId: paymentTypeId }, function (data) {
			var item = "";
			$("#" + companyAccountId).empty();
			$.each(data, function (i, companyAccount) {
				item += '<option value="' + companyAccount.companyAccountId + '">' + companyAccount.companyAccountNum + ' - hello</option>'
			});
			$("#" + companyAccountId).html(item);
		});
	});

	$('.modal').on('change', '#vendorList', function () {
		console.log('in on change vendor')
		var vendorid = $(this).val();
		$.fn.ChangeVendorBusinessId(vendorid);
	});
	$("#vendorList").change(function () {
		var vendorid = $("#vendorList").val();
		$.fn.ChangeVendorBusinessId(vendorid);
	});
	$.fn.ChangeVendorBusinessId = function (vendorid) {
		var newBusinessID = "";

		//will throw an error if its a null value so tests it here
		if (vendorid > 0) {
			//load the url of the Json Get from the controller
			var url = "/Vendors/GetVendorBusinessID";
			$.getJSON(url, { VendorID: vendorid }, function (data) {
				//get the business id from json
				newBusinessID = data.vendorBuisnessID;
				//console.log("data.vendorBuisnessID: " + data.vendorBuisnessID);
				//console.log("data: " + data);
				//console.log("newBusinessID: " + newBusinessID);
				//cannot only use the load outside. apparently it needs this one in order to work
				$(".vendorBusinessId").val(newBusinessID);
				$(".vendorBusinessId").text(newBusinessID);
			})
		}
		//console.log("newBusinessID: " + newBusinessID);
		//if nothing was selected want to load a blank
		$(".vendorBusinessId").val(newBusinessID);
		//put the business id into the form
	}

	//view documents on modal view
	$(".view-docs").click(function (clickEvent) {
		console.log("order docs clicked!");
		clickEvent.preventDefault();
		clickEvent.stopPropagation();
		var title = $(this).val();
		$(".images-header").html(title + " Documents Uploaded:");
	});


	$(".expected-supply-days").change(function () {
		console.log("---------------------Request ExpectedSupplyDays: " + $(this).val() + " -------------------------------");
		var date;
		if ($(".for-supply-date-calc").length > 0) {
			console.log("in first of of check roder date")
			var date = $(".for-supply-date-calc").val().split("-");
			var dd = parseInt(date[2]);
			var mm = parseInt(date[1]);
			var yyyy = parseInt(date[0]);
		} else {
			date = new Date();
			var dd = parseInt(date.getDate());
			var mm = parseInt(date.getMonth() + 1);
			var yyyy = parseInt(date.getFullYear());
		}

		for (i = 0; i < $(this).val(); i++) {
			switch (mm) {
				case 1:
				case 3:
				case 5:
				case 7:
				case 8:
				case 10:
				case 12:
					if (dd == 31) {
						dd = 1;
						if (mm == 12) {
							mm = 1;
							yyyy = yyyy + 1;
						}
						else {
							mm = mm + 1;
						}
					}
					else {
						dd = dd + 1;
					}
					break;
				case 4:
				case 6:
				case 9:
				case 11:
					if (dd == 30) {
						dd = 1;
						if (mm == 12) {
							mm = 1;
							yyyy = yyyy + 1;
						}
						else {
							mm = mm + 1;
						}
					}
					else {
						dd = dd + 1;
					}
					break;
				case 2:
					var endDayOfFeb = 28;
					if (yyyy % 4 === 0) {
						endDayOfFeb = 29;
					}
					console.log("dd: " + dd)
					if (dd == endDayOfFeb) {
						dd = 1;
						if (mm == 12) {
							mm = 1;
							yyyy = yyyy + 1;
						}
						else {
							mm = mm + 1;
						}
					}
					else {
						dd = dd + 1;
					}
					break;
			}
		}
		if (dd < 10) { dd = '0' + dd }
		if (mm < 10) { mm = '0' + mm }
		var supplyDate = yyyy + '-' + mm + '-' + dd;
		$("input[name='expected-supply-days']").val(supplyDate);

	});


	$("#expected-supply-date").change(function () {
		var date = new Date($(this).val());
		if (date < new Date()) {
			return;
		}
		console.log("-------expected supply date: " + date)
		var Sdd = parseInt(date.getDate());
		var Smm = parseInt(date.getMonth() + 1);
		var Syyyy = parseInt(date.getFullYear());
		console.log("sdd + smm + syyyy: " + Sdd + " " + Smm + " " + Syyyy);
		var OrderDate;
		if ($('.for-supply-date-calc').length > 0) {
			console.log("in first of of check roder date")
			OrderDate = $(".for-supply-date-calc").val().split("-");
			var Idd = parseInt(OrderDate[2]);
			var Imm = parseInt(OrderDate[1]);
			var Iyyyy = parseInt(OrderDate[0]);
		} else {
			OrderDate = new Date();
			var Idd = parseInt(OrderDate.getDate());
			var Imm = parseInt(OrderDate.getMonth() + 1);
			var Iyyyy = parseInt(OrderDate.getFullYear());

		}

		console.log("idd + imm + iyyyy: " + Idd + " " + Imm + " " + Iyyyy);

		var amountOfDays = 0;
		var flag = false;
		while (!flag) {
			if (Sdd != Idd || Smm != Imm || Syyyy != Iyyyy) {
				amountOfDays++;
				switch (Imm) {
					case 1:
					case 3:
					case 5:
					case 7:
					case 8:
					case 10:
					case 12:
						if (Idd == 31) {
							Idd = 1;
							if (Imm == 12) {
								Imm = 1;
								Iyyyy = Iyyyy + 1;
							}
							else {
								Imm = Imm + 1;
							}
						}
						else {
							Idd = Idd + 1;
						}
						break;
					case 4:
					case 6:
					case 9:
					case 11:
						if (Idd == 30) {
							Idd = 1;
							if (Imm == 12) {
								Imm = 1;
								Iyyyy = Iyyyy + 1;
							}
							else {
								Imm = Imm + 1;
							}
						}
						else {
							Idd = Idd + 1;
						}
						break;
					case 2:
						var endDayOfFeb = 28;
						if (Iyyyy % 4 === 0) {
							endDayOfFeb = 29;
						}
						if (Idd == endDayOfFeb) {
							Idd = 1;
							if (Imm == 12) {
								Imm = 1;
								Iyyyy = Iyyyy + 1;
							}
							else {
								Imm = Imm + 1;
							}
						}
						else {
							Idd = Idd + 1;
						}
						break;
				}
				console.log("amount of days: " + amountOfDays);
			}
			else {
				flag = true;
			}
		}
		$(".expected-supply-days").val(amountOfDays);
	});

	$("#Request_Warranty").change(function () {
		var date = null;
		if ($("#Request_ParentRequest_OrderDate").length > 0) {
			date = $("#Request_ParentRequest_OrderDate").val().split("-");
			var dd = date[2];
			var mm = parseInt(date[1]); //January is 0! ?? do we still need th get month???
			var yyyy = date[0];
		} else {
			date = new Date();
			var dd = date.getDate();
			var mm = parseInt(date.getMonth() + 1); //January is 0! ?? do we still need th get month???
			var yyyy = date.getFullYear();
		}
		console.log("Date: " + date);

		var newmm = parseInt(mm) + parseInt($(this).val());
		mm = newmm;

		if (dd < 10) { dd = '0' + dd }

		var flag = true;
		while (flag) {
			if (mm > 12) {
				mm = parseInt(mm) - 12;
				yyyy = parseInt(yyyy) + 1;
			}
			else {
				flag = false;
			}
		}
		if (mm < 10) { mm = '0' + mm }

		var warrantyDate = yyyy + '-' + mm + '-' + dd;

		$("input[name='WarrantyDate']").val(warrantyDate);
	});


	$("#Request_ExpectedSupplyDays").change(function () {
		//var date = $("#Request_ParentRequest_InvoiceDate").val();
		//console.log("Order date: " + date);
		//console.log("this.val: " + $(this).val());
		//var supplyDate = Date.addDays($("#Request_ExpectedSupplyDays").val())
		//console.log("supplyDate: " + supplyDate);
		//$("input[name='expected-supply-days']").val(supplyDate);

	});

	$.fn.leapYear = function (year) {
		return ((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0);
	}

	//PRICE PAGE ON MODAL VIEW//
	$("#price-tab").click(function () {
		$.fn.CheckUnitsFilled();
		$.fn.CheckSubUnitsFilled();
		//I don't think that we need $.fn.CheckSubSubUnitsFilled over here b/c we don't need to enable or disable anything and the CalculateSubSubUnits should already run
		$.fn.CalculateSumPlusVat();
		$.fn.CheckCurrency();
	});

	$("#currency").change(function (e) {
		$.fn.CheckCurrency();
	});

	$("#Request_ExchangeRate").change(function (e) {
		$.fn.CalculateSumPlusVat();
		$.fn.CalculateUnitAmounts();
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
	});

	$("#Request_Cost").change(function (e) {
		$.fn.CalculateSumPlusVat();
		$.fn.CalculateUnitAmounts();
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();

	});

	$("#sum-dollars").change(function (e) {
		$.fn.CalculateSumPlusVat();
		$.fn.CalculateUnitAmounts();
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
		$.fn.updateDebt();
	});

	$("#Request_Unit").change(function () {
		$.fn.CheckUnitsFilled();
	});

	$("#Request_UnitTypeID").change(function () {
		$.fn.CheckUnitsFilled();
	});
	$("#select-options-Request_UnitTypeID").change(function () {
		//console.log("unit type id changed");
		$.fn.CheckUnitsFilled();
	});

	$("#Request_SubUnit").change(function () {
		console.log("about to check subunitsfilled");
		$.fn.CheckSubUnitsFilled();
	});

	$("#Request_SubUnitTypeID").change(function () {
		console.log("about to check subunitsfilled");
		$.fn.CheckSubUnitsFilled();
	});
	$("#select-options-Request_SubUnitTypeID").change(function () {
		console.log("about to check subunitsfilled");
		$.fn.CheckSubUnitsFilled();
	});

	$("#Request_SubSubUnit").change(function () {
		console.log("about to check subunitsfilled");
		$.fn.CheckSubUnitsFilled();
	});

	$("#Request_SubSubUnitTypeID").change(function () {
		console.log("about to check subunitsfilled");
		$.fn.CheckSubUnitsFilled();
	});
	$("#select-options-Request_SubSubUnitTypeID").change(function () {
		console.log("about to check subunitsfilled");
		$.fn.CheckSubUnitsFilled();
	});

	$.fn.CheckCurrency = function () {
		var currencyType = $("#currency").val();
		switch (currencyType) {
			case "dollar":
				$("#Request_Cost").prop("readonly", true);
				$("#sum-dollars").prop("disabled", false);
				break;
			case "shekel":
				$("#Request_Cost").prop("readonly", false);
				$("#sum-dollars").prop("disabled", true);
				break;
		}
	};

	$.fn.CheckUnitsFilled = function () {
		//console.log("in check units function");
		if (($("#edit #Request_Unit").val() > 0 && $("#edit #Request_UnitTypeID").val())
			|| ($("#select-options-Request_Unit").val() > 0 && $("#select-options-Request_UnitTypeID").val())) {
			//console.log("both have values");
			$.fn.EnableSubUnits();
			$.fn.ChangeSubUnitDropdown();
		}
		else {
			$.fn.DisableSubUnits();
			$.fn.DisableSubSubUnits();
		}
		$.fn.CalculateUnitAmounts();
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
	};

	$.fn.CheckSubUnitsFilled = function () {
		console.log("in check sub units function");
		if (($("#Request_SubUnit").val() > 0 && $("#Request_SubUnitTypeID").val())
			|| ($("#Request_SubUnit").val() > 0 && $("#select-options-Request_SubUnitTypeID").val())) {
			$.fn.EnableSubSubUnits();
			console.log("about to change subsubunitdropdown");
			$.fn.ChangeSubSubUnitDropdown();
		}
		else {
			$.fn.DisableSubSubUnits();
		}
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
	}


	$.fn.EnableSubUnits = function () {
		//console.log("enable subunits");
		$("#Request_SubUnit").prop("disabled", false);
		$("#Request_SubUnitTypeID").destroyMaterialSelect();
		$("#Request_SubUnitTypeID").removeAttr("disabled")
		$('#Request_SubUnitTypeID').materialSelect();
		//$("#select-options-Request_SubUnitTypeID").prop("disabled", false);
		//$("#select-options-Request_SubUnitTypeID").removeAttr("disabled");
		$('[data-activates="select-options-Request_SubUnitTypeID"]').prop('disabled', false);
	};

	$.fn.EnableSubSubUnits = function () {
		$("#Request_SubSubUnit").prop("disabled", false);
		$("#Request_SubSubUnitTypeID").destroyMaterialSelect()
		$("#Request_SubSubUnitTypeID").removeAttr("disabled")
		$('#Request_SubSubUnitTypeID').materialSelect();
		//$("#select-options-Request_SubSubUnitTypeID").prop("disabled", false);
		//$("#select-options-Request_SubSubUnitTypeID").removeAttr("disabled");
		$('[data-activates="select-options-Request_SubSubUnitTypeID"]').prop('disabled', false);
	};

	$.fn.DisableSubUnits = function () {
		$("#Request_SubUnit").prop("disabled", true);
		$("#Request_SubUnitTypeID").prop("disabled", true);
		//$("#select-options-Request_SubUnitTypeID").prop("disabled", true);
		//$("#select-options-Request_SubUnitTypeID").prop("aria-disabled", true);
		$('[data-activates="select-options-Request_SubUnitTypeID"]').prop('disabled', true);
	};

	$.fn.DisableSubSubUnits = function () {
		$("#Request_SubSubUnit").prop("disabled", true);
		$("#Request_SubSubUnitTypeID").prop("disabled", true);
		//$("#select-options-Request_SubSubUnitTypeID").prop("disabled", true);
		//$("#select-options-Request_SubSubUnitTypeID").prop("aria-disabled", true);
		$('[data-activates="select-options-Request_SubSubUnitTypeID"]').prop('disabled', true);
	};

	$.fn.CalculateUnitAmounts = function () {
		$unitSumShekel = parseFloat($("#Request_Cost").val()) / $("#Request_Unit").val();
		$iptBox = $("input[name='unit-price-shekel']");
		$.fn.ShowResults($iptBox, $unitSumShekel);
		var $exchangeRate = $("#Request_ExchangeRate").val();
		$unitSumDollars = $unitSumShekel / $exchangeRate;
		$iptBox = $("input[name='unit-price-dollars']");
		$.fn.ShowResults($iptBox, $unitSumDollars);
	};

	$.fn.CalculateSubUnitAmounts = function () {
		$subUnitSumShekel = $("#unit-price-shekel").val() / $("#Request_SubUnit").val();
		$iptBox = $("input[name='subunit-price-shekel']");
		$.fn.ShowResults($iptBox, $subUnitSumShekel);
		var $exchangeRate = $("#Request_ExchangeRate").val();
		$subUnitSumDollars = $subUnitSumShekel / $exchangeRate;
		$iptBox = $("input[name='subunit-price-dollars']");
		$.fn.ShowResults($iptBox, $subUnitSumDollars);
	};

	$.fn.CalculateSubSubUnitAmounts = function () {
		$subSubUnitSumShekel = $("#subunit-price-shekel").val() / $("#Request_SubSubUnit").val();
		$iptBox = $("input[name='subsubunit-price-shekel']");
		$.fn.ShowResults($iptBox, $subSubUnitSumShekel);
		var $exchangeRate = $("#Request_ExchangeRate").val();
		$subSubUnitSumDollars = $subSubUnitSumShekel / $exchangeRate;
		$iptBox = $("input[name='subsubunit-price-dollars']");
		$.fn.ShowResults($iptBox, $subSubUnitSumDollars);
	};

	$.fn.CalculateSumPlusVat = function () {
		var $exchangeRate = $("#Request_ExchangeRate").val();
		var sumShek = $("#Request_Cost").val();
		//console.log("sumShek: " + sumShek);
		var vatCalc = sumShek * .17;
		//console.log("VatPercentage: " + VatPercentage);
		//console.log("vatCalc: " + vatCalc);
		//$("#Request_VAT").val(vatCalc)
		//var vatInShekel = $("#Request_VAT").val();
		if ($("#sum-dollars").prop("disabled")) {
			$sumDollars = parseFloat($("#Request_Cost").val()) / $exchangeRate;
			$iptBox = $('input[name="sum-dollars"]');
			$.fn.ShowResults($iptBox, $sumDollars);
		}
		else if ($("#Request_Cost").prop("readonly")) {
			$sumShekel = $("#sum-dollars").val() * $exchangeRate;
			$iptBox = $("input[name='Request.Cost']");
			$.fn.ShowResults($iptBox, $sumShekel);
		}
		$sumShekel = parseFloat($("#Request_Cost").val());
		//$vatOnshekel = $sumShekel * parseFloat(vatCalc);
		$('#Request_VAT').val(vatCalc.toFixed(2));
		$sumTotalVatShekel = $sumShekel + vatCalc;
		$iptBox = $("input[name='sumPlusVat-Shekel']");
		$.fn.ShowResults($iptBox, $sumTotalVatShekel);
		$sumTotalVatDollars = $sumTotalVatShekel / $exchangeRate;
		$iptBox = $("input[name='sumPlusVat-Dollar']");
		$.fn.ShowResults($iptBox, $sumTotalVatDollars);
	};

	$.fn.ChangeSubUnitDropdown = function () {
		var selected = $(':selected', $("#Request_UnitTypeID"));
		var selected2 = $(':selected', $("#select-options-Request_UnitTypeID"));
		//console.log("u selected: " + selected);
		var optgroup = selected.closest('optgroup').attr('label');
		var optgroup2 = selected2.closest('optgroup').attr('label');
		console.log("u optgroup: " + optgroup);
		console.log("u optgroup2: " + optgroup2);
		//the following is based on the fact that the unit types and parents are seeded with primary key values
		switch (optgroup) {
			case "Units":
				console.log("inside optgroup units");
				//$("#Request_SubUnitTypeID optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
				//$("#Request_SubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Units'] li").prop('disabled', false).prop('hidden', false);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume'] li").prop('disabled', false).prop('hidden', false);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Units'] li").show();
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume'] li").show();
				$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume']").css("display", "none");
				//$("#Request_SubUnitTypeID optgroup[label='Units'] li").show();
				//$("#Request_SubUnitTypeID optgroup[label='Weight/Volume'] li").show();
				break;
			case "Weight/Volume":
				console.log("inside optgroup weight/volume");
				//$("#Request_SubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				//$("#Request_SubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Units'] li").prop('disabled', true).prop('hidden', true);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume'] li").prop('disabled', false).prop('hidden', false);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Units'] li").hide();
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume'] li").show();
				//$("#Request_SubUnitTypeID optgroup[label='Units'] li").hide();
				//$("#Request_SubUnitTypeID optgroup[label='Weight/Volume'] li").show();
				break;
			case "Test":
				console.log("inside optgroup test");
				//$("#Request_SubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				//$("#Request_SubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Units'] li").prop('disabled', true).prop('hidden', true);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume'] li").prop('disabled', true).prop('hidden', true);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Units'] li").hide();
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume'] li").hide();
				//$("#Request_SubUnitTypeID optgroup[label='Units'] li").hide();
				//$("#Request_SubUnitTypeID optgroup[label='Weight/Volume'] li").hide();
				break;
		}
		switch (optgroup2) {
			case "Units":
				console.log("inside optgroup2 units");
				$("#select-options-Request_SubUnitTypeID optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
				$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				break;
			case "Weight/Volume":
				console.log("inside optgroup2 weight/volume");
				$("#select-options-Request_SubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				break;
			case "Test":
				console.log("inside optgroup2 test");
				$("#select-options-Request_SubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
				break;
		}
	};
	//change sub sub unit dropdown
	$.fn.ChangeSubSubUnitDropdown = function () {
		console.log("in change subsubunitdropdown");
		var selected = $(':selected', $("#Request_SubUnitTypeID"));
		var selected2 = $(':selected', $("#select-options-Request_SubUnitTypeID"));
		var optgroup = selected.closest('optgroup').attr('label');
		var optgroup2 = selected.closest('optgroup').attr('label');
		switch (optgroup) {
			case "Units":
				$("#Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
				$("#Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				break;
			case "Weight/Volume":
				$("#Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				$("#Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				break;
			case "Test":
				$("#Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				$("#Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
				break;
		}
		switch (optgroup2) {
			case "Units":
				$("#select-options-Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
				$("#select-options-Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				break;
			case "Weight/Volume":
				$("#select-options-Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				$("#select-options-Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				break;
			case "Test":
				$("#select-options-Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				$("#select-options-Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
				break;
		}
	};

	$.fn.ShowResults = function ($inputBox, $value) { //this function ensures that the value passed back won't be NaN or undefined --> it'll instead send back a blank
		var theResult = parseFloat($value);
		theResult = theResult.toFixed(2);
		theResult = isFinite(theResult) && theResult || "";
		$inputBox.val(theResult);
	}


	//LOCATIONS:


	//AJAX load full partial view for modalview manage locations
	$("#locationTypeDepthZero").change(function () {
		var myDiv = $(".divSublocations");
		var selectedId = $(this).children("option:selected").val();
		$.ajax({
			//IMPORTANT: ADD IN THE ID
			url: "/Requests/ReceivedModalSublocations/?LocationTypeID=" + selectedId,
			type: 'GET',
			cache: false,
			context: myDiv,
			success: function (result) {
				this.html(result);
			}
		});
		//$(document).ajaxStart(function () {
		//	$("#loading").show();
		//});

		//$(document).ajaxComplete(function () {
		//	$("#loading").hide();
		//});
	});

	//Received Modal => fill up the next selectLocationInstance with the right selections
	$(".SLI").change(function () {


		//ONE ---> GET THE NEXT DROPDOWNLIST

		var name = $(this).attr('name');
		var number = name.charAt(name.length - 2);
		var place = parseInt(number);

		console.log("")
		var nextSelectClass = name.replace(place.toString(), (place + 1).toString());
		var nextSelect = $("select[name='" + nextSelectClass + "']");

		var locationInstanceParentId = $(this).children("option:selected").val();
		var url = "/Requests/GetSublocationInstancesList";/*/?LocationInstanceParentID=" + locationInstanceParentId;*/

		if (nextSelect.length) { //if there is another one


			var NotFinished = true;
			var placeCounter = place + 1; //adding one to skip the select that we will fill in below
			var newNextSelect = nextSelect;

			//if the same one was selected jquery automatically deals with it
			while (NotFinished) {
				//var incrementedPlaceCounter = placeCounter + 1);
				nextSelectClass = nextSelectClass.replace(placeCounter.toString(), (placeCounter + 1).toString());
				newNextSelect = $("select[name='" + nextSelectClass + "']");
				if (newNextSelect.length) {
					newNextSelect.html("");
				}
				else {
					NotFinished = false;
				}
				placeCounter++;
			}


			$.getJSON(url, { locationInstanceParentId, locationInstanceParentId }, function (result) {
				var item = "<option>Select Location Instance</option>";
				$.each(result, function (i, field) {
					item += '<option value="' + field.locationInstanceID + '">' + field.locationInstanceName + '</option>'
				});
				nextSelect.html(item);
			});

		}
		//else {
		//	console.log("in the else unfortunately... :( ");
		//}


		//TWO ---> FILL VISUAL VIEW
		var myDiv = $(".visualView");
		if (locationInstanceParentId == 0) { //if zero was selected
			console.log("selected was 0");
			//check if there is a previous select box
			var oldSelectClass = name.replace(place.toString(), (place - 1).toString());
			var oldSelect = $("select[name='" + oldSelectClass + "']");
			if (oldSelect.length) {
				console.log("oldSelectClass " + oldSelectClass + " exists and refilling with that");
				var oldSelected = $("." + oldSelect).children("option:selected").val();
				console.log("oldSelected: " + oldSelected);
				$.ajax({
					url: "/Requests/ReceivedModalVisual/?LocationInstanceID=" + oldSelected,
					type: 'GET',
					cache: false,
					context: myDiv,
					success: function (result) {
						this.html(result);
					}
				});
			}
			else {
				console.log("oldSelectClass " + oldSelectClass + " does not exist and clearing");
				myDiv.html("");
			}
		}
		else {
			console.log("regular visual");
			$.ajax({
				url: "/Requests/ReceivedModalVisual/?LocationInstanceID=" + locationInstanceParentId,
				type: 'GET',
				cache: false,
				context: myDiv,
				success: function (result) {
					this.html(result);
				}
			});
		}


	});

	//$.fn.CallAjax() = function ($url, $div) {

	//	$.ajax({
	//		//IMPORTANT: ADD IN THE ID
	//		url: $url,
	//		type: 'GET',
	//		cache: false,
	//		context: $div,
	//		success: function (result) {
	//			this.html(result); //do we need to change this to $div?
	//		}
	//	});
	//};

	$(".modal").on('hide.bs.modal', function () {
		$('.modal-backdrop').remove()
	});



	$(".visual-locations-zoom").off("click").on("click", function (e) {
		console.log("called visual locations zoom with an id of: " + $(this).val());
		var myDiv = $(".location-modal-view");
		console.log("myDiv: " + myDiv);
		var parentId = $(this).val();
		$("#visualZoomModal").replaceWith('');
		$(".modal").replaceWith('');
		//console.log("about to call ajax with a parentid of: " + parentId);
		$.ajax({
			async: true,
			url: "/Locations/VisualLocationsZoom/?VisualContainerId=" + parentId,
			type: 'GET',
			cache: false,
			success: function (data) {
				var modal = $(data);
				$('body').append(modal);
				$("#visualZoomModal").modal({
					backdrop: true,
					keyboard: true,
				});
				$(".modal").modal('show');
				//$('.modal-backdrop').remove()
				var firstTDFilled = $("td.lab-man-50-background-color");
				var height = firstTDFilled.height();
				var width = firstTDFilled.width();
				console.log("h: " + height + "------ w: " + width);
				//$("td").height(height);
				//$("td").width(width);
				$(".visualzoom td").css('height', height);
				$(".visualzoom td").css('width', width);
				//$("td").addClass("danger-color");
			}
		});
		//$.ajax({
		//	//IMPORTANT: ADD IN THE ID
		//	url: "/Locations/VisualLocationsZoom/?VisualContainerId=" + parentId,
		//	type: 'GET',
		//	cache: false,
		//	context: myDiv,
		//	success: function (result) {
		//		this.html(result);
		//		//turn off data dismiss by clicking out of the box and by pressing esc
		//		myDiv.modal({
		//			backdrop: 'static',
		//			keyboard: false
		//		});
		//		//shows the modal
		//		myDiv.modal('show');
		//		//bootstrap dynamically adds a class of modal-backdrop which must be taken off to make it clickable
		//		$(".modal-backdrop").remove();

		//	}
		//});
	});

	//$(".clicked-inside-button").click(function () {
	//	$(this).parent().removeClass("td-selected-inside");
	//	$(this).parent().removeClass("lab-man-50-background-color");
	//	$(this).parent().addClass("td-selected-inside");
	//	$(this).parent().addClass("lab-man-50-background-color");
	//});

	//$(".clicked-outer-button").click(function () {
	//	$(this).parent().removeClass("td-selected-outer");
	//	$(this).parent().parent().removeClass("td-selected-outer");
	//	$(this).parent().removeClass("lab-man-50-background-color");
	//	$(this).parent().parent().removeClass("lab-man-50-background-color");
	//	$(this).parent().parent().parent().removeClass("lab-man-50-background-color");
	//	$(this).parent().addClass("td-selected-outer");
	//	$(this).parent().parent().addClass("td-selected-outer");
	//	$(this).parent().addClass("lab-man-50-background-color");
	//	$(this).parent().parent().addClass("lab-man-50-background-color");
	//	$(this).parent().parent().parent().addClass("lab-man-50-background-color");
	//});



	function changeTerms(checkbox) {
		if (checkbox.checked) {
			$(".installments").hide();
			console.log("checked");
			$("#Request_Terms").append(`<option value="${"0"}">${"Pay Now"}</option>`);;
			$("#Request_Terms").val("0");
			$("#Request_Terms").attr("disabled", true);
		}
		else {
			console.log("unchecked");
			$(".installments").show()
			$("#Request_Terms").attr("disabled", false);
			$("#Request_Terms option[value='0']").remove();
		}
	}

	$(".documents-tab").click(function (e) {
		//this is for validation
		$("#myForm").valid();
		$.fn.validatePriceTab();

		$.fn.HideAllDocs();
		//$.fn.CheckIfFileSelectsAreFull();


	});

	$(".open-invoice-doc-modal").click(function (e) {
		e.preventDefault();
		e.stopPropagation();
		//console.log("in opened invoice doc modal");
		//$("#documentsModal").replaceWith('');
		//var urltogo = $("#documentSubmit").attr("url");
		//var arrRequestIds = $(".form-check.accounting-select .form-check-input:checked").map(function () {
		//	return $(this).attr("id")
		//}).get()
		//urltogo = urltogo + "?ids=" + arrRequestIds + "&RequestFolderNameEnum=Invoices&IsEdittable=true&IsOperations=false&IsNotifications=true";
		//console.log("urltogo: " + urltogo);
		//$.ajax({
		//	async: true,
		//	url: urltogo,
		//	type: 'GET',
		//	cache: false,
		//	success: function (data) {
		//		var modal = $(data);
		//		$('body').append(modal);
		//		$("#documentsModal").modal({
		//			backdrop: false,
		//			keyboard: true,
		//		});
		//		$(".modal").modal('show');
		//	}
		//});
	});

	$(".open-document-modal").click(function (e) {
		e.preventDefault();
		e.stopPropagation();
		console.log("clicked open doc modal");
		$(".open-document-modal").removeClass("active-document-modal");
		var isOperations = $(".open-document-modal").hasClass('operations')
		$(this).addClass("active-document-modal");
		var enumString = $(this).data("string");
		console.log("enumString: " + enumString);
		var requestId = $(this).data("id");
		console.log("requestId: " + requestId);
		var isEdittable = $(this).data("val");
		console.log("isEdittable: " + isEdittable);
		$.fn.OpenDocumentsModal(enumString, requestId, isEdittable, isOperations);
	});

	$.fn.OpenDocumentsModal = function (enumString, requestId, isEdittable, isOperations) {
		console.log("in open doc modal");
		$("#documentsModal").replaceWith('');
		var urltogo = $("#documentSubmit").attr("url");
		//var urlToGo = "DocumentsModal?id=" + requestId + "&RequestFolderNameEnum=" + enumString + "&IsEdittable=" + isEdittable;*/
		console.log("urltogo: " + urltogo);
		urltogo = urltogo + "?id=" + requestId + "&RequestFolderNameEnum=" + enumString + "&IsEdittable=" + isEdittable + "&IsOperations=" + isOperations
		//$(".modal-backdrop").first().removeClass();
		$.ajax({
			async: true,
			url: urltogo,
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

	$(".file-select").on("change", function (e) {
		console.log("file was changed");
		$cardDiv = $(this).closest("div.card");
		console.log("cardDiv: " + JSON.stringify($cardDiv));
		$cardDiv.addClass("document-border");
	});



	$.fn.HideAllDocs = function () {
		//$(".orders-view").hide();
		$(".invoices-view").hide();
		$(".shipments-view").hide();
		$(".quotes-view").hide();
		$(".info-view").hide();
		$(".pictures-view").hide();
		$(".returns-view").hide();
		$(".credits-view").hide();
	};

	$(".show-orders-view").click(function () {
		$(".invoices-view").hide();
		$(".shipments-view").hide();
		$(".quotes-view").hide();
		$(".info-view").hide();
		$(".pictures-view").hide();
		$(".returns-view").hide();
		$(".credits-view").hide();
		$(".orders-view").toggle();
	});
	$(".show-invoices-view").click(function () {
		$(".orders-view").hide();
		$(".shipments-view").hide();
		$(".quotes-view").hide();
		$(".info-view").hide();
		$(".pictures-view").hide();
		$(".returns-view").hide();
		$(".credits-view").hide();
		$(".invoices-view").toggle();
	});
	$(".show-shipments-view").click(function () {
		$(".orders-view").hide();
		$(".invoices-view").hide();
		$(".quotes-view").hide();
		$(".info-view").hide();
		$(".pictures-view").hide();
		$(".returns-view").hide();
		$(".credits-view").hide();
		$(".shipments-view").toggle();
	});
	$(".show-quotes-view").click(function () {
		$(".orders-view").hide();
		$(".invoices-view").hide();
		$(".shipments-view").hide();
		$(".info-view").hide();
		$(".pictures-view").hide();
		$(".returns-view").hide();
		$(".credits-view").hide();
		$(".quotes-view").toggle();
	});
	$(".show-info-view").click(function () {
		$(".orders-view").hide();
		$(".invoices-view").hide();
		$(".shipments-view").hide();
		$(".quotes-view").hide();
		$(".pictures-view").hide();
		$(".returns-view").hide();
		$(".credits-view").hide();
		$(".info-view").toggle();
	});
	$(".show-pictures-view").click(function () {
		$(".orders-view").hide();
		$(".invoices-view").hide();
		$(".shipments-view").hide();
		$(".quotes-view").hide();
		$(".info-view").hide();
		$(".returns-view").hide();
		$(".credits-view").hide();
		$(".pictures-view").toggle();
	});
	$(".show-returns-view").click(function () {
		$(".orders-view").hide();
		$(".invoices-view").hide();
		$(".shipments-view").hide();
		$(".quotes-view").hide();
		$(".info-view").hide();
		$(".pictures-view").hide();
		$(".credits-view").hide();
		$(".returns-view").toggle();
	});
	$(".show-credits-view").click(function () {
		$(".orders-view").hide();
		$(".invoices-view").hide();
		$(".shipments-view").hide();
		$(".quotes-view").hide();
		$(".info-view").hide();
		$(".pictures-view").hide();
		$(".returns-view").hide();
		$(".credits-view").toggle();
	});


	//$(".close").click(function () {
	//	console.log("close");
	//	$(".modal").hide();
	//	$(".modal").modal('hide');
	//	$('.modal').replaceWith('');
	//	//$.ajax({
	//	//	async: true,
	//	//	url: "Locations/Index",
	//	//	type: 'GET',
	//	//	cache: false,
	//	//});
	//});


	$(".load-product-edit").on("click", function (e) {
		console.log("inside of load product edit")
		e.preventDefault();
		e.stopPropagation();
		var $itemurl = "";
		if ($(this).hasClass('operations')) {
			$itemurl = "Operations/EditModalView/?id=" + $(this).val();
		}
		else {
			//takes the item value and calls the Products controller with the ModalView view to render the modal inside
			$itemurl = "Requests/EditModalView/?id=" + $(this).val();
		}
		console.log("itemurl: " + $itemurl);
		$.fn.CallPageRequest($itemurl, "edit");
		return false;
	});

	$(".load-product-edit-summary").on("click", function (e) {
		console.log("inside of load product edit")
		e.preventDefault();
		e.stopPropagation();
		//takes the item value and calls the Products controller with the ModalView view to render the modal inside
		var $itemurl = "Requests/EditSummaryModalView/?id=" + $(this).val();
		console.log("itemurl: " + $itemurl);
		$.fn.CallPageRequest($itemurl, "edit");
		return false;
	});


	$.fn.updateDebt = function () {
		console.log("in update debt");
		var sum = $("#Request_Cost").val();
		var installments = $("#Request_ParentRequest_Installments").val();
		var tdate = new Date();
		var dd = tdate.getDate(); //yields day
		var MM = tdate.getMonth(); //yields month
		var yyyy = tdate.getFullYear(); //yields year
		if (dd < 10) {
			dd = "0" + dd
		}
		if (MM < 10) {
			MM = "0" + (MM + 1)
		} else {
			MM = MM + 1;
		}
		var today = yyyy + "-" + (MM) + "-" + dd;
		console.log("today:" + today);
		//count how many installment dates already passed
		var count = 0;
		if (installments != 0) {
			$(".payments-table .payment-date").each(function (index) {
				var date = $(this).val();
				console.log("date: " + date);
				if (today >= date) {
					console.log("today > date");
					//date passed
					count = count + 1
				}
			});
		}
		if (count != 0) {
			var paymentPerMonth = sum / installments;
			console.log(" paymentPerMonth: " + paymentPerMonth);
			var amountToSubstractFromDebt = paymentPerMonth * count;
			console.log(" amountToSubstractFromDebt: " + amountToSubstractFromDebt);
			var debt = sum - amountToSubstractFromDebt
			console.log(" sum: " + sum);
			console.log(" debt: " + debt);
			$("#Debt").val(debt);
		} else {
			$("#Debt").val(sum);
		}
		var vatCalc = sum * .17;
		console.log("vatCalc" + vatCalc);
		$('#Request_VAT').val(vatCalc.toFixed(2));
	};

	$(".payments-table").on("change", ".payment-date", function (e) {
		console.log("in change .payments-table ");
		$.fn.updateDebt();
	});

	$("#Request_ExchangeRate").change(function (e) {
		console.log("in change #Request_ExchangeRate ");
		$.fn.updateDebt();
	});


	$("#sum-dollars").change(function (e) {
		console.log("in change #sum-dollars ");
		$.fn.updateDebt();
	});

	$("#Request_ParentRequest_Installments").change(function () {
		console.log("in change Request_ParentRequest_Installments ");
		$.fn.updateDebt();
	});


	$(".open-loading").on("click", function (e) {
		$("#loading").show();
	});


	$(".load-location-index-view").click(function (e) {
		//clear the div to restart filling with new children
		$(".load-location-index-view").removeClass("location-type-selected");
		$.fn.setUpLocationIndexList($(this).val())
	});

	$.fn.setUpLocationIndexList = function (val) {
		//clear second div just in case
		var subDiv = $(".colTwoSublocations");
		subDiv.html("");

		//fill up col 2 with the next one
		$("#loading3")/*.delay(1000)*/.show(0);
		var myDiv = $(".colOne");
		var typeId = val;
		$.ajax({
			url: "/Locations/LocationIndex/?typeId=" + typeId,
			type: 'GET',
			cache: false,
			context: myDiv,
			success: function (result) {
				$(".VisualBoxColumn").hide();
				$(".colTwoSublocations").hide();
				$("#loading3").hide();
				$("#loading3")/*.delay(1000)*/.hide(0);
				// the following line keeps the parent type on top underlined
				$('button[value="' + typeId + '"]').addClass("location-type-selected");
				myDiv.show();
				this.html(result);

			}
		});

	};

	$(".load-visual-sublocation-view").off("click").on("click", function (e) {
		console.log("load visual sublocation view");//delete all prev tables:
		var tableVal = $(this).val();
		$('div[id^="table"]').each(function () {
			var tableID = $(this).attr("id");
			var tableNum = tableID.substr(5, tableID.length);
			if (parseInt(tableVal) < parseInt(tableNum)) {
				console.log(tableVal + " < " + tableNum);
				$(this).hide();
			}
			else {
				console.log(tableVal + " > " + tableNum);
			}
		});

		var myDiv = $(".colTwoSublocations");
		var table = $(this).closest('table');

		//Begin CSS Styling
		var stylingClass = "lab-man-50-background-color";

		table.children('tbody').children('tr').children('td').removeClass(stylingClass);
		$(this).parent().addClass(stylingClass);

		var parentStylingClass = "parent-location-selected-outer-lab-man";
		if ($(this).hasClass("parent-location")) {
			$("body td").removeClass(parentStylingClass);
			$(this).parent().addClass(parentStylingClass);

		}
		//End CSS Styling
		$.fn.setUpVisual($(this).val());
	});


	$(".load-sublocation-view").off("click").on("click", function (e) {
		//e.preventDefault();
		//e.stopPropagation();
		//add or remove the background class in col 1
		//$(".load-sublocation-view").parent().removeClass("td-selected");
		//$(this).parent().addClass("td-selected");
		$("#loading1")/*.delay(1000)*/.show(0);

		//delete all prev tables:
		var tableVal = $(this).val();
		console.log("-------------------------------------------------------------");
		console.log("table val: " + tableVal);
		$('div[id^="table"]').each(function () {
			var tableID = $(this).attr("id");
			var tableNum = tableID.substr(5, tableID.length);
			console.log("tableID: " + tableID);
			console.log("tableNum: " + tableNum);
			if (parseInt(tableVal) < parseInt(tableNum)) {
				console.log(tableVal + " < " + tableNum);
				$(this).hide();
			}
			else {
				console.log(tableVal + " > " + tableNum);
			}
		});

		var myDiv = $(".colTwoSublocations");
		var table = $(this).closest('table');

		////delete all children tables
		//var div = $(this).closest('div');
		//var divid = $(this).closest('div').prop("id");
		//console.log("div: " + div);
		//console.log("divid: " + divid);

		//if (divid != "") {
		//	var nextDiv = div.nextAll(".sublocation-index");
		//	var nextDivID = nextDiv.prop("id");
		//	console.log("nextdiv: " + nextDiv);
		//	console.log("nextdivid: " + nextDivID);

		//	nextDiv.html("");
		//	//while (nextDiv != null) {
		//	//	nextDiv = div.next(".sublocation-index");
		//	//	//nextDiv.html("");
		//	//}
		//}

		//Begin CSS Styling
		var stylingClass = "lab-man-50-background-color";
		//$("body td").removeClass(stylingClass);

		//$(table + " td").removeClass(stylingClass);
		//$(table + " td").removeClass(stylingClass);
		table.children('tbody').children('tr').children('td').removeClass(stylingClass);
		$(this).parent().addClass(stylingClass);

		//$("." + stylingClass).addClass(stylingClass);

		var parentStylingClass = "parent-location-selected-outer-lab-man";
		if ($(this).hasClass("parent-location")) {
			//console.log("is parent location!");
			$("body td").removeClass(parentStylingClass);
			$(this).parent().addClass(parentStylingClass);

		}
		else {
			//console.log("is not parent location");
		}
		//End CSS Styling

		//fill up col 2 with the next one
		var parentId = $(this).val();
		console.log("open sublocation view, index: " + parentId);

		var parentsParentId = $(this).closest('tr').attr('name');

		if ($("#colTwoSublocations" + parentsParentId).length == 0) {
			$('.colTwoSublocations').append('<div class="colTwoSublocationsChildren" id="colTwoSublocations' + parentsParentId + '"></div>');
		}
		//console.log("about to call ajax with a parentid of: " + parentId);
		$.ajax({
			//IMPORTANT: ADD IN THE ID
			url: "/Locations/SublocationIndex/?parentId=" + parentId,
			type: 'GET',
			cache: false,
			context: $("#colTwoSublocations" + parentsParentId),
			success: function (result) {
				myDiv.show();
				$("#loading1").hide();
				$("#loading1")/*.delay(1000)*/.hide(0);
				this.html(result);

			}
		});

		$.fn.setUpVisual($(this).val());


	});

	$.fn.setUpVisual = function (val) {
		$("#loading2")/*.delay(1000)*/.show(0);
		console.log("in set up visual");
		//fill up col three with the visual
		var visualDiv = $(".VisualBoxColumn");
		var visualContainerId = val;
		//console.log("about to call ajax with a visual container id of: " + visualContainerId);
		$.ajax({
			url: "/Locations/VisualLocations/?VisualContainerId=" + visualContainerId,
			type: 'GET',
			cache: true,
			context: visualDiv,
			success: function (result) {
				visualDiv.show();
				this.html(result);
				$("#loading2").hide();
				$("#loading2")/*.delay(1000)*/.hide(0);
			}
		});
	};

	$("#UserImage").on("change", function () {
		var imgPath = $("#UserImage")[0].value;
		console.log("imgPath: " + imgPath);
		var extn = imgPath.substring(imgPath.lastIndexOf('.') + 1).toLowerCase();
		console.log("extn: " + extn);
		var imageHolder = $("#user-image");
		imageHolder.empty();

		if (extn == "gif" || extn == "png" || extn == "jpg" || extn == "jpeg") {
			console.log("inside the if statement");
			if (typeof (FileReader) != "undefined") {
				console.log("file reader does not equal undefined");
				var reader = new FileReader();
				reader.onload = function (e) {
					console.log(e.target.result);
					//$("<img />", {
					//	"src": e.target.result,
					//	"class": "thumb-image"
					//}).appendTo(imageHolder);
					$("#user-image").attr("src", e.target.result);
				}
				imageHolder.show();
				reader.readAsDataURL($(this)[0].files[0]);
			}
		}
		else {
			alert("Please only select images");
		}
	});

	$("#InvoiceImage").on("change", function () {
		var imgPath = $("#InvoiceImage")[0].value;
		console.log("imgPath: " + imgPath);
		var extn = imgPath.substring(imgPath.lastIndexOf('.') + 1).toLowerCase();
		console.log("extn: " + extn);
		var imageHolder = $("#invoice-image");
		imageHolder.empty();

		if (extn == "gif" || extn == "png" || extn == "jpg" || extn == "jpeg") {
			console.log("inside the if statement");
			if (typeof (FileReader) != "undefined") {
				console.log("file reader does not equal undefined");
				var reader = new FileReader();
				reader.onload = function (e) {
					//console.log(e.target.result);
					//$("<img />", {
					//	"src": e.target.result,
					//	"class": "thumb-image"
					//}).appendTo(imageHolder);
					$("#invoive-image").attr("src", e.target.result);
				}
				imageHolder.show();
				reader.readAsDataURL($(this)[0].files[0]);
			}
		}
		else {
			alert("Please only select images");
		}
	});


	$.fn.validateItemTab = function () {
		$(".request-price-tab").prop("disabled", true);
		$(".request-location-tab").prop("disabled", true);
		$(".request-order-tab").prop("disabled", true);

		console.log("in $.fn.validateItemTab");
		valid = $("#parentlist").attr('aria-invalid');
		if (valid == "true" || $("#parentlist").val() == "") {
			console.log("valid: " + valid);
			return;
		}
		valid = $("#Request_Product_ProductName").attr('aria-invalid');
		if (valid == "true" || $("#Request_Product_ProductName").val() == "") {
			console.log("valid: " + valid);
			return;
		}
		valid = $("#sublist").attr('aria-invalid');
		if (valid == "true" || $("#sublist").val() == "") {
			return;
		}
		valid = $("#Request_SubProject_ProjectID").attr('aria-invalid');
		if (valid == "true" || $("#Request_SubProject_ProjectID").val() == "") {
			return;
		}
		console.log("valid1: " + valid);
		valid = $("#SubProject").attr('aria-invalid');
		if (valid == "true" || $("#SubProject").val() == "") {
			return;
		}
		valid = $("#vendorList").attr('aria-invalid');
		if (valid == "true" || $("#vendorList").val() == "") {
			return;
		}
		valid = $("#Request_Warranty").attr('aria-invalid');
		if (valid == "true") {
			return;
		}
		valid = $("#Request_ParentQuote_QuoteDate").attr('aria-invalid');
		if (valid == "true" || $("#Request_ParentQuote_QuoteDate").val() == "") {
			return;
		}
		valid = $("#Request_ParentQuote_QuoteNumber").attr('aria-invalid');
		if (valid == "true" || $("#Request_ParentQuote_QuoteNumber").val() == "") {
			return;
		}
		valid = $("#Request_ExpectedSupplyDays").attr('aria-invalid');
		if (valid == "true") {
			return;
		}
		valid = $("#Request_CatalogNumber").attr('aria-invalid');
		if (valid == "true" || $("#Request_CatalogNumber").val() == "") {
			return;
		}
		if (valid == "false" || valid == undefined) {
			console.log("made it here!!")
			$(".request-price-tab").prop("disabled", false);
			$(".request-location-tab").prop("disabled", false);
			$(".request-order-tab").prop("disabled", false);
		}
		return valid;
	}

	$('#myModal').change(function () {
		$.validator.unobtrusive.parse("#myForm");
	});

	$("#saveEditModal").click(function (e) {


	});

	$.fn.validateDateisGreaterThanOrEqualToToday = function (date) {
		var tdate = new Date();
		var dd = tdate.getDate(); //yields day
		var MM = tdate.getMonth(); //yields month
		var yyyy = tdate.getFullYear(); //yields year
		if (dd < 10) {
			dd = "0" + dd
		}
		if (MM < 10) {
			MM = "0" + (MM + 1);
		} else {
			MM = MM + 1;
		}
		var today = yyyy + "-" + (MM) + "-" + dd;
		console.log("today:" + today);
		//count how many installment dates already passed
		if (date < today) {
			return false;
		}
		return true;
	};

	$(".request-price-tab").click(function () {
		console.log("in onclick price tab");
		$("#myForm").valid();
		$.fn.validateItemTab();

	});
	$(".create-modal-submit").click(function (e) {
		console.log('$("#myForm").valid()');
		if ($("#myForm").valid()) {
			$(".create-modal-submit").removeClass("disabled-submit");
			return true;
		} else {
			e.preventDefault();
			e.stopPropagation();
			$(".create-modal-submit").addClass("disabled-submit");
			return false;
		}

	});


	$(".request-archive-tab").click(function () {
		console.log("in onclick archive-tab");
		$("#myForm").valid();
		$.fn.validatePriceTab();

	});

	$(".request-history-tab").click(function () {
		console.log("in onclick history-tab");
		$("#myForm").valid();
		$.fn.validateItemTab();
		$.fn.validatePriceTab();

	});

	$(".request-order-tab").click(function () {
		console.log("in onclick order-tab");
		$("#myForm").valid();
		var valid = $.fn.validateItemTab();
		if (valid == "false" || valid == undefined) {
			$.fn.validatePriceTab();
		};

	});

	$(".request-comments-tab").click(function () {
		console.log("in onclick comments-tab");
		$("#myForm").valid();
		$.fn.validatePriceTab();
	});

	$(".request-location-tab").click(function () {
		console.log("in onclick location-tab");
		$("#myForm").valid();
		$.fn.validateItemTab();
	});


	$("#Request_Terms").change(function () {
		console.log("in change Request_Terms");
		if ($(this).val() == -1) {
			$(".installments").hide();
		} else {
			$(".installments").show();
		}

	});

	$.fn.validatePriceTab = function () {
		//all the true and falses are opposite because fo the ariainvalid is true if invalid
		$(".request-documents-tab").prop("disabled", true);
		$(".request-comments-tab").prop("disabled", true);
		$(".request-archive-tab").prop("disabled", true);
		$(".request-history-tab").prop("disabled", true);
		$(".request-order-tab").prop("disabled", true);
		valid = $("#Request_Product_ProductName").attr('aria-invalid');
		if (valid == "true" || $("#Request_Product_ProductName").val() == "") {
			return;
		}
		valid = $("#Request_Unit").attr('aria-invalid');
		if (valid == "true" || $("#Request_Unit").val() == "") {
			return;
		}
		valid = $("#Request_Unit").attr('aria-invalid');
		if (valid == "true" || $("#Request_Unit").val() == "") {
			return;
		}

		//TALK TO DEBBIE ABOUT THE NEW MATERIAL SELECT
		console.log("valid2: " + valid);
		valid = $("#Request_ExchangeRate").attr('aria-invalid');
		if (valid == "true" || $("#Request_ExchangeRate").val() == "") {
			return;
		}
		console.log("valid3: " + valid);
		valid = $("#Request_Cost").attr('aria-invalid');
		if (valid == "true" || $("#Request_Cost").val() == "") {
			return;
		}
		console.log("final valid: " + valid);
		if ($("#Request_Terms").val() != -1) {
			console.log(".paymentType: " + $(".paymentType").val());
			if ($(".paymentType").val() != "") {
				valid = "false";
			} else {
				valid = "true";
			}
		}

		if (valid == "false" || valid == undefined) {
			$(".request-item-tab").prop("disabled", false);
			$(".request-documents-tab").prop("disabled", false);
			$(".request-comments-tab").prop("disabled", false);
			$(".request-archive-tab").prop("disabled", false);
			$(".request-history-tab").prop("disabled", false);
			$(".request-order-tab").prop("disabled", false);
		}
		return valid;
	}

	$(".create-user .permissions-tab").on("click", function () {
		console.log("permissions tab opened");
		$.fn.HideAllPermissionsDivs();
		$.fn.ChangeUserPermissionsButtons();
	});

	$(".edit-user .permissions-tab").on("click", function () {
		console.log("permissions tab opened");
		$.fn.HideAllPermissionsDivs();
		$.fn.ChangeUserPermissionsButtons();
	});




	$.fn.HideAllPermissionsDivs = function () {
		console.log("hide all permissions function entered");
		$(".orders-list").hide();
		$(".protocols-list").hide();
		$(".operations-list").hide();
		$(".biomarkers-list").hide();
		$(".timekeeper-list").hide();
		$(".lab-list").hide();
		$(".accounting-list").hide();
		$(".expenses-list").hide();
		$(".income-list").hide();
		$(".users-list").hide();
	};

	$(".back-to-main-permissions").on("click", function (e) {
		console.log("back to main permissions clicked");
		e.preventDefault();
		e.stopPropagation();

		$.fn.ChangeUserPermissionsButtons();

		$.fn.HideAllPermissionsDivs();
		$(".main-permissions").show();
	});

	$.fn.ChangeUserPermissionsButtons = function () {
		var checks = $("input[type='checkbox']:checked");
		var checkTypes = [];
		for (var x = 0; x < checks.length; x++) {
			var name = $(checks[x]).attr("name");
			var bracket = name.indexOf('[');
			var type = name.substring(0, bracket);
			checkTypes.push(type);
			console.log("type: " + type);

		}

		if (checkTypes.indexOf("OrderRoles") > -1) {
			$(".orders-main").show();
			$(".orders-grey").hide();
		}
		else {
			$(".orders-main").hide();
			$(".orders-grey").show();
		}
		if (checkTypes.indexOf("ProtocolRoles") > -1) {
			$(".protocols-main").show();
			$(".protocols-grey").hide();
		}
		else {
			$(".protocols-main").hide();
			$(".protocols-grey").show();
		}
		if (checkTypes.indexOf("OperationRoles") > -1) {
			$(".operations-main").show();
			$(".operations-grey").hide();
		}
		else {
			$(".operations-main").hide();
			$(".operations-grey").show();
		}
		if (checkTypes.indexOf("BiomarkerRoles") > -1) {
			$(".biomarkers-main").show();
			$(".biomarkers-grey").hide();
		}
		else {
			$(".biomarkers-main").hide();
			$(".biomarkers-grey").show();
		}
		if (checkTypes.indexOf("TimekeeperRoles") > -1) {
			$(".timekeeper-main").show();
			$(".timekeeper-grey").hide();
		}
		else {
			$(".timekeeper-main").hide();
			$(".timekeeper-grey").show();
		}
		if (checkTypes.indexOf("LabManagementRoles") > -1) {
			$(".lab-main").show();
			$(".lab-grey").hide();
		}
		else {
			$(".lab-main").hide();
			$(".lab-grey").show();
		}
		if (checkTypes.indexOf("AccountingRoles") > -1) {
			$(".accounting-main").show();
			$(".accounting-grey").hide();
		}
		else {
			$(".accounting-main").hide();
			$(".accounting-grey").show();
		}
		if (checkTypes.indexOf("ExpenseesRoles") > -1) {
			$(".expenses-main").show();
			$(".expenses-grey").hide();
		}
		else {
			$(".expenses-main").hide();
			$(".expenses-grey").show();
		}
		if (checkTypes.indexOf("IncomeRoles") > -1) {
			$(".income-main").show();
			$(".income-grey").hide();
		}
		else {
			$(".income-main").hide();
			$(".income-grey").show();
		}
		if (checkTypes.indexOf("UserRoles") > -1) {
			$(".users-main").show();
			$(".users-grey").hide();
		}
		else {
			$(".users-main").hide();
			$(".users-grey").show();
		}
	};

	$(".open-orders-list").on("click", function (e) {
		console.log("open orders lsit clicked");
		e.preventDefault();
		e.stopPropagation();
		$(".main-permissions").hide();
		$(".orders-list").show();
	});

	$(".open-protocols-list").on("click", function (e) {
		console.log("open protocols lsit clicked");
		e.preventDefault();
		e.stopPropagation();
		$(".main-permissions").hide();
		$(".protocols-list").show();
	});

	$(".open-operations-list").on("click", function (e) {
		console.log("open operations lsit clicked");
		e.preventDefault();
		e.stopPropagation();
		$(".main-permissions").hide();
		$(".operations-list").show();
	});

	$(".open-biomarkers-list").on("click", function (e) {
		console.log("open biomarkers lsit clicked");
		e.preventDefault();
		e.stopPropagation();
		$(".main-permissions").hide();
		$(".biomarkers-list").show();
	});

	$(".open-timekeepers-list").on("click", function (e) {
		console.log("open timekeeper lsit clicked");
		e.preventDefault();
		e.stopPropagation();
		$(".main-permissions").hide();
		$(".timekeeper-list").show();
	});

	$(".open-lab-list").on("click", function (e) {
		console.log("open lab lsit clicked");
		e.preventDefault();
		e.stopPropagation();
		$(".main-permissions").hide();
		$(".lab-list").show();
	});

	$(".open-accounting-list").on("click", function (e) {
		console.log("open accounting lsit clicked");
		e.preventDefault();
		e.stopPropagation();
		$(".main-permissions").hide();
		$(".accounting-list").show();
	});

	$(".open-expenses-list").on("click", function (e) {
		console.log("open expenses lsit clicked");
		e.preventDefault();
		e.stopPropagation();
		$(".main-permissions").hide();
		$(".expenses-list").show();
	});

	$(".open-income-list").on("click", function (e) {
		console.log("open income lsit clicked");
		e.preventDefault();
		e.stopPropagation();
		$(".main-permissions").hide();
		$(".income-list").show();
	});

	$(".open-users-list").on("click", function (e) {
		console.log("open users lsit clicked");
		e.preventDefault();
		e.stopPropagation();
		$(".main-permissions").hide();
		$(".users-list").show();
	});

	$("#vendor-payments-tab").click(function () {
		console.log("$('#vendor - payments - tab')");
		$("#createModalForm").validate();
		$("#createModalForm").valid();
		$.fn.validateVendorDetailsTab();
	});
	$("#vendor-categories-tab").click(function () {
		console.log("$('#vendor - payments - tab')");
		$("#createModalForm").valid();
		$.fn.validateVendorDetailsTab();
	});
	$("#vendor-comment-tab").click(function () {
		$("#createModalForm").valid();
		$.fn.validateVendorPayment();
	});
	$("#vendor-contact-tab").click(function () {
		$('[data-toggle="popover"]').popover('hide');
		$("#createModalForm").valid();
		$.fn.validateVendorPayment();
	});
	$("#addSupplier").click(function () {
		console.log('$("#createModalForm").valid()');
		if ($("#createModalForm").valid()) {
			$("#addSupplier").removeClass("disabled-submit");
			return true;
		} else {
			e.preventDefault();
			e.stopPropagation();
			$("#addSupplier").addClass("disabled-submit");
			return false;
		}
	});

	$.fn.validateVendorDetailsTab = function () {
		//all the true and falses are opposite because fo the ariainvalid is true if invalid
		valid = $("#Vendor_VendorEnName").attr('aria-invalid');
		console.log("valid: " + valid);
		if (valid == "true" || $("#Vendor_VendorEnName").val() == "") {
			console.log("valid: " + valid);
			return;
		}
		valid = $("#Vendor_VendorHeName").attr('aria-invalid');
		console.log("valid: " + valid);
		if (valid == "true" || $("#Vendor_VendorHeName").val() == "") {
			console.log("valid: " + valid);
			return;
		}
		console.log("valid1: " + valid);
		valid = $("#Vendor_VendorBuisnessID").attr('aria-invalid');
		if (valid == "true" || $("#Vendor_VendorBuisnessID").val() == "") {
			return;
		}
		console.log("valid2: " + valid);
		valid = $("#Vendor_VendorCountry").attr('aria-invalid');
		if (valid == "true" || $("#Vendor_VendorCountry").val() == "") {
			return;
		}
		console.log("valid3: " + valid);
		valid = $("#Vendor_VendorCity").attr('aria-invalid');
		if (valid == "true" || $("#Vendor_VendorCity").val() == "") {
			return;
		}
		console.log("valid1: " + valid);
		valid = $("#Vendor_VendorStreet").attr('aria-invalid');
		if (valid == "true" || $("#Vendor_VendorStreet").val() == "") {
			return;
		}
		console.log("valid2: " + valid);
		valid = $("#Vendor_VendorTelephone").attr('aria-invalid');
		if (valid == "true" || $("#Vendor_VendorTelephone").val() == "") {
			return;
		}
		console.log("valid3: " + valid);
		valid = $("#Vendor_VendorFax").attr('aria-invalid');
		if (valid == "true") {
			return;
		}
		valid = $("#VendorCategoryTypes").attr('aria-invalid');
		if (valid == "true" || $("#VendorCategoryTypes").val() == "") {
			return;
		}
		console.log("valid1: " + valid);
		valid = $("#Vendor_OrdersEmail").attr('aria-invalid');
		if (valid == "true" || $("#Vendor_OrdersEmail").val() == "") {
			return;
		}
		console.log("valid2: " + valid);
		valid = $("#Vendor_InfoEmail").attr('aria-invalid');
		if (valid == "true") {
			return;
		}
		console.log("valid3: " + valid);
		valid = $("#Vendor_VendorCellPhone").attr('aria-invalid');
		if (valid == "true") {
			return;
		}
		if (valid == "false" || valid == undefined) {
			$("#vendor-payments-tab").prop("disabled", false);
			$("#vendor-categories-tab").prop("disabled", false);
		}
		return valid;
	}
	$.fn.validateVendorPayment = function () {
		//all the true and falses are opposite because fo the ariainvalid is true if invalid
		valid = $("#Vendor_VendorBank").attr('aria-invalid');
		console.log("valid: " + valid);
		if (valid == "true" || $("#Vendor_VendorBank").val() == "") {
			console.log("valid: " + valid);
			return;
		}
		valid = $("#Vendor_VendorBankBranch").attr('aria-invalid');
		console.log("valid: " + valid);
		if (valid == "true" || $("#Vendor_VendorBankBranch").val() == "") {
			console.log("valid: " + valid);
			return;
		}
		console.log("valid1: " + valid);
		valid = $("#Vendor_VendorAccountNum").attr('aria-invalid');
		if (valid == "true" || $("#Vendor_VendorAccountNum").val() == "") {
			return;
		}

		if (valid == "false" || valid == undefined) {
			$("#vendor-comment-tab").prop("disabled", false);
			$("#vendor-contact-tab").prop("disabled", false);
		}
		return valid;
	}


	$(".vendor-contact").change(function () {
		if ($("#createModalForm").valid()) {
			$("#addSupplier").prop("disabled", false);
			$("#addSupplier").removeClass("disabled-submit");
		}
	});

	$("#vendor-contact-tab").click(function () {
		console.log("in onclickcontact");
		$(".contact-info:hidden:first").find(".contact-active").val(true);
		$(".contact-info:hidden:first").show();
	});

	$("#addSuplierContact").click(function () {
		console.log("in onclick addSuplierContact");
		$(".contact-info:hidden:first").find(".contact-active").val(true);
		$(".contact-info:hidden:first").show();
	});



	$("#addUser").click(function () {
		console.log($("#createModalForm").valid());
		if (!$("#createModalForm").valid()) {
			e.preventDefault();
			e.stopPropagation();
		}
	});




	$.fn.addComment = function (type) {
		console.log("$('#Comment').click");
		$(".comment-info:hidden:first").find(".comment-active").val(true);
		$(".comment-info:hidden:first").find(".comment-type").val(type);
		console.log(type);
		if (type === "Comment") {
			$(".comment-info:hidden:first i").addClass("icon-comment-24px ");
			$(".comment-info:hidden:first i").css("color", "#30BCC9");
		} else if (type === "Warning") {
			$(".comment-info:hidden:first i").addClass("icon-report_problem-24px-2");
			$(".comment-info:hidden:first i").css("color", "var(--danger-color)");
		}


		$(".comment-info:hidden:first").show();
	}

	$("#user-permissions-tab").click(function () {
		$("#createModalForm").valid();
		$.fn.validateUserDetailsTab();
	});

	$("#user-budget-tab").click(function () {
		$("#createModalForm").valid();
	});

	$("#user-more-tab").click(function () {
		console.log('"#user-more-tab").click');
		$("#createModalForm").valid();
		$("#addUser").prop("disabled", false);
		$("#addUser").removeClass("disabled-submit");
	});

	$.fn.validateUserDetailsTab = function () {
		//all the true and falses are opposite because fo the ariainvalid is true if invalid
		valid = $("#FirstName").attr('aria-invalid');
		if (valid == "true" || $("#FirstName").val() == "") {
			console.log("valid: " + valid);
			return;
		}
		valid = $("#LastName").attr('aria-invalid');
		if (valid == "true" || $("#LastName").val() == "") {
			return;
		}
		valid = $("#Email").attr('aria-invalid');
		if (valid == "true" || $("#Email").val() == "") {
			console.log("valid: " + valid);
			return;
		}
		valid = $("#SecureAppPass").attr('aria-invalid');
		if (valid == "true" /*|| $("#SecureAppPass").val() == ""*/) {
			return;
		}
		valid = $("#CentarixID").attr('aria-invalid');
		if (valid == "true" || $("#CentarixID").val() == "") {
			return;
		}
		valid = $("#Password").attr('aria-invalid');
		if (valid == "true" /*|| $("#Password").val() == ""*/) {
			return;
		}
		valid = $("#ConfirmPassword").attr('aria-invalid');
		if (valid == "true" /*|| $("#ConfirmPassword").val() == ""*/) {
			return;
		}
		if (!$('input[name="NewEmployee.EmployeeStatusID"]:checked').length) {
			// none of the radio buttons were checked
			return;
		}
		if (valid == "false" || valid == undefined) {
			$("#user-permissions-tab").prop("disabled", false);
			$("#user-budget-tab").prop("disabled", false);
			$("#user-more-tab").prop("disabled", false);
		}
		return valid;
	}


	$("#SaveInvoiceModal").on("click", function (e) {
		$("#myForm").valid();
		var valid = $("#Invoice_InvoiceNumber").attr("aria-invalid");
		console.log("invoice number validation + " + valid);
		if (valid == "true" || $("#Invoice_InvoiceNumber") == "") {
			e.preventDefault();
			e.stopPropagation();
			$("#invoice-number-validation").html("Please enter a valid Number");
		}
	})


	$("#reorderRequest").click(function () {
		console.log($("#reorderForm").valid());
		if (!$("#reorderForm").valid()) {
			$("#reorderRequest").prop("disabled", true);

		}
		$("#reorderRequest").prop("disabled", false);

	});



	/*--------------------------------Accounting Payment Notifications--------------------------------*/
	$(".payments-pay-now").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var vendorid = $(this).attr("vendor");
		var paymentstatusid = $(this).attr("paymentstatus");
		console.log("vendor: " + vendorid);
		console.log("payment status: " + paymentstatusid);
		//var $itemurl = "Requests/TermsModal/?id=" + @TempData["RequestID"] + "&isSingleRequest=true"
		var itemurl = "/Requests/PaymentsPayModal/?vendorid=" + vendorid + "&paymentstatusid=" + paymentstatusid;
		$("#loading").show();
		$.fn.CallModal(itemurl);
	});

	$(".invoice-add-all").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var vendorid = $(this).attr("vendor");
		var itemUrl = "/Requests/AddInvoiceModal/?vendorid=" + vendorid;
		$("#loading").show();
		$.fn.CallModal(itemUrl);
	});

	$("#add-to-selected").off("click").on("click", function (e) {
		var arrayOfSelected = $(".form-check.accounting-select .form-check-input:checked").map(function () {
			return $(this).attr("id")
		}).get()
		console.log("arrayOfSelected: " + arrayOfSelected);
		//var itemUrl = "/Requests/AddInvoiceModal/?requestids=" + arrayOfSelected;
		$("#loading").show();
		$('.modal').replaceWith('');
		$(".modal-backdrop").remove();
		$.ajax({
			type: "GET",
			url: "/Requests/AddInvoiceModal/",
			traditional: true,
			data: { 'requestIds': arrayOfSelected },
			cache: true,
			success: function (data) {
				$("#loading").hide();
				console.log("data:");
				console.log(data);
				var modal = $(data);
				$('body').append(modal);
				//replaces the modal-view class with the ModalView view
				//$(".modal-view").html(data);
				//turn off data dismiss by clicking out of the box and by pressing esc
				$(".modal-view").modal({
					backdrop: true,
					keyboard: false,
				});
				//shows the modal
				$(".modal").modal('show');
			}
		});
		//$.fn.CallModal(itemUrl);
	});

	$(".invoice-add-one").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var requestid = $(this).attr("request");
		var itemUrl = "/Requests/AddInvoiceModal/?requestid=" + requestid;
		$("#loading").show();
		$.fn.CallModal(itemUrl);
	});

	$.fn.CallModal = function (url) {
		console.log("in call modal, url: " + url);
		$('.modal').replaceWith('');
		$(".modal-backdrop").remove();
		$.ajax({
			async: false,
			url: url,
			type: 'GET',
			cache: false,
			success: function (data) {
				$("#loading").hide();
				$("#loading").hide();
				var modal = $(data);
				$('body').append(modal);
				//replaces the modal-view class with the ModalView view
				//$(".modal-view").html(data);
				//turn off data dismiss by clicking out of the box and by pressing esc
				$(".modal-view").modal({
					backdrop: true,
					keyboard: false,
				});
				//shows the modal
				$(".modal").modal('show');
				return false;
			}
		});
	};



	$(".remove-invoice-item").off("click").on("click", function (e) {
		e.stopPropagation();
		e.preventDefault();
	});



	$("#share-payment").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		console.log("in share payments site.js");
	});

	function SharePayment(e) {
		e.preventDefault();
		e.stopPropagation();
		console.log("in share payments fx site.js");
	};

	$("body").on("click", "#share-payment", function (e) {
		e.preventDefault();
		e.stopPropagation();
		console.log("in share payments body fx site.js");
	});

	$("#entry").dblclick(function () {
		console.log("in entry")
		$('#myForm').trigger('submit');
	});
	$("#exit").dblclick(function () {
		console.log("in exit")
		$.fn.GetEmployeeHourFromToday();
		return false;
	});
	$("#entry").click(function (e) {
		e.preventDefault();
	});
	$("#exit").click(function (e) {
		e.preventDefault();
	});
	$('.monthsHours .select-dropdown').change(function (e) {
		console.log(".monthsHours chnage")
		if ($(this).val() != '') {
			$.fn.SortByMonth($(this).val())
		}
	});
	$.fn.SortByMonth = function (month) {
		$.ajax({
			async: false,
			url: 'HoursPage?month=' + month,
			type: 'GET',
			cache: false,
			success: function (data) {
				$("#hoursTable").html(data);
			}
		});
	};
	$(".open-work-from-home-modal").click(function (e) {
		var itemurl = "ReportHoursFromHomeModal";
		$("#loading").show();
		$.fn.CallModal(itemurl);
	});
	$(".open-update-hours-modal").click(function (e) {
		var val = $(this).val();
		if (val != '') {
			var date = new Date(val).toISOString();
			console.log(date)
		}
		var itemurl = "UpdateHours?chosenDate=" + date;
		$("#loading").show();
		$.fn.CallModal(itemurl);
	});

	$(".report-vacation-days").click(function (e) {
		var itemurl = "Vacation"
		$("#loading").show();
		$.fn.CallModal(itemurl);
	});

	$(".report-sick-days").click(function (e) {
		var itemurl = "SickDay"
		$("#loading").show();
		$.fn.CallModal(itemurl);
	});


	$("body").on("change", "#Date", function (e) {
		$('.day-of-week').val($.fn.GetDayOfWeek($(this).val()));
	});

	$.fn.GetDayOfWeek = function (date) {
		var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
		var dayNum = new Date(date).getDay();
		console.log("daynum" + dayNum)
		var dayOfWeek = days[dayNum];
		return dayOfWeek
	}

	$("body").on("change", "#Date.update-hour-date", function (e) {
		$.fn.GetEmployeeHour($(this).val());
	});
	$("body").on("change", "#Date.update-work-from-home", function (e) {
		$.fn.GetEmployeeHourFromHome($(this).val());
	});

	$.fn.GetEmployeeHour = function (date) {
		console.log(date);
		$.fn.CallModal('UpdateHours?chosenDate=' + new Date(date).toISOString())
	};


	//RECEIVEDMODAL:
	function changePartialDelivery() {
		console.log("in partial delivery, checkbox val: " + $(this).val());
	};

	function changeClarify() {
		console.log("in clarify, checkbox val: " + $(this).val());
	};

	$.fn.GetEmployeeHourFromHome = function (date) {
		console.log(date);
		$.fn.CallModal('ReportHoursFromHomeModal?chosenDate=' + new Date(date).toISOString())
	};
	$.fn.GetEmployeeHourFromToday = function () {
		$.fn.CallModal('ExitModal');
	};



	//DROPDOWN
	/*Dropdown Menu*/
	$('.dropdown-main').off("click").on("click", function () {
		$(this).attr('tabindex', 1).focus();
		//$(this).toggleClass('active');
		$(this).find('.dropdown-menu').slideToggle(300);
	});
	$('.dropdown-main').focusout(function () {
		$(this).removeClass('active');
		$(this).find('.dropdown-menu').slideUp(300);
	});
	$('.dropdown-main .dropdown-menu li').click(function () {
		$(this).parents('.dropdown-main').find('span').text($(this).text());
		$(this).parents('.dropdown-main').find('input').attr('value', $(this).attr('id'));
	});
	/*End Dropdown Menu*/


	$('.dropdown-menu li').click(function () {
		var input = '<strong>' + $(this).parents('.dropdown').find('input').val() + '</strong>',
			msg = '<span class="msg">Hidden input value: ';
		$('.msg').html(msg + input + '</span>');
	});



	$('.dropdown-multiple').click(function () {
		$(this).attr('tabindex', 1).focus();
		$(this).addClass('active');
		$(this).find('.dropdown-menu-multiple').slideToggle(300);
	});
	$('.dropdown-multiple').focusout(function () {
		$(this).removeClass('active');
		$(this).find('.dropdown-menu-multiple').slideUp(300);
	});
	$('.dropdown-multiple .dropdown-menu div label').click(function () {
		//$(this).parents('.dropdown').find('span').text($(this).text());
		$(this).parents('.dropdown-multiple').find('input').attr('value', $(this).attr('id'));
		$(this).parents('.dropdown-multiple').addClass('active');
	});
	/*End Dropdown Menu*/


	//$('.dropdown-menu li').click(function () {
	//	var input = '<strong>' + $(this).parents('.dropdown').find('input').val() + '</strong>',
	//		msg = '<span class="msg">Hidden input value: ';
	//	$('.msg').html(msg + input + '</span>');
	//}); 

	$("body").on("change", "#TotalHours", function (e) {
		$('#Exit1').val('');
		$('#Entry1').val('');
		$('#Exit2').val('');
		$('#Entry2').val('');
	});

	$("body").on("change", "#Exit1", function (e) {
		$('#TotalHours').val('');
	});
	$("body").on("change", "#Entry1", function (e) {
		$('#TotalHours').val('');
	});
	$("body").on("change", "#Exit2", function (e) {
		$('#TotalHours').val('');
	});
	$("body").on("change", "#Entry2", function (e) {
		$('#TotalHours').val('');
	});
	//});
	/*End Dropdown Menu*/


	//$('.dropdown-menu li').click(function () {
	//	var input = '<strong>' + $(this).parents('.dropdown').find('input').val() + '</strong>',
	//		msg = '<span class="msg">Hidden input value: ';
	//	$('.msg').html(msg + input + '</span>');
	//}); 
	$.fn.SaveOffDays = function (url) {
		var rangeFrom = $('.datepicker--cell.-selected-.-range-from-');
		var rangeTo = $('.datepicker--cell.-selected-.-range-to-');
		var dateRangeFromDay = rangeFrom.attr('data-date');
		var dateRangeFromMonth = rangeFrom.attr('data-month');
		var dateRangeFromYear = rangeFrom.attr('data-year');
		var dateRangeToDay = rangeTo.attr('data-date');
		var dateRangeToMonth = rangeTo.attr('data-month');
		var dateRangeToYear = rangeTo.attr('data-year');
		var dateFrom = new Date(dateRangeFromYear, dateRangeFromMonth, dateRangeFromDay, 0, 0, 0).toISOString();
		var dateTo = '';
		if (dateRangeToDay == undefined) {
			dateTo = null;
		}
		else {
			dateTo = new Date(dateRangeToYear, dateRangeToMonth, dateRangeToDay, 0, 0, 0).toISOString();
		}

		console.log(dateFrom + "-" + dateTo);
		$.ajax({
			async: false,
			url: url + '?dateFrom=' + dateFrom + "&dateTo=" + dateTo,
			type: 'POST',
			cache: false,
			success: function (data) {
				$(".modal").modal('hide');
			}
		});
	}

	$("body").on("click", "#saveVacation", function (e) {
		e.preventDefault();
		$.fn.SaveOffDays("SaveVacation");
	});
	$("body").on("click", "#saveSick", function (e) {
		e.preventDefault();
		$.fn.SaveOffDays("SaveSick");
	});

	$(".approve-hours").click(function (e) {
		$.ajax({
			async: true,
			url: "ApproveHours" + '?id=' + $(this).val(),
			type: 'GET',
			cache: false,
			success: function (data) {
				$("body").html(data);
			}
		});
	});
	//$('.employee-type').change(function () {
	//	console.log($(this).val());
	//	$('#NewEmployee_EmployeeStatusID').val($(this).val())
	//})

	$('.salary-tab').off("click").on("click", function (e) {
		$("#createModalForm").valid();
		console.log("salary tab clicked");
		$.fn.validatePersonalTab();
	});
	$.fn.validatePersonalTab = function () {
		console.log("validatePersonalTab");
		$(".salary-tab").prop("disabled", true);
		$(".documents-tab").prop("disabled", true);


		var statIdVal = parseInt($("#NewEmployee_EmployeeStatusID").val());
		console.log("vpt statIdVal: " + statIdVal);
		if (parseInt(statIdVal) == 1 || statIdVal == 2 || statIdVal == 3) {
			console.log("inside if statement");
			//if (statIdVal != 4) {
			//	console.log("statIdVal: " + statIdVal + " entered if statement");
			if (/*checkedItem1 != true && checkedItem2 != true && checkedItem3 != true && checkedItem4 != true*/$('#NewEmployee_EmployeeStatusID').val() == '' || $('#NewEmployee_EmployeeStatusID').val() == 0) {
				$("#validation-EmployeeStatus").removeClass("hidden");
				return;
			}
			else {
				$("#validation-EmployeeStatus").addClass("hidden");
			}
			valid = $("#NewEmployee_IDNumber").attr('aria-invalid');
			if (valid == "true" || $("#NewEmployee_IDNumber").val() == "") {
				console.log("valid: " + valid);
				return;
			}
			valid = $("#NewEmployee_CentarixID").attr('aria-invalid');
			if (valid == "true" || $("#NewEmployee_CentarixID").val() == "") {
				console.log("valid: " + valid);
				return;
			}
			valid = $("#NewEmployee_JobTitle").attr('aria-invalid');
			if (valid == "true" || $("#NewEmployee_JobTitle").val() == "") {
				return;
			}
			valid = $("#NewEmployee_Email").attr('aria-invalid');
			if (valid == "true" || $("#NewEmployee_Email").val() == "") {
				return;
			}
			valid = $("#NewEmployee_PhoneNumber").attr('aria-invalid');
			if (valid == "true" || $("#NewEmployee_PhoneNumber").val() == "") {
				return;
			}
			valid = $("#NewEmployee_PhoneNumber").attr('aria-invalid');
			if (valid == "true") {
				return;
			}
			valid = $("#NewEmployee_StartedWorking").attr('aria-invalid');
			if (valid == "true" || $("#NewEmployee_StartedWorking").val() == "") {
				return;

			}
			if (valid == "false" || valid == undefined) {
				$(".salary-tab").prop("disabled", false);
				$(".security-tab").prop("disabled", false);
			}
		}
		else if (statIdVal == 4) {
			$(".salary-tab").prop("disabled", false);
		}
		return valid;
	}
	$(".security-tab").on("click", function () {
		if (parseInt($("#NewEmployee_EmployeeStatusID").val()) == 4) {
			$(".security-tab").prop("disabled", false);
		}
		else {
			$.fn.validateSalaryTab();
		}
	});
	$.fn.validateSalaryTab = function () {
		$(".security-tab").prop("disabled", true);
		console.log("in $.fn.validateItemTab");
		valid = $("#NewEmployee_SalariedEmployee_HoursPerDay").attr('aria-invalid');
		if (valid == "true" || $("#NewEmployee_SalariedEmployee_HoursPerDay").val() == "") {
			return;
		}
		valid = $("#NewEmployee_VacationDays").attr('aria-invalid');
		if (valid == "true" || $("#NewEmployee_VacationDays").val() == "") {
			return;
		}
		if (valid == "false" || valid == undefined) {
			$(".security-tab").prop("disabled", false);
		}
		return valid;
	};
	$('#addWorker').submit(function () {
		if ($("#createModalForm").valid()) {
			$("#addWorker").removeClass("disabled-submit");
			return true;
		} else {
			e.preventDefault();
			e.stopPropagation();
			$("#addWorker").addClass("disabled-submit");
			return false;
		}
	});
	$('.workersHours').change(function () {
		var url = $(this).val();
		if (url != null && url != '') {
			window.location.href = url;
		}
	});



	$('.turn-edit-on-off').change(function () {
		var type = $(this).attr('name');
		console.log(type);
		var url = '';
		if ($(this).hasClass('operations')) {
			url = "/Operations/EditModalView";
		} else if ($(this).hasClass('suppliers') || $(this).hasClass('accounting')) {
			url = "/Vendors/Edit";
		} else {
			url = "/Requests/EditModalView";

		}

		if (type == 'edit') {
			var formData = new FormData($("#myForm")[0]);
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
				}
			});

			$('.mark-readonly').prop("disabled", true);
			$('.mark-readonly input').prop("disabled", true);
			$('.mark-edditable').data("val", false)
			$('.edit-mode-switch-description').text("Edit Mode Off");
			$('.turn-edit-on-off').attr('name', 'details')

		}
		else if (type == 'details') {
			$('.mark-readonly').attr("disabled", false);
			$('.mark-edditable').data("val", true);
			$('.edit-mode-switch-description').text("Edit Mode On");
			$('.turn-edit-on-off').attr('name', 'edit')
			if ($(this).hasClass('operations') || $(this).hasClass('orders')) {
				console.log("orders operations")
				$.fn.EnableMaterialSelect('#parentlist', 'select-options-parentlist')
				$.fn.EnableMaterialSelect('#sublist', 'select-options-sublist')
				$.fn.EnableMaterialSelect('#vendorList', 'select-options-vendorList')
				$.fn.EnableMaterialSelect('#currency', 'select-options-currency')
			}
			if ($(this).hasClass('orders')) {
				console.log("orders")
				$.fn.EnableMaterialSelect('#Request_SubProject_ProjectID', 'select-options-Request_SubProject_ProjectID');
				$.fn.EnableMaterialSelect('#SubProject', 'select-options-SubProject');
				$.fn.EnableMaterialSelect('#Request_UnitTypeID', 'select-options-Request_UnitTypeID');
				$.fn.CheckUnitsFilled();
				$.fn.CheckSubUnitsFilled();
			}
			if ($(this).hasClass('suppliers') || $(this).hasClass('accounting')) {
				$.fn.EnableMaterialSelect('#VendorCategoryTypes', 'select-options-VendorCategoryTypes');
			}



		}
	});
	$.fn.EnableMaterialSelect = function (selectID, dataActivates) {
		var selectedIndex = $(selectID).find(":selected").index();		
		console.log("selectedIndex: "+selectedIndex);
		$(selectID).prop("disabled", false);
		$(selectID).destroyMaterialSelect();
		$(selectID).removeAttr("disabled")
		$(selectID).materialSelect();
		$('[data-activates="' + dataActivates + '"]').prop('disabled', false);
		$(selectID).prop('selectedIndex', selectedIndex);
	}
	$("#addSupplierComment").click(function () {
		$('[data-toggle="popover"]').popover('dispose');
		$('#addSupplierComment').popover({
			sanitize: false,
			placement: 'bottom',
			html: true,
			content: function () {
				return $('#popover-content').html();
			}
		});
		$('#addSupplierComment').popover('toggle');

	});
	$('.employee-status-radio').off("click").on("click", function () {
		console.log('employee status')
		$('#NewEmployee_EmployeeStatusID').val($(this).val())
		$("#validation-EmployeeStatus").addClass("hidden");
		var statIdVal = $("#NewEmployee_EmployeeStatusID").val();
		console.log("statIdVal: " + statIdVal);
		if (statIdVal = 4) {

		}
	})


});