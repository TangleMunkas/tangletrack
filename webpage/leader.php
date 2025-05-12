<?php
header("Access-Control-Allow-Origin: *");
header("Content-Type: application/json");

// PlayFab adatok
$playFabTitleId = "1fe0e"; // Cseréld ki a saját Title ID-re
$secretKey = "5FN1ZIX8ONNGC77XR7BZ3AHN34MRWC9SXTT5M1PYCS93WHMJ5S"; // Cseréld ki a saját Secret Key-re

// API URL beállítása
$url = "https://$playFabTitleId.playfabapi.com/Server/GetLeaderboard";

// Lekérdezendő adatok (statisztika neve frissítve: XP)
$data = [
    "StatisticName" => "XP", // FONTOS: Most már az XP alapján kérdezi le!
    "StartPosition" => 0,
    "MaxResultsCount" => 10
];

// cURL inicializálása
$ch = curl_init();
curl_setopt($ch, CURLOPT_URL, $url);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
curl_setopt($ch, CURLOPT_POST, 1);
curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($data));
curl_setopt($ch, CURLOPT_HTTPHEADER, [
    "Content-Type: application/json",
    "X-SecretKey: $secretKey"
]);

// API kérés végrehajtása
$result = curl_exec($ch);
$httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
if ($result === false) {
    echo json_encode(["error" => "cURL error: " . curl_error($ch)]);
    exit;
}
curl_close($ch);

// Ellenőrizzük a HTTP státuszkódot
if ($httpCode !== 200) {
    echo json_encode(["error" => "API HTTP Error: $httpCode", "response" => $result]);
    exit;
}

// Ellenőrizzük, hogy a válasz JSON-e
$json = json_decode($result, true);
if ($json === null) {
    echo json_encode(["error" => "Invalid JSON response from API", "response" => $result]);
    exit;
}

// Sikeres válasz küldése
echo json_encode(["success" => true, "data" => $json]);
?>
