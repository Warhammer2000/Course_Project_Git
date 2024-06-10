$(function () {
    $(document).on('keydown', function (e) {

        if (e.key === 'F2') {
            $('#customConsole').toggle(); 
            $('#consoleInput').focus(); 
            e.preventDefault(); 
        }
    });
});


function executeCommand() {
    var command = $('#consoleInput').val().trim();

    if (command.toLowerCase() === 'rungame') {
        window.open("/Home/UnityGame", "_blank");
    } else {
        alert("Неизвестная команда: " + command);
    }
}
