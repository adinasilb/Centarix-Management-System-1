$(function () {
	$(".sort-fb").on('click', function () {
		$.fn.CallRequestsIndexWithSort("fb");
	});
	$(".sort-hp").on('click', function () {
		$.fn.CallRequestsIndexWithSort("hp");
	});
	$.fn.CallRequestsIndexWithSort = function(filter){
		switch (filter) {
			case "fb":
				alert("sort by frequently bought");
				break;
			case "hp":
				alert("sorty by highest price");
				break;
		}

		var url = "/Requests/_IndexTable";

		$.ajax({
			async: true,
			url: url,
			type: 'GET',
			traditional: true,
			cache: false,
			data: { ExpensesFilter: filter },
			success: function (data) {
				$(".index-table").empty();
				$(".index-table").html(data);
			}
		});
	};
});