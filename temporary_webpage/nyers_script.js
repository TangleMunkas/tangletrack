function betoltve() {
    trackVisitors();
    idozito();
}

function idozito() {
    setTimeout(gratulacio, 120000);
}

function gratulacio() {
    alert("Are you still here? You are amazing! ❤");
}

function loadIframe() {
    document.getElementById("try-game-button").style.display = "none";
    var iframeContainer = document.getElementById('iframe-container');
    iframeContainer.innerHTML = '<iframe class="demo_keret" src="demo1.html" title="Tangle Track demo game" alt="Tangle Track demo game" width="700" height="525"></iframe>';
}

function trackVisitors() {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "track_visitors.php", true); // Hívja a PHP szkriptet
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            //console.log(xhr.responseText); // Válasz a PHP szkriptből
        }
    };
    xhr.send(); // Nincs szükség paraméterekre, mivel a PHP szerveroldali adatokat használ
}

function trackClick(platform) {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "track_social_click.php", true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            // console.log(xhr.responseText); // Eredmény konzolba
        }
    };
    xhr.send("platform=" + platform);
}

function trackDemoGame() {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "track_game_click.php", true); // Hívja a PHP szkriptet
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            //console.log(xhr.responseText); // Válasz a PHP szkriptből
        }
    };
    xhr.send(); //
}

function trackErrors(status_code) {
    //console.log("trackErrors called with status code: " + status_code);
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "track_errors.php", true); // Hívja a PHP szkriptet
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            //console.log(xhr.responseText); // Válasz a PHP szkriptből
        }
    };
    xhr.send("status_code=" + status_code); //
}