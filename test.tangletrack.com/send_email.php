<?php
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $name = htmlspecialchars($_POST["name"]);
    $email = filter_var($_POST["email"], FILTER_SANITIZE_EMAIL);
    $message = htmlspecialchars($_POST["message"]);

    // Validáció (üres mezők ellenőrzése)
    if (empty($name) || empty($email) || empty($message)) {
        echo "error";
        exit;
    }

    // Email beállítások
    $to = "tanglemunkas@gmail.com"; // A te e-mail címed
    $subject = "Új üzenet érkezett a kapcsolatfelvételi űrlapon keresztül";
    $headers = "From: " . $email . "\r\n";
    $headers .= "Reply-To: " . $email . "\r\n";
    $headers .= "Content-Type: text/plain; charset=UTF-8\r\n";

    $email_body = "Név: $name\n";
    $email_body .= "E-mail: $email\n";
    $email_body .= "Üzenet:\n$message\n";

    // Email küldése
    if (mail($to, $subject, $email_body, $headers)) {
        echo "success";
    } else {
        echo "error";
    }
} else {
    echo "error";
}
?>