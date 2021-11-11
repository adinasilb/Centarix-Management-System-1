$(function (e) {
    $(".edit-mode-input").on("click", function (e) {
        console.log($(this))
        if ($(this).hasClass("on")) {
            var valid = $(".bio-form").valid()
            if (valid) {
                var guid = $(".hidden-guid").val();
                $.ajax({
                    async: true,
                    url: "/Biomarkers/SaveTestModal?Guid=" + guid,
                    type: 'GET',
                    cache: false,
                    success: function (data) {
                        $.fn.OpenModal('save-test-modal', 'save-bio-test-modal', data);
                    }
                });
            }
            else{
                $(".edit-mode-input").prop('checked', true);
            }
        }
        else if ($(this).hasClass("off")) {
            console.log($(this))
            $(this).addClass("on");
            $(this).removeClass("off");
            $(this).attr("value", "true");
            $(".edit-mode-input").prop('checked', true);
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