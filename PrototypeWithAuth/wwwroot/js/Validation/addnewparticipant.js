$(function () {
    $(".addparticipantform").validate({
        normalizer: function (value) {
            return $.trim(value);
        },
        rules: {
            'Participant.CentarixID': {
                required: true,
                remote: {
                    url: '/Biomarkers/CheckUniqueCentarixID',
                    type: 'POST',
                    data: {
                        "CentarixID": function () { return $("#Participant_CentarixID").val() },
                    },
                },
            },
            'Participant.DOB': "required",
            'Participant.GenderID': "selectRequired",
            
        },
        messages: {
            'Participant.CentarixID': {
                remote: "This Centarix ID has already been used"
            },
        }
    });
    $(".editparticipantform").validate({
        normalizer: function (value) {
            return $.trim(value);
        },
        rules: {
            'Participant.CentarixID': {
                required: true,
                remote: {
                    url: '/Biomarkers/CheckUniqueCentarixID',
                    type: 'POST',
                    data: {
                        "CentarixID": function () { return $("#Participant_CentarixID").val() },
                    },
                },
            },
            'Participant.DOB': "required",
            'Participant.GenderID': "selectRequired",
            'Participant.ParticipantStatusID': "selectRequired",
            
        },
        messages: {
            'Participant.CentarixID': {
                remote: "This Centarix ID has already been used"
            },
        }
    });

    
})