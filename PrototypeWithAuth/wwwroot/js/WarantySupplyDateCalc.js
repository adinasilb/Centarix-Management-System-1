
	$(".expected-supply-days").change(function () {
		var OrderDate; 
		if ($('.for-supply-date-calc').length > 0) {
			OrderDate =moment($('.for-supply-date-calc').val().split("/").reverse().join("-"));
			
		} else {
			OrderDate = moment();
			
		}
		var supplyDate = OrderDate.add($(this).val(), "d").format('DD/MM/yyyy');
		console.log(supplyDate)
		$("input[name='expected-supply-days']").val(supplyDate);
		$("input[name='expected-supply-days']").attr("data-val",supplyDate.split("/").reverse().join("-"));

	});


	$("#expected-supply-date").change(function () {
		var val =$(this).val();
		val = val.split("/").reverse().join("-");
		var date =moment(val);
		date.set("time", 0)
		if (date < moment()) {
			return;
		}
		var OrderDate; 
		if ($('.for-supply-date-calc').length > 0) {
			console.log("in first of of check roder date")
			OrderDate =moment($('.for-supply-date-calc').val().split("/").reverse().join("-"));
			
		} else {
			OrderDate = moment();
			
		}
		var amountOfDays = Math.ceil(date.diff(OrderDate,'days', true))
		$(".expected-supply-days").val(amountOfDays);
	});

	$("#Request_Warranty").change(function () {
			var date; 
		if ($("#Request_ParentRequest_OrderDate").length > 0) {
			console.log("in first of of check roder date")
			date =moment($("#Request_ParentRequest_OrderDate").val().split("/").reverse().join("-"));
			
		} else {
			date = moment();
			
		}
		var warrantyDate = date.add($(this).val(), "d").format('DD/MM/yyyy');
		$("input[name='WarrantyDate']").val(warrantyDate);
		$("input[name='WarrantyDate']").attr("data-val",warrantyDate.split("/").reverse().join("-"));
	});