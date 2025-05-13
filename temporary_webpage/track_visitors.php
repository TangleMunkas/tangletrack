<?php
session_start(); // Session indítása az oldal tetején

// Ellenőrzés, hogy a látogató adata már el lett-e tárolva
if (!isset($_SESSION['data_stored'])) {

    // Adatbázis kapcsolódási adatok
    $servername = "localhost";
    $username = "rh41261_teszt"; // Adatbázis felhasználó
    $password = "!TangleTrack69!"; // Adatbázis felhasználó jelszava
    $dbname = "rh41261_tangle"; // Adatbázis neve

    // Kapcsolat létrehozása
    $conn = new mysqli($servername, $username, $password, $dbname);

    // Kapcsolat ellenőrzése
    if ($conn->connect_error) {
        die("Kapcsolódási hiba: " . $conn->connect_error);
    }

    // Látogató IP címének lekérése
    $visitor_ip = $_SERVER['REMOTE_ADDR'];

    // User Agent (Látogató böngészője) lekérése
    $user_agent = $_SERVER['HTTP_USER_AGENT'];

    // Session ID lekérése
    $session_id = session_id();
    
    // IP-cím hostnevének lekérése
    $server_name = gethostbyaddr($visitor_ip);

    // Ha a gethostbyaddr() nem talál semmit, adj meg egy alapértelmezett értéket
    if (!$server_name) {
        $server_name = 'Unknown'; // Alapértelmezett érték, ha nem található a hostnév
    }

    // Aktuális szerver idő lekérése
    date_default_timezone_set('Europe/Budapest');
    $current_time = date('Y-m-d H:i:s', time()); // Formázott szerver idő: ÉÉÉÉ-HH-NN ÓÓ:PP:SS

    // Hivatkozó URL-cím (referer URL) lekérése
    $referer_url = isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : 'N/A';

    // Kért URL-cím (requested URL) lekérése
    $requested_url = $_SERVER['REQUEST_URI'];

    // Robot szűrési kulcsszavak
    $robot_keywords = ["google", "msnbot", "ahrefs.com", "yandex", "amazon", "proxy", "microsoft"];

    // Ellenőrzés, hogy az IP-cím vagy Session ID alapján már létezik-e rekord a "visitors_robots" vagy "visitors_users" táblában
    $check_session_sql = "SELECT * FROM visitors_robots WHERE session_id = '$session_id' 
                          UNION 
                          SELECT * FROM visitors_users WHERE session_id = '$session_id'";
    echo "Lekérdezés a session ID alapján: " . $check_session_sql . "<br>";
    $result = $conn->query($check_session_sql);

    if ($result->num_rows > 0) {
        // Ha már létezik ilyen Session ID-val rekord, nem csinálunk semmit
        $_SESSION['data_stored'] = true;
        echo "Az adat már el lett tárolva ebben a sessionben!";
    } else {
        // IP ellenőrzése a "visitors_robots" táblában
        $check_ip_sql = "SELECT * FROM visitors_robots WHERE ip_address = '$visitor_ip'";
        echo "Lekérdezés az IP alapján: " . $check_ip_sql . "<br>";
        $ip_result = $conn->query($check_ip_sql);

        // Ha IP cím nincs a "visitors_robots" táblában, és a szervernév tartalmazza valamely robot kulcsszót
        if ($ip_result->num_rows === 0 && preg_match('/' . implode('|', $robot_keywords) . '/i', $server_name)) {
            // Robot adat hozzáadása a "visitors_robots" táblához
            $sql_insert_robot = "INSERT INTO visitors_robots (ip_address, server_name, visit_time, user_agent, session_id, referer_url, requested_url) 
                                VALUES ('$visitor_ip', '$server_name', '$current_time', '$user_agent', '$session_id', '$referer_url', '$requested_url')";

            if ($conn->query($sql_insert_robot) === TRUE) {
                echo "Sikeres beszúrás a visitors_robots táblába!";
            } else {
                echo "Hiba történt a visitors_robots táblához való beszúráskor: " . $conn->error;
            }

            $_SESSION['data_stored'] = true;
        } else {
            // Nem robot, adjuk hozzá a "visitors_users" táblához, ha még nem szerepel benne
            $sql_insert_user = "INSERT INTO visitors_users (ip_address, visit_time, user_agent, session_id, referer_url, requested_url, server_name) 
                               VALUES ('$visitor_ip', '$current_time', '$user_agent', '$session_id', '$referer_url', '$requested_url', '$server_name')";

            echo "Felhasználó beszúrás SQL: " . $sql_insert_user . "<br>";

            if ($conn->query($sql_insert_user) === TRUE) {
                echo "Sikeres beszúrás a visitors_users táblába!";
            } else {
                echo "Hiba történt a visitors_users táblához való beszúráskor: " . $conn->error;
            }

            $_SESSION['data_stored'] = true;
        }
    }

    // Kapcsolat lezárása
    $conn->close();

} else {
    echo "Az adat már el lett tárolva ebben a sessionben!";
}
?>
