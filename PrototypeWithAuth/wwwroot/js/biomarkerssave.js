$(function (e) {
    $(".edit-mode-input").on("click", function (e) {
        e.preventDefault();
        if ($(this).hasClass("on")) {
            $.ajax({
                async: true,
                url: "/Biomarkers/SaveTestModal/",
                type: 'GET',
                cache: false,
                success: function (data) {
                    $.fn.OpenModal('save-test-modal', 'save-bio-test-modal', data);
                }
            });
        }
        else if ($(this).hasClass("off")) {
            $(this).addClass("on");
            $(this).removeClass("off");
            $(this).attr("checked", "checked");
            $(this).attr("value", "true");
            var editLabel = $(".edit-mode-label");
            editLabel.text("Edit Mode On");
            editLabel.removeClass("off");
            editLabel.addClass("on")
            $('._testvalues input').attr("disabled", false);
            $('._testvalues input').removeClass("disabled");
        }
    });
});