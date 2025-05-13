<?php
// Adatbázis kapcsolódási adatok
$servername = "localhost";
$username = "rh41261_teszt";
$password = "!TangleTrack69!";
$dbname = "rh41261_tangle";

// Kapcsolat létrehozása
$conn = new mysqli($servername, $username, $password, $dbname);

// Kapcsolat ellenőrzése
if ($conn->connect_error) {
    //die("Kapcsolódási hiba: " . $conn->connect_error);
}

// IP-cím lekérése
$visitor_ip = $_SERVER['REMOTE_ADDR'];

// Aktuális látogatás idejének beállítása (opcionális, ha nem használod a CURRENT_TIMESTAMP-et az adatbázisban)
$current_time = date('Y-m-d H:i:s');

// Teszt adat beszúrása
$sql = "INSERT INTO demo_games (ip_address, visit_time) VALUES ('$visitor_ip', '$current_time')";

if ($conn->query($sql) === TRUE) {
    //echo "Sikeres rögzítés";
} else {
    //echo "Hiba történt: " . $conn->error;
}

// Kapcsolat lezárása
$conn->close();
?>
