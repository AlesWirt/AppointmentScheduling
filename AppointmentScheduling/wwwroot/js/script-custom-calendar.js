var routeURL = location.protocol + "//" + location.host;
$(document).ready(function () {
    $("#appointmentDate").kendoDateTimePicker({
        value: new Date(),
        dateInput: false,
        format: 'dd/MM/yyyy HH:mm:ss'
    });
    InitializeCalendar();
});

function InitializeCalendar() {
    try {


        var calendarEl = document.getElementById('calendar');
        if (calendarEl != null) {
            var calendar = new FullCalendar.Calendar(calendarEl,
                {
                    
                    initialView: 'dayGridMonth',
                    headerToolbar: {
                        left: 'prev,next,today',
                        center: 'title',
                        right: 'dayGridMonth,timeGridWeek,timeGridDay'
                    },
                    
                    //timeZone
                    selectable: true,
                    editable: false,
                    select: function (event) {
                        onShowModal(event, null);
                    },
                    eventDisplay: 'block',
                    events: function (fetchInfo, successCallback, failureCallback) {
                        $.ajax({
                            url: routeURL + '/api/Appointment/GetCalendarData?doctorId=' + $("#doctorId").val(),
                            type: 'GET',
                            dataType: 'JSON',
                            success: function (response) {
                                console.log(response);
                                var events = [];
                                if (response.status === 1) {
                                    $.each(response.dataEnum, function (i, data) {
                                        

                                        events.push({
                                            
                                            title: data.title,
                                            description: data.description,
                                            start: data.startDate,
                                            end: data.endDate,
                                            backgroundColor: data.isDoctorApprove ? "#28a745" : "#dc3545",
                                            borderColor: "#162466",
                                            textColor: "white",
                                            id: data.id
                                        });
                                    })
                                }
                                //вот для этого
                                console.log(events);
                                successCallback(events);
                                //successCallback([
                                //    {
                                //        title: 'event1',
                                //        start: '2021-09-01'
                                //    },
                                //    {
                                //        title: 'event2',
                                //        start: '2021-10-02',
                                //        end: '2010-01-07'
                                //    },
                                //    {
                                //        title: 'event3',
                                //        start: '2021-09-11T12:30:00',
                                //        allDay: false // will make the time show
                                //    }
                                //]);
                            },
                            error: function (xhr) {
                                $.notify("Error", "error");
                            }
                        });
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
    if (checkValidation()) {

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
    }
}

function checkValidation() {
    var isValid = true;
    if ($("#title").val() === undefined || $("#title").val() === "") {
        isValid = false;
        $("#title").addClass('error');
    }
    else {
        $("#title").removeClass('error');
    }

    if ($("#appointmentDate").val() === undefined || $("#appointmentDate").val() === "") {
        isValid = false;
        $("#appointmentDate").addClass('error');
    }
    else {
        $("#appointmentDate").removeClass('error');
    }

    return isValid;
}