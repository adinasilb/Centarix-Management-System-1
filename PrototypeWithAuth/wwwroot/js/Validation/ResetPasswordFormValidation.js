

$('.resetPasswordForm').validate({
	rules: {
		"Input.Password": {
			required: true,
			nonAlphaNumeric: true,
			uppercase: true,
			lowercase: true,
			containsNumber: true,
			minlength: 8,
			maxlength: 20
		},
		"Input.ConfirmPassword": {
			required: true,
			equalTo: "#Input_Password"
		},
		"Input.TwoFactorAuthenticationViewModel.TwoFACode": {
			required: true,
			integer: true,
			number: true
		}
	}
});