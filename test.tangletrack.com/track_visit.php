<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);
require 'db_connect.php'; // Adatbáziskapcsolat

$data = json_decode(file_get_contents("php://input"), true);

if ($data) {
    $session_id = $data['session_id'];
    $page_url = $data['page_url'];
    $visit_start = date("Y-m-d H:i:s", $data['visit_start'] / 1000);
    $visit_end = date("Y-m-d H:i:s", $data['visit_end'] / 1000);
    $duration = $data['duration'];

    $ip_address = $_SERVER['REMOTE_ADDR'];
    $user_agent = $_SERVER['HTTP_USER_AGENT'];

    $sql = "INSERT INTO visitor_sessions (session_id, page_url, ip_address, user_agent, visit_start, visit_end, duration) 
            VALUES (?, ?, ?, ?, ?, ?, ?)";
    
    $stmt = $conn->prepare($sql);
    $stmt->bind_param("ssssssi", $session_id, $page_url, $ip_address, $user_agent, $visit_start, $visit_end, $duration);
    $stmt->execute();

    echo json_encode(["status" => "success"]);
}
?>
