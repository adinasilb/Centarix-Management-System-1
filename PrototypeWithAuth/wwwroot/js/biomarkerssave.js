$(function (e) {
    $(".edit-mode-input").on("click", function (e) {
        e.preventDefault();
        $.ajax({
            async: true,
            url: "/Biomarkers/SaveTestModal/",
            type: 'GET',
            cache: false,
            success: function (data) {
                $.fn.OpenModal('save-test-modal', 'save-bio-test-modal', data);
            }
        });
    });
});