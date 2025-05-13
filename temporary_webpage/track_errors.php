<?php
session_start(); // Session indítása, ha még nincs

// Adatbázis kapcsolódási adatok
$servername = "localhost";
$username = "rh41261_teszt"; // Adatbázis felhasználó
$password = "!TangleTrack69!"; // Adatbázis felhasználó jelszava
$dbname = "rh41261_tangle"; // Adatbázis neve

// Kapcsolat létrehozása
$conn = new mysqli($servername, $username, $password, $dbname);

// Kapcsolat ellenőrzése
if ($conn->connect_error) {
    //die("Kapcsolódási hiba: " . $conn->connect_error);
}

// Session ID lekérése
$session_id = session_id();

// Látogató IP címének lekérése
$visitor_ip = $_SERVER['REMOTE_ADDR'];

// User Agent lekérése (látogató böngészője)
$user_agent = $_SERVER['HTTP_USER_AGENT'];

// Hivatkozó URL-cím (referer URL) lekérése
$referer_url = isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : 'N/A';

// Kért URL-cím (requested URL) lekérése
$requested_url = $_SERVER['REQUEST_URI'];

// Aktuális szerver idő lekérése
date_default_timezone_set('Europe/Budapest');
$visit_time = date('Y-m-d H:i:s', time()); // Formázott szerver idő

// Hibakód beállítása (pl. 404, 500, stb.)
$error_code = $_POST['status_code']; // Jelenlegi státuszkódot adja vissza

// Napló adat beszúrása az adatbázisba
$sql = "INSERT INTO error_logs (session_id, ip_address, user_agent, referer_url, requested_url, visit_time, error_code) 
        VALUES ('$session_id', '$visitor_ip', '$user_agent', '$referer_url', '$requested_url', '$visit_time', '$error_code')";

if ($conn->query($sql) === TRUE) {
    // Sikeres beszúrás
    // echo "Hiba adat sikeresen eltárolva!";
} else {
    // Ha hiba történik, kiíratjuk a hibát
    //echo "Hiba történt: " . $conn->error;
}

// Kapcsolat lezárása
$conn->close();
?>
