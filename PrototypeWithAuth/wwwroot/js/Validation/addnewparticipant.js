$(function () {
    $(".addparticipantform").validate({
        normalizer: function (value) {
            return $.trim(value);
        },
        rules: {
            'Participant.CentarixID': "required",
            'Participant.DOB': "required",
            'Participant.GenderID': "selectRequired"
        }
    });
    $(".editparticipantform").validate({
        normalizer: function (value) {
            return $.trim(value);
        },
        rules: {
            'Participant.CentarixID': "required",
            'Participant.DOB': "required",
            'Participant.GenderID': "selectRequired",
            'Participant.ParticipantStatusID': "selectRequired"
        }
    });
})