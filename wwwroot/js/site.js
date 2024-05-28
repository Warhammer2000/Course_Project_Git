// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function toggleTheme() {
    var themeStylesheet = document.getElementById('themeStylesheet');
    var currentTheme = themeStylesheet.getAttribute('href');
    var newTheme = currentTheme === '/css/light-theme.css' ? '/css/dark-theme.css' : '/css/light-theme.css';
    themeStylesheet.setAttribute('href', newTheme);

    // Сохранение темы в localStorage
    localStorage.setItem('theme', newTheme);
}

// Установка темы при загрузке страницы
window.onload = function () {
    var savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
        var themeStylesheet = document.getElementById('themeStylesheet');
        if (themeStylesheet) {
            themeStylesheet.setAttribute('href', savedTheme);
        }
    }
}
