$('.documentModalForm').validate({
    rules :{

        "FilesToSave": { required: true, extension: "jpg|jpeg|png|pdf" }
    }
})