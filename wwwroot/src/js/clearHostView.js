window.onload = () => {
    if (document.readyState == "complete") {
        let center = document.querySelector('center');
        if (center != null) {
            while (center.nextElementSibling) {
                center.nextElementSibling.remove();
            }
            center.remove();
        }
    }

    window.onload = null;
}