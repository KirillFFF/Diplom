const sideMenu = document.querySelector("aside");
const themeToggler = document.querySelector(".theme-toggler");

function changeTheme() {
    if (darkTheme != null) {
        darkTheme.remove();
        darkTheme = null;
    }
    else {
        document.body.classList.toggle('dark-theme-variables');
        themeToggler.querySelector('span:nth-child(1)').classList.toggle('active');
        themeToggler.querySelector('span:nth-child(2)').classList.toggle('active');
    }

    setCookie('darkTheme', `${(darkThemeSelected = !darkThemeSelected)}`, { secure: true });
}

function menuOpen() {
    sideMenu.style.display = 'block';
    document.body.style.overflowY = 'hidden';
    document.body.children[0].addEventListener("mouseup", menuListener);
}

function menuClose() {
    sideMenu.removeAttribute("style");
    document.body.style.overflowY = 'auto';
    document.body.children[0].removeEventListener("mouseup", menuListener);
}

function menuListener(event) {
    if (sideMenu.style.display == 'block' && (!sideMenu.contains(event.target) || document.getElementById('shadow').contains(event.target))) {
        menuClose();
    }
}

document.querySelectorAll('.sidebar a').forEach(x => {
    x.onclick = () => {
        document.querySelector('.sidebar a.active').classList.toggle('active');
        x.classList.toggle('active');
    }
})