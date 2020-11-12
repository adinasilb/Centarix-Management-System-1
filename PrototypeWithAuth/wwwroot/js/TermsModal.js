﻿$(document).ready(function () {
	$("#Terms").val("Select");
});
$(function () {
	$("#Installments").on('change', function () {
		console.log("installments changed...");
		$.fn.ChangePaymentsTable($(this).val());
	});


	$.fn.ChangePaymentsTable = function (installments) {
		//var installments = $(this).val();
		var countPrevInstallments = $(".installments").length;
		var difference = installments - countPrevInstallments;
		var index = $('#index').val();
		//console.log("installments: " + installments);
		//console.log("countPrevInstallments: " + countPrevInstallments);
		//console.log("difference: " + difference);
	//January is 0 but do not add because everytime you create a new line it adds 1

		if (countPrevInstallments <1) {
			var today = new Date();
			var prevdd = today.getDate();
			var prevmm = today.getMonth(); //January is 0 but do not add because everytime you create a new line it adds 1
			var prevyyyy = today.getFullYear();
		}
		else {
			var date = $('.paymentDate' + (countPrevInstallments - 1)).val()
			console.log(date)
			var lastDate = new Date(date)
			var prevdd = lastDate.getDate();
			var prevmm = lastDate.getMonth()+1; 
			var prevyyyy = lastDate.getFullYear();
			console.log("prevmm"+prevmm);
		}

	

		if (prevdd < 10) { prevdd = '0' + prevdd }

		if (difference > 0) {
			for (var x = difference; x > 0; x--) {
				
				var newmm = 0;
				var newyyyy = 0;
				if (prevmm < 12) {
					console.log("if (prevmm < 12)");
					newmm = parseInt(prevmm);
					console.log(newmm)
					newmm += 1;
		
					newyyyy = prevyyyy;
				}
				else {
					console.log("else");
					newyyyy = parseInt(prevyyyy) + 1;
					newmm = 1;
				}

				if (newmm < 10) { newmm = '0' + newmm }
				console.log(newmm)
				var paymentDate = newyyyy + '-' + newmm + '-' + prevdd;
		
				prevyyyy = newyyyy;
				prevmm = newmm;
				console.log("prevmm "+prevmm)

				$.ajax({
					async: false,
					url: '/Requests/InstallmentsPartial?index='+ (index),
					type: 'GET',
					cache: true,
					success: function (data) {
						$(".terms-modal").append(data);
						var dateElem = '.paymentDate' + index;
						$(dateElem).val(paymentDate)
						$('.mdb-select' + index).materialSelect();	
						
					
						var dateClass = "paymentDate" + index;
						var sumID = "#NewPayments_" + index + "__Sum";
						var paymentDateID = "#NewPayments_" + index + "__PaymentDate";
						var paymentTypeID = "#NewPayments_" + index + "__CompanyAccount_PaymentType";
						var referenceID = "#NewPayments_" + index + "__Reference";
						$(sumID).rules("add",{ required: true, min :1 });
						$(paymentDateID).rules("add",{ required: true });
						$(paymentTypeID).rules("add",{ selectRequired: true });
						$('#index').val(++index);
					}
				});
			
			};

		}
		else if (difference < 0) { //TODO: rework the remove
			for (var y = difference; y < 0; y++) {
				console.log("y:" + y)
				$(".terms-modal").children('.row').last().remove();
				$('#index').val(--index);
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
			var item = '<option value="">Select</option>';
			$("#" + newid).empty();

			$.each(data, function (i, companyAccount) {
				item += '<option value="' + companyAccount.companyAccountID + '">' + companyAccount.companyAccountNum + '</option>'
			});
			$("#" + newid).html(item);
			$("#" + newid).rules("add",{selectRequired : true})
		});
	});

	$(".modal").on('change', "#Paid", function () {
		var val = $(this).val();
		console.log("in paid fx " + val);
		if (val == 'true') {
			console.log("true");
			$("#Terms").destroyMaterialSelect();
			$("#Terms").attr("disabled", true)
			$("#Terms").materialSelect();
			$("#Installments").attr("disabled", true);
		}
		else {
			console.log("false");
			$("#Terms").destroyMaterialSelect();
			$("#Terms").attr("disabled", false)
			$("#Terms").materialSelect();
			$("#Installments").attr("disabled", false);
		};
	});

	$(".modal").on('change',"#Terms" , function () {
		var val = $(this).val();
		console.log("in terms fx: " + val);
		if (val == '') {
			console.log("true");
			$("#Paid").destroyMaterialSelect();
			$("#Paid").attr("disabled", false)
			$("#Paid").materialSelect();
			$("#Installments").attr("disabled", false);
		}
		else {
			console.log("false");
			$("#Paid").destroyMaterialSelect();
			$("#Paid").attr("disabled", true)
			$("#Paid").materialSelect();
			$("#Installments").attr("disabled", true);
		}
	});

	$("#Installments").on('change', function () {
		var val = $(this).val();
		if (val > 0) {
			$("#Paid").destroyMaterialSelect();
			$("#Paid").attr("disabled", true)
			$("#Paid").materialSelect();
			$("#Terms").destroyMaterialSelect();
			$("#Terms").attr("disabled", true)
			$("#Terms").materialSelect();
		}
		else {
			$("#Terms").destroyMaterialSelect();
			$("#Terms").attr("disabled", false)
			$("#Terms").materialSelect();
			$("#Paid").destroyMaterialSelect();
			$("#Paid").attr("disabled", false)
			$("#Paid").materialSelect();
		};
	});


})