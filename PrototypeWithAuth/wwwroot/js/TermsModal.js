$(document).ready(function () {
	$("#Terms").val("Select");
});
$(function () {
	$("#Installments").on('change', function () {
		console.log("installments changed...");
		$.fn.ChangePaymentsTable($(this).val());
	});


	$.fn.ChangePaymentsTable = function (installments) {
		//var installments = $(this).val();
		var countPrevInstallments = $(".type-1").length;
		var difference = installments - countPrevInstallments;
		//console.log("installments: " + installments);
		//console.log("countPrevInstallments: " + countPrevInstallments);
		//console.log("difference: " + difference);

		var today = new Date();
		var prevdd = today.getDate();
		var prevmm = today.getMonth(); //January is 0 but do not add because everytime you create a new line it adds 1
		var prevyyyy = today.getFullYear();

		if (prevdd < 10) { prevdd = '0' + prevdd }

		if (difference > 0) {
			console.log("in add");
			var newIncrementNumber = countPrevInstallments;
			for (x = difference; x > 0; x--) {
				console.log("in for")
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

				var paymentDate = newyyyy + '-' + newmm + '-' + prevdd;

				prevyyyy = newyyyy;
				prevmm = newmm;
				var index = $('#index').val();
				$.ajax({
					async: false,
					url: '/Requests/InstallmentsPartial?index='+ index,
					type: 'GET',
					cache: false,
					success: function (data) {
						$(".terms-modal").append(data);
						$('#index').val(++index);
						newIncrementNumber++;

						
					}
				});
			
			};

		}
		else if (difference < 0) { //TODO: rework the remove
			for (x = difference; x < 0; x++) {
				$(".terms-modal").last().remove();
			}
		}
	};



	//since the paymentType field is dynamically created, the function needs to be bound the payments-table b/c js binds server-side
	$(".modal").on("change", ".paymentType", function (e) {
		console.log("changepayment type")
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

	$("#Paid").on('change', function () {
		var val = $(this).val();
		console.log("in paid fx " + val);
		if (val == 'true') {
			console.log("true");
			$(".Terms").materialSelect({ destroy: true });
			$(".terms-disabled").show();
			$("#Installments").attr("disabled", true);
		}
		else {
			console.log("false");
			$(".terms-disabled").hide();
			$(".Terms:last").materialSelect();
			$("#Installments").attr("disabled", false);
		};
	});

	$("#Terms").on('change', function () {
		var val = $(this).val();
		console.log("in terms fx: " + val);
		if (val == '') {
			console.log("true");
			//$("#Paid").attr("disabled", false);
			$("#Paid:last").materialSelect();
			$(".paid-disabled").hide();
			$("#Installments").attr("disabled", false);
		}
		else {
			console.log("false");
			//$("#Paid").attr("disabled", true);
			$("#Paid").materialSelect({ destroy: true });
			$(".paid-disabled").show();
			$("#Installments").attr("disabled", true);
		}
	});

	$("#Installments").on('change', function () {
		var val = $(this).val();
		if (val > 0) {
			$("#Paid").materialSelect({ destroy: true });
			$(".paid-disabled").show();
			$(".Terms").materialSelect({ destroy: true });
			$(".terms-disabled").show();
		}
		else {
			$(".terms-disabled").hide();
			$(".Terms:last").materialSelect();
			$("#Paid:last").materialSelect();
			$(".paid-disabled").hide();
		};
	});


})