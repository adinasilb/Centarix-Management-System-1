$(function () {
    $('.removeEntry').click(function (e) {
        e.preventDefault();
        $('.entry2').addClass('d-none');
        $('.addEntryButton').removeClass('d-none');
        $('#EmployeeHour_EmployeeHoursStatusEntry2ID').destroyMaterialSelect();
        $('#EmployeeHour_EmployeeHoursStatusEntry2ID').prop("disabled", true)
        $('#EmployeeHour_EmployeeHoursStatusEntry2ID').materialSelect();
        $('#EmployeeHour_Entry2').prop("disabled", true)
        $('#EmployeeHour_Exit2').prop("disabled", true)
    });
    $('#addEntry').click(function (e) {
        e.preventDefault();
        $('.entry2').removeClass('d-none');
        $('.addEntryButton').addClass('d-none');
        $('#EmployeeHour_EmployeeHoursStatusEntry2ID').destroyMaterialSelect();
        $('#EmployeeHour_EmployeeHoursStatusEntry2ID').prop("disabled", false)
        $('#EmployeeHour_EmployeeHoursStatusEntry2ID').materialSelect();
        $('#EmployeeHour_Entry2').prop("disabled", false)
        $('#EmployeeHour_Exit2').prop("disabled", false)
    });
})