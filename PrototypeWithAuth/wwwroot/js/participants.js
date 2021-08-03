$(function () {
    $(".open-participant-entries").on("click", function (e) {
        e.preventDefault();
        var particpantId = $(this).attr("value");
        alert("participant ID: " + particpantId);
        $.ajax({
            async: true,
            url: "/Biomarkers/_Entries/?ParticipantID=" + particpantId,
            type: 'GET',
            cache: true,
            success: function (result) {
                $('.bio-form').html(result);
            }
        });
    });
});