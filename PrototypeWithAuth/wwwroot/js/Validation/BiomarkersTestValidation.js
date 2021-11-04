$(function () {
    $.validator.addClassRules("double", {
        number: true /*,
        other rules */
    });
    $(".bio-form").validate({
        normalizer: function (value) {
            return $.trim(value);
        },
        rules: {
            'Participant.CentarixID': "required",
            'Participant.DOB': "required",
            'Participant.Gender': "selectRequired",
            'Participant.ParticipantStatus': "selectRequired"
        }
    });
})