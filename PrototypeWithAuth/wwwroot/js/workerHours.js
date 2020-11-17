﻿$(function () {
    $.fn.reloadHoursPage = function (year, month, yearlyMonthlyEnum) {
        var yeardiff = false;
        if ($('#TotalWorkingDaysInYear').attr('year') == year) {
            var amountInYear = $('#TotalWorkingDaysInYear').val();

        }
        else {
            yeardiff = true;
        }
        
        $.ajax({
            async: true,
            url: "/ApplicationUsers/_Hours" + '?yearlyMonthlyEnum=' + yearlyMonthlyEnum + "&year=" + year + "&month=" + month + "&amountInYear=" + amountInYear,
            type: 'GET',
            cache: false,
            success: function (data) {
                $(".hours-partial").html(data);
                $('.mdb-select-workers').materialSelect();
               
                if (yeardiff) {
                    $('#TotalWorkingDaysInYear').val($('#newYearAmount').val())
                    $('#TotalWorkingDaysInYear').attr('year', $('#currentYear').val() )
                }
            }
        });
    };
    $('.yearlyMonthlySwitch').off('click').click(function (e) {
        e.preventDefault();
        var year = $(this).attr('year');
        var yearlyMonthlyEnum = $(el).attr('yearlyMonthlyEnum');
        var month = $(this).attr('month');
        $.fn.reloadHoursPage(year, month, yearlyMonthlyEnum);
    });

    $('.workersHoursMonths').off('change').change(function () {
        var year = $('.workerHoursAttr').attr('year');
        var yearlyMonthlyEnum = $('.workerHoursAttr').attr('yearlyMonthlyEnum');
        var month = $(this).val();
        $.fn.reloadHoursPage(year, month, yearlyMonthlyEnum);     
    });

    //$('body').off("change", ".workersHoursYears").on("change", ".workersHoursYears", function () {
    //    alert("workers hours years changed from body");
    //    var year = $(this).val();
    //    console.log("on change: " + $(this).val())
    //    var yearlyMonthlyEnum = $('.workerHoursAttr').attr('yearlyMonthlyEnum');
    //    var month = $('.workerHoursAttr').attr('month');

    //    $.fn.reloadHoursPage(year, month, yearlyMonthlyEnum);
    //});

    $(".workersHoursYears input").off("click").on("click", function () {
        alert("workers hours years input changed");
        var year = $(this).val();
        console.log("on change: " + $(this).val())
        var yearlyMonthlyEnum = $('.workerHoursAttr').attr('yearlyMonthlyEnum');
        var month = $('.workerHoursAttr').attr('month');

        $.fn.reloadHoursPage(year, month, yearlyMonthlyEnum);
    });

    //$('.workersHoursYears').off('change').change(function () {
    //    alert("workers hours years changed");
    //    var year = $(this).val();
    //    console.log("on change: "+$(this).val())
    //    var yearlyMonthlyEnum = $('.workerHoursAttr').attr('yearlyMonthlyEnum');
    //    var month = $('.workerHoursAttr').attr('month');
   
    //    $.fn.reloadHoursPage(year, month, yearlyMonthlyEnum);
    //});
})