$(".bio-form").validate({
    normalizer: function (value) {
        return $.trim(value);
    },
    rules: {
        'Participant.CentarixID': "required",
        'Participant.DOB': "required",
        'Participant.Gender': "required",
        'Participant.ParticipantStatus': "required"
    }
});
$(".test-field.double").rules("add", {
    double: true
});