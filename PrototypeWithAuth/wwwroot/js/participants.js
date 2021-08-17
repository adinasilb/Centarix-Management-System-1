$(function () {
    $(".open-participant-entries").on("click", function (e) {
        e.preventDefault();
        var particpantId = $(this).attr("value");
        window.location.href = "/Biomarkers/Entries/?ParticipantID=" + particpantId;
        //$.ajax({
        //    async: true,
        //    url: "/Biomarkers/Entries/?ParticipantID=" + particpantId,
        //    type: 'GET',
        //    cache: true,
        //    success: function (result) {
        //        window.location.href = 
        //    }
        //});
    });
});