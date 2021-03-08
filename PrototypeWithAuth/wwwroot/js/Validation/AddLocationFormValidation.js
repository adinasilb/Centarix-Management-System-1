
$('.addLocationForm').validate({
	rules: {
		"LocationInstance.LocationInstanceName": "required",
		"LocationInstance.LocationTypeID": {
			required: true,
			min: 1,
		},
		"LocationInstances[0].Height": {
			required: true,
			number: true,
			min: 1,
			integer: true
		},
		"LocationInstances[0].Width": {
			required: true,
			number: true,
			min: 1,
			integer: true
		},
		"LocationInstances[1].Height": {
			required: true,
			number: true,
			min: 1,
			integer: true
		},
		"LocationInstances[2].Height": {
			required: true,
			number: true,
			min: 1,
			integer: true
		},
			"LocationInstances[3].Height": {
			required: true,
			number: true,
			min: 1,
			integer: true
		},
		"LocationInstances[4].Height": {
			selectRequired : true
		},
		"LocationInstances[1].LabPartID" :{
		    selectRequired : true
		},
		"LocationInstances[0].LocationRoomInstanceID" :{
			   selectRequired : true
		},
		"LocationInstances[2].LocationTypeID": "selectRequired"
	}
});