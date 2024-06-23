var sessionTimeout = 300000;
var isIdleTimerOn = false;
var IsTimeout = false;
$(window).scroll(function (e) {
    startIdleTime()
});
$(document).bind('keypress.session', function (ed, e) {
    startIdleTime()
});
$(document).bind('mousedown keydown', function (ed, e) {
    startIdleTime();
});
$(document).mousemove(function (event) {
    startIdleTime();
});

function startIdleTime() {
    //  stopIdleTime();
    if (!IsTimeout) {
        localStorage.setItem("sessIdleTimeCounter", $.now());
        idleIntervalID = setInterval('checkIdleTimeout()', 1000);
        isIdleTimerOn = true;
    }
}

function checkIdleTimeout() {
    var idleTime = (parseInt(localStorage.getItem('sessIdleTimeCounter')) + (sessionTimeout));
    if ($.now() > idleTime) {
        clearInterval(idleIntervalID);
        if (isIdleTimerOn) {
            var message = "Session is expired. Please Re-Login...";
            var buttonName = "Re-Login";

            localStorage.clear();
            sessionStorage.clear();
            swal.fire({
                allowOutsideClick: false,
                title: '',
                text: message,
                showCancelButton: false,
                confirmButtonColor: '#3085d6',
                confirmButtonText: buttonName
            }).then(function (result) {
                if (result.value) {
                    var redirect = window.location.origin;
                    window.location.replace(redirect);
                }
            });
            isIdleTimerOn = false;
            IsTimeout = true;
        }

    }
}

function showLoader() {
    $(".overlay").show();
};
function hideLoader() {
    $(".overlay").hide();
};