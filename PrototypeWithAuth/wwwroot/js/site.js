// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//global Exchange Rate variable (usd --> nis)

$(function () {
	//$('.modal form').attr('autocomplete', 'off');
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
	$("#parentlist").off("change").on("change", function () {
		$.fn.parentListChange();
	});

	//$('.modal').off("change").on('change', '#parentlist', function () {
	//	$.fn.parentListChange();
	//});


	$.fn.parentListChange = function () {
		console.log("in parent list");
		var parentCategoryId = $("#parentlist").val();
		console.log("parentcategoryid: " + parentCategoryId);
		var url = "/Requests/GetSubCategoryList";
		console.log("url: " + url);

		$.getJSON(url, { ParentCategoryId: parentCategoryId }, function (data) {
			console.log(" in json")
			var firstitem1 = '<option value=""> Select Subcategory</option>';
			$("#sublist").empty();
			$("#sublist").append(firstitem1);

			$.each(data, function (i, subCategory) {
				var newitem1 = '<option value="' + subCategory.productSubcategoryID + '">' + subCategory.productSubcategoryDescription + '</option>';
				$("#sublist").append(newitem1);
			});
			$("#sublist").materialSelect();
			return false;
		});
	};

	//change product subcategory dropdown according to the parent categroy selection when a parent category is selected
	//$(".Project").change(function () {
	//	$.fn.changeProject($(this).val());
	//});

	//$('.modal').off('change').on('change', ".Project", function () {
	//	$.fn.changeProject($(this).val());
	//});
	//$(".Project").off("change").on("change", function () {
	//	console.log("in on change before fx");
	//	$.fn.changeProject($(this).val());
	//});
	$("body").off("change", ".Project").on("change", ".Project", function () {
		console.log("in on change before fx");
		//$.fn.changeProject($(this).val());
		console.log("project was changed");
		var projectId = $(this).val();
		var url = "/Requests/GetSubProjectList";

		$.getJSON(url, { ProjectID: projectId }, function (data) {
			var item1 = "<option value=''>Select Sub Project</option>";
			$("#SubProject").empty();
			$("#SubProject").append(item1);
			$.each(data, function (i, subproject) {
				item = '<option value="' + subproject.subProjectID + '">' + subproject.subProjectDescription + '</option>'
				$("#SubProject").append(item);
			});
			$("#SubProject").materialSelect();
			return false;
		});
		return false;
	});



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
		//$.fn.CheckVendorAndCatalogNumbers();
	});
	$("#vendorList").change(function () {
		var vendorid = $("#vendorList").val();
		$.fn.ChangeVendorBusinessId(vendorid);
		//CheckVendorAndCatalogNumbers();
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
				//cannot only use the load outside. apparently it needs this one in order to work
				$(".vendorBusinessId").val(newBusinessID);
				$(".vendorBusinessId").text(newBusinessID);
				$("#vendor-primary-email").val(data.ordersEmail);
				if ($.isFunction($.fn.UpdatePrimaryOrderEmail)) {
					$.fn.UpdatePrimaryOrderEmail();
				}
			})
		}
		//console.log("newBusinessID: " + newBusinessID);
		//if nothing was selected want to load a blank
		$(".vendorBusinessId").val(newBusinessID);
		//put the business id into the form
	}




	$.fn.CheckVendorAndCatalogNumbers = function () {
		//var vendorID = $("#vendorList").val();
		//var catalogNumber = $("#Request_Catalog").val();
		//alert("vendor id: " + vendorID + " && catalog number: " + catalogNumber);
	}

	//view documents on modal view
	$(".view-docs").click(function (clickEvent) {
		console.log("order docs clicked!");
		clickEvent.preventDefault();
		clickEvent.stopPropagation();
		var title = $(this).val();
		$(".images-header").html(title + " Documents Uploaded:");
	});





	$.fn.leapYear = function (year) {
		return ((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0);
	}




	$(".modal").on('hide.bs.modal', function () {
		$('.modal-backdrop').remove()
	});



	$(".visual-locations-zoom").off("click").on("click", function (e) {
		console.log("called visual locations zoom with an id of: " + $(this).val());
		var myDiv = $(".location-modal-view");
		var sectionType = $("#SectionType").val();
		console.log("myDiv: " + myDiv);
		var parentId = $(this).val();
		$("#visualZoomModal").replaceWith('');
		$("#visualZoomModal").replaceWith('');
		//console.log("about to call ajax with a parentid of: " + parentId);
		$.ajax({
			async: true,
			url: "/Locations/VisualLocationsZoom/?VisualContainerId=" + parentId + "&SectionType=" + sectionType,
			type: 'GET',
			cache: false,
			success: function (data) {
				var modal = $(data);
				$('body').append(modal);
				$("#visualZoomModal").modal({
					backdrop: true,
					keyboard: true,
				});
				$("#visualZoomModal").modal('show');
				//$('.modal-backdrop').remove()
				var firstTDFilled = $(".visualzoom td");
				var height = firstTDFilled.height();
				var width = firstTDFilled.width();
				console.log("h: " + height + "------ w: " + width);
				//$("td").height(height);
				//$("td").width(width);
				$(".visualzoom td").css('height', width);
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

	$(".open-invoice-doc-modal").off('click').click(function (e) {
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


	$(".open-document-modal").off('click').on("click", function (e) {

		e.preventDefault();
		e.stopPropagation();
		console.log("clicked open doc modal");
		$(".open-document-modal").removeClass("active-document-modal");
		var section = "";
		if ($(".open-document-modal").hasClass('operations') || $(".open-document-modal").hasClass('Operations')) {
			section = "Operations";
		} else if ($(".open-document-modal").hasClass('labManagement') || $(".open-document-modal").hasClass('LabManagement')) {
			section = "LabManagement";
		}
		$(this).addClass("active-document-modal");
		var enumString = $(this).data("string");
		console.log("enumString: " + enumString);
		var requestId = $(this).data("id");
		console.log("requestId: " + requestId);
		var isEdittable = $(".turn-edit-on-off").attr("name") == "edit";
		console.log($("#masterSidebarType").val())
		var modalType = $("#modalType").val();
		console.log($("#modalType").val())
		$.fn.OpenDocumentsModal(enumString, requestId, isEdittable, section, modalType);
		return true;
	});

	$.fn.OpenDocumentsModal = function (enumString, requestId, isEdittable, section, modalType) {
		$(".documentsModal").replaceWith('');
		var urltogo = $("#documentSubmit").attr("url");
		//var urlToGo = "DocumentsModal?id=" + requestId + "&RequestFolderNameEnum=" + enumString + "&IsEdittable=" + isEdittable;*/
		console.log("urltogo: " + urltogo);
		urltogo = urltogo + "?id=" + requestId + "&RequestFolderNameEnum=" + enumString + "&IsEdittable=" + isEdittable + "&SectionType=" + section + "&ModalType=" + modalType;
		console.log("urltogo: " + urltogo);
		//$(".modal-backdrop").first().removeClass();
		$.ajax({
			async: true,
			url: urltogo,
			type: 'GET',
			cache: false,
			success: function (data) {
				var modal = $(data);
				$('body').append(modal);
				$(".documentsModal").modal({
					backdrop: false,
					keyboard: true,
				});
				$(".documentsModal").modal('show');
				return true;
			}

		});
		return true;
	};

	$(".file-select").on("change", function (e) {
		console.log("file was changed");
		$cardDiv = $(this).closest("div.card");
		console.log("cardDiv: " + JSON.stringify($cardDiv));
		$cardDiv.addClass("document-border");
		return true;
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

	//$.fn.updateDebt = function () {
	//	console.log("in update debt");
	//	var sum = $("#Request_Cost").val();
	//	var installments = $("#Request_ParentRequest_Installments").val();
	//	var tdate = new Date();
	//	var dd = tdate.getDate(); //yields day
	//	var MM = tdate.getMonth(); //yields month
	//	var yyyy = tdate.getFullYear(); //yields year
	//	if (dd < 10) {
	//		dd = "0" + dd
	//	}
	//	if (MM < 10) {
	//		MM = "0" + (MM + 1)
	//	} else {
	//		MM = MM + 1;
	//	}
	//	var today = yyyy + "-" + (MM) + "-" + dd;
	//	console.log("today:" + today);
	//	//count how many installment dates already passed
	//	var count = 0;
	//	if (installments != 0) {
	//		$(".payments-table .payment-date").each(function (index) {
	//			var date = $(this).val();
	//			console.log("date: " + date);
	//			if (today >= date) {
	//				console.log("today > date");
	//				//date passed
	//				count = count + 1
	//			}
	//		});
	//	}
	//	if (count != 0) {
	//		var paymentPerMonth = sum / installments;
	//		console.log(" paymentPerMonth: " + paymentPerMonth);
	//		var amountToSubstractFromDebt = paymentPerMonth * count;
	//		console.log(" amountToSubstractFromDebt: " + amountToSubstractFromDebt);
	//		var debt = sum - amountToSubstractFromDebt
	//		console.log(" sum: " + sum);
	//		console.log(" debt: " + debt);
	//		$("#Debt").val(debt);
	//	} else {
	//		$("#Debt").val(sum);
	//	}
	//	var vatCalc = sum * .17;
	//	console.log("vatCalc" + vatCalc);
	//	$('#Request_VAT').val(vatCalc.toFixed(2));
	//};

	//$(".payments-table").on("change", ".payment-date", function (e) {
	//	console.log("in change .payments-table ");
	//	$.fn.updateDebt();
	//});

	//$("#Request_ExchangeRate").change(function (e) {
	//	console.log("in change #Request_ExchangeRate ");
	//	$.fn.updateDebt();
	//});


	//$("#sum-dollars").change(function (e) {
	//	console.log("in change #sum-dollars ");
	//	$.fn.updateDebt();
	//});

	//$("#Request_ParentRequest_Installments").change(function () {
	//	console.log("in change Request_ParentRequest_Installments ");
	//	$.fn.updateDebt();
	//});


	$(".load-location-index-view").off("click").on("click", function (e) {
		//clear the div to restart filling with new children
		$(".load-location-index-view").removeClass("location-type-selected");
		$(this).addClass("location-type-selected");
		$.fn.setUpLocationIndexList($(this).attr("value"))
	$(".second-col").removeClass("filled-location-class");
	});

	$.fn.setUpLocationIndexList = function (val) {
		//clear second div just in case
		var subDiv = $(".colTwoSublocations");
		subDiv.html("");
		$(".li-name").html("");

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
		//remove all from column 2
		var thisLocationInstanceID = $(this).val();
		$(".sublocation-index").each(function () {
			var sublocationIndexParsed = $(this).children("table").children("tbody").children("tr:first-child").children("td").children("button").val();
			console.log("sublocationIndexParsed: " + sublocationIndexParsed);
			if (parseInt(sublocationIndexParsed) > parseInt(thisLocationInstanceID)) {
				console.log("hiding " + sublocationIndexParsed + "...");
				$(this).remove();
			}
		});

		var myDiv = $(".colTwoSublocations");
		var table = $(this).closest('table');

		//Begin CSS Styling
		var stylingClass = "filled-location-class";
		var trStylingClass = "filled-location-tr";

		table.children('tbody').children('tr').children('td').removeClass(stylingClass);
		table.children('tbody').children('tr').removeClass(trStylingClass);
		$(this).parent().addClass(stylingClass);
		$(this).parent().parent().addClass(trStylingClass);

		//var parentStylingClass = "parent-location-selected-outer-lab-man";
		//var trParentStylingClass = "pl-border-right-0";
		//if ($(this).hasClass("parent-location")) {
		//	$("body td").removeClass(parentStylingClass);
		//	$("body tr").removeClass(trParentStylingClass);
		//	$(this).parent().addClass(parentStylingClass);
		//	$(this).parent().parent().addClass(trParentStylingClass);

		//}
		//End CSS Styling
		$.fn.setUpVisual($(this).val(), false);
	});


	$(".load-sublocation-view").off("click").on("click", function (e) {

		//e.preventDefault();
		//e.stopPropagation();
		//add or remove the background class in col 1
		//$(".load-sublocation-view").parent().removeClass("td-selected");
		//$(this).parent().addClass("td-selected");
		$("#loading1")/*.delay(1000)*/.show(0);

		//delete all prev tables:
		//var tableVal = $(this).val();
		//console.log("-------------------------------------------------------------");
		//console.log("table val: " + tableVal);
		//$('div[id^="table"]').each(function () {
		//	var tableID = $(this).attr("id");
		//	var tableNum = tableID.substr(5, tableID.length);
		//	console.log("tableID: " + tableID);
		//	console.log("tableNum: " + tableNum);
		//	if (parseInt(tableVal) < parseInt(tableNum)) {
		//		console.log(tableVal + " < " + tableNum);
		//		$(this).hide();
		//	}
		//	else {
		//		console.log(tableVal + " > " + tableNum);
		//	}
		//});

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
		var stylingClass = "filled-location-class";
		var trstylingClass = "filled-location-tr";
		//$("body td").removeClass(stylingClass);

		//$(table + " td").removeClass(stylingClass);
		//$(table + " td").removeClass(stylingClass);
		table.children('tbody').children('tr').children('td').removeClass(stylingClass);
		table.children('tbody').children('tr').removeClass(trstylingClass);
		$(this).parent().addClass(stylingClass);
		$(this).parent().parent().addClass(trstylingClass);

		//$("." + stylingClass).addClass(stylingClass);

		var parentStylingClass = "parent-location-selected-outer-lab-man  location-open-border-right";
		var isParent = false;
		if ($(this).hasClass("parent-location")) {
			//console.log("is parent location!");
			console.log("this has been clicked by a parent location");
			//add heading name
			var name =$(this).attr("name");
			//remove prev sidebars
			$("body td").removeClass(parentStylingClass);
			//add new sidebars
			$(this).parent().addClass(parentStylingClass);

			//remove all from column 2
			$(".sublocation-index").each(function () {
				$(this).hide();
			});


			isParent = true;

		}
		else {

			console.log("is not parent location");
			//remove all columns to the right
			var thisLocationInstanceTypeID = $(this).attr("typeid");
			console.log("thisLocationInstanceID: " + thisLocationInstanceTypeID);
			$(".sublocation-index").each(function () {
				var sublocationIndexParsed = $(this).children("table").children("tbody").children("tr:first-child").children("td").children("button").attr("typeid");
				console.log("sublocationIndexParsed: " + sublocationIndexParsed);
				if (parseInt(sublocationIndexParsed) > parseInt(thisLocationInstanceTypeID)) {
					console.log("hiding " + sublocationIndexParsed + "...");
					$(this).replaceWith('');
				}
			});
		}
		//End CSS Styling

		//fill up col 2 with the next one
		var parentId = $(this).val();
		var parentLocation = $(this);
		console.log("open sublocation view, index: " + parentId);

		var parentsParentId = $(this).closest('tr').attr('name');

		//if ($("#colTwoSublocations" + parentsParentId).length == 0) {
		//	//if (!$('.second-col .li-name').val()) {

		//	//}
		//	$('.colTwoSublocations').append('<div class="colTwoSublocationsChildren" id="colTwoSublocations' + parentsParentId + '"></div>');
		//}
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
				myDiv.append(result);
				if ($(parentLocation).hasClass("parent-location")) {
					//$(".second-col .li-name").html($(".col.sublocation-index").attr("parentName"));
					//$("table td.li-name").html($(parentLocation).attr("name"));
					//$("table td.li-name").removeClass("filled-location-class-color")
					$(".second-col").addClass("filled-location-class");
				}
				//this.html(result);
				//add heading name
				


			}
		});

		$.fn.setUpVisual($(this).val(), isParent, name);


	});

	$.fn.setUpVisual = function (val, isParent, name) {
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
				var width = $('.visual-locations-table td').width();
				if ($('.visual-locations-table td').hasClass("is25")) {
					$('.visual-locations-table td').css("height", width);
				}
				if(isParent)
				{
					$(".li-name").html(name);
					$(".li-name-container").removeClass("d-none");
				}
				if ($(".hasVisual").length > 0 && isParent == true) {
					$(".li-name").html("");

					$(".li-name-container").addClass("d-none");
					$(".second-col").removeClass("filled-location-class");
								
				}
				
				$("#loading2").hide();
				$("#loading2")/*.delay(1000)*/.hide(0);
			}
		});
	};

	$(".modal").on("change", '#UserImageModal', function () {
		var imgPath = $("#UserImageModal")[0].value;
		console.log("imgPath: " + imgPath);
		var extn = imgPath.substring(imgPath.lastIndexOf('.') + 1).toLowerCase();
		console.log("extn: " + extn);
		var imageHolder = $("#user-image-modal");
		imageHolder.empty();

		var url = "/Admin/SaveTempUserImage";
		console.log("url : " + url);
		var formData = new FormData($("#userImageForm")[0]);
		var data = $("#userImageForm").serialize();
		//var formData = new FormData($(this));
		//console.log("data : " + data);
		console.log("formData : " + formData);
		//console.log("data : " + model);


		$.ajax({
			url: url,
			method: 'POST',
			data: formData,
			success: (result) => {
				console.log("result: " + result);
				if (result) {
					if (extn == "gif" || extn == "png" || extn == "jpg" || extn == "jpeg") {
						console.log("inside the if statement");
						if (typeof (FileReader) != "undefined") {
							console.log("file reader does not equal undefined");
							var reader = new FileReader();
							reader.onload = function (e) {
								console.log(e.target.result);
								$("#user-image-modal").attr("src", e.target.result);
							}
							imageHolder.show();
							reader.readAsDataURL($(this)[0].files[0]);
							$('.file-name').text(this.files[0].name);
						}

						$("#UserImagePath").val(result);
					}
					else {
						//alert("Please only select images");
					}

				}

				//return false;
			},
			processData: false,
			contentType: false
		});



		//Saving the user image here with a val of Saved = Temp


	});
	$('#saveUserImage').click(function () {
		//Save the user image under a temp file name
		$("#UserImageSaved").val(true);

		//var imgPath = $("#UserImageModal")[0].value;
		var imgPath = $("#UserImagePath").val();
		//$(".user-image").html('<img src="~/' + imgPath + '" class="user-image" />');
		$("#user-image").attr("src", "/" + imgPath);
		$(".userImage i").hide();

		$('.modal.userImageModal').remove();
		$('.modal-backdrop').remove();
	});

	$("#InvoiceImage").on("change", function () {
		var imgPath = $("#InvoiceImage")[0].value;
		console.log("imgPath: " + imgPath);
		var extn = imgPath.substring(imgPath.lastIndexOf('.') + 1).toLowerCase();
		console.log("extn: " + extn);
		var imageHolder = $("#invoice-image");
		imageHolder.empty();

		if (extn == "pdf" || extn == "png" || extn == "jpg" || extn == "jpeg") {
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
		//$("#myForm").valiD);
		//$.fn.validateItemTab();
		//$.fn.validateItemTab();

	});

	$("#Request_Terms").change(function () {
		console.log("in change Request_Terms");
		if ($(this).val() == -1) {
			$(".installments").hide();
		} else {
			$(".installments").show();
		}

	});

	$(".create-user .permissions-tab").on("click", function () {
		console.log("permissions tab opened");
		$.fn.HideAllPermissionsDivs();
		$.fn.ChangeUserPermissionsButtons();
		$(".main-permissions").show();
	});

	$(".modal .permissions-tab").on("click", function () {
		console.log("permissions tab opened");
		$.fn.HideAllPermissionsDivs();
		$.fn.ChangeUserPermissionsButtons();
		$(".main-permissions").show();
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


	$.fn.AddContactValidation = function () {
		$(".contact-name").each(
			function () { $(this).rules("add", "required") });

		$(".contact-email").each(
			function () {
				$(this).rules("add", {
					required: true,
					email: true
				});
			});
		$(".contact-phone").each(
			function () {
				$(this).rules("add", {
					required: true,
					minlength: 9
				});
			});
	};


	$("#addSuplierContact").off('click').click(function () {
		var index = $('#contact-index').val();
		$.ajax({
			async: false,
			url: '/Vendors/ContactInfoPartial?index=' + index,
			type: 'GET',
			cache: false,
			success: function (data) {
				$("#contact-info").append(data);
				$('#contact-index').val(++index);
				$.fn.AddContactValidation();
			}
		});

	});




	$.fn.addSupplierComment = function (type) {
		console.log(type);
		var index = $('#comment-index').val();
		$.ajax({
			async: false,
			url: '/Vendors/CommentInfoPartialView?type=' + type + '&index=' + index,
			type: 'GET',
			cache: false,
			success: function (data) {
				$("#comment-info").append(data);
				$('#comment-index').val(++index);
			}
		});
	}

	$.fn.addRequestComment = function (type) {
		console.log(type);
		var index = $('#index').val();
		$.ajax({
			async: false,
			url: '/Requests/CommentInfoPartialView?type=' + type + '&index=' + index,
			type: 'GET',
			cache: false,
			success: function (data) {
				$("#comment-info").append(data);
				$('#index').val(++index);
			}
		});
	}

	/*--------------------------------Accounting Payment Notifications--------------------------------*/
	$(".payments-pay-now").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var vendorid = $(this).attr("vendor");
		var paymentstatusid = $(this).attr("paymentstatus");
		var typeEnum = $(this).attr("type");
		console.log("vendor: " + vendorid);
		console.log("payment status: " + paymentstatusid);
		//var $itemurl = "Requests/TermsModal/?id=" + @TempData["RequestID"] + "&isSingleRequest=true"
		var itemurl = "/Requests/PaymentsPayModal/?vendorid=" + vendorid + "&paymentstatusid=" + paymentstatusid + "&accountingPaymentsEnum=" + typeEnum;
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
				//console.log("data:");
				//console.log(data);
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
				var modal = $(data);
				$('body').append(modal);
				//replaces the modal-view class with the ModalView view
				//$(".modal-view").html(data);
				//turn off data dismiss by clicking out of the box and by pressing esc
				$(".modal").modal({
					backdrop: true,
					keyboard: false,
				});
				//shows the modal
				$(".modal").modal('show');
				return false;
			}
		});
	};

	$.fn.CallModal2 = function (url) {
		console.log("in call modal2, url: " + url);
		$(".userImageModal").replaceWith('');
		$.ajax({
			async: true,
			url: url,
			type: 'GET',
			cache: false,
			success: function (data) {
				var modal = $(data);
				$('body').append(modal);
				$(".userImageModal").modal({
					backdrop: false,
					keyboard: true,
				});
				$(".userImageModal").modal('show');
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
		$('#entryForm').trigger('submit');
	});
	$("#exit").dblclick(function () {
		console.log("in exit")
		$.fn.GetEmployeeHourFromToday();
		return false;
	});
	$("#entry").off('click').click(function (e) {
		e.preventDefault();
	});
	$("#exit").off('click').click(function (e) {
		e.preventDefault();
	});
	$('.monthsHours .select-dropdown').off('change').change(function (e) {
		console.log(".monthsHours chnage")
		if ($(this).val() != '') {
			$.fn.SortByMonth($(this).val(), $('#SelectedYear').val())
		}
	});
	$('.yearsHours .select-dropdown').off('change').change(function (e) {
		console.log(".yearsHours chnage")
		if ($(this).val() != '') {
			$.fn.SortByMonth($('#months').val(), $(this).val())
		}
	});
	$.fn.SortByMonth = function (month, year) {
		$.ajax({
			async: false,
			url: '/Timekeeper/HoursPage?month=' + month + "&year=" + year + "&pageType=" + $('#masterPageType').val(),
			type: 'GET',
			cache: false,
			success: function (data) {
				$("#hoursTable").html(data);
			}
		});
	};

	$(".open-work-from-home-modal").off('click').click(function (e) {
		e.preventDefault()
		if ($(this).hasClass("SummaryHours")) {
			pageType = "SummaryHours";
		}
		if ($(this).hasClass("ReportHours")) {
			pageType = "ReportHours";
		}
		var itemurl = "UpdateHours?PageType=" + pageType + "&isWorkFromHome=" + true;
		$("#loading").show();
		$.fn.CallModal(itemurl);
	});

	$(".open-update-hours-modal").off('click').click(function (e) {
		e.preventDefault();
		var val = $(this).attr("value");
		if (val != '') {
			var date = new Date(val).toISOString();
			console.log(date)
		}
		if ($(this).hasClass("SummaryHours")) {
			pageType = "SummaryHours";
		}
		if ($(this).hasClass("ReportHours")) {
			pageType = "ReportHours";
		}
		var itemurl = "UpdateHours?PageType=" + pageType + "&chosenDate=" + date;
		$("#loading").show();
		$.fn.CallModal(itemurl);
	});

	$(".report-off-day").off('click').click(function (e) {
		var offDayType = $(this).attr("value");
		var pageType = $("#masterPageType").val();
		var itemurl = "OffDayModal?PageType=" + pageType + "&OffDayType=" + offDayType;
		$("#loading").show();
		$.fn.CallModal(itemurl);
	});

	$('.no-hours-reported').off('change').change(function (e) {
		var selectedDate = $(this).attr("date");
		var pageType = "SummaryHours";
		switch ($(this).val()) {
			case "1":
				var itemurl = "UpdateHours?PageType=" + pageType + "&chosenDate=" + selectedDate + "&isWorkFromHome=" + true;
				$("#loading").show();
				$.fn.CallModal(itemurl)
				break;
			case "2":
				var itemurl = "UpdateHours?PageType=" + pageType + "&chosenDate=" + selectedDate;
				$("#loading").show();
				$.fn.CallModal(itemurl);
				break;
			case "3":
				var itemurl = "OffDayConfirmModal?PageType=" + $("#masterPageType").val() + "&date=" + selectedDate + "&OffDayType=SickDay";
				$.fn.CallModal(itemurl);
				break;
			case "4":
				var itemurl = "OffDayConfirmModal?PageType=" + $("#masterPageType").val() + "&date=" + selectedDate + "&OffDayType=VacationDay";
				$.fn.CallModal(itemurl);
				break;
		}
	});
	//$(".confirm-report-offDay").off('click').click(function (e) {

	//	$("#loading").show();
	//	var pageType = $("#masterPageType").val();
	//	var	selectedDate = $(this).val();
	//	console.log("selecteddate: " + selectedDate);
	//	var itemurl = "OffDayConfirmModal?PageType=" + pageType + "&date=" + selectedDate;
	//	$.fn.CallModal(itemurl);
	//});
	$("body").on("change", "#EmployeeHour_Date", function (e) {
		$('.day-of-week').val($.fn.GetDayOfWeek($.fn.formatDateForSubmit($(this).val())));
	});

	$.fn.GetDayOfWeek = function (date) {
		var dayOfWeek = moment(date).format("dddd");
		console.log("dayOfWeek" + dayOfWeek)
		return dayOfWeek
	}

	$("body").on("change", "#EmployeeHour_Date.update-hour-date", function (e) {
		e.preventDefault();
		$.fn.GetEmployeeHour($.fn.formatDateForSubmit($(this).val()), $(this).attr("data-workday"));
	});
	//$("body").on("change", "#EmployeeHour_Date.update-work-from-home", function (e) {
	//	$.fn.GetEmployeeHourFromHome($.fn.formatDateForSubmit($(this).val()));
	//});

	$.fn.GetEmployeeHour = function (date, workDay) {
		console.log(date);
		var workFromHome = false;
		if (workDay == "1") {
			workFromHome = true;
		}
		$.ajax({
			async: false,
			url: '_UpdateHours?chosenDate=' + date + "&isWorkFromHome=" + workFromHome,
			type: 'GET',
			cache: false,
			success: function (data) {
				$("#loading").hide();
		        $(".update-hours-partial").html(data);
				return false;
			}
		});
	};


	//RECEIVEDMODAL:
	function changePartialDelivery() {
		console.log("in partial delivery, checkbox val: " + $(this).val());
	};

	function changeClarify() {
		console.log("in clarify, checkbox val: " + $(this).val());
	};

	//$.fn.GetEmployeeHourFromHome = function (date) {
	//	console.log(date);
	//	$.fn.CallModal('UpdateHours?chosenDate=' + date + "&isWorkFromHome=" + true)
	//};
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
		console.log($(this).text())
		$(this).parents('.dropdown-main').find('span:not(.caret)').text($(this).text());
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
	$('.dropdown-multiple .dropdown-menu div .form-check-input').click(function () {
		//$(this).parents('.dropdown').find('span').text($(this).text());
		console.log("in multiple")
		//alert("in multiple");
		$(this).parents('.dropdown-multiple').find('span:not(.caret)').append($(this).find('label').text());
	});
	/*End Dropdown Menu*/


	//$('.dropdown-menu li').click(function () {
	//	var input = '<strong>' + $(this).parents('.dropdown').find('input').val() + '</strong>',
	//		msg = '<span class="msg">Hidden input value: ';
	//	$('.msg').html(msg + input + '</span>');
	//}); 
	$("body").on("change", "#EmployeeHour_TotalHours", function (e) {
		$('#EmployeeHour_Exit1').val('');
		$('#EmployeeHour_Exit2').val('');
		$('#EmployeeHour_Entry1').val('');
		$('#EmployeeHour_Entry2').val('');
	});
	$("body").on("change", "#EmployeeHour_Exit1", function (e) {
		$.fn.UpdateTotalHours();
		//$('#EmployeeHour_TotalHours').val('');
	});
	$("body").on("change", "#EmployeeHour_Entry1", function (e) {
		$.fn.UpdateTotalHours();
		//$('#EmployeeHour_TotalHours').val('');
	});

	$("body").on("change", "#EmployeeHour_Exit2", function (e) {
		$.fn.UpdateTotalHours();
		//$('#EmployeeHour_TotalHours').val('');
	});
	$("body").on("change", "#EmployeeHour_Entry2", function (e) {
		$.fn.UpdateTotalHours();
		//$('#EmployeeHour_TotalHours').val('');
	});

	$.fn.UpdateTotalHours = function () {
		var totalHours = '';
		var totalentryhours = '';
		var exit1 = $('#EmployeeHour_Exit1').val();
		var exit2 = $('#EmployeeHour_Exit2').val();
		var entry1 = $('#EmployeeHour_Entry1').val();
		var entry2 = $('#EmployeeHour_Entry2').val();
		if (entry1 != '' && exit1 != '') {
			exit1split = exit1.split(":");
			var exit1fullhours = parseFloat(exit1split[0]) + parseFloat(exit1split[1]) / 60;
			entry1split = entry1.split(":");
			var entry1fullhours = parseFloat(entry1split[0]) + parseFloat(entry1split[1]) / 60;
			totalentryhours = exit1fullhours - entry1fullhours;
		}
		if (entry2 != '' && exit2 != '') {
			exit2split = exit2.split(":");
			var exit2fullhours = parseFloat(exit2split[0]) + parseFloat(exit2split[1]) / 60;
			entry2split = entry2.split(":");
			var entry2fullhours = parseFloat(entry2split[0]) + parseFloat(entry2split[1]) / 60;
			var totalentry2hours = exit2fullhours - entry2fullhours;
			totalentryhours += totalentry2hours;
		}

		if (totalentryhours != '') {
			var hours = Math.floor(totalentryhours);
			//if (hours < 10) { hours = '0' + hours }
			var mins = Math.round(60 * (totalentryhours - hours));
			if (mins < 10) { mins = '0' + mins }
			
			var totalHours = hours + ":" + mins;
			console.log(hours+":"+ mins)
			if (hours < 0 || isNaN(hours) || isNaN(mins)) {
				totalHours = "";
            }
		}

		$('#EmployeeHour_TotalHours').val(totalHours);
	}

	//});
	/*End Dropdown Menu*/
	//$("body").off('click').on("click", ".upload-image", function (e) {
	//$("body").off('click').on("click", ".upload-image", function (e) {
	//	console.log("in upload image");
	//	$.fn.CallModal2("/Admin/UserImageModal");
	//});

	//$('.dropdown-menu li').click(function () {
	//	var input = '<strong>' + $(this).parents('.dropdown').find('input').val() + '</strong>',
	//		msg = '<span class="msg">Hidden input value: ';
	//	$('.msg').html(msg + input + '</span>');
	//}); 
	$.fn.SaveOffDays = function (url, month) {
		var rangeTo = $('.datepicker--cell.-selected-.-range-to-');
		var dateRangeToDay = rangeTo.attr('data-date');

		var dateFrom = $('#vacation-dates').datepicker().data('datepicker').minRange.toISOString()
		var dateTo = '';
		if (dateRangeToDay == undefined) {
			dateTo = null;
		}
		else {
			dateTo = $('#vacation-dates').datepicker().data('datepicker').maxRange.toISOString()
		}

		$("#Month").val(month);
		$("#FromDate").val(dateFrom);
		$("#ToDate").val(dateTo);

		var formData = new FormData($("#myForm")[0]);
		//console.log(...formData)
		console.log(dateFrom + "-" + dateTo);
		alert("about to go into ajax, url: " + url);
		$.ajax({
			processData: false,
			contentType: false,
			async: true,
			url: "/Timekeeper/" + url , //+ '?dateFrom=' + dateFrom + "&dateTo=" + dateTo + "&PageType=" + pageType +  "&month=" + month,
			type: 'POST',
			data: formData,
			cache: true,
			success: function (data) {
				console.log(data)
				$(".modal").modal('hide');
				if (pageType = "ReportDaysOff") {

					$(".report-days-off-partial").html(data);
				}
				else {
					alert("else")
					$(".render-body").html(data);
				}
			}

		});
	}



	$("#saveOffDay").off('click').click(function (e) {
		e.preventDefault();

		var pageType = $('#masterPageType').val()

		$.fn.SaveOffDays("OffDayModal", pageType, "");
	});


	$(".modal").on("click", "#saveOffDayConfirmation", function (e) {
		e.preventDefault();
		alert("save sick confirmation");
		//alert($('#SelectedDate').val())
		$("#FromDate").val($('#SelectedDate').val());
		$("#Month").val($("#months").val());
		//var dd = parseInt(date[2]);
		//var mm = parseInt(date[1]);
		//var yyyy = parseInt(date[0]);
		//var dateFrom = new Date(yyyy, mm, dd);;
		var formData = new FormData($("#myForm")[0])
		$.ajax({
			processData: false,
			contentType: false,
			async: true,
			url: "/Timekeeper/OffDayConfirmModal",
			type: 'POST',
			data: formData,
			cache: false,
			success: function (data) {
				$(".modal").modal('hide');
				$("#hoursTable").html(data);
			}
		});
	});

	
	//function ChangeEdits() {
	//	alert("sitejs change edits");
	//};

	$(".open-ehaa-modal").off("click").on("click", function (e) {
		e.preventDefault();
		$("#loading").show();
		$("#ehaaModal").replaceWith('');
		console.log("in ehaa modal: " + $(this).val());
		$.ajax({
			async: false,
			url: '/Timekeeper/_EmployeeHoursAwaitingApproval?ehaaID=' + $(this).attr("value"),
			type: 'GET',
			cache: false,
			success: function (data) {
				$("#loading").hide();
				var modal = $(data);
				$('body').append(modal);
				$("#ehaaModal").modal({
					backdrop: false,
					keyboard: false,
				});
				//shows the modal
				$("#ehaaModal").modal('show');
			}
		});
	});
	$(".back-button").off("click").on("click", function () {
		console.log('back button');
		var type = $(".turn-edit-on-off").attr('name');
		console.log('type' + type);
		if (type == 'edit') {
			var section = "";
			console.log($('#masterSectionType').val());
			if ($('#masterSectionType').val() == "Operations") {
				section = "Operations";
			}
			else if ($('#masterSectionType').val() == "LabManagement") {
				section = "LabManagement";
			}
			else if ($('#masterSectionType').val() == "Accounting") {
				section = "Accounting";
			}
			else if ($('#masterSectionType').val() == "Users") {
				section = "Users";
			}
			else if ($('#masterSectionType').val() == "Requests") {
				section = "Requests";
			}
			$("#loading").show();
			$itemurl = "/Requests/ConfirmExit/?MenuItem=" + section;
			console.log($itemurl);
			$.ajax({
				async: true,
				url: $itemurl,
				type: 'GET',
				cache: true,
				success: function (data) {
					$("#loading").hide();
					var modal = $(data);
					$('body').append(modal);
					$(".confirm-exit-modal").modal({
						backdrop: false,
						keyboard: false,
					});
					//shows the modal
					$(".confirm-exit-modal").modal('show');
					$(".modal-open-state").attr("text", "open");
				}

			});
		}
		else {
			if($('#masterPageType').val()=="RequestLocation")
			{
				$(this).closest('.editModal').remove();
				$(this).closest('.editModal').replaceWith('');
			}
			else
			{
				$(this).closest('.modal').modal('hide');
				$(this).closest('.modal').replaceWith('');
			}
			
        }
	})

	$('.turn-edit-on-off').off("click").on("click", function () {
		//if ($('.modal-open-state').attr("text") == "open") {
		//	alert("turn edit on off");
		//	$(".modal-open-state").attr("text", "close");
		//	$(".confirm-edit-modal").remove();
		//	return false;
		//}
		//else {
		var type = $(this).attr('name');
		console.log(type);
		var url = '';
		var section = "";
		if ($(this).hasClass('operations')) {
			url = "/Operations/EditModalView";
			section = "Operations";
		} else if ($(this).hasClass('suppliers')) {
			url = "/Vendors/Edit";
			section = "LabManagement";
		} else if ($(this).hasClass('accounting')) {
			url = "/Vendors/Edit";
			section = "Accounting";
		}
		else if ($(this).hasClass('users')) {
			url = "/Admin/EditUser";
			section = "Users";
		} else if ($(this).hasClass('orders')) {
			url = "/Requests/EditModalView";
			section = "Requests";
		}
		if ($(this).hasClass('orders') && $(this).hasClass('equipment')) {
			url = "/Requests/EditModalView";
			section = "LabManagement";
		}

		if (type == 'edit') {
			$("#loading").show();
			console.log("in if edit");
			$itemurl = "/Requests/ConfirmEdit/?MenuItem=" + section;
			console.log("itemurl: " + $itemurl);
			$.ajax({
				async: true,
				url: $itemurl,
				type: 'GET',
				cache: true,
				success: function (data) {
					$("#loading").hide();
					var modal = $(data);
					$('body').append(modal);
					$(".confirm-edit-modal").modal({
						backdrop: false,
						keyboard: false,
					});
					//shows the modal
					$(".confirm-edit-modal").modal('show');
					$(".modal-open-state").attr("text", "open");

				}

			});


		}
		else if (type == 'details') {
			enableMarkReadonly($(this));
			$(".proprietryHidenCategory").attr("disabled", false);
		}
		//}
	});
	$.fn.DisableMaterialSelect = function (selectID, dataActivates) {
		var selectedIndex = $('#' + dataActivates).find(".active").index();
		selectedIndex = selectedIndex - 1;
		$(selectID).destroyMaterialSelect();
		$(selectID).prop("disabled", true);
		$(selectID).prop('selectedIndex', selectedIndex);
		$(selectID).attr("disabled", true)
		$('[data-activates="' + dataActivates + '"]').prop('disabled', true);
		$(selectID).materialSelect();
    }

	$.fn.EnableMaterialSelect = function (selectID, dataActivates) {
		var selectedIndex = $('#' + dataActivates).find(".active").index();
		var isOptGroup = false;
		if ($('#' + dataActivates + ' li:nth-of-type(' + selectedIndex + ')').hasClass('optgroup') || $('#' + dataActivates + ' li:nth-of-type(' + selectedIndex + ')').hasClass('optgroup-option')) { isOptGroup = true; }
		if (isOptGroup) {
			var selected = $(':selected', $(selectID));
			console.log(selectID + "  " + selectedIndex);
			var optgroup = selected.closest('optgroup').attr('label');
			switch (optgroup) {
				case "Units":
					console.log("Units")
					selectedIndex = selectedIndex - 1;
					break;
				case "Weight/Volume":
					console.log("Volume")
					selectedIndex = selectedIndex - 2;
					break;
				case "Test":
					console.log("Test")
					selectedIndex = selectedIndex - 3;
					break;
			}

		} else {
			selectedIndex = selectedIndex - 1;
		}
		$(selectID).destroyMaterialSelect();
		$(selectID).prop("disabled", false);
		$(selectID).prop('selectedIndex', selectedIndex);
		$(selectID).removeAttr("disabled")
		$('[data-activates="' + dataActivates + '"]').prop('disabled', false);
		$(selectID).materialSelect();
		$.fn.ChangeCheckboxesToFilledInWithoutMDB();
		$('.open-document-modal').attr("data-val", true);
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

	$("#home-btn").click(function () {
		$('[data-toggle="popover"]').popover('dispose');
		$("#home-btn").popover({
			sanitize: false,
			placement: 'bottom',
			html: true,
			content: function () {
				return $('#home-content').html();
			}
		});
		$("#home-btn").popover('toggle');

	});
	$("#addRequestComment").click(function () {
		$('[data-toggle="popover"]').popover('dispose');
		$('#addRequestComment').popover({
			sanitize: false,
			placement: 'bottom',
			html: true,
			content: function () {
				return $('#popover-content').html();
			}
		});
		$('#addRequestComment').popover('toggle');

	});
	$(".more").off('click').click(function () {
		var val = $(this).val();
		$('[data-toggle="popover"]').popover('dispose');
		$(this).popover({
			sanitize: false,
			placement: 'bottom',
			html: true,
			content: function () {
				return $('#' + val).html();
			}
		});
		$(this).popover('toggle');

	});


	$('.isRepeat').off("click").on("click", function () {
		//console.log('employee status')
		var val = $(this).val();
		$('#ExternalCalibrations_0__IsRepeat').val(val)
	});


	//$(".mdb-select ul").off("click").on("click", function () {
	//	alert("select ul clicked!");
	//});

	//$(".mdb-select ul").off("change").on("change", function () {
	//	alert("select ul changed!");
	//});

	//$(".mdb-select ul li").off("click").on("click", function () {
	//	alert("select ul li clicked!");
	//});

	//$(".mdb-select ul li").off("change").on("change", function () {
	//	alert("select ul li changed!");
	//});

	//$(".select-dropdown").off("click").on("click", function () {
	//	alert("select dropdown clicked");
	//});

	//$(".select-dropdown").off("change").on("change", function () {
	//	alert("select dropdown changed");
	//});

	//$(".mdb-select").off("click").on("click", function () {
	//	alert("select clicked!");
	//});

	//$(".mdb-select").off("change").on("change", function () {
	//	alert("select changed!");
	//});
	$('.modal #FirstName').off('change').change(function () {
		$('.userName').val($(this).val() + " " + $('#LastName').val())
	});
	$('.modal #LastName').off('change').change(function () {
		$('.userName').val($('#FirstName').val() + " " + $(this).val())
	});
	$('#FirstName').off('change').change(function () {
		$('.userName').val($(this).val() + " " + $('#LastName').val())
	});
	$('#LastName').off('change').change(function () {
		$('.userName').val($('#FirstName').val() + " " + $(this).val())
	});
	$.fn.CallPageTimekeeper = function (url) {
		$.ajax({
			async: true,
			url: url,
			type: 'GET',
			cache: true,
			success: function (data) {
				$('.render-body').html(data);
			}
		});
	}
	$('.exitModal').off('click').on('click', '.close', function (e) {
		console.log("close edit modal");
		//$.fn.CallPageTimeKeeper();
		$.ajax({
			async: true,
			url: '/Timekeeper/ReportHours',
			type: 'GET',
			cache: true,
			success: function (data) {
				$('.render-body').html(data);
				$('.modal').replaceWith('');
				$(".modal-backdrop").remove();

			}
		});
	})

	$(".save-item").off("click").on("click", function (e) {
		e.preventDefault();
		var url = "";
		if ($(this).hasClass("side-menu")) {
			url = $(this).parent("a").attr("href");
		}
		else {
			var url = $(this).attr("href")
		}
		$itemurl = "/Requests/ConfirmExit/?url=" + url;
		console.log($itemurl);
		$(".confirm-exit-modal").replaceWith('');
		$.ajax({
			async: true,
			url: $itemurl,
			type: 'GET',
			cache: true,
			success: function (data) {
				$("#loading").hide();
				var modal = $(data);
				$('body').append(modal);
					
				$(".confirm-exit-modal").modal({
					backdrop: false,
					keyboard: false,
				});
				//shows the modal
				$(".confirm-exit-modal").modal('show');
				$(".modal-open-state").attr("text", "open");
			}

		});

	})

	//$('body').on('click', '.callIndexPartial', function () {
	//	var url = $(this).attr('url');
	//	$.ajax({
	//		async: true,
	//		url: url,
	//		type: 'GET',
	//		cache: true,
	//		success: function (data) {
	//			$('.index-partial').html(data);
	//		}
	//	});
	//});




});



