$(function (e) {
    $(".edit-mode-input").on("click", function (e) {
        //e.preventDefault();
        if ($(this).hasClass("on")) {
            console.log($(this))
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
            console.log($(this))
            $(this).addClass("on");
            $(this).removeClass("off");
            $(this).attr("value", "true");
            $(this).attr("checked", "checked");
            $('.open-document-modal').attr("data-val", true);
            var editLabel = $(".edit-mode-label");
            editLabel.text("Edit Mode On");
            editLabel.removeClass("off");
            editLabel.addClass("on")
            $('._testvalues input').attr("disabled", false);
            $('._testvalues input').removeClass("disabled");
        }
    });
});