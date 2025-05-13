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

// Az AJAX POST lekérdezésből kapjuk a platform nevét
if (isset($_POST['platform'])) {
    $platform = $_POST['platform'];
    //echo "Platform: " . $platform; // Debug célra


    // Ellenőrizzük, hogy a platform már létezik-e
    $sql = "SELECT clicks FROM social_clicks WHERE platform = '$platform'";
    $result = $conn->query($sql);

    if ($result->num_rows > 0) {
        // Ha már létezik, növeljük a kattintások számát
        $sql = "UPDATE social_clicks SET clicks = clicks + 1 WHERE platform = '$platform'";
    } else {
        // Ha nem létezik, létrehozzuk a rekordot
        $sql = "INSERT INTO social_clicks (platform, clicks) VALUES ('$platform', 1)";
    }

    // SQL lekérdezés végrehajtása
    if ($conn->query($sql) === TRUE) {
        //echo "Sikeres frissítés!";
    } else {
        //echo "Hiba: " . $conn->error;
    }

    // Kapcsolat lezárása
    $conn->close();
} else {
    //echo "Nincs platform adat megadva!";
}
?>
