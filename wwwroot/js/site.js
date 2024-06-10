function toggleTheme() {
    var themeStylesheet = document.getElementById('themeStylesheet');
    var currentTheme = themeStylesheet.getAttribute('href');
    var newTheme = currentTheme === '/css/light-theme.css' ? '/css/dark-theme.css' : '/css/light-theme.css';
    themeStylesheet.setAttribute('href', newTheme);

    localStorage.setItem('theme', newTheme);
}


window.onload = function () {
    var savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
        var themeStylesheet = document.getElementById('themeStylesheet');
        if (themeStylesheet) {
            themeStylesheet.setAttribute('href', savedTheme);
        }
    }
}
