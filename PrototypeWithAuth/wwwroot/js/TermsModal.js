
$("#Installments").on('change', function () {
	console.log("installments changed...");
	$.fn.ChangePaymentsTable($(this).val());
});

$.fn.ChangePaymentsTable = function (installments) {
	//var installments = $(this).val();
	var countPrevInstallments = $(".type-1").length;
	var difference = installments - countPrevInstallments;
	console.log("installments: " + installments);
	console.log("countPrevInstallments: " + countPrevInstallments);
	console.log("difference: " + difference);

	if (difference > 0) {
		console.log("in add");
		var newIncrementNumber = countPrevInstallments;
		for (x = difference; x > 0; x--) {

			var newmm = 0;
			var newyyyy = 0;
			if (prevmm < 12) {
				newmm = parseInt(prevmm);
				newmm = newmm + 1;
				newyyyy = prevyyyy;
			}
			else {
				newyyyy = parseInt(prevyyyy) + 1;
				newmm = 1;
			}

			if (newmm < 10) { newmm = '0' + newmm }

			var paymentDate = newyyyy + '-' + newmm + '-' + dd;

			prevyyyy = newyyyy;
			prevmm = newmm;
			$.fn.AddNewPaymentLine(newIncrementNumber, paymentDate);
			newIncrementNumber++;
		};

		$.fn.AdjustPaymentDates();
	}
	else if (difference < 0) { //TODO: rework the remove
		console.log("in subtract");
		for (x = difference; x < 0; x++) {
			$(".payment-date input:last").remove();
			$(".payment-type select:last").remove();
			$(".payment-account select:last").remove();
			$(".payment-reference input:last").remove();
		}
	}
};



$.fn.AddNewPaymentLine = function (increment, date) {
	var htmlTR = "";
	//htmlTR += "<tr class='payment-line m-0 p-0'>";
	//htmlTR += "<td class='m-0 p-0'>";
	var htmlPD = "";
	htmlPD += '<input class="form-control-plaintext border-bottom payment-date date-1" type="date" data-val="true" data-val-required="The PaymentDate field is required." id="NewPayments_' + increment + '__PaymentDate" name="NewPayments[' + increment + '].PaymentDate" value="' + date + '" />';
	htmlPD += '<span class="text-danger field-validation-valid" data-valmsg-for="NewPayments[' + increment + '].PaymentDate" data-valmsg-replace="true"></span>';
	//htmlTR += '</td>';
	//htmlTR += '<td class="m-0 p-0">';
	var htmlPT = "";
	htmlPT += '<select class="form-control-plaintext border-bottom paymentType type-1" id="NewPayments_' + increment + '__CompanyAccount_PaymentType" name="NewPayments[' + increment + '].CompanyAccount.PaymentType"><option value="">Select A Payment Type </option>';
	htmlPT += '<option value="1">Credit Card</option>';
	htmlPT += '<option value="2">Bank Account</option>';
	htmlPT += '</select>';
	htmlPT += '<span class="text-danger field-validation-valid" data-valmsg-for="NewPayments[' + increment + '].CompanyAccount.PaymentType" data-valmsg-replace="true"></span>';
	//htmlTR += '</td>';
	//htmlTR += '<td class="m-0 p-0">';
	var htmlPA = "";
	var newPaymentsId = "NewPayments_" + increment + "__CompanyAccountID";
	var newPaymentsName = "NewPayments[" + increment + "].CompanyAccountID";
	htmlPA += '<select class="form-control-plaintext border-bottom companyAccountNum account1" id="' + newPaymentsId + '" name="' + newPaymentsName + '"></select>';
	htmlPA += '<span class="text-danger field-validation-valid" data-valmsg-for="NewPayments[' + increment + '].CompanyAccount.CompanyAccountID" data-valmsg-replace="true"></span>';
	//htmlTR += '</td>';
	//htmlTR += '<td class="m-0 p-0">';
	var htmlPR = "";
	htmlPR += '<input class="form-control-plaintext border-bottom reference1" type="number" data-val="true" data-val-required="The PaymentID field is required." id="NewPayments_' + increment + '__PaymentID" name="NewPayments[' + increment + '].PaymentID" value="" />';
	htmlPR += '<span class="text-danger field-validation-valid" data-valmsg-for="NewPayments[' + increment + '].PaymentID" data-valmsg-replace="true"></span>';
	//htmlTR += '</td>';
	//htmlTR += '</tr >';
	//$("body").append(htmlTR);
	//$(".payments-table tr:last").after(htmlTR);
	$(".payment-date").append(htmlPD);
	$(".payment-type").append(htmlPT);
	$(".payment-account").append(htmlPA);
	$(".payment-reference").append(htmlPR);
};

$.fn.AdjustInputHeights = function () {
	var input = $("#Installments");
	var height = input.height();
	$(".payment-date").height(height);
	$(".payment-type").height(height);
	$(".payment-account").height(height);
	$(".payment-reference").height(height);
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