let darkThemeSelected = getCookie('darkTheme') == 'true' ? true : false;
const darkThemeLink = document.createElement('link');
let darkTheme;

if (darkThemeSelected) {
    darkThemeLink.id = 'darkTheme'
    darkThemeLink.rel = 'stylesheet'
    darkThemeLink.href = './src/css/darkTheme.css';
    document.head.appendChild(darkThemeLink);
    darkTheme = document.querySelector('link#darkTheme');
}