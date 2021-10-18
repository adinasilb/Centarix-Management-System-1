$(function () {
	$(".change-month").on("change", function () {
		$.fn.$.fn.ajaxPartialIndexTable();
	});
	$(".change-year").on("change", function () {
		var years = $("#select-years").val();
		var currency = $("#select-months").val();
		console.log("year: " + year + " , currency: " + currency);
		var url = "/Protocols/_ReportsIndexTable?months=" + months + "&years=" + years;
		$.fn.$.fn.ajaxPartialIndexTable(url, "._IndexTable", "GET" );
	});

	$(".create-report").click(function (e) {
		var sidebarType = $('#masterSidebarType').val()
		var reportCategory = $(".reportCategoryID").val()
		$.ajax({
			url: "/Protocols/NewReportModal?reportCategoryId=" + reportCategory + "&sidebarType=" + sidebarType,
			type: 'GET',
			cache: false,
			success: function (data) {
				$.fn.OpenModal('new-report-modal', "new-report", data)
			}
		});
	})

	$(".edit-report").click(function (e) {
		var reportID = $(this).attr("value")
		$.ajax({
			url: "/Protocols/CreateReport?reportID=" + reportID,
			type: 'GET',
			cache: false,
			success: function (data) {
				$(".render-body").html(data)
				console.log("focus")
				$(".start-div").trigger("focus")
			}
		});
    })
	var reportFavoritesHasRun = false; //This is preventing the double click
	$(".report-favorite").off("click").on("click", function (e) {
	//$(this).off("click");
	//alert("in click fr");
	if (!reportFavoritesHasRun) {
		reportFavoritesHasRun = true;
		$("#loading").show();
		var reportFavorite = $(this);
		//alert(" in favorite request fx");
		var emptyHeartClass = "icon-favorite_border-24px";
		var fullHeartClass = "icon-favorite-24px";
		var unfav = "report-unlike";
		var title = "Favorite";
		var FavType = "favorite";
		var sidebarType = $('#masterSidebarType').val();
		if (reportFavorite.hasClass("report-unlike")) {
			FavType = "unlike";
			$.ajax({
				async: true,
				url: "/Protocols/FavoriteReport?reportID=" + reportFavorite.attr("value") + "&Favtype=" + FavType + '&sidebarType=' + sidebarType,
				traditional: true,
				type: "GET",
				cache: false,
				success: function (data) {
					reportFavoritesHasRun = false;
					reportFavorite.children("i").addClass(emptyHeartClass);
					reportFavorite.children("i").removeClass(fullHeartClass);
					reportFavorite.attr("data-original-title", title);
					reportFavorite.removeClass(unfav);
					$("#loading").hide();
					if (sidebarType == 'Favorites') {
						$('[data-toggle="tooltip"]').tooltip('dispose'); //is this the right syntax?
						$('._IndexTable').html(data);
					}
				}
			})
		}
		else {
			title = "Unfavorite";
			$.ajax({
				async: true,
				url: "/Protocols/FavoriteReport?reportID=" + reportFavorite.attr("value") + "&Favtype=" + FavType + '&sidebarType=' + sidebarType,
				traditional: true,
				type: "GET",
				cache: false,
				success: function (data) {
					reportFavoritesHasRun = false;
					reportFavorite.children("i").removeClass(emptyHeartClass);
					reportFavorite.children("i").addClass(fullHeartClass);
					reportFavorite.attr("data-original-title", title);
					reportFavorite.addClass(unfav);
					$("#loading").hide();

				}
			})
		}		
	}
});
});