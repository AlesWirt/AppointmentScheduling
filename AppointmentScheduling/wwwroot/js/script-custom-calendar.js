var routeURL = location.protocol + "//" + location.host;
$(document).ready(function () {

    $("#appointmentDate").kendoDateTimePicker({
        value: new Date(),
        dateInput: false
    });

    InitializeCalendar();
})
var calendar;
function InitializeCalendar() {
    try {
        
        var calendarEl = document.getElementById('calendar');
        if (calendarEl != null) {
            calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                headerToolbar: {
                    left: 'prev,next,today',
                    center: 'title',
                    right: 'dayGridMonth,timeGridWeek,timeGridDay'
                },
                selectable: true,
                editable: false,
                select: function (event) {
                    $("#appointmentInput").modal("show");
                    onShowModal(event, null);
                }
            });
            calendar.render();
        }
    }
    catch (e) {
        alert(e);
    }
}

function onShowModal(obj, isEventDetails) {
    $("#appointmentInput").modal("show");
}

function onCloseModal() {
    $("#appointmentInput").modal("hide");
}

function onSubmitForm() {
    /*if (checkValidation()) {*/
        
        var requestData = {
            Id: parseInt($("#id").val()),
            Title: $("#title").val(),
            Description: $("#description").val(),
            StartDate: $("#appointmentDate").val(),
            Duration: $("#duration").val(),
            DoctorId: $("#doctorId").val(),
            PatientId: $("#patientId").val()
        };
    
        $.ajax({
            url: routeURL + '/api/Appointment/SaveCalendarData',
            type: 'POST',
            data: JSON.stringify(requestData),
            contentType: 'application/json',
            success: function (response) {
                if (response.status === 1 || response.status === 2) {
                    $.notify(response.message, "success");
                    onCloseModal();
                }
                else {
                    $.notify(response.message, "error");
                }
            },
            error: function (xhr) {
                $.notify("Error", "error");
            }
        });
    /*}*/
}

function checkValidation() {
    var isValid = true;
    if ($("#title").val() === undefined || $("#title").val() === "") {
        isValid = false;
        $("#title").val().addClass('error');
    }
    else {
        $("#title").val().removeClass('error');
    }

    if ($("#appointmentDate").val() === undefined || $("#appointmentDate").val() === "") {
        isValid = false;
        $("#appointmentDate").val().addClass('error');
    }
    else {
        $("#appointmentDate").val().removeClass('error');
    }

    return isValid;
}