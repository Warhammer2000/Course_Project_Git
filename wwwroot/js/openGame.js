$(function () {
    // Слушаем нажатие клавиш
    $(document).on('keydown', function (e) {
        // Проверка на F2
        if (e.key === 'F2') {
            $('#customConsole').toggle(); // Показать/скрыть консоль
            $('#consoleInput').focus(); // Фокус на поле ввода
            e.preventDefault(); // Предотвращаем стандартное поведение браузера для F2
        }
    });
});

// Функция для выполнения команд
function executeCommand() {
    var command = $('#consoleInput').val().trim();

    if (command.toLowerCase() === 'rungame') {
        // Открываем Unity игру в новом окне
        window.open("/Home/UnityGame", "_blank");
    } else {
        // Если команда не распознана
        alert("Неизвестная команда: " + command);
    }
}
