$(".addparticipantform").validate({
    normalizer: function (value) {
        return $.trim(value);
    },
    rules: {
        'Participant.CentarixID': "required",
        'Participant.DOB': "required",
        'Participant.Gender': "required"
    }
});